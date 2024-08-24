using data;
using HarmonyLib;
using Il2CppSystem;
using play.day.border;
using psp_papers_mod.Twitch;
using System.Collections.Generic;
using System.Linq;

namespace psp_papers_mod.Patches;

public class ChatterGuards {
    public static ChatterCollection WantGuardChatters = new ();
    public static Chatter[] Guards = new Chatter[5];
    // 0-1: left guards, 2-4: right guards
    
    public static bool SetGuards() {
        if (WantGuardChatters.Count < 1) return false;

        int[] order = { 2, 3, 4, 0, 1 }; // Prioritize setting the right ones first

        foreach (int i in order) {
            for (int j = 0; j < WantGuardChatters.Count; j++) {
                Chatter chatter = WantGuardChatters.GetRandomChatter();
        
                if (!Guards.Contains(chatter)) {
                    Guards[i] = chatter;
                    break;
                }
            }
        }
        
        return true;
    }

    public static string GuardNames(bool right) {
        Chatter[] guards = right ? Guards[2..] : Guards[..2];
        return string.Join(", ", guards.Distinct().Select(c => c?.Username));
    }
}


[HarmonyPatch(typeof(Person))]
public class GuardPersonPatch {

    [HarmonyPrefix]
    [HarmonyPatch("setAnim", typeof(Anim), typeof(Object))]
    private static void PersonSetAnim(Anim anim, Object movingHorizontal, Person __instance) {
        if (!anim.death) return;
        // left guards can't die as of rn
        if(__instance.id == "guard2") ChatterGuards.Guards[2]?.Shot();
        if(__instance.id == "guard3") ChatterGuards.Guards[3]?.Shot();
        if(__instance.id == "guard4") ChatterGuards.Guards[4]?.Shot();

    }
}