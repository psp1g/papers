using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace psp_papers_installer {
    public partial class Welcome : UserControl  {

        private const string usualPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\PapersPlease";

        public Welcome() {
            InitializeComponent();

            string exePath = Path.Combine(usualPath, "PapersPlease.exe");
            string dataPath = Path.Combine(usualPath, "PapersPlease_Data");

            if (!File.Exists(exePath) && !File.Exists(dataPath)) return;

            this.papersPath.Text = usualPath;
            this.papersPath_TextChanged(null, null);
            if (!Program.AlreadyInstalled(usualPath))
                this.pathStatus.Text = "Detected Path";
        }

        private void browse_Click(object sender, EventArgs e) {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog()) {
                dialog.ShowNewFolderButton = false;
                DialogResult result = dialog.ShowDialog();

                if (result != DialogResult.OK) return;

                string exePath = Path.Combine(dialog.SelectedPath, "PapersPlease.exe");
                string dataPath = Path.Combine(dialog.SelectedPath, "PapersPlease_Data");

                if (!File.Exists(exePath)) {
                    MessageBox.Show("Couldn't find PapersPlease.exe in the directory you selected!", "Papers, Please not found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Directory.Exists(dataPath)) {
                    MessageBox.Show("You need the most up-to-date (Unity!) version of the game from Steam for this mod. PapersPlease_Data could not be found!", "Incorrect Papers, Please version", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.papersPath.Text = dialog.SelectedPath;
            }
        }

        private void papersPath_TextChanged(object sender, EventArgs e) {
            string path = this.papersPath.Text.Trim();

            if (path == "") {
                this.pathStatus.Text = "";
                this.@continue.Enabled = false;
                return;
            }

            string exePath = Path.Combine(path, "PapersPlease.exe");
            string dataPath = Path.Combine(path, "PapersPlease_Data");

            this.@continue.Text = "Install";
            this.toInstall.Text = "To Install";
            this.netsdk.Checked = true;
            this.bepInEx.Checked = true;

            if (!File.Exists(exePath)) {
                this.pathStatus.Text = "PapersPlease.exe couldn't be found!";
                this.pathStatus.ForeColor = Color.Crimson;
                this.@continue.Enabled = false;
                return;
            }

            if (!Directory.Exists(dataPath)) {
                this.pathStatus.Text = "Incorrect Game Version! (Unity Version Required)";
                this.pathStatus.ForeColor = Color.Crimson;
                this.@continue.Enabled = false;
                return;
            }

            if (Program.AlreadyInstalled(path)) {
                this.@continue.Text = "Update";
                this.toInstall.Text = "To Update";
                this.pathStatus.Text = "Mod Already Installed";
                this.pathStatus.ForeColor = Color.Teal;
                this.@continue.Enabled = true;
                this.netsdk.Checked = false;
                this.bepInEx.Checked = false;
                return;
            }

            this.pathStatus.Text = "Looks good!";
            this.pathStatus.ForeColor = Color.LimeGreen;
            this.@continue.Enabled = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe", "https://github.com/psp1g/papers");
        }

        private void continue_Click(object sender, EventArgs e) {
            this.Hide();

            Program.PapersDir = this.papersPath.Text.Trim();

            Installing installing = new Installing();
            installing.Dock = DockStyle.Fill;
            installing.Parent = Program.window;
            installing.Show();
        }

    }
}