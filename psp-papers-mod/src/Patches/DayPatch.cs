using HarmonyLib;
using play.day;
using psp_papers_mod.Utils;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Day))]
public class DayPatch {

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Day.setAttackResultWithPriority))]
    private static bool PreventEnd(AttackResult ar) {
        return !ar.IsType<AttackResult_HIT_INNOCENT>() && !ar.IsType<AttackResult_HIT_GUARD>();
    }
    
}