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
        Console.Out.WriteLine(message + __result.penaltyCost + __result.type.ToString());
        return false; // causes an error
    }

}