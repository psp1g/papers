using HarmonyLib;
using app;
using data;
using psp_papers_mod.Twitch;


[HarmonyPatch(typeof(FactLib))]
public static class TravelerPatch {

    [HarmonyPostfix]
    [HarmonyPatch("getRandomNationality", typeof(Rand), typeof(string))]
    static void CtorPostfix(ref string __result) {
        if (TwitchIntegration.ActiveChatter == null)
            return;
         
        var country = TwitchIntegration.ActiveChatter.GetLocalization();
        if (country == null) return;

        __result = country;
    }
}

