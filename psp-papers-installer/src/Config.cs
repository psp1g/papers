using System;
using System.IO;
using System.Windows.Forms;

namespace psp_papers_installer;

public partial class Config : UserControl {

    public Config() {
        this.InitializeComponent();
    }

    private void cont_Click(object sender, EventArgs e) {
        string cfgPath = Path.Combine(Program.PapersDir, "BepInEx", "config", "wtf.psp.papers.cfg");
        File.WriteAllText(cfgPath, this.cfgEditor.Text);

        this.Hide();

        Finished fin = new();
        fin.Dock = DockStyle.Fill;
        fin.Parent = Program.window;
        fin.Show();
    }

    private void openFolder_Click(object sender, EventArgs e) {
        System.Diagnostics.Process.Start("explorer.exe", Path.Combine(Program.PapersDir, "BepInEx", "config"));
    }

    private void show_Click(object sender, EventArgs e) {
        this.show.Visible = false;
        this.showLabel.Visible = false;
        this.cfgEditor.ReadOnly = false;

        this.note.Visible = true;
        this.cont.Enabled = true;
        this.openFolder.Enabled = true;

        string cfgPath = Path.Combine(Program.PapersDir, "BepInEx", "config", "wtf.psp.papers.cfg");
        this.cfgEditor.Text = File.ReadAllText(cfgPath);
    }

}