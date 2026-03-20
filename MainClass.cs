using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Components;
using MTM101BaldAPI.ObjectCreation;
using MTM101BaldAPI.OptionsAPI;
using MTM101BaldAPI.PlusExtensions;
using MTM101BaldAPI.Reflection;
using MTM101BaldAPI.Registers;
using PlusStudioLevelFormat;
using PlusStudioLevelLoader;

using System.Collections;
using TheHardestMod.Events;
using TheHardestMod.Extensions;
using TheHardestMod.Npcs;
using TheHardestMod.ObjectExtensions;
using TheHardestMod.Others;
using TMPro;
using UnityEngine;




namespace TheHardestMod
{
    [BepInPlugin("Partynoob.HardestMod","The hardest mod", "2.0.0.0")]
    
    internal class MainClass : BaseUnityPlugin
    {

        internal static MainClass Instance {get; private set;}
        internal List<RoomAsset> Classes;
        internal string modpath;
        internal ItemObject RandomEffect;
        internal TextMeshProUGUI CurrentTimerText;
        internal ItemObject MetalPipe;
        internal ItemObject Hammer;
        internal AudioClip randomEffect_PickUp;
        internal Sprite MysteryPotionImage;
        internal Sprite MetalPipeImage;
        internal Sprite HammerImage;
        internal Sprite PandemoniumImage;
        internal Sprite HarbingerImage;
        internal Sprite BaldiHeadImage;
        internal Sprite SubspaceTripmineImage;
        internal Sprite CursorImage;
        internal Sprite GoalImage;
        internal Sprite WitchingHourImage;
        internal Sprite Itsbalditime;
        internal Sprite sp_Lap2;
        internal Sprite sp_Lap3;
        internal Sprite sp_Lap4;
        internal Sprite sp_Lap5;
        internal PandemoniumNPC Pandemonium;
        internal PandemoniumWanderNPC PandemoniumW;
        internal HarbingerNPC Harbinger;
        internal BaldiHeadNPC BaldiHead;
        internal BaldiHeadWanderNPC BaldiHeadW;
        internal witchinghourNPC Witchinghour;
        internal SubspaceTripmine SubspaceTripmineNPC;
        internal SoundObject MetalPipe_Sound;
        internal AudioClip MetalPipe_Sfx;
        internal AudioClip Sfx_RandomEffect_Drink;
        internal AudioClip Sfx_Pandemonium_Scream;
        internal AudioClip Sfx_Pandemonium_Moving;
        internal int laps = 0;
        internal SoundObject Snd_Sfx_RandomEffect_Drink;
        public SoundObject Snd_Sfx_Pandemonium_Scream;
        internal SoundObject Snd_pd1;
        internal SoundObject Snd_pd2;
        internal SoundObject Snd_pd3;
        internal SoundObject Snd_Bang_Locker;
        internal SoundObject Snd_funtwisted;
        internal AudioClip pd1;
        internal AudioClip pd2;
        internal AudioClip pd3;
        internal LocalizationAsset Eng;
        internal SceneObject CurrSO;
        internal AudioClip MinigameMusic;
        internal AudioClip MinigameMusic2;
        internal AudioClip FINALE;
        internal AudioClip SpeedrunMusic;
        internal AudioClip Lap1;
        internal AudioClip Lap2;
        internal AudioClip Lap3;
        internal AudioClip Lap4;
        internal AudioClip nextLap;
        internal AudioClip PlusPoint;
        internal AudioClip MinusPoint;
        internal AudioClip Bal_Lap1;
        internal AudioClip SubMine_Activate;
        internal AudioClip SubMine_Explode;
        internal AudioClip SubMineVoice;
        internal AudioClip HarbingerMusic;
        internal AudioClip HarbingerComing;
        internal AudioClip Har1;
        internal AudioClip FinalePart2;
        internal SoundObject Snd_mus_Minigame_Pand;
        internal SoundObject Snd_mus_Minigame_Pand2;
        internal SoundObject Snd_FINALE;
        internal SoundObject Snd_FINALE2;
        internal SoundObject Snd_speedrunMusic;
        internal SoundObject Snd_Lap1;
        internal SoundObject Snd_Lap2;
        internal SoundObject Snd_Lap3;
        internal SoundObject Snd_Lap4;
        internal SoundObject Snd_Har1;
        internal SoundObject Snd_SubMineVoice;
        internal SoundObject Snd_NextLap;
        internal SoundObject Snd_Sfx_Pandemonium_Moving;
        internal SoundObject Snd_Bal_Lap1;
        internal SoundObject Snd_PlusPoint;
        internal SoundObject Snd_MinusPoint;
        internal SoundObject Snd_SubMine_Activate;
        internal SoundObject Snd_SubMine_Explode;
        internal SoundObject Snd_HarbingerMusic;
        internal SoundObject Snd_HarbingerComing;
        internal SoundObject Snd_SP1;
        internal SoundObject Snd_SP2;
        internal SoundObject Snd_SP3;
        internal SoundObject Snd_SP4;
        internal SoundObject Snd_SP5;
        internal SoundObject Snd_FinaleChallenge1;
        internal RandomEvent PandemoniumEvent;
        internal RandomEvent SubTripmineEvent;
        internal PlayerManager PlayerInLocker;
        internal Notebook lastNotebookCollected;
        internal AssetManager assetManager;


        internal GameLoader GL;
        internal ElevatorScreen ELS;


        internal SceneObject Challenge1;
        internal SceneObject Challenge2;
        internal SceneObject Challenge3;
        internal SceneObject Challenge4;
        internal List<Sprite> TimerSprites = new();
        ConfigEntry<bool> InsaneMode;
        SceneObject Yay;
        List<string> SafeMods = [""];
        void Awake()
        {
            assetManager = new();
            var deeznuts = new GameObject("Mc");
            deeznuts.AddComponent<ModifiersCategorySettings>();
            Harmony harmony = new Harmony("Partynoob.HardestMod");
            // Things to put
            MTM101BaldiDevAPI.AddWarningScreen("Access this mod's challenges by going in the options and finding the challenges tab ", false);
            MTM101BaldiDevAPI.AddWarningScreen("There are now modifiers (currently only 9) , to activate or deactivate a modifier just go in the settings ", false);
            // pre-release thing?
            MTM101BaldiDevAPI.AddWarningScreen("You are currently playing the 10th public version (2.0) \n\n and also it's not finished YET so there is still a <color=green>LOT</color> to come", false);
            MTM101BaldiDevAPI.AddWarningScreen("Now before we start let's all say:\n \"Thank you mystman for making the hardest mod 5 times harder\" \n\nok bye", false);

            // credits
            MTM101BaldiDevAPI.AddWarningScreen("oh yeah credits (purple = discord, red=youtube) thanks to: \n <color=purple>@cheemzit_kiri</color> for some sprites/sounds \n <color=purple>@_pixelguy</color> for helping me for some script and also baldi voice for events</color>\n <color=purple>@missingtextureman101</color> for helping me too, and also their api!", false);
            MTM101BaldiDevAPI.AddWarningScreen("thanks also to everyone down here who made some of the musics used in the mod:\n <color=red>@bartuscus</color>,<color=red>@NoLongerNullMUSIC</color> and <color=purple>@bsideskid</color>", false);
            MTM101BaldiDevAPI.AddWarningScreen("and finally thanks to <color=purple>@test_dithered99</color> for playing the pre-release 1, 2,3 and 4", false);
            modpath = MTM101BaldAPI.AssetTools.AssetLoader.GetModPath(this);

            MTM101BaldAPI.Registers.LoadingEvents.RegisterOnAssetsLoaded(this.Info,OnLoaded(),LoadingEventOrder.Pre);
            
            AssetLoader.LocalizationFromFile(Path.Combine(modpath,"Localization","Eng.json"), Language.English);
            InsaneMode = Config.Bind<bool>("Modifiers", "Insane Mode", false, "Removes everything that makes the game easier (Speed boost, baldi pausing (except pandemonium), and on lap 2 baldi will only slow a little bit instead of a lot) and every floor is bigger too Good luck!");
            CustomOptionsCore.OnMenuInitialize += AddCategory;


            
            // Items

            
          








            MTM101BaldAPI.Registers.GeneratorManagement.Register(this, MTM101BaldAPI.Registers.GenerationModType.Override, (LevelName, LevelNo,CustomLO) => {
                foreach (var level in CustomLO.GetCustomLevelObjects())
                {


                    RoomGroup[] aaa = [level.roomGroup.First(x => x.name == "Class"), level.roomGroup.First(x => x.name == "Faculty"), level.roomGroup.First(x => x.name == "Office")];

                      

                    CustomLO.mapPrice = Int32.MaxValue;

                    level.minEventGap = 2;
                    level.maxEventGap = 120;
                    aaa[0].potentialRooms = [];
                    foreach (var classs in Classes)
                    {
                        

                        aaa[0].potentialRooms = aaa[0].potentialRooms.AddToArray(new()
                        {
                            selection = classs,
                            weight = 100
                        });
                    }
                    level.potentialItems = [
                    new WeightedItemObject{
                        weight = 100,
                        selection = RandomEffect
                    },
                    new WeightedItemObject{
                        weight = 100,
                        selection = MetalPipe
                    }
                    ];
                    level.standardDarkLevel = Color.black;
                    level.standardLightStrength = 6;

                    CustomLO.totalShopItems = 8;
                    CustomLO.shopItems = [new WeightedItemObject{
                        weight = 100,
                        selection = MetalPipe
                    },
                    new WeightedItemObject{
                        weight = 45,
                        selection = RandomEffect
                    }];
                    level.timeLimit = int.MaxValue;
                    level.randomEvents = [
                        new WeightedRandomEvent{
                            weight = 100,
                            selection = PandemoniumEvent
                        },
                        new WeightedRandomEvent{
                            weight = 125,
                            selection = SubTripmineEvent
                        }
                        ];

                    if (LevelName == "F1")
                    {
                        aaa[0].maxRooms = 5;
                        aaa[0].minRooms = 5;
                        level.maxSize = new IntVector2(28, 28);
                        level.minSize = new IntVector2(12, 12);
                        level.randomEvents = [

                        new WeightedRandomEvent{
                            weight = 125,
                            selection = SubTripmineEvent
                        }
                        ];
                        CustomLO.mapPrice = Int32.MaxValue;

                        CustomLO.usesMap = true;
                        level.additionTurnChance = -20;
                        level.deadEndBuffer = 1;
                        level.forcedItems = [];

                    }
                    if (LevelName == "F2")
                    {
                        aaa[0].maxRooms = 8;
                        aaa[0].minRooms = 8;
                        aaa[1].maxRooms = 20;
                        aaa[1].minRooms = 20;
                        level.exitCount = 1;
                        level.maxSize = new IntVector2(36, 36);
                        level.minSize = new IntVector2(30, 30);

                        



                        CustomLO.usesMap = true;
                        level.additionTurnChance = -20;
                        level.deadEndBuffer = 1;
                        level.forcedItems = [];


                    }
                    if (LevelName == "F3")
                    {
                        aaa[0].maxRooms = 13;
                        aaa[0].minRooms = 13;
                        aaa[1].maxRooms = 40;
                        aaa[1].minRooms = 40;
                        level.exitCount = 2;
                        level.maxSize = new IntVector2(67, 67);
                        level.minSize = new IntVector2(61, 61);



                        CustomLO.usesMap = true;
                        level.additionTurnChance = 500;
                        level.deadEndBuffer = 1;
                        level.forcedItems = [];


                    }
                    if (LevelName == "F4")
                    {
                        aaa[0].maxRooms = 17;
                        aaa[0].minRooms = 17;
                        aaa[1].maxRooms = 40;
                        aaa[1].minRooms = 40;
                        level.exitCount = 2;
                        level.maxSize = new IntVector2(87, 87);
                        level.minSize = new IntVector2(80, 80);
                        

                        CustomLO.usesMap = true;
                        level.additionTurnChance = 500;
                        level.deadEndBuffer = 1;
                        level.forcedItems = [];


                    }
                    if (LevelName == "F5")
                    {
                        aaa[0].maxRooms = 1;
                        aaa[0].minRooms = 1;
                        aaa[1].maxRooms = 40;
                        aaa[1].minRooms = 40;
                        level.exitCount = 4;
                        level.maxSize = new IntVector2(99, 99);
                        level.minSize = new IntVector2(92, 92);



                        CustomLO.usesMap = true;
                        level.additionTurnChance = 500;
                        level.deadEndBuffer = 1;
                        level.forcedItems = [];
                        level.finalLevel = false;
                        

                    }




                }


            });


            var AllSmth = Resources.FindObjectsOfTypeAll<MathMachine>();

            


            MTM101BaldAPI.SaveSystem.ModdedSaveGame.AddSaveHandler(this.Info);
            Instance = this;
            harmony.PatchAll();
        }
        void OnMen(OptionsMenu instance, CustomOptionsHandler Ins) {
            if (Singleton<CoreGameManager>.Instance != null) return;
            var modifiers = Ins.AddCategory<CustomOptionsCategory>("Modifiers");


            

        }
       /* IEnumerator LoadAssets() {
            
            randomEffect_PickUp = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath,"Sounds","pickup_randomglasseffect.wav"));
            MetalPipe_Sfx = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath,"Sounds","MetalPipe_SFX.wav"));
            
            Classes = EditorCustomRooms.RoomFactory.CreateAssetsFromPath(Path.Combine(modpath,"Rooms","ClassRooms.cbld"),40,true,null,false,false,null,true,false);
            
            MysteryPotionImage = AssetLoader.SpriteFromFile(Path.Combine(modpath,"Items","MysteryPotion.png"),new Vector2(0.5f,0.5f),30f);
            MetalPipeImage = AssetLoader.SpriteFromFile(Path.Combine(modpath,"Items","MetalPipe.png"),new Vector2(0.5f,0.5f),40f);
           


            yield break;
        }*/
        // Example of a method to override behavior in the game
        private void Update()
        {
            // Place code here that runs every frame (if needed)
        }

        void AddCategory(OptionsMenu __instance, CustomOptionsHandler handler)
        {
            if (Singleton<CoreGameManager>.Instance != null) return;
            handler.AddCategory<ModifiersCategory>("Modifiers");
            handler.AddCategory<ChallengesMenu>("Challenges");
        }

        private void CustomLevelGenerator(SceneObject SO) {
            foreach (var level in SO.GetCustomLevelObjects())
            {


                level.timeLimit = int.MaxValue;
                RoomGroup[] aaa = [level.roomGroup.First(x => x.name == "Class"), level.roomGroup.First(x => x.name == "Faculty"), level.roomGroup.First(x => x.name == "Office")];
                SO.mapPrice = Int32.MaxValue;
                level.minEventGap = 5;
                level.maxEventGap = 120;
                foreach (var classs in Classes)
                {
                    aaa[0].potentialRooms=aaa[0].potentialRooms.AddToArray(new()
                    {
                        selection = classs,
                        weight = 100
                    });
                }
                level.potentialItems = [
                new WeightedItemObject{
                        weight = 100,
                        selection = RandomEffect
                    },
                    new WeightedItemObject{
                        weight = 10,
                        selection = MetalPipe
                    }
                ];
                SO.totalShopItems = 1;
                SO.shopItems = [new WeightedItemObject{
                        weight = 100,
                        selection = MetalPipe
                    }];
                level.randomEvents = [

                ];


                if (SO.levelTitle == "F6")
                {
                    aaa[0].maxRooms = 25;
                    aaa[0].minRooms = 25;
                    aaa[1].maxRooms = 1;
                    aaa[1].minRooms = 1;
                    level.maxSize = new IntVector2(20, 20);
                    level.minSize = new IntVector2(20, 20);
                    level.exitCount = 4;
                    aaa[0].potentialRooms = [
                    new WeightedRoomAsset
                        {
                            selection = assetManager.Get<RoomAsset>("rm_small"),
                            weight = 100
                        }



                ];
                }



            }




                    Debug.Log(SO.levelTitle + " has been modified sucessfully");

        }





            IEnumerator OnLoaded() {
            // assets
                yield return 9;
                yield return "Loading audio...";
           
            randomEffect_PickUp = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath,"Sounds","pickup_randomglasseffect.wav"));
           
            MetalPipe_Sfx = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath,"Sounds","MetalPipe_SFX.wav"));

            MinigameMusic = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath,"Sounds","Knock Knock.wav"));
            
            MinigameMusic2 = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "waitoftheworldbaldi.wav"));

            FINALE = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "THEFINALE.wav"));

            FinalePart2 = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "phase3.ogg"));

            SpeedrunMusic = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath,"Sounds","dremdrama.mp3"));

            Lap1 = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath,"Sounds","Lap1.mp3"));

            Lap2= MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath,"Sounds","Lap2.mp3"));

            Lap3= MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath,"Sounds","youaresoDEAD.wav"));

            Lap4= MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath,"Sounds","Lap4.wav"));
           
            Sfx_RandomEffect_Drink = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath,"Sounds","Drinking.wav"));

            nextLap = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath,"Sounds","nextLap.wav"));

            Sfx_Pandemonium_Scream = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "Pand_SCREAM.mp3"));

            Sfx_Pandemonium_Moving = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "Pand_MOVING.wav"));

            Har1 = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "ItsTimeForSure.wav"));

            HarbingerComing = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "harbingeriscoming.mp3"));

            HarbingerMusic = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "justgiveup.mp3"));

            SubMine_Activate = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "st_activate.wav"));

            SubMine_Explode = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "st_explode.wav"));


            var Sfx_Locker_Bang = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath,"Sounds","LockerBang.wav"));

            PlusPoint = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath,"Sounds","PlusPoint.wav"));

            MinusPoint = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath,"Sounds","LostPoint.wav"));

            pd1 = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Voices", "Pandemonium1.wav"));
            pd2 = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Voices", "Pandemonium2.wav"));
            pd3 = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Voices", "Pandemonium3.wav"));

            SubMineVoice = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Voices", "Tripmine.wav"));

            Bal_Lap1 = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath,"Voices","CollectingWow.wav"));
            yield return "Loading rooms...";
            var rooms = Extensions.Extensions.LoadFolderRooms(Path.Combine(modpath, "Rooms", "Classrooms"));
            Classes = new List<RoomAsset>();
            foreach (var item in rooms)
            {
                Classes.Add(LevelImporter.CreateVanillaRoomAsset(item));
            }



            var PendoRoom = LevelImporter.CreateVanillaRoomAsset(Extensions.Extensions.LoadRoom(Path.Combine(modpath, "Rooms", "PandemoniumRoomYeah.rbpl")));
            var SmolRoom = LevelImporter.CreateVanillaRoomAsset(Extensions.Extensions.LoadRoom(Path.Combine(modpath, "Rooms", "SmolRoom.rbpl")));
            assetManager.Add("rm_small", SmolRoom);
            yield return "Loading images...";

            MysteryPotionImage = AssetLoader.SpriteFromFile(Path.Combine(modpath,"Items","MysteryPotion.png"),new Vector2(0.5f,0.5f),30f);
            
            MetalPipeImage = AssetLoader.SpriteFromFile(Path.Combine(modpath,"Items","MetalPipe.png"),new Vector2(0.5f,0.5f),40f);

            HammerImage = AssetLoader.SpriteFromFile(Path.Combine(modpath,"Items","hammer.png"),new Vector2(0.5f,0.5f),1f);

            PandemoniumImage = AssetLoader.SpriteFromFile(Path.Combine(modpath, "Npcs", "Pandemonium.png"), new Vector2(0.5f, 0.5f), 40f);

            BaldiHeadImage = AssetLoader.SpriteFromFile(Path.Combine(modpath, "Npcs", "baldihead.png"), new Vector2(0.5f, 0.5f), 40f);

            HarbingerImage = AssetLoader.SpriteFromFile(Path.Combine(modpath, "Npcs", "Harbinger.png"), new Vector2(0.5f, 0.5f), 26.66f);

            SubspaceTripmineImage = AssetLoader.SpriteFromFile(Path.Combine(modpath, "Npcs", "Subspace_Tripmine.png"), new Vector2(0.5f, 0.5f), 50f);

            CursorImage = AssetLoader.SpriteFromFile(Path.Combine(modpath,"Image","Cursor.png"),new Vector2(0.5f,0.5f),2f);

            GoalImage = AssetLoader.SpriteFromFile(Path.Combine(modpath, "Image", "Goal.png"), new Vector2(0.5f, 0.5f), 2f);

            Itsbalditime = AssetLoader.SpriteFromFile(Path.Combine(modpath, "Image", "balditime1.png"), new Vector2(0.5f, 0.5f), 2f);

            sp_Lap2 = AssetLoader.SpriteFromFile(Path.Combine(modpath, "Image", "Lap2.png"), new Vector2(0.5f, 0.5f), 2f);
            sp_Lap3 = AssetLoader.SpriteFromFile(Path.Combine(modpath, "Image", "Lap3.png"), new Vector2(0.5f, 0.5f), 2f);
            sp_Lap4 = AssetLoader.SpriteFromFile(Path.Combine(modpath, "Image", "Lap4.png"), new Vector2(0.5f, 0.5f), 2f);
            sp_Lap5 = AssetLoader.SpriteFromFile(Path.Combine(modpath, "Image", "Lap5.png"), new Vector2(0.5f, 0.5f), 2f);

            assetManager.Add("spr_helpHide", AssetLoader.SpriteFromFile(Path.Combine(modpath, "Image", "HideHereHelper.png"), new Vector2(0.5f, 0.5f), 45f));

            WitchingHourImage = AssetLoader.SpriteFromFile(Path.Combine(modpath, "Image", "witchinghour.png"), new Vector2(0.5f, 0.5f), 160f);

            for (int i = 0; i < 10; i++)
            {
                var eee = AssetLoader.SpriteFromFile(Path.Combine(modpath, "Image", $"timer{i}.png"), new Vector2(0.5f, 0.5f), 23f);
                TimerSprites.Add(eee);
            }
            TimerSprites.Add(AssetLoader.SpriteFromFile(Path.Combine(modpath, "Image", $"timercolon.png"), new Vector2(0.5f, 0.5f), 23f));

            yield return "Creating sound objects...";
            
            // soundObjects
            SoundObject PickUpSoundRndEffect = ScriptableObject.CreateInstance<SoundObject>();
            PickUpSoundRndEffect.color = Color.white;
            PickUpSoundRndEffect.soundKey = "sfx_PickUp_ITM_RandomEffect";
            PickUpSoundRndEffect.soundType = SoundType.Effect;
            PickUpSoundRndEffect.soundClip = randomEffect_PickUp;

            MetalPipe_Sound = MTM101BaldAPI.ObjectCreators.CreateSoundObject(MetalPipe_Sfx, "Sfx_MetalPipe",SoundType.Effect,Color.gray);
            MetalPipe_Sound.volumeMultiplier = 20;

            Snd_Sfx_RandomEffect_Drink = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Sfx_RandomEffect_Drink, "Sfx_RandomEffect_Drink",SoundType.Effect,Color.magenta);
            Snd_Har1 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Har1, "event_itstime1", SoundType.Voice, Color.green);
            Snd_pd1 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(pd1, "event_itstime1", SoundType.Voice, Color.green);
            Snd_pd1.additionalKeys = [
                new SubtitleTimedKey {
                    key="event_itstime2",
                    time=4.89f
                },
                new SubtitleTimedKey {
                    key="event_itstime3",
                    time=6.429f
                }
                ];
            Snd_pd2 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(pd2, "event_pdm", SoundType.Voice, Color.green);
            Snd_pd3 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(pd3, "event_pdm", SoundType.Voice, Color.green);

            Snd_Bal_Lap1 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Bal_Lap1,"bal_troll",SoundType.Voice,Color.green);

            Snd_Sfx_Pandemonium_Scream = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Sfx_Pandemonium_Scream,"Pandemonium_SCREAM",SoundType.Voice,Color.black,1200);

            Snd_Sfx_Pandemonium_Moving = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Sfx_Pandemonium_Moving,"Pandemonium_MOVING",SoundType.Voice,Color.black,1200);

            Snd_mus_Minigame_Pand = MTM101BaldAPI.ObjectCreators.CreateSoundObject(MinigameMusic, "*music*", SoundType.Music, Color.black);

            Snd_mus_Minigame_Pand2 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(MinigameMusic2, "*music*", SoundType.Music, Color.black);

            Snd_FINALE = MTM101BaldAPI.ObjectCreators.CreateSoundObject(FINALE, "*music*", SoundType.Music, Color.black, 0.1f);

            Snd_FINALE2 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(FinalePart2, "*music*", SoundType.Music, Color.black, 0.1f);

            Snd_speedrunMusic = MTM101BaldAPI.ObjectCreators.CreateSoundObject(SpeedrunMusic,"*music*",SoundType.Music,Color.black,0.1f);

            Snd_Lap1 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Lap1,"*music*",SoundType.Music,Color.black,0.1f);

            Snd_Lap2 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Lap2,"*music*",SoundType.Music,Color.black,0.1f);

            Snd_Lap3 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Lap3, "*music*", SoundType.Music, Color.black, 0.1f);

            Snd_Lap4 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Lap4, "*music*", SoundType.Music, Color.black, 0.1f);

            Snd_HarbingerComing = MTM101BaldAPI.ObjectCreators.CreateSoundObject(HarbingerComing, "har_coming", SoundType.Voice, Color.magenta, 1200);

            Snd_HarbingerMusic = MTM101BaldAPI.ObjectCreators.CreateSoundObject(HarbingerMusic, "*music*", SoundType.Effect, Color.black);

            Snd_SubMineVoice = MTM101BaldAPI.ObjectCreators.CreateSoundObject(SubMineVoice, "bal_submine", SoundType.Voice, Color.green);

            Snd_NextLap = MTM101BaldAPI.ObjectCreators.CreateSoundObject(nextLap,"*music*",SoundType.Music,Color.black,0.1f);

            Snd_Bang_Locker = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Sfx_Locker_Bang,"*bang*",SoundType.Effect,Color.gray,0.6f);

            Snd_PlusPoint = MTM101BaldAPI.ObjectCreators.CreateSoundObject(PlusPoint,"UnusedSubtitle",SoundType.Effect,Color.gray,0.6f);

            Snd_MinusPoint = MTM101BaldAPI.ObjectCreators.CreateSoundObject(MinusPoint,"UnusedSubtitle",SoundType.Effect,Color.gray,0.6f);

            Snd_SubMine_Activate = MTM101BaldAPI.ObjectCreators.CreateSoundObject(SubMine_Activate, "Submine_Activate", SoundType.Effect, Color.magenta,9);

            Snd_SubMine_Explode = MTM101BaldAPI.ObjectCreators.CreateSoundObject(SubMine_Explode, "Submine_Explode", SoundType.Effect, Color.magenta);

            var Temp = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "SPPhase1.ogg"));
            Snd_SP1 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Temp, "UnusedSubtite", SoundType.Effect, Color.white);
             Temp = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "SPPhase2.ogg"));
            Snd_SP2 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Temp, "UnusedSubtite", SoundType.Effect, Color.white);
             Temp = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "SPPhase3.ogg"));
            Snd_SP3 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Temp, "UnusedSubtite", SoundType.Effect, Color.white);
             Temp = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "SPPhase4.ogg"));
            Snd_SP4 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Temp, "UnusedSubtite", SoundType.Effect, Color.white);
            Temp = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "SPPhase5.ogg"));
            Snd_SP5 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Temp, "UnusedSubtite", SoundType.Effect, Color.white);
            Temp = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "twistedfun.ogg"));
            Snd_funtwisted = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Temp, "UnusedSubtite", SoundType.Effect, Color.white);
            Temp = MTM101BaldAPI.AssetTools.AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "finaleChallenge1.ogg"));
            Snd_FinaleChallenge1 = MTM101BaldAPI.ObjectCreators.CreateSoundObject(Temp, "UnusedSubtitle", SoundType.Voice, Color.green);
            assetManager.Add("Snd_calm", MTM101BaldAPI.ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "calm.ogg")), "UnusedSubtitle", SoundType.Voice, Color.green));

            assetManager.Get<SoundObject>("Snd_calm").subtitle = false;

            assetManager.Add("Snd_nocalm", MTM101BaldAPI.ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(modpath, "Sounds", "haha.mp3")), "UnusedSubtitle", SoundType.Voice, Color.green));

            assetManager.Get<SoundObject>("Snd_nocalm").subtitle = false;
            Snd_Sfx_Pandemonium_Scream.volumeMultiplier = 0.25f;
            Snd_Sfx_Pandemonium_Moving.volumeMultiplier = 0.5f;
            Snd_HarbingerMusic.volumeMultiplier = 1.5f;
            Snd_HarbingerComing.volumeMultiplier = 0.25f;
            Snd_Lap1.subtitle = false;
            Snd_Lap2.subtitle = false;
            Snd_Lap3.subtitle = false;
            Snd_Lap4.subtitle = false;
            Snd_NextLap.subtitle = false;
            Snd_FINALE.subtitle = false;
            Snd_FINALE2.subtitle = false;
            Snd_FinaleChallenge1.subtitle = false;
            Snd_SP1.subtitle = false;
            Snd_SP2.subtitle = false;
            Snd_SP3.subtitle = false;
            Snd_funtwisted.subtitle = false;
            Snd_SP4.subtitle = false;
            Snd_SP5.subtitle = false;
            Snd_HarbingerMusic.subtitle = false;
            Snd_PlusPoint.subtitle = false;
            Snd_MinusPoint.subtitle = false;
            Snd_speedrunMusic.subtitle = false;
            // items
            yield return "Creating items...";
            RandomEffect = new ItemBuilder(this.Info)
            .SetGeneratorCost(1)
            .SetEnum("RandomEffect")
            .SetShopPrice(100)  
            .SetItemComponent<ITM_RandomEffect>()
            .SetSprites(MysteryPotionImage,MysteryPotionImage)
            .SetPickupSound(PickUpSoundRndEffect)
            .SetNameAndDescription("Itm_MysteryPotion", "Desc_MysteryPotion")
            .Build();

            MetalPipe = new ItemBuilder(this.Info)
            .SetGeneratorCost(60)
            .SetEnum("MetalPipe")
            .SetShopPrice(350)
            .SetItemComponent<ITM_MetalPipe>()
            .SetSprites(MetalPipeImage,MetalPipeImage)
            .SetNameAndDescription("Itm_MetalPipe_3", "Desc_MetalPipe")
            
            .Build();

            Hammer = new ItemBuilder(this.Info)
            .SetGeneratorCost(120)
            .SetEnum("Hammer")
            .SetShopPrice(500)
            .SetItemComponent<ITM_MetalPipe>()
            .SetSprites(HammerImage,HammerImage)
            .SetNameAndDescription("Itm_Hammer", "Desc_Hammer")
            
            .Build();
            yield return "Creating npcs...";
            // npcs
            Pandemonium = new NPCBuilder<PandemoniumNPC>(this.Info)
            .AddTrigger()
            .SetEnum("Pandemonium")
            .AddLooker()
            .SetMinMaxAudioDistance(10, 10 * 100)

            .Build();

            Pandemonium.spriteRenderer[0].sprite = PandemoniumImage;

            Pandemonium.AudMan = Pandemonium.GetComponent<PropagatedAudioManager>();

            PandemoniumW = new NPCBuilder<PandemoniumWanderNPC>(this.Info)
            .AddTrigger()
            .SetEnum("PandemoniumWander")
            .AddLooker()
            .SetMinMaxAudioDistance(10, 10 * 15)
            .AddHeatmap()

            .Build();

            PandemoniumW.spriteRenderer[0].sprite = PandemoniumImage;

            PandemoniumW.AudMan =  PandemoniumW.GetComponent<PropagatedAudioManager>();

            Harbinger = new NPCBuilder<HarbingerNPC>(this.Info)
            .AddTrigger()
            .SetEnum("Harbinger")
            .AddLooker()
            .SetMinMaxAudioDistance(10, 10 * 100)

            .Build();
            Harbinger.spriteRenderer[0].sprite = HarbingerImage;

            Harbinger.AudMan = Harbinger.GetComponent<PropagatedAudioManager>();

            BaldiHead = new NPCBuilder<BaldiHeadNPC>(this.Info)
            .AddTrigger()
            .SetEnum("BaldiHead")
            .AddLooker()
            .SetMinMaxAudioDistance(10, 10 * 45)

            .Build();
            BaldiHead.spriteRenderer[0].sprite = BaldiHeadImage;


            BaldiHead.AudMan = BaldiHead.GetComponent<PropagatedAudioManager>();

            BaldiHeadW = new NPCBuilder<BaldiHeadWanderNPC>(this.Info)
            .AddTrigger()
            .SetEnum("BaldiHeadWander")
            .AddLooker()
            .SetMinMaxAudioDistance(10, 10 * 45)

            .Build();
            BaldiHeadW.spriteRenderer[0].sprite = BaldiHeadImage;


            BaldiHeadW.AudMan = BaldiHeadW.GetComponent<PropagatedAudioManager>();

            Witchinghour = new NPCBuilder<witchinghourNPC>(this.Info)
            .AddTrigger()
            .SetEnum("pinkguy")
            .AddLooker()
            .SetMinMaxAudioDistance(0,7)

            .Build();
            Witchinghour.spriteRenderer[0].sprite = WitchingHourImage;


            Witchinghour.AudMan = Witchinghour.GetComponent<PropagatedAudioManager>();

            SubspaceTripmineNPC = new NPCBuilder<SubspaceTripmine>(this.Info)
            .SetStationary()
            .SetMinMaxAudioDistance(10, 10 * 150)
            .SetEnum("SubspaceTripmine")
            
            .Build();

            SubspaceTripmineNPC.spriteRenderer[0].sprite = SubspaceTripmineImage;

            SubspaceTripmineNPC.AudMan = SubspaceTripmineNPC.GetComponent<PropagatedAudioManager>();

            // events
            yield return "Creating events...";
            PandemoniumEvent = new RandomEventBuilder<pandemoniumEvent>(this.Info)
            .SetSound(Snd_pd1)
            .SetMinMaxTime(5, 5)
            .SetEnum("PandeEvent")
            .AddRoomAsset(PendoRoom)
            .Build();

            SubTripmineEvent = new RandomEventBuilder<SubspaceTripmineEvent>(this.Info)
            .SetSound(Snd_SubMineVoice)
            .SetMinMaxTime(5, 5)
            .SetEnum("SubSpaceEvent")
            
            .Build();




            var AudMan = MetalPipe.item.gameObject.CreateAudioManager(99999,99999);
            AudMan.positional = false;
            AudMan.audioDevice = MetalPipe.item.gameObject.CreateAudioSource(99999,99999);

            AudMan = RandomEffect.item.gameObject.CreateAudioManager(99999,99999);
            AudMan.positional = false;
            AudMan.audioDevice = RandomEffect.item.gameObject.CreateAudioSource(99999,99999);


            yield return "Creating and editing new floors...";
            // new floors
            var SceneObjects = Resources.FindObjectsOfTypeAll<SceneObject>();

            SceneObject F1 = SceneObjects.Where(x => x.levelTitle == "F1").First();
            SceneObject F2 = SceneObjects.Where(x => x.levelTitle == "F2").First();
            SceneObject F3 = SceneObjects.Where(x => x.levelTitle == "F3").First();
            SceneObject F4 = SceneObjects.Where(x => x.levelTitle == "F4").First();
            SceneObject F5 = SceneObjects.Where(x => x.levelTitle == "F5").First();
            SceneObject END = SceneObjects.Where(x => x.name == "PlaceholderEnding").First();

            var F6 = F1.DuplicateAndNewLevel("F6", 5, false, true);
            F5.nextLevel = F6;
            F6.nextLevel = END;
            
            CustomLevelGenerator(F6);




            // broken
            Challenge1 = F1.DuplicateAndNewLevel("CHAL1", -100, false, true);
            Challenge1.MarkAsNeverUnload();

            var ch1_lo = Challenge1.levelObject;
            ch1_lo.maxEvents = 0;
            ch1_lo.minEvents = 0;
            ch1_lo.randomEvents = [];
            RoomGroup[] aaa = [ch1_lo.roomGroup.First(x => x.name == "Class"), ch1_lo.roomGroup.First(x => x.name == "Faculty"), ch1_lo.roomGroup.First(x => x.name == "Office")];
            aaa[0].maxRooms = 20;
            aaa[0].minRooms = 20;
            WeightedRoomAsset[] e = [];
            foreach (var item in Classes)
            {
                e = e.AddToArray(new()
                {
                    selection = item,
                    weight = 100
                });
            }
            aaa[0].potentialRooms = e;
            aaa[1].maxRooms = 0;
            aaa[1].minRooms = 0;
            ch1_lo.forcedItems = [];

            ch1_lo.potentialItems = [new WeightedItemObject {
                selection = MainClass.Instance.MetalPipe,
                weight = 0
            }];
            Challenge1.additionalNPCs = 0;
            ch1_lo.exitCount = 4;

            ch1_lo.maxSize = new IntVector2(50, 50);
            ch1_lo.minSize = new IntVector2(50, 50);
            Challenge1.potentialNPCs = [];
            ch1_lo.forcedNpcs = [];
            ch1_lo.additionTurnChance = 50;
            ch1_lo.fillEmptySpace = false;

            Challenge1.baldiPrefab = null;



            Challenge2 = F1.DuplicateAndNewLevel("calm", -102, false, true);
            Challenge2.MarkAsNeverUnload();

            var ch2_lo = Challenge2.levelObject;
            ch2_lo.maxEvents = 0;
            ch2_lo.minEvents = 0;
            ch2_lo.randomEvents = [];
            aaa = [ch2_lo.roomGroup.First(x => x.name == "Class"), ch2_lo.roomGroup.First(x => x.name == "Faculty"), ch2_lo.roomGroup.First(x => x.name == "Office")];
            aaa[0].maxRooms = 35;
            aaa[0].minRooms = 35;
            e = [];
            
                e = e.AddToArray(new()
                {
                    selection = SmolRoom,
                    weight = 100
                });
            
            aaa[0].potentialRooms = e;
            aaa[1].maxRooms = 0;
            aaa[1].minRooms = 0;
            ch2_lo.forcedItems = [];

            ch2_lo.potentialItems = [new WeightedItemObject {
                selection = MainClass.Instance.MetalPipe,
                weight = 0
            }];
            Challenge1.additionalNPCs = 0;
            ch2_lo.exitCount = 4;

            ch2_lo.maxSize = new IntVector2(30, 30);
            ch2_lo.minSize = new IntVector2(30, 30);
            Challenge2.potentialNPCs = [];
            Challenge2.forcedNpcs = [];
            ch2_lo.forcedNpcs = [];
            ch2_lo.additionTurnChance = 50;
            ch2_lo.fillEmptySpace = true;
            ch2_lo.standardLightColor = Color.magenta;
            Challenge2.baldiPrefab = null;



            var t = new GameObject();
            t.ConvertToPrefab(true);
            t.name = "Gamemanager";
            var tt = t.AddComponent<CusChallengeGameManager>();
            Challenge1.manager = tt;
            Challenge1.manager.ReflectionSetVariable("destroyOnLoad", false);
            Challenge2.manager = tt;
            Challenge2.manager.ReflectionSetVariable("destroyOnLoad", false);




        }
    }
}
