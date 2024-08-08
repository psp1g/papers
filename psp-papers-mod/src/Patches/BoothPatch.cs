using HarmonyLib;
using play.day.booth;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Booth))]
public class BoothPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Booth.get_timeIsUp))]
    public static bool OverrideTimeIsUp(ref Booth __instance, ref bool __result) {
        __result = false;
        __instance.day.minTravelers = 1;
        if (__instance.engine.numTravelers >= __instance.day.minTravelers) {
            __result = ConsoleClockPatch.DayFinished;
        }

        return false;
    }
}

public class ConsoleClockPatch {
    
    public static bool DayFinished;
    
    public static void CheckDayFinished(ConsoleClock __instance) {
        if (System.Math.Abs(__instance.hour - 18.0) < 0.1) {
            DayFinished = true;
        }
    }
}