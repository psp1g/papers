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
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.twitchChannel = new System.Windows.Forms.TextBox();
            this.botUsername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.botToken = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.@continue = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.error = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(638, 43);
            this.label1.TabIndex = 2;
            this.label1.Text = "Twitch Configuration";
            //
            // label2
            //
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(30, 90);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Twitch Channel";
            //
            // twitchChannel
            //
            this.twitchChannel.Location = new System.Drawing.Point(30, 120);
            this.twitchChannel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.twitchChannel.Name = "twitchChannel";
            this.twitchChannel.Size = new System.Drawing.Size(219, 29);
            this.twitchChannel.TabIndex = 4;
            this.twitchChannel.Text = "psp1g";
            this.twitchChannel.TextChanged += new System.EventHandler(this.onTextChanged);
            //
            // botUsername
            //
            this.botUsername.Location = new System.Drawing.Point(30, 184);
            this.botUsername.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.botUsername.Name = "botUsername";
            this.botUsername.Size = new System.Drawing.Size(219, 29);
            this.botUsername.TabIndex = 6;
            this.botUsername.Text = "ai1g";
            this.botUsername.TextChanged += new System.EventHandler(this.onTextChanged);
            //
            // label3
            //
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(30, 154);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "Bot Username";
            //
            // botToken
            //
            this.botToken.Location = new System.Drawing.Point(30, 248);
            this.botToken.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.botToken.Name = "botToken";
            this.botToken.PasswordChar = '*';
            this.botToken.Size = new System.Drawing.Size(603, 29);
            this.botToken.TabIndex = 8;
            this.botToken.TextChanged += new System.EventHandler(this.onTextChanged);
            //
            // label4
            //
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(30, 218);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 25);
            this.label4.TabIndex = 7;
            this.label4.Text = "Bot Token";
            //
            // continue
            //
            this.@continue.Enabled = false;
            this.@continue.Location = new System.Drawing.Point(531, 529);
            this.@continue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.@continue.Name = "continue";
            this.@continue.Size = new System.Drawing.Size(126, 44);
            this.@continue.TabIndex = 9;
            this.@continue.Text = "Continue";
            this.@continue.UseVisualStyleBackColor = true;
            this.@continue.Click += new System.EventHandler(this.continue_Click);
            //
            // label5
            //
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 332);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(644, 23);
            this.label5.TabIndex = 10;
            this.label5.Text = "Note: Config can be edited anytime in PapersPlease\\BepInEx\\config\\wtf.psp.papers." + "cfg";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // error
            //
            this.error.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.error.ForeColor = System.Drawing.Color.Crimson;
            this.error.Location = new System.Drawing.Point(13, 300);
            this.error.Name = "error";
            this.error.Size = new System.Drawing.Size(644, 23);
            this.error.TabIndex = 11;
            this.error.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // Config
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.error);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.@continue);
            this.Controls.Add(this.botToken);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.botUsername);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.twitchChannel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(685, 625);
            this.MinimumSize = new System.Drawing.Size(685, 625);
            this.Name = "Config";
            this.Size = new System.Drawing.Size(685, 625);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label error;

        private System.Windows.Forms.Button @continue;
        private System.Windows.Forms.Label label5;

        private System.Windows.Forms.TextBox botUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox botToken;
        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox twitchChannel;

        private System.Windows.Forms.Label label1;

        #endregion
    }
}