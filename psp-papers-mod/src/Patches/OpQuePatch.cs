using data;
using HarmonyLib;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(OpQue))]
public class OpQuePatch {
    
    [HarmonyPrefix]
    [HarmonyPatch("push")]
    static bool PreventOriginalSayOps(ref Op op, object overrideDelay) {
        Op_SAY sayOp = op.TryCast<Op_SAY>();
        return sayOp == null || sayOp.speechId.StartsWith("__override__");
    }
    
}