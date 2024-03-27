using System.Runtime.CompilerServices;
using HarmonyLib;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Game))]
public class GamePatch {

    public static Game instance;

    [HarmonyPrefix]
    [HarmonyPatch("boot")]
    private static void SetTextPrefix(Game __instance) {
        GamePatch.instance = __instance;
    }

}