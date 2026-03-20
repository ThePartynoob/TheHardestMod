using HarmonyLib;
using MTM101BaldAPI.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace TheHardestMod.Patches
{
    [HarmonyPatch]
    internal class MainMenuPatch
    {
        [HarmonyPatch(typeof(MainMenu), "Start")]
        [HarmonyPostfix]
        static void OnMainMenu(MainMenu __instance)
        {
            if (!MainClass.Instance.GL)
                MainClass.Instance.GL = Resources.FindObjectsOfTypeAll<GameLoader>().First(x => x.GetInstanceID() > 0);

            if (!MainClass.Instance.ELS)
                MainClass.Instance.ELS = Resources.FindObjectsOfTypeAll<ElevatorScreen>().First(x => x.GetInstanceID() > 0 && x.gameObject.scene == __instance.gameObject.scene);

        }

    }
}

