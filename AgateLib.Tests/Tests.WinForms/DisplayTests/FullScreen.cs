using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Platform.WinForms.Resources;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.InputLib.Legacy;

namespace AgateLib.Tests.DisplayTests
{
	class FullscreenTest : ILegacyAgateTest
	{
		string text = "Press Esc or Tilde to exit.\nStarting Text";
		Point mousePosition;

		public string Name
		{
			get { return "Full Screen"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		public void Main(string[] args)
		{
			new PassiveModel(args).Run( () =>
			{
				DisplayWindow wind = DisplayWindow.CreateFullScreen("Hello World", 640, 480);
				Surface mySurface = new Surface("jellybean.png");

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

					font.DrawText(0, 480 - font.FontHeight, "Frames: {0}", frames);

					mySurface.Draw(mousePosition.X, mousePosition.Y);

					Display.EndFrame();
					Core.KeepAlive();
					frames++;
				}
			});
		}

		void Keyboard_KeyDown(object sender, AgateInputEventArgs e)
		{
			text += e.KeyString;
		}
	}
}