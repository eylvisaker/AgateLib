using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Configuration;

namespace AgateLib.Tests.DisplayTests
{
	class FullScreenTest : IAgateTest
	{
		static List<Resolution> resolutions = new List<Resolution>
		{
			new Resolution(640, 480),
			new Resolution(800, 600),
			new Resolution(1024, 768),
			new Resolution(1280, 720),
			new Resolution(1920, 1080),
			new Resolution(3840, 1960)
		};

		string topText = @"Press Esc or Enter to exit.
Press arrow keys to adjust resolution
";
		string bottomText = $@"Press F1-F12 to change resolution
    F1 - {resolutions[0 % resolutions.Count]} - Retain Aspect Ratio
    F2 - {resolutions[1 % resolutions.Count]} - Retain Aspect Ratio
    F3 - {resolutions[2 % resolutions.Count]} - Retain Aspect Ratio
    F4 - {resolutions[3 % resolutions.Count]}- Retain Aspect Ratio
    F5 - {resolutions[4 % resolutions.Count]} - Retain Aspect Ratio
    F6 - {resolutions[5 % resolutions.Count]} - Retain Aspect Ratio
    F7 - {resolutions[6 % resolutions.Count]} - Stretch
    F8 - {resolutions[7 % resolutions.Count]} - Stretch
    F9 - {resolutions[8 % resolutions.Count]} - Stretch
    F10 - {resolutions[9 % resolutions.Count]} - Stretch
    F11 - {resolutions[10 % resolutions.Count]} - Stretch
    F12 - {resolutions[11 % resolutions.Count]} - Stretch";

		Point mousePosition;

		public string Name => "Full Screen";

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

			Size bottomSize = font.MeasureString(bottomText);
			Size topSize = font.MeasureString(topText + "abc");

			// Run the program while the window is open.
			while (Core.IsAlive &&
				Input.Unhandled.Keys[KeyCode.Escape] == false &&
				Input.Unhandled.Keys[KeyCode.Tilde] == false)
			{
				var mouseText = topText + $"Mouse: {mousePosition}";

				Display.BeginFrame();
				Display.Clear(Color.DarkGreen);

				font.DrawText(0, Display.CurrentWindow.Height - bottomSize.Height, topText);

				Display.FillRect(new Rectangle(0, 0, Display.CurrentWindow.Width, topSize.Height),
					Color.Maroon);
					
				font.DrawText(mouseText);

				mySurface.Draw(mousePosition.X, mousePosition.Y);

				Display.EndFrame();
				Core.KeepAlive();
				frames++;
			}
		}

		void Keyboard_KeyDown(object sender, AgateInputEventArgs e)
		{
			switch(e.KeyCode)
			{

			}
		}

		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
		}
	}
}