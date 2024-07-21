using TwitchLib.Client.Models;
using data;
using Il2CppSystem;
using psp_papers_mod.MonoBehaviour;
using psp_papers_mod.Patches;
using psp_papers_mod.Utils;

namespace psp_papers_mod.Twitch.Commands;

public static class Commands {
    [ChatCommand("leave")]
    public static void LeaveCommand(Chatter sender, ChatMessage chatMessage, string[] _) {
        if (!sender.Moderator && !sender.Streamer && !sender.IsActiveChatter) return;
        // Force the active booth chatter to leave
        UnityThreadInvoker.Invoke(() => {
            if (BorderPatch.Border.booth.stater.curState.name != "working") return;
            BoothEnginePatch.BoothEngine.applyOps(
                Il2CppUtils.HaxeArrayOf(new Op_ENABLEBUTTON("Leave")),
                true.ToIl2CppBoxed()
            );
        });
    }

    [ChatCommand("force")]
    public static async void ForceCommand(Chatter sender, ChatMessage chatMessage, string[] args) {
        if (!sender.Moderator && !sender.Streamer) return;

        Chatter forceChatter = sender;

        if (args.Length > 0) {
            string chatterName = args[0].ToLower();
            forceChatter = await PapersPSP.Twitch.FrequentChatters.FromUsername(chatterName);

            if (forceChatter == null) {
                chatMessage.Reply("I can't find a chatter by that username!");
                return;
            }
        }

        TwitchIntegration.ForcedActiveQueue.Enqueue(forceChatter);
    }
}