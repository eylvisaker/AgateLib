using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Platform.WinForms.Resources;
using AgateLib.Platform.WinForms.ApplicationModels;

namespace AgateLib.Testing.DisplayTests
{
	class DisplayWindowEvents : IAgateTest
	{
		public string Name
		{
			get { return "Display Window Events"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		string text;
		bool closedEvent;
		bool closingEvent;

		public void Main(string[] args)
		{
			new PassiveModel(args).Run( () =>
			{
				DisplayWindow wind = DisplayWindow.CreateWindowed("Display Window Events", 640, 480, true);

				wind.Resize += new EventHandler(wind_Resize);
				wind.Closed += new EventHandler(wind_Closed);
				wind.Closing += new CancelEventHandler(wind_Closing);

				while (wind.IsClosed == false)
				{
					Display.BeginFrame();
					Display.Clear();

					BuiltinResources.AgateSans24.DrawText(text);

					Display.EndFrame();
					Core.KeepAlive();
				}
			});

			if (closedEvent == false)
			{
				System.Windows.Forms.MessageBox.Show(
					"Closed event did not fire!");
			}
		}

		void wind_Closing(object sender, ref bool cancel)
		{
			if (closingEvent == false)
			{
				text = "Closing event fired!" + Environment.NewLine + "Press close again to exit test.";

				closingEvent = true;
				cancel = true;
			}
		}

		void wind_Closed(object sender, EventArgs e)
		{
			closedEvent = true;
		}

		void wind_Resize(object sender, EventArgs e)
		{
			text = "Resize event fired!";
		}

	}
}
