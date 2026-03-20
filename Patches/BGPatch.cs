using HarmonyLib;
using UnityEngine;
using MTM101BaldAPI.Reflection;
using TheHardestMod.Npcs;
namespace TheHardestMod
{
    [HarmonyPatch]
    public class BGPatch
    {


        [HarmonyPrefix]
        [HarmonyPatch(typeof(MainGameManager), "AllNotebooks")]
        static public bool AllNBPatch(MainGameManager __instance)
        {
           
            

            if (TheHardestMod.MainClass.Instance.laps == 0 && Singleton<CoreGameManager>.Instance.sceneObject.levelTitle == "F6")
            {

                TheHardestMod.MainClass.Instance.laps += 1;
                if (Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>() == null) Singleton<BaseGameManager>.Instance.gameObject.AddComponent<PizzatowerAhhTimerManager>();
                else Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>().AddTime(10f);
                foreach (Notebook nb in Singleton<BaseGameManager>.Instance.Ec.notebooks)
                {
                    if (nb != MainClass.Instance.lastNotebookCollected)
                    {
                        nb.activity.ReInit();
                        Singleton<BaseGameManager>.Instance.AddNotebookTotal(1);
                    }

                }
                if (!Singleton<ModifiersCategorySettings>.Instance.a) Singleton<BaseGameManager>.Instance.Ec.GetBaldi().SetAnger(-16f);
                else Singleton<BaseGameManager>.Instance.Ec.GetBaldi().GetAngry(-45);

                Singleton<CoreGameManager>.Instance.AddPoints(200, 0, false);
                Singleton<BaseGameManager>.Instance.Ec.AddTimeScale(new TimeScaleModifier(1.025f, 1.25f, 1.3f));
                Singleton<CoreGameManager>.Instance.audMan.FlushQueue(true);
                Singleton<CoreGameManager>.Instance.audMan.QueueAudio(TheHardestMod.MainClass.Instance.Snd_FINALE);
                Singleton<CoreGameManager>.Instance.audMan.SetLoop(true);
                Singleton<CoreGameManager>.Instance.GetHud(0).BaldiTv.Speak(MainClass.Instance.Snd_Bal_Lap1);
                Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>().AddScore(120f, false, true, "Finale");
                return false;

            }
            else if (TheHardestMod.MainClass.Instance.laps == 1 && Singleton<CoreGameManager>.Instance.sceneObject.levelTitle == "F6")
            {

                TheHardestMod.MainClass.Instance.laps += 1;
                if (Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>() == null) Singleton<BaseGameManager>.Instance.gameObject.AddComponent<PizzatowerAhhTimerManager>();
                else Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>().SetTime(25);
                foreach (Notebook nb in Singleton<BaseGameManager>.Instance.Ec.notebooks)
                {
                    if (nb != MainClass.Instance.lastNotebookCollected)
                    {
                        nb.activity.ReInit();
                        Singleton<BaseGameManager>.Instance.AddNotebookTotal(1);
                    }

                }
                if (!Singleton<ModifiersCategorySettings>.Instance.a) Singleton<BaseGameManager>.Instance.Ec.GetBaldi().SetAnger(-16f);
                else Singleton<BaseGameManager>.Instance.Ec.GetBaldi().GetAngry(-45);
                __instance.Ec.SpawnNPC(MainClass.Instance.BaldiHead, __instance.Ec.AllTilesNoGarbage(false, true)[0].position);
                Singleton<CoreGameManager>.Instance.AddPoints(200, 0, false);
                Singleton<BaseGameManager>.Instance.Ec.AddTimeScale(new TimeScaleModifier(1.025f, 1.25f, 1.3f));
                Singleton<CoreGameManager>.Instance.audMan.FlushQueue(true);
                Singleton<CoreGameManager>.Instance.audMan.QueueAudio(TheHardestMod.MainClass.Instance.Snd_FINALE2);
                Singleton<CoreGameManager>.Instance.audMan.SetLoop(true);
                
                Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>().AddScore(120f, false, true, "you are so fucking dead");
                return false;

            }
            else
            {
                var lapMax =  Singleton<ModifiersCategorySettings>.Instance.d;
                if (lapMax != 0)
                {
                    
                    if (TheHardestMod.MainClass.Instance.laps < lapMax)
                    {
                        TheHardestMod.MainClass.Instance.laps+= 1;

                        if (TheHardestMod.MainClass.Instance.laps == 1)
                        {
                            
                            if (Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>() == null){ 
                                Singleton<BaseGameManager>.Instance.gameObject.AddComponent<PizzatowerAhhTimerManager>();
                                Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>().SetTime(30);
                            }
                            else Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>().AddTime(10f);
                            Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>().ShowLapSpr(MainClass.Instance.sp_Lap2);
                            foreach (Notebook nb in Singleton<BaseGameManager>.Instance.Ec.notebooks)
                            {
                                if (nb != MainClass.Instance.lastNotebookCollected)
                                {
                                    nb.activity.ReInit();
                                    Singleton<BaseGameManager>.Instance.AddNotebookTotal(1);
                                }

                            }
                            if (!Singleton<ModifiersCategorySettings>.Instance.a) if (Singleton<BaseGameManager>.Instance.Ec.GetBaldi() != null) Singleton<BaseGameManager>.Instance.Ec.GetBaldi().SetAnger(-9f);
                            else if (Singleton<BaseGameManager>.Instance.Ec.GetBaldi() != null) Singleton<BaseGameManager>.Instance.Ec.GetBaldi().GetAngry(-20);

                            Singleton<CoreGameManager>.Instance.AddPoints(100, 0, false);
                            Singleton<BaseGameManager>.Instance.Ec.AddTimeScale(new TimeScaleModifier(1.05f, 1.05f, 1.07f));
                            Singleton<CoreGameManager>.Instance.audMan.FlushQueue(true);
                            Singleton<CoreGameManager>.Instance.audMan.QueueAudio(TheHardestMod.MainClass.Instance.Snd_Lap1);
                            Singleton<CoreGameManager>.Instance.audMan.SetLoop(true);
                            Singleton<CoreGameManager>.Instance.GetHud(0).BaldiTv.Speak(MainClass.Instance.Snd_Bal_Lap1);
                        }
                        if (TheHardestMod.MainClass.Instance.laps == 2)
                        {
                            if (Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>() == null) Singleton<BaseGameManager>.Instance.gameObject.AddComponent<PizzatowerAhhTimerManager>();
                            else Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>().AddTime(10f);
                            Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>().ShowLapSpr(MainClass.Instance.sp_Lap3);
                            foreach (Notebook nb in Singleton<BaseGameManager>.Instance.Ec.notebooks)
                            {
                                if (nb != MainClass.Instance.lastNotebookCollected)
                                {
                                    nb.activity.ReInit();
                                    Singleton<BaseGameManager>.Instance.AddNotebookTotal(1);
                                }

                            }
                            if (!Singleton<ModifiersCategorySettings>.Instance.a) if (Singleton<BaseGameManager>.Instance.Ec.GetBaldi() != null) Singleton<BaseGameManager>.Instance.Ec.GetBaldi().SetAnger(6f);
                            else if (Singleton<BaseGameManager>.Instance.Ec.GetBaldi() != null) Singleton<BaseGameManager>.Instance.Ec.GetBaldi().GetAngry(-10);
                            
                            Singleton<CoreGameManager>.Instance.AddPoints(200, 0, false);
                            Singleton<BaseGameManager>.Instance.Ec.AddTimeScale(new TimeScaleModifier(1.05f, 1.05f, 1.07f));
                            Singleton<CoreGameManager>.Instance.audMan.FlushQueue(true);
                            Singleton<CoreGameManager>.Instance.audMan.QueueAudio(TheHardestMod.MainClass.Instance.Snd_Lap2);
                            Singleton<CoreGameManager>.Instance.audMan.SetLoop(true);
                        }
                        if (TheHardestMod.MainClass.Instance.laps == 3)
                        {

                            if (Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>() == null) Singleton<BaseGameManager>.Instance.gameObject.AddComponent<PizzatowerAhhTimerManager>();
                            else Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>().AddTime(10f);
                            Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>().ShowLapSpr(MainClass.Instance.sp_Lap4);
                            foreach (Notebook nb in Singleton<BaseGameManager>.Instance.Ec.notebooks)
                            {
                                if (nb != MainClass.Instance.lastNotebookCollected)
                                {
                                    nb.activity.ReInit();
                                    Singleton<BaseGameManager>.Instance.AddNotebookTotal(1);
                                }

                            }

                            foreach (Cell light in Singleton<BaseGameManager>.Instance.Ec.AllCells()) {
                                light.SetLight(true);
                                light.lightColor = Color.cyan;
                                light.lightStrength = UnityEngine.Random.Range(2,8);
                                Singleton<BaseGameManager>.Instance.Ec.QueueLightSourceForRegenerate(light);
                            }


                            if (!Singleton<ModifiersCategorySettings>.Instance.a) if (Singleton<BaseGameManager>.Instance.Ec.GetBaldi() != null) Singleton<BaseGameManager>.Instance.Ec.GetBaldi().SetAnger(0f);
                            else if (Singleton<BaseGameManager>.Instance.Ec.GetBaldi() != null) Singleton<BaseGameManager>.Instance.Ec.GetBaldi().GetAngry(-8);

                            Singleton<CoreGameManager>.Instance.AddPoints(350, 0, false);
                            Singleton<BaseGameManager>.Instance.Ec.AddTimeScale(new TimeScaleModifier(1.05f, 1.05f, 1.07f));
                            Singleton<CoreGameManager>.Instance.audMan.FlushQueue(true);
                            Singleton<CoreGameManager>.Instance.audMan.QueueAudio(TheHardestMod.MainClass.Instance.Snd_Lap3);
                            Singleton<CoreGameManager>.Instance.audMan.SetLoop(true);
                        }
                        if (TheHardestMod.MainClass.Instance.laps == 4)
                        {
                            if (Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>() == null) Singleton<BaseGameManager>.Instance.gameObject.AddComponent<PizzatowerAhhTimerManager>();
                            else Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>().SetTime(10f);
                            Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>().ShowLapSpr(MainClass.Instance.sp_Lap5);
                            foreach (Notebook nb in Singleton<BaseGameManager>.Instance.Ec.notebooks)
                            {
                                if (nb != MainClass.Instance.lastNotebookCollected)
                                {
                                    nb.activity.ReInit();
                                    Singleton<BaseGameManager>.Instance.AddNotebookTotal(1);
                                }

                            }
                            
                            if (Singleton<BaseGameManager>.Instance.Ec.GetBaldi() != null) Singleton<BaseGameManager>.Instance.Ec.GetBaldi().Praise(6f,false);

                            foreach (Cell light in Singleton<BaseGameManager>.Instance.Ec.AllCells()) {
                                light.SetLight(true);
                                light.lightColor = Color.red;
                                light.lightStrength = UnityEngine.Random.Range(0,5);
                                Singleton<BaseGameManager>.Instance.Ec.QueueLightSourceForRegenerate(light);
                            }


                            if (!Singleton<ModifiersCategorySettings>.Instance.a) if (Singleton<BaseGameManager>.Instance.Ec.GetBaldi() != null) Singleton<BaseGameManager>.Instance.Ec.GetBaldi().SetAnger(-6f);
                            else if (Singleton<BaseGameManager>.Instance.Ec.GetBaldi() != null) Singleton<BaseGameManager>.Instance.Ec.GetBaldi().GetAngry(-8);

                            Singleton<CoreGameManager>.Instance.AddPoints(350, 0, false);
                            Singleton<BaseGameManager>.Instance.Ec.AddTimeScale(new TimeScaleModifier(1.05f, 1.05f, 1.07f));
                            Singleton<CoreGameManager>.Instance.audMan.FlushQueue(true);
                            Singleton<CoreGameManager>.Instance.audMan.QueueAudio(TheHardestMod.MainClass.Instance.Snd_Lap4);
                            Singleton<CoreGameManager>.Instance.audMan.SetLoop(true);
                        }
                        Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>().AddScore(80 * TheHardestMod.MainClass.Instance.laps * Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>().time / 350, false,true,"New Lap");
                        Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>().multiplierAdditive += 3.5f * TheHardestMod.MainClass.Instance.laps;


                        Singleton<CoreGameManager>.Instance.audMan.PlaySingle(MainClass.Instance.Snd_NextLap);
                        Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>().TimeBetweenPoint -= 0.025f;
                        Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>().scoreToRemove += 2;
                        return false;
                    } else return true;
                    
                } else {
                return true;
            }
            }

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MainGameManager), "BeginPlay")]

        static public bool beginPlayPatch(MainGameManager __instance)
        {
            Singleton<BaseGameManager>.Instance.Ec.map.CompleteMap();
            if(Singleton<ModifiersCategorySettings>.Instance.h)
            {
                var funnifog = new Fog();
                funnifog.startDist = 0;
                funnifog.maxDist = 80;
                funnifog.color = Color.black;
                funnifog.strength = 2;
                Singleton<BaseGameManager>.Instance.Ec.AddFog(funnifog);
                Singleton<BaseGameManager>.Instance.Ec.UpdateFog();
            }




            if (Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>() == null) {
                
                Singleton<CoreGameManager>.Instance.gameObject.AddComponent<ScoreManager>();
                var ScoreManagerV = Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>();




            var a = Singleton<ModifiersCategorySettings>.Instance.a;
            var b = Singleton<ModifiersCategorySettings>.Instance.b;
            var c = Singleton<ModifiersCategorySettings>.Instance.c;
            var d = Singleton<ModifiersCategorySettings>.Instance.d;
            var e = Singleton<ModifiersCategorySettings>.Instance.e;
            var f = Singleton<ModifiersCategorySettings>.Instance.f;
                var g = Singleton<ModifiersCategorySettings>.Instance.g;
                var h = Singleton<ModifiersCategorySettings>.Instance.h;
                var i = Singleton<ModifiersCategorySettings>.Instance.i;


                ScoreManagerV.multiplier += a ? 0.45f : 0f;
            ScoreManagerV.multiplier += b ? 0.1f : 0f;
            ScoreManagerV.multiplier += c ? 0.15f : 0f;
            ScoreManagerV.multiplier += 0.25f * d;
            ScoreManagerV.multiplier += e ? -1f : 0f;
            ScoreManagerV.multiplier += 0.05f * f;
            ScoreManagerV.multiplier += g ? 1f : 0f;
                ScoreManagerV.multiplier += h ? 0.35f : 0f;
                ScoreManagerV.multiplier += i ? 0.5f : 0f;
            }


            Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>().CurrentScore = Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>().OldScore;
            
            TheHardestMod.MainClass.Instance.laps = 0;
              
            if (!Singleton<ModifiersCategorySettings>.Instance.a) Singleton<BaseGameManager>.Instance.Ec.AddTimeScale(new TimeScaleModifier(0.25f * Singleton<ModifiersCategorySettings>.Instance.f + 1, 1.3f, 1.75f));
            if (MainClass.Instance.CurrentTimerText != null) UnityEngine.Object.Destroy(MainClass.Instance.CurrentTimerText);
            if (Singleton<CoreGameManager>.Instance.currentMode != Mode.Main)
            {
                // the one who is in explorer mode IS COOKED 💀
                // (spawns 200 baldis if not in hide and seek mode)
                for (int i = 1; i < 100; i++)
                {
                    Singleton<CoreGameManager>.Instance.SetLives(-1,true);
                    Singleton<BaseGameManager>.Instance.Ec.SpawnNPC(MainClass.Instance.PandemoniumW, __instance.Ec.AllTilesNoGarbage(false, false)[UnityEngine.Random.Range(0, __instance.Ec.AllTilesNoGarbage(false, false).Count())].position);
                    UnityEngine.Debug.LogError(new Exception("YOU CANNOT CHEAT YOUR WAY USING EXPLORER MODE, NOW SUFFER"));

                }

            }
            else
            {

            }
            if (Singleton<ModifiersCategorySettings>.Instance.c)
            {
                Singleton<BaseGameManager>.Instance.gameObject.AddComponent<PizzatowerAhhTimerManager>();
                Singleton<BaseGameManager>.Instance.gameObject.GetComponent<PizzatowerAhhTimerManager>().SetTime(40);
                Singleton<CoreGameManager>.Instance.audMan.FlushQueue(true);
                Singleton<CoreGameManager>.Instance.audMan.QueueAudio(TheHardestMod.MainClass.Instance.Snd_speedrunMusic);
                Singleton<CoreGameManager>.Instance.audMan.SetLoop(true);

            }

            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MainGameManager), "CollectNotebook")]

        static public bool CollePatch(MainGameManager __instance, ref Notebook notebook)
        {
            TheHardestMod.MainClass.Instance.lastNotebookCollected = notebook;
            if (Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>() != null)
            {
                if (!Singleton<ModifiersCategorySettings>.Instance.c)
                {
                    Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>().AddTime(6f);
                }
                else
                {
                    Singleton<BaseGameManager>.Instance.GetComponent<PizzatowerAhhTimerManager>().AddTime(14f);
                }
            }
            if (Singleton<BaseGameManager>.Instance.Ec.GetBaldi() != null && !Singleton<ModifiersCategorySettings>.Instance.a)
            {
                Singleton<BaseGameManager>.Instance.Ec.GetBaldi().Praise(3f,false);
            }
            if (Singleton<ModifiersCategorySettings>.Instance.g) {
               
                 Singleton<BaseGameManager>.Instance.Ec.SpawnNPC(Singleton<BaseGameManager>.Instance.Ec.GetBaldi(), __instance.Ec.AllTilesNoGarbage(false, false)[UnityEngine.Random.Range(0, __instance.Ec.AllTilesNoGarbage(false, false).Count())].position);
                 foreach(Cell cell in Singleton<BaseGameManager>.Instance.Ec.AllTilesNoGarbage(true,true)) {
                    cell.lightColor = new Color32((byte)UnityEngine.Random.Range(0f,255f),(byte)UnityEngine.Random.Range(0f,255f),(byte)UnityEngine.Random.Range(0f,255f),255);
                    cell.lightStrength = ((int)UnityEngine.Random.Range(0f,8f));
                    cell.SetLight(true);
                    Singleton<BaseGameManager>.Instance.Ec.QueueLightSourceForRegenerate(cell);
                 }
               
            
            }
            Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>().AddScore(50f,false,true, "Notebook collected");
            Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.AddStamina(CoreGameManager.Instance.GetPlayer(0).plm.staminaMax,true);
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Elevator), "SetState")]
        static public bool ElevClosedPatch(ref ElevatorState state) {
            if (Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>() && state == ElevatorState.OutOfOrder)
            {
                Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>().AddScore(35f, false, true, "Elevator closed");
            }
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BaseGameManager), "LoadNextLevel")]
        static public void OnStartPatch(BaseGameManager __instance)
        {
            Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>().OldScore = Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>().CurrentScore;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BaseGameManager), "EndGame")]
        static public void ded(BaseGameManager __instance)
        {
            Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>().CurrentScore = Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>().OldScore ;
            Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>().multiplierAdditive = 0;
        }



    }
}

