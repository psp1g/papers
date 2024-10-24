﻿using psp_papers_mod.Twitch;
using psp_papers_mod.MonoBehaviour;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using psp_papers_mod.Patches;
using System.Linq;
using psp_papers_mod.src.Twitch;

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

        if (
            Cfg.Channel.Value.Trim() == "" ||
            Cfg.BotName.Value.Trim() == "" ||
            (Cfg.AccessToken.Value.Trim() == "" && !Cfg.UseLocalAuthServer.Value)
        ) {
            Log.LogError("No channel/bot credentials provided to integrate with! The PSP papers plugin will have no effect.");
            Log.LogError("Please configure this in \"<steam>\\PapersPlease\\BepInEx\\config\\wtf.psp.papers.cfg\"");
            return;
        }

        ClassInjector.RegisterTypeInIl2Cpp<UnityThreadInvoker>();

        this.AddComponent<UnityThreadInvoker>();

        Twitch = new TwitchIntegration();
        Twitch.Connect().Wait();

        Initialized = true;
        EmotePapers.Initialize();
        ChatterLocalization.LoadFromFile();

        this.harmony.PatchAll(Assembly.GetExecutingAssembly());
        MethodInfo propSetter = typeof(app.vis.Text).GetProperty("text")!.GetSetMethod();
        MethodInfo method = typeof(app.vis.Text).GetMethods().First(m => m.Name == "set_text" && m != propSetter);
        this.harmony.Patch(method, prefix: new HarmonyMethod(typeof(TextPatch).GetMethod("SetMenuTextPrefix")));
        propSetter = typeof(play.day.booth.ConsoleClock).GetProperty("hour")!.GetSetMethod();
        method = typeof(play.day.booth.ConsoleClock).GetMethods().First(m => m.Name == "set_hour" && m != propSetter);
        this.harmony.Patch(method, postfix: new HarmonyMethod(typeof(ConsoleClockPatch).GetMethod("CheckDayFinished")));
    }
}