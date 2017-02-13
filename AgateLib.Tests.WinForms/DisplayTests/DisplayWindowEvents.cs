using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;

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

		public void Run(string[] args)
		{
			using (var window = new DisplayWindowBuilder(args)
				.BackbufferSize(640, 480)
				.QuitOnClose()
				.AllowResize()
				.AutoResizeBackBuffer()
				.Build())
			{
				window.Resize += wind_Resize;
				window.Closed += wind_Closed;
				window.Closing += wind_Closing;

				while (window.IsClosed == false)
				{
					Display.BeginFrame();
					Display.Clear(Color.Blue);

					Font.AgateSans.Size = 12;
					Font.AgateSans.DrawText(instructionText + count);
					Font.AgateSans.DrawText(0, Font.AgateSans.FontHeight, text);

					Display.EndFrame();
					AgateApp.KeepAlive();
				}

				if (closedEvent == false)
				{
					System.Windows.Forms.MessageBox.Show(
						"Closed event did not fire!");
				}
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
