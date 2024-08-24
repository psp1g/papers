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
        if (v == null) return;
        
        if (v is "The day was cut short by a terrorist attack.") {
            Chatter attacker = TwitchIntegration.ActiveAttacker;
            if (attacker == null) return;

            v = v.Replace(".", " by " + attacker.Username + ".");
            return;
        }

        if (ChatterGuards.WantToGuardChatters.Count < 1) {
            if (v is "Assigned guards:" || v.Contains("__GUARD")) v = "";
        } else {
            ChatterGuards.SetGuards(); 
            if (v is "__GUARD0__") v = ChatterGuards.Guards[0]?.Username;
            if (v is "__GUARD1__") v = ChatterGuards.Guards[1]?.Username;
            if (v is "__GUARD2__") v = ChatterGuards.Guards[2]?.Username;
            if (v is "__GUARD3__") v = ChatterGuards.Guards[3]?.Username;
            if (v is "__GUARD4__") v = ChatterGuards.Guards[4]?.Username;
        }
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