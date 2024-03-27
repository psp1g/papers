using app;
using data;
using HarmonyLib;
using play.day;
using psp_papers_mod.Twitch;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(BoothEngine))]
public class BoothEnginePatch {

    private static BoothEngine instance;
    private static StampApprovalKind lastStamp = null;

    [HarmonyPostfix]
    [HarmonyPatch("start")]
    private static void StartPostfix(BoothEngine __instance) {
        __instance.speak("POOP LOL XD", true);
        BoothEnginePatch.instance = __instance;
        PapersPSP.Log.LogInfo($"CONSTRUCTOR");
    }

    [HarmonyPrefix]
    [HarmonyPatch("handleEvent", typeof(EngineEvent))]
    private static void HandleEventPrefix(EngineEvent @event) {
        if (@event == EngineEvent.PROCESSING_FINISH) {
            if (TwitchIntegration.ActiveChatter == null) {
                lastStamp = null;
                return;
            }

            if (lastStamp == StampApprovalKind.DENIED)
                TwitchIntegration.ActiveChatter.Deny(TwitchIntegration.ACTIVE_CHATTER_DENY_TIMEOUT_SECONDS);

            lastStamp = null;
        }
    }

    /*
     * Prevent the execution of the speak function only if:
     *  - The mod is enabled
     *  - Not overriden
     *  - The Traveler is speaking (not the player/inspector)
     *
     * Return true if `speak` should run
     */
    [HarmonyPrefix]
    [HarmonyPatch("speak", typeof(string), typeof(bool))]
    private static bool SpeakPrefix(ref string text, bool fromInspector) {
        PapersPSP.Log.LogInfo($"Character Speaking: \"{text}\" Inspector: {fromInspector}");

        if (!PapersPSP.Enabled) return true;

        if (text.StartsWith("__override__")) {
            text = text.Replace("__override__", "");
            return true;
        }

        return fromInspector;
    }

    public static void Speak(string text, bool inspector = false) {
        BoothEnginePatch.instance.speak($"__override__{text}", inspector);
    }

    // Timeout active user in chat on denied stamp
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BoothEngine), "stampPaper", typeof(string), typeof(StampApprovalKind))]
    private static void StampPrefix(string idWithIndex, StampApprovalKind approvalType) {
        PapersPSP.Log.LogInfo($"STAMP {approvalType.toString()}");
        if (approvalType != StampApprovalKind.DENIED || TwitchIntegration.ActiveChatter == null) return;
        TwitchIntegration.ActiveChatter.Deny(TwitchIntegration.ACTIVE_CHATTER_DENY_TIMEOUT_SECONDS);
    }

    public static void Stamp(StampApprovalKind kind) {

    }

}