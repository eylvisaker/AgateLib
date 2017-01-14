﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgateLib;
using AgateLib.DisplayLib;

namespace Examples.Initialization.WindowsFormsInitialization
{
	public partial class Form1 : Form
	{
		DisplayWindow window;

		public Form1()
		{
			InitializeComponent();

			window = DisplayWindow.CreateFromControl(renderTarget);
		}

		private void Draw()
		{
			Display.BeginFrame();
			Display.Clear(AgateLib.Geometry.Color.Green);

			Display.FillRect(new AgateLib.Geometry.Rectangle(20, 20, 80, 80), AgateLib.Geometry.Color.Blue);

			Display.EndFrame();
			Core.KeepAlive();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			Draw();
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			timer1.Enabled = false;
		}
	}
}
