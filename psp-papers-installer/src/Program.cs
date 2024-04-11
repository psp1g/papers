using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Diagnostics;
using System.Windows.Forms;

namespace psp_papers_installer;

static class Program {

    internal static Window window;
    internal static string PapersDir;
    internal static string latestVersion;

    internal static readonly HttpClient client = new();

    internal static string SteamLocation { get; private set; }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        Program.SteamLocation = FindSteam();

        Window win = new();

        Program.window = win;

        Welcome welcome = new();
        welcome.Dock = DockStyle.Fill;
        welcome.Parent = win;
        welcome.Show();

        Application.Run(win);
    }

    static string FindSteam() {
        string usualPath = "C:\\Program Files (x86)\\Steam\\steam.exe";
        if (File.Exists(usualPath)) return usualPath;
        return null;
    }

    internal static bool AlreadyInstalled() {
        return AlreadyInstalled(PapersDir);
    }

    internal static bool AlreadyInstalled(string path) {
        return File.Exists(Path.Combine(path, "papers-main", "version"));
    }

    internal static string InstalledVersion(string path) {
        string versionPath = Path.Combine(path, "papers-main", "version");
        return File.Exists(versionPath) ? File.ReadAllText(versionPath) : "None";
    }

    internal static Process RunGame(bool runDirect = false) {
        return Program.SteamLocation == null || runDirect ?
            // Run the game directly, but steam (if installed with it) will force restart the game with steam
            Process.Start(Path.Combine(Program.PapersDir, "PapersPlease.exe")) :
            // Run the game through steam
            // https://steamdb.info/app/239030/
            Process.Start(Program.SteamLocation, "steam://rungameid/239030");
    }

    internal static void CloseGame() {
        Process.GetProcessesByName("PapersPlease")
            .FirstOrDefault()?.CloseMainWindow();
    }

}