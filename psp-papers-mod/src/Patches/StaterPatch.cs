using app;
using HarmonyLib;
using play.day;
using play.day.border;
using System;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Stater))]
public static class StaterPatch {
    [HarmonyPrefix]
    [HarmonyPatch("go")]
    private static bool PreventFade(string name, object instant) {
        Border border = BorderPatch.Border; 
        if (border is { day.endless: not null }) {
            return name != "waiting-to-fade-to-night" || BorderPatch.ThrewGrenade;
        }

        return true;
    }
}