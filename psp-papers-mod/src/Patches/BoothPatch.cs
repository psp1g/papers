using HarmonyLib;
using play.day.booth;
using data;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Booth))]
public static class BoothPatch {


    [HarmonyPostfix]
    [HarmonyPatch("deskItem_onMounted", typeof(DeskItem), typeof(bool))]
    private static void onMounted(DeskItem deskItem, bool mounted) {
        // if mounted the paper doesn't get deleted after traveler leaves

        PapersPSP.Log.LogWarning("mounted: " + deskItem.id);
        Paper paper = BorderPatch.Border.booth.autoFindPaper(deskItem.id);

        if (mounted) {
            paper.def.stay = Stay.DAY;
        } else {
            paper.def.stay = Stay.NONE;

        }

    }


}