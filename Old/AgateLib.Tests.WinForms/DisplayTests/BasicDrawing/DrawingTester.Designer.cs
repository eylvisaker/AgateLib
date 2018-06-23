// The contents of this file are public domain.
// You may use them as you wish.
//
namespace AgateLib.Tests.DisplayTests.BasicDrawing
{
	partial class DrawingTester
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnDrawLine = new System.Windows.Forms.Button();
			this.btnFillRect = new System.Windows.Forms.Button();
			this.btnDrawRect = new System.Windows.Forms.Button();
			this.btnColor = new System.Windows.Forms.Button();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.btnClear = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.nudAlpha = new System.Windows.Forms.NumericUpDown();
			this.btnDrawCircle = new System.Windows.Forms.Button();
			this.btnFillCircle = new System.Windows.Forms.Button();
			this.btnDrawPolygon = new System.Windows.Forms.Button();
			this.btnFillPolygon = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nudAlpha)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Location = new System.Drawing.Point(106, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(237, 289);
			this.panel1.TabIndex = 0;
			// 
			// btnDrawLine
			// 
			this.btnDrawLine.Location = new System.Drawing.Point(12, 67);
			this.btnDrawLine.Name = "btnDrawLine";
			this.btnDrawLine.Size = new System.Drawing.Size(88, 23);
			this.btnDrawLine.TabIndex = 0;
			this.btnDrawLine.Text = "Draw Line";
			this.btnDrawLine.UseVisualStyleBackColor = true;
			// 
			// btnFillRect
			// 
			this.btnFillRect.Location = new System.Drawing.Point(12, 182);
			this.btnFillRect.Name = "btnFillRect";
			this.btnFillRect.Size = new System.Drawing.Size(88, 23);
			this.btnFillRect.TabIndex = 1;
			this.btnFillRect.Text = "Fill Rect";
			this.btnFillRect.UseVisualStyleBackColor = true;
			// 
			// btnDrawRect
			// 
			this.btnDrawRect.Location = new System.Drawing.Point(12, 96);
			this.btnDrawRect.Name = "btnDrawRect";
			this.btnDrawRect.Size = new System.Drawing.Size(88, 23);
			this.btnDrawRect.TabIndex = 2;
			this.btnDrawRect.Text = "Draw Rect";
			this.btnDrawRect.UseVisualStyleBackColor = true;
			// 
			// btnColor
			// 
			this.btnColor.BackColor = System.Drawing.Color.Red;
			this.btnColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnColor.Location = new System.Drawing.Point(12, 12);
			this.btnColor.Name = "btnColor";
			this.btnColor.Size = new System.Drawing.Size(75, 23);
			this.btnColor.TabIndex = 3;
			this.btnColor.Text = "Color";
			this.btnColor.UseVisualStyleBackColor = false;
			this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
			// 
			// btnClear
			// 
			this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnClear.Location = new System.Drawing.Point(12, 278);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(88, 23);
			this.btnClear.TabIndex = 4;
			this.btnClear.Text = "Clear";
			this.btnClear.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 44);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(34, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Alpha";
			// 
			// nudAlpha
			// 
			this.nudAlpha.DecimalPlaces = 1;
			this.nudAlpha.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.nudAlpha.Location = new System.Drawing.Point(46, 41);
			this.nudAlpha.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudAlpha.Name = "nudAlpha";
			this.nudAlpha.Size = new System.Drawing.Size(41, 20);
			this.nudAlpha.TabIndex = 6;
			this.nudAlpha.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
			// 
			// btnDrawCircle
			// 
			this.btnDrawCircle.Location = new System.Drawing.Point(12, 124);
			this.btnDrawCircle.Name = "btnDrawCircle";
			this.btnDrawCircle.Size = new System.Drawing.Size(88, 23);
			this.btnDrawCircle.TabIndex = 7;
			this.btnDrawCircle.Text = "Draw Circle";
			this.btnDrawCircle.UseVisualStyleBackColor = true;
			// 
			// btnFillCircle
			// 
			this.btnFillCircle.Location = new System.Drawing.Point(12, 211);
			this.btnFillCircle.Name = "btnFillCircle";
			this.btnFillCircle.Size = new System.Drawing.Size(88, 23);
			this.btnFillCircle.TabIndex = 8;
			this.btnFillCircle.Text = "Fill Circle";
			this.btnFillCircle.UseVisualStyleBackColor = true;
			// 
			// btnDrawPolygon
			// 
			this.btnDrawPolygon.Location = new System.Drawing.Point(12, 153);
			this.btnDrawPolygon.Name = "btnDrawPolygon";
			this.btnDrawPolygon.Size = new System.Drawing.Size(88, 23);
			this.btnDrawPolygon.TabIndex = 9;
			this.btnDrawPolygon.Text = "Draw Polygon";
			this.btnDrawPolygon.UseVisualStyleBackColor = true;
			// 
			// btnFillPolygon
			// 
			this.btnFillPolygon.Location = new System.Drawing.Point(12, 240);
			this.btnFillPolygon.Name = "btnFillPolygon";
			this.btnFillPolygon.Size = new System.Drawing.Size(88, 23);
			this.btnFillPolygon.TabIndex = 10;
			this.btnFillPolygon.Text = "Fill Polygon";
			this.btnFillPolygon.UseVisualStyleBackColor = true;
			// 
			// DrawingTester
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(355, 313);
			this.Controls.Add(this.btnFillPolygon);
			this.Controls.Add(this.btnDrawPolygon);
			this.Controls.Add(this.btnFillCircle);
			this.Controls.Add(this.btnDrawCircle);
			this.Controls.Add(this.nudAlpha);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.btnColor);
			this.Controls.Add(this.btnDrawRect);
			this.Controls.Add(this.btnFillRect);
			this.Controls.Add(this.btnDrawLine);
			this.Controls.Add(this.panel1);
			this.Name = "DrawingTester";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Drawing Tester";
			((System.ComponentModel.ISupportInitialize)(this.nudAlpha)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.Panel panel1;
		public System.Windows.Forms.Button btnDrawLine;
		public System.Windows.Forms.Button btnFillRect;
		public System.Windows.Forms.Button btnDrawRect;
		public System.Windows.Forms.Button btnColor;
		private System.Windows.Forms.ColorDialog colorDialog1;
		public System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown nudAlpha;
		public System.Windows.Forms.Button btnDrawCircle;
		public System.Windows.Forms.Button btnFillCircle;
		public System.Windows.Forms.Button btnDrawPolygon;
		public System.Windows.Forms.Button btnFillPolygon;
	}
}