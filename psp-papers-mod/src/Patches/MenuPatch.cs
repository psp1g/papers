using app.ent;
using data;
using HarmonyLib;
using play.screen;
using play.ui;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(Menu))]
public class MenuPatch {
    [HarmonyPrefix]
    [HarmonyPatch("button_onClick", typeof(Button))]
    private static bool ButtonOnClickPrefix(ref Button b) {
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch("addPushButton", typeof(string), typeof(string), typeof(app.vis.Align))]
    private static bool PreventStoryButton(string id, string text, app.vis.Align align, Menu __instance) {
        return id != "story";
    }
}

[HarmonyPatch(typeof(TitleScreen))]
public class TitleScreenPatch {
    [HarmonyPrefix]
    [HarmonyPatch("menu_onClick", typeof(string))]
    private static bool SkipEndlessScreen(string id, TitleScreen __instance) {
        if (id != "endless") return true;

        EntEnv env = __instance.trunk.trunkEnv;
        env.gameTransition.fadeToEndlessDay(new EndlessId(env.db.endlessLib, "endurance", "course3"));
        return false;
    }
}