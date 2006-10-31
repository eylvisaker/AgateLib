// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;

using ERY.AgateLib;
using ERY.AgateLib.Geometry;

namespace TilingTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (AgateSetup setup = new AgateSetup("Tiling Test", args))
            {
                setup.Initialize(true, false, false);

                if (setup.Cancel)
                    return;

                DisplayWindow wnd = new DisplayWindow("Tiling Test", 600, 600, false, true);

                int frame = 0;

                Surface[] tiles = new Surface[2];

                tiles[0] = new Surface("tile1.png");
                tiles[1] = new Surface("tile2.png");

                while (wnd.Closed == false)
                {
                    Display.BeginFrame();
                    Display.Clear(Color.FromArgb(
                        (int)(128 * Math.Abs(Math.Cos(frame / 70.0))),
                        (int)(128 * Math.Abs(Math.Sin(frame / 90.0))),
                        (int)(128 * Math.Abs(Math.Sin(frame / 95.0)))));

                    int x = 0, y = 0;

                    tiles[0].SetScale(1, 1);
                    tiles[1].SetScale(1, 1);

                    for (int i = 0; i < wnd.Width / tiles[0].DisplayWidth; i++)
                    {
                        y = 0;

                        for (int j = 0; j < 4; j++)
                        {
                            int index = (i + j) % 2;

                            tiles[index].Draw(x, y);

                            y += tiles[0].DisplayHeight;
                        }

                        x += tiles[0].DisplayWidth;
                    }

                    double scale = 1.32;

                    tiles[0].SetScale(scale, scale);
                    tiles[1].SetScale(scale, scale);

                    x = 0;

                    for (int i = 0; i < wnd.Width / tiles[0].DisplayWidth; i++)
                    {
                        y = 200;

                        for (int j = 0; j < 4; j++)
                        {
                            int index = (i + j) % 2;

                            tiles[index].Draw(x, y);

                            y += tiles[0].DisplayHeight;
                        }

                        x += tiles[0].DisplayWidth;
                    }

                    Display.EndFrame();
                    Core.KeepAlive();

                    frame++;
                }

            }
        }
    }
}