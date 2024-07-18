using System.Linq;
using HarmonyLib;
using Il2CppSystem;
using play.day;

namespace psp_papers_mod.Patches;

public static class CustomPapers {
    public static string PassedJuicerCheck = "JuicerCheckPassed";
    public static string[] FailedJuicerCheck = ["JuicerCheckFailed1", "JuicerCheckFailed2", "JuicerCheckFailed3"];
}

[HarmonyPatch(typeof(BoothEnv))]
public class BoothEnvPatch {

    internal static BoothEnv BoothEnv;

    private static readonly string[] BlockPaperIDs = ["EntryPermit", "WorkPermit", "IdSupplement", "IdCard", "DiplomaticAuth"];

    [HarmonyPostfix]
    [HarmonyPatch("__hx_ctor_play_day_BoothEnv", typeof(BoothEnv), typeof(BoothEnvRun), typeof(Day))]
    static void CtorPostfix(object[] __args) {
        BoothEnvPatch.BoothEnv = (BoothEnv)__args[0];
    }

    [HarmonyPrefix]
    [HarmonyPatch("addPaper", typeof(string))]
    static bool AddPaperPrefix(ref string paperId) {
        Console.Out.WriteLine("PAPER ID " + paperId);
        bool moddedPaper = paperId.StartsWith("modded-");
        if (moddedPaper) paperId = paperId.Replace("modded-", "");
        return !BlockPaperIDs.Contains(paperId) || moddedPaper;
    }

    public static void AddPaper(string paperId, int ct = 1) {
        if (ct < 1) return;
        for (int i = 0; i < ct; i++) BoothEnv.addPaper($"modded-{paperId}");
    }

}