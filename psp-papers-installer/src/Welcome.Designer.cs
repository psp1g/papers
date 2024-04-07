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
        private void InitializeComponent()
        {
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            linkLabel1 = new System.Windows.Forms.LinkLabel();
            toInstall = new System.Windows.Forms.GroupBox();
            checkBox3 = new System.Windows.Forms.CheckBox();
            bepInEx = new System.Windows.Forms.CheckBox();
            netsdk = new System.Windows.Forms.CheckBox();
            papersPath = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            browse = new System.Windows.Forms.Button();
            label4 = new System.Windows.Forms.Label();
            pathStatus = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            currentVersion = new System.Windows.Forms.Label();
            latestVersion = new System.Windows.Forms.Label();
            cont = new System.Windows.Forms.Button();
            toInstall.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.Location = new System.Drawing.Point(0, 17);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(685, 41);
            label1.TabIndex = 1;
            label1.Text = "PSP PAPERS PLEASE MOD INSTALLER";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(0, 92);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(685, 28);
            label2.TabIndex = 3;
            label2.Text = "This will install the PSP Papers Please Twitch integration mod and its dependencies.";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel1
            // 
            linkLabel1.Location = new System.Drawing.Point(13, 584);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new System.Drawing.Size(90, 25);
            linkLabel1.TabIndex = 7;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Source Code";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // toInstall
            // 
            toInstall.Controls.Add(checkBox3);
            toInstall.Controls.Add(bepInEx);
            toInstall.Controls.Add(netsdk);
            toInstall.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            toInstall.Location = new System.Drawing.Point(33, 147);
            toInstall.Name = "toInstall";
            toInstall.Size = new System.Drawing.Size(624, 144);
            toInstall.TabIndex = 6;
            toInstall.TabStop = false;
            toInstall.Text = "To Install";
            // 
            // checkBox3
            // 
            checkBox3.AutoCheck = false;
            checkBox3.Checked = true;
            checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBox3.Cursor = System.Windows.Forms.Cursors.No;
            checkBox3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            checkBox3.Location = new System.Drawing.Point(17, 73);
            checkBox3.Margin = new System.Windows.Forms.Padding(0);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new System.Drawing.Size(336, 24);
            checkBox3.TabIndex = 3;
            checkBox3.Text = "PSP Papers Mod - Latest";
            checkBox3.UseVisualStyleBackColor = true;
            // 
            // bepInEx
            // 
            bepInEx.AutoCheck = false;
            bepInEx.Checked = true;
            bepInEx.CheckState = System.Windows.Forms.CheckState.Checked;
            bepInEx.Cursor = System.Windows.Forms.Cursors.No;
            bepInEx.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            bepInEx.Location = new System.Drawing.Point(17, 49);
            bepInEx.Margin = new System.Windows.Forms.Padding(0);
            bepInEx.Name = "bepInEx";
            bepInEx.Size = new System.Drawing.Size(336, 24);
            bepInEx.TabIndex = 2;
            bepInEx.Text = "BepInEx 6 BE";
            bepInEx.UseVisualStyleBackColor = true;
            // 
            // netsdk
            // 
            netsdk.AutoCheck = false;
            netsdk.Checked = true;
            netsdk.CheckState = System.Windows.Forms.CheckState.Checked;
            netsdk.Cursor = System.Windows.Forms.Cursors.No;
            netsdk.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            netsdk.Location = new System.Drawing.Point(17, 25);
            netsdk.Margin = new System.Windows.Forms.Padding(0);
            netsdk.Name = "netsdk";
            netsdk.Size = new System.Drawing.Size(336, 24);
            netsdk.TabIndex = 1;
            netsdk.Text = ".NET 6 Core SDK";
            netsdk.UseVisualStyleBackColor = true;
            // 
            // papersPath
            // 
            papersPath.Location = new System.Drawing.Point(33, 374);
            papersPath.Name = "papersPath";
            papersPath.Size = new System.Drawing.Size(624, 25);
            papersPath.TabIndex = 4;
            papersPath.TextChanged += papersPath_TextChanged;
            // 
            // label3
            // 
            label3.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label3.Location = new System.Drawing.Point(33, 348);
            label3.Margin = new System.Windows.Forms.Padding(0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(221, 23);
            label3.TabIndex = 8;
            label3.Text = "Path to Papers, Please";
            // 
            // browse
            // 
            browse.Location = new System.Drawing.Point(567, 402);
            browse.Margin = new System.Windows.Forms.Padding(0);
            browse.Name = "browse";
            browse.Size = new System.Drawing.Size(90, 36);
            browse.TabIndex = 5;
            browse.Text = "Browse";
            browse.UseVisualStyleBackColor = true;
            browse.Click += browse_Click;
            // 
            // label4
            // 
            label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label4.Location = new System.Drawing.Point(361, 352);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(270, 19);
            label4.TabIndex = 10;
            label4.Text = "The updated Steam/Unity version is required";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pathStatus
            // 
            pathStatus.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            pathStatus.Location = new System.Drawing.Point(33, 402);
            pathStatus.Name = "pathStatus";
            pathStatus.Size = new System.Drawing.Size(270, 19);
            pathStatus.TabIndex = 11;
            pathStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label5.Location = new System.Drawing.Point(500, 506);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(157, 19);
            label5.TabIndex = 12;
            label5.Text = "Latest Version:";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            label6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label6.Location = new System.Drawing.Point(500, 487);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(157, 19);
            label6.TabIndex = 13;
            label6.Text = "Currently Installed:";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // currentVersion
            // 
            currentVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            currentVersion.Location = new System.Drawing.Point(623, 487);
            currentVersion.Name = "currentVersion";
            currentVersion.Size = new System.Drawing.Size(48, 19);
            currentVersion.TabIndex = 14;
            currentVersion.Text = "1.1.1";
            currentVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // latestVersion
            // 
            latestVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            latestVersion.Location = new System.Drawing.Point(623, 506);
            latestVersion.Name = "latestVersion";
            latestVersion.Size = new System.Drawing.Size(48, 19);
            latestVersion.TabIndex = 15;
            latestVersion.Text = "1.1.1";
            latestVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cont
            // 
            cont.Enabled = false;
            cont.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            cont.Location = new System.Drawing.Point(541, 551);
            cont.Name = "cont";
            cont.Size = new System.Drawing.Size(130, 57);
            cont.TabIndex = 16;
            cont.Text = "Continue";
            cont.UseVisualStyleBackColor = true;
            cont.Click += cont_Click;
            // 
            // Welcome
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(cont);
            Controls.Add(latestVersion);
            Controls.Add(currentVersion);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(pathStatus);
            Controls.Add(label4);
            Controls.Add(browse);
            Controls.Add(label3);
            Controls.Add(papersPath);
            Controls.Add(toInstall);
            Controls.Add(linkLabel1);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            MaximumSize = new System.Drawing.Size(685, 625);
            MinimumSize = new System.Drawing.Size(685, 625);
            Name = "Welcome";
            Size = new System.Drawing.Size(685, 625);
            toInstall.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Button cont;

        #endregion
    }
}