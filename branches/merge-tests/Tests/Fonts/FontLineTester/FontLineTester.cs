using System;
using System.Collections.Generic;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Utility;

namespace Tests.FontLineTester
{
    static class FontLineTester
    {
        static List<FontSurface> fonts = new List<FontSurface>();
        static int currentFont = 0;
        static string text = "This text is a test\nof multiline text.  How\ndid it work?\n\n"+
            "You can type into this box with the keyboard.\nThe rectangle is drawn by calling "+
            "the\nStringDisplaySize function to get the size of the text.";


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [AgateTest("Font Line Tester", "Fonts")]
        static void Main()
		{
            using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
                setup.Initialize(true, false, false);
                if (setup.WasCanceled)
                    return;

                DisplayWindow wind = DisplayWindow.CreateWindowed("Font Line Tester", 640, 480);
                Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);
                Core.AutoPause = true;

                // TODO: Fix this
                //FontSurface bmpFont = FontSurface.LoadBitmapFont("bitmapfont.png", "bitmapfont.xml");

                //fonts.Add(bmpFont);
                fonts.Add(new FontSurface("Arial", 12));
                fonts.Add(new FontSurface("Arial", 20));
                fonts.Add(new FontSurface("Times", 12));
                fonts.Add(new FontSurface("Times", 20));
                fonts.Add(new FontSurface("Tahoma", 14));
                fonts.Add(new FontSurface("Comic", 16));

                while (wind.IsClosed == false)
                {
                    Display.BeginFrame();
                    Display.Clear(Color.Navy);

                    Rectangle drawRect;

                    FontTests(fonts[currentFont], out drawRect);

                    Display.DrawRect(drawRect, Color.Red);

                    //bmpFont.DrawText(0, 370, "Use numeric keypad to switch fonts.");
                    //bmpFont.DrawText(0, 400,
                    //    "Measured size was: " + drawRect.Size.ToString());

                    Display.EndFrame();
                    Core.KeepAlive();

                    if (Keyboard.Keys[KeyCode.Escape])
                        return;
                }
            }
        }

        private static void FontTests(FontSurface fontSurface, out Rectangle drawRect)
        {
            Point drawPoint = new Point(10, 10);
            Size fontsize = fontSurface.StringDisplaySize(text);
            drawRect = new Rectangle(drawPoint, fontsize);

            fontSurface.DrawText(drawPoint, text);
        }

        static void Keyboard_KeyDown(InputEventArgs e)
        {
            if (e.KeyCode >= KeyCode.NumPad0 && e.KeyCode <= KeyCode.NumPad9)
            {
                int key = e.KeyCode - KeyCode.NumPad0 - 1;

                if (key < 0) key = 10;

                if (key < fonts.Count)
                    currentFont = key;
            }
            else if (e.KeyCode == KeyCode.BackSpace && text.Length > 0)
            {
                text = text.Substring(0, text.Length - 1);
            }
            else if (!string.IsNullOrEmpty(e.KeyString))
            {
                text += e.KeyString;
            }
        }
    }
}