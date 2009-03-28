using System;
using System.Collections.Generic;
using System.Diagnostics;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace Tests.ScreenCaptureExample
{
    static class ScreenCaptureTest
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [AgateTest("Screen Capture Test", "Display")]
        static void Main()
		{
            using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
                setup.Initialize(true, false, false);
                if (setup.WasCanceled) return;

                DisplayWindow wind = DisplayWindow.CreateWindowed("Hello", 800, 600);
                Surface someSurface = new Surface("wallpaper.png");
                Surface captureSurface = new Surface(1600, 1200);
                bool capturing = false;

                while (wind.IsClosed == false)
                {
                    if (Keyboard.Keys[KeyCode.C])
                    {
                        capturing = true;
                        Keyboard.ReleaseKey(KeyCode.C);
                    }
                    if (capturing)
                    {
                        Display.RenderTarget = captureSurface;
                        someSurface.SetScale(2, 2);
                    }

                    Display.BeginFrame();

                    Display.Clear(Color.White);

                    someSurface.Draw();

                    Display.EndFrame();

                    if (capturing)
                    {
                        captureSurface.SaveTo("CapturedImage.png", ImageFileFormat.Png);
                        Display.RenderTarget = wind;
                        someSurface.SetScale(1, 1);
                        capturing = false;

                        Debug.Print("Captured image to CapturedImage.png");
                    }

                    Core.KeepAlive();
                    System.Threading.Thread.Sleep(10);
                }
            }
        }
    }
}
