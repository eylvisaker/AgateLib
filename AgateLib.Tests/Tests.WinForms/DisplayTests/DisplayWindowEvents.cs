using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Platform.WinForms.Resources;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.Configuration;

namespace AgateLib.Tests.DisplayTests
{
	class DisplayWindowEvents : IAgateTest
	{
		int count = 0;
		string instructionText = "This window responds to resize, closed and closing events. ";
		string text;
		bool closedEvent;
		bool closingEvent;

		public string Name => "Display Window Events";

		public string Category => "Display";

		public AgateConfig Configuration { get; set; }

		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
		}

		public void Run()
		{
			DisplayWindow wind = DisplayWindow.CreateWindowed("Display Window Events", 640, 480, true);

			wind.Resize += new EventHandler(wind_Resize);
			wind.Closed += new EventHandler(wind_Closed);
			wind.Closing += new CancelEventHandler(wind_Closing);

			while (wind.IsClosed == false)
			{
				Display.BeginFrame();
				Display.Clear();

				DefaultAssets.Fonts.AgateSans.Size = 12;
				DefaultAssets.Fonts.AgateSans.DrawText(instructionText + count);
				DefaultAssets.Fonts.AgateSans.DrawText(0, DefaultAssets.Fonts.AgateSans.FontHeight, text);

				Display.EndFrame();
				Core.KeepAlive();
			}

			if (closedEvent == false)
			{
				System.Windows.Forms.MessageBox.Show(
					"Closed event did not fire!");
			}
		}

		void wind_Closing(object sender, ref bool cancel)
		{
			count++;
			if (closingEvent == false)
			{
				text = "Closing event fired!\nPress close again to exit test.";

				closingEvent = true;
				cancel = true;
			}
		}

		void wind_Closed(object sender, EventArgs e)
		{
			count++;
			closedEvent = true;
		}

		void wind_Resize(object sender, EventArgs e)
		{
			count++;
			text = "Resize event fired!";
		}
	}
}
