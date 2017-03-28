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
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which represents a font. Font objects have their own state, but a new font can be constructed
	/// from an existing font to separate the state.
	/// </summary>
	public class Font : IFont
	{
		/// <summary>
		/// Built-in sans serif font.
		/// </summary>
		public static Font AgateSans => AgateApp.State.Display.DefaultResources.AgateSans;

		/// <summary>
		/// Built-in serif font
		/// </summary>
		public static Font AgateSerif => AgateApp.State.Display.DefaultResources.AgateSerif;

		/// <summary>
		/// Built-in monospae font.
		/// </summary>
		public static Font AgateMono => AgateApp.State.Display.DefaultResources.AgateMono;

		IFontCore core;
		FontState state = new FontState();

		/// <summary>
		/// Creates a new, empty font object.
		/// </summary>
		/// <param name="name"></param>
		public Font(string name)
		{
			core = new FontCore(name);
		}

		/// <summary>
		/// Creates a copy of another font with its own state.
		/// </summary>
		/// <param name="prototypeFont"></param>
		public Font(Font prototypeFont)
		{
			core = prototypeFont.Core;

			state = prototypeFont.state.Clone();
		}

		/// <summary>
		/// Creates a copy of a another font with its own state and size.
		/// </summary>
		/// <param name="prototypeFont"></param>
		/// <param name="size"></param>
		public Font(Font prototypeFont, int size)
		{
			core = prototypeFont.Core;

			state = prototypeFont.state.Clone();
			state.Size = size;
		}

		/// <summary>
		/// Creates a copy of a another font with its own state and style.
		/// </summary>
		/// <param name="prototypeFont"></param>
		/// <param name="style"></param>
		public Font(Font prototypeFont, FontStyles style)
		{
			core = prototypeFont.Core;

			state = prototypeFont.state.Clone();
			state.Style = style;
		}

		/// <summary>
		/// Creates a copy of a another font with its own state, size and style.
		/// </summary>
		/// <param name="prototypeFont"></param>
		/// <param name="size"></param>
		/// <param name="style"></param>
		public Font(Font prototypeFont, int size, FontStyles style)
		{
			core = prototypeFont.Core;

			state = prototypeFont.state.Clone();
			state.Size = size;
			state.Style = style;
		}

		internal IFontCore Core => core;

		/// <summary>
		/// Gets the collection of FontSurface objects that make up the font.
		/// </summary>
		public IReadOnlyDictionary<FontSettings, FontSurface> FontSurfaces
			=> core.FontItems;

		/// <summary>
		/// Gets the name of the font.
		/// </summary>
		public string Name => core.Name;

		/// <summary>
		/// Gets or sets the alpha blending value.
		/// </summary>
		public double Alpha
		{
			get { return state.Alpha; }
			set { state.Alpha = value; }
		}

		/// <summary>
		/// Gets or sets the drawing color.
		/// </summary>
		public Color Color
		{
			get { return state.Color; }
			set { state.Color = value; }
		}

		/// <summary>
		/// Gets or sets the text alignment when drawn.
		/// </summary>
		public OriginAlignment TextAlignment
		{
			get { return state.TextAlignment; }
			set { state.TextAlignment = value; }
		}

		/// <summary>
		/// Gets or sets the display alignment.
		/// </summary>
		[Obsolete("Use TextAlignment instead.", true)]
		public OriginAlignment DisplayAlignment
		{
			get { return TextAlignment; }
			set { TextAlignment = value; }
		}

		/// <summary>
		/// Gets the height of a single line of text using this font in its current state.
		/// </summary>
		public int FontHeight => core.FontHeight(state);

		/// <summary>
		/// Gets or sets the size of the font.
		/// </summary>
		public int Size
		{
			get { return state.Size; }
			set { state.Size = value; }
		}

		/// <summary>
		/// Gets or sets the style of the font.
		/// </summary>
		public FontStyles Style
		{
			get { return state.Style; }
			set { state.Style = value; }
		}

		/// <summary>
		/// Gets or sets how text should be laid out around images.
		/// </summary>
		public TextImageLayout TextImageLayout
		{
			get { return state.TextImageLayout; }
			set { state.TextImageLayout = value; }
		}

		/// <summary>
		/// Gets or sets the interpolation hint of the text.
		/// </summary>
		public InterpolationMode InterpolationHint
		{
			get { return state.InterpolationHint; }
			set { state.InterpolationHint = value; }
		}

		/// <summary>
		/// Disposes of the Font object.
		/// </summary>
		public void Dispose()
		{
			core.Dispose();
		}

		/// <summary>
		/// Draws text at the point (0, 0).
		/// </summary>
		/// <param name="text"></param>
		public void DrawText(string text)
		{
			core.DrawText(state, text);
		}

		/// <summary>
		/// Draws text at the specified point.
		/// </summary>
		/// <param name="dest"></param>
		/// <param name="text"></param>
		public void DrawText(Vector2 dest, string text)
		{
			core.DrawText(state, dest, text);
		}

		/// <summary>
		/// Draws text at the specified point.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="text"></param>
		/// <param name="parameters"></param>
		public void DrawText(Vector2 dest, string text, params object[] parameters)
		{
			core.DrawText(state, dest, text, parameters);
		}
		
		/// <summary>
		/// Returns the size the text would take given the current settings.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public Size MeasureString(string text)
		{
			return core.MeasureString(state, text);
		}

		/// <summary>
		/// Converts to a string for debug output.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"{core.Name} {Size} Style:{Style}";
		}

		internal void AddFontSurface(FontSettings settings, FontSurface fontSurface)
		{
			core.AddFontSurface(settings, fontSurface);
		}
	}

	/// <summary>
	/// Interface for a Font object.
	/// </summary>
	public interface IFont : IDisposable
	{
		/// <summary>
		/// Gets or sets the rendering color.
		/// </summary>
		Color Color { get; set; }

		/// <summary>
		/// Gets or sets the display alignment.
		/// </summary>
		OriginAlignment DisplayAlignment { get; set; }

		/// <summary>
		/// Gets the height of a single line of text.
		/// </summary>
		int FontHeight { get; }

		/// <summary>
		/// Gets or sets the style of the font.
		/// </summary>
		FontStyles Style { get; set; }

		/// <summary>
		/// Gets or sets the size of the font.
		/// </summary>
		int Size { get; set; }

		/// <summary>
		/// Gets or sets how text is laid out around images.
		/// </summary>
		TextImageLayout TextImageLayout { get; set; }

		/// <summary>
		/// Gets or sets alpha blending value.
		/// </summary>
		double Alpha { get; set; }

		/// <summary>
		/// Gets the name of the font.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Draws text at (0, 0).
		/// </summary>
		/// <param name="text"></param>
		void DrawText(string text);
		
		/// <summary>
		/// Draws text at the specified point.
		/// </summary>
		/// <param name="dest"></param>
		/// <param name="text"></param>
		void DrawText(Vector2 dest, string text);

		/// <summary>
		/// Draws text at the specified point.
		/// </summary>
		/// <param name="dest"></param>
		/// <param name="text"></param>
		/// <param name="parameters"></param>
		void DrawText(Vector2 dest, string text, params object[] parameters);

		/// <summary>
		/// Returns the size of the text when rendered given the current size/style settings.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		Size MeasureString(string text);
	}
}
