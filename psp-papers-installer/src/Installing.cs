using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace psp_papers_installer;

// Gross code
// Just wanted it to work without anything fancy
// Its already a windows form app (cope)
public partial class Installing : UserControl {
    private const string Git = $"https://github.com/psp1g/papers/archive/refs/heads/{Program.Branch}.zip";

    private const string BepInEx =
        "https://builds.bepinex.dev/projects/bepinex_be/688/BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.688%2B4901521.zip";

    private const string dotNetInstallScript =
        "https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.ps1";

    private const string PapersTools =
        "https://github.com/psp1g/papers-tools-rs/releases/latest/download/PapersTools-win-x64.exe";

    private const string dotNetPathPattern = """Adding to current process PATH: "(.+)"\.""";

    private const int totalSteps = 1100;

    private string dotNetDir = "";

    private bool[] dlSteps = [false, false, false, false];

    private bool extract;

    private bool update;

    public Installing() {
        this.InitializeComponent();
        this.log.TextChanged += (sender, args) => {
            this.log.SelectionStart = this.log.Text.Length;
            this.log.ScrollToCaret();
        };

        if (Program.AlreadyInstalled()) {
            this.update = true;
            this.log.AppendText($"Updating PSP Papers Please to Latest (@{Program.latestVersion})\n");
            this.title.Text = @"Updating Mod...";
        } else
            this.log.AppendText($"Starting install of PSP Papers Please Latest - @{Program.latestVersion}\n");

        Thread installerThread = new(this.Download);
        installerThread.Start();
    }

    private void SetProgress(int prog) {
        double pct = prog / (totalSteps * 1d);
        this.progressBar1.Invoke(new Action(() =>
            this.progressBar1.Value = (int)(pct * 100)
        ));
    }

    private void Log(string message) {
        this.log.Invoke(() => {
            this.log.AppendText($"{message}\n");
            this.log.SelectionStart = this.log.Text.Length;
            this.log.ScrollToCaret();
        });
    }

    private void DownloadProgress(ref bool finished) {
        if (this.extract) return;

        finished = true;

        int ctFinished = this.dlSteps.Count(step => step);
        this.SetProgress(ctFinished * 100);
    }

    private async void Download() {
        // Remove old mod files
        if (this.update) {
            File.Delete(Path.Combine(Program.PapersDir, "psp-paper-mod.zip"));
            File.Delete(Path.Combine(Program.PapersDir, "papers-tools.exe"));

            if (Directory.Exists(Path.Combine(Program.PapersDir, Program.GitFolderName)))
                Directory.Delete(Path.Combine(Program.PapersDir, Program.GitFolderName), true);

            if (Directory.Exists(Path.Combine(Program.PapersDir, "BepInEx", "plugins"))) {
                Directory.Delete(Path.Combine(Program.PapersDir, "BepInEx", "plugins"), true);
                Directory.CreateDirectory(Path.Combine(Program.PapersDir, "BepInEx", "plugins"));
            }
        } else {
            // If not updating, remove existing BepInEx stuff if any
            if (Directory.Exists(Path.Combine(Program.PapersDir, "BepInEx")))
                Directory.Delete(Path.Combine(Program.PapersDir, "BepInEx"), true);
        }

        if (this.update) {
            this.DownloadProgress(ref this.dlSteps[1]);
            this.DownloadProgress(ref this.dlSteps[2]);
        }

        List<Task> tasks = [
            Program.client.DownloadFileAsync(Git, Path.Combine(Program.PapersDir, "psp-paper-mod.zip"), true)
                .ContinueWith(_ => this.DownloadProgress(ref this.dlSteps[0])),
            Program.client.DownloadFileAsync(PapersTools, Path.Combine(Program.PapersDir, "papers-tools.exe"), true)
                .ContinueWith(_ => this.DownloadProgress(ref this.dlSteps[3])),
        ];

        this.Log($"Downloading PSP Papers Mod @{Program.latestVersion} - {Git}");

        if (!this.update) {
            this.Log($"Downloading BepInEx 6 BE - {BepInEx}");
            this.Log($"Downloading .NET 8 SDK Install Script - {dotNetInstallScript}");

            tasks.AddRange([
                Program.client.DownloadFileAsync(BepInEx, Path.Combine(Program.PapersDir, "bepinex.zip"), true)
                    .ContinueWith(_ => this.DownloadProgress(ref this.dlSteps[1])),
                Program.client.DownloadFileAsync(dotNetInstallScript, Path.Combine(Program.PapersDir, "dotnet6.ps1"), true)
                    .ContinueWith(_ => this.DownloadProgress(ref this.dlSteps[2])),
            ]);
        }

        await Task.WhenAll(tasks);

        this.Extract();
    }

    private void Extract() {
        if (this.extract) return;
        this.extract = true;

        if (!this.update) {
            this.Log($"Extracting BepInEx 6 BE to {Program.PapersDir}");
            ZipFile.ExtractToDirectory(
                Path.Combine(Program.PapersDir, "bepinex.zip"),
                Program.PapersDir,
                Encoding.UTF8,
                true
            );
            this.SetProgress(400);
            this.Log("Extracted BepInEx");
        }

        this.Log($"Extracting PSP Papers Mod to {Program.PapersDir}");
        ZipFile.ExtractToDirectory(
            Path.Combine(Program.PapersDir, "psp-paper-mod.zip"),
            Program.PapersDir,
            Encoding.UTF8,
            true
        );
        this.SetProgress(500);

        this.Log("Extracted PSP Papers Mod");
        this.DotNetInstall();
    }

    private void DotNetInstall() {
        this.Log("Checking .NET 8 SDK installation");

        ProcessStartInfo startInfo = new() {
            FileName = "powershell.exe",
            Arguments =
                $"-ExecutionPolicy Bypass -WindowStyle hidden -NoLogo -command \"& '{Path.Combine(Program.PapersDir, "dotnet6.ps1")}' -Channel 8.0\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process netProc = new() { StartInfo = startInfo, EnableRaisingEvents = true };

        netProc.Exited += this.OnNetFinish;
        netProc.OutputDataReceived += this.NetProcOutput;

        netProc.Start();
        netProc.BeginOutputReadLine();
    }

    private void PatchAssets() {
        string patchDir = Path.Combine(Program.PapersDir, Program.GitFolderName, "asset_patches");

        if (!Directory.Exists(patchDir)) {
            this.Log("No game assets found to patch.");
            this.Finish();
            return;
        }

        this.Log("Patching game assets..");

        ProcessStartInfo startInfo = new() {
            FileName = Path.Combine(Program.PapersDir, "papers-tools.exe"),
            Arguments = $"-g \"{Program.PapersDir}\" patch -p \"{patchDir}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process patchProc = new() { StartInfo = startInfo, EnableRaisingEvents = true };

        patchProc.OutputDataReceived += this.LogProcOutput;
        patchProc.Exited += (_, _) => this.Finish();

        patchProc.Start();
        patchProc.BeginOutputReadLine();
    }


    private void Run() {
        this.Log("Running the game with BepInEx for the first time (Generating hollowed assemblies)");
        this.Log("DO NOT CLOSE THE GAME!!! IT WILL CLOSE AUTOMATICALLY\n");
        Process gProc = Program.RunGame(true);

        gProc?.WaitForExit();
        this.SetProgress(650);

        Thread.Sleep(1500);
        this.Log("Closing game");
        Program.CloseGame();

        string hollowedPath = Path.Combine(Program.PapersDir, "BepInEx", "interop", "Assembly-CSharp.dll");

        if (!File.Exists(hollowedPath)) {
            this.Log("The game didn't generate hollowed assemblies as expected!! Trying again..");
            this.SetProgress(600);
            this.Run();
            return;
        }

        this.Log("Success! Assembly-CSharp.dll found");

        this.SetProgress(700);
        this.Restore();
    }

    private void Restore() {
        this.Log("Installing C# nuget dependencies");

        string projPath = Path.Combine(Program.PapersDir, Program.GitFolderName, "psp-papers-mod", "psp-papers-mod.csproj");
        ProcessStartInfo startInfo = new() {
            FileName = Path.Combine(this.dotNetDir, "dotnet.exe"),
            Arguments = $"restore \"{projPath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process netProc = new() { StartInfo = startInfo, EnableRaisingEvents = true };

        netProc.Exited += this.OnRestoreFinish;
        netProc.OutputDataReceived += this.LogProcOutput;

        netProc.Start();
        netProc.BeginOutputReadLine();
    }

    private void Compile() {
        this.Log("Compiling psp-papers-mod.csproj");

        string projPath = Path.Combine(Program.PapersDir, Program.GitFolderName, "psp-papers-mod", "psp-papers-mod.csproj");
        ProcessStartInfo startInfo = new() {
            FileName = Path.Combine(this.dotNetDir, "dotnet.exe"),
            Arguments = $"msbuild \"{projPath}\" -p:PapersPleaseDir=\"{Program.PapersDir}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process netProc = new() { StartInfo = startInfo, EnableRaisingEvents = true };

        netProc.Exited += this.OnCompileFinish;
        netProc.OutputDataReceived += this.CompileProcOutput;
        netProc.ErrorDataReceived += this.ErrorOutput;

        netProc.Start();
        netProc.BeginOutputReadLine();
    }

    private void Install() {
        this.Log("Installing PSP Papers Mod");

        // Copy built plugin DLLs into plugins folder
        string[] paths = Directory.GetFiles(
            Path.Combine(Program.PapersDir, Program.GitFolderName, "psp-papers-mod", "bin", "Debug", "net8.0"),
            "*.dll"
        );
        foreach (string path in paths) {
            FileInfo info = new(path);
            info.CopyTo(Path.Combine(Program.PapersDir, "BepInEx", "plugins", info.Name));
        }

        this.SetProgress(950);

        if (!this.update) {
            this.SetProgress(1000);

            this.Log("Generating default configurations..");

            // lol
            Program.RunGame();
            Thread.Sleep(5000);
            this.Log("Closing game");

            Program.CloseGame();
        }

        this.SetProgress(1050);

        ProcessModule processModule = Process.GetCurrentProcess().MainModule;

        if (processModule != null) {
            this.Log("Moving installer to game files");
            string installerLocation = processModule.FileName;
            string locInstallerPath = Path.Combine(Program.PapersDir, "PspPapersInstaller.exe");

            if (File.Exists(locInstallerPath)) File.Delete(locInstallerPath);

            FileInfo installerInfo = new(installerLocation);
            installerInfo.CopyTo(locInstallerPath);
        }

        string gitVersionPath = Path.Combine(Program.PapersDir, Program.GitFolderName, "version");
        if (File.Exists(gitVersionPath)) {
            string versionPath = Path.Combine(Program.PapersDir, "psp-mod-version");
            
            if (File.Exists(versionPath)) File.Delete(versionPath);

            FileInfo versionInfo = new(gitVersionPath);
            versionInfo.CopyTo(versionPath);
        }

        this.SetProgress(1075);

        this.PatchAssets();
    }

    private void Finish() {
        this.SetProgress(1100);
        this.cont.Invoke(new Action(() => this.cont.Enabled = true));
        this.Log("\n\n~~~~~~~~~~~\nFinished!");
    }
    
    private void OnNetFinish(object sender, EventArgs e) {
        this.SetProgress(600);

        // Skip first BepInEx run step on update
        if (this.update) this.Restore();
        else this.Run();
    }

    private void OnRestoreFinish(object sender, EventArgs e) {
        this.SetProgress(800);
        this.Compile();
    }

    private void OnCompileFinish(object sender, EventArgs e) {
        this.SetProgress(900);
        this.Install();
    }

    private void LogProcOutput(object sender, DataReceivedEventArgs e) {
        if (e?.Data == null) return;
        this.Log($"{e.Data}");
    }

    private void NetProcOutput(object sender, DataReceivedEventArgs e) {
        if (e?.Data == null) return;
        Match match = Regex.Match(e.Data, dotNetPathPattern);
        if (match.Success) this.dotNetDir = match.Groups[1].Value;

        this.Log($"{e.Data}");
    }

    private void CompileProcOutput(object sender, DataReceivedEventArgs e) {
        if (e?.Data == null) return;
        this.Log($"{e.Data}");
    }

    private void ErrorOutput(object sender, DataReceivedEventArgs e) {
        if (e?.Data == null) return;
        this.Log($"ERROR: {e.Data}");
    }

    private void cont_Click(object sender, EventArgs e) {
        this.Hide();

        Config cfg = new Config();
        cfg.Dock = DockStyle.Fill;
        cfg.Parent = Program.window;
        cfg.Show();
    }
}