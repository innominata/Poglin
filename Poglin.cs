using BepInEx.Configuration;
using Peglin;
using HarmonyLib;
using I2.Loc;
using Mono.CompilerServices.SymbolWriter;
using UnityEngine;

namespace Poglin
{
    public class Poglin
    {
        public static string Version;
        public static ConfigEntry<bool> textReplace;
        public static ConfigEntry<float> speed;
        [HarmonyPostfix, HarmonyPatch(typeof(LocalizationManager), "GetTranslation")]
        public static void GetTranslationPostfix(ref string __result)
        {
            if (__result != null && textReplace.Value) __result = __result.Replace("eglin", "oglin");
        }
        [HarmonyPostfix, HarmonyPatch(typeof(TimescaleManager), "Update")]
        public static void UpdatePostfix(TimescaleManager __instance)
        {
            if (Time.timeScale == 1) Time.timeScale = speed.Value;
        }
    }
}