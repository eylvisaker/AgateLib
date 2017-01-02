using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Platform.WinForms;

namespace BasicInitialization
{
	static class BasicInitializationProgram
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			using (var setup = new AgateSetupWinForms(args))
			{
				// At minimum, we must specify to the setup object what the desired size of the display window is.
				setup.DesiredDisplayWindowResolution = new Size(500, 400);

				// This call completes initialization of AgateLib and allows us to begin drawing.
				setup.InitializeAgateLib();

				// Register a key press handler. This will terminate the application if the escape key is pressed.
				Input.Unhandled.KeyDown += (sender, e) =>
				{
					if (e.KeyCode == KeyCode.Escape)
						Display.CurrentWindow.Dispose();
				};

				// Run the game loop
				while (Display.CurrentWindow.IsClosed == false)
				{
					// All drawing calls must be contained between Display.BeginFrame and Display.EndFrame calls.
					Display.BeginFrame();
					Display.Clear(Color.Gray);

					Point point = new Point(10, 10);
					Size size = new Size(15, 15);

					for(int i = 0; i < 36; i++)
					{
						Rectangle dest = new Rectangle(point, size);
						Display.FillRect(dest, Color.FromHsv(i * 10, 1, 1));

						point.X += 10;
						point.Y += 10;
					}

					Display.EndFrame();

					// A call to Core.KeepAlive is required to process input events and play nice with the OS.
					Core.KeepAlive();
				}
			}
		}
	}
}
