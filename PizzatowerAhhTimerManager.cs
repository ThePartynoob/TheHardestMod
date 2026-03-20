using HarmonyLib;
using MTM101BaldAPI.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Collections;
using TheHardestMod.Others;
using TheHardestMod.Npcs;

namespace TheHardestMod
{

    public class PizzatowerAhhTimerManager : MonoBehaviour
    {
        float baldiAngered = 0f;
        float timeToAdd = 0f;
        float timeBeforeUpdate = 1.5f;
	   public float time = 10;
       float timeelapes = 0f;
       public float TimeBetweenPoint = 1f;
        public int scoreToRemove = 2;
        public UnityEngine.UI.Image ImageLap;
       bool isredded = false;
       List<Cell> toUpdate = new List<Cell>();
       TextMeshProUGUI Timer;
       void Start() {
            Timer = MTM101BaldAPI.UI.UIHelpers.CreateText<TextMeshProUGUI>(MTM101BaldAPI.UI.BaldiFonts.ComicSans36, (Mathf.Round(time*10)/10).ToString(),Singleton<CoreGameManager>.Instance.GetHud(0).Canvas().transform,Vector3.zero);
            MainClass.Instance.CurrentTimerText = Timer;
            Timer.color = Color.red;
            Timer.horizontalAlignment = HorizontalAlignmentOptions.Center;
            Timer.rectTransform.anchorMax = new Vector2(0.5f,0.15f);
            Timer.rectTransform.anchorMin = new Vector2(0.5f,0.15f);
            ImageLap = MTM101BaldAPI.UI.UIHelpers.CreateImage(MainClass.Instance.Itsbalditime, Singleton<CoreGameManager>.Instance.GetHud(0).Canvas().transform, Vector3.zero,true);
            ImageLap.rectTransform.anchorMax = new Vector2(0.5f, 0.75f);
            ImageLap.rectTransform.anchorMin = new Vector2(0.5f, 0.75f);
            ImageLap.color = new Color(0, 0, 0, 0);
            ImageLap.rectTransform.localScale = new Vector3(0.6f, 0.328f);
        }
       void Update() {
            time -= Time.deltaTime;
            timeelapes += Time.deltaTime;
            timeBeforeUpdate -= Time.deltaTime;
            Timer.text = (Mathf.Round(time*10)/10).ToString();
            if (Singleton<BaseGameManager>.Instance is MainGameManager)
            {
                if (time <= 0)
                {
                    Singleton<BaseGameManager>.Instance.AngerBaldi(0.05f);
                    baldiAngered += 0.05f;
                    var cells = Singleton<BaseGameManager>.Instance.Ec.AllTilesNoGarbage(false, false);

                    Singleton<BaseGameManager>.Instance.Ec.standardDarkLevel = new Color(1f, 0f, 0f);



                }
                else
                {
                    Singleton<BaseGameManager>.Instance.AngerBaldi(-baldiAngered);
                    baldiAngered = 0;

                }
            }

            if (Singleton<BaseGameManager>.Instance is CusChallengeGameManager)
            {
                if (time <= 0)
                {
                    var e = Singleton<CusChallengeGameManager>.Instance.Ec.SpawnNPC(MainClass.Instance.BaldiHead, Singleton<CusChallengeGameManager>.Instance.Ec.AllTilesNoGarbage(false, false)[UnityEngine.Random.Range(0, Singleton<CusChallengeGameManager>.Instance.Ec.AllTilesNoGarbage(false, false).Count())].position);
                    
                    Singleton<BaseGameManager>.Instance.Ec.standardDarkLevel = new Color(1f, 0f, 0f);



                }
                else
                {
                   

                }
            }
            if (timeelapes >= TimeBetweenPoint) {
                
                Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>().AddScore(-scoreToRemove,true);
                timeelapes = 0;
            }
            if (timeBeforeUpdate <= 0) {
                
                    

            }

            if (timeToAdd >= 0 ) {
                timeToAdd -= 0.2f;
                time += 0.2f;
            }
       }
       public void AddTime(float timeAdd) {
            timeToAdd += timeAdd;
       }




       public void SetTime(float SetTime) {
            time = SetTime;
       }

        public void ShowLapSpr(Sprite spr)
        {
           StartCoroutine(Showlap(spr));
        }

        

        private IEnumerator Showlap(Sprite spr)
        {
            ImageLap.sprite = spr;
            ImageLap.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(5f);
            ImageLap.color = new Color(1, 1, 1, 0);

        }

    }
}