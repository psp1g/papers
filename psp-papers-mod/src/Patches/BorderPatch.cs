using app.ent;
using app.vis;
using System;
using HarmonyLib;
using play.day;
using play.day.border;
using psp_papers_mod.Twitch;
using psp_papers_mod.Utils;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Border))]
public static class BorderPatch {

    public static Border Border;
    public static bool ThrewGrenade;

    [HarmonyPrefix]
    [HarmonyPatch("panic")]
    private static bool AvoidPanic() {
        return ThrewGrenade;
    }

    [HarmonyPrefix]
    [HarmonyPatch("panicLeavingPersonRight")]
    private static bool AvoidSinglePanic() {
        return false;
    }
    
    
    [HarmonyPrefix]
    [HarmonyPatch("throwGrenade")]
    public static void ThrowGrenade() {
        ThrewGrenade = true;
        Border.day.setAttackResultWithPriority(AttackResult.FAILED_DIDNOTHING);
    }

    public static void SendChatterRunner() {
        Chatter chatter = PapersPSP.Twitch.FrequentChatters.GetRandomChatter(true);
        if (chatter != null) TwitchIntegration.SetActiveAttacker(chatter);
        // todo; Fix delays triggering end of game (?)
        //UnityThreadInvoker.Invoke(() => Border.sendRunner());
        Border.sendRunner();
    }

    [HarmonyPrefix]
    [HarmonyPatch("checkSniped", typeof(Person), typeof(PointData), typeof(string))]
    private static bool SnipedPrefix(Person person, PointData pos, string shotAnim, ref bool __result) {
        if (person.id != "guard0" && person.id != "guard1") return true;
        // Killing two left guards during detains breaks the game

        __result = false;
        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch("checkSniped", typeof(Person), typeof(PointData), typeof(string))]
    private static void SnipedPostfix(Person person, PointData pos, string shotAnim, ref bool __result) {
        if (!__result) return;

        // Can't call traveler if they were shot, have to remove from queue
        if (person.id == "waiting") {
            Border.waitingLine.people.remove(person);
        }

        Console.Out.WriteLine("SNIPED " + person.id + " SHOTANIM: " + shotAnim);
    }

    [HarmonyPostfix]
    [HarmonyPatch("booth_onDetaineeLeaves", typeof(bool))]
    private static void OnDetain(bool resisting) {
        if (TwitchIntegration.ActiveChatter == null) return;
        TwitchIntegration.ActiveChatter.Detain();
    }
    
}

[HarmonyPatch(typeof(RifleButton))]
public static class RifleButtonPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(RifleButton.react))]
    private static bool DeselectRifle(Input input, RifleButton __instance) {
        if (__instance.isTranq || !__instance.selected || __instance.stater.curState.name != "unlocked") return true;
        Rect rect = __instance.rifleNormalSprite.hitRect(false.ToIl2CppBoxed());
        if (!input.checkPointerJustDown(__instance, __instance.worldPos(), rect, null)) return true;
        
        __instance.set_selected(false);
        return false;

    }
}