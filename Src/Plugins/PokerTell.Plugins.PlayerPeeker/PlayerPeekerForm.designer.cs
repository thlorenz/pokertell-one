/*
Created by SharpDevelop.
 * Thorsten Lorenz
 * Date: 1/24/2009
 * Time: 12:46 PM
 */
namespace PokerTell.PlayerPeeker
{
	partial class PlayerPeekerForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerPeekerForm));
            this.dgvStats = new System.Windows.Forms.DataGridView();
            this.StatusSTrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.butLogin = new System.Windows.Forms.ToolStripDropDownButton();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Player = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Games = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Profit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvgProf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ROI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BuyIn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ITM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Win = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Finish = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Entrants = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStats)).BeginInit();
            this.StatusSTrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvStats
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvStats.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvStats.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvStats.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStats.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Player,
            this.Games,
            this.Profit,
            this.AvgProf,
            this.ROI,
            this.BuyIn,
            this.ITM,
            this.FT,
            this.Win,
            this.Finish,
            this.Entrants});
            this.dgvStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvStats.Location = new System.Drawing.Point(0, 0);
            this.dgvStats.Name = "dgvStats";
            this.dgvStats.ReadOnly = true;
            this.dgvStats.RowHeadersVisible = false;
            this.dgvStats.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgvStats.Size = new System.Drawing.Size(642, 215);
            this.dgvStats.TabIndex = 2;
            // 
            // StatusSTrip
            // 
            this.StatusSTrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.butLogin});
            this.StatusSTrip.Location = new System.Drawing.Point(0, 218);
            this.StatusSTrip.Name = "StatusSTrip";
            this.StatusSTrip.Size = new System.Drawing.Size(642, 22);
            this.StatusSTrip.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(38, 17);
            this.lblStatus.Text = "Ready";
            // 
            // butLogin
            // 
            this.butLogin.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.butLogin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.butLogin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginToolStripMenuItem,
            this.resetToolStripMenuItem});
            this.butLogin.Image = ((System.Drawing.Image)(resources.GetObject("butLogin.Image")));
            this.butLogin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.butLogin.Name = "butLogin";
            this.butLogin.Size = new System.Drawing.Size(29, 20);
            this.butLogin.Text = "Login";
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.loginToolStripMenuItem.Text = "Login";
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.LoginToolStripMenuItemClick);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.ResetToolStripMenuItemClick);
            // 
            // Player
            // 
            this.Player.Frozen = true;
            this.Player.HeaderText = "Player";
            this.Player.Name = "Player";
            this.Player.ReadOnly = true;
            this.Player.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Player.Width = 61;
            // 
            // Games
            // 
            this.Games.HeaderText = "Games";
            this.Games.Name = "Games";
            this.Games.ReadOnly = true;
            this.Games.Width = 65;
            // 
            // Profit
            // 
            this.Profit.HeaderText = "Profit";
            this.Profit.Name = "Profit";
            this.Profit.ReadOnly = true;
            this.Profit.Width = 56;
            // 
            // AvgProf
            // 
            this.AvgProf.HeaderText = "AvgProf";
            this.AvgProf.Name = "AvgProf";
            this.AvgProf.ReadOnly = true;
            this.AvgProf.Width = 70;
            // 
            // ROI
            // 
            this.ROI.HeaderText = "ROI";
            this.ROI.Name = "ROI";
            this.ROI.ReadOnly = true;
            this.ROI.Width = 51;
            // 
            // BuyIn
            // 
            this.BuyIn.HeaderText = "BuyIn";
            this.BuyIn.Name = "BuyIn";
            this.BuyIn.ReadOnly = true;
            this.BuyIn.Width = 59;
            // 
            // ITM
            // 
            this.ITM.HeaderText = "ITM";
            this.ITM.Name = "ITM";
            this.ITM.ReadOnly = true;
            this.ITM.Width = 51;
            // 
            // FT
            // 
            this.FT.HeaderText = "FT";
            this.FT.Name = "FT";
            this.FT.ReadOnly = true;
            this.FT.Width = 45;
            // 
            // Win
            // 
            this.Win.HeaderText = "Win";
            this.Win.Name = "Win";
            this.Win.ReadOnly = true;
            this.Win.Width = 51;
            // 
            // Finish
            // 
            this.Finish.HeaderText = "Finish";
            this.Finish.Name = "Finish";
            this.Finish.ReadOnly = true;
            this.Finish.Width = 59;
            // 
            // Entrants
            // 
            this.Entrants.HeaderText = "Entrants";
            this.Entrants.Name = "Entrants";
            this.Entrants.ReadOnly = true;
            this.Entrants.Width = 71;
            // 
            // PlayerPeekerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 240);
            this.Controls.Add(this.StatusSTrip);
            this.Controls.Add(this.dgvStats);
            this.MaximumSize = new System.Drawing.Size(650, 400);
            this.Name = "PlayerPeekerForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "PlayerPeeker";
            this.SizeChanged += new System.EventHandler(this.FrmPlayerPeekSizeChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPlayerPeekFormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStats)).EndInit();
            this.StatusSTrip.ResumeLayout(false);
            this.StatusSTrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
		private System.Windows.Forms.ToolStripDropDownButton butLogin;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.StatusStrip StatusSTrip;
		private System.Windows.Forms.DataGridView dgvStats;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Player;
        private System.Windows.Forms.DataGridViewTextBoxColumn Games;
        private System.Windows.Forms.DataGridViewTextBoxColumn Profit;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvgProf;
        private System.Windows.Forms.DataGridViewTextBoxColumn ROI;
        private System.Windows.Forms.DataGridViewTextBoxColumn BuyIn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITM;
        private System.Windows.Forms.DataGridViewTextBoxColumn FT;
        private System.Windows.Forms.DataGridViewTextBoxColumn Win;
        private System.Windows.Forms.DataGridViewTextBoxColumn Finish;
        private System.Windows.Forms.DataGridViewTextBoxColumn Entrants;
		
		
	}
}
