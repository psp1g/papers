using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace psp_papers_installer {

    public partial class Config : UserControl {

        public Config() {
            this.InitializeComponent();
            this.onTextChanged(null, null);
        }

        private void onTextChanged(object sender, EventArgs e) {
            if (this.twitchChannel.Text.Trim() == "")
                this.error.Text = "Please enter a twitch channel";
            else if (this.botUsername.Text.Trim() == "")
                this.error.Text = "Please enter a bot username";
            else if (this.botToken.Text.Trim() == "")
                this.error.Text = "Please enter a bot token";
            else
                this.error.Text = "";

            this.@continue.Enabled = this.error.Text == "";
        }

        private void continue_Click(object sender, EventArgs e) {
            string cfgPath = Path.Combine(Program.PapersDir, "BepInEx", "config", "wtf.psp.papers.cfg");
            string toml = File.ReadAllText(cfgPath);

            // yah
            string result = Regex.Replace(toml, @"Channel = .+", $"Channel = {this.twitchChannel.Text.Trim()}");
            result = Regex.Replace(result, @"Username = .+", $"Username = {this.botUsername.Text.Trim()}");
            result = Regex.Replace(result, @"Token = .+", $"Token = {this.botToken.Text.Trim()}");

            File.WriteAllText(cfgPath, result);

            this.Hide();

            Finished fin = new Finished();
            fin.Dock = DockStyle.Fill;
            fin.Parent = Program.window;
            fin.Show();
        }

    }

}