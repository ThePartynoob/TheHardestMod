using MTM101BaldAPI.OptionsAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MTM101BaldAPI;
using MTM101BaldAPI.Reflection;

using TheHardestMod.ObjectExtensions;

namespace TheHardestMod.Others
{
    internal class ChallengesMenu : CustomOptionsCategory

    {
        public override void Build()
        {


            Vector3 originVec = new Vector3(35f, 75f, 0f);
            // broken
            //CreateText("sorry", "Coming back soon :)", originVec, MTM101BaldAPI.UI.BaldiFonts.ComicSans18, TextAlignmentOptions.Center, new Vector2(250, 30), Color.black,false);
            CreateTextButton(() =>
            {


                Debug.Log("idk but it clicked");
                var e = FindObjectOfType<OptionsMenu>();
                e.gameObject.SetActive(false);
                MainClass.Instance.GL.useSeed = false;
                MainClass.Instance.GL.Initialize(0);

                MainClass.Instance.GL.AssignElevatorScreen(MainClass.Instance.ELS);
                MainClass.Instance.GL.LoadLevel(MainClass.Instance.Challenge1);
                MainClass.Instance.ELS.gameObject.SetActive(true);
                MainClass.Instance.GL.SetMode((int)Mode.Main);



            }
            , "Pressurin", "challenge_1", originVec + new Vector3(0, 0, 0), MTM101BaldAPI.UI.BaldiFonts.ComicSans12, TextAlignmentOptions.Center, new Vector2(250, 10), Color.black);

            CreateTextButton(() =>
            {


                Debug.Log("idk but it clicked");
                var e = FindObjectOfType<OptionsMenu>();
                e.gameObject.SetActive(false);
                MainClass.Instance.GL.useSeed = false;
                MainClass.Instance.GL.Initialize(0);

                MainClass.Instance.GL.AssignElevatorScreen(MainClass.Instance.ELS);
                MainClass.Instance.GL.LoadLevel(MainClass.Instance.Challenge2);
                MainClass.Instance.ELS.gameObject.SetActive(true);
                MainClass.Instance.GL.SetMode((int)Mode.Main);



            }
            , "idking", "challenge_2", originVec + new Vector3(0, -50, 0), MTM101BaldAPI.UI.BaldiFonts.ComicSans12, TextAlignmentOptions.Center, new Vector2(250, 10), Color.black);
        }


    }
}
