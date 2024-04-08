using System.ComponentModel;

namespace psp_papers_installer {
    partial class Config {
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
            label5 = new System.Windows.Forms.Label();
            error = new System.Windows.Forms.Label();
            cont = new System.Windows.Forms.Button();
            cfgEditor = new System.Windows.Forms.RichTextBox();
            label2 = new System.Windows.Forms.Label();
            openFolder = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.Location = new System.Drawing.Point(0, 15);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(685, 43);
            label1.TabIndex = 2;
            label1.Text = "Configuration";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label5.Location = new System.Drawing.Point(0, 506);
            label5.Name = "label5";
            label5.Padding = new System.Windows.Forms.Padding(8);
            label5.Size = new System.Drawing.Size(682, 42);
            label5.TabIndex = 10;
            label5.Text = "Note: Config can be edited anytime in PapersPlease\\BepInEx\\config\\wtf.psp.papers.cfg";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // error
            // 
            error.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            error.ForeColor = System.Drawing.Color.Crimson;
            error.Location = new System.Drawing.Point(13, 300);
            error.Name = "error";
            error.Size = new System.Drawing.Size(644, 23);
            error.TabIndex = 11;
            error.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cont
            // 
            cont.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            cont.Location = new System.Drawing.Point(541, 551);
            cont.Name = "cont";
            cont.Size = new System.Drawing.Size(130, 57);
            cont.TabIndex = 17;
            cont.Text = "Continue";
            cont.UseVisualStyleBackColor = true;
            cont.Click += cont_Click;
            // 
            // cfgEditor
            // 
            cfgEditor.Location = new System.Drawing.Point(13, 87);
            cfgEditor.Name = "cfgEditor";
            cfgEditor.Size = new System.Drawing.Size(658, 416);
            cfgEditor.TabIndex = 18;
            cfgEditor.Text = "";
            // 
            // label2
            // 
            label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(-11, 58);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(696, 26);
            label2.TabIndex = 19;
            label2.Text = "Twitch Auth keys need to be configured for the mod to work";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openFolder
            // 
            openFolder.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            openFolder.Location = new System.Drawing.Point(405, 551);
            openFolder.Name = "openFolder";
            openFolder.Size = new System.Drawing.Size(130, 57);
            openFolder.TabIndex = 20;
            openFolder.Text = "Open Folder";
            openFolder.UseVisualStyleBackColor = true;
            openFolder.Click += openFolder_Click;
            // 
            // Config
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(openFolder);
            Controls.Add(label2);
            Controls.Add(cfgEditor);
            Controls.Add(cont);
            Controls.Add(error);
            Controls.Add(label5);
            Controls.Add(label1);
            Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MaximumSize = new System.Drawing.Size(685, 625);
            MinimumSize = new System.Drawing.Size(685, 625);
            Name = "Config";
            Size = new System.Drawing.Size(685, 625);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Label error;

        private System.Windows.Forms.Label label5;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Button cont;

        #endregion

        private System.Windows.Forms.RichTextBox cfgEditor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button openFolder;
    }
}