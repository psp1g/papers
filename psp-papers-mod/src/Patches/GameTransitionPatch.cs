using play;
using System;
using HarmonyLib;

namespace psp_papers_mod.Patches {

    [HarmonyPatch(typeof(GameTransition))]
    public class GameTransitionPatch {

        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameTransition), "call")]
        public static void Prefix(GameTransitionKind kind) {
            Console.Out.WriteLine("KIND: " + kind.toString());
        }

    }

}