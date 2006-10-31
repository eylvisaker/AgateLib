using System;
using System.Collections.Generic;

using ERY.AgateLib;
using ERY.AgateLib.Geometry;

namespace PixelBufferTest
{
    static class Program
    {
        static Surface image;
        static Point imageLocation = new Point(50, 50);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
                setup.Initialize(true, false, false);
                if (setup.Cancel)
                    return;

                DisplayWindow wind = new DisplayWindow("Pixel Buffer Test", 640, 480, false, true);
                
                image = new Surface(100, 100);

                Mouse.MouseDown += new InputEventHandler(Mouse_MouseDown);
                Mouse.MouseMove += new InputEventHandler(Mouse_MouseMove);

                while (wind.Closed == false)
                {
                    Display.BeginFrame();
                    Display.Clear();

                    Display.DrawRect(new Rectangle(imageLocation, image.SurfaceSize), Color.White);

                    image.Draw(imageLocation);
                    
                    Display.EndFrame();
                    Core.KeepAlive();
                }

            }
        }

        static void Mouse_MouseMove(InputEventArgs e)
        {
            if (Mouse.Buttons[Mouse.MouseButtons.Primary])
            {
                Point pt = new Point(e.MousePosition.X - imageLocation.X,
                    e.MousePosition.Y - imageLocation.Y);

                if (pt.X >= image.SurfaceWidth || pt.Y >= image.SurfaceHeight 
                    || pt.X < 0 || pt.Y < 0)
                    return;

                PixelBuffer buffer = image.ReadPixels();
                int pixelLocation = pt.Y * buffer.RowStride + pt.X * buffer.PixelStride;

                buffer.Data[pixelLocation + 1] = 0xff;
                buffer.Data[pixelLocation + 2] = 0xff;
                buffer.Data[pixelLocation + 3] = 0xff;

                image.WritePixels(buffer);

            }
        }

        static void Mouse_MouseDown(InputEventArgs e)
        {
            Mouse_MouseMove(e);
        }
    }
}