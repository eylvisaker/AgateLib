using System;
using System.Collections.Generic;

using ERY.AgateLib;
using ERY.AgateLib.Geometry;

namespace PixelBufferTest
{
    static class PixelBufferTest
    {
        static Surface image;
        static Point imageLocation = new Point(50, 50);
        static PixelBuffer buffer;
        static PixelBufferForm frm;
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

                frm = new PixelBufferForm();
                frm.Show();

                DisplayWindow wind = new DisplayWindow(frm.panel1);

                image = new Surface("image.png");
                buffer = image.ReadPixels(PixelFormat.RGBA8888);

                Mouse.MouseDown += new InputEventHandler(Mouse_MouseDown);
                Mouse.MouseMove += new InputEventHandler(Mouse_MouseMove);

                while (wind.Closed == false)
                {
                    Display.BeginFrame();
                    Display.Clear();

                    image.Draw(imageLocation);
                    
                    Display.EndFrame();
                    Core.KeepAlive();
                }

            }
        }

        static void Mouse_MouseMove(InputEventArgs e)
        {
            Color clr;
            Point pt = new Point(e.MousePosition.X - imageLocation.X,
                                 e.MousePosition.Y - imageLocation.Y);

            if (buffer.IsPointValid(pt) == false)
            {
                frm.lblPixelColor.Text = "No Pixel";
                return;
            }

            if (Mouse.Buttons[Mouse.MouseButtons.Primary])
            {
                buffer.SetPixel(pt.X, pt.Y, (Color)(frm.btnColor.BackColor));
                image.WritePixels(buffer);
            }

            clr = buffer.GetPixel(e.MousePosition.X - imageLocation.X,
            e.MousePosition.Y - imageLocation.Y);

            frm.lblPixelColor.Text =
                string.Format("R: {0}  G: {1}\r\nB: {2}  A: {3}",
                FormatComponent(clr.R), FormatComponent(clr.G),
                FormatComponent(clr.B), FormatComponent(clr.A));

        }

        private static string FormatComponent(byte p)
        {
            return (p / 255.0).ToString("0.00");
        }

        static void Mouse_MouseDown(InputEventArgs e)
        {
            Mouse_MouseMove(e);
        }
    }
}