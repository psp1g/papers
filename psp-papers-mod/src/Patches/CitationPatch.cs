using data;
using HarmonyLib;
using Il2CppSystem;
using play.day;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Day))]
public class CitationPatch {

    [HarmonyPrefix]
    [HarmonyPatch("addCitation", typeof(string))]
    private static bool AddCitationPostfix(string message, Citation __result) {
        // Prevent any Citations, makes endless mode truly endless, and you can't lose
        // This causes an error, but doesn't seem to break anything (other than citations, like intended)
        return false;
    }

}