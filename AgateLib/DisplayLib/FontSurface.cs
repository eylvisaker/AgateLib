//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which represents a font to draw on the screen.
	/// <remarks>When creating a FontSurface, if you are going to be
	/// scaling the font, it usually looks much better to make a large font
	/// and scale it to a smaller size, rather than vice-versa.</remarks>
	/// </summary>
	public sealed class FontSurface : IDisposable
	{
		static Regex substituteMatch = new Regex(@"\{.*?\}|\{\{\}|\{\}\}|\r\n|\n");
		static Regex indexMatch = new Regex(@"[0-9]+:?");

		private FontSurfaceImpl mImpl;
		private StringTransformer mTransformer = StringTransformer.None;

		/// <summary>
		/// Initializer passing in a FontSurfaceImpl object.
		/// </summary>
		/// <param name="implToUse"></param>
		public FontSurface(FontSurfaceImpl implToUse)
		{
			if (implToUse == null)
				throw new ArgumentNullException("implToUse");

			mImpl = implToUse;
			Display.DisposeDisplay += Dispose;
		}

		/// <summary>
		/// Returns the implementation object.
		/// </summary>
		public FontSurfaceImpl Impl
		{
			get { return mImpl; }
		}

		/// <summary>
		/// Gets the name of the font.
		/// </summary>
		public string FontName
		{
			get { return mImpl.FontName; }
		}

		/// <summary>
		/// Initializes a FontSurface object with a given implementation object.
		/// </summary>
		/// <param name="implToUse"></param>
		/// <returns></returns>
		public static FontSurface FromImpl(FontSurfaceImpl implToUse)
		{
			return new FontSurface(implToUse);
		}

		/// <summary>
		/// This function loads a monospace bitmap font from the specified image file.
		/// Only the character size is given.  It is assumed that all ASCII characters 
		/// from 0 to 255 are present, in order from left to right, and top to bottom.
		/// </summary>
		/// <remarks>
		/// [Experimental - The API is likely to change in the future.]
		/// </remarks>
		/// <param name="filename"></param>
		/// <param name="characterSize"></param>
		/// <returns></returns>
		public static FontSurface BitmapMonospace(string filename, Size characterSize)
		{
			FontSurfaceImpl impl = new BitmapFontImpl(filename, characterSize);

			return new FontSurface(impl);
		}

		/// <summary>
		/// Disposes of this object.
		/// </summary>
		public void Dispose()
		{
			mImpl?.Dispose();

			mImpl = null;
		}


		/// <summary>
		/// Measures the display size of the specified string.
		/// </summary>
		/// <param name="state">The FontState object to use.</param>
		/// <param name="text"></param>
		/// <returns></returns>
		public Size MeasureString(FontState state, string text)
		{
			return mImpl.MeasureString(state, text);
		}

		/// <summary>
		/// Gets the height in pixels of a single line of text.
		/// </summary>
		public int FontHeight(FontState state)
		{
			return mImpl.FontHeight(state);
		}

		/// <summary>
		/// Draws the specified string at the specified location.
		/// </summary>
		/// <param name="state">The FontState object to use.</param>
		/// <param name="destX"></param>
		/// <param name="destY"></param>
		/// <param name="text"></param>
		public void DrawText(FontState state, double destX, double destY, string text)
		{
			if (string.IsNullOrEmpty(text))
				return;

			state.Location = new PointF((float)destX, (float)destY);
			state.Text = mTransformer.Transform(text);

			DrawText(state);
		}
		/// <summary>
		/// Draws the specified string at the specified location.
		/// </summary>
		/// <param name="state">The FontState object to use.</param>
		/// <param name="destPt"></param>
		/// <param name="text"></param>
		public void DrawText(FontState state, Point destPt, string text)
		{
			if (string.IsNullOrEmpty(text))
				return;

			state.Location = new PointF(destPt.X, destPt.Y);
			state.Text = mTransformer.Transform(text);

			DrawText(state);
		}
		/// <summary>
		/// Draws the specified string at the specified location.
		/// </summary>
		/// <param name="state">The FontState object to use.</param>
		/// <param name="destPt"></param>
		/// <param name="text"></param>
		public void DrawText(FontState state, PointF destPt, string text)
		{
			if (string.IsNullOrEmpty(text))
				return;

			state.Location = destPt;
			state.Text = mTransformer.Transform(text);

			DrawText(state);
		}
		/// <summary>
		/// Draws the specified string at the origin.
		/// </summary>
		/// <param name="state">The state object to use.</param>
		/// <param name="text"></param>
		public void DrawText(FontState state, string text)
		{
			if (string.IsNullOrEmpty(text))
				return;

			state.Location = PointF.Empty;
			state.Text = mTransformer.Transform(text);

			DrawText(state);
		}
		/// <summary>
		/// Draws text using the specified FontState object.
		/// </summary>
		/// <param name="state">The FontState to use.</param>
		public void DrawText(FontState state)
		{
			mImpl.DrawText(state);
		}
		/// <summary>
		/// Draws formatted text.
		/// </summary>
		/// <param name="state">The state object to use.</param>
		/// <param name="destX">X position of destination.</param>
		/// <param name="destY">Y position of destination.</param>
		/// <param name="formatString">The formatting string.</param>
		/// <param name="args">Arguments that are used to fill {x} members of the formatString.  Surface objects
		/// are laid out according to the TextImageLayout member.</param>
		public void DrawText(FontState state, int destX, int destY, string formatString, params object[] args)
		{
			if (string.IsNullOrEmpty(formatString))
				return;

			TextLayoutCache layout = CreateLayout(state, formatString, args);

			layout.Translate(new Point(destX, destY));
			layout.DrawAll();
		}

		/// <summary>
		/// Creates a text layout from a format string and list of arguments.
		/// </summary>
		/// <param name="formatString">The formatting string.</param>
		/// <param name="args">Arguments that are used to fill {x} members of the formatString.  Surface objects
		/// are laid out according to the TextImageLayout member.</param>
		/// <returns>Returns a TextLayout object which contains all the layout information needed to draw
		/// the text/images on screen.</returns>
		public TextLayoutCache CreateLayout(FontState state, string formatString, params object[] args)
		{
			var matches = substituteMatch.Matches(formatString);

			if (matches.Count == 0)
			{
				var singleLayout = new LayoutText
				{
					Font = this,
					State = state.Clone(),
					LineIndex = 0,
					Text = formatString
				};

				singleLayout.State.Location = PointF.Empty;

				return new TextLayoutCache { singleLayout };
			}

			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] == null)
					args[i] = "null";
			}

			int lastIndex = 0;
			string result = string.Empty;

			PointF dest = PointF.Empty;

			TextLayoutCache layout = new TextLayoutCache();
			int lineHeight = FontHeight(state);
			int spaceAboveLine = 0;
			int lineIndex = 0;
			LayoutCacheAlterFont currentAlterText = null;

			for (int i = 0; i < matches.Count; i++)
			{
				string format = formatString.Substring(matches[i].Index, matches[i].Length);

				result += formatString.Substring(lastIndex, matches[i].Index - lastIndex);

				var argsIndexText = indexMatch.Match(format);
				int argsIndex;

				if (format == "\r\n" || format == "\n")
				{
					PushLayoutText(state, lineIndex, layout, ref dest, ref lineHeight, ref spaceAboveLine,
						result, currentAlterText);

					result = string.Empty;

					ShiftLine(layout, spaceAboveLine, lineIndex);

					dest.X = 0;
					dest.Y += lineHeight;

					lineIndex++;
					lineHeight = FontHeight(state);

					spaceAboveLine = 0;
				}
				else if (int.TryParse(argsIndexText.ToString(), out argsIndex))
				{
					if (argsIndex >= args.Length)
					{
						throw new IndexOutOfRangeException(string.Format(
							"Argument number {0} was specified, but only {1} arguments were given.", argsIndex, args.Length));
					}
					object obj = args[argsIndex];

					if (obj is ISurface)
					{
						PushLayoutText(state, lineIndex, layout, ref dest, ref lineHeight, ref spaceAboveLine,
							result, currentAlterText);

						PushLayoutImage(state, lineIndex, layout, ref dest, ref lineHeight, ref spaceAboveLine,
							(ISurface)obj);

						result = string.Empty;
					}
					else if (obj is LayoutCacheAlterFont)
					{
						// push text with the old state
						PushLayoutText(state, lineIndex, layout, ref dest, ref lineHeight, ref spaceAboveLine,
							result, currentAlterText);

						// store the new alter object to affect the state of the next block.
						currentAlterText = (LayoutCacheAlterFont)obj;
						result = string.Empty;
					}
					else
					{
						result += ConvertToString(obj, format);
					}
				}
				else if (format.StartsWith("{"))
				{
					if (format == "{{}")
						result += "{";
					else if (format == "{}}")
						result += "}";
				}

				lastIndex = matches[i].Index + matches[i].Length;
			}

			result += formatString.Substring(lastIndex);
			PushLayoutText(state, lineIndex, layout, ref dest, ref lineHeight, ref spaceAboveLine,
				result, currentAlterText);

			ShiftLine(layout, spaceAboveLine, lineIndex);

			return layout;
		}

		private static void ShiftLine(IEnumerable<LayoutCacheItem> layout, int lineShift, int lineIndex)
		{
			foreach (var item in layout.Where(x => x.LineIndex == lineIndex))
			{
				item.Location = new PointF(
					item.Location.X, item.Location.Y + lineShift);
			}
		}

		private void PushLayoutImage(FontState state, int lineIndex, TextLayoutCache layout,
			ref PointF dest, ref int lineHeight, ref int spaceAboveLine,
			ISurface surface)
		{
			if (layout == null)
				throw new ArgumentNullException("layout");

			int newSpaceAbove;
			LayoutCacheSurface t = new LayoutCacheSurface { Location = dest, Surface = surface, LineIndex = lineIndex };
			t.State = surface.State.Clone();

			var update = Origin.Calc(state.DisplayAlignment, surface.SurfaceSize);

			lineHeight = Math.Max(lineHeight, surface.DisplayHeight);
			dest.X += surface.DisplayWidth;

			switch (state.TextImageLayout)
			{
				case TextImageLayout.InlineTop:
					break;
				case TextImageLayout.InlineCenter:
					newSpaceAbove = (surface.DisplayHeight - FontHeight(state)) / 2;
					t.Y -= newSpaceAbove;
					spaceAboveLine = Math.Max(spaceAboveLine, newSpaceAbove);

					break;

				case TextImageLayout.InlineBottom:
					newSpaceAbove = surface.DisplayHeight - FontHeight(state);
					t.Y -= newSpaceAbove;
					spaceAboveLine = Math.Max(spaceAboveLine, newSpaceAbove);

					break;
			}

			layout.Add(t);
		}

		private void PushLayoutText(FontState state, int lineIndex, TextLayoutCache layout,
			ref PointF dest, ref int lineHeight, ref int spaceAboveLine,
			string text, LayoutCacheAlterFont alter)
		{
			if (string.IsNullOrEmpty(text))
				return;

			LayoutText t = new LayoutText
			{
				Font = this,
				State = state.Clone(),
				Location = dest,
				Text = text,
				LineIndex = lineIndex
			};

			if (alter != null)
			{
				alter.ModifyState(t.State);
			}

			var size = MeasureString(t.State, text);
			var update = Origin.Calc(state.DisplayAlignment, size);

			int newSpaceAbove = size.Height - FontHeight(state);
			t.Y -= newSpaceAbove;
			spaceAboveLine = Math.Max(spaceAboveLine, newSpaceAbove);

			dest.X += size.Width;

			layout.Add(t);
		}

		private string ConvertToString(object obj, string format)
		{
			return obj.ToString();
		}
	}
}
