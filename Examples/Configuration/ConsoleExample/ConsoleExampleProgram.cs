using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgateLib;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform.WinForms;

namespace Examples.Configuration.ConsoleExample
{
	static class ConsoleExampleProgram
	{
		[STAThread]
		static void Main(string[] args)
		{ 
			// .InstallConsoleCommands() and .DefaultConsoleTheme() must come AFTER .Initialize() here.
			using (new AgateWinForms(args)
					.Initialize()
					.InstallConsoleCommands()
					.DefaultConsoleTheme(ConsoleThemes.Paper))
			using (new DisplayWindowBuilder(args)
					.Title("Console Command Example")
					.BackbufferSize(1280, 720)
					.AutoResizeBackBuffer()
					.QuitOnClose()
					.Build())
			{
				Input.Unhandled.KeyDown += (sender, e) =>
				{
					if (e.KeyCode == KeyCode.Escape)
						AgateApp.IsAlive = false;
				};

				var font = new Font(Font.AgateSerif)
				{
					Size = 14,
					Style = FontStyles.Bold,
				};

				List<Point> points = new List<Point>();

				AgateConsole.CommandLibraries.Add(
					new LibraryVocabulary(new ExampleVocabulary(points)));

				while (AgateApp.IsAlive)
				{
					Display.BeginFrame();
					Display.Clear(Color.Gray);

					Size size = new Size(10, 10);

					for (int i = 0; i < points.Count; i++)
					{
						var point = points[i];
						Color clr = Color.FromHsv(i / 10.0, 1, 1);

						Rectangle dest = new Rectangle(point, size);

						Display.Primitives.FillRect(clr, dest);
					}

					var target = new Point(
						0,
						Display.RenderTarget.Height - font.FontHeight);

					Display.Primitives.FillRect(Color.FromArgb(128, 128, 128, 128),
						new Rectangle(target, Display.RenderTarget.Size));

					font.DrawText(target,
						"Press ~ to open the console. Type help to see a list of commands.");

					Display.EndFrame();
					AgateApp.KeepAlive();
				}
			}
		}
	}
}
