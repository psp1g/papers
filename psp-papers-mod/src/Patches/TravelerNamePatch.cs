using data;
using HarmonyLib;
using Il2CppSystem;
using psp_papers_mod.MonoBehaviour;
using psp_papers_mod.Twitch;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(TravelerName))]
public class TravelerNamePatch {

    private static int i = 0;

    [HarmonyPrefix]
    [HarmonyPatch("randomize")]
    private static bool RandomizePrefix(TravelerName __instance, ref TravelerName __result) {
        PapersPSP.Twitch.FrequentChatters.CheckChatExpiry();
        Chatter chatter = PapersPSP.Twitch.FrequentChatters.SelectRandomActiveChatter();

        // Allow detaining anytime
        BorderPatch.Border.booth.detainButton.set_dropped(true);

        // Give weapon keys
        if (i++ == 0) {
            // BorderPatch.Border.set_snipingEnabled(true);
            // BorderPatch.Border.killRifleButton.set_active(true);
            // BorderPatch.Border.tranqRifleButton.set_active(true);
            BoothEnvPatch.AddPaper(BorderPatch.Border.killRifleButton.keyDeskItemId);
            BoothEnvPatch.AddPaper(BorderPatch.Border.tranqRifleButton.keyDeskItemId);
        }

        // if (i > 1) {
        //     BorderPatch.Border.sendRunner();
        // }

        if (chatter == null) return true;

        chatter.JuicerCheck()
            .SuccessWithUnityThread(() => {
                // todo; Add correct juicer ticket
                Console.Out.WriteLine("XD IS JUICER : " + chatter.Juicer);
                BoothEnvPatch.AddPaper(CustomPapers.PassedJuicerCheck);
            });

        PapersPSP.Log.LogInfo($"Selecting user: ${chatter.First} ${chatter.Last}");

        __result = new TravelerName(__instance.nameCycler, __instance.male, chatter.First, chatter.Last);
        return false;
    }

}