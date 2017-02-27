// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Tests.FontTests
{
	class Fonts : IAgateTest
	{
		public string Name => "Font Tester";
		public string Category => "Fonts";

		public void Run(string[] args)
		{
			using (var wind = new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				DisplayWindow fullWind = null;

				FontSurface bitmapFontSurface = FontSurface.BitmapMonospace("lotafont.png", new Size(16, 16));
				Font bitmapFont = new FontBuilder("lotafont").AddFontSurface(
					new FontSettings(16, FontStyles.None), bitmapFontSurface).Build();

				int frame = 0;

				while (AgateApp.IsAlive)
				{
					Display.BeginFrame();
					Display.Clear(Color.DarkGray);

					IFont font = Font.AgateSans;
					font.Size = 12;

					// test the color changing
					font.Color = Color.LightGreen;
					font.DrawText(20, 150, "This is regular green text.");
					font.Color = Color.White;

					// test display alignment property
					Point textPoint = new Point(100, 50);
					string text = string.Format("This text is centered on {0},{1}.", textPoint.X, textPoint.Y);
					Size textSize = font.MeasureString(text);

					// draw a box around where the text should be displayed.
					Display.Primitives.DrawRect(Color.Gray,
						new Rectangle(textPoint.X - textSize.Width / 2, textPoint.Y - textSize.Height / 2,
						textSize.Width, textSize.Height));

					font.DisplayAlignment = OriginAlignment.Center;
					font.DrawText(textPoint, text);
					font.DisplayAlignment = OriginAlignment.TopLeft;

					// test text scaling
					font.Size = 24;
					text = "This text is twice as big.";
					textPoint = new Point(50, 75);
					textSize = font.MeasureString(text);

					// draw a box with the same size the text should appear as
					Display.Primitives.DrawRect(Color.White, new Rectangle(textPoint, textSize));

					font.DrawText(textPoint, text);
					font.Size = 12;

					// this draws a white background behind the text we want to Display.
					text = "F2: Toggle VSync   F5:  Toggle Windowed / Fullscreen      ";
					text += "FPS: " + Display.FramesPerSecond.ToString("0.00") + "    ";

					if (AgateApp.IsActive)
						text += "Active";
					else
						text += "Not Active";

					// figure out how big the displayed text will be
					textSize = font.MeasureString(text);

					// draw the white background
					Display.Primitives.FillRect(Color.White, new Rectangle(new Point(0, 0), textSize));

					// draw the text on top of the background
					font.Color = Color.Black;
					font.DrawText(text); // supplying no position arguments defaults to (0, 0)

					// draw something which moves to let us know the program is running
					Display.Primitives.FillRect(Color.Red, new Rectangle(
						10, 200, 70 + (int)(50 * Math.Cos(frame / 10.0)), 50));

					// do some bitmap font stuff
					bitmapFont.DrawText(10, 350, "THIS IS BITMAP FONT TEXT.");

					bitmapFont.Color = Color.Red;
					bitmapFont.DrawText(10, 366, "THIS IS RED TEXT.");
					bitmapFont.Color = Color.White;

					bitmapFont.Size = 32;
					bitmapFont.DrawText(10, 382, "THIS IS BIGG.");

					Display.Primitives.FillRect(Color.Blue, new Rectangle(95, 425, 10, 10));
					bitmapFont.TextAlignment = OriginAlignment.Center;
					bitmapFont.DrawText(100, 430, "CHECK");
					bitmapFont.TextAlignment = OriginAlignment.TopLeft;

					Display.Primitives.FillRect(Color.Green, new Rectangle(-10, -10, 20, 20));

					// and we're done.
					Display.EndFrame();
					AgateApp.KeepAlive();

					frame++;

					// toggle full screen if the user pressed F5;
					if (Input.Unhandled.Keys[KeyCode.F5])
					{
						System.Diagnostics.Debug.Print("IsFullscreen: {0}", Display.CurrentWindow.IsFullScreen);

						if (Display.CurrentWindow.IsFullScreen == false)
						{
							fullWind = DisplayWindow.CreateFullScreen("Font Tester", 800, 600);
						}
						else
						{
							fullWind.Dispose();
							Display.RenderTarget = wind.FrameBuffer;
						}

						Input.Unhandled.Keys.ReleaseAll();
						System.Diagnostics.Debug.Print("IsFullscreen: {0}", Display.CurrentWindow.IsFullScreen);
					}
					else if (Input.Unhandled.Keys[KeyCode.F2])
					{
						Display.RenderState.WaitForVerticalBlank = !Display.RenderState.WaitForVerticalBlank;
						Input.Unhandled.Keys.Release(KeyCode.F2);
					}
					else if (Input.Unhandled.Keys[KeyCode.Escape])
					{
						Display.Dispose();
						return;
					}
				}
			}
		}
	}
}