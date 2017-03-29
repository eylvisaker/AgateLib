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
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Object which contains layout information for text.
	/// </summary>
	public class ContentLayout
	{
		class TextContent
		{
			public Point Destination;
			public string Text;
			public IFont Font;
			public int Size;
			public FontStyles Style;
			public Color Color;

			public void Draw(Point origin)
			{
				Point target = new Point(Destination.X + origin.X, Destination.Y + origin.Y);

				Font.Color = Color;
				Font.Size = Size;
				Font.Style = Style;

				Font.DrawText(target, Text);
			}
		}

		private IFont font;
		private List<TextContent> layouts = new List<TextContent>();
		private Point insertionPoint;

		/// <summary>
		/// Constructs a ContentLayout object.
		/// </summary>
		/// <param name="font"></param>
		/// <param name="maxWidth"></param>
		public ContentLayout(IFont font, int? maxWidth)
		{
			this.font = font;
			MaxWidth = maxWidth;
		}

		/// <summary>
		/// Gets the maximum width for the layout.
		/// </summary>
		public int? MaxWidth { get; private set; }

		/// <summary>
		/// Gets the height of the layout.
		/// </summary>
		public int Height
		{
			get
			{
				var lastTop = layouts.Max(x => x.Destination.Y);
				var items = layouts.Where(x => x.Destination.Y == lastTop);

				return items.Max(x => x.Destination.Y + font.MeasureString(x.Text).Height);
			}
		}

		/// <summary>
		/// Adds text to the layout.
		/// </summary>
		/// <param name="text"></param>
		public void Append(string text)
		{
			LayoutText(text);
		}

		/// <summary>
		/// Renders the text in the layout.
		/// </summary>
		/// <param name="dest"></param>
		public void Draw(Point dest)
		{
			foreach (var item in layouts)
			{
				item.Draw(dest);
			}
		}

		private void Add(string text)
		{
			layouts.Add(new TextContent
			{
				Destination = insertionPoint,
				Font = font,
				Size = font.Size,
				Style = font.Style,
				Color = font.Color,
				Text = text
			});

			var size = font.MeasureString(text);

			insertionPoint.X += size.Width;
		}

		private void LayoutText(string text)
		{
			List<int> spacePositions = new List<int>();
			List<int> wrapPositions = new List<int>();

			for (int i = 0; i < text.Length; i++)
				if (text[i] == ' ' || text[i] == '\n')
					spacePositions.Add(i);

			spacePositions.Add(text.Length);

			int lineStart = 0;
			int x = insertionPoint.X;
			for (int i = 1; i < spacePositions.Count; i++)
			{
				if (spacePositions[i] < text.Length)
				{
					char ch = text[spacePositions[i]];
					if (ch == '\n')
					{
						lineStart = spacePositions[i] + 1;
						wrapPositions.Add(spacePositions[i] + 1);
						continue;
					}
				}

				int length = spacePositions[i] - lineStart;
				string subText = text.Substring(lineStart, length);
				int right = font.MeasureString(subText).Width + x;

				if (right > MaxWidth)
				{
					lineStart = spacePositions[i - 1] + 1;
					wrapPositions.Add(lineStart);
					x = 0;
				}
			}

			wrapPositions.Add(text.Length);
			wrapPositions.Sort();

			int writeLineStart = 0;

			Point dest = insertionPoint;

			for (int i = 0; i < wrapPositions.Count; i++)
			{
				if (i > 0)
				{
					insertionPoint.Y += font.FontHeight;
					insertionPoint.X = 0;
				}

				int length = wrapPositions[i] - writeLineStart;

				string subText = text.Substring(writeLineStart, length);

				Add(subText);

				writeLineStart = wrapPositions[i];
			}
		}
	}
}
