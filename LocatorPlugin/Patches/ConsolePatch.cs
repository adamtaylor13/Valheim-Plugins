using HarmonyLib;
using Purps.Valheim.Framework;

namespace Purps.Valheim.Locator.Patches {
    [HarmonyPatch(typeof(Console), "InputText")]
    public class ConsolePatch {
        [HarmonyPostfix]
        internal static void Postfix(Console __instance) {
            var commandStr = __instance.m_input.text;
            if (!string.IsNullOrWhiteSpace(commandStr)) BasePlugin.ExecuteCommand(__instance.m_input.text);
        }
    }
}