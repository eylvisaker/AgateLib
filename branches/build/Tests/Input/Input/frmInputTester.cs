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
        AgateSetup setup = new AgateSetup();

        public Form1()
        {
            InitializeComponent();

            Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);
            Keyboard.KeyUp += new InputEventHandler(Keyboard_KeyUp);

            Mouse.MouseMove += new InputEventHandler(Mouse_MouseMove);
            Mouse.MouseDown += new InputEventHandler(Mouse_MouseDown);
            Mouse.MouseUp += new InputEventHandler(Mouse_MouseUp);
            Mouse.MouseDoubleClickEvent += new InputEventHandler(Mouse_MouseDoubleClickEvent);

            setup.AskUser = true;
            setup.Initialize(true, false, true);
            if (setup.WasCanceled)
                throw new Exception();

            new DisplayWindow(CreateWindowParams.FromControl(pictureBox1));

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

                string text =
                    "Axis Count: " + j.AxisCount + Environment.NewLine;

                for (int i = 0; i < j.AxisCount; i++)
                {
                    text += "Axis " + i.ToString() + ": " + j.Axes[i].ToString() + Environment.NewLine;
                }

                text += Environment.NewLine + 
                    "X: " + j.Xaxis.ToString() + Environment.NewLine +
                    "Y: " + j.Yaxis.ToString() + Environment.NewLine + 
                    Environment.NewLine + 
                    "Buttons: ";

                for (int i = 0; i < j.ButtonCount; i++)
                {
                    if (j.Buttons[i])
                        text += i.ToString();
                }


                lblJoystick.Text = text;

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
    }
}