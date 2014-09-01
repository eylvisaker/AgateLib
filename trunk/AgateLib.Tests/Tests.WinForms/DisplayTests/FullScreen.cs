using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Platform.WinForms.Resources;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.InputLib.Legacy;

namespace AgateLib.Testing.DisplayTests
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
			new PassiveModel(args).Run( () =>
			{
				DisplayWindow wind = DisplayWindow.CreateFullScreen("Hello World", 640, 480);
				Surface mySurface = new Surface("jellybean.png");

				Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);
				Font font = AgateLib.Assets.Fonts.AgateSans;

				int frames = 1;

				// Run the program while the window is open.
				while (Display.CurrentWindow.IsClosed == false &&
					Keyboard.Keys[KeyCode.Escape] == false &&
					Keyboard.Keys[KeyCode.Tilde] == false)
				{
					Display.BeginFrame();
					Display.Clear(Color.DarkGreen);

					font.DrawText(text);

					font.DrawText(0, 480 - font.FontHeight, "Frames: {0}", frames);

					mySurface.Draw(Mouse.X, Mouse.Y);

					Display.EndFrame();
					Core.KeepAlive();
					frames++;
				}
			});
		}

		void Keyboard_KeyDown(InputEventArgs e)
		{
			text += e.KeyString;
		}

	}
}