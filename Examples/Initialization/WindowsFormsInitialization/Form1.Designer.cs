namespace WindowsFormsInitialization
{
	partial class Form1
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
			this.renderTarget = new AgateLib.Platform.WinForms.Controls.AgateRenderTarget();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// renderTarget
			// 
			this.renderTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.renderTarget.Location = new System.Drawing.Point(12, 12);
			this.renderTarget.Name = "renderTarget";
			this.renderTarget.Size = new System.Drawing.Size(278, 257);
			this.renderTarget.TabIndex = 0;
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(302, 281);
			this.Controls.Add(this.renderTarget);
			this.Name = "Form1";
			this.Text = "Windows Forms Initialization";
			this.ResumeLayout(false);

		}

		#endregion

		private AgateLib.Platform.WinForms.Controls.AgateRenderTarget renderTarget;
		private System.Windows.Forms.Timer timer1;
	}
}

