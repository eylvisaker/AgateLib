namespace AgateLib.Tests
{
	partial class frmLauncher
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLauncher));
			this.lstTests = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtCommandLine = new System.Windows.Forms.TextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.txtTestInfo = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// lstTests
			// 
			this.lstTests.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstTests.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lstTests.FormattingEnabled = true;
			this.lstTests.Location = new System.Drawing.Point(12, 36);
			this.lstTests.MultiColumn = true;
			this.lstTests.Name = "lstTests";
			this.lstTests.Size = new System.Drawing.Size(646, 277);
			this.lstTests.TabIndex = 0;
			this.lstTests.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstTests_DrawItem);
			this.lstTests.SelectedIndexChanged += new System.EventHandler(this.lstTests_SelectedIndexChanged);
			this.lstTests.DoubleClick += new System.EventHandler(this.lstTests_DoubleClick);
			this.lstTests.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstTests_KeyUp);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(248, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(131, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Command line parameters:";
			// 
			// txtCommandLine
			// 
			this.txtCommandLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtCommandLine.Location = new System.Drawing.Point(385, 12);
			this.txtCommandLine.Name = "txtCommandLine";
			this.txtCommandLine.Size = new System.Drawing.Size(273, 20);
			this.txtCommandLine.TabIndex = 2;
			this.txtCommandLine.Text = "-window 800x600 -debuggui";
			// 
			// txtTestInfo
			// 
			this.txtTestInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTestInfo.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTestInfo.Location = new System.Drawing.Point(12, 337);
			this.txtTestInfo.Multiline = true;
			this.txtTestInfo.Name = "txtTestInfo";
			this.txtTestInfo.ReadOnly = true;
			this.txtTestInfo.Size = new System.Drawing.Size(646, 91);
			this.txtTestInfo.TabIndex = 3;
			// 
			// frmLauncher
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(670, 440);
			this.Controls.Add(this.txtTestInfo);
			this.Controls.Add(this.txtCommandLine);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lstTests);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(450, 398);
			this.Name = "frmLauncher";
			this.Text = "AgateLib Test Launcher";
			this.Shown += new System.EventHandler(this.frmLauncher_Shown);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmLauncher_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lstTests;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtCommandLine;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.TextBox txtTestInfo;
	}
}