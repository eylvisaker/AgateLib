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
using AgateLib.InputLib.Legacy;
using AgateLib.Configuration;

namespace AgateLib.Tests.FontTests
{
	class Kerning : IAgateTest
	{
		bool useKerning = true;

		public string Name => "Kerning";
		public string Category => "Fonts";

		public AgateConfig Configuration { get; set; }

		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
		}

		public void Run(string[] args)
		{
			Input.Unhandled.KeyDown += Keyboard_KeyDown;
			DisplayWindow wind = DisplayWindow.CreateWindowed("Kerning test", 800, 600);
			FontState state = new FontState
			{
				Size = 14,
				Style = FontStyles.None,
			};

			FontSurface font = Font.AgateSans.Impl.FontSurface(state);

			FontSurface unkerned = ConstructUnkernedFont(font);

			string text = ConstructKerningText(wind, font);

			while (wind.IsClosed == false)
			{
				Display.BeginFrame();
				Display.Clear();

				FontSurface thisFont = useKerning ? font : unkerned;

				if (useKerning)
					thisFont.DrawText(state,"Using kerning. (space to toggle)");
				else
					thisFont.DrawText(state, "No kerning used. (space to toggle)");

				state.Color = Color.White;
				thisFont.DrawText(state, 0, thisFont.FontHeight(state), text);

				Display.EndFrame();
				AgateApp.KeepAlive();
			}
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
			return "THIS IMPLEMENTATION NEEDS FIXING";

			//	var bmp = font.Impl as AgateLib.DisplayLib.BitmapFont.BitmapFontImpl;

			//	FontMetrics metrics = bmp.FontMetrics.Clone();

			//	StringBuilder text = new StringBuilder();

			//	int count = 0;
			//	int maxLine = wind.Width / font.FontHeight();

			//	foreach (char first in metrics.Keys)
			//	{
			//		foreach (var kern in metrics[first].KerningPairs)
			//		{
			//			text.Append(first);
			//			text.Append(kern.Key);
			//			text.Append(" ");

			//			count += 2;

			//			if (count > maxLine)
			//			{
			//				text.AppendLine();
			//				count = 0;
			//			}
			//		}
			//	}

			//	string displayText = text.ToString();
			//	return displayText;
		}
}
}
