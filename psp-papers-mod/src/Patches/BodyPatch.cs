using data;
using HarmonyLib;
using play.day;
using play.day.booth;
using psp_papers_mod.Twitch;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Body))]
public class BodyPatch {

    [HarmonyPrefix]
    [HarmonyPatch("setTraveler", typeof(Traveler))]
    private static void HandleEventPrefix(ref Traveler traveler) {
        PapersPSP.Twitch.FrequentChatters.CheckChatExpiry();
        Chatter chatter = PapersPSP.Twitch.FrequentChatters.SelectRandomActiveChatter();
        PapersPSP.Log.LogInfo($"TRAVELLER !!!!");

        if (chatter == null) return;
        PapersPSP.Log.LogInfo($" NAMEEE ${chatter.First} ${chatter.Last}");

        traveler.name.first = chatter.First;
        traveler.name.last = chatter.Last;
    }

}