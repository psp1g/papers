using app.plat;
using HarmonyLib;
using Il2CppSystem;
using LibCpp2IL.BinaryStructures;
using play;

namespace psp_papers_mod.Patches;

// Prevent execution of all these functions so that we
// don't send any potential false data to the leaderboards

// [HarmonyPatch(typeof(PlatformSocialService))]
// public class PlatformSocialServicePatch {
//
//     [HarmonyPrefix]
//     [HarmonyPatch("pushLeaderboard", typeof(string), typeof(int), typeof(int))]
//     private static bool PushLeaderboardPrefix() {
//         return false;
//     }
//
//     [HarmonyPrefix]
//     [HarmonyPatch("reportStat", typeof(string), typeof(int))]
//     private static bool ReportStatPrefix() {
//         return false;
//     }
//
// }

[HarmonyPatch(typeof(PlatformSocial))]
public class PlatformSocialPatch {

    [HarmonyPrefix]
    [HarmonyPatch("reportLeaderboardScore", typeof(string), typeof(int), typeof(int))]
    private static bool ReportLeaderboardScorePrefix() {
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch("reportStat", typeof(string), typeof(int))]
    private static bool ReportStatPrefix(string name, int value) {
        Console.Out.WriteLine(name +" "+ value);
        return false;
    }

}

[HarmonyPatch(typeof(AlltimeStats))]
public class AlltimeStatsPatch {

    [HarmonyPrefix]
    [HarmonyPatch("addScore", typeof(string), typeof(ScoreEntry))]
    private static bool AddScorePrefix() {
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch("addScoreLocal", typeof(string), typeof(ScoreEntry))]
    private static bool AddScoreLocalPrefix() {
        return false;
    }

}