using System;
using System.Collections.Generic;
using ERY.AgateLib;

namespace FontLineTester
{
    static class FontLineTester
    {
        static List<FontSurface> fonts = new List<FontSurface>();
        static int currentFont = 0;
        static string text = "This text is a test\nof multiline text.  How\ndid it work?";


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

                DisplayWindow wind = new DisplayWindow("Font Line Tester", 640, 480);
                Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);
                Core.AutoPause = true;

                FileManager.ImagePath.Clear();
                FileManager.ImagePath.Add("../../");

                fonts.Add(new FontSurface("Arial", 12));
                fonts.Add(FontSurface.BitmapMonospace("font.png", new Size(16, 16)));
                fonts[1].StringTransformer = StringTransformer.ToUpper;

                while (wind.Closed == false)
                {
                    Display.BeginFrame();
                    Display.Clear(Color.Navy);

                    Rectangle drawRect;

                    FontTests(fonts[currentFont], out drawRect);

                    Display.DrawRect(drawRect, Color.Red);

                    fonts[0].DrawText(0, drawRect.Bottom + 20, 
                        "Measured size was: " + drawRect.Size.ToString());

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
            if (e.KeyID >= KeyCode.NumPad0 && e.KeyID <= KeyCode.NumPad9)
            {
                int key = e.KeyID - KeyCode.NumPad0 - 1;

                if (key < 0) key = 10;

                if (key < fonts.Count)
                    currentFont = key;
            }
            else if (e.KeyID == KeyCode.BackSpace && text.Length > 0)
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