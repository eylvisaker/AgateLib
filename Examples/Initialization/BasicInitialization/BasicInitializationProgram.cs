using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Platform.WinForms;

namespace Examples.Initialization.BasicInitialization
{
	static class BasicInitializationProgram
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			using (new AgateWinForms(args).Initialize())
			{
				// Use the DisplayWindowBuilder fluent interface
				// to construct a DisplayWindow to render to.
				// This specifies the title, the size of the back buffer, and
				// that we want the application to quit when the window is
				// closed.
				DisplayWindow window = new DisplayWindowBuilder(args)
					.Title("Basic AgateLib Initialization")
					.BackbufferSize(500, 400) 
					.QuitOnClose()
					.Build();

				// Register a key press handler. This will terminate the application if the escape key is pressed.
				Input.Unhandled.KeyDown += (sender, e) =>
				{
					if (e.KeyCode == KeyCode.Escape)
						AgateApp.IsAlive = false;
				};

				// Run the game loop. Always check AgateApp.IsAlive before continuing the game loop.
				// AgateApp.IsAlive will be set false if the user closes the window or if the system
				// is shutting down.
				while (AgateApp.IsAlive)
				{
					// Set the window as the current render target.
					Display.RenderTarget = window.FrameBuffer;

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

					var font = new Font(Font.AgateSerif)
					{
						Size = 14,
						Style = FontStyles.Bold,
						DisplayAlignment = OriginAlignment.Center
					};
					
					font.DrawText(350, 75, "Welcome to\nAgateLib!");

					Display.EndFrame();

					// A call to AgateApp.KeepAlive is required to process input events and play nice with the OS.
					AgateApp.KeepAlive();
				}
			}
		}
	}
}
