using System.ComponentModel;

namespace psp_papers_installer {
    partial class Welcome {
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
            this.label1 = new System.Windows.Forms.Label();
            this.@continue = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.toInstall = new System.Windows.Forms.GroupBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.bepInEx = new System.Windows.Forms.CheckBox();
            this.netsdk = new System.Windows.Forms.CheckBox();
            this.papersPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.browse = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.pathStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.currentVersion = new System.Windows.Forms.Label();
            this.latestVersion = new System.Windows.Forms.Label();
            this.toInstall.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(645, 41);
            this.label1.TabIndex = 1;
            this.label1.Text = "PSP PAPERS PLEASE MOD INSTALLER";
            // 
            // continue
            // 
            this.@continue.BackColor = System.Drawing.SystemColors.Control;
            this.@continue.Enabled = false;
            this.@continue.Location = new System.Drawing.Point(531, 529);
            this.@continue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.@continue.Name = "continue";
            this.@continue.Size = new System.Drawing.Size(126, 44);
            this.@continue.TabIndex = 6;
            this.@continue.Text = "Install";
            this.@continue.UseVisualStyleBackColor = false;
            this.@continue.Click += new System.EventHandler(this.continue_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(33, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(673, 28);
            this.label2.TabIndex = 3;
            this.label2.Text = "This will install the PSP Papers Please Twitch integration mod and its dependenci" + "es.";
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(14, 552);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(125, 25);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Source Code";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // toInstall
            // 
            this.toInstall.Controls.Add(this.checkBox3);
            this.toInstall.Controls.Add(this.bepInEx);
            this.toInstall.Controls.Add(this.netsdk);
            this.toInstall.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toInstall.Location = new System.Drawing.Point(33, 147);
            this.toInstall.Name = "toInstall";
            this.toInstall.Size = new System.Drawing.Size(598, 144);
            this.toInstall.TabIndex = 6;
            this.toInstall.TabStop = false;
            this.toInstall.Text = "To Install";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoCheck = false;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Cursor = System.Windows.Forms.Cursors.No;
            this.checkBox3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox3.Location = new System.Drawing.Point(17, 73);
            this.checkBox3.Margin = new System.Windows.Forms.Padding(0);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(336, 24);
            this.checkBox3.TabIndex = 3;
            this.checkBox3.Text = "PSP Papers Mod - Latest";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // bepInEx
            // 
            this.bepInEx.AutoCheck = false;
            this.bepInEx.Checked = true;
            this.bepInEx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bepInEx.Cursor = System.Windows.Forms.Cursors.No;
            this.bepInEx.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bepInEx.Location = new System.Drawing.Point(17, 49);
            this.bepInEx.Margin = new System.Windows.Forms.Padding(0);
            this.bepInEx.Name = "bepInEx";
            this.bepInEx.Size = new System.Drawing.Size(336, 24);
            this.bepInEx.TabIndex = 2;
            this.bepInEx.Text = "BepInEx 6 BE";
            this.bepInEx.UseVisualStyleBackColor = true;
            // 
            // netsdk
            // 
            this.netsdk.AutoCheck = false;
            this.netsdk.Checked = true;
            this.netsdk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.netsdk.Cursor = System.Windows.Forms.Cursors.No;
            this.netsdk.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.netsdk.Location = new System.Drawing.Point(17, 25);
            this.netsdk.Margin = new System.Windows.Forms.Padding(0);
            this.netsdk.Name = "netsdk";
            this.netsdk.Size = new System.Drawing.Size(336, 24);
            this.netsdk.TabIndex = 1;
            this.netsdk.Text = ".NET 6 Core SDK";
            this.netsdk.UseVisualStyleBackColor = true;
            // 
            // papersPath
            // 
            this.papersPath.Location = new System.Drawing.Point(33, 374);
            this.papersPath.Name = "papersPath";
            this.papersPath.Size = new System.Drawing.Size(598, 25);
            this.papersPath.TabIndex = 4;
            this.papersPath.TextChanged += new System.EventHandler(this.papersPath_TextChanged);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(33, 348);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(221, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "Path to Papers, Please";
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(541, 402);
            this.browse.Margin = new System.Windows.Forms.Padding(0);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(90, 36);
            this.browse.TabIndex = 5;
            this.browse.Text = "Browse";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.browse_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(361, 352);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(270, 19);
            this.label4.TabIndex = 10;
            this.label4.Text = "The updated Steam/Unity version is required";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pathStatus
            // 
            this.pathStatus.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathStatus.Location = new System.Drawing.Point(33, 402);
            this.pathStatus.Name = "pathStatus";
            this.pathStatus.Size = new System.Drawing.Size(270, 19);
            this.pathStatus.TabIndex = 11;
            this.pathStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(500, 506);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 19);
            this.label5.TabIndex = 12;
            this.label5.Text = "Latest Version:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(500, 487);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(157, 19);
            this.label6.TabIndex = 13;
            this.label6.Text = "Currently Installed:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // currentVersion
            // 
            this.currentVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentVersion.Location = new System.Drawing.Point(609, 487);
            this.currentVersion.Name = "currentVersion";
            this.currentVersion.Size = new System.Drawing.Size(48, 19);
            this.currentVersion.TabIndex = 14;
            this.currentVersion.Text = "1.1.1";
            this.currentVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // latestVersion
            // 
            this.latestVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.latestVersion.Location = new System.Drawing.Point(609, 506);
            this.latestVersion.Name = "latestVersion";
            this.latestVersion.Size = new System.Drawing.Size(48, 19);
            this.latestVersion.TabIndex = 15;
            this.latestVersion.Text = "1.1.1";
            this.latestVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Welcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.latestVersion);
            this.Controls.Add(this.currentVersion);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pathStatus);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.browse);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.papersPath);
            this.Controls.Add(this.toInstall);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.@continue);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(685, 625);
            this.MinimumSize = new System.Drawing.Size(685, 625);
            this.Name = "Welcome";
            this.Size = new System.Drawing.Size(685, 625);
            this.toInstall.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label currentVersion;
        private System.Windows.Forms.Label latestVersion;

        private System.Windows.Forms.Label pathStatus;

        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.TextBox papersPath;
        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.CheckBox netsdk;
        private System.Windows.Forms.CheckBox bepInEx;
        private System.Windows.Forms.CheckBox checkBox3;

        private System.Windows.Forms.GroupBox toInstall;

        private System.Windows.Forms.LinkLabel linkLabel1;

        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.Button @continue;

        private System.Windows.Forms.Label label1;

        #endregion
    }
}