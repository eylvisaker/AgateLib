// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.InputLib.Legacy;

namespace AgateLib.Testing.InputTests.InputTester
{
	public partial class frmInputTester : Form
	{
		private Label[] joystickLabels = new Label[4];

		public frmInputTester()
		{
			InitializeComponent();

			Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);
			Keyboard.KeyUp += new InputEventHandler(Keyboard_KeyUp);

			Mouse.MouseWheel += new InputEventHandler(Mouse_MouseWheel);
			Mouse.MouseMove += new InputEventHandler(Mouse_MouseMove);
			Mouse.MouseDown += new InputEventHandler(Mouse_MouseDown);
			Mouse.MouseUp += new InputEventHandler(Mouse_MouseUp);
			Mouse.MouseDoubleClick += new InputEventHandler(Mouse_MouseDoubleClickEvent);

			DisplayWindow.CreateFromControl(agateRenderTarget1);

			joystickLabels[0] = lblJoystick1;
			joystickLabels[1] = lblJoystick2;
			joystickLabels[2] = lblJoystick3;
			joystickLabels[3] = lblJoystick4;

		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			Application_Idle(sender, e);
		}
		void Application_Idle(object sender, EventArgs e)
		{
			if (Visible == false)
				return;

			for (int i = 0; i < JoystickInput.Joysticks.Count; i++ )
			{
				FillJoystickInfo(i, joystickLabels[i]);
			}

			timer1.Enabled = false;
			Core.KeepAlive();

			timer1.Enabled = true;
		}

		private void FillJoystickInfo(int index, Label label)
		{
			Joystick j = JoystickInput.Joysticks[index];

			StringBuilder b = new StringBuilder();
			b.Append("Joystick ");
			b.AppendLine(index.ToString());
			b.AppendLine(j.Name);
			b.Append("Axis Count: ");
			b.AppendLine(j.AxisCount.ToString());

			for (int i = 0; i < j.AxisCount; i++)
			{
				b.Append("Axis ");
				b.Append(i.ToString());
				b.Append(": ");
				b.Append(j.GetAxisValue(i).ToString());
				b.AppendLine();
			}

			b.AppendLine();

			b.Append("X: ");
			b.AppendLine(j.Xaxis.ToString());
			b.Append("Y: ");
			b.AppendLine(j.Yaxis.ToString());
			b.AppendLine();

			b.Append("Buttons: ");

			for (int i = 0; i < j.ButtonCount; i++)
			{
				if (j.GetButtonState(i))
					b.Append(i.ToString());
			}

			b.AppendLine();
			b.Append("Hats:");

			for (int i = 0; i < j.HatCount; i++)
			{
				b.Append("    ");
				b.Append(i.ToString());
				b.Append(": ");
				b.Append(j.GetHatState(i).ToString());
				b.AppendLine();
			}

			label.Text = b.ToString();
		}

		void Mouse_MouseDoubleClickEvent(InputEventArgs e)
		{
			lblMouseButton.Text = "Mouse Double Click " + e.MouseButtons.ToString();
		}
		void Mouse_MouseUp(InputEventArgs e)
		{
			lblMouseButton.Text = "Mouse Button Up " + e.MouseButtons.ToString();
		}
		void Mouse_MouseDown(InputEventArgs e)
		{
			lblMouseButton.Text = "Mouse Button Down " + e.MouseButtons.ToString();
		}
		void Mouse_MouseMove(InputEventArgs e)
		{
			lblMouseMove.Text = "Mouse Moved " + e.MousePosition.ToString();
		}
		void Mouse_MouseWheel(InputEventArgs e)
		{
			lblMouseButton.Text = "Mouse Wheel " + e.WheelDelta.ToString();
		}

		void Keyboard_KeyUp(InputEventArgs e)
		{
			this.lblKeyPress.Text = "Released key " + e.KeyCode;
		}
		void Keyboard_KeyDown(InputEventArgs e)
		{
			this.lblKeyPress.Text = "Pressed key " + e.KeyCode;
			this.lblKeyString.Text = "Pressed key string [" + e.KeyString + "]";
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			Display.Dispose();
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			lblKeyEvent.Text = "KeyCode: " + e.KeyCode.ToString() + "\r\n" +
				"KeyData: " + e.KeyData.ToString() + "\r\n" +
				"KeyValue: " + e.KeyValue.ToString() + "\r\n" +
				"KeyValueChar: " + ((char)e.KeyValue).ToString();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Keyboard.ReleaseAllKeys();
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{

		}

	}
}