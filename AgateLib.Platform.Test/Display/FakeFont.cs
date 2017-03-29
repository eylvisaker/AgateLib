//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Platform.Test.Display
{
	public class FakeFont : IFont
	{
		public FakeFont(string name)
		{
			Name = name;
		}

		public string Name { get; set; }

		public Color Color { get; set; } = Color.Black;

		public double Alpha
		{
			get { return Color.A; }
			set { Color = Color.FromArgb((int)(255 * value), Color); }
		}

		public OriginAlignment DisplayAlignment { get; set; }

		public int FontHeight { get; set; } = 10;

		public FontStyles Style { get; set; }

		public int Size { get; set; }

		public TextImageLayout TextImageLayout { get; set; }

		public void Dispose()
		{
		}

		public void DrawText(string text)
		{
			LogDrawText(Point.Zero, text);
		}

		public void DrawText(Vector2 dest, string text)
		{
			LogDrawText(dest, text);
		}

		public void DrawText(Vector2 dest, string text, params object[] args)
		{
			LogDrawText(dest, string.Format(text, args));
		}

		public Size MeasureString(string text)
		{
			if (text.Length == 0)
				return Mathematics.Geometry.Size.Empty;

			int lineLength = 0;
			int longestLine = 0;
			int lineCount = 1;

			for(int i = 0; i < text.Length; i++)
			{
				lineLength++;
				if (text[i] == '\n')
				{
					lineLength = 0;
					lineCount++;
				}

				longestLine = Math.Max(lineLength, longestLine);
			}

			return new Size(FontHeight / 2 * longestLine, FontHeight * lineCount);
		}

		private void LogDrawText(Vector2 dest, string text, params object[] Parameters)
		{
			Log.WriteLine($"{Name} draw to ({dest}): {Color.ToArgbString()}");
			Log.WriteLine(text);
			LogParameters(Parameters);
		}

		private void LogParameters(object[] parameters)
		{
			foreach(var p in parameters)
			{
				Log.WriteLine("    " + p);
			}
		}
	}
}
