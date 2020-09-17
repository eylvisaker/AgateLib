using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace Tests.DisplayTests
{
	class HelloWorldProgram : IAgateTest
	{
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
			using (AgateSetup setup = new AgateSetup(args))
			{
				setup.InitializeAll();
				if (setup.WasCanceled)
					return;

				DisplayWindow wind = DisplayWindow.CreateFullScreen("Hello World", 640, 480);
				Surface mySurface = new Surface("jellybean.png");

				// Run the program while the window is open.
				while (!(Display.CurrentWindow.IsClosed || Keyboard.Keys[KeyCode.Escape]))
				{
					Display.BeginFrame();
					Display.Clear(Color.DarkGreen);
					mySurface.Draw(Mouse.X, Mouse.Y);
					Display.EndFrame();
					Core.KeepAlive();
				}
			}
		}

	}
}