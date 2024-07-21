using data;
using HarmonyLib;
using play.day;
using psp_papers_mod.Twitch;
using psp_papers_mod.Utils;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(BoothEngine))]
public class BoothEnginePatch {

    public static BoothEngine BoothEngine;
    private static StampApprovalKind lastStamp;

    [HarmonyPostfix]
    [HarmonyPatch("start")]
    private static void StartPostfix(BoothEngine __instance) {
        BoothEnginePatch.BoothEngine = __instance;
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
                TwitchIntegration.ActiveChatter.Deny();
            else if (lastStamp == StampApprovalKind.APPROVED)
                TwitchIntegration.ActiveChatter.Approve();

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

        if (text.StartsWith("__override__")) {
            text = text.Replace("__override__", "");
            return true;
        }

        return fromInspector && text == "Papers, please.";
    }

    public static void Speak(string text, bool inspector = false) {
        BoothEnginePatch.BoothEngine.speak($"__override__{text}", inspector);
    }

    // Timeout active user in chat on denied stamp
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BoothEngine), "stampPaper", typeof(string), typeof(StampApprovalKind))]
    private static void StampPrefix(string idWithIndex, StampApprovalKind approvalType) {
        lastStamp = approvalType;
    }

    public static void GivePaperNow(string paperId, int ct = 1) {
        if (ct < 1) return;
        for (int i = 0; i < ct; i++) BoothEngine.debugAddPaper($"{paperId}");
    }

    public static void ForceTravelerLeave() {
        if (BorderPatch.Border.booth.stater.curState.name != "working") return;
        BoothEngine.applyOps(
            Il2CppUtils.HaxeArrayOf(new Op_ENABLEBUTTON("Leave")),
            true.ToIl2CppBoxed()
        );
    }

}