using BepInEx.Configuration;

namespace psp_papers_mod;

public static class Cfg {

    internal static ConfigEntry<string> Channel { get; private set; }
    internal static ConfigEntry<string> BotName { get; private set; }
    internal static ConfigEntry<string> BotPass { get; private set; }
    internal static ConfigEntry<string> APIClientID { get; private set; }
    internal static ConfigEntry<string> APISecret { get; private set; }

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
    }

}