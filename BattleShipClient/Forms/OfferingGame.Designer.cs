namespace BattleShipClient
{
    partial class OfferingGame
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
            this.label2 = new System.Windows.Forms.Label();
            this.BYes = new System.Windows.Forms.Button();
            this.BNo = new System.Windows.Forms.Button();
            this.CBEneNicks = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(45, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(220, 22);
            this.label2.TabIndex = 5;
            this.label2.Text = "Do you want start game ?";
            // 
            // BYes
            // 
            this.BYes.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.BYes.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BYes.Location = new System.Drawing.Point(34, 242);
            this.BYes.Name = "BYes";
            this.BYes.Size = new System.Drawing.Size(116, 50);
            this.BYes.TabIndex = 7;
            this.BYes.Text = "Yes";
            this.BYes.UseVisualStyleBackColor = false;
            this.BYes.Click += new System.EventHandler(this.BYes_Click);
            // 
            // BNo
            // 
            this.BNo.BackColor = System.Drawing.Color.Tomato;
            this.BNo.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BNo.Location = new System.Drawing.Point(167, 242);
            this.BNo.Name = "BNo";
            this.BNo.Size = new System.Drawing.Size(116, 50);
            this.BNo.TabIndex = 8;
            this.BNo.Text = "No";
            this.BNo.UseVisualStyleBackColor = false;
            this.BNo.Click += new System.EventHandler(this.BNo_Click);
            // 
            // CBEneNicks
            // 
            this.CBEneNicks.FormattingEnabled = true;
            this.CBEneNicks.Location = new System.Drawing.Point(295, 183);
            this.CBEneNicks.Name = "CBEneNicks";
            this.CBEneNicks.Size = new System.Drawing.Size(17, 21);
            this.CBEneNicks.TabIndex = 9;
            this.CBEneNicks.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::BattleShipClient.Properties.Resources._1_xVBrq2LQs4Xd8zXkCxyzyA;
            this.pictureBox1.Location = new System.Drawing.Point(34, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(231, 136);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // OfferingGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 310);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.CBEneNicks);
            this.Controls.Add(this.BNo);
            this.Controls.Add(this.BYes);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OfferingGame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Battleship - Let\'s play";
            this.Load += new System.EventHandler(this.OfferingGame_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BYes;
        private System.Windows.Forms.Button BNo;
        private System.Windows.Forms.ComboBox CBEneNicks;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}