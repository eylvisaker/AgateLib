﻿namespace AgateLib.Tests.CoreTests.PlatformDetection
{
	partial class PlatformDetection
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
			this.lblPlatform = new System.Windows.Forms.Label();
			this.lblRuntime = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblPlatform
			// 
			this.lblPlatform.AutoSize = true;
			this.lblPlatform.Location = new System.Drawing.Point(12, 9);
			this.lblPlatform.Name = "lblPlatform";
			this.lblPlatform.Size = new System.Drawing.Size(45, 13);
			this.lblPlatform.TabIndex = 0;
			this.lblPlatform.Text = "Platform";
			// 
			// lblRuntime
			// 
			this.lblRuntime.AutoSize = true;
			this.lblRuntime.Location = new System.Drawing.Point(12, 40);
			this.lblRuntime.Name = "lblRuntime";
			this.lblRuntime.Size = new System.Drawing.Size(83, 13);
			this.lblRuntime.TabIndex = 1;
			this.lblRuntime.Text = "DotNet Runtime";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(197, 69);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Close";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// PlatformDetection
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 104);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.lblRuntime);
			this.Controls.Add(this.lblPlatform);
			this.Name = "PlatformDetection";
			this.Text = "Platform Detection";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblPlatform;
		private System.Windows.Forms.Label lblRuntime;
		private System.Windows.Forms.Button button1;
	}
}