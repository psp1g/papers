using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace psp_papers_installer {

    public partial class Finished : UserControl {

        public Finished() {
            this.InitializeComponent();
        }

        private void play_Click(object sender, EventArgs e) {
            Process.Start(Path.Combine(Program.PapersDir, "PapersPlease.exe"));
            Application.Exit();
        }

    }

}