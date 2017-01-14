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
			this.btnLaunch = new System.Windows.Forms.Button();
			this.cboArgs = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.pctImage)).BeginInit();
			this.panel1.SuspendLayout();
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
			this.lstExamples.DoubleClick += new System.EventHandler(this.lstExamples_DoubleClick);
			// 
			// pctImage
			// 
			this.pctImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pctImage.BackColor = System.Drawing.SystemColors.ControlDark;
			this.pctImage.Location = new System.Drawing.Point(270, 15);
			this.pctImage.Name = "pctImage";
			this.pctImage.Size = new System.Drawing.Size(622, 484);
			this.pctImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pctImage.TabIndex = 1;
			this.pctImage.TabStop = false;
			// 
			// btnLaunch
			// 
			this.btnLaunch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLaunch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnLaunch.Location = new System.Drawing.Point(517, 0);
			this.btnLaunch.Name = "btnLaunch";
			this.btnLaunch.Size = new System.Drawing.Size(105, 37);
			this.btnLaunch.TabIndex = 2;
			this.btnLaunch.Text = "Launch";
			this.btnLaunch.UseVisualStyleBackColor = true;
			this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
			// 
			// cboArgs
			// 
			this.cboArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cboArgs.FormattingEnabled = true;
			this.cboArgs.Items.AddRange(new object[] {
            "",
            "-window",
            "-window 640x480",
            "-window 800x600"});
			this.cboArgs.Location = new System.Drawing.Point(85, 8);
			this.cboArgs.Name = "cboArgs";
			this.cboArgs.Size = new System.Drawing.Size(408, 24);
			this.cboArgs.TabIndex = 3;
			this.cboArgs.Text = "-window";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 17);
			this.label1.TabIndex = 4;
			this.label1.Text = "Arguments";
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.btnLaunch);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.cboArgs);
			this.panel1.Location = new System.Drawing.Point(270, 517);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(622, 37);
			this.panel1.TabIndex = 5;
			// 
			// LauncherView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(904, 566);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.pctImage);
			this.Controls.Add(this.lstExamples);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "LauncherView";
			this.Text = "Example Launcher";
			((System.ComponentModel.ISupportInitialize)(this.pctImage)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lstExamples;
		private System.Windows.Forms.PictureBox pctImage;
		private System.Windows.Forms.Button btnLaunch;
		private System.Windows.Forms.ComboBox cboArgs;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
	}
}

