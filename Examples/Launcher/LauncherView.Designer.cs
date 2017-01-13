namespace Examples.Launcher
{
	partial class LauncherView
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
			this.lstExamples = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// lstExamples
			// 
			this.lstExamples.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lstExamples.DisplayMember = "Name";
			this.lstExamples.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.lstExamples.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstExamples.FormattingEnabled = true;
			this.lstExamples.Location = new System.Drawing.Point(12, 12);
			this.lstExamples.Name = "lstExamples";
			this.lstExamples.Size = new System.Drawing.Size(178, 433);
			this.lstExamples.TabIndex = 0;
			this.lstExamples.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstExamples_DrawItem);
			this.lstExamples.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lstExamples_MeasureItem);
			// 
			// LauncherView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(440, 460);
			this.Controls.Add(this.lstExamples);
			this.Name = "LauncherView";
			this.Text = "Example Launcher";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lstExamples;
	}
}

