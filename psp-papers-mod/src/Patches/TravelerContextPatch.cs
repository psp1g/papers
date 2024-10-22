using HarmonyLib;
using data;
using play.day;
using psp_papers_mod.Twitch;
using System;


[HarmonyPatch]
public static class TravelerContextPatch {
    // First call of makeTravelerSpec generates the traveler's nationality. Any calls after that are used to find a
    // value for an invalid issuing city or similar.
    private static bool isSpecCall;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(TravelerContext), nameof(TravelerContext.makeTravelerSpec))]
    private static void MarkSpecCall() {
        Console.WriteLine("TravelerContext.makeTravelerSpec called");
        isSpecCall = true;
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(FactLib), nameof(FactLib.getRandomNationality))]
    private static void FixCountry(ref string __result) {
        if (TwitchIntegration.ActiveChatter == null || !isSpecCall)
            return;
        Console.WriteLine("Fixing country");
        isSpecCall = false;
        string country = TwitchIntegration.ActiveChatter.GetLocalization();
        if (country == null) return;
        
        __result = country;
    }
}

