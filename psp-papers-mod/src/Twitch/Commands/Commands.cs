namespace psp_papers_mod.Twitch.Commands;

public static class Commands {

    [ChatCommand("leave")]
    public static void LeaveCommand(Chatter chatter, string[] _) {
        if (!chatter.Moderator && !chatter.Streamer && !chatter.IsActiveChatter) return;
        // Force the active booth chatter to leave
    }

    [ChatCommand("force")]
    public static void ForceCommand(Chatter chatter, string[] args) {
        if (!chatter.Moderator && !chatter.Streamer) return;
        TwitchIntegration.ForcedActiveQueue.Enqueue(chatter);
    }

}