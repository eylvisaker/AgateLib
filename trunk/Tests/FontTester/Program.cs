// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using ERY.AgateLib;

namespace FontTester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            using (AgateSetup setupDisplay = new AgateSetup())
            {
                setupDisplay.AskUser = true;
                setupDisplay.Initialize(true, false, false);

                if (setupDisplay.Cancel)
                    return;

                DisplayWindow wind = new DisplayWindow("Font Tester", 640, 480, false, true);
                FontSurface font = new FontSurface("Arial", 12);
                FontSurface bitmapFont = FontSurface.BitmapMonospace("font.png", new Size(16, 16));

                int frame = 0;

                while (wind.Closed == false)
                {
                    Display.BeginFrame();
                    Display.Clear(Color.DarkGray);

                    // test the color changing
                    font.Color = Color.LightGreen;
                    font.DrawText(20, 150, "This is regular green text.");
                    font.Color = Color.White;

                    // test display alignment property
                    Point textPoint = new Point(100, 50);
                    string text = string.Format("This text is centered on {0},{1}.", textPoint.X, textPoint.Y);
                    Size textSize = font.StringDisplaySize(text);

                    // draw a box around where the text should be displayed.
                    Display.DrawRect(new Rectangle(textPoint.X - textSize.Width / 2, textPoint.Y - textSize.Height / 2,
                        textSize.Width, textSize.Height), Color.Gray);

                    font.DisplayAlignment = OriginAlignment.Center;
                    font.DrawText(textPoint, text);
                    font.DisplayAlignment = OriginAlignment.TopLeft;

                    // test text scaling
                    font.SetScale(2.0, 2.0);
                    text = "This text is twice as big.";
                    textPoint = new Point(50, 75);
                    textSize = font.StringDisplaySize(text);

                    // draw a box with the same size the text should appear as
                    Display.DrawRect(new Rectangle(textPoint, textSize), Color.White);

                    font.DrawText(textPoint, text);
                    font.SetScale(1.0, 1.0);



                    // this draws a white background behind the text we want to display.
                    text = "Press F5 to toggle Windowed / Fullscreen";

                    // figure out how big the displayed text will be
                    textSize = font.StringDisplaySize(text);

                    // draw the white background
                    Display.FillRect(new Rectangle(new Point(0, 0), textSize), Color.White);

                    // draw the text on top of the background
                    font.Color = Color.Black;
                    font.DrawText(text);  // supplying no position arguments defaults to (0, 0)

                    // draw something which moves to let us know the program is running
                    Display.FillRect(new Rectangle(
                        10, 200, 70 + (int)( 50 * Math.Cos(frame / 10.0)), 50), Color.Red);

                    // do some bitmap font stuff
                    bitmapFont.DrawText(10, 350, "THIS IS BITMAP FONT TEXT.");

                    bitmapFont.Color = Color.Red;
                    bitmapFont.DrawText(10, 366, "THIS IS RED TEXT.");
                    bitmapFont.Color = Color.White;

                    bitmapFont.SetScale(3, 2);
                    bitmapFont.DrawText(10, 382, "THIS IS BIGG.");
                    bitmapFont.SetScale(1, 1);

                    Display.FillRect(new Rectangle(95, 425, 10, 10), Color.Blue);
                    bitmapFont.DisplayAlignment = OriginAlignment.Center;
                    bitmapFont.DrawText(100, 430, "CHECK");
                    bitmapFont.DisplayAlignment = OriginAlignment.TopLeft;

                    // and we're done.
                    Display.EndFrame();
                    Core.KeepAlive();

                    frame++;

                    // toggle full screen if the user pressed F5;
                    if (Keyboard.Keys[KeyCode.F5])
                    {
                        Display.CurrentWindow.ToggleFullScreen();
                    }
                    else if (Keyboard.Keys[KeyCode.Escape])
                    {
                        Display.Dispose();
                        return;
                    }

                }

            }

        }
    }
}