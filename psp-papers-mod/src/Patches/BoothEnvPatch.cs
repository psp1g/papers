using data;
using System.Linq;
using HarmonyLib;
using Il2CppInterop.Runtime;
using play.day;
using psp_papers_mod.Twitch;
using psp_papers_mod.Utils;
using System;

namespace psp_papers_mod.Patches;

public static class CustomPapers {
    public static string PassedJuicerCheck = "JuicerCheckPassed";
    public static string[] FailedJuicerCheck = ["JuicerCheckFailed1", "JuicerCheckFailed2", "JuicerCheckFailed3"];
}

[HarmonyPatch(typeof(BoothEnv))]
public class BoothEnvPatch {

    internal static BoothEnv BoothEnv;

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
        
        PapersPSP.Log.LogDebug("Paper ID Given: " + paperId + " Modded: " + (moddedPaper ? "Yes" : "No"));
        return true;
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

    [HarmonyPrefix]
    [HarmonyPatch(nameof(BoothEnv.makeFactValue))]
    public static bool MakeModdedFactValues(string path, bool valid, Il2CppSystem.Object confusing, ref FactValue __result) {
        if (!path.Split("/").Last().Equals("JuicerCheckId")) return true;
        string id = "74126";
        if (!valid) {
            char[] chars = id.ToCharArray();
            Random random = new();
            int idx = random.Next(chars.Length);
            char[] digits = "0123456789".ToCharArray();
            chars[idx] = digits[random.Next(digits.Length - 1)];
            if (chars[idx] == id[idx]) {
                chars[idx] = digits[^1];
            }
            id = new string(chars);
        }

        __result = new FactValue(id, null, null);
        return false;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(nameof(BoothEnv.makeNewTraveler))]
    public static void ChooseChatterTraveler() {
        TwitchIntegration.NextActiveChatter();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(BoothEnv.makeNewTraveler))]
    public static void AddJuicerCheck(BoothEnv __instance) {
        Error error = __instance.traveler.modifiedError;
        if (error != null && error.id.Contains("<juicer>")) {
            AddPaper(error.ops[0].Cast<Op_REQUIREMENT>().tokens[0].ToManagedString());
        } else {
            AddPaper("JuicerCheck");
        }
    }

    public static void AddPaper(string paperId, int ct = 1) {
        if (ct < 1) return;
        for (int i = 0; i < ct; i++) BoothEnv.addPaper($"modded-{paperId}");
    }

}