using System;
using System.Collections.Generic;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Utility;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.InputLib.Legacy;

namespace AgateLib.Testing.FontTests
{
	class FontLineTester : IAgateTest
	{
		List<IFont> fonts = new List<IFont>();
		int currentFont = 0;
		string text = "This text is a test\nof multiline text.  How\ndid it work?\n\n" +
			"You can type into this box with the keyboard.\nThe rectangle is drawn by calling " +
			"the\nStringDisplaySize function to get the size of the text.";

		public string Name { get { return "Font Lines"; } }
		public string Category { get { return "Fonts"; } }

		public void Main(string[] args)
		{
			new PassiveModel(args).Run( () =>
			{
				DisplayWindow wind = DisplayWindow.CreateWindowed("Font Line Tester", 640, 480);
				Input.Unhandled.KeyDown += Keyboard_KeyDown;
				Core.AutoPause = true;

				// TODO: Fix this
				//FontSurface bmpFont = FontSurface.LoadBitmapFont("bitmapfont.png", "bitmapfont.xml");

				//fonts.Add(bmpFont);
				fonts.Add(AgateLib.DefaultAssets.Fonts.AgateSans);
				fonts.Add(AgateLib.DefaultAssets.Fonts.AgateSerif);
				fonts.Add(AgateLib.DefaultAssets.Fonts.AgateMono);

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

					if (Input.Unhandled.Keys[KeyCode.Escape])
						return;
				}
			});
		}

		private void FontTests(IFont font, out Rectangle drawRect)
		{
			Point drawPoint = new Point(10, 10);
			Size fontsize = font.MeasureString(text);
			drawRect = new Rectangle(drawPoint, fontsize);

			font.DrawText(drawPoint, text);
		}

		void Keyboard_KeyDown(object sender, AgateInputEventArgs e)
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