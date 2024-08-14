using HarmonyLib;
using psp_papers_mod;
using play.day;
using app;
using data;
using psp_papers_mod.Utils;
using psp_papers_mod.Twitch;
using System.Diagnostics.Metrics;


[HarmonyPatch(typeof(FactLib))]
public static class TravelerPatch {

    [HarmonyPostfix]
    [HarmonyPatch("getRandomNationality", typeof(Rand), typeof(string))]
    static void CtorPostfix(object[] __args, Rand rand, string purpose, ref string __result) {
        //var traveler = (Traveler)__args[0];
        if (TwitchIntegration.ActiveChatter == null)
            return;

        var country = TwitchIntegration.ActiveChatter.GetLocalization();
        if (country == null) return;

        __result = country;
    }
}

