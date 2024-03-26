using data;
using HarmonyLib;
using psp_papers_mod.Twitch;

namespace psp_papers_mod.Patches {

    [HarmonyPatch(typeof(TravelerSpec))]
    public class TravelerSpecPatch {

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TravelerSpec), "get_name")]
        public static bool Prefix(ref string __result) {

            PapersPSP.Twitch.FrequentChatters.CheckChatExpiry();
            Chatter chatter = PapersPSP.Twitch.FrequentChatters.SelectRandomActiveChatter();
            PapersPSP.Log.LogInfo($"TRAVELLER !!!!");

            // If there's no chatter selected, proceed with the normal execution
            if (chatter == null) return true;
            PapersPSP.Log.LogInfo($" NAMEEE ${chatter.First} ${chatter.Last}");

            __result = chatter.Username;

            // Skip execution of the normal function
            return false;
        }

    }

}