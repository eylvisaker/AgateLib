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
			this.pctImage = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pctImage)).BeginInit();
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
			this.lstExamples.Location = new System.Drawing.Point(16, 15);
			this.lstExamples.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.lstExamples.Name = "lstExamples";
			this.lstExamples.Size = new System.Drawing.Size(236, 532);
			this.lstExamples.TabIndex = 0;
			this.lstExamples.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstExamples_DrawItem);
			this.lstExamples.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lstExamples_MeasureItem);
			this.lstExamples.SelectedIndexChanged += new System.EventHandler(this.lstExamples_SelectedIndexChanged);
			// 
			// pctImage
			// 
			this.pctImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pctImage.Location = new System.Drawing.Point(270, 15);
			this.pctImage.Name = "pctImage";
			this.pctImage.Size = new System.Drawing.Size(622, 484);
			this.pctImage.TabIndex = 1;
			this.pctImage.TabStop = false;
			// 
			// LauncherView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(904, 566);
			this.Controls.Add(this.pctImage);
			this.Controls.Add(this.lstExamples);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "LauncherView";
			this.Text = "Example Launcher";
			((System.ComponentModel.ISupportInitialize)(this.pctImage)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lstExamples;
		private System.Windows.Forms.PictureBox pctImage;
	}
}

