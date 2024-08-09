using app.ent;
using data;
using HarmonyLib;
using play.day.booth;
using app.vis;


namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(DeskItem))]
public class DeskItemPatch {

    private static DeskItem deskItem;


    [HarmonyPrefix]
    [HarmonyPatch("__hx_ctor_play_day_booth_DeskItem", typeof(DeskItem), typeof(Ent), typeof(string), typeof(Filer), typeof(Reveal), typeof(Hide), typeof(FilerType), typeof(SoundsDef), typeof(TouchGlows), typeof(Carousel), typeof(PointData), typeof(bool), typeof(bool))]
    static void CtorPrefix(object[] __args, Ent parent_, string id_, Filer filer_, Reveal reveal_, Hide hide_, FilerType filerType_, SoundsDef soundsDef_, TouchGlows touchGlows_, Carousel carousel_, PointData visaCenter_, bool canPinch_, bool multitouchEnabled_) {        
        deskItem = (DeskItem)__args[0];
    }

    
    [HarmonyPrefix]
    [HarmonyPatch("startRevealAnim", typeof(PointData) )]
    static bool preventReveal(PointData mountPos) {
        if (deskItem.id.StartsWith("emoteBlank")) return false;

        return true; 
    }

}