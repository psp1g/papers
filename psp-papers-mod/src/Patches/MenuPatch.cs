using data;
using HarmonyLib;
using Il2CppSystem;
using play;
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
    [HarmonyPatch("addButton", typeof(Button), typeof(Object))]
    private static void AddButtonPrefix(ref Button button) {
        if (button?.text?.text == null) return;
        if (button.text.text.ToLower() != "endless")
            button.text.text = "ENDLESS CHAT";

        PapersPSP.Log.LogInfo($"{button.text.text}");
    }

}