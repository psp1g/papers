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
            label2 = new System.Windows.Forms.Label();
            twitchChannel = new System.Windows.Forms.TextBox();
            botUsername = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            botToken = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            error = new System.Windows.Forms.Label();
            cont = new System.Windows.Forms.Button();
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
            label1.Text = "Twitch Configuration";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(30, 90);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(150, 25);
            label2.TabIndex = 3;
            label2.Text = "Twitch Channel";
            // 
            // twitchChannel
            // 
            twitchChannel.Location = new System.Drawing.Point(30, 120);
            twitchChannel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            twitchChannel.Name = "twitchChannel";
            twitchChannel.Size = new System.Drawing.Size(305, 29);
            twitchChannel.TabIndex = 4;
            twitchChannel.Text = "psp1g";
            twitchChannel.TextChanged += onTextChanged;
            // 
            // botUsername
            // 
            botUsername.Location = new System.Drawing.Point(352, 120);
            botUsername.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            botUsername.Name = "botUsername";
            botUsername.Size = new System.Drawing.Size(305, 29);
            botUsername.TabIndex = 6;
            botUsername.Text = "ai1g";
            botUsername.TextChanged += onTextChanged;
            // 
            // label3
            // 
            label3.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label3.Location = new System.Drawing.Point(352, 90);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(150, 25);
            label3.TabIndex = 5;
            label3.Text = "Bot Username";
            // 
            // botToken
            // 
            botToken.Location = new System.Drawing.Point(30, 194);
            botToken.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            botToken.Name = "botToken";
            botToken.PasswordChar = '*';
            botToken.Size = new System.Drawing.Size(627, 29);
            botToken.TabIndex = 8;
            botToken.TextChanged += onTextChanged;
            // 
            // label4
            // 
            label4.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label4.Location = new System.Drawing.Point(30, 164);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(150, 25);
            label4.TabIndex = 7;
            label4.Text = "Bot Token";
            // 
            // label5
            // 
            label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label5.Location = new System.Drawing.Point(13, 332);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(644, 23);
            label5.TabIndex = 10;
            label5.Text = "Note: Config can be edited anytime in PapersPlease\\BepInEx\\config\\wtf.psp.papers.cfg";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            cont.Enabled = false;
            cont.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            cont.Location = new System.Drawing.Point(541, 551);
            cont.Name = "cont";
            cont.Size = new System.Drawing.Size(130, 57);
            cont.TabIndex = 17;
            cont.Text = "Continue";
            cont.UseVisualStyleBackColor = true;
            cont.Click += cont_Click;
            // 
            // Config
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(cont);
            Controls.Add(error);
            Controls.Add(label5);
            Controls.Add(botToken);
            Controls.Add(label4);
            Controls.Add(botUsername);
            Controls.Add(label3);
            Controls.Add(twitchChannel);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MaximumSize = new System.Drawing.Size(685, 625);
            MinimumSize = new System.Drawing.Size(685, 625);
            Name = "Config";
            Size = new System.Drawing.Size(685, 625);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label error;

        private System.Windows.Forms.Label label5;

        private System.Windows.Forms.TextBox botUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox botToken;
        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox twitchChannel;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Button cont;

        #endregion
    }
}