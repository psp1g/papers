using play;
using System;
using HarmonyLib;
using psp_papers_mod.Twitch;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Game))]
public class GameTransitionPatch {
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Game), "applyScreenChange")]
    public static void Prefix(ScreenChange screenChange) {
        Console.Out.WriteLine("Name: " + screenChange.screenName);
        // screenChange.screenName is the name of the current screen.
        // title, news, endless, day, night, ......
        if (screenChange.screenName == "news") {
            screenChange.gameTransitionKind = GameTransitionKind.NONE;
            screenChange.screenName = "day";
            Console.Out.WriteLine("Changed to : " + screenChange.screenName);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameTransition), "call")]
    public static void Prefix(GameTransitionKind gameTransitionKind) {
        string transitionName = gameTransitionKind.ToString();
        if (!transitionName.Contains("FADE_TO_ENDLESS_DAY") && !transitionName.Contains("ADVANCE_TO_NEXTDAY")) return;
        
        TravelerNamePatch.Reset();
        BorderPatch.ThrewGrenade = false;
        TwitchIntegration.ActiveAttacker = null;
        TwitchIntegration.ActiveAttackerPerson = null;
        TwitchIntegration.ChattersPerPerson.Clear();
        TwitchIntegration.PrepareNextChatter();
    }
}