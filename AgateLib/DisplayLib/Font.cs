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

using AgateLib.Quality;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib.BitmapFont;
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
		/// Gets or sets the display alignment.
		/// </summary>
		public OriginAlignment DisplayAlignment
		{
			get{return state.DisplayAlignment;}
			set{state.DisplayAlignment = value;}
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
		public void DrawText(PointF dest, string text)
		{
			core.DrawText(state, dest, text);
		}

		/// <summary>
		/// Draws text at the specified point.
		/// </summary>
		/// <param name="dest"></param>
		/// <param name="text"></param>
		public void DrawText(Point dest, string text)
		{
			core.DrawText(state, dest, text);
		}

		/// <summary>
		/// Draws text at the specified point.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="text"></param>
		public void DrawText(double x, double y, string text)
		{
			core.DrawText(state, x, y, text);
		}

		/// <summary>
		/// Draws text at the specified point.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="text"></param>
		public void DrawText(int x, int y, string text)
		{
			core.DrawText(state, x, y, text);
		}

		/// <summary>
		/// Draws text at the specified point.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="text"></param>
		/// <param name="Parameters"></param>
		public void DrawText(int x, int y, string text, params object[] Parameters)
		{
			core.DrawText(state, x, y, text, Parameters);
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
	/// Constructs a font from a set of font surfaces.
	/// </summary>
	public class FontBuilder
	{
		private Font font;

		public FontBuilder(string name)
		{
			font = new Font(name);
		}

		/// <summary>
		/// Adds a font surface to the font.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="surface"></param>
		/// <returns></returns>
		public FontBuilder AddFontSurface(FontSettings settings, FontSurface surface)
		{
			Condition.Requires<InvalidOperationException>(font != null,
				"FontBuilder objects cannot be reused.");

			font.AddFontSurface(settings, surface);

			return this;
		}

		/// <summary>
		/// Builds the resulting Font object.
		/// </summary>
		/// <returns></returns>
		public Font Build()
		{
			var result = font;

			font = null;

			return result;
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
		void DrawText(Point dest, string text);

		/// <summary>
		/// Draws text at the specified point.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="text"></param>
		void DrawText(int x, int y, string text);

		/// <summary>
		/// Draws text at the specified point.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="text"></param>
		/// <param name="parameters"></param>
		void DrawText(int x, int y, string text, params object[] parameters);

		/// <summary>
		/// Draws text at the specified point.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="text"></param>
		void DrawText(double x, double y, string text);

		/// <summary>
		/// Draws text at the specified point.
		/// </summary>
		/// <param name="dest"></param>
		/// <param name="text"></param>
		void DrawText(PointF dest, string text);

		/// <summary>
		/// Returns the size of the text when rendered given the current size/style settings.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		Size MeasureString(string text);
	}
}
