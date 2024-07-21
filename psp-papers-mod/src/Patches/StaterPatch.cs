using app;
using HarmonyLib;
using play.day.border;
using psp_papers_mod.Twitch;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Stater))]
public static class StaterPatch {

    [HarmonyPrefix]
    [HarmonyPatch("go")]
    private static bool PreventFade(string name, object instant) {
        Border border = BorderPatch.Border;

        if (border is not { day.endless: not null }) return true;
        if (name != "waiting-to-fade-to-night") return true;

        if (BorderPatch.ThrewGrenade) TwitchIntegration.ActiveChatter = null;
        return BorderPatch.ThrewGrenade;
    }

}