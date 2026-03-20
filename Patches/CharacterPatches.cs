using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using MTM101BaldAPI.Reflection;
namespace TheHardestMod.Patches
{
    [HarmonyPatch]
    internal class CharacterPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Beans), "Initialize")]
        static public void BeansPatch(Beans __instance)
        {
            __instance.ReflectionSetVariable("cooldownTime", 0f);
            __instance.ReflectionSetVariable("chewTime", 0f);

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(FirstPrize), "Initialize")]
        static public void firstprizePatch(FirstPrize __instance)
        {
            __instance.turnSpeed = 720f;
            __instance.wanderSpeed = 40;
            __instance.chaseSpeed = 500;

        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Jumprope), "Start")]
        static public void ptPatch(Jumprope __instance)
        {
            __instance.ReflectionSetVariable("ropeDelay", 0.65f);
            __instance.ReflectionSetVariable("ropeTime", 0.6f);
        }





    }
}
