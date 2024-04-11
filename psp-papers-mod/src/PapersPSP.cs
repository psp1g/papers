using System;
using System.Reflection;
using System.Threading.Tasks;
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

    internal static bool Initialized { get; private set; }

    internal static TwitchIntegration Twitch { get; private set; }

    internal static readonly Random Random = new();

    internal new static ManualLogSource Log { get; private set; }

    private readonly Harmony harmony = new("PapersPSP");

    public override void Load() {
        Log = base.Log;

        Cfg.StartBindings(this);
        Log.LogInfo("Loaded config...");

        if (Cfg.Channel.Value.Trim() == "" || Cfg.BotName.Value.Trim() == "" || Cfg.BotPass.Value.Trim() == "") {
            Log.LogError("No channel/bot credentials provided to integrate with! The PSP papers plugin will have no effect.");
            return;
        }

        Twitch = new TwitchIntegration();
        Twitch.Connect().Wait();

        Initialized = true;

        this.harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

}
