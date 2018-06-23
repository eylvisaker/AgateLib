using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Tests.FontTests
{
	class FontLineTester : IAgateTest
	{
		List<IFont> fonts = new List<IFont>();
		int currentFont = 0;
		string text = "This text is a test\n" +
		              "of multiline text.  How\n" +
		              "did it work?\n\n" +
		              "You can type into this box with the keyboard.\n" +
		              "The rectangle is drawn by calling the\n" +
		              "StringDisplaySize function to get the size of the text.\n" +
		              "You can use the numeric keypad to change fonts.";

		public string Name => "Font Lines";
		public string Category => "Fonts";

		public void Run(string[] args)
		{
			using (new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				Input.Unhandled.KeyDown += Keyboard_KeyDown;
				AgateApp.AutoPause = true;
				
				fonts.Add(Font.AgateSans);
				fonts.Add(Font.AgateSerif);
				fonts.Add(Font.AgateMono);

				while (AgateApp.IsAlive)
				{
					Display.BeginFrame();
					Display.Clear(Color.Navy);

					Rectangle drawRect;

					FontTests(fonts[currentFont], out drawRect);

					Display.Primitives.DrawRect(Color.Red, drawRect);
					
					Display.EndFrame();
					AgateApp.KeepAlive();

					if (Input.Unhandled.Keys[KeyCode.Escape])
						return;
				}
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