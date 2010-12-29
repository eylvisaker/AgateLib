using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace Tests.DisplayTests
{
	class FullscreenTest : IAgateTest
	{
		public string Name
		{
			get { return "Full Screen"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		string text = "Press Esc or Tilde to exit." + Environment.NewLine + "Starting Text";

		public void Main(string[] args)
		{
			using (AgateSetup setup = new AgateSetup(args))
			{
				setup.InitializeAll();
				if (setup.WasCanceled)
					return;

				DisplayWindow wind = DisplayWindow.CreateFullScreen("Hello World", 640, 480);
				Surface mySurface = new Surface("jellybean.png");

				Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);
				FontSurface font = FontSurface.AgateSans14;

				// Run the program while the window is open.
				while (Display.CurrentWindow.IsClosed == false && 
					Keyboard.Keys[KeyCode.Escape] == false && 
					Keyboard.Keys[KeyCode.Tilde] == false)
				{
					Display.BeginFrame();
					Display.Clear(Color.DarkGreen);

					font.DrawText(text);
					mySurface.Draw(Mouse.X, Mouse.Y);

					Display.EndFrame();
					Core.KeepAlive();
				}
			}
		}

		void Keyboard_KeyDown(InputEventArgs e)
		{
			text += e.KeyString;
		}

	}
}