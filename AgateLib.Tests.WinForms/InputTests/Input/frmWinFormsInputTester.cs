// The contents of this file are public domain.
// You may use them as you wish.
//

using System;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using AgateLib.DisplayLib;
using AgateLib.InputLib;

namespace AgateLib.Tests.InputTests.Input
{
	public partial class frmInputTester : Form
	{
		private Label[] joystickLabels = new Label[4];

		public frmInputTester()
		{
			InitializeComponent();

			InputLib.Input.Unhandled.KeyDown += Keyboard_KeyDown;
			InputLib.Input.Unhandled.KeyUp += Keyboard_KeyUp;

			InputLib.Input.Unhandled.MouseWheel += Mouse_MouseWheel;
			InputLib.Input.Unhandled.MouseMove += Mouse_MouseMove;
			InputLib.Input.Unhandled.MouseDown += Mouse_MouseDown;
			InputLib.Input.Unhandled.MouseUp += Mouse_MouseUp;
			InputLib.Input.Unhandled.MouseDoubleClick += Mouse_MouseDoubleClickEvent;

			InputLib.Input.Unhandled.JoystickAxisChanged += Joystick_AxisChanged;
			InputLib.Input.Unhandled.JoystickButtonPressed += Joystick_ButtonPressed;
			InputLib.Input.Unhandled.JoystickButtonReleased += Joystick_ButtonReleased;
			InputLib.Input.Unhandled.JoystickHatChanged += Joystick_HatChanged;

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

			for (int i = 0; i < InputLib.Input.Joysticks.Count; i++ )
			{
				FillJoystickInfo(i, joystickLabels[i]);
			}

			timer1.Enabled = false;
			AgateApp.KeepAlive();

			timer1.Enabled = true;
		}

		private void FillJoystickInfo(int index, Label label)
		{
			IJoystick j = InputLib.Input.Joysticks[index];

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
				b.Append(j.AxisState(i));
				b.AppendLine();
			}

			b.AppendLine();

			b.Append("X: ");
			b.AppendLine(j.AxisState(0).ToString(CultureInfo.CurrentCulture));
			b.Append("Y: ");
			b.AppendLine(j.AxisState(1).ToString(CultureInfo.CurrentCulture));
			b.AppendLine();

			b.Append("Buttons: ");

			bool comma = false;
			for (int i = 0; i < j.ButtonCount; i++)
			{
				if (j.ButtonState(i))
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
				b.Append(j.HatState(i));
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

			UpdatePressedKeys();
		}
		void Keyboard_KeyDown(object sender, AgateInputEventArgs e)
		{
			this.lblKeyPress.Text = "Pressed key " + e.KeyCode;
			this.lblKeyString.Text = "Pressed key string [" + e.KeyString + "]";

			UpdatePressedKeys();
		}

		private void UpdatePressedKeys()
		{
			lblPressedKeys.Text = "Pressed Keys:\n" +
			                      string.Join(",", InputLib.Input.Unhandled.Keys.PressedKeys);
		}


		private void Joystick_AxisChanged(object sender, AgateInputEventArgs e)
		{
			lblJoystick.Text = $"Joystick {e.JoystickIndex} axis changed.";
		}
		private void Joystick_ButtonPressed(object sender, AgateInputEventArgs e)
		{
			lblJoystick.Text = $"Joystick {e.JoystickIndex} button {e.JoystickButtonIndex} pressed.";
		}
		private void Joystick_ButtonReleased(object sender, AgateInputEventArgs e)
		{
			lblJoystick.Text = $"Joystick {e.JoystickIndex} button {e.JoystickButtonIndex} released.";
		}
		private void Joystick_HatChanged(object sender, AgateInputEventArgs e)
		{
			lblJoystick.Text = $"Joystick {e.JoystickIndex} hat changed.";
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
			InputLib.Input.Unhandled.Keys.ReleaseAll();
		}
	}
}