using System.Linq;
using HarmonyLib;
using Il2CppSystem;
using play.day;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(BoothEnv))]
public class BoothEnvPatch {

    private static readonly string[] BlockPaperIDs = ["EntryPermit"];

    [HarmonyPrefix]
    [HarmonyPatch("addPaper", typeof(string))]
    private static bool AddPaperPrefix(ref string paperId) {
        Console.Out.WriteLine("PAPER ID " + paperId);
        bool moddedPaper = paperId.StartsWith("modded-");
        if (moddedPaper) paperId = paperId.Replace("modded-", "");
        return !BlockPaperIDs.Contains(paperId) || paperId.StartsWith("mod-");
    }

}