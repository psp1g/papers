using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace psp_papers_installer {
    // Gross code
    // Just wanted it to work without anything fancy
    // Its already a windows form app (cope)
    public partial class Installing : UserControl {
        private const string Git = "https://github.com/psp1g/papers/archive/refs/heads/main.zip";

        private const string BepInEx =
            "https://builds.bepinex.dev/projects/bepinex_be/688/BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.688%2B4901521.zip";

        private const string dotNetInstallScript =
            "https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.ps1";

        private const string dotNetPathPattern = @"Adding to current process PATH: ""(.+)""\.$";

        private const int totalSteps = 1100;

        private string dotNetDir = "";

        private int[] dlSteps = { 0, 0, 0 };

        private bool extract = false;

        private bool update = false;

        public Installing() {
            this.InitializeComponent();

            if (Program.AlreadyInstalled()) {
                this.update = true;
                this.log.AppendText($"Updating PSP Papers Please to @{Program.latestVersion}\n");
                this.title.Text = "Updating Mod...";
            }
            else
                this.log.AppendText($"Starting install of PSP Papers Please @{Program.latestVersion}\n");

            Thread installerThread = new Thread(this.Download);
            installerThread.Start();
        }

        private void SetProgress(int prog) {
            double pct = prog / (totalSteps * 1d);
            this.progressBar1.Invoke(new Action(() =>
                this.progressBar1.Value = (int)(pct * 100)
            ));
        }

        private void Log(string message) {
            this.log.Invoke(new Action(() => this.log.AppendText($"{message}\n")));
        }

        private void Download() {
            // Remove old mod files
            if (this.update) {
                File.Delete(Path.Combine(Program.PapersDir, "psp-paper-mod.zip"));

                if (Directory.Exists(Path.Combine(Program.PapersDir, "papers-main")))
                    Directory.Delete(Path.Combine(Program.PapersDir, "papers-main"), true);

                if (Directory.Exists(Path.Combine(Program.PapersDir, "BepInEx", "plugins"))) {
                    Directory.Delete(Path.Combine(Program.PapersDir, "BepInEx", "plugins"), true);
                    Directory.CreateDirectory(Path.Combine(Program.PapersDir, "BepInEx", "plugins"));
                }
            }
            else {
                // If not updating, remove existing BepInEx stuff if any
                if (Directory.Exists(Path.Combine(Program.PapersDir, "BepInEx")))
                    Directory.Delete(Path.Combine(Program.PapersDir, "BepInEx"), true);
            }

            using (WebClient wc = new WebClient()) {
                this.Log($"Downloading PSP Papers Mod @{Program.latestVersion} - {Git}");

                wc.DownloadProgressChanged += modDlProgress;
                wc.DownloadFileAsync(
                    new Uri(Git),
                    Path.Combine(Program.PapersDir, "psp-paper-mod.zip")
                );
            }

            if (!this.update) {
                using (WebClient wc = new WebClient()) {
                    this.Log($"Downloading BepInEx 6 BE - {BepInEx}");

                    wc.DownloadProgressChanged += bepInExDlProgress;
                    wc.DownloadFileAsync(
                        new Uri(BepInEx),
                        Path.Combine(Program.PapersDir, "bepinex.zip")
                    );
                }

                using (WebClient wc = new WebClient()) {
                    this.Log($"Downloading .NET 6 SDK Install Script - {dotNetInstallScript}");
                    wc.DownloadProgressChanged += dotnetDlProgress;
                    wc.DownloadFileAsync(
                        new Uri(dotNetInstallScript),
                        Path.Combine(Program.PapersDir, "dotnet6.ps1")
                    );
                }
            }
            else {
                this.dlSteps[0] = 100;
                this.dlSteps[2] = 100;
            }
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
            this.Log("Installing .NET 6 SDK");

            ProcessStartInfo startInfo = new ProcessStartInfo {
                FileName = "powershell.exe",
                Arguments =
                    $"-ExecutionPolicy Bypass -WindowStyle hidden -NoLogo -command \"& '{Path.Combine(Program.PapersDir, "dotnet6.ps1")}' -Channel 6.0.1xx\"",

                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process netProc = new Process { StartInfo = startInfo, EnableRaisingEvents = true };

            netProc.Exited += this.onNetFinish;
            netProc.OutputDataReceived += this.netProcOutput;

            netProc.Start();
            netProc.BeginOutputReadLine();
        }

        private void Run() {
            this.Log("Running the game with BepInEx (Generating hollowed assemblies)");
            Process gProc = Process.Start(Path.Combine(Program.PapersDir, "PapersPlease.exe"));

            gProc?.WaitForExit();
            this.SetProgress(650);

            Thread.Sleep(3000);

            this.Log("Closing game");
            Process.GetProcessesByName("PapersPlease").FirstOrDefault()?.CloseMainWindow();

            this.SetProgress(700);
            this.Restore();
        }

        private void Restore() {
            this.Log("Installing C# nuget dependencies");

            string projPath = Path.Combine(Program.PapersDir, "papers-main", "psp-papers-mod", "psp-papers-mod.csproj");
            ProcessStartInfo startInfo = new ProcessStartInfo {
                FileName = Path.Combine(this.dotNetDir, "dotnet.exe"),
                Arguments = $"restore \"{projPath}\"",

                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process netProc = new Process { StartInfo = startInfo, EnableRaisingEvents = true };

            netProc.Exited += this.onRestoreFinish;
            netProc.OutputDataReceived += this.logProcOutput;

            netProc.Start();
            netProc.BeginOutputReadLine();
        }

        private void Compile() {
            this.Log("Compiling psp-papers-mod.csproj");

            string projPath = Path.Combine(Program.PapersDir, "papers-main", "psp-papers-mod", "psp-papers-mod.csproj");
            ProcessStartInfo startInfo = new ProcessStartInfo {
                FileName = Path.Combine(this.dotNetDir, "dotnet.exe"),
                Arguments = $"msbuild \"{projPath}\" -p:PapersPleaseDir=\"{Program.PapersDir}\"",

                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process netProc = new() { StartInfo = startInfo, EnableRaisingEvents = true };

            netProc.Exited += this.onCompileFinish;
            netProc.OutputDataReceived += this.netProcOutput;

            netProc.Start();
            netProc.BeginOutputReadLine();
        }

        private void Install() {
            this.Log("Installing PSP Papers Mod");

            // Copy built plugin DLLs into plugins folder
            string[] paths = Directory.GetFiles(
                Path.Combine(Program.PapersDir, "papers-main", "psp-papers-mod", "bin", "Debug", "net6.0"),
                "*.dll"
            );
            foreach (string path in paths) {
                FileInfo info = new FileInfo(path);
                info.CopyTo(Path.Combine(Program.PapersDir, "BepInEx", "plugins", info.Name));
            }

            this.SetProgress(950);

            if (Directory.Exists(Path.Combine(Program.PapersDir, "img_patch")))
                Directory.Delete(Path.Combine(Program.PapersDir, "img_patch"), true);

            // Finally, move the image patch folder to the root directory
            Directory.Move(
                Path.Combine(Program.PapersDir, "papers-main", "img_patch"),
                Path.Combine(Program.PapersDir, "img_patch")
            );

            if (!this.update) {
                this.SetProgress(1000);

                this.Log("Generating default configurations..");

                // lol
                Process gProc = Process.Start(Path.Combine(Program.PapersDir, "PapersPlease.exe"));
                gProc?.WaitForExit();
                Thread.Sleep(1000);
                this.Log("Closing game");
                Process.GetProcessesByName("PapersPlease").FirstOrDefault()?.CloseMainWindow();
            }

            this.SetProgress(1050);

            ProcessModule processModule = Process.GetCurrentProcess().MainModule;

            if (processModule != null) {
                this.Log("Moving installer to game files");
                string installerLocation = processModule.FileName;
                string locInstallerPath = Path.Combine(Program.PapersDir, "PspPapersInstaller.exe");

                if (File.Exists(locInstallerPath)) File.Delete(locInstallerPath);

                FileInfo installerInfo = new FileInfo(installerLocation);
                installerInfo.CopyTo(locInstallerPath);
            }

            this.SetProgress(1100);

            this.cont.Invoke(new Action(() => this.cont.Enabled = true));
            this.Log("\n\n~~~~~~~~~~~\nFinished!");
        }

        private void onNetFinish(object sender, EventArgs e) {
            this.SetProgress(600);

            // Skip first BepInEx run step on update
            if (this.update) this.Restore();
            else this.Run();
        }

        private void onRestoreFinish(object sender, EventArgs e) {
            this.SetProgress(800);
            this.Compile();
        }

        private void onCompileFinish(object sender, EventArgs e) {
            this.SetProgress(900);
            this.Install();
        }

        private void logProcOutput(object sender, DataReceivedEventArgs e) {
            if (e?.Data == null) return;
            this.Log($"{e.Data}");
        }

        private void netProcOutput(object sender, DataReceivedEventArgs e) {
            if (e?.Data == null) return;
            Match match = Regex.Match(e.Data, dotNetPathPattern);
            if (match.Success) this.dotNetDir = match.Groups[1].Value;

            this.Log($"{e.Data}");
        }

        private void bepInExDlProgress(object sender, DownloadProgressChangedEventArgs e) {
            this.dlSteps[0] = e.ProgressPercentage;
            this.dlProgChanged();
        }

        private void modDlProgress(object sender, DownloadProgressChangedEventArgs e) {
            this.dlSteps[1] = e.ProgressPercentage;
            this.dlProgChanged();
        }

        private void dotnetDlProgress(object sender, DownloadProgressChangedEventArgs e) {
            this.dlSteps[2] = e.ProgressPercentage;
            this.dlProgChanged();
        }

        private void dlProgChanged() {
            if (this.extract) return;

            this.SetProgress(this.dlSteps.Sum());

            if (this.dlSteps.Sum() == this.dlSteps.Length * 100) this.Extract();
        }

        private void cont_Click(object sender, EventArgs e) {
            this.Hide();

            Config cfg = new Config();
            cfg.Dock = DockStyle.Fill;
            cfg.Parent = Program.window;
            cfg.Show();
        }
    }
}