// The contents of this file are public domain.
// You may use them as you wish.
//
namespace Tests.MultipleWindows
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
			this.btnClearSurface = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.Location = new System.Drawing.Point(12, 38);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(136, 32);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Tag = "Red Window";
			// 
			// pictureBox2
			// 
			this.pictureBox2.Location = new System.Drawing.Point(12, 156);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(136, 70);
			this.pictureBox2.TabIndex = 1;
			this.pictureBox2.TabStop = false;
			this.pictureBox2.Tag = "Green Window";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(133, 26);
			this.label1.TabIndex = 2;
			this.label1.Text = "This window should be red\r\nwith a blue rectangle:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 127);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(148, 26);
			this.label2.TabIndex = 3;
			this.label2.Text = "This window should be green \r\nwith a yellow rectangle:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(179, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(142, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "This is drawn from a surface:";
			// 
			// pictureBox3
			// 
			this.pictureBox3.Location = new System.Drawing.Point(182, 35);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(200, 150);
			this.pictureBox3.TabIndex = 4;
			this.pictureBox3.TabStop = false;
			this.pictureBox3.Tag = "Red Window";
			// 
			// btnDraw
			// 
			this.btnDraw.Location = new System.Drawing.Point(307, 198);
			this.btnDraw.Name = "btnDraw";
			this.btnDraw.Size = new System.Drawing.Size(75, 39);
			this.btnDraw.TabIndex = 6;
			this.btnDraw.Text = "Draw On Surface";
			this.toolTip1.SetToolTip(this.btnDraw, "Tests using a surface as a render target\r\nby drawing directly to the surface.");
			this.btnDraw.UseVisualStyleBackColor = true;
			// 
			// btnClearSurface
			// 
			this.btnClearSurface.Location = new System.Drawing.Point(226, 198);
			this.btnClearSurface.Name = "btnClearSurface";
			this.btnClearSurface.Size = new System.Drawing.Size(75, 39);
			this.btnClearSurface.TabIndex = 8;
			this.btnClearSurface.Text = "Clear Surface";
			this.btnClearSurface.UseVisualStyleBackColor = true;
			// 
			// MultipleRenderTargetExample
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(401, 249);
			this.Controls.Add(this.btnClearSurface);
			this.Controls.Add(this.btnDraw);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.pictureBox3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.pictureBox1);
			this.Name = "MultipleRenderTargetExample";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Multiple Render Targets Example";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

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
	}
}

