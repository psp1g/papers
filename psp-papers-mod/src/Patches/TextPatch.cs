using System.Text.RegularExpressions;
using HarmonyLib;
using Il2CppSystem;
using play.ui;
using psp_papers_mod.Twitch;

namespace psp_papers_mod.Patches;

public static class TextPatch {

    public static string Process(string text) {
        return Regex.Replace(text, "arstotzka", "SUSUSTERJA", RegexOptions.IgnoreCase);
    }
    
    public static void SetMenuTextPrefix(ref string v) {
        if (v is not "The day was cut short by a terrorist attack.") return;

        Chatter attacker = TwitchIntegration.ActiveAttacker;
        if (attacker == null) return;

        v = v.Replace(".", " by " + attacker.Username + ".");
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

public class MenuTextPatch {

    public static void SetTextPrefix(string v) {
        // v = TextPatch.Process(v);
    }

}