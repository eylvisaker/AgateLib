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

			Input.Unhandled.KeyDown += Keyboard_KeyDown;
			Input.Unhandled.KeyUp += Keyboard_KeyUp;

			Input.Unhandled.MouseWheel += Mouse_MouseWheel;
			Input.Unhandled.MouseMove += Mouse_MouseMove;
			Input.Unhandled.MouseDown += Mouse_MouseDown;
			Input.Unhandled.MouseUp += Mouse_MouseUp;
			Input.Unhandled.MouseDoubleClick += Mouse_MouseDoubleClickEvent;

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

			bool comma = false;
			for (int i = 0; i < j.ButtonCount; i++)
			{
				if (j.ButtonState[i])
				{
					if (comma)
						b.Append(",");
					b.Append(i.ToString());
					comma = true;
				}
			}

			b.AppendLine();
			b.Append("Hats:");

			for (int i = 0; i < j.HatCount; i++)
			{
				b.Append("    ");
				b.Append(i.ToString());
				b.Append(": ");
				b.Append(j.HatState[i].ToString());
				b.AppendLine();
			}

			label.Text = b.ToString();
		}

		void Mouse_MouseDoubleClickEvent(object sender, AgateInputEventArgs e)
		{
			lblMouseButton.Text = "Mouse Double Click " + e.MouseButton.ToString();
		}
		void Mouse_MouseUp(object sender, AgateInputEventArgs e)
		{
			lblMouseButton.Text = "Mouse Button Up " + e.MouseButton.ToString();
		}
		void Mouse_MouseDown(object sender, AgateInputEventArgs e)
		{
			lblMouseButton.Text = "Mouse Button Down " + e.MouseButton.ToString();
		}
		void Mouse_MouseMove(object sender, AgateInputEventArgs e)
		{
			lblMouseMove.Text = "Mouse Moved " + e.MousePosition.ToString();
		}
		void Mouse_MouseWheel(object sender, AgateInputEventArgs e)
		{
			lblMouseButton.Text = "Mouse Wheel " + e.MouseWheelDelta.ToString();
		}

		void Keyboard_KeyUp(object sender, AgateInputEventArgs e)
		{
			this.lblKeyPress.Text = "Released key " + e.KeyCode;
		}
		void Keyboard_KeyDown(object sender, AgateInputEventArgs e)
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
			Input.Unhandled.Keys.ReleaseAll();
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{

		}

	}
}