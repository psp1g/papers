using data;
using HarmonyLib;
using play.day.border;
using psp_papers_mod.Twitch;
using System;
using System.Linq;
using Object = Il2CppSystem.Object;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Person))]
public class PersonPatch {

    private static readonly string[] ExitPathIds = ["entrant-detainstart", "exit-right", "exit-left"];
    
    [HarmonyPrefix]
    [HarmonyPatch("setAnim", typeof(Anim), typeof(Object))]
    private static void PersonSetAnim(Anim anim, Object movingHorizontal, Person __instance) {
        if (__instance.id == "runner" && anim.id == "run" && TwitchIntegration.ActiveAttackerPerson == null)
            TwitchIntegration.ActiveAttackerPerson = __instance;

        if (!anim.death) return;
        
        if (__instance.id == "runner") {
            TwitchIntegration.ActiveAttacker?.Shot();
        } else if (TwitchIntegration.ChattersPerPerson.TryGetValue(__instance.Pointer.ToInt64(), out Chatter chatter)) {
            chatter.Shot();
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch("setPath", typeof(string), typeof(Object), typeof(Object))]
    private static void PersonSetPath(string pathId, Object pathNumStops, Object delay, Person __result) {
        if (!ExitPathIds.Contains(pathId)) return;
        Chatter chatter = TwitchIntegration.ActiveChatter;
        if (chatter == null) return;

        TwitchIntegration.ChattersPerPerson.Add(__result.Pointer.ToInt64(), chatter);
    }
}