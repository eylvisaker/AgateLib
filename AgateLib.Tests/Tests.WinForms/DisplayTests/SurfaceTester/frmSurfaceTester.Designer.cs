// The contents of this file are public domain.
// You may use them as you wish.
//
namespace AgateLib.Tests.DisplayTests.SurfaceTester
{
	partial class frmSurfaceTester
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.colorBox = new System.Windows.Forms.PictureBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.nudScaleHeight = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.nudAngle = new System.Windows.Forms.NumericUpDown();
			this.nudScaleWidth = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.cboRotation = new System.Windows.Forms.ComboBox();
			this.nudAlpha = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.cboAlignment = new System.Windows.Forms.ComboBox();
			this.nudY = new System.Windows.Forms.NumericUpDown();
			this.nudX = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			((System.ComponentModel.ISupportInitialize)(this.pctGraphics)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.colorBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudScaleHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudAngle)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudScaleWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudAlpha)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudX)).BeginInit();
			this.SuspendLayout();
			// 
			// pctGraphics
			// 
			this.pctGraphics.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pctGraphics.Location = new System.Drawing.Point(0, 0);
			this.pctGraphics.Name = "pctGraphics";
			this.pctGraphics.Size = new System.Drawing.Size(380, 217);
			this.pctGraphics.TabIndex = 0;
			this.pctGraphics.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.colorBox);
			this.panel1.Controls.Add(this.label9);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.nudScaleHeight);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.label7);
			this.panel1.Controls.Add(this.nudAngle);
			this.panel1.Controls.Add(this.nudScaleWidth);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.cboRotation);
			this.panel1.Controls.Add(this.nudAlpha);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.cboAlignment);
			this.panel1.Controls.Add(this.nudY);
			this.panel1.Controls.Add(this.nudX);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 217);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(380, 148);
			this.panel1.TabIndex = 17;
			// 
			// colorBox
			// 
			this.colorBox.BackColor = System.Drawing.Color.White;
			this.colorBox.Location = new System.Drawing.Point(200, 86);
			this.colorBox.Name = "colorBox";
			this.colorBox.Size = new System.Drawing.Size(40, 28);
			this.colorBox.TabIndex = 18;
			this.colorBox.TabStop = false;
			this.colorBox.Click += new System.EventHandler(this.colorBox_Click);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(163, 94);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(31, 13);
			this.label9.TabIndex = 17;
			this.label9.Text = "Color";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 15);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(14, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "X";
			// 
			// nudScaleHeight
			// 
			this.nudScaleHeight.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.nudScaleHeight.Location = new System.Drawing.Point(82, 118);
			this.nudScaleHeight.Maximum = new decimal(new int[] {
            720,
            0,
            0,
            0});
			this.nudScaleHeight.Minimum = new decimal(new int[] {
            720,
            0,
            0,
            -2147483648});
			this.nudScaleHeight.Name = "nudScaleHeight";
			this.nudScaleHeight.Size = new System.Drawing.Size(66, 20);
			this.nudScaleHeight.TabIndex = 16;
			this.nudScaleHeight.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(163, 41);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(77, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Rotation Angle";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(12, 120);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(68, 13);
			this.label7.TabIndex = 15;
			this.label7.Text = "Scale Height";
			// 
			// nudAngle
			// 
			this.nudAngle.DecimalPlaces = 1;
			this.nudAngle.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.nudAngle.Location = new System.Drawing.Point(262, 39);
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
			this.nudAngle.TabIndex = 2;
			this.nudAngle.ValueChanged += new System.EventHandler(this.nudAngle_ValueChanged);
			// 
			// nudScaleWidth
			// 
			this.nudScaleWidth.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.nudScaleWidth.Location = new System.Drawing.Point(82, 92);
			this.nudScaleWidth.Maximum = new decimal(new int[] {
            720,
            0,
            0,
            0});
			this.nudScaleWidth.Minimum = new decimal(new int[] {
            720,
            0,
            0,
            -2147483648});
			this.nudScaleWidth.Name = "nudScaleWidth";
			this.nudScaleWidth.Size = new System.Drawing.Size(66, 20);
			this.nudScaleWidth.TabIndex = 14;
			this.nudScaleWidth.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(163, 68);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(81, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Rotation Center";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(12, 94);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(65, 13);
			this.label8.TabIndex = 13;
			this.label8.Text = "Scale Width";
			// 
			// cboRotation
			// 
			this.cboRotation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRotation.FormattingEnabled = true;
			this.cboRotation.Location = new System.Drawing.Point(262, 65);
			this.cboRotation.MaxDropDownItems = 9;
			this.cboRotation.Name = "cboRotation";
			this.cboRotation.Size = new System.Drawing.Size(117, 21);
			this.cboRotation.TabIndex = 4;
			this.cboRotation.SelectedIndexChanged += new System.EventHandler(this.cboRotation_SelectedIndexChanged);
			// 
			// nudAlpha
			// 
			this.nudAlpha.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.nudAlpha.Location = new System.Drawing.Point(52, 66);
			this.nudAlpha.Maximum = new decimal(new int[] {
            720,
            0,
            0,
            0});
			this.nudAlpha.Name = "nudAlpha";
			this.nudAlpha.Size = new System.Drawing.Size(66, 20);
			this.nudAlpha.TabIndex = 12;
			this.nudAlpha.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudAlpha.ValueChanged += new System.EventHandler(this.nudAlpha_ValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(163, 15);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(93, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Surface Alignment";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 68);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(34, 13);
			this.label6.TabIndex = 11;
			this.label6.Text = "Alpha";
			// 
			// cboAlignment
			// 
			this.cboAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAlignment.FormattingEnabled = true;
			this.cboAlignment.Location = new System.Drawing.Point(262, 12);
			this.cboAlignment.MaxDropDownItems = 9;
			this.cboAlignment.Name = "cboAlignment";
			this.cboAlignment.Size = new System.Drawing.Size(117, 21);
			this.cboAlignment.TabIndex = 6;
			this.cboAlignment.SelectedIndexChanged += new System.EventHandler(this.cboAlignment_SelectedIndexChanged);
			// 
			// nudY
			// 
			this.nudY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.nudY.Location = new System.Drawing.Point(52, 38);
			this.nudY.Maximum = new decimal(new int[] {
            720,
            0,
            0,
            0});
			this.nudY.Name = "nudY";
			this.nudY.Size = new System.Drawing.Size(66, 20);
			this.nudY.TabIndex = 10;
			this.nudY.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// nudX
			// 
			this.nudX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.nudX.Location = new System.Drawing.Point(52, 12);
			this.nudX.Maximum = new decimal(new int[] {
            720,
            0,
            0,
            0});
			this.nudX.Name = "nudX";
			this.nudX.Size = new System.Drawing.Size(66, 20);
			this.nudX.TabIndex = 8;
			this.nudX.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.nudX.ValueChanged += new System.EventHandler(this.nudX_ValueChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 41);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(14, 13);
			this.label5.TabIndex = 9;
			this.label5.Text = "Y";
			// 
			// frmSurfaceTester
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(380, 365);
			this.Controls.Add(this.pctGraphics);
			this.Controls.Add(this.panel1);
			this.Name = "frmSurfaceTester";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Surface Tester";
			this.Load += new System.EventHandler(this.frmImageRotation_Load);
			((System.ComponentModel.ISupportInitialize)(this.pctGraphics)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.colorBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudScaleHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudAngle)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudScaleWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudAlpha)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudX)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pctGraphics;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown nudScaleHeight;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown nudAngle;
		private System.Windows.Forms.NumericUpDown nudScaleWidth;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox cboRotation;
		private System.Windows.Forms.NumericUpDown nudAlpha;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox cboAlignment;
		private System.Windows.Forms.NumericUpDown nudY;
		private System.Windows.Forms.NumericUpDown nudX;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.PictureBox colorBox;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ColorDialog colorDialog1;
	}
}

