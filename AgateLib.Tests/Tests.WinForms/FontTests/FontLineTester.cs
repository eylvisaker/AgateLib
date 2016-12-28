using System;
using System.Collections.Generic;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Utility;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.InputLib.Legacy;
using AgateLib.Configuration;

namespace AgateLib.Tests.FontTests
{
	class FontLineTester : IAgateTest
	{
		List<IFont> fonts = new List<IFont>();
		int currentFont = 0;
		string text = "This text is a test\nof multiline text.  How\ndid it work?\n\n" +
			"You can type into this box with the keyboard.\nThe rectangle is drawn by calling " +
			"the\nStringDisplaySize function to get the size of the text.";

		public string Name => "Font Lines";
		public string Category => "Fonts";

		public AgateConfig Configuration { get; set; }

		public void ModifySetup(IAgateSetup setup)
		{
		}

		public void Run()
		{
			Input.Unhandled.KeyDown += Keyboard_KeyDown;
			Core.AutoPause = true;

			// TODO: Fix this
			//FontSurface bmpFont = FontSurface.LoadBitmapFont("bitmapfont.png", "bitmapfont.xml");

			//fonts.Add(bmpFont);
			fonts.Add(Font.AgateSans);
			fonts.Add(Font.AgateSerif);
			fonts.Add(Font.AgateMono);

			while (Display.CurrentWindow.IsClosed == false)
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