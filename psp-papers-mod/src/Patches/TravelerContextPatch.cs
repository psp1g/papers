using data;
using HarmonyLib;
using play.day;
using psp_papers_mod.Utils;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(TravelerContext))]
public class TravelerContextPatch {
    
    [HarmonyPostfix]
    [HarmonyPatch(nameof(TravelerContext.makeTravelerSpec))]
    public static void FixInitialPapers(ref TravelerSpec __result) {
        __result.initialPaperIds.add("JuicerCheck".ToIl2Cpp());
    }
    
}