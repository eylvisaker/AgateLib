// The contents of this file are public domain.
// You may use them as you wish.
//
namespace AgateLib.Tests.DisplayTests.SpriteTester
{
	partial class frmSpriteTester
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
			this.pctGraphics = new System.Windows.Forms.PictureBox();
			this.nudTimePerFrame = new System.Windows.Forms.NumericUpDown();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label8 = new System.Windows.Forms.Label();
			this.nudAngle = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			this.cboRotation = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.nudScale = new System.Windows.Forms.NumericUpDown();
			this.btnRestart = new System.Windows.Forms.Button();
			this.cboFrame = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.chkAnimating = new System.Windows.Forms.CheckBox();
			this.cboAnimationType = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.chkPlayReverse = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btn64 = new System.Windows.Forms.Button();
			this.btn128 = new System.Windows.Forms.Button();
			this.btn96 = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtHeight = new System.Windows.Forms.TextBox();
			this.txtWidth = new System.Windows.Forms.TextBox();
			this.btnLoadSprite = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.cboAlignment = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.openFile = new System.Windows.Forms.OpenFileDialog();
			this.lblFrameRate = new System.Windows.Forms.Label();
			this.chkVSync = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.pctGraphics)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTimePerFrame)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudAngle)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudScale)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pctGraphics
			// 
			this.pctGraphics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pctGraphics.Location = new System.Drawing.Point(12, 12);
			this.pctGraphics.Name = "pctGraphics";
			this.pctGraphics.Size = new System.Drawing.Size(266, 345);
			this.pctGraphics.TabIndex = 0;
			this.pctGraphics.TabStop = false;
			// 
			// nudTimePerFrame
			// 
			this.nudTimePerFrame.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudTimePerFrame.Location = new System.Drawing.Point(140, 3);
			this.nudTimePerFrame.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nudTimePerFrame.Name = "nudTimePerFrame";
			this.nudTimePerFrame.Size = new System.Drawing.Size(76, 20);
			this.nudTimePerFrame.TabIndex = 2;
			this.nudTimePerFrame.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudTimePerFrame.ValueChanged += new System.EventHandler(this.nudTimePerFrame_ValueChanged);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.nudAngle);
			this.panel1.Controls.Add(this.label9);
			this.panel1.Controls.Add(this.cboRotation);
			this.panel1.Controls.Add(this.label7);
			this.panel1.Controls.Add(this.nudScale);
			this.panel1.Controls.Add(this.btnRestart);
			this.panel1.Controls.Add(this.cboFrame);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.chkAnimating);
			this.panel1.Controls.Add(this.cboAnimationType);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.chkPlayReverse);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.cboAlignment);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.nudTimePerFrame);
			this.panel1.Location = new System.Drawing.Point(284, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(219, 345);
			this.panel1.TabIndex = 3;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(3, 58);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(77, 13);
			this.label8.TabIndex = 20;
			this.label8.Text = "Rotation Angle";
			// 
			// nudAngle
			// 
			this.nudAngle.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.nudAngle.Location = new System.Drawing.Point(109, 56);
			this.nudAngle.Maximum = new decimal(new int[] {
            720,
            0,
            0,
            0});
			this.nudAngle.Minimum = new decimal(new int[] {
            720,
            0,
            0,
            -2147483648});
			this.nudAngle.Name = "nudAngle";
			this.nudAngle.Size = new System.Drawing.Size(62, 20);
			this.nudAngle.TabIndex = 21;
			this.nudAngle.ValueChanged += new System.EventHandler(this.nudAngle_ValueChanged);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(3, 85);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(81, 13);
			this.label9.TabIndex = 22;
			this.label9.Text = "Rotation Center";
			// 
			// cboRotation
			// 
			this.cboRotation.FormattingEnabled = true;
			this.cboRotation.Location = new System.Drawing.Point(109, 82);
			this.cboRotation.MaxDropDownItems = 9;
			this.cboRotation.Name = "cboRotation";
			this.cboRotation.Size = new System.Drawing.Size(107, 21);
			this.cboRotation.TabIndex = 23;
			this.cboRotation.SelectedIndexChanged += new System.EventHandler(this.cboRotation_SelectedIndexChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(122, 201);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(34, 13);
			this.label7.TabIndex = 19;
			this.label7.Text = "Scale";
			// 
			// nudScale
			// 
			this.nudScale.DecimalPlaces = 2;
			this.nudScale.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.nudScale.Location = new System.Drawing.Point(162, 199);
			this.nudScale.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.nudScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.nudScale.Name = "nudScale";
			this.nudScale.Size = new System.Drawing.Size(53, 20);
			this.nudScale.TabIndex = 4;
			this.nudScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudScale.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
			// 
			// btnRestart
			// 
			this.btnRestart.Location = new System.Drawing.Point(162, 225);
			this.btnRestart.Name = "btnRestart";
			this.btnRestart.Size = new System.Drawing.Size(54, 23);
			this.btnRestart.TabIndex = 18;
			this.btnRestart.Text = "Restart";
			this.btnRestart.UseVisualStyleBackColor = true;
			this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
			// 
			// cboFrame
			// 
			this.cboFrame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFrame.FormattingEnabled = true;
			this.cboFrame.Location = new System.Drawing.Point(64, 198);
			this.cboFrame.Name = "cboFrame";
			this.cboFrame.Size = new System.Drawing.Size(53, 21);
			this.cboFrame.TabIndex = 17;
			this.cboFrame.SelectedIndexChanged += new System.EventHandler(this.cboFrame_SelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 201);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(55, 13);
			this.label6.TabIndex = 16;
			this.label6.Text = "Set Frame";
			// 
			// chkAnimating
			// 
			this.chkAnimating.AutoSize = true;
			this.chkAnimating.Location = new System.Drawing.Point(108, 153);
			this.chkAnimating.Name = "chkAnimating";
			this.chkAnimating.Size = new System.Drawing.Size(72, 17);
			this.chkAnimating.TabIndex = 15;
			this.chkAnimating.Text = "Animating";
			this.chkAnimating.UseVisualStyleBackColor = true;
			this.chkAnimating.CheckedChanged += new System.EventHandler(this.chkAnimating_CheckedChanged);
			// 
			// cboAnimationType
			// 
			this.cboAnimationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAnimationType.FormattingEnabled = true;
			this.cboAnimationType.Location = new System.Drawing.Point(108, 126);
			this.cboAnimationType.Name = "cboAnimationType";
			this.cboAnimationType.Size = new System.Drawing.Size(107, 21);
			this.cboAnimationType.TabIndex = 14;
			this.cboAnimationType.SelectedIndexChanged += new System.EventHandler(this.cboAnimationType_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(3, 129);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 13);
			this.label5.TabIndex = 13;
			this.label5.Text = "Animation Type";
			// 
			// chkPlayReverse
			// 
			this.chkPlayReverse.AutoSize = true;
			this.chkPlayReverse.Location = new System.Drawing.Point(108, 176);
			this.chkPlayReverse.Name = "chkPlayReverse";
			this.chkPlayReverse.Size = new System.Drawing.Size(89, 17);
			this.chkPlayReverse.TabIndex = 12;
			this.chkPlayReverse.Text = "Play Reverse";
			this.chkPlayReverse.UseVisualStyleBackColor = true;
			this.chkPlayReverse.CheckedChanged += new System.EventHandler(this.chkPlayReverse_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btn64);
			this.groupBox1.Controls.Add(this.btn128);
			this.groupBox1.Controls.Add(this.btn96);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtHeight);
			this.groupBox1.Controls.Add(this.txtWidth);
			this.groupBox1.Controls.Add(this.btnLoadSprite);
			this.groupBox1.Location = new System.Drawing.Point(9, 254);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 100);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Sprite Loader";
			// 
			// btn64
			// 
			this.btn64.Location = new System.Drawing.Point(134, 13);
			this.btn64.Name = "btn64";
			this.btn64.Size = new System.Drawing.Size(60, 23);
			this.btn64.TabIndex = 12;
			this.btn64.Text = "64x64";
			this.btn64.UseVisualStyleBackColor = true;
			this.btn64.Click += new System.EventHandler(this.btn64_Click);
			// 
			// btn128
			// 
			this.btn128.Location = new System.Drawing.Point(134, 70);
			this.btn128.Name = "btn128";
			this.btn128.Size = new System.Drawing.Size(60, 23);
			this.btn128.TabIndex = 11;
			this.btn128.Text = "128x128";
			this.btn128.UseVisualStyleBackColor = true;
			this.btn128.Click += new System.EventHandler(this.btn128_Click);
			// 
			// btn96
			// 
			this.btn96.Location = new System.Drawing.Point(134, 42);
			this.btn96.Name = "btn96";
			this.btn96.Size = new System.Drawing.Size(60, 23);
			this.btn96.TabIndex = 10;
			this.btn96.Text = "96x96";
			this.btn96.UseVisualStyleBackColor = true;
			this.btn96.Click += new System.EventHandler(this.btn96_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 22);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(35, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "Width";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Height";
			// 
			// txtHeight
			// 
			this.txtHeight.Location = new System.Drawing.Point(50, 45);
			this.txtHeight.Name = "txtHeight";
			this.txtHeight.Size = new System.Drawing.Size(41, 20);
			this.txtHeight.TabIndex = 7;
			this.txtHeight.Text = "96";
			// 
			// txtWidth
			// 
			this.txtWidth.Location = new System.Drawing.Point(50, 19);
			this.txtWidth.Name = "txtWidth";
			this.txtWidth.Size = new System.Drawing.Size(41, 20);
			this.txtWidth.TabIndex = 6;
			this.txtWidth.Text = "96";
			// 
			// btnLoadSprite
			// 
			this.btnLoadSprite.Location = new System.Drawing.Point(9, 71);
			this.btnLoadSprite.Name = "btnLoadSprite";
			this.btnLoadSprite.Size = new System.Drawing.Size(91, 23);
			this.btnLoadSprite.TabIndex = 4;
			this.btnLoadSprite.Text = "Load Sprite...";
			this.btnLoadSprite.UseVisualStyleBackColor = true;
			this.btnLoadSprite.Click += new System.EventHandler(this.btnLoadSprite_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(83, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Sprite Alignment";
			// 
			// cboAlignment
			// 
			this.cboAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAlignment.FormattingEnabled = true;
			this.cboAlignment.Location = new System.Drawing.Point(109, 29);
			this.cboAlignment.Name = "cboAlignment";
			this.cboAlignment.Size = new System.Drawing.Size(107, 21);
			this.cboAlignment.TabIndex = 4;
			this.cboAlignment.SelectedIndexChanged += new System.EventHandler(this.cboAlignment_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(114, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Milliseconds per Frame";
			// 
			// openFile
			// 
			this.openFile.Filter = "All files|*.*";
			// 
			// lblFrameRate
			// 
			this.lblFrameRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblFrameRate.Location = new System.Drawing.Point(12, 360);
			this.lblFrameRate.Name = "lblFrameRate";
			this.lblFrameRate.Size = new System.Drawing.Size(290, 27);
			this.lblFrameRate.TabIndex = 24;
			this.lblFrameRate.Text = "Framerate";
			// 
			// chkVSync
			// 
			this.chkVSync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chkVSync.AutoSize = true;
			this.chkVSync.Checked = true;
			this.chkVSync.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVSync.Location = new System.Drawing.Point(221, 359);
			this.chkVSync.Name = "chkVSync";
			this.chkVSync.Size = new System.Drawing.Size(57, 17);
			this.chkVSync.TabIndex = 25;
			this.chkVSync.Text = "VSync";
			this.chkVSync.UseVisualStyleBackColor = true;
			this.chkVSync.CheckedChanged += new System.EventHandler(this.chkVSync_CheckedChanged);
			// 
			// frmSpriteTester
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(515, 382);
			this.Controls.Add(this.chkVSync);
			this.Controls.Add(this.lblFrameRate);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.pctGraphics);
			this.Name = "frmSpriteTester";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Sprite Tester";
			this.Load += new System.EventHandler(this.frmSpriteTester_Load);
			((System.ComponentModel.ISupportInitialize)(this.pctGraphics)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTimePerFrame)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudAngle)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudScale)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pctGraphics;
		private System.Windows.Forms.NumericUpDown nudTimePerFrame;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cboAlignment;
		private System.Windows.Forms.Button btnLoadSprite;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btn128;
		private System.Windows.Forms.Button btn96;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtHeight;
		private System.Windows.Forms.TextBox txtWidth;
		private System.Windows.Forms.OpenFileDialog openFile;
		private System.Windows.Forms.CheckBox chkPlayReverse;
		private System.Windows.Forms.ComboBox cboAnimationType;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox chkAnimating;
		private System.Windows.Forms.Button btnRestart;
		private System.Windows.Forms.ComboBox cboFrame;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown nudScale;
		private System.Windows.Forms.Button btn64;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown nudAngle;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cboRotation;
		private System.Windows.Forms.Label lblFrameRate;
		public System.Windows.Forms.CheckBox chkVSync;
	}
}

