using play;
using System;
using HarmonyLib;

namespace psp_papers_mod.Patches {

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
                //TravelerNamePatch.Reset(); " OLD METHOD TO NOT MAKE TRAVELERS PANIC AND FADE INTO END-SCREEN "
            }
            
        }
    }

    
    
}