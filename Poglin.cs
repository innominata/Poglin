using BepInEx.Configuration;
using HarmonyLib;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Poglin
{
    public class Poglin
    {
        public static string Version;
        public static GameObject bombCountGO;
        public static TextMeshProUGUI bombCount;
        public static ConfigEntry<bool> textReplace;
        public static ConfigEntry<float> speed;
        public static ConfigEntry<bool> bombCounterEnabled;
        [HarmonyPostfix, HarmonyPatch(typeof(LocalizationManager), "GetTranslation")]
        public static void GetTranslationPostfix(ref string __result)
        {
            if (!textReplace.Value) return;
            if (__result != null && textReplace.Value) __result = __result.Replace("eglin", "oglin");
            if (__result != null && textReplace.Value) __result = __result.Replace("aglin", "oglin");
            if (__result != null && textReplace.Value) __result = __result.Replace("=poglin", "=peglin");
        }
        [HarmonyPostfix, HarmonyPatch(typeof(TimescaleManager), "Update")]
        public static void UpdatePostfix(TimescaleManager __instance)
        {
            if (Time.timeScale == 1) Time.timeScale = speed.Value;
        }
        [HarmonyPostfix, HarmonyPatch(typeof(BattleController), "Update")]
        public static void BattleControllerUpdatePostfix(BattleController __instance)
        {
            if (!bombCounterEnabled.Value) return;
            if (bombCountGO == null || bombCount == null)
            {
                CreateBombCountGameObject();
            }
            if (__instance._bombCount > 0)
            {
                // Bootstrap.Debug(__instance._bombCount);
                bombCount.text = $"Bombs:{__instance._bombCount}";
            }
            else
            {
                bombCount.text = "Bombs:----";
            }
        }
        public static void CreateBombCountGameObject()
        {
            if (!bombCounterEnabled.Value) return;
            var g = GameObject.Find("OrbDetails/Garbage/DiscardCountText");
            bombCountGO = GameObject.Instantiate(g, g.transform.parent, true);
            var position = g.transform.localPosition;
            bombCountGO.transform.localPosition = new Vector3(position.x -5, position.y,
                position.z);
            bombCount = bombCountGO.GetComponent<TextMeshProUGUI>();
            bombCount.autoSizeTextContainer = true;
            // bombCount.enableAutoSizing = true;
            bombCount.alignment = TextAlignmentOptions.BottomLeft;
            bombCount.text = "Bombs:----";
        }
    }
}