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
        private void InitializeComponent() {
            this.title = new System.Windows.Forms.Label();
            this.log = new System.Windows.Forms.RichTextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.@continue = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // title
            //
            this.title.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(12, 9);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(645, 43);
            this.title.TabIndex = 2;
            this.title.Text = "Installing Mod...";
            //
            // log
            //
            this.log.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.log.Location = new System.Drawing.Point(12, 82);
            this.log.Name = "log";
            this.log.ReadOnly = true;
            this.log.Size = new System.Drawing.Size(645, 374);
            this.log.TabIndex = 3;
            this.log.Text = "";
            //
            // progressBar1
            //
            this.progressBar1.Location = new System.Drawing.Point(12, 462);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(645, 23);
            this.progressBar1.TabIndex = 4;
            //
            // continue
            //
            this.@continue.Enabled = false;
            this.@continue.Location = new System.Drawing.Point(531, 529);
            this.@continue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.@continue.Name = "continue";
            this.@continue.Size = new System.Drawing.Size(126, 44);
            this.@continue.TabIndex = 7;
            this.@continue.Text = "Continue";
            this.@continue.UseVisualStyleBackColor = true;
            this.@continue.Click += new System.EventHandler(this.continue_Click);
            //
            // Installing
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.@continue);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.log);
            this.Controls.Add(this.title);
            this.MaximumSize = new System.Drawing.Size(685, 625);
            this.MinimumSize = new System.Drawing.Size(685, 625);
            this.Name = "Installing";
            this.Size = new System.Drawing.Size(685, 625);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button @continue;

        private System.Windows.Forms.RichTextBox log;
        private System.Windows.Forms.ProgressBar progressBar1;

        private System.Windows.Forms.Label title;

        #endregion
    }
}