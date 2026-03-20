using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Components;
using MTM101BaldAPI.ObjectCreation;
using MTM101BaldAPI.PlusExtensions;
using MTM101BaldAPI.Registers;
using System.Collections;
using System.Reflection;
using TheHardestMod.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheHardestMod.ObjectExtensions
{
    public static class CustomLevelCreator
    {
        private static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("THM_ObjectExtensions");
        private static readonly Dictionary<object, object> _objectMap = new Dictionary<object, object>();
        private static AssetManager assetMan;

        public static LevelObject CloneLevelObjectDeepReflection(LevelObject originalLevelObject)
        {
            if (originalLevelObject == null)
            {
                return null;
            }

            _objectMap.Clear();
            return (LevelObject)InternalDeepCopy(originalLevelObject);
        }

        private static object InternalDeepCopy(object originalObject)
        {
            if (originalObject == null)
            {
                return null;
            }

            Type type = originalObject.GetType();
            if (type.IsValueType || type == typeof(string))
            {
                return originalObject;
            }

            if (_objectMap.ContainsKey(originalObject))
            {
                return _objectMap[originalObject];
            }

            if (originalObject is Object unityObject)
            {
                return Object.Instantiate(unityObject);
            }

            object newObject;
            try
            {
                newObject = Activator.CreateInstance(type);
            }
            catch (MissingMethodException)
            {
                Logger.LogError($"[ModName] Type '{type.FullName}' does not have a public parameterless constructor. " +
                                "Cannot create a new instance for deep copy. Returning original object (shallow copy).");
                return originalObject;
            }

            _objectMap.Add(originalObject, newObject);

            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    object originalValue = field.GetValue(originalObject);

                    if (originalValue == null)
                    {
                        field.SetValue(newObject, null);
                        continue;
                    }

                    Type fieldType = field.FieldType;

                    if (typeof(IList).IsAssignableFrom(fieldType) && fieldType.IsGenericType)
                    {
                        IList originalList = (IList)originalValue;
                        IList newList = (IList)Activator.CreateInstance(fieldType);

                        foreach (object item in originalList)
                        {
                            newList.Add(InternalDeepCopy(item));
                        }
                        field.SetValue(newObject, newList);
                    }
                    else if (fieldType.IsArray)
                    {
                        Array originalArray = (Array)originalValue;
                        Array newArray = (Array)Activator.CreateInstance(fieldType, originalArray.Length);

                        for (int i = 0; i < originalArray.Length; i++)
                        {
                            newArray.SetValue(InternalDeepCopy(originalArray.GetValue(i)), i);
                        }
                        field.SetValue(newObject, newArray);
                    }
                    else if (!fieldType.IsValueType && fieldType != typeof(string) && !(originalValue is Object))
                    {
                        field.SetValue(newObject, InternalDeepCopy(originalValue));
                    }
                    else
                    {
                        field.SetValue(newObject, originalValue);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError($"[ModName] Error copying field '{field.Name}' of type '{type.Name}': {ex.Message}");
                }
            }

            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    try
                    {
                        object originalValue = prop.GetValue(originalObject, null);

                        if (originalValue == null)
                        {
                            prop.SetValue(newObject, null, null);
                            continue;
                        }

                        Type propType = prop.PropertyType;

                        if (typeof(IList).IsAssignableFrom(propType) && propType.IsGenericType)
                        {
                            IList originalList = (IList)originalValue;
                            IList newList = (IList)Activator.CreateInstance(propType);

                            foreach (object item in originalList)
                            {
                                newList.Add(InternalDeepCopy(item));
                            }
                            prop.SetValue(newObject, newList, null);
                        }
                        else if (propType.IsArray)
                        {
                            Array originalArray = (Array)originalValue;
                            Array newArray = (Array)Activator.CreateInstance(propType, originalArray.Length);

                            for (int i = 0; i < originalArray.Length; i++)
                            {
                                newArray.SetValue(InternalDeepCopy(originalArray.GetValue(i)), i);
                            }
                            prop.SetValue(newObject, newArray, null);
                        }
                        else if (!propType.IsValueType && propType != typeof(string) && !(originalValue is Object))
                        {
                            prop.SetValue(newObject, InternalDeepCopy(originalValue), null);
                        }
                        else
                        {
                            prop.SetValue(newObject, originalValue, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"[ModName] Error copying property '{prop.Name}' of type '{type.Name}': {ex.Message}");
                    }
                }
            }

            return newObject;
        }

        public static SceneObject DuplicateAndNewLevel(
            this SceneObject SO,
            string NewLvName,
            int LevelNo,
            bool SetPrevLevelToNewLevel,
            bool Isfinal)
        {
            SceneObject sceneObj = SO;

            if (sceneObj != null)
            {
                SceneObject sceneObject = Object.Instantiate<SceneObject>(sceneObj);

                WeightedLevelObject[] weightedLevelObjectArray = new WeightedLevelObject[0];

                foreach (WeightedLevelObject originalWeightedLevelObject in sceneObj.randomizedLevelObject)
                {
                    WeightedLevelObject copyOfWeightedLevelObject = new WeightedLevelObject();

                    LevelObject copyOfLevelObject = null;
                    try
                    {
                        copyOfLevelObject = CloneLevelObjectDeepReflection(originalWeightedLevelObject.selection);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"[ModName] Critical error during LevelObject deep reflection clone! " +
                                       $"Falling back to shallow copy of LevelObject. Error: {ex.Message}");
                        copyOfLevelObject = originalWeightedLevelObject.selection;
                    }

                    copyOfWeightedLevelObject.selection = copyOfLevelObject;
                    copyOfWeightedLevelObject.weight = originalWeightedLevelObject.weight;

                    weightedLevelObjectArray = HarmonyLib.CollectionExtensions.AddToArray<WeightedLevelObject>(weightedLevelObjectArray, copyOfWeightedLevelObject);
                }

                sceneObject.randomizedLevelObject = weightedLevelObjectArray;

                sceneObject.levelTitle = NewLvName;
                sceneObject.levelNo = LevelNo;
                sceneObject.nameKey = "MainLevel_" + LevelNo;
                sceneObject.name = "MainLevel_" + LevelNo;

                if (SetPrevLevelToNewLevel)
                {
                    sceneObj.nextLevel = sceneObject;
                }

                foreach (CustomLevelObject customLevelObject in sceneObject.GetCustomLevelObjects())
                {
                    LevelObject levelobj = Object.Instantiate<LevelObject>(customLevelObject);

                    if (Isfinal)
                    {
                        levelobj.finalLevel = true;
                        customLevelObject.finalLevel = true;
                    }
                    else
                    {
                        levelobj.finalLevel = false;
                        customLevelObject.finalLevel = false;
                    }

                    BaseGameManager gameman = sceneObject.manager;
                    LevelGenerationParameters genparam = gameman.levelObject;

                    genparam.name = "MainLevel_" + LevelNo;

                    gameman.name = "Lvl" + LevelNo + "_MainGameManager";
                    gameman.levelObject = genparam;
                    sceneObject.manager = gameman;

                    levelobj.name = levelobj.type + "_Lvl" + LevelNo;

                    sceneObject.levelObject = levelobj;
                }

                Logger.LogInfo($"<color=green>{NewLvName}</color> has been created with its previous level being {(sceneObj.levelTitle)}");
                return sceneObject;
            }

            Logger.LogError($"Level {NewLvName} Could not have been created because the Previous Level Has not been assigned");
            return null;
        }

    }
    public static class ObjectCreationExtensions
    {
	   public static AudioManager CreateAudioManager(this GameObject target, float minDistance = 25f, float maxDistance = 50f)
        {
            var audio = target.AddComponent<AudioManager>();
            audio.audioDevice = target.CreateAudioSource(minDistance, maxDistance);

            return audio;
        }
        public static AudioSource CreateAudioSource(this GameObject target, float minDistance = 25f, float maxDistance = 50f)
        {
            var audio = target.AddComponent<AudioSource>();
            audio.minDistance = minDistance;
            audio.maxDistance = maxDistance;
            return audio;
        }


        public static ItemObject DuplicateItem(this ItemObject target, string newName, bool Enabled) {
            var target2IO = UnityEngine.GameObject.Instantiate(target);
            var target2 = UnityEngine.GameObject.Instantiate(target.item);
            target2IO.AddMeta(target.GetMeta());
            target2.gameObject.ConvertToPrefab(Enabled);
            target2IO.item = target2;
            target2IO.name = newName;
            target2IO.nameKey = newName;
            return target2IO;
        }
        [Obsolete("just use Mathf.lerp()")]
        public static float lerp(this float a,float b,float t) {
            return Mathf.Lerp(a,b,t);
        }
        public static T FindResourceObjectByName<T>(string name) where T : UnityEngine.Object
        {
            return FindResourceObjects<T>().First((T x) => x.name == name);
        }
        public static T[] FindResourceObjects<T>() where T : UnityEngine.Object
        {
            List<T> list = new List<T>();
            list.AddRange(from x in Resources.FindObjectsOfTypeAll<T>()
                          where x.GetInstanceID() > 0
                          select x);
            return list.ToArray();
        }



        
    }
}