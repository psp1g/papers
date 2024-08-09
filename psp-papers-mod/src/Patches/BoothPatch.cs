using HarmonyLib;
using play.day.booth;
using data;

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

    [HarmonyPostfix]
    [HarmonyPatch("deskItem_onMounted", typeof(DeskItem), typeof(bool))]
    private static void onMounted(DeskItem deskItem, bool mounted) {
        // if mounted the paper doesn't get deleted after traveler leaves
        PapersPSP.Log.LogWarning("mounted: " + deskItem.id);
        Paper paper = BorderPatch.Border.booth.autoFindPaper(deskItem.id);
        if (mounted) {
            paper.def.stay = Stay.DAY;
        } else {
            paper.def.stay = Stay.NONE;
        }
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

