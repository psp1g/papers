using System.ComponentModel;

namespace psp_papers_installer {
    partial class Installing {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            title = new System.Windows.Forms.Label();
            log = new System.Windows.Forms.RichTextBox();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            cont = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // title
            // 
            title.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            title.Location = new System.Drawing.Point(0, 10);
            title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            title.Name = "title";
            title.Size = new System.Drawing.Size(685, 49);
            title.TabIndex = 2;
            title.Text = "Installing Mod...";
            title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // log
            // 
            log.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            log.Location = new System.Drawing.Point(14, 95);
            log.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            log.Name = "log";
            log.ReadOnly = true;
            log.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            log.Size = new System.Drawing.Size(656, 393);
            log.TabIndex = 3;
            log.Text = "";
            log.UseWaitCursor = true;
            // 
            // progressBar1
            // 
            progressBar1.Location = new System.Drawing.Point(14, 494);
            progressBar1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(656, 27);
            progressBar1.TabIndex = 4;
            // 
            // cont
            // 
            cont.Enabled = false;
            cont.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            cont.Location = new System.Drawing.Point(541, 551);
            cont.Name = "cont";
            cont.Size = new System.Drawing.Size(130, 57);
            cont.TabIndex = 5;
            cont.Text = "Continue";
            cont.UseVisualStyleBackColor = true;
            cont.Click += cont_Click;
            // 
            // Installing
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(cont);
            Controls.Add(progressBar1);
            Controls.Add(log);
            Controls.Add(title);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximumSize = new System.Drawing.Size(685, 625);
            MinimumSize = new System.Drawing.Size(685, 625);
            Name = "Installing";
            Size = new System.Drawing.Size(685, 625);
            ResumeLayout(false);
        }

        private System.Windows.Forms.RichTextBox log;
        private System.Windows.Forms.ProgressBar progressBar1;

        private System.Windows.Forms.Label title;

        private System.Windows.Forms.Button cont;

        #endregion
    }
}