using app.ent;
using HarmonyLib;
using Il2CppSystem;
using play.day.booth;
using play.day.border;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Ent))]
public class EntPatch {

    [HarmonyPostfix]
    [HarmonyPatch("__hx_ctor_app_ent_Ent", typeof(Ent), typeof(Ent), typeof(string))]
    static void CtorPostfix(object[] __args) {
        Ent ent = (Ent)__args[0];

        Border border = ent.TryCast<Border>();
        if (border != null) {
            BorderPatch.Border = border;
            return;
        }
    }

}