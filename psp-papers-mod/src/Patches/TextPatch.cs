using System.Diagnostics;
using System.Text.RegularExpressions;
using app.vis;
using HarmonyLib;
using Il2CppSystem;
using play.ui;

namespace psp_papers_mod.Patches;

public static class TextPatch {

    public static string Process(string text) {

        PapersPSP.Log.LogInfo($"TEXT: {text}");
        return Regex.Replace(text, "arstotzka", "SUSUSTERJA", RegexOptions.IgnoreCase);
    }

}

[HarmonyPatch(typeof(RevealTextEnt))]
public class RevealTextPatch {

    [HarmonyPrefix]
    [HarmonyPatch("set_text", typeof(string))]
    private static void SetTextPrefix(ref string text_) {
        text_ = TextPatch.Process(text_);
    }

}

[HarmonyPatch(typeof(SpeechBubble))]
public class SpeechBubblePatch {

    [HarmonyPrefix]
    [HarmonyPatch("showText", typeof(string), typeof(Object))]
    private static void SetTextPrefix(ref string text) {
        text = TextPatch.Process(text);
    }

}