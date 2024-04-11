using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace psp_papers_installer;

public partial class Finished : UserControl {

    public Finished() {
        this.InitializeComponent();
    }

    private void play_Click(object sender, EventArgs e) {
        Program.RunGame();
        Application.Exit();
    }

}