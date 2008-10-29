using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib.Core;
using AgateLib.Display;
using AgateLib.Geometry;

namespace TileTester
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            new Program().Run();
        }

        Surface tile;
        float xval, yval;

        void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
                setup.Initialize(true, false, false);
                if (setup.Cancel)
                    return;

                Form1 frm = new Form1();
                frm.Show();

                tile = new Surface("bg-bricks.png");

                while (frm.IsDisposed == false)
                {
                    AgateDisplay.BeginFrame();
                    AgateDisplay.Clear(Color.FromArgb(255, 0, 255));

                    DrawTiles();

                    AgateDisplay.EndFrame();
                    AgateCore.KeepAlive();

                    if (frm.ScrollX)
                    {
                        xval += (float)AgateDisplay.DeltaTime / 20.0f;
                    }
                    if (frm.ScrollY)
                    {
                        // move at 50 pixels per second
                        yval += (float)AgateDisplay.DeltaTime / 20.0f;
                    }
                }
            }
        }

        private void DrawTiles()
        {
            int cols = (int)Math.Ceiling(AgateDisplay.RenderTarget.Width / (float)tile.DisplayWidth);
            int rows = (int)Math.Ceiling(AgateDisplay.RenderTarget.Height / (float)tile.DisplayHeight);

            while (xval > tile.DisplayWidth)
                xval -= tile.DisplayWidth;
            
            while (yval > tile.DisplayHeight)
                yval -= tile.DisplayHeight;

            for (int j = -1; j < rows; j++)
            {
                for (int i = -1; i < cols; i++)
                {
                    tile.Draw(xval + i * tile.DisplayWidth, 
                              yval + j * tile.DisplayHeight);
                }
            }
        }
    }
}