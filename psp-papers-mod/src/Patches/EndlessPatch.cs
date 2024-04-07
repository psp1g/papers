using HarmonyLib;
using play.day;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Endless))]
public class EndlessPatch {

    // Make it REALLY endless
    [HarmonyPrefix]
    [HarmonyPatch("get_isEnding")]
    private static bool GetIsEndingPrefix(ref bool __result) {
        __result = false;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch("queEnd")]
    private static bool QueEndPrefix() {
        return false;
    }

}