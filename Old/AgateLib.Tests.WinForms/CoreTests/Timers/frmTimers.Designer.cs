// The contents of this file are public domain.
// You may use them as you wish.
//
namespace AgateLib.Tests.CoreTests.Timers
{
	partial class frmTimerTester
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTimerTester));
			this.label1 = new System.Windows.Forms.Label();
			this.txtTimer = new System.Windows.Forms.TextBox();
			this.btnPause = new System.Windows.Forms.Button();
			this.btnResume = new System.Windows.Forms.Button();
			this.btnForceResume = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.btnCustomForceResume = new System.Windows.Forms.Button();
			this.btnCustomResume = new System.Windows.Forms.Button();
			this.btnCustomPause = new System.Windows.Forms.Button();
			this.btnCustomReset = new System.Windows.Forms.Button();
			this.txtCustomTimer = new System.Windows.Forms.TextBox();
			this.btnPauseAll = new System.Windows.Forms.Button();
			this.btnResumeAll = new System.Windows.Forms.Button();
			this.btnForceAll = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.txtEnv = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtDeltaTime = new System.Windows.Forms.TextBox();
			this.txtDeltaTimeAvg = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(55, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(111, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Application Timer";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtTimer
			// 
			this.txtTimer.Location = new System.Drawing.Point(172, 12);
			this.txtTimer.Name = "txtTimer";
			this.txtTimer.Size = new System.Drawing.Size(100, 20);
			this.txtTimer.TabIndex = 1;
			this.txtTimer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// btnPause
			// 
			this.btnPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPause.Location = new System.Drawing.Point(287, 12);
			this.btnPause.Name = "btnPause";
			this.btnPause.Size = new System.Drawing.Size(75, 23);
			this.btnPause.TabIndex = 3;
			this.btnPause.Text = "Pause";
			this.btnPause.UseVisualStyleBackColor = true;
			this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
			// 
			// btnResume
			// 
			this.btnResume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnResume.Location = new System.Drawing.Point(287, 41);
			this.btnResume.Name = "btnResume";
			this.btnResume.Size = new System.Drawing.Size(75, 23);
			this.btnResume.TabIndex = 4;
			this.btnResume.Text = "Resume";
			this.btnResume.UseVisualStyleBackColor = true;
			this.btnResume.Click += new System.EventHandler(this.btnResume_Click);
			// 
			// btnForceResume
			// 
			this.btnForceResume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnForceResume.Location = new System.Drawing.Point(287, 70);
			this.btnForceResume.Name = "btnForceResume";
			this.btnForceResume.Size = new System.Drawing.Size(75, 23);
			this.btnForceResume.TabIndex = 5;
			this.btnForceResume.Text = "Force";
			this.btnForceResume.UseVisualStyleBackColor = true;
			this.btnForceResume.Click += new System.EventHandler(this.btnForceResume_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(72, 174);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(94, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Custom Timer";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnCustomForceResume
			// 
			this.btnCustomForceResume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCustomForceResume.Location = new System.Drawing.Point(287, 253);
			this.btnCustomForceResume.Name = "btnCustomForceResume";
			this.btnCustomForceResume.Size = new System.Drawing.Size(75, 23);
			this.btnCustomForceResume.TabIndex = 11;
			this.btnCustomForceResume.Text = "Force";
			this.btnCustomForceResume.UseVisualStyleBackColor = true;
			this.btnCustomForceResume.Click += new System.EventHandler(this.btnCustomForceResume_Click);
			// 
			// btnCustomResume
			// 
			this.btnCustomResume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCustomResume.Location = new System.Drawing.Point(287, 224);
			this.btnCustomResume.Name = "btnCustomResume";
			this.btnCustomResume.Size = new System.Drawing.Size(75, 23);
			this.btnCustomResume.TabIndex = 10;
			this.btnCustomResume.Text = "Resume";
			this.btnCustomResume.UseVisualStyleBackColor = true;
			this.btnCustomResume.Click += new System.EventHandler(this.btnCustomResume_Click);
			// 
			// btnCustomPause
			// 
			this.btnCustomPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCustomPause.Location = new System.Drawing.Point(287, 195);
			this.btnCustomPause.Name = "btnCustomPause";
			this.btnCustomPause.Size = new System.Drawing.Size(75, 23);
			this.btnCustomPause.TabIndex = 9;
			this.btnCustomPause.Text = "Pause";
			this.btnCustomPause.UseVisualStyleBackColor = true;
			this.btnCustomPause.Click += new System.EventHandler(this.btnCustomPause_Click);
			// 
			// btnCustomReset
			// 
			this.btnCustomReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCustomReset.Location = new System.Drawing.Point(287, 166);
			this.btnCustomReset.Name = "btnCustomReset";
			this.btnCustomReset.Size = new System.Drawing.Size(75, 23);
			this.btnCustomReset.TabIndex = 8;
			this.btnCustomReset.Text = "Reset";
			this.btnCustomReset.UseVisualStyleBackColor = true;
			this.btnCustomReset.Click += new System.EventHandler(this.btnCustomReset_Click);
			// 
			// txtCustomTimer
			// 
			this.txtCustomTimer.Location = new System.Drawing.Point(172, 169);
			this.txtCustomTimer.Name = "txtCustomTimer";
			this.txtCustomTimer.Size = new System.Drawing.Size(100, 20);
			this.txtCustomTimer.TabIndex = 7;
			this.txtCustomTimer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// btnPauseAll
			// 
			this.btnPauseAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnPauseAll.Location = new System.Drawing.Point(10, 336);
			this.btnPauseAll.Name = "btnPauseAll";
			this.btnPauseAll.Size = new System.Drawing.Size(75, 23);
			this.btnPauseAll.TabIndex = 12;
			this.btnPauseAll.Text = "Pause All";
			this.btnPauseAll.UseVisualStyleBackColor = true;
			this.btnPauseAll.Click += new System.EventHandler(this.btnPauseAll_Click);
			// 
			// btnResumeAll
			// 
			this.btnResumeAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnResumeAll.Location = new System.Drawing.Point(91, 336);
			this.btnResumeAll.Name = "btnResumeAll";
			this.btnResumeAll.Size = new System.Drawing.Size(75, 23);
			this.btnResumeAll.TabIndex = 13;
			this.btnResumeAll.Text = "Resume All";
			this.btnResumeAll.UseVisualStyleBackColor = true;
			this.btnResumeAll.Click += new System.EventHandler(this.btnResumeAll_Click);
			// 
			// btnForceAll
			// 
			this.btnForceAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnForceAll.Location = new System.Drawing.Point(172, 336);
			this.btnForceAll.Name = "btnForceAll";
			this.btnForceAll.Size = new System.Drawing.Size(114, 23);
			this.btnForceAll.TabIndex = 14;
			this.btnForceAll.Text = "Force Resume All";
			this.btnForceAll.UseVisualStyleBackColor = true;
			this.btnForceAll.Click += new System.EventHandler(this.btnForceAll_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(19, 38);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(147, 13);
			this.label3.TabIndex = 15;
			this.label3.Text = "- Environment.TickCount";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtEnv
			// 
			this.txtEnv.Location = new System.Drawing.Point(172, 35);
			this.txtEnv.Name = "txtEnv";
			this.txtEnv.Size = new System.Drawing.Size(100, 20);
			this.txtEnv.TabIndex = 16;
			this.txtEnv.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(12, 73);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(154, 13);
			this.label4.TabIndex = 17;
			this.label4.Text = "Sum of Display.DeltaTime ";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtDeltaTime
			// 
			this.txtDeltaTime.Location = new System.Drawing.Point(172, 70);
			this.txtDeltaTime.Name = "txtDeltaTime";
			this.txtDeltaTime.Size = new System.Drawing.Size(100, 20);
			this.txtDeltaTime.TabIndex = 18;
			this.txtDeltaTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// txtDeltaTimeAvg
			// 
			this.txtDeltaTimeAvg.Location = new System.Drawing.Point(172, 96);
			this.txtDeltaTimeAvg.Name = "txtDeltaTimeAvg";
			this.txtDeltaTimeAvg.Size = new System.Drawing.Size(100, 20);
			this.txtDeltaTimeAvg.TabIndex = 20;
			this.txtDeltaTimeAvg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(12, 99);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(152, 13);
			this.label5.TabIndex = 19;
			this.label5.Text = "Avg of Display.DeltaTime ";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(374, 371);
			this.Controls.Add(this.txtDeltaTimeAvg);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtDeltaTime);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtEnv);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnForceAll);
			this.Controls.Add(this.btnResumeAll);
			this.Controls.Add(this.btnPauseAll);
			this.Controls.Add(this.btnCustomForceResume);
			this.Controls.Add(this.btnCustomResume);
			this.Controls.Add(this.btnCustomPause);
			this.Controls.Add(this.btnCustomReset);
			this.Controls.Add(this.txtCustomTimer);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnForceResume);
			this.Controls.Add(this.btnResume);
			this.Controls.Add(this.btnPause);
			this.Controls.Add(this.txtTimer);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtTimer;
		private System.Windows.Forms.Button btnPause;
		private System.Windows.Forms.Button btnResume;
		private System.Windows.Forms.Button btnForceResume;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnCustomForceResume;
		private System.Windows.Forms.Button btnCustomResume;
		private System.Windows.Forms.Button btnCustomPause;
		private System.Windows.Forms.Button btnCustomReset;
		private System.Windows.Forms.TextBox txtCustomTimer;
		private System.Windows.Forms.Button btnPauseAll;
		private System.Windows.Forms.Button btnResumeAll;
		private System.Windows.Forms.Button btnForceAll;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtEnv;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtDeltaTime;
		private System.Windows.Forms.TextBox txtDeltaTimeAvg;
		private System.Windows.Forms.Label label5;
	}
}

