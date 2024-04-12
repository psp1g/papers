using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using psp_papers_mod.Twitch;
using psp_papers_mod.MonoBehaviour;
using Il2CppInterop.Runtime.Injection;

namespace psp_papers_mod;

[BepInPlugin("wtf.psp.papers", "PSP PAPERS", "1.1.0")]
[BepInProcess("PapersPlease.exe")]
public class PapersPSP : BasePlugin {

    internal static bool Initialized { get; private set; }

    internal static TwitchIntegration Twitch { get; private set; }

    internal static readonly System.Random Random = new();

    internal static new ManualLogSource Log { get; private set; }

    private readonly Harmony harmony = new("PapersPSP");

    public override void Load() {
        Log = base.Log;

        Cfg.StartBindings(this);
        Log.LogInfo("Loaded config...");

        if (Cfg.Channel.Value.Trim() == "" || Cfg.BotName.Value.Trim() == "" || Cfg.AccessToken.Value.Trim() == "") {
            Log.LogError("No channel/bot credentials provided to integrate with! The PSP papers plugin will have no effect.");
            return;
        }

        ClassInjector.RegisterTypeInIl2Cpp<UnityThreadInvoker>();

        this.AddComponent<UnityThreadInvoker>();

        Twitch = new TwitchIntegration();
        Twitch.Connect().Wait();

        Initialized = true;

        this.harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

}
