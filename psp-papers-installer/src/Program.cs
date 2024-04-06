using System;
using System.IO;
using System.Windows.Forms;

namespace psp_papers_installer {

    static class Program {

        internal static Window window;
        internal static string PapersDir;
        internal static string latestVersion;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Window window = new Window();

            Program.window = window;

            Welcome welcome = new Welcome();
            welcome.Dock = DockStyle.Fill;
            welcome.Parent = window;
            welcome.Show();

            Application.Run(window);
        }

        internal static bool AlreadyInstalled() {
            return AlreadyInstalled(PapersDir);
        }

        internal static bool AlreadyInstalled(string path) {
            return File.Exists(Path.Combine(path, "papers-main", "version"));
        }

        internal static string InstalledVersion() {
            return InstalledVersion(PapersDir);
        }

        internal static string InstalledVersion(string path) {
            string versionPath = Path.Combine(path, "papers-main", "version");
            if (!File.Exists(versionPath)) return "None";
            return File.ReadAllText(versionPath);
        }

    }

}