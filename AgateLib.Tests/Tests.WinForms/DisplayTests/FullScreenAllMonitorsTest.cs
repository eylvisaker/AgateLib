using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Configuration;

namespace AgateLib.Tests.DisplayTests
{
	class FullscreenAllMonitorsTest : IAgateTest
	{
		string text = "Press Esc or Tilde to exit.\nStarting Text";
		Point mousePosition;

		public string Name => "Full Screen All Monitors";

		public string Category => "Display";

		public AgateConfig Configuration { get; set; }

		public void Run()
		{
			DisplayWindow wind = DisplayWindow.CreateFullScreen("Hello World", 640, 480);
			Surface mySurface = new Surface("Images/jellybean.png");

			Input.Unhandled.KeyDown += Keyboard_KeyDown;
			Input.Unhandled.MouseMove += (sender, e) => mousePosition = e.MousePosition;

			IFont font = Font.AgateSans;

			int frames = 1;

			// Run the program while the window is open.
			while (Display.CurrentWindow.IsClosed == false &&
				Input.Unhandled.Keys[KeyCode.Escape] == false &&
				Input.Unhandled.Keys[KeyCode.Tilde] == false)
			{
				Display.BeginFrame();
				Display.Clear(Color.DarkGreen);

				font.DrawText(text);
				font.DrawText(0, font.FontHeight, $"Mouse Location: {mousePosition}");
				font.DrawText(0, 480 - font.FontHeight, $"Frames: {frames}");

				mySurface.Draw(mousePosition.X, mousePosition.Y);

				Display.EndFrame();
				Core.KeepAlive();
				frames++;
			}
		}

		void Keyboard_KeyDown(object sender, AgateInputEventArgs e)
		{
			text += e.KeyString;
		}

		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
		}
	}
}