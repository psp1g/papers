using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace psp_papers_installer;

public partial class Welcome : UserControl  {

    private const string UsualPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\PapersPlease";
    private const string RemoteVersion = $"https://raw.githubusercontent.com/psp1g/papers/{Program.Branch}/version";

    private bool upToDate;
    private bool clickedOnce;

    public Welcome() {
        this.InitializeComponent();

        this.currentVersion.Text = Program.InstalledVersion(UsualPath);

        try {
            Program.client.GetAsync(RemoteVersion)
                .ContinueWith(async res => {
                    if (!res.IsCompletedSuccessfully) {
                        this.latestVersion.Invoke(() => this.latestVersion.Text = @"??");
                        Program.Console.Error("Failed to get latest version!");
                        return;
                    }

                    string version = await res.Result.Content.ReadAsStringAsync();
                    Program.Console.WriteLine($"{version}");

                    string latestText = Program.Branch == "main" ? "Latest" : Program.Branch;
                    
                    this.latestVersion.Invoke(() => {
                        this.latestVersion.Text = Program.Branch == "main" ? version : $"{version} ({Program.Branch})";
                        Program.latestVersion = version;

                        this.checkBox3.Text = $@"PSP Papers Mod - {latestText} @{version}";

                        this.upToDate = this.latestVersion.Text.Trim() == this.currentVersion.Text.Trim();
                        this.papersPath_TextChanged(null, null);
                    });
                });
        } catch (Exception e) {
            this.latestVersion.Text = @"??";
            Program.Console.Error($"{e.Message}\n{e.StackTrace}");
        }

        string exePath = Path.Combine(UsualPath, "PapersPlease.exe");
        string dataPath = Path.Combine(UsualPath, "PapersPlease_Data");

        if (!File.Exists(exePath) && !File.Exists(dataPath)) return;

        this.papersPath.Text = UsualPath;
        this.papersPath_TextChanged(null, null);
        if (!Program.AlreadyInstalled(UsualPath))
            this.pathStatus.Text = @"Detected Path";
    }

    private void browse_Click(object sender, EventArgs e) {
        using (FolderBrowserDialog dialog = new FolderBrowserDialog()) {
            dialog.ShowNewFolderButton = false;
            DialogResult result = dialog.ShowDialog();

            if (result != DialogResult.OK) return;

            string exePath = Path.Combine(dialog.SelectedPath, "PapersPlease.exe");
            string dataPath = Path.Combine(dialog.SelectedPath, "PapersPlease_Data");

            if (!File.Exists(exePath)) {
                MessageBox.Show(@"Couldn't find PapersPlease.exe in the directory you selected!", @"Papers, Please not found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(dataPath)) {
                MessageBox.Show(@"You need the most up-to-date (Unity!) version of the game from Steam for this mod. PapersPlease_Data could not be found!", @"Incorrect Papers, Please version", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.papersPath.Text = dialog.SelectedPath;
        }
    }

    private void papersPath_TextChanged(object sender, EventArgs e) {
        string path = this.papersPath.Text.Trim();

        if (path == "") {
            this.pathStatus.Text = "";
            this.cont.Enabled = false;
            return;
        }

        string exePath = Path.Combine(path, "PapersPlease.exe");
        string dataPath = Path.Combine(path, "PapersPlease_Data");

        this.cont.Text = @"Install";
        this.toInstall.Text = @"To Install";
        this.currentVersion.Text = @"None";
        this.netsdk.Checked = true;
        this.bepInEx.Checked = true;
        this.clickedOnce = false;

        if (!File.Exists(exePath)) {
            this.pathStatus.Text = @"PapersPlease.exe couldn't be found!";
            this.pathStatus.ForeColor = Color.Crimson;
            this.cont.Enabled = false;
            return;
        }

        if (!Directory.Exists(dataPath)) {
            this.pathStatus.Text = @"Incorrect Game Version! (Unity Version Required)";
            this.pathStatus.ForeColor = Color.Crimson;
            this.cont.Enabled = false;
            return;
        }

        if (Program.AlreadyInstalled(path)) {
            this.cont.Text = this.upToDate ? "Up to Date" : "Update";

            this.toInstall.Text = @"To Update";
            this.pathStatus.Text = @"Mod Already Installed";
            this.currentVersion.Text = Program.InstalledVersion(path);

            this.pathStatus.ForeColor = Color.Teal;

            this.cont.Enabled = true;
            this.netsdk.Checked = false;
            this.bepInEx.Checked = false;
            return;
        }

        this.pathStatus.Text = @"Looks good!";
        this.pathStatus.ForeColor = Color.LimeGreen;
        this.cont.Enabled = true;
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
        System.Diagnostics.Process.Start("explorer.exe", "https://github.com/psp1g/papers");
    }

    private void cont_Click(object sender, EventArgs e) {
        string path = this.papersPath.Text.Trim();

        string bepPath = Path.Combine(path, "BepInEx");

        if (this.upToDate && !this.clickedOnce) {
            this.clickedOnce = true;
            this.cont.Text = @"Re-install anyway?";
            return;
        }

        if (Directory.Exists(bepPath) && !Program.AlreadyInstalled(path)) {
            DialogResult result = MessageBox.Show(@"BepInEx seems to be installed already! Would you like to continue installing and replace with BepInEx 6?", @"BepInEx already installed", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (result != DialogResult.Yes) return;
        }

        this.Hide();

        Program.PapersDir = this.papersPath.Text.Trim();

        Installing installing = new Installing();
        installing.Dock = DockStyle.Fill;
        installing.Parent = Program.window;
        installing.Show();
    }

}