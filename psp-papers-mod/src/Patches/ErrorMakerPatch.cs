using data;
using HarmonyLib;
using Il2CppSystem;
using psp_papers_mod.MonoBehaviour;
using psp_papers_mod.Twitch;
using psp_papers_mod.Utils;
using System.Collections.Generic;
using Console = System.Console;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(ErrorMaker))]
public class ErrorMakerPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(ErrorMaker.makeError))]
    private static bool PrintErrorOps(string errorIdPatterns, ErrorContext context, ErrorMaker __instance, ref Error __result) {
        Chatter chatter = TwitchIntegration.ActiveChatter;
        if (chatter == null) return true;
        
        chatter.JuicerCheck().Wait(); // FIXME: Run this while the previous traveler is being processed asynchronously
        if (!chatter.Juicer) return true;
        
        List<Object> matches = __instance.errorLib.getMatchingErrors("<juicer>").ToIlList();
        if (matches.Count == 0) return true;
        __result = matches[System.Math.Abs(__instance.rand.nextInt()) % matches.Count].TryCast<Error>();
        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(ErrorMaker.makeError))]
    private static void PrintErrorOps(ref Error __result) {
        if (__result == null) return;
        var iterator = __result.ops.iterator();
        while (iterator.hasNext()) {
            Console.WriteLine(iterator.next().Cast<Op>().ToString());
        }
    }
}