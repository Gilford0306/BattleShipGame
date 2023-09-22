﻿namespace BattleShipClient
{
    partial class ServerConnectionPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
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
            this.LNick = new System.Windows.Forms.Label();
            this.LServerIP = new System.Windows.Forms.Label();
            this.TBNick = new System.Windows.Forms.TextBox();
            this.TBServerIP = new System.Windows.Forms.TextBox();
            this.BConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LNick
            // 
            this.LNick.AutoSize = true;
            this.LNick.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LNick.Location = new System.Drawing.Point(48, 143);
            this.LNick.Name = "LNick";
            this.LNick.Size = new System.Drawing.Size(95, 22);
            this.LNick.TabIndex = 1;
            this.LNick.Text = "Type nick:";
            // 
            // LServerIP
            // 
            this.LServerIP.AutoSize = true;
            this.LServerIP.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold);
            this.LServerIP.Location = new System.Drawing.Point(8, 175);
            this.LServerIP.Name = "LServerIP";
            this.LServerIP.Size = new System.Drawing.Size(135, 22);
            this.LServerIP.TabIndex = 2;
            this.LServerIP.Text = "Type server IP:";
            // 
            // TBNick
            // 
            this.TBNick.Location = new System.Drawing.Point(149, 143);
            this.TBNick.MaxLength = 16;
            this.TBNick.Name = "TBNick";
            this.TBNick.Size = new System.Drawing.Size(119, 20);
            this.TBNick.TabIndex = 3;
            this.TBNick.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.EnterClicked);
            // 
            // TBServerIP
            // 
            this.TBServerIP.Location = new System.Drawing.Point(149, 177);
            this.TBServerIP.Name = "TBServerIP";
            this.TBServerIP.Size = new System.Drawing.Size(119, 20);
            this.TBServerIP.TabIndex = 4;
            this.TBServerIP.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.EnterClicked);
            // 
            // BConnect
            // 
            this.BConnect.BackColor = System.Drawing.Color.Maroon;
            this.BConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BConnect.ForeColor = System.Drawing.Color.White;
            this.BConnect.Location = new System.Drawing.Point(74, 228);
            this.BConnect.Name = "BConnect";
            this.BConnect.Size = new System.Drawing.Size(148, 54);
            this.BConnect.TabIndex = 5;
            this.BConnect.Text = "Connect";
            this.BConnect.UseVisualStyleBackColor = false;
            this.BConnect.Click += new System.EventHandler(this.BConnect_Click);
            // 
            // ServerConnectionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 304);
            this.Controls.Add(this.BConnect);
            this.Controls.Add(this.TBServerIP);
            this.Controls.Add(this.TBNick);
            this.Controls.Add(this.LServerIP);
            this.Controls.Add(this.LNick);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ServerConnectionPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Battleship - connection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label LNick;
        private System.Windows.Forms.Label LServerIP;
        private System.Windows.Forms.TextBox TBNick;
        private System.Windows.Forms.TextBox TBServerIP;
        private System.Windows.Forms.Button BConnect;
    }
}