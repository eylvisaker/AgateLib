// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.ApplicationModels;

namespace AgateLib.Testing.DisplayTests
{
	class Tiling : ISerialModelTest
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public void EntryPoint()
		{
			double time = 0;

			Surface[] tiles = new Surface[3];

			tiles[0] = new Surface("tile1.png");
			tiles[1] = tiles[0];
			tiles[2] = new Surface("tile2.png");

			var wnd = AgateAppModel.Instance.AutoCreatedWindow;

			while(tiles.Any(x => x.IsLoaded == false))
			{
				Display.BeginFrame();
				Display.Clear(Color.Blue);
				Display.EndFrame();
				Core.KeepAlive();
			}

			while (wnd.IsClosed == false)
			{
				Display.BeginFrame();
				Display.Clear(Color.FromArgb(
					(int)(128 * Math.Abs(Math.Cos(time / 3.5))),
					(int)(128 * Math.Abs(Math.Sin(time / 4.5))),
					(int)(128 * Math.Abs(Math.Sin(time / 5.0)))));

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
				Core.KeepAlive();

				time += Display.DeltaTime / 1000.0;
			}
		}

		public string Name
		{
			get { return "Tiling"; }
		}

		public string Category
		{
			get { return "Display"; }
		}


		public void ModifyModelParameters(SerialModelParameters parameters)
		{
		}
	}
}