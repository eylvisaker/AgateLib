using System;
using System.Collections.Generic;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Utility;
using AgateLib.Platform.WindowsForms.ApplicationModels;

namespace Tests.FontLineTester
{
	class FontLineTester : IAgateTest
	{
		List<FontSurface> fonts = new List<FontSurface>();
		int currentFont = 0;
		string text = "This text is a test\nof multiline text.  How\ndid it work?\n\n" +
			"You can type into this box with the keyboard.\nThe rectangle is drawn by calling " +
			"the\nStringDisplaySize function to get the size of the text.";

		#region IAgateTest Members

		public string Name { get { return "Font Lines"; } }
		public string Category { get { return "Fonts"; } }

		#endregion

		public void Main(string[] args)
		{
			PassiveModel.Run(args, () =>
			{
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
			});
		}

		private void FontTests(FontSurface fontSurface, out Rectangle drawRect)
		{
			Point drawPoint = new Point(10, 10);
			Size fontsize = fontSurface.MeasureString(text);
			drawRect = new Rectangle(drawPoint, fontsize);

			fontSurface.DrawText(drawPoint, text);
		}

		void Keyboard_KeyDown(InputEventArgs e)
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