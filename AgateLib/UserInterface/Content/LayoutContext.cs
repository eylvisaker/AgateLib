//
//    Copyright (c) 2006-2018 Erik Ylvisaker
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
using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using AgateLib.Quality;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AgateLib.UserInterface.Content.Commands;

namespace AgateLib.UserInterface.Content
{
	public class LayoutContext
	{
		private List<string> tokens;
		private int currentToken = -1;
		private List<IContentLayoutItem> layoutItems = new List<IContentLayoutItem>();
		private readonly ContentLayoutOptions options;

		public LayoutContext(List<string> tokens, ContentLayoutOptions layoutOptions)
		{
			Require.ArgumentNotNull(tokens, nameof(tokens));
			Require.ArgumentNotNull(layoutOptions, nameof(layoutOptions));

			this.tokens = tokens;
			this.options = layoutOptions;
		}

		public bool AnyTokensLeft => currentToken < tokens.Count - 1;

		public string CurrentToken => currentToken < tokens.Count ? tokens[currentToken] : null;

		public IContentLayout Layout => new ContentLayout(layoutItems);

		public int MaxWidth => options.MaxWidth;

		public ContentLayoutOptions Options => options;

		public Point InsertionPoint;

		public int LineHeight { get; set; }

		public Font Font => options.Font;

		public string ReadNextToken()
		{
			currentToken++;

			return CurrentToken;
		}

		public void ExpandLineHeight(int height)
		{
			if (LineHeight < height)
				LineHeight = height;
		}

		public Vector2 ReserveSpace(Size size)
		{
			if (InsertionPoint.X + size.Width > MaxWidth)
			{
				InsertionPoint.X = 0;
				InsertionPoint.Y += LineHeight;
				LineHeight = 0;
			}

			Vector2 result = InsertionPoint.ToVector2();

			InsertionPoint.X += size.Width;
			ExpandLineHeight(size.Height);

			return result;
		}

		public void NewLine()
		{
			InsertionPoint.X = 0;
			InsertionPoint.Y += Math.Max(LineHeight, Font.FontHeight);
			LineHeight = 0;
		}

		public void AppendText(string text, int recursion = 0)
		{
			if (string.IsNullOrEmpty(text))
				return;

			var font = new Font(Font);
			
			while (text.StartsWith("\n") || text.StartsWith("\r\n"))
			{
				NewLine();

				if (text.StartsWith("\n"))
				{
					if (text.Length == 1)
						return;

					text = text.Substring(1);
				}
				else if (text.StartsWith("\r\n"))
				{
					if (text.Length == 2)
						return;

					text = text.Substring(2);
				}
			}

			var newLineIndex = text.IndexOf('\n');

			if (newLineIndex > 0)
			{
				if (text[newLineIndex - 1] == '\r')
					newLineIndex--;

				AppendText(text.Substring(0, newLineIndex), recursion + 1);
				AppendText(text.Substring(newLineIndex), recursion + 1);
				return;
			}

			string thisLineText = text;
			int advance = thisLineText.Length;

			var textSize = font.MeasureString(thisLineText);

			while (InsertionPoint.X + textSize.Width > MaxWidth)
			{
				var whiteSpaceIndex = thisLineText.LastIndexOf(' ');

				if (whiteSpaceIndex <= 0)
				{
					if (InsertionPoint.X == 0)
					{
						// insert the word anyway, since it's too long for the full width.
						break;
					}

					NewLine();

					thisLineText = thisLineText.TrimStart();
					textSize = font.MeasureString(thisLineText);
					continue;
				}

				thisLineText = thisLineText.Substring(0, whiteSpaceIndex);
				advance = whiteSpaceIndex;
				textSize = font.MeasureString(thisLineText);
			}

			var item = new ContentText(thisLineText, font, InsertionPoint.ToVector2());

			ReserveSpace(item.Size);

			Add(item);

			if (advance < text.Length)
			{
				var remainingText = text.Substring(advance);

				AppendText(remainingText, recursion + 1);
			}
		}

		public void Add(IContentLayoutItem item)
		{
			layoutItems.Add(item);
		}
	}
}