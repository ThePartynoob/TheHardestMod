using UnityEngine;
using UnityEngine.UI;
using MTM101BaldAPI;
using TMPro;
namespace TheHardestMod
{
    public class HarbingerMinigame : MonoBehaviour
    {
        
        float timeLeft = 10f;
        float BlockTimer = 50f;
        float totalTime = 0f;
        float shakeStrenght = 0f;
        float WaitBeforeBang = 3f;
        bool pushingUseButton = false;
        public bool done = false;
        public bool failure = false; // IF U FAIL UR THE BIGGEST NOOB EVER anyway
        int numberButtonToPress = UnityEngine.Random.Range(0, 5);
        TextMeshProUGUI TMPUGUI;

        private void Start()
        {
            timeLeft = 600;
            Singleton<BaseGameManager>.Instance.Ec.GetBaldi().Praise(timeLeft + 5,false);
            TMPUGUI = MTM101BaldAPI.UI.UIHelpers.CreateText<TextMeshProUGUI>(MTM101BaldAPI.UI.BaldiFonts.ComicSans24, (Mathf.Round(BlockTimer*10)/10).ToString() + " - press use button to block it",Singleton<CoreGameManager>.Instance.GetHud(0).Canvas().transform,Vector3.zero);
            TMPUGUI.color = Color.white;



        }


        public void Stop()
        {
            // Reset camera position and destroy the objects
            Singleton<CoreGameManager>.Instance.GetCamera(0).offestPos = Vector3.zero;
            Destroy(TMPUGUI.gameObject);
            Destroy(this);
        }

        private void Update()
        {
            // Get mouse position from Input.mousePosition
            Singleton<CoreGameManager>.Instance.GetCamera(0).offestPos = new Vector3(UnityEngine.Random.Range(-shakeStrenght, shakeStrenght),UnityEngine.Random.Range(-shakeStrenght, shakeStrenght),UnityEngine.Random.Range(-shakeStrenght, shakeStrenght));
            TMPUGUI.text = (Mathf.Round(BlockTimer*10)/10).ToString() + " - press <b><color=red>" + numberButtonToPress.ToString()  + "</color></b> to block it\n <color=red>good luck.</color>";
            timeLeft -= Time.deltaTime;

            if (UnityEngine.Input.GetKey(numberButtonToPress.ToString()) && !pushingUseButton) {
                pushingUseButton = true;
                BlockTimer = Mathf.Clamp(BlockTimer + 0.2f,0,50f);
                numberButtonToPress = UnityEngine.Random.Range(1, 8);
            } else {
                pushingUseButton = false;
            }
            if (totalTime > 3) {
                BlockTimer -= Time.deltaTime;
                WaitBeforeBang -= Time.deltaTime;
                shakeStrenght = Mathf.Clamp(shakeStrenght - Time.deltaTime,0,999);
            }

            if (WaitBeforeBang <= 0) {
                WaitBeforeBang = UnityEngine.Random.Range(0.05f, 1.1f);
                shakeStrenght += 0.25f;
                BlockTimer -= UnityEngine.Random.Range(3f, 4f);
                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(MainClass.Instance.Snd_Bang_Locker);
            }

            if (timeLeft < 0) {
                done = true;
            }
            if (BlockTimer < 0 ) {
                failure = true;
            }

            totalTime += Time.deltaTime;

        }

    }
}
