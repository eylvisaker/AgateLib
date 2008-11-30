using System;
using System.Collections.Generic;
using System.Diagnostics;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace AgateTest
{
    static class ScreenCapture
    {
        static bool capturing = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    using (AgateSetup setup = new AgateSetup())
        //    {
        //        setup.AskUser = true;
        //        setup.Initialize(true, false, false);
        //        if (setup.Cancel)
        //            return;

        //        Size size = new Size(400, 400);

        //        DisplayWindow displayWindow1 = new DisplayWindow("Test", size.Width, size.Height);
        //        Surface captureSurface = new Surface(size.Width, size.Height);
        //        Surface image = new Surface("test.png");

        //        Random rnd = new Random();

        //        Mouse.MouseDown += new InputEventHandler(Mouse_MouseDown);

        //        FontSurface font = new FontSurface("Times", 14);
        //        int fontHeight = font.StringDisplayHeight("M");
        //        string captureString = null;

        //        while (displayWindow1.IsClosed == false)
        //        {
        //            if (capturing)
        //                AgateDisplay.RenderTarget = captureSurface;
        //            else
        //                AgateDisplay.RenderTarget = displayWindow1;

        //            AgateDisplay.BeginFrame();

        //            AgateDisplay.Clear();
        //            for (int i = 0; i < 50; i++)
        //            {
        //                AgateDisplay.FillRect(new ERY.AgateLib.Geometry.Rectangle(
        //                    rnd.Next(0, 100), fontHeight * 2 + image.DisplayHeight + rnd.Next(0, 100),
        //                    rnd.Next(100, 200), rnd.Next(100, 200)),
        //                    Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255)));

        //            }

        //            image.Draw(0, fontHeight * 2);

        //            font.DrawText("Click to capture image.");
        //            font.DrawText(0, fontHeight, captureString);

        //            AgateDisplay.EndFrame();

        //            if (capturing)
        //            {
        //                captureSurface.SaveTo("example.png", ImageFileFormat.Png);
        //                captureString = "Saved image to example.png";

        //                capturing = false;
        //            }

        //            Core.KeepAlive();
        //        }
        //    }
        //}

        static void Mouse_MouseDown(InputEventArgs e)
        {
            capturing = !capturing;
            Console.WriteLine("Capturing : {0}", capturing);
        }
    }
}

namespace ScreenCaptureExample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
		{
			// These two lines are used by AgateLib tests to locate
			// driver plugins and images.
			AgateLib.Utility.FileManager.AssemblyPath.Add("../Libraries");
			AgateLib.Utility.FileManager.ImagePath.Add("../../../Tests/TestImages");

            using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
                setup.Initialize(true, false, false);
                if (setup.WasCanceled) return;

                //Form1 someForm = new Form1();
                //someForm.Size = new System.Drawing.Size(800, 600);
                //someForm.Show();

                DisplayWindow displayWindow1 = new DisplayWindow("Hello", 800, 600);
                Surface someSurface = new Surface("wallpaper.png");
                Surface captureSurface = new Surface(1600, 1200);
                bool capturing = false;

                while (displayWindow1.IsClosed == false)
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
                        Display.RenderTarget = displayWindow1;
                        someSurface.SetScale(1, 1);
                        capturing = false;

                        Debug.Print("Captured image to CapturedImage.png");
                    }

                    // KeepAlive processes events.
                    Core.KeepAlive();
                    System.Threading.Thread.Sleep(10);
                }
            }
        }
    }
}
