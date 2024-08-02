using data;
using HarmonyLib;
using play.day;
using play.day.border;
using psp_papers_mod.Twitch;
using psp_papers_mod.MonoBehaviour;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(TravelerName))]
public class TravelerNamePatch {

    internal static int TotalTravellers;

    [HarmonyPrefix]
    [HarmonyPatch("randomize")]
    private static bool RandomizePrefix(TravelerName __instance, ref TravelerName __result) {
        AttackHandler.AttackIfPossible();

        AttackHandler.TravelerCtSinceLast++;

        PapersPSP.Twitch.FrequentChatters.CheckChatExpiry();
        Chatter chatter = PapersPSP.Twitch.FrequentChatters.GetRandomChatter();

        if (chatter != null)
            TwitchIntegration.SetActiveChatter(chatter);

        // Allow detaining anytime
        BorderPatch.Border.booth.detainButton.set_dropped(true);

        // Always display the reason (gagaga) stamp 
        BorderPatch.Border.booth.stampBar.set_reasonStampEnabled(true);

        // Enable sniping & give weapon keys
        if (TotalTravellers++ == 0) {
            BorderPatch.Border.day.featureFlags |= 1 << Feature.SNIPING._hx_index;
            BorderPatch.Border.set_snipingEnabled(true);
            BorderPatch.Border.killRifleButton.set_numBullets(999);
            BorderPatch.Border.tranqRifleButton.set_state(State.OFF);

            BoothEnginePatch.GivePaperNow(BorderPatch.Border.killRifleButton.keyDeskItemId);
            //BoothEnginePatch.GivePaperNow(BorderPatch.Border.tranqRifleButton.keyDeskItemId);
        }

        if (chatter == null) return true;

        chatter.JuicerCheck()
            .SuccessWithUnityThread(() => {
                BoothEnvPatch.AddPaper(
                    !chatter.Juicer ? CustomPapers.PassedJuicerCheck : CustomPapers.FailedJuicerCheck.Random()
                );
            });

        PapersPSP.Log.LogInfo($"Selecting user: ${chatter.First} ${chatter.Last}");

        __result = new TravelerName(__instance.nameCycler, __instance.male, chatter.First, chatter.Last);
        return false;
    }

    internal static void Reset() {
        TotalTravellers = 0;
    }
}