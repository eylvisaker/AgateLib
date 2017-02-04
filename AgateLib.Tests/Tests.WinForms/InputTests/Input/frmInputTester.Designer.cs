// The contents of this file are public domain.
// You may use them as you wish.
//
namespace AgateLib.Tests.InputTests.InputTester
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
			this.lblJoystick2 = new System.Windows.Forms.Label();
			this.lblJoystick3 = new System.Windows.Forms.Label();
			this.lblJoystick4 = new System.Windows.Forms.Label();
			this.lblJoystickEvent = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblJoystick = new System.Windows.Forms.Label();
			this.lblPressedKeys = new System.Windows.Forms.Label();
			this.agateRenderTarget1 = new AgateLib.Platform.WinForms.Controls.AgateRenderTarget();
			this.SuspendLayout();
			// 
			// lblKeyPress
			// 
			this.lblKeyPress.Location = new System.Drawing.Point(369, 89);
			this.lblKeyPress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblKeyPress.Name = "lblKeyPress";
			this.lblKeyPress.Size = new System.Drawing.Size(270, 26);
			this.lblKeyPress.TabIndex = 0;
			this.lblKeyPress.Text = "Pressed Key";
			// 
			// lblMouseMove
			// 
			this.lblMouseMove.Location = new System.Drawing.Point(18, 51);
			this.lblMouseMove.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblMouseMove.Name = "lblMouseMove";
			this.lblMouseMove.Size = new System.Drawing.Size(270, 25);
			this.lblMouseMove.TabIndex = 1;
			this.lblMouseMove.Text = "lblMouseMove";
			// 
			// lblMouseButton
			// 
			this.lblMouseButton.Location = new System.Drawing.Point(18, 92);
			this.lblMouseButton.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblMouseButton.Name = "lblMouseButton";
			this.lblMouseButton.Size = new System.Drawing.Size(286, 23);
			this.lblMouseButton.TabIndex = 2;
			this.lblMouseButton.Text = "lblMouseButton";
			// 
			// lblKeyEvent
			// 
			this.lblKeyEvent.AutoSize = true;
			this.lblKeyEvent.Location = new System.Drawing.Point(369, 14);
			this.lblKeyEvent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblKeyEvent.Name = "lblKeyEvent";
			this.lblKeyEvent.Size = new System.Drawing.Size(80, 20);
			this.lblKeyEvent.TabIndex = 4;
			this.lblKeyEvent.Text = "Key Event";
			// 
			// lblKeyString
			// 
			this.lblKeyString.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblKeyString.Location = new System.Drawing.Point(370, 119);
			this.lblKeyString.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblKeyString.Name = "lblKeyString";
			this.lblKeyString.Size = new System.Drawing.Size(320, 20);
			this.lblKeyString.TabIndex = 5;
			this.lblKeyString.Text = "Pressed Key String []";
			// 
			// lblJoystick1
			// 
			this.lblJoystick1.AutoSize = true;
			this.lblJoystick1.Location = new System.Drawing.Point(369, 171);
			this.lblJoystick1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblJoystick1.Name = "lblJoystick1";
			this.lblJoystick1.Size = new System.Drawing.Size(97, 20);
			this.lblJoystick1.TabIndex = 8;
			this.lblJoystick1.Text = "Joystick Info";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(560, 7);
			this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(129, 35);
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
			// lblJoystick2
			// 
			this.lblJoystick2.AutoSize = true;
			this.lblJoystick2.Location = new System.Drawing.Point(549, 171);
			this.lblJoystick2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblJoystick2.Name = "lblJoystick2";
			this.lblJoystick2.Size = new System.Drawing.Size(97, 20);
			this.lblJoystick2.TabIndex = 11;
			this.lblJoystick2.Text = "Joystick Info";
			// 
			// lblJoystick3
			// 
			this.lblJoystick3.AutoSize = true;
			this.lblJoystick3.Location = new System.Drawing.Point(717, 171);
			this.lblJoystick3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblJoystick3.Name = "lblJoystick3";
			this.lblJoystick3.Size = new System.Drawing.Size(97, 20);
			this.lblJoystick3.TabIndex = 12;
			this.lblJoystick3.Text = "Joystick Info";
			// 
			// lblJoystick4
			// 
			this.lblJoystick4.AutoSize = true;
			this.lblJoystick4.Location = new System.Drawing.Point(872, 171);
			this.lblJoystick4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblJoystick4.Name = "lblJoystick4";
			this.lblJoystick4.Size = new System.Drawing.Size(97, 20);
			this.lblJoystick4.TabIndex = 13;
			this.lblJoystick4.Text = "Joystick Info";
			// 
			// lblJoystickEvent
			// 
			this.lblJoystickEvent.AutoSize = true;
			this.lblJoystickEvent.Location = new System.Drawing.Point(717, 14);
			this.lblJoystickEvent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblJoystickEvent.Name = "lblJoystickEvent";
			this.lblJoystickEvent.Size = new System.Drawing.Size(110, 20);
			this.lblJoystickEvent.TabIndex = 14;
			this.lblJoystickEvent.Text = "Joystick Event";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(18, 14);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(102, 20);
			this.label1.TabIndex = 15;
			this.label1.Text = "Mouse Event";
			// 
			// lblJoystick
			// 
			this.lblJoystick.Location = new System.Drawing.Point(717, 49);
			this.lblJoystick.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblJoystick.Name = "lblJoystick";
			this.lblJoystick.Size = new System.Drawing.Size(254, 51);
			this.lblJoystick.TabIndex = 16;
			this.lblJoystick.Text = "Joystick Info Appears Here";
			// 
			// lblPressedKeys
			// 
			this.lblPressedKeys.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPressedKeys.Location = new System.Drawing.Point(370, 139);
			this.lblPressedKeys.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblPressedKeys.Name = "lblPressedKeys";
			this.lblPressedKeys.Size = new System.Drawing.Size(320, 32);
			this.lblPressedKeys.TabIndex = 17;
			this.lblPressedKeys.Text = "Pressed Keys";
			// 
			// agateRenderTarget1
			// 
			this.agateRenderTarget1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.agateRenderTarget1.Location = new System.Drawing.Point(18, 171);
			this.agateRenderTarget1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.agateRenderTarget1.Name = "agateRenderTarget1";
			this.agateRenderTarget1.Size = new System.Drawing.Size(300, 308);
			this.agateRenderTarget1.TabIndex = 10;
			// 
			// frmInputTester
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(988, 502);
			this.Controls.Add(this.lblPressedKeys);
			this.Controls.Add(this.lblJoystick);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblJoystickEvent);
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
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
		private AgateLib.Platform.WinForms.Controls.AgateRenderTarget agateRenderTarget1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label lblJoystick2;
		private System.Windows.Forms.Label lblJoystick3;
		private System.Windows.Forms.Label lblJoystick4;
		private System.Windows.Forms.Label lblJoystickEvent;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblJoystick;
		private System.Windows.Forms.Label lblPressedKeys;
	}
}

