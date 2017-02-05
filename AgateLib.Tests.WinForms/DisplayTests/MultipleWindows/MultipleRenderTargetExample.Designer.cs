// The contents of this file are public domain.
// You may use them as you wish.
//
namespace AgateLib.Tests.DisplayTests.MultipleWindows
{
	partial class MultipleRenderTargetExample
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.btnDraw = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.btnDrawText = new System.Windows.Forms.Button();
			this.btnClearSurface = new System.Windows.Forms.Button();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.Location = new System.Drawing.Point(12, 32);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(203, 91);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Tag = "Red Window";
			// 
			// pictureBox2
			// 
			this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox2.Location = new System.Drawing.Point(12, 29);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(203, 97);
			this.pictureBox2.TabIndex = 1;
			this.pictureBox2.TabStop = false;
			this.pictureBox2.Tag = "Green Window";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(133, 26);
			this.label1.TabIndex = 2;
			this.label1.Text = "This window should be red\r\nwith a blue rectangle:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(148, 26);
			this.label2.TabIndex = 3;
			this.label2.Text = "This window should be green \r\nwith a yellow rectangle:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(142, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "This is drawn from a surface:";
			// 
			// pictureBox3
			// 
			this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox3.Location = new System.Drawing.Point(6, 32);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(279, 170);
			this.pictureBox3.TabIndex = 4;
			this.pictureBox3.TabStop = false;
			this.pictureBox3.Tag = "Red Window";
			// 
			// btnDraw
			// 
			this.btnDraw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDraw.Location = new System.Drawing.Point(127, 208);
			this.btnDraw.Name = "btnDraw";
			this.btnDraw.Size = new System.Drawing.Size(75, 39);
			this.btnDraw.TabIndex = 6;
			this.btnDraw.Text = "Draw On Surface";
			this.toolTip1.SetToolTip(this.btnDraw, "Tests using a surface as a render target\r\nby drawing directly to the surface.");
			this.btnDraw.UseVisualStyleBackColor = true;
			// 
			// btnDrawText
			// 
			this.btnDrawText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDrawText.Location = new System.Drawing.Point(208, 208);
			this.btnDrawText.Name = "btnDrawText";
			this.btnDrawText.Size = new System.Drawing.Size(75, 39);
			this.btnDrawText.TabIndex = 9;
			this.btnDrawText.Text = "Draw Text On Surface";
			this.toolTip1.SetToolTip(this.btnDrawText, "Tests using a surface as a render target\r\nby drawing directly to the surface.");
			this.btnDrawText.UseVisualStyleBackColor = true;
			// 
			// btnClearSurface
			// 
			this.btnClearSurface.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClearSurface.Location = new System.Drawing.Point(46, 208);
			this.btnClearSurface.Name = "btnClearSurface";
			this.btnClearSurface.Size = new System.Drawing.Size(75, 39);
			this.btnClearSurface.TabIndex = 8;
			this.btnClearSurface.Text = "Clear Surface";
			this.btnClearSurface.UseVisualStyleBackColor = true;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.btnDrawText);
			this.splitContainer1.Panel2.Controls.Add(this.label3);
			this.splitContainer1.Panel2.Controls.Add(this.btnClearSurface);
			this.splitContainer1.Panel2.Controls.Add(this.pictureBox3);
			this.splitContainer1.Panel2.Controls.Add(this.btnDraw);
			this.splitContainer1.Size = new System.Drawing.Size(517, 259);
			this.splitContainer1.SplitterDistance = 218;
			this.splitContainer1.TabIndex = 9;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.label1);
			this.splitContainer2.Panel1.Controls.Add(this.pictureBox1);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.label2);
			this.splitContainer2.Panel2.Controls.Add(this.pictureBox2);
			this.splitContainer2.Size = new System.Drawing.Size(218, 259);
			this.splitContainer2.SplitterDistance = 126;
			this.splitContainer2.TabIndex = 0;
			// 
			// MultipleRenderTargetExample
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(517, 259);
			this.Controls.Add(this.splitContainer1);
			this.Name = "MultipleRenderTargetExample";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Multiple Render Targets Example";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.Panel2.PerformLayout();
			this.splitContainer2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.PictureBox pictureBox1;
		public System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.PictureBox pictureBox3;
		private System.Windows.Forms.ToolTip toolTip1;
		public System.Windows.Forms.Button btnDraw;
		public System.Windows.Forms.Button btnClearSurface;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		public System.Windows.Forms.Button btnDrawText;
	}
}

