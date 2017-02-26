using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform.WinForms;

namespace Examples.Initialization.WindowedAllMonitors
{
	static class WindowedAllMonitorsProgram
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			using (new AgateWinForms(args).Initialize())
			using (var windows = new DisplayWindowCollection())
			{
				Dictionary<DisplayWindow, Color> windowColors = new Dictionary<DisplayWindow, Color>();
				double hue = 0;
				DisplayWindow primary = null;

				foreach (var screen in Display.Screens.AllScreens)
				{
					var window = new DisplayWindow(new CreateWindowParams
					{
						TargetScreen = screen,
						Resolution = new Resolution(500, 400)
					});

					// Some fancy code in here to make each window have a different color.
					windowColors[window] = Color.FromHsv(hue, 1, 1);
					hue += 60;

					if (screen.IsPrimary)
					{
						// If the primary window is closed, exit the example.
						primary = window;
						primary.Closed += (sender, e) => AgateApp.IsAlive = false;
					}

					windows.Add(window);
				}

				// Register a key press handler. This will terminate the application if the escape key is pressed.
				Input.Unhandled.KeyDown += (sender, e) =>
				{
					if (e.KeyCode == KeyCode.Escape)
						AgateApp.IsAlive = false;
				};

				var font = new Font(Font.AgateSerif)
				{
					Size = 14,
					Style = FontStyles.Bold,
					TextAlignment = OriginAlignment.Center
				};

				// Run the game loop
				while (AgateApp.IsAlive)
				{
					foreach (var window in windows.Where(window => window.IsClosed == false))
					{
						// We need to set the render target before drawing so that we can draw to
						// each monitor individually.
						Display.RenderTarget = window.FrameBuffer;
						Display.BeginFrame();
						Display.Clear(windowColors[window]);

						Point point = new Point(10, 10);
						Size size = new Size(15, 15);

						for (int i = 0; i < 36; i++)
						{
							Rectangle dest = new Rectangle(point, size);
							Display.Primitives.FillRect(Color.FromHsv(i * 10, 1, 1), dest);

							point.X += 10;
							point.Y += 10;
						}
						
						if (window == primary)
						{
							font.DrawText(300, 100, "Welcome to\nAgateLib!");
						}

						Display.EndFrame();
					}

					// A call to Core.KeepAlive is required to process input events and play nice with the OS.
					AgateApp.KeepAlive();
				}
			}
		}
	}
}
