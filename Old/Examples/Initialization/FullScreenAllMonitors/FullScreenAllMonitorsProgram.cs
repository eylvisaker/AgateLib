using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform.WinForms;

namespace Examples.Initialization.FullScreenAllMonitors
{
	static class FullScreenAllMonitorsProgram
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			using (new AgateWinForms(args).Initialize())
			using (DisplayWindowCollection windows = new DisplayWindowBuilder()
				.Title("Full Screen All Monitors")
				.BackbufferSize(500, 400)
				.QuitOnClose()
				.BuildSeparateWindowsForAllScreens())
			{
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
					foreach (var window in windows)
					{
						// We need to set the render target before drawing so that we can draw to
						// each monitor individually.
						Display.RenderTarget = window.FrameBuffer;
						Display.BeginFrame();
						Display.Clear(Color.Gray);

						Point point = new Point(10, 10);
						Size size = new Size(15, 15);

						for (int i = 0; i < 36; i++)
						{
							Rectangle dest = new Rectangle(point, size);
							Display.Primitives.FillRect(Color.FromHsv(i * 10, 1, 1), dest);

							point.X += 10;
							point.Y += 10;
						}

						if (window.Screen.IsPrimary)
						{
							font.DrawText(350, 75, "Welcome to\nAgateLib!");
						}
						else
						{
							font.DrawText(350, 75, window.Screen.Bounds.ToString());
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
