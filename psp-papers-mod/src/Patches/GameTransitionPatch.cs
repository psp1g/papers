using play;
using System;
using HarmonyLib;

namespace psp_papers_mod.Patches {

    [HarmonyPatch(typeof(GameTransition))]
    public class GameTransitionPatch {

        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameTransition), "call")]
        public static void Prefix(GameTransitionKind gameTransitionKind) {
            Console.Out.WriteLine("KIND: " + gameTransitionKind.toString());
        }

    }

}