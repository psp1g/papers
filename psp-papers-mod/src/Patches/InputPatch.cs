using HarmonyLib;
using haxe.io;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Input))]
public class InputPatch {
    
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Input.readInt16))]
    private static void PrintInt16Postfix(Input __instance, ref int __result) {
        // papers tools sets the sign bit to 1 to indicate a 32-bit integer
        if (__result >= 0) return;
        int first = __instance.readByte();
        int second = __instance.readByte();
        __result = __result << 16 | second << 8 | first;
        __result &= 0x7FFFFFFF;
    }
    
}