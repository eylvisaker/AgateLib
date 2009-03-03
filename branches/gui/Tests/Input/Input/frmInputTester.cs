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
using AgateLib.InputLib.Old;

namespace InputTester
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();

            Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);
            Keyboard.KeyUp += new InputEventHandler(Keyboard_KeyUp);

            Mouse.MouseWheel += new InputEventHandler(Mouse_MouseWheel);
            Mouse.MouseMove += new InputEventHandler(Mouse_MouseMove);
            Mouse.MouseDown += new InputEventHandler(Mouse_MouseDown);
            Mouse.MouseUp += new InputEventHandler(Mouse_MouseUp);
            Mouse.MouseDoubleClick += new InputEventHandler(Mouse_MouseDoubleClickEvent);

            new DisplayWindow(CreateWindowParams.FromControl(agateRenderTarget1));

            Application.Idle += new EventHandler(Application_Idle);
        }

        void Application_Idle(object sender, EventArgs e)
        {
            while (this.Visible)
            {
                if (Input.JoystickCount == 0)
                    return;

                numericUpDown1.Maximum = Input.JoystickCount - 1;
                Joystick j = Input.Joysticks[(int)numericUpDown1.Value];

                StringBuilder b = new StringBuilder();
                b.AppendLine(j.Name);
                b.Append("Axis Count: ");
                b.AppendLine(j.AxisCount.ToString());

                for (int i = 0; i < j.AxisCount; i++)
                {
                    b.Append("Axis ");
                    b.Append(i.ToString());
                    b.Append(": ");
                    b.Append(j.Axes[i].ToString());
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
                    if (j.Buttons[i])
                        b.Append(i.ToString());
                }


                lblJoystick.Text = b.ToString();

                Core.KeepAlive();
            }
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
            this.lblKeyPress.Text = "Released key " + e.KeyID;
        }
        void Keyboard_KeyDown(InputEventArgs e)
        {
            this.lblKeyPress.Text = "Pressed key " + e.KeyID;
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