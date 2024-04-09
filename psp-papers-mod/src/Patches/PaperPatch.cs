using app.ent;
using data;
using HarmonyLib;
using Il2CppSystem;
using play.day;
using play.day.booth;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Paper))]
public class PaperPatch {

    [HarmonyPostfix]
    [HarmonyPatch("__hx_ctor_play_day_booth_Paper", typeof(Paper), typeof(Ent), typeof(Filer), typeof(PaperDef), typeof(BoothEnv), typeof(int), typeof(TouchGlows), typeof(Object), typeof(Carousel))]
    static void CtorPostfix(PaperDef def_, int multiPaperIndex_) {
        Console.Out.WriteLine("PAPER!!! " + def_.id + " : " + def_.outerImageName  + " : " + def_.mountImageName  + " : " + multiPaperIndex_);
    }

}