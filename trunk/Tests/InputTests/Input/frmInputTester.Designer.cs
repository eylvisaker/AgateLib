// The contents of this file are public domain.
// You may use them as you wish.
//
namespace Tests.InputTester
{
	partial class frmInputTester
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
			this.lblKeyPress = new System.Windows.Forms.Label();
			this.lblMouseMove = new System.Windows.Forms.Label();
			this.lblMouseButton = new System.Windows.Forms.Label();
			this.lblKeyEvent = new System.Windows.Forms.Label();
			this.lblKeyString = new System.Windows.Forms.Label();
			this.lblJoystick1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.agateRenderTarget1 = new AgateLib.Platform.WindowsForms.WinForms.AgateRenderTarget();
			this.lblJoystick2 = new System.Windows.Forms.Label();
			this.lblJoystick3 = new System.Windows.Forms.Label();
			this.lblJoystick4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblKeyPress
			// 
			this.lblKeyPress.Location = new System.Drawing.Point(12, 9);
			this.lblKeyPress.Name = "lblKeyPress";
			this.lblKeyPress.Size = new System.Drawing.Size(180, 17);
			this.lblKeyPress.TabIndex = 0;
			this.lblKeyPress.Text = "Pressed Key";
			// 
			// lblMouseMove
			// 
			this.lblMouseMove.Location = new System.Drawing.Point(12, 64);
			this.lblMouseMove.Name = "lblMouseMove";
			this.lblMouseMove.Size = new System.Drawing.Size(180, 16);
			this.lblMouseMove.TabIndex = 1;
			this.lblMouseMove.Text = "label1";
			// 
			// lblMouseButton
			// 
			this.lblMouseButton.Location = new System.Drawing.Point(12, 91);
			this.lblMouseButton.Name = "lblMouseButton";
			this.lblMouseButton.Size = new System.Drawing.Size(191, 15);
			this.lblMouseButton.TabIndex = 2;
			this.lblMouseButton.Text = "label1";
			// 
			// lblKeyEvent
			// 
			this.lblKeyEvent.AutoSize = true;
			this.lblKeyEvent.Location = new System.Drawing.Point(246, 9);
			this.lblKeyEvent.Name = "lblKeyEvent";
			this.lblKeyEvent.Size = new System.Drawing.Size(56, 13);
			this.lblKeyEvent.TabIndex = 4;
			this.lblKeyEvent.Text = "Key Event";
			// 
			// lblKeyString
			// 
			this.lblKeyString.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblKeyString.Location = new System.Drawing.Point(12, 32);
			this.lblKeyString.Name = "lblKeyString";
			this.lblKeyString.Size = new System.Drawing.Size(213, 21);
			this.lblKeyString.TabIndex = 5;
			this.lblKeyString.Text = "Pressed Key String []";
			// 
			// lblJoystick1
			// 
			this.lblJoystick1.AutoSize = true;
			this.lblJoystick1.Location = new System.Drawing.Point(246, 111);
			this.lblJoystick1.Name = "lblJoystick1";
			this.lblJoystick1.Size = new System.Drawing.Size(66, 13);
			this.lblJoystick1.TabIndex = 8;
			this.lblJoystick1.Text = "Joystick Info";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(249, 80);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(86, 23);
			this.button1.TabIndex = 9;
			this.button1.Text = "Clear All Keys";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 20;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// agateRenderTarget1
			// 
			this.agateRenderTarget1.Location = new System.Drawing.Point(12, 111);
			this.agateRenderTarget1.Name = "agateRenderTarget1";
			this.agateRenderTarget1.Size = new System.Drawing.Size(200, 200);
			this.agateRenderTarget1.TabIndex = 10;
			// 
			// lblJoystick2
			// 
			this.lblJoystick2.AutoSize = true;
			this.lblJoystick2.Location = new System.Drawing.Point(366, 111);
			this.lblJoystick2.Name = "lblJoystick2";
			this.lblJoystick2.Size = new System.Drawing.Size(66, 13);
			this.lblJoystick2.TabIndex = 11;
			this.lblJoystick2.Text = "Joystick Info";
			// 
			// lblJoystick3
			// 
			this.lblJoystick3.AutoSize = true;
			this.lblJoystick3.Location = new System.Drawing.Point(478, 111);
			this.lblJoystick3.Name = "lblJoystick3";
			this.lblJoystick3.Size = new System.Drawing.Size(66, 13);
			this.lblJoystick3.TabIndex = 12;
			this.lblJoystick3.Text = "Joystick Info";
			// 
			// lblJoystick4
			// 
			this.lblJoystick4.AutoSize = true;
			this.lblJoystick4.Location = new System.Drawing.Point(581, 111);
			this.lblJoystick4.Name = "lblJoystick4";
			this.lblJoystick4.Size = new System.Drawing.Size(66, 13);
			this.lblJoystick4.TabIndex = 13;
			this.lblJoystick4.Text = "Joystick Info";
			// 
			// frmInputTester
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(659, 326);
			this.Controls.Add(this.lblJoystick4);
			this.Controls.Add(this.lblJoystick3);
			this.Controls.Add(this.lblJoystick2);
			this.Controls.Add(this.agateRenderTarget1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.lblJoystick1);
			this.Controls.Add(this.lblKeyString);
			this.Controls.Add(this.lblKeyEvent);
			this.Controls.Add(this.lblMouseButton);
			this.Controls.Add(this.lblMouseMove);
			this.Controls.Add(this.lblKeyPress);
			this.Name = "frmInputTester";
			this.Text = "Input Tester";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblKeyPress;
		private System.Windows.Forms.Label lblMouseMove;
		private System.Windows.Forms.Label lblMouseButton;
		private System.Windows.Forms.Label lblKeyEvent;
		private System.Windows.Forms.Label lblKeyString;
		private System.Windows.Forms.Label lblJoystick1;
		private System.Windows.Forms.Button button1;
		private AgateLib.Platform.WindowsForms.WinForms.AgateRenderTarget agateRenderTarget1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label lblJoystick2;
		private System.Windows.Forms.Label lblJoystick3;
		private System.Windows.Forms.Label lblJoystick4;
	}
}

