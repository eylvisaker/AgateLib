// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Platform.WindowsForms.ApplicationModels;
using AgateLib.InputLib.Legacy;

namespace Tests.FontTester
{
	class Program : IAgateTest
	{
		#region IAgateTest Members

		public string Name { get { return "Fonts"; } }
		public string Category { get { return "Fonts"; } }

		#endregion

		public void Main(string[] args)
		{
			new PassiveModel(args).Run( () =>
{
	DisplayWindow wind = DisplayWindow.CreateWindowed("Font Tester", 800, 600);
	DisplayWindow fullWind = null;

	FontSurface font = new FontSurface("Sans Serif", 12);
	FontSurface bitmapFont = FontSurface.BitmapMonospace("lotafont.png", new Size(16, 16));

	int frame = 0;


	while (wind.IsClosed == false)
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
		Size textSize = font.MeasureString(text);

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
		textSize = font.MeasureString(text);

		// draw a box with the same size the text should appear as
		Display.DrawRect(new Rectangle(textPoint, textSize), Color.White);

		font.DrawText(textPoint, text);
		font.SetScale(1.0, 1.0);

		// this draws a white background behind the text we want to Display.
		text = "F2: Toggle VSync   F5:  Toggle Windowed / Fullscreen      ";
		text += "FPS: " + Display.FramesPerSecond.ToString("0.00") + "    ";

		if (Core.IsActive)
			text += "Active";
		else
			text += "Not Active";

		// figure out how big the displayed text will be
		textSize = font.MeasureString(text);

		// draw the white background
		Display.FillRect(new Rectangle(new Point(0, 0), textSize), Color.White);

		// draw the text on top of the background
		font.Color = Color.Black;
		font.DrawText(text);  // supplying no position arguments defaults to (0, 0)

		// draw something which moves to let us know the program is running
		Display.FillRect(new Rectangle(
			10, 200, 70 + (int)(50 * Math.Cos(frame / 10.0)), 50), Color.Red);

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

		Display.FillRect(new Rectangle(-10, -10, 20, 20), Color.Green);

		// and we're done.
		Display.EndFrame();
		Core.KeepAlive();

		frame++;

		// toggle full screen if the user pressed F5;
		if (Keyboard.Keys[KeyCode.F5])
		{
			System.Diagnostics.Debug.Print("IsFullscreen: {0}", Display.CurrentWindow.IsFullScreen);

			if (Display.CurrentWindow.IsFullScreen == false)
			{
				fullWind = DisplayWindow.CreateFullScreen("Font Tester", 800, 600);
			}
			else
			{
				fullWind.Dispose();
				Display.RenderTarget = wind;
			}

			Keyboard.ReleaseAllKeys();
			System.Diagnostics.Debug.Print("IsFullscreen: {0}", Display.CurrentWindow.IsFullScreen);
		}
		else if (Keyboard.Keys[KeyCode.F2])
		{
			Display.RenderState.WaitForVerticalBlank = !Display.RenderState.WaitForVerticalBlank;
			Keyboard.ReleaseKey(KeyCode.F2);
		}
		else if (Keyboard.Keys[KeyCode.Escape])
		{
			Display.Dispose();
			return;
		}

	}

});

		}
	}
}