using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.Geometry;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Platform.WinForms.Resources;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.InputLib.Legacy;

namespace AgateLib.Tests.FontTests
{
	class Kerning : IAgateTest 
	{
		public string Name
		{
			get { return "Kerning"; }
		}

		public string Category
		{
			get { return "Fonts"; }
		}

		bool useKerning = true;

		public void Main(string[] args)
		{
			new PassiveModel(args).Run( () =>
			{
				Input.Unhandled.KeyDown += Keyboard_KeyDown;
				DisplayWindow wind = DisplayWindow.CreateWindowed("Kerning test", 800, 600);

				FontSurface font = ((Font)DefaultAssets.Fonts.AgateSans).GetFontSurface(14, FontStyles.None);
				FontSurface unkerned = ConstructUnkernedFont(font);

				string text = ConstructKerningText(wind, font);

				while (wind.IsClosed == false)
				{
					Display.BeginFrame();
					Display.Clear();

					FontSurface thisFont = useKerning ? font : unkerned;

					if (useKerning)
						thisFont.DrawText("Using kerning. (space to toggle)");
					else
						thisFont.DrawText("No kerning used. (space to toggle)");

					thisFont.Color = Color.White;
					thisFont.DrawText(0, thisFont.FontHeight, text);

					Display.EndFrame();
					Core.KeepAlive();
				}
			});
		}

		void Keyboard_KeyDown(object sender, AgateInputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Space)
				useKerning = !useKerning;
		}


		private FontSurface ConstructUnkernedFont(FontSurface font)
		{
			var bmp = font.Impl as AgateLib.DisplayLib.BitmapFont.BitmapFontImpl;

			FontMetrics metrics = bmp.FontMetrics.Clone();

			foreach (var glyph in metrics.Keys)
			{
				metrics[glyph].KerningPairs.Clear();
			}

			return FontSurface.FromImpl(new BitmapFontImpl(bmp.Surface, metrics, "Unkerned " + bmp.FontName));
		}

		private static string ConstructKerningText(DisplayWindow wind, FontSurface font)
		{

			var bmp = font.Impl as AgateLib.DisplayLib.BitmapFont.BitmapFontImpl;

			FontMetrics metrics = bmp.FontMetrics.Clone();

			StringBuilder text = new StringBuilder();

			int count = 0;
			int maxLine = wind.Width / font.FontHeight;

			foreach (char first in metrics.Keys)
			{
				foreach (var kern in metrics[first].KerningPairs)
				{
					text.Append(first);
					text.Append(kern.Key);
					text.Append(" ");

					count += 2;

					if (count > maxLine)
					{
						text.AppendLine();
						count = 0;
					}
				}
			}

			string displayText = text.ToString();
			return displayText;
		}

	}
}
