using data;
using System.Linq;
using HarmonyLib;
using play.day;

namespace psp_papers_mod.Patches;

public static class CustomPapers {
    public static string PassedJuicerCheck = "JuicerCheckPassed";
    public static string[] FailedJuicerCheck = ["JuicerCheckFailed1", "JuicerCheckFailed2", "JuicerCheckFailed3"];
}

[HarmonyPatch(typeof(BoothEnv))]
public class BoothEnvPatch {

    internal static BoothEnv BoothEnv;

    private static readonly string[] BlockPaperIDs = ["EntryPermit", "WorkPermit", "IdSupplement", "VaccineCert", "IdCard", "DiplomaticAuth"];

    [HarmonyPostfix]
    [HarmonyPatch("__hx_ctor_play_day_BoothEnv", typeof(BoothEnv), typeof(BoothEnvRun), typeof(Day))]
    static void CtorPostfix(object[] __args) {
        BoothEnvPatch.BoothEnv = (BoothEnv)__args[0];
    }

    [HarmonyPrefix]
    [HarmonyPatch("addPaper", typeof(string))]
    static bool AddPaperPrefix(ref string paperId) {
        bool moddedPaper = paperId.StartsWith("modded-");
        if (moddedPaper) paperId = paperId.Replace("modded-", "");

        bool allowed = !BlockPaperIDs.Contains(paperId) || moddedPaper;
        PapersPSP.Log.LogDebug("Paper ID Given: " + paperId + " Modded: " + (moddedPaper ? "Yes" : "No") + " Blocked: " + (!allowed ? "Yes" : "No"));
        return allowed;
    }

    [HarmonyPrefix]
    [HarmonyPatch("applyOp")]
    static bool ChangeTranscriptOps(ref Op op, ref ActionResult __result) {
        Op_SAY sayOp = op.TryCast<Op_SAY>();
        if (sayOp == null) return true;
        if (!sayOp.speechId.StartsWith("__override__")) {
            __result = ActionResult.NONE;
            return false;
        }

        string text = sayOp.speechId["__override__".Length..];
        if (text.Length > 70) {
            text = text[..67] + "...";
        }

        sayOp.speechId = text;
        return true;
    }

    public static void AddPaper(string paperId, int ct = 1) {
        if (ct < 1) return;
        for (int i = 0; i < ct; i++) BoothEnv.addPaper($"modded-{paperId}");
    }

}