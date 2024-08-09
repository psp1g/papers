using data;
using play;
using System;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using play.day;
using play.day.border;
using psp_papers_mod.Twitch;
using psp_papers_mod.Utils;
using Object = Il2CppSystem.Object;

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
        ConsoleClockPatch.DayFinished = false;
        
        if(!transitionName.Contains("ADVANCE_TO_NEXTDAY")) return;
        Day day = BorderPatch.Border.day;
        day.numDetains = 0;
        day.numProcessedTravelersPaid = 0;
        day.numProcessedTravelersUnpaid = 0;
        day.citations = Il2CppUtils.NewHaxeArray(0);

    }
}

[HarmonyPatch(typeof(EndlessId))]
public class EndlessIdPatch {

    [HarmonyPrefix]
    [HarmonyPatch(nameof(EndlessId.__hx_ctor_data_EndlessId))]
    public static void ChangeId(ref string styleId_, ref string courseId_) {
        styleId_ = "endurance";
        courseId_ = "course3";
    }
    
}