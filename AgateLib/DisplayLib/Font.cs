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

		public IReadOnlyDictionary<FontSettings, FontSurface> FontSurfaces
			=> core.FontItems;

		public string Name => core.Name;

		public double Alpha
		{
			get { return state.Alpha; }
			set { state.Alpha = value; }
		}

		public Color Color
		{
			get { return state.Color; }
			set { state.Color = value; }
		}

		public OriginAlignment DisplayAlignment
		{
			get{return state.DisplayAlignment;}
			set{state.DisplayAlignment = value;}
		}

		public int FontHeight => core.FontHeight(state);

		public int Size
		{
			get { return state.Size; }
			set { state.Size = value; }
		}

		public FontStyles Style
		{
			get { return state.Style; }
			set { state.Style = value; }
		}

		public TextImageLayout TextImageLayout
		{
			get { return state.TextImageLayout; }
			set { state.TextImageLayout = value; }
		}

		public InterpolationMode InterpolationHint
		{
			get { return state.InterpolationHint; }
			set { state.InterpolationHint = value; }
		}

		public void Dispose()
		{
			core.Dispose();
		}

		public void DrawText(string text)
		{
			core.DrawText(state, text);
		}

		public void DrawText(PointF dest, string text)
		{
			core.DrawText(state, dest, text);
		}

		public void DrawText(Point dest, string text)
		{
			core.DrawText(state, dest, text);
		}

		public void DrawText(double x, double y, string text)
		{
			core.DrawText(state, x, y, text);
		}

		public void DrawText(int x, int y, string text)
		{
			core.DrawText(state, x, y, text);
		}

		public void DrawText(int x, int y, string text, params object[] Parameters)
		{
			core.DrawText(state, x, y, text, Parameters);
		}

		public Size MeasureString(string text)
		{
			return core.MeasureString(state, text);
		}
		
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

	public interface IFont : IDisposable
	{
		Color Color { get; set; }
		OriginAlignment DisplayAlignment { get; set; }
		int FontHeight { get; }
		FontStyles Style { get; set; }
		int Size { get; set; }
		TextImageLayout TextImageLayout { get; set; }
		double Alpha { get; set; }
		string Name { get; }

		void DrawText(string text);
		void DrawText(Point dest, string text);
		void DrawText(int x, int y, string text);
		void DrawText(int x, int y, string text, params object[] Parameters);
		void DrawText(double x, double y, string text);
		void DrawText(PointF dest, string text);

		Size MeasureString(string text);
	}
}
