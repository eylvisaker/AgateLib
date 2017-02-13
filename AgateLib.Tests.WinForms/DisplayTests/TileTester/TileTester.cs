using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;
using AgateLib.DisplayLib;

namespace AgateLib.Tests.DisplayTests.TileTester
{
	class TileTester : IAgateTest
	{
		Surface tile;
		float xval, yval;

		public string Name => "Tile Tester";
		public string Category => "Display";

		public void Run(string[] args)
		{
			frmTileTester frm = new frmTileTester();
			frm.Show();

			tile = new Surface("bg-bricks.png");

			while (frm.IsDisposed == false)
			{
				Display.BeginFrame();
				Display.Clear(Color.FromRgb(255, 0, 255));

				DrawTiles();

				Display.EndFrame();
				AgateApp.KeepAlive();

				// move at 100 pixels per second
				if (frm.ScrollX)
				{
					xval += (float)AgateApp.GameClock.Elapsed.TotalMilliseconds / 10.0f;
				}
				if (frm.ScrollY)
				{
					yval += (float)AgateApp.GameClock.Elapsed.TotalMilliseconds / 10.0f;
				}

				frm.FPS = Display.FramesPerSecond;
			}
		}

		private void DrawTiles()
		{
			int cols = (int)Math.Ceiling(Display.RenderTarget.Width / (float)tile.DisplayWidth);
			int rows = (int)Math.Ceiling(Display.RenderTarget.Height / (float)tile.DisplayHeight);

			while (xval > tile.DisplayWidth)
				xval -= tile.DisplayWidth;

			while (yval > tile.DisplayHeight)
				yval -= tile.DisplayHeight;

			for (int j = -1; j < rows; j++)
			{
				for (int i = -1; i < cols; i++)
				{
					tile.Draw((int)(xval + i * tile.DisplayWidth),
							  (int)(yval + j * tile.DisplayHeight));
				}
			}
		}

	}
}