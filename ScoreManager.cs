using HarmonyLib;
using MTM101BaldAPI.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TheHardestMod
{
    public enum ScoreMode
    {
        Normal,
        Zen
    }
    public class ScoreManager : MonoBehaviour
    {
	   public float multiplier = 1f;
       public float CurrentScore = 100f;
        public float displayScore = 0;
       public float OldScore = 100f;
        public float multiplierAdditive = 0f;
        public float currentMultiplier = 1f;
        private float nextrankPoints = 0;
       private float TimeElapsed = 0f;
        internal ScoreMode scoreMode = ScoreMode.Normal;
       public TextMeshProUGUI CurrentScoreText;
       public void AddScore(float score, bool unaffectedMultiplier = false, bool Playsound = false, string reason = "") {
            var CalculatedScore = 0f;
            if (!unaffectedMultiplier) CurrentScore += score * currentMultiplier;
            else CurrentScore += score;
            if (!unaffectedMultiplier) CalculatedScore += score * currentMultiplier;
            else CalculatedScore += score;
            if (Playsound) {
                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(CalculatedScore >= 0 ? MainClass.Instance.Snd_PlusPoint : MainClass.Instance.Snd_MinusPoint);
            }
            CalculatedScore = Mathf.Round(CalculatedScore);
            var addedScoreText = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans36, CalculatedScore > 0 ? "<color=green>+" + CalculatedScore.ToString() + "</color>" : CalculatedScore < 0 ? "<color=red>" + CalculatedScore.ToString() + "</color>" : "<color=gray>" + CalculatedScore.ToString() + "</color>",Singleton<CoreGameManager>.Instance.GetHud(0).Canvas().transform,Vector3.zero);
            addedScoreText.rectTransform.anchorMax = new Vector2(UnityEngine.Random.Range(0.25f,0.35f),0.75f);
            addedScoreText.rectTransform.anchorMin = new Vector2(UnityEngine.Random.Range(0.25f,0.35f),0.75f);
            addedScoreText.enableWordWrapping = false;
            var addedScorereason = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans18, CalculatedScore > 0 ? "<color=green>+" + reason + "</color>" : CalculatedScore < 0 ? "<color=red>" + reason + "</color>" : "<color=#7d7d7d>" + reason + "</color>",Singleton<CoreGameManager>.Instance.GetHud(0).Canvas().transform,Vector3.zero);
            addedScorereason.rectTransform.anchorMax = new Vector2(UnityEngine.Random.Range(0.25f,0.35f),0.6f);
            addedScorereason.rectTransform.anchorMin = new Vector2(UnityEngine.Random.Range(0.25f,0.35f),0.6f);
            addedScorereason.enableWordWrapping = false;
            
            StartCoroutine(AnimText(addedScoreText, addedScorereason));

       }
       IEnumerator AnimText(TextMeshProUGUI text, TextMeshProUGUI reason)
        {
            var speed = 1.5f;
            
            
            for (float i = 0; i < 70; i+=1) {
                
                text.rectTransform.anchoredPosition += new Vector2(0,speed);
                reason.rectTransform.anchoredPosition += new Vector2(0,speed);
                speed -= 0.1f;
                if (i>50) { 
                text.alpha -= 0.05f;
                reason.alpha -= 0.05f;
            }
                yield return new WaitForSeconds(0.001f);
            }
            Destroy(text);
            Destroy(reason);

        }
       public string GetGrade(float points)
        {
            if (scoreMode == ScoreMode.Normal)
            {
                if (points < 1000)
                {
                    nextrankPoints = 1000;
                    return "<color=#ff0000>F</color>";
                }
                if (points >= 1000 && points < 4000)
                {
                    nextrankPoints = 4000;
                    return "<color=#ff4d00>C</color>";
                }
                if (points >= 4000 && points < 8500)
                {
                    nextrankPoints = 8500;
                    return "<color=#ffcc00>B</color>";
                }
                if (points >= 8500 && points < 10000)
                {
                    nextrankPoints = 10000;
                    return "<color=#c3ff00>A</color>";
                }
                if (points >= 10000 && points < 13500)
                {
                    nextrankPoints = 13500;
                    return "<color=#c3ff00>S</color>";
                }
                if (points >= 13500)
                {
                    string grade = "<color=#c3ff00>S</color>";
                    int totalSteps = Mathf.FloorToInt((points - 13500) / 1500f);

                    for (int i = 0; i < totalSteps; i++)
                    {
                        if (i > 0) grade += "<color=#6b03fc>+</color>";
                    }

                    // CALCULATION FOR NEXT RANK
                    // Since your loop needs i to be at least 1 to add the first '+', 
                    // the first upgrade (S+) happens at 13500 + (2 * 1500) = 16500.
                    if (totalSteps < 1)
                    {
                        // If they are in the base "S" zone, they need to reach the 2nd step to get a "+"
                        nextrankPoints = 13500 + (2 * 1500);
                    }
                    else
                    {
                        // Once they have pluses, the next one is just the current step + 1
                        nextrankPoints = 13500 + ((totalSteps + 1) * 1500);
                    }
                    return $"<color=#a200ff>{grade}</color>";
                }
            }
            else if (scoreMode == ScoreMode.Zen)
            {
                if (points < 250) { nextrankPoints = 250; return "<color=#ff0000>F</color>"; }
                if (points < 1000) { nextrankPoints = 1000; return "<color=#ff4d00>C</color>"; }
                if (points < 3000) { nextrankPoints = 3000; return "<color=#ffcc00>B</color>"; }
                if (points < 6000) { nextrankPoints = 6000; return "<color=#c3ff00>A</color>"; }
                if (points < 9000) { nextrankPoints = 9000; return "<color=#c3ff00>S</color>"; }

                // --- EXPONENTIAL S+ LOGIC ---
                float baseZenS = 9000f;
                float initialStep = 2000f;

                // We use Log2 to figure out how many "doublings" fit into the current score
                // n = log2((points - base) / initialStep + 1)
                float progress = (points - baseZenS) / initialStep;
                int totalSteps = Mathf.FloorToInt(Mathf.Log(progress + 1, 2));

                // Safety check for negative log results
                if (totalSteps < 0) totalSteps = 0;

                string grade = "<color=#c3ff00>S</color>";
                for (int i = 0; i < totalSteps; i++)
                {
                    grade += "<color=#6b03fc>+</color>";
                }

                // Calculate next rank: The points required for (totalSteps + 1) pluses
                // Formula: Base + InitialStep * (2^(n+1) - 1)
                nextrankPoints = baseZenS + initialStep * (Mathf.Pow(2, totalSteps + 1) - 1);

                return grade;
            }
            return "ASS";
        }
       void Awake() {
            CurrentScoreText = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans24,"score: null",Singleton<CoreGameManager>.Instance.GetHud(0).Canvas().transform,Vector3.zero) ;
            CurrentScoreText.rectTransform.anchorMax = new Vector2(0.3f,0.8f);
            CurrentScoreText.rectTransform.anchorMin = new Vector2(0.3f,0.8f);
            CurrentScoreText.color = Color.magenta;
            CurrentScoreText.horizontalAlignment = HorizontalAlignmentOptions.Center;
            CurrentScoreText.enableWordWrapping = false;

       }
        private string GetNextGradeLetter(float nextPoints)
        {
            if (scoreMode == ScoreMode.Normal)
            {
                if (nextPoints <= 1000) return "<color=#ff4d00>C</color>";
                if (nextPoints <= 4000) return "<color=#ffcc00>B</color>";
                if (nextPoints <= 8500) return "<color=#c3ff00>A</color>";
                if (nextPoints <= 10000) return "<color=#c3ff00>S</color>";

                // For S+ ranks, we use your loop logic on the future points
                int nextSteps = Mathf.FloorToInt((nextPoints - 13500) / 1500f);
                string nGrade = "<color=#c3ff00>S</color>";
                for (int i = 0; i < nextSteps; i++)
                {
                    if (i > 0) nGrade += "<color=#6b03fc>+</color>";
                }
                return nGrade;
            }
            else if (scoreMode == ScoreMode.Zen)
            {
                if (nextPoints <= 250) return "<color=#ff4d00>C</color>";
                if (nextPoints <= 1000) return "<color=#ffcc00>B</color>";
                if (nextPoints <= 3000) return "<color=#c3ff00>A</color>";
                if (nextPoints <= 6000) return "<color=#c3ff00>S</color>";
                // This handles the gap between Rank A and the first S+
                if (nextPoints <= 9000) return "<color=#c3ff00>S</color>";

                // --- EXPONENTIAL S+ LOGIC ---
                // Calculate how many "doublings" fit into the future points
                float progress = (nextPoints - 9000) / 2000f;
                // Mathf.Max(0, ...) prevents math errors if points are exactly 9000
                int nextSteps = Mathf.FloorToInt(Mathf.Log(Mathf.Max(0, progress) + 1, 2));

                string nGrade = "<color=#c3ff00>S</color>";
                for (int i = 0; i < nextSteps; i++)
                {
                    nGrade += "<color=#6b03fc>+</color>";
                }
                return nGrade;
            }
            return "not a grade.";
            }
        void Update() {
            currentMultiplier = multiplier + multiplierAdditive;
            multiplierAdditive = Mathf.Lerp(multiplierAdditive, 0, 0.002f);
            TimeElapsed += Time.deltaTime;
            CurrentScoreText.text = "Score: " +Mathf.Round(displayScore).ToString() + "(X" + (Mathf.Round(currentMultiplier*100)/100).ToString() + $") - Rank: {GetGrade(CurrentScore)} \nNext rank: {GetNextGradeLetter(nextrankPoints)} in {Mathf.Round(nextrankPoints - displayScore)}";
            CurrentScoreText.rectTransform.anchoredPosition = new Vector2(CurrentScoreText.rectTransform.anchoredPosition.x,Mathf.Sin(TimeElapsed / 3) *20);
            displayScore = Mathf.Lerp(displayScore, CurrentScore, 0.05f);
       }
    }
}