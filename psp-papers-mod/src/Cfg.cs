using BepInEx.Configuration;

namespace psp_papers_mod;

public static class Cfg {

    internal static ConfigEntry<string> Channel { get; private set; }

    internal static ConfigEntry<string> BotName { get; private set; }
    internal static ConfigEntry<string> BotPass { get; private set; }

    internal static ConfigEntry<string> APIClientID { get; private set; }
    internal static ConfigEntry<string> APISecret { get; private set; }

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
            "ai1g",
            "Username of the bot to sign into"
        );

        BotPass = mod.Config.Bind(
            "Twitch.Bot",
            "Token",
            "",
            "Twitch bot authentication token"
        );

        APIClientID = mod.Config.Bind(
            "Twitch.API",
            "ClientID",
            "",
            "Twitch API client ID"
        );

        APISecret = mod.Config.Bind(
            "Twitch.API",
            "Secret",
            "",
            "Twitch API Auth Token/Secret"
        );

        AlwaysPreventSameActiveChatter = mod.Config.Bind(
            "Mod.ActiveChatterSelection",
            "AlwaysPreventSameActiveChatter",
            false,
            "A recent active chatter has less chance of being an active chatter again, and has no chance to be the active chatter twice in a row. Set this to true if you never want the same active chatter twice"
        );

        VIPWeightMultiplier = mod.Config.Bind(
            "Mod.ActiveChatterSelection",
            "VIPWeightMultiplier",
            1.0d,
            "A VIP's weight (chance) to be selected as an active chatter is multiplied by this number (0.5 = half of normal chance, 2 = double chance)"
        );

        ModeratorWeightMultiplier = mod.Config.Bind(
            "Mod.ActiveChatterSelection",
            "ModeratorWeightMultiplier",
            1.0d,
            "A moderator's weight (chance) to be selected as an active chatter is multiplied by this number (0.5 = half of normal chance, 2 = double chance)"
        );

        SubscriberWeightMultiplier = mod.Config.Bind(
            "Mod.ActiveChatterSelection",
            "SubscriberWeightMultiplier",
            1.0d,
            "A subscriber's weight (chance) to be selected as an active chatter is multiplied by this number (0.5 = half of normal chance, 2 = double chance)"
        );

        TwitchStaffWeightMultiplier = mod.Config.Bind(
            "Mod.ActiveChatterSelection",
            "StaffWeightMultiplier",
            1.0d,
            "A Twitch Staff's weight (chance) to be selected as an active chatter is multiplied by this number (0.5 = half of normal chance, 2 = double chance)"
        );
    }

}