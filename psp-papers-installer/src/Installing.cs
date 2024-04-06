using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
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
            this.progressBar1.Value = (int)(pct * 100);
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

            using (WebClient wc = new WebClient()) {
                this.log.AppendText($"Downloading PSP Papers Mod @{Program.latestVersion} - {Git}\n");
                wc.DownloadProgressChanged += modDlProgress;
                wc.DownloadFileAsync(
                    new Uri(Git),
                    Path.Combine(Program.PapersDir, "psp-paper-mod.zip")
                );
            }

            if (!this.update) {
                using (WebClient wc = new WebClient()) {
                    this.log.AppendText($"Downloading BepInEx 6 BE - {BepInEx}\n");
                    wc.DownloadProgressChanged += bepInExDlProgress;
                    wc.DownloadFileAsync(
                        new Uri(BepInEx),
                        Path.Combine(Program.PapersDir, "bepinex.zip")
                    );
                }

                using (WebClient wc = new WebClient()) {
                    this.log.AppendText($"Downloading .NET 6 SDK Install Script - {dotNetInstallScript}\n");
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
                this.log.AppendText($"Extracting BepInEx 6 BE to {Program.PapersDir}\n");
                ZipFile.ExtractToDirectory(
                    Path.Combine(Program.PapersDir, "bepinex.zip"),
                    Program.PapersDir
                );
                this.SetProgress(400);
                this.log.AppendText("Extracted BepInEx\n");
            }

            this.log.AppendText($"Extracting PSP Papers Mod to {Program.PapersDir}\n");
            ZipFile.ExtractToDirectory(
                Path.Combine(Program.PapersDir, "psp-paper-mod.zip"),
                Program.PapersDir
            );
            this.SetProgress(500);

            this.log.AppendText("Extracted PSP Papers Mod\n");
            this.DotNetInstall();
        }

        private void DotNetInstall() {
            this.log.AppendText("Installing .NET 6 SDK\n");

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
            this.log.AppendText("Running the game with BepInEx (Generating hollowed assemblies)\n");
            Process gProc = Process.Start(Path.Combine(Program.PapersDir, "PapersPlease.exe"));

            gProc?.WaitForExit();
            this.SetProgress(650);

            Thread.Sleep(3000);

            this.log.AppendText("Closing game\n");
            Process.GetProcessesByName("PapersPlease").FirstOrDefault()?.CloseMainWindow();

            this.SetProgress(700);
            this.Restore();
        }

        private void Restore() {
            this.log.AppendText("Installing C# nuget dependencies\n");

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
            this.log.AppendText("Compiling psp-papers-mod.csproj\n");

            string projPath = Path.Combine(Program.PapersDir, "papers-main", "psp-papers-mod", "psp-papers-mod.csproj");
            ProcessStartInfo startInfo = new ProcessStartInfo {
                FileName = Path.Combine(this.dotNetDir, "dotnet.exe"),
                Arguments = $"msbuild \"{projPath}\" -p:PapersPleaseDir=\"{Program.PapersDir}\"",

                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process netProc = new Process { StartInfo = startInfo, EnableRaisingEvents = true };

            netProc.Exited += this.onCompileFinish;
            netProc.OutputDataReceived += this.netProcOutput;

            netProc.Start();
            netProc.BeginOutputReadLine();
        }

        private void Install() {
            this.log.AppendText("Installing PSP Papers Mod\n");

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

                this.log.AppendText("Generating default configurations..\n");

                // lol
                Process gProc = Process.Start(Path.Combine(Program.PapersDir, "PapersPlease.exe"));
                gProc?.WaitForExit();
                Thread.Sleep(1000);
                this.log.AppendText("Closing game\n");
                Process.GetProcessesByName("PapersPlease").FirstOrDefault()?.CloseMainWindow();
            }

            this.SetProgress(1050);

            ProcessModule processModule = Process.GetCurrentProcess().MainModule;

            if (processModule != null) {
                this.log.AppendText("Moving installer to game files\n");
                string installerLocation = processModule.FileName;
                string locInstallerPath = Path.Combine(Program.PapersDir, "PspPapersInstaller.exe");

                if (File.Exists(locInstallerPath)) File.Delete(locInstallerPath);

                FileInfo installerInfo = new FileInfo(installerLocation);
                installerInfo.CopyTo(locInstallerPath);
            }

            this.SetProgress(1100);

            this.cont.Enabled = true;
            this.log.AppendText("\n\n~~~~~~~~~~~\nFinished!");
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
            this.log.AppendText($"{e.Data}\n");
        }

        private void netProcOutput(object sender, DataReceivedEventArgs e) {
            if (e?.Data == null) return;
            Match match = Regex.Match(e.Data, dotNetPathPattern);
            if (match.Success) this.dotNetDir = match.Groups[1].Value;

            this.log.AppendText($"{e.Data}\n");
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