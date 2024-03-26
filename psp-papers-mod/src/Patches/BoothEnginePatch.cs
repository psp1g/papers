using System;
using data;
using HarmonyLib;
using play.day;
using psp_papers_mod.Twitch;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(BoothEngine))]
public class BoothEnginePatch {

    /*
     * Prevent the execution of the speak function only if:
     *  - The mod is enabled
     *  - The Traveler is speaking (not the player/inspector)
     *
     * Return true if `speak` should run
     */
    [HarmonyPrefix]
    [HarmonyPatch("speak", typeof(string), typeof(bool))]
    private static bool SpeakPrefix(string text, bool fromInspector) {
        PapersPSP.Log.LogInfo($"HI {text} {fromInspector}");
        return fromInspector || !PapersPSP.Enabled;
    }

    // original speak method
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(BoothEngine), "speak")]
    public static void SpeakReversePatch(object instance, string text, bool fromInspector) {
        throw new NotImplementedException("stub");
    }

    public static void Speak(string text, bool inspector = false) {

    }

    // Timeout active user in chat on denied stamp
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BoothEngine), "stampPaper", typeof(string), typeof(StampApprovalKind))]
    private static void StampPrefix(string idWithIndex, StampApprovalKind approvalKind) {
        if (approvalKind != StampApprovalKind.DENIED || TwitchIntegration.ActiveChatter == null) return;
        TwitchIntegration.ActiveChatter.Deny(TwitchIntegration.ACTIVE_CHATTER_DENY_TIMEOUT_SECONDS);
    }

    // original stamp method
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(BoothEngine), "stampPaper")]
    public static void Stamp(object instance, int idWithIndex, StampApprovalKind kind) {
        throw new NotImplementedException("stub");
    }

    public static void Stamp(StampApprovalKind kind) {

    }

}