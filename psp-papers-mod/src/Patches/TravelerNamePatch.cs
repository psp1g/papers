using data;
using HarmonyLib;
using Il2CppSystem;
using play.day;
using play.day.booth;
using play.day.border;
using psp_papers_mod.Twitch;
using psp_papers_mod.MonoBehaviour;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(TravelerName))]
public class TravelerNamePatch {

    private static int i;

    [HarmonyPrefix]
    [HarmonyPatch("randomize")]
    private static bool RandomizePrefix(TravelerName __instance, ref TravelerName __result) {
        PapersPSP.Twitch.FrequentChatters.CheckChatExpiry();
        Chatter chatter = PapersPSP.Twitch.FrequentChatters.SelectRandomActiveChatter();

        // Allow detaining anytime
        BorderPatch.Border.booth.detainButton.set_dropped(true);

        // Give weapon keys
        if (i++ == 0) {
            BorderPatch.Border.killRifleButton.set_numBullets(999);
            BorderPatch.Border.killRifleButton.set_state(State.DOCKED);
            BorderPatch.Border.tranqRifleButton.set_numBullets(999);
            BorderPatch.Border.tranqRifleButton.set_state(State.DOCKED);

            // new KeyDesk___hx_ctor_play_day_booth_KeyDesk_143__Fun(false, BorderPatch.Border.booth.keyDesk)
            //     .__hx_invoke0_o();

            BorderPatch.Border.sendRunner();
            new Border_set_snipingEnabled_718__Fun(true, BorderPatch.Border).__hx_invoke0_o();

            BoothEnvPatch.AddPaper(BorderPatch.Border.killRifleButton.keyDeskItemId);
            BoothEnvPatch.AddPaper(BorderPatch.Border.tranqRifleButton.keyDeskItemId);
        }

        // if (i > 1) {
        //     BorderPatch.Border.sendRunner();
        // }

        if (chatter == null) return true;

        chatter.JuicerCheck()
            .SuccessWithUnityThread(() => {
                BoothEnvPatch.AddPaper(
                    !chatter.Juicer ?
                        CustomPapers.PassedJuicerCheck :
                        CustomPapers.FailedJuicerCheck.Random()
                );
            });

        PapersPSP.Log.LogInfo($"Selecting user: ${chatter.First} ${chatter.Last}");

        __result = new TravelerName(__instance.nameCycler, __instance.male, chatter.First, chatter.Last);
        return false;
    }

    internal static void Reset() {
        i = 0;
    }

}