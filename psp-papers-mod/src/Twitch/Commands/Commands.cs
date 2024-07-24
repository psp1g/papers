using TwitchLib.Client.Models;
using psp_papers_mod.MonoBehaviour;
using psp_papers_mod.Patches;

namespace psp_papers_mod.Twitch.Commands;

public static class Commands {

    [ChatCommand("leave")]
    public static void LeaveCommand(Chatter sender, ChatMessage chatMessage, string[] _) {
        if (!sender.Moderator && !sender.Streamer && !sender.IsActiveChatter) return;
        // Force the active booth chatter to leave
        UnityThreadInvoker.Invoke(BoothEnginePatch.ForceTravelerLeave);
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

    [ChatCommand("attack")]
    public static void Attack(Chatter sender, ChatMessage chatMessage, string[] args) {
        sender.WantsAttack();
    }

    // todo; PAP-23
    [ChatCommand("bomb")]
    public static void Bomb(Chatter sender, ChatMessage chatMessage, string[] args) {
        sender.WantsBomb = true;
    }

    [ChatCommand("nobomb")]
    public static void NoBomb(Chatter sender, ChatMessage chatMessage, string[] args) {
        sender.WantsBomb = false;
    }

}