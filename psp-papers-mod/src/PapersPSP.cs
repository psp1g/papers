using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using psp_papers_mod.Twitch;

namespace psp_papers_mod;

[BepInPlugin("wtf.psp.papers", "PSP PAPERS", "1.0.0")]
[BepInProcess("PapersPlease.exe")]
public class PapersPSP : BasePlugin {

    public static bool Enabled { get; set; } = true;

    public static bool Initialized { get; private set; }

    public static TwitchIntegration Twitch { get; private set; }

    internal new static ManualLogSource Log { get; private set; }

    internal static ConfigEntry<string> Channel { get; private set; }
    internal static ConfigEntry<string> BotName { get; private set; }
    internal static ConfigEntry<string> BotPass { get; private set; }

    private readonly Harmony harmony = new("PapersPSP");

    public override void Load() {
        Log = base.Log;

        Channel = this.Config.Bind(
                "Twitch",
                "Channel",
                "psp1g",
                "The chat channel to integrate with"
            );

        BotName = this.Config.Bind(
                "Twitch.Bot",
                "Username",
                "ai1g",
                "Username of the bot to sign into"
            );

        BotPass = this.Config.Bind(
                "Twitch.Bot",
                "Token",
                "",
                "Twitch bot authentication token"
            );

        Log.LogInfo("Loaded config...");

        if (Channel.Value.Trim() == "" || BotName.Value.Trim() == "" || BotPass.Value.Trim() == "") {
            Log.LogError("No channel/bot credentials provided to integrate with! The PSP papers plugin will have no effect.");
            return;
        }

        Twitch = new TwitchIntegration();
        Initialized = true;

        this.harmony.PatchAll();
    }

}
