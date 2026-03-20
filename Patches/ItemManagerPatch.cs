using HarmonyLib;


namespace TheHardestMod.Patches
{   [HarmonyPatch]
    public class ItemManagerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ItemManager),"Update")]
        static private void AwakePatch(ItemManager __instance) {
            if (!Singleton<ModifiersCategorySettings>.Instance.b) {
                Singleton<CoreGameManager>.Instance.GetHud(0).UpdateInventorySize(3);
                __instance.maxItem = 2;
            } else {
                Singleton<CoreGameManager>.Instance.GetHud(0).UpdateInventorySize(1);
                __instance.maxItem = 0;
            }
        }

	
    }
}