using data;
using HarmonyLib;
using psp_papers_mod.Twitch;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(TravelerName))]
public class TravelerNamePatch {

    [HarmonyPrefix]
    [HarmonyPatch("randomize")]
    private static bool RandomizePrefix(TravelerName __instance, ref TravelerName __result) {
        PapersPSP.Twitch.FrequentChatters.CheckChatExpiry();
        Chatter chatter = PapersPSP.Twitch.FrequentChatters.SelectRandomActiveChatter();

        if (chatter == null) return true;
        PapersPSP.Log.LogInfo($"Selecting user: ${chatter.First} ${chatter.Last}");

        __result = new TravelerName(__instance.nameCycler, __instance.male, chatter.First, chatter.Last);
        return false;
    }

}