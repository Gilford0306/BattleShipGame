namespace BattleShipClient
{
    partial class EnemySelectionPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.BConnect = new System.Windows.Forms.Button();
            this.DGVAvailableEnemies = new System.Windows.Forms.DataGridView();
            this.IPAndPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EnemyNick = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.DGVAvailableEnemies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // BConnect
            // 
            this.BConnect.BackColor = System.Drawing.Color.Navy;
            this.BConnect.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BConnect.ForeColor = System.Drawing.Color.White;
            this.BConnect.Location = new System.Drawing.Point(65, 272);
            this.BConnect.Name = "BConnect";
            this.BConnect.Size = new System.Drawing.Size(252, 63);
            this.BConnect.TabIndex = 6;
            this.BConnect.Text = "Invite player ?";
            this.BConnect.UseVisualStyleBackColor = false;
            this.BConnect.Click += new System.EventHandler(this.BConnect_Click);
            // 
            // DGVAvailableEnemies
            // 
            this.DGVAvailableEnemies.AllowUserToAddRows = false;
            this.DGVAvailableEnemies.AllowUserToDeleteRows = false;
            this.DGVAvailableEnemies.AllowUserToResizeRows = false;
            this.DGVAvailableEnemies.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DGVAvailableEnemies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGVAvailableEnemies.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DGVAvailableEnemies.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVAvailableEnemies.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DGVAvailableEnemies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVAvailableEnemies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IPAndPort,
            this.EnemyNick});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGVAvailableEnemies.DefaultCellStyle = dataGridViewCellStyle2;
            this.DGVAvailableEnemies.Location = new System.Drawing.Point(65, 167);
            this.DGVAvailableEnemies.MultiSelect = false;
            this.DGVAvailableEnemies.Name = "DGVAvailableEnemies";
            this.DGVAvailableEnemies.ReadOnly = true;
            this.DGVAvailableEnemies.RowHeadersVisible = false;
            this.DGVAvailableEnemies.Size = new System.Drawing.Size(252, 71);
            this.DGVAvailableEnemies.TabIndex = 7;
            this.DGVAvailableEnemies.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVAvailableEnemies_CellClick);
            // 
            // IPAndPort
            // 
            this.IPAndPort.HeaderText = "IPAndPort";
            this.IPAndPort.Name = "IPAndPort";
            this.IPAndPort.ReadOnly = true;
            this.IPAndPort.Visible = false;
            // 
            // EnemyNick
            // 
            this.EnemyNick.HeaderText = "Now online";
            this.EnemyNick.Name = "EnemyNick";
            this.EnemyNick.ReadOnly = true;
            this.EnemyNick.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::BattleShipClient.Properties.Resources.Ramsey_PRO_05_23_Hero;
            this.pictureBox1.Location = new System.Drawing.Point(34, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(311, 110);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // EnemySelectionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 390);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.DGVAvailableEnemies);
            this.Controls.Add(this.BConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "EnemySelectionPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Battleship - enemy selection";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CloseApp);
            this.Load += new System.EventHandler(this.EnemySelectionPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGVAvailableEnemies)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button BConnect;
        private System.Windows.Forms.DataGridViewTextBoxColumn IPAndPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn EnemyNick;
        public System.Windows.Forms.DataGridView DGVAvailableEnemies;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}