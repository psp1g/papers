using HarmonyLib;
using play.day;
using psp_papers_mod.Utils;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Day))]
public class DayPatch {

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Day.__hx_ctor_play_day_Day))]
    private static void PrepareDay(object[] __args) {
        Day day = (Day)__args[0];
        day.minTravelers = 1;
        day.durationInMinutes = 10;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Day.setAttackResultWithPriority))]
    private static bool PreventEnd(AttackResult ar) {
        if (!BorderPatch.ThrewGrenade && !ar.IsType<AttackResult_NONE>())
            return false;
        return !ar.IsType<AttackResult_HIT_INNOCENT>() && !ar.IsType<AttackResult_HIT_GUARD>();
    }
    
}