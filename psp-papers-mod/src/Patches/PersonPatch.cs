using data;
using HarmonyLib;
using play.day.border;
using psp_papers_mod.Twitch;
using Object = Il2CppSystem.Object;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Person))]
public class PersonPatch {

    [HarmonyPrefix]
    [HarmonyPatch("setAnim", typeof(Anim), typeof(Object))]
    private static void PersonSetAnim(Anim anim, Object movingHorizontal, Person __instance) {
        if (__instance.id == "runner" && anim.id == "run" && TwitchIntegration.ActiveAttackerPerson == null)
            TwitchIntegration.ActiveAttackerPerson = __instance;

        if (!anim.death) return;
        if (__instance.id == "runner") TwitchIntegration.ActiveAttacker?.Shot();
    }

}