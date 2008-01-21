using System;
using System.Collections.Generic;
using ERY.AgateLib;
using ERY.AgateLib.Geometry;

namespace AgateTest
{
    static class ScreenCapture
    {
        static bool capturing = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
                setup.InitializeAll();
                if (setup.Cancel)
                    return;

                Size size = new Size(400, 400);

                DisplayWindow displayWindow1 = new DisplayWindow("Test", size.Width, size.Height);
                Surface captureSurface = new Surface(size.Width, size.Height);
                Surface image = new Surface("test.png");

                Random rnd = new Random();

                Mouse.MouseDown += new InputEventHandler(Mouse_MouseDown);

                FontSurface font = new FontSurface("Times", 14);
                int fontHeight = font.StringDisplayHeight("M");
                string captureString = null;

                while (displayWindow1.IsClosed == false)
                {
                    if (capturing)
                        Display.RenderTarget = captureSurface;
                    else
                        Display.RenderTarget = displayWindow1;

                    Display.BeginFrame();

                    Display.Clear();
                    for (int i = 0; i < 50; i++)
                    {
                        Display.FillRect(new ERY.AgateLib.Geometry.Rectangle(
                            rnd.Next(0, 100), fontHeight * 2 + image.DisplayHeight + rnd.Next(0, 100),
                            rnd.Next(100, 200), rnd.Next(100, 200)),
                            Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255)));

                    }

                    image.Draw(0, fontHeight * 2);

                    font.DrawText("Click to capture image.");
                    font.DrawText(0, fontHeight, captureString);

                    Display.EndFrame();

                    if (capturing)
                    {
                        captureSurface.SaveTo("example.png", ImageFileFormat.Png);
                        captureString = "Saved image to example.png";

                        capturing = false;
                    }

                    Core.KeepAlive();
                }
            }
        }

        static void Mouse_MouseDown(InputEventArgs e)
        {
            capturing = !capturing;
            Console.WriteLine("Capturing : {0}", capturing);
        }


    }
}