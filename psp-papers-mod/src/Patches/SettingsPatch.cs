using HarmonyLib;
using play;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Settings))]
public class SettingsPatch {

    public static Settings instance;

    [HarmonyPrefix]
    [HarmonyPatch("load")]
    private static void SetTextPrefix(Settings __instance) {
        SettingsPatch.instance = __instance;
        __instance.set_endlessUnlocked(true);
    }

}