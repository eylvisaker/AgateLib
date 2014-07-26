using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.BitmapFont;
using AgateLib.Geometry;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Platform.WindowsForms.Resources;
using AgateLib.Platform.WindowsForms.ApplicationModels;

namespace Tests.Fonts
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
			PassiveModel.Run(args, () =>
			{
				Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);
				DisplayWindow wind = DisplayWindow.CreateWindowed("Kerning test", 800, 600);

				FontSurface font = BuiltinResources.AgateSans14;
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

		void Keyboard_KeyDown(InputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Space)
				useKerning = !useKerning;
		}


		private FontSurface ConstructUnkernedFont(FontSurface font)
		{
			var bmp = font.Impl as AgateLib.BitmapFont.BitmapFontImpl;

			FontMetrics metrics = bmp.FontMetrics.Clone();

			foreach (var glyph in metrics.Keys)
			{
				metrics[glyph].KerningPairs.Clear();
			}

			return FontSurface.FromImpl(new BitmapFontImpl(bmp.Surface, metrics, "Unkerned " + bmp.FontName));
		}

		private static string ConstructKerningText(DisplayWindow wind, FontSurface font)
		{

			var bmp = font.Impl as AgateLib.BitmapFont.BitmapFontImpl;

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
