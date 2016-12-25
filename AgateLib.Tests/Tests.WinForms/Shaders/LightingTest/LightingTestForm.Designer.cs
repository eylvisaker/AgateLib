namespace AgateLib.Tests.Shaders.LightingTest
{
	partial class LightingTestForm
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
			this.chkSurfaceGradient = new System.Windows.Forms.CheckBox();
			this.nudAngle = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.btnDiffuse = new System.Windows.Forms.Button();
			this.btnAmbient = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.chkMoveLight = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.enableShader = new System.Windows.Forms.CheckBox();
			this.lblFPS = new System.Windows.Forms.Label();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.agateRenderTarget1 = new AgateLib.Platform.WinForms.Controls.AgateRenderTarget();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudAngle)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.enableShader);
			this.panel1.Controls.Add(this.chkSurfaceGradient);
			this.panel1.Controls.Add(this.nudAngle);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.btnDiffuse);
			this.panel1.Controls.Add(this.btnAmbient);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.chkMoveLight);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Location = new System.Drawing.Point(385, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(126, 352);
			this.panel1.TabIndex = 0;
			// 
			// chkSurfaceGradient
			// 
			this.chkSurfaceGradient.AutoSize = true;
			this.chkSurfaceGradient.Location = new System.Drawing.Point(6, 220);
			this.chkSurfaceGradient.Name = "chkSurfaceGradient";
			this.chkSurfaceGradient.Size = new System.Drawing.Size(106, 17);
			this.chkSurfaceGradient.TabIndex = 4;
			this.chkSurfaceGradient.Text = "Surface Gradient";
			this.chkSurfaceGradient.UseVisualStyleBackColor = true;
			// 
			// nudAngle
			// 
			this.nudAngle.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.nudAngle.Location = new System.Drawing.Point(51, 183);
			this.nudAngle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
			this.nudAngle.Name = "nudAngle";
			this.nudAngle.Size = new System.Drawing.Size(51, 20);
			this.nudAngle.TabIndex = 9;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 185);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(39, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "Rotate";
			// 
			// btnDiffuse
			// 
			this.btnDiffuse.BackColor = System.Drawing.Color.White;
			this.btnDiffuse.Location = new System.Drawing.Point(51, 114);
			this.btnDiffuse.Name = "btnDiffuse";
			this.btnDiffuse.Size = new System.Drawing.Size(20, 23);
			this.btnDiffuse.TabIndex = 7;
			this.btnDiffuse.UseVisualStyleBackColor = false;
			this.btnDiffuse.Click += new System.EventHandler(this.btnDiffuse_Click);
			// 
			// btnAmbient
			// 
			this.btnAmbient.BackColor = System.Drawing.Color.Green;
			this.btnAmbient.Location = new System.Drawing.Point(51, 143);
			this.btnAmbient.Name = "btnAmbient";
			this.btnAmbient.Size = new System.Drawing.Size(20, 23);
			this.btnAmbient.TabIndex = 6;
			this.btnAmbient.UseVisualStyleBackColor = false;
			this.btnAmbient.Click += new System.EventHandler(this.btnAmbient_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 149);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(45, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Ambient";
			// 
			// chkMoveLight
			// 
			this.chkMoveLight.AutoSize = true;
			this.chkMoveLight.Checked = true;
			this.chkMoveLight.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMoveLight.Location = new System.Drawing.Point(3, 80);
			this.chkMoveLight.Name = "chkMoveLight";
			this.chkMoveLight.Size = new System.Drawing.Size(79, 17);
			this.chkMoveLight.TabIndex = 1;
			this.chkMoveLight.Text = "Move Light";
			this.chkMoveLight.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 119);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Diffuse";
			// 
			// enableShader
			// 
			this.enableShader.AutoSize = true;
			this.enableShader.Checked = true;
			this.enableShader.CheckState = System.Windows.Forms.CheckState.Checked;
			this.enableShader.Location = new System.Drawing.Point(3, 3);
			this.enableShader.Name = "enableShader";
			this.enableShader.Size = new System.Drawing.Size(101, 17);
			this.enableShader.TabIndex = 0;
			this.enableShader.Text = "Enable Shading";
			this.enableShader.UseVisualStyleBackColor = true;
			// 
			// lblFPS
			// 
			this.lblFPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblFPS.Location = new System.Drawing.Point(12, 354);
			this.lblFPS.Name = "lblFPS";
			this.lblFPS.Size = new System.Drawing.Size(162, 23);
			this.lblFPS.TabIndex = 3;
			this.lblFPS.Text = "FPS:";
			// 
			// agateRenderTarget1
			// 
			this.agateRenderTarget1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.agateRenderTarget1.Location = new System.Drawing.Point(0, 0);
			this.agateRenderTarget1.Name = "agateRenderTarget1";
			this.agateRenderTarget1.Size = new System.Drawing.Size(379, 325);
			this.agateRenderTarget1.TabIndex = 0;
			// 
			// LightingTestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(514, 376);
			this.Controls.Add(this.lblFPS);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.agateRenderTarget1);
			this.Name = "LightingTestForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Lighting Test";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudAngle)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		public System.Windows.Forms.CheckBox enableShader;
		public AgateLib.Platform.WinForms.Controls.AgateRenderTarget agateRenderTarget1;
		public System.Windows.Forms.CheckBox chkMoveLight;
		public System.Windows.Forms.Label lblFPS;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ColorDialog colorDialog1;
		public System.Windows.Forms.Button btnDiffuse;
		public System.Windows.Forms.Button btnAmbient;
		private System.Windows.Forms.Label label4;
		public System.Windows.Forms.NumericUpDown nudAngle;
		public System.Windows.Forms.CheckBox chkSurfaceGradient;
	}
}

