using app.ent;
using data;
using HarmonyLib;
using Il2CppSystem;
using play.screen;
using play.ui;
using psp_papers_mod.MonoBehaviour;
using System.Threading.Tasks;

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

    [HarmonyPostfix]
    [HarmonyPatch("addButton", typeof(Button), typeof(Object))]
    private static void ChangeVersion(Button button, Object centerButton) {
        if (button.id != "version") return;

        // sdd
        Task.Delay(200)
            .SuccessWithUnityThread(() =>
                button.text.set_text($"PSPMod v-{ModInformation.Version}")
            );
    }

}

[HarmonyPatch(typeof(TitleScreen))]
public class TitleScreenPatch {

    [HarmonyPrefix]
    [HarmonyPatch("menu_onClick", typeof(string))]
    private static bool MenuClickPatch(string id, TitleScreen __instance) {
        switch (id) {
            case "endless":
                EntEnv env = __instance.trunk.trunkEnv;
                env.gameTransition.fadeToEndlessDay(new EndlessId(env.db.endlessLib, "endurance", "course3"));
                break;

            case "version":
                System.Diagnostics.Process.Start("explorer.exe", "https://github.com/psp1g/papers");
                break;

            default:
                return true;
        }

        return false;
    }

}