namespace Tests
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
			this.lstTests = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// lstTests
			// 
			this.lstTests.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstTests.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lstTests.FormattingEnabled = true;
			this.lstTests.Location = new System.Drawing.Point(12, 12);
			this.lstTests.MultiColumn = true;
			this.lstTests.Name = "lstTests";
			this.lstTests.Size = new System.Drawing.Size(513, 368);
			this.lstTests.TabIndex = 0;
			this.lstTests.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstTests_DrawItem);
			this.lstTests.DoubleClick += new System.EventHandler(this.lstTests_DoubleClick);
			this.lstTests.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstTests_KeyUp);
			// 
			// frmLauncher
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(537, 391);
			this.Controls.Add(this.lstTests);
			this.Name = "frmLauncher";
			this.Text = "AgateLib Test Launcher";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lstTests;


	}
}