using System;
using System.IO;
using System.Windows.Forms;

namespace psp_papers_installer {

    static class Program {

        internal static Window window;
        internal static string PapersDir;

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
            return File.Exists(Path.Combine(path, "BepInEx", "plugins", "psp-papers-mod.dll")) ||
                   File.Exists(Path.Combine(path, "psp-paper-mod.zip")) ||
                   Directory.Exists(Path.Combine(path, "papers-main")) ||
                   Directory.Exists(Path.Combine(path, "img_patch"));
        }

    }

}