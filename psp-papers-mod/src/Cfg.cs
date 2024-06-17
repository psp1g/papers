using BepInEx.Configuration;

namespace psp_papers_mod;

public static class Cfg {

    internal static ConfigEntry<string> Channel { get; private set; }

    internal static ConfigEntry<string> BotName { get; private set; }
    internal static ConfigEntry<string> AccessToken { get; private set; }
    internal static ConfigEntry<string> ClientID { get; private set; }
    internal static ConfigEntry<bool> UseLocalAuthServer { get; private set; }

    internal static ConfigEntry<bool> AlwaysPreventSameActiveChatter { get; private set; }

    internal static ConfigEntry<double> VIPWeightMultiplier { get; private set; }
    internal static ConfigEntry<double> ModeratorWeightMultiplier { get; private set; }
    internal static ConfigEntry<double> SubscriberWeightMultiplier { get; private set; }
    internal static ConfigEntry<double> TwitchStaffWeightMultiplier { get; private set; }

    public static void StartBindings(PapersPSP mod) {
        Channel = mod.Config.Bind(
            "Twitch",
            "Channel",
            "psp1g",
            "The chat channel to integrate with"
        );

        BotName = mod.Config.Bind(
            "Twitch.Bot",
            "Username",
            "",
            "Username of the bot to sign into"
        );

        AccessToken = mod.Config.Bind(
            "Twitch.Bot",
            "AccessToken",
            "",
            "Twitch bot authentication token"
        );

        ClientID = mod.Config.Bind(
            "Twitch.Bot",
            "ClientID",
            "",
            "Twitch API client ID"
        );
        
        UseLocalAuthServer = mod.Config.Bind(
            "Twitch.Bot",
            "UseLocalAuthServer",
            false,
            "Only use this if you know what you are doing. Requests a twitch auth token from a local auth server. Requires botUsername"
        );

        AlwaysPreventSameActiveChatter = mod.Config.Bind(
            "Twitch.ChatterSelection",
            "AlwaysPreventSameActiveChatter",
            false,
            "A recent active chatter has less chance of being an active chatter again, and has no chance to be the active chatter twice in a row. Set this to true if you never want the same active chatter twice"
        );

        VIPWeightMultiplier = mod.Config.Bind(
            "Twitch.ChatterSelection",
            "VIPWeightMultiplier",
            1.0d,
            "A VIP's weight (chance) to be selected as an active chatter is multiplied by this number (0.5 = half of normal chance, 2 = double chance)"
        );

        ModeratorWeightMultiplier = mod.Config.Bind(
            "Twitch.ChatterSelection",
            "ModeratorWeightMultiplier",
            1.0d,
            "A moderator's weight (chance) to be selected as an active chatter is multiplied by this number (0.5 = half of normal chance, 2 = double chance)"
        );

        SubscriberWeightMultiplier = mod.Config.Bind(
            "Twitch.ChatterSelection",
            "SubscriberWeightMultiplier",
            1.0d,
            "A subscriber's weight (chance) to be selected as an active chatter is multiplied by this number (0.5 = half of normal chance, 2 = double chance)"
        );

        TwitchStaffWeightMultiplier = mod.Config.Bind(
            "Twitch.ChatterSelection",
            "StaffWeightMultiplier",
            1.0d,
            "A Twitch Staff's weight (chance) to be selected as an active chatter is multiplied by this number (0.5 = half of normal chance, 2 = double chance)"
        );
    }

}