using HarmonyLib;
using play.day.border;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Border))]
public static class BorderPatch {
    public static Border Border;

    //     BorderPatch.Border.set_snipingEnabled(true); // Not needed?
    //     BorderPatch.Border.sendRunner(); // Works, but turns the endless game into story mode
    //     Console.Out.WriteLine("DESK ITEM ID " + BorderPatch.Border.killRifleButton.keyDeskItemId);
    //     AutoStepTraveler.Give(BorderPatch.Border.killRifleButton.keyDeskItemId); // Doesnt work

    [HarmonyPrefix]
    [HarmonyPatch("panic")]
    private static bool AvoidPanic() {
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch("panicLeavingPersonRight")]
    private static bool AvoidSinglePanic() {
        return false;
    }

}