using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Registers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
namespace TheHardestMod.Others
{
    // -100 = entities are quirky
    internal class CusChallengeGameManager : BaseGameManager
    {
        private int collectedNotebooks = 0;
        private int timeleft = 15;
        private NPC CurrentMainNpc;
        private bool calm = true;
        private bool allnotebooktriggered = false;
        public override void ExitedSpawn()
        {
            base.ExitedSpawn();
            if (this.levelNo == -100)
            {
                this.ec.map.CompleteMap();

                this.Ec.AddTimeScale(new TimeScaleModifier(1, 1.3f, 1.75f));
                Singleton<CoreGameManager>.Instance.audMan.FlushQueue(true);
                Singleton<MusicManager>.Instance.StopMidi();
                Singleton<CoreGameManager>.Instance.audMan.QueueAudio(MainClass.Instance.Snd_SP1);
                Singleton<CoreGameManager>.Instance.audMan.SetLoop(true);
                Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.AddItem(MainClass.Instance.MetalPipe);
                Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.AddItem(MainClass.Instance.MetalPipe);
                Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.AddItem(MainClass.Instance.MetalPipe);

            }
            if (this.levelNo == -101)
            {
                this.ec.map.CompleteMap();

                this.Ec.AddTimeScale(new TimeScaleModifier(1, 1.3f, 2.35f));
                Singleton<CoreGameManager>.Instance.audMan.FlushQueue(true);
                Singleton<CoreGameManager>.Instance.audMan.QueueAudio(MainClass.Instance.Snd_funtwisted);
                Singleton<CoreGameManager>.Instance.audMan.SetLoop(true);
                this.ec.SpawnNPC(MainClass.Instance.BaldiHeadW, this.ec.AllTilesNoGarbage(false, false)[UnityEngine.Random.Range(0, this.Ec.AllTilesNoGarbage(false, false).Count())].position);

            }
            if (this.levelNo == -102)
            {
                this.ec.map.CompleteMap();

                this.Ec.AddTimeScale(new TimeScaleModifier(1, 1.3f, 2.35f));
                Singleton<CoreGameManager>.Instance.audMan.FlushQueue(true);
                Singleton<MusicManager>.Instance.StopMidi();
                Singleton<CoreGameManager>.Instance.audMan.QueueAudio(MainClass.Instance.assetManager.Get<SoundObject>("Snd_calm"));
                Singleton<CoreGameManager>.Instance.audMan.SetLoop(true);
                // 

            }


        }

        public override void CollectNotebook(Notebook notebook)
        {
            MainClass.Instance.lastNotebookCollected = notebook;
            base.CollectNotebook(notebook);
            
            Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.AddStamina(10000000, true);
            if (this.levelNo == -100)
            {
                collectedNotebooks+= 1;

                if (collectedNotebooks % 3 == 1) this.ec.SpawnNPC(MainClass.Instance.PandemoniumW, this.ec.AllTilesNoGarbage(false, false)[UnityEngine.Random.Range(0, this.Ec.AllTilesNoGarbage(false, false).Count())].position);
                
                if (collectedNotebooks == 6)
                {
                    Singleton<CoreGameManager>.Instance.audMan.FlushQueue(true);
                    Singleton<CoreGameManager>.Instance.audMan.QueueAudio(MainClass.Instance.Snd_SP2);
                    Singleton<CoreGameManager>.Instance.audMan.SetLoop(true);
                }
                if (collectedNotebooks == 14)
                {
                    Singleton<CoreGameManager>.Instance.audMan.FlushQueue(true);
                    Singleton<CoreGameManager>.Instance.audMan.QueueAudio(MainClass.Instance.Snd_SP3);
                    Singleton<CoreGameManager>.Instance.audMan.SetLoop(true);
                }
                if (collectedNotebooks == 17)
                {
                    Singleton<CoreGameManager>.Instance.audMan.FlushQueue(true);
                    Singleton<CoreGameManager>.Instance.audMan.QueueAudio(MainClass.Instance.Snd_SP4);
                    Singleton<CoreGameManager>.Instance.audMan.SetLoop(true);
                    
                    Singleton<CoreGameManager>.Instance.GetHud(0).SetNotebookDisplay(false);
                }

            }
            if (this.levelNo == -101)
            {
                collectedNotebooks += 1;
                if (collectedNotebooks % 5 == 0)
                {

                    Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.AddItem(ItemMetaStorage.Instance.FindByEnum(Items.Quarter).value);
                }
                if (collectedNotebooks % 10 == 0)
                {

                    this.ec.SpawnNPC(MainClass.Instance.BaldiHeadW, this.ec.AllTilesNoGarbage(false, false)[UnityEngine.Random.Range(0, this.Ec.AllTilesNoGarbage(false, false).Count())].position);
                }

                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(MainClass.Instance.Snd_PlusPoint);
                
            }
            if (levelNo == -102)
            {
                collectedNotebooks += 1;
                if (Singleton<CoreGameManager>.Instance.gameObject.GetComponent<ScoreManager>())
                {
                    Singleton<CoreGameManager>.Instance.gameObject.GetComponent<ScoreManager>().AddScore(5 * (collectedNotebooks-35), true, true, "Collected Notebook");
                }
            }

        }
        protected override void AllNotebooks()
        {

            if (this.levelNo == -100)
            {
                base.AllNotebooks();
                Singleton<CoreGameManager>.Instance.audMan.FlushQueue(true);
                Singleton<CoreGameManager>.Instance.audMan.QueueAudio(MainClass.Instance.Snd_SP5);
                Singleton<CoreGameManager>.Instance.audMan.SetLoop(true);
                this.ec.SpawnNPC(MainClass.Instance.BaldiHead, this.ec.AllTilesNoGarbage(false, false)[UnityEngine.Random.Range(0, this.Ec.AllTilesNoGarbage(false, false).Count())].position);
                Singleton<CoreGameManager>.Instance.GetHud(0).BaldiTv.Speak(MainClass.Instance.Snd_FinaleChallenge1);

            }
            if (levelNo == -102 && !calm)
            {
                if (!allnotebooktriggered)
                {
                    allnotebooktriggered = true;
                    base.AllNotebooks();
                }

                Singleton<BaseGameManager>.Instance.gameObject.GetComponent<PizzatowerAhhTimerManager>().AddTime(120-collectedNotebooks/17);
                Singleton<BaseGameManager>.Instance.gameObject.GetComponent<PizzatowerAhhTimerManager>().scoreToRemove *= 2;
                Singleton<CoreGameManager>.Instance.gameObject.GetComponent<ScoreManager>().AddScore(1, true, true, "Leave or continue.");
                foreach (Notebook nb in Singleton<BaseGameManager>.Instance.Ec.notebooks)
                {
                    if (nb != MainClass.Instance.lastNotebookCollected)
                    {
                        nb.activity.ReInit();
                        Singleton<BaseGameManager>.Instance.AddNotebookTotal(1);
                    }

                }
            }
            if (levelNo == -102 && calm)
            {
                calm = false;
                Singleton<CoreGameManager>.Instance.audMan.FlushQueue(true);
                Singleton<CoreGameManager>.Instance.audMan.QueueAudio(MainClass.Instance.assetManager.Get<SoundObject>("Snd_nocalm"));
                Singleton<CoreGameManager>.Instance.audMan.SetLoop(true);
                Singleton<CoreGameManager>.Instance.gameObject.AddComponent<ScoreManager>();
                Singleton<CoreGameManager>.Instance.gameObject.GetComponent<ScoreManager>().scoreMode = ScoreMode.Zen;
                Singleton<BaseGameManager>.Instance.gameObject.AddComponent<PizzatowerAhhTimerManager>();
                Singleton<BaseGameManager>.Instance.gameObject.GetComponent<PizzatowerAhhTimerManager>().SetTime(60*2.5f);
                this.ec.SpawnNPC(MainClass.Instance.BaldiHead, this.ec.AllTilesNoGarbage(false, false)[UnityEngine.Random.Range(0, this.Ec.AllTilesNoGarbage(false, false).Count())].position);
                this.ec.SpawnNPC(MainClass.Instance.PandemoniumW, this.ec.AllTilesNoGarbage(false, false)[UnityEngine.Random.Range(0, this.Ec.AllTilesNoGarbage(false, false).Count())].position);
                this.ec.SpawnNPC(MainClass.Instance.PandemoniumW, this.ec.AllTilesNoGarbage(false, false)[UnityEngine.Random.Range(0, this.Ec.AllTilesNoGarbage(false, false).Count())].position);
                foreach (Notebook nb in Singleton<BaseGameManager>.Instance.Ec.notebooks)
                {
                    if (nb != MainClass.Instance.lastNotebookCollected)
                    {
                        nb.activity.ReInit();
                        Singleton<BaseGameManager>.Instance.AddNotebookTotal(1);
                    }

                }
            }
            
        }

        public override void EndGame(Transform player, Baldi baldi)
        {
            if(Singleton<BaseGameManager>.Instance.gameObject.GetComponent<PizzatowerAhhTimerManager>()) Destroy(Singleton<BaseGameManager>.Instance.gameObject.GetComponent<PizzatowerAhhTimerManager>());
            base.EndGame(player, baldi);
        }

        public override void Initialize()
        {
            if (Singleton<BaseGameManager>.Instance.gameObject.GetComponent<PizzatowerAhhTimerManager>()) Destroy(Singleton<BaseGameManager>.Instance.gameObject.GetComponent<PizzatowerAhhTimerManager>());
            allnotebooktriggered = false;
            base.Initialize();
        }
        public override void LoadNextLevel()
        {
            AudioListener.pause = true;
            Time.timeScale = 0f;
            Singleton<CoreGameManager>.Instance.disablePause = true;
            Singleton<CoreGameManager>.Instance.ReturnToMenu();
        }
        IEnumerator Timer()
        {
            var timer = MTM101BaldAPI.UI.UIHelpers.CreateText<TextMeshProUGUI>(MTM101BaldAPI.UI.BaldiFonts.ComicSans18, "time left: N/A", Singleton<CoreGameManager>.Instance.GetHud(0).Canvas().transform, Vector3.zero);
            timer.rectTransform.anchorMax = new Vector2(0.5f, 0.75f);
            timer.rectTransform.anchorMin = new Vector2(0.5f, 0.75f);
            timer.color = Color.red;
            while (true) {
                yield return new WaitForSeconds(1);
                timeleft -= 1;
                timer.text = "time left: " + timeleft.ToString();
                if (timeleft < 0) {
                    var playerpos = Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position;
                    this.ec.SpawnNPC(MainClass.Instance.BaldiHead, new IntVector2(Mathf.RoundToInt(playerpos.x), Mathf.RoundToInt(playerpos.z)));

                }
            }
            
        }
    }
}
