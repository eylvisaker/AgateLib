// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib;
using AgateLib.DisplayLib;

namespace AgateLib.Tests.DisplayTests
{
	class Tiling : IAgateTest
	{
		public string Name => "Tiling";

		public string Category => "Display";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public void Run(string[] args)
		{
			using (var wnd = new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Title(Name)
				.Build())
			{
				double time = 0;

				Surface[] tiles = new Surface[3];

				tiles[0] = new Surface("Images/tile1.png");
				tiles[1] = tiles[0];
				tiles[2] = new Surface("Images/tile2.png");

				while (AgateApp.IsAlive && tiles.Any(x => x.IsLoaded == false))
				{
					Display.BeginFrame();
					Display.Clear(Color.Blue);
					Display.EndFrame();
					AgateApp.KeepAlive();
				}

				while (AgateApp.IsAlive)
				{
					Display.BeginFrame();
					Display.Clear(Color.FromRgb(
						(int) (128 * Math.Abs(Math.Cos(time / 3.5))),
						(int) (128 * Math.Abs(Math.Sin(time / 4.5))),
						(int) (128 * Math.Abs(Math.Sin(time / 5.0)))));

					int x = 0, y = 0;

					foreach (var tile in tiles)
						tile.SetScale(1, 1);

					y = 0;

					for (int j = 0; j < 4; j++)
					{
						x = 0;
						for (int i = 0; i < wnd.Width / tiles[0].DisplayWidth; i++)
						{
							int index = (i + j) % tiles.Length;

							tiles[index].Draw(x, y);

							x += tiles[0].DisplayWidth;
						}

						y += tiles[0].DisplayHeight;
					}
					y += tiles[0].DisplayHeight;

					double scale = 1.32;

					foreach (var tile in tiles)
						tile.SetScale(scale, scale);

					for (int j = 0; j < 4; j++)
					{
						x = 0;
						for (int i = 0; i < wnd.Width / tiles[0].DisplayWidth; i++)
						{
							int index = (i + j) % tiles.Length;

							tiles[index].Draw(x, y);

							x += tiles[0].DisplayWidth;
						}

						y += tiles[0].DisplayHeight;
					}

					Display.EndFrame();
					AgateApp.KeepAlive();

					time += AgateApp.GameClock.Elapsed.TotalSeconds;
				}
			}
		}

	}
}