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
using AgateLib.Geometry;
using AgateLib.Quality;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.DisplayLib
{
	public class Font : IFont
	{
		public static Font AgateSans => AgateApp.State.Display.DefaultResources.AgateSans;
		public static Font AgateSerif => AgateApp.State.Display.DefaultResources.AgateSerif;
		public static Font AgateMono => AgateApp.State.Display.DefaultResources.AgateMono;

		IFontImpl impl;
		FontState state = new FontState();

		/// <summary>
		/// Creates a new, empty font object.
		/// </summary>
		/// <param name="name"></param>
		public Font(string name)
		{
			impl = new FontImplementation(name);
		}

		/// <summary>
		/// Creates a copy of another font with its own state.
		/// </summary>
		/// <param name="prototypeFont"></param>
		public Font(Font prototypeFont)
		{
			impl = prototypeFont.Impl;

			state = prototypeFont.state.Clone();
		}

		/// <summary>
		/// Creates a copy of a another font with its own state and size.
		/// </summary>
		/// <param name="prototypeFont"></param>
		/// <param name="size"></param>
		public Font(Font prototypeFont, int size)
		{
			impl = prototypeFont.Impl;

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
			impl = prototypeFont.Impl;

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
			impl = prototypeFont.Impl;

			state = prototypeFont.state.Clone();
			state.Size = size;
			state.Style = style;
		}

		internal IFontImpl Impl => impl;

		public string Name => impl.Name;

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

		public int FontHeight => impl.FontHeight(state);

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

		public void Dispose()
		{
			impl.Dispose();
		}

		public void DrawText(string text)
		{
			impl.DrawText(state, text);
		}

		public void DrawText(PointF dest, string text)
		{
			impl.DrawText(state, dest, text);
		}

		public void DrawText(Point dest, string text)
		{
			impl.DrawText(state, dest, text);
		}

		public void DrawText(double x, double y, string text)
		{
			impl.DrawText(state, x, y, text);
		}

		public void DrawText(int x, int y, string text)
		{
			impl.DrawText(state, x, y, text);
		}

		public void DrawText(int x, int y, string text, params object[] Parameters)
		{
			impl.DrawText(state, x, y, text, Parameters);
		}

		public Size MeasureString(string text)
		{
			return impl.MeasureString(state, text);
		}

		internal void AddFontSurface(FontSettings settings, FontSurface fontSurface)
		{
			impl.AddFontSurface(settings, fontSurface);
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

	internal interface IFontImpl : IDisposable
	{
		string Name { get; }

		int FontHeight(FontState state);

		void DrawText(FontState state, string text);
		void DrawText(FontState state, Point dest, string text);
		void DrawText(FontState state, int x, int y, string text);
		void DrawText(FontState state, int x, int y, string text, params object[] parameters);
		void DrawText(FontState state, double x, double y, string text);
		void DrawText(FontState state, PointF dest, string text);

		Size MeasureString(FontState state, string text);

		void AddFontSurface(FontSettings settings, FontSurface fontSurface);

		FontSettings GetClosestFontSettings(FontSettings settings);

		FontSurface FontSurface(FontState fontState);
	}

	internal class FontImplementation : IFontImpl
	{
		// TODO: Move this to somewhere so this knowledge doesn't get lost.
		private static int FontSizeStep(int minSize)
		{
			if (minSize < 18)
				return 2;
			else
				return 4;
		}

		Dictionary<FontSettings, FontSurface> fontSurfaces = new Dictionary<FontSettings, FontSurface>();

		public FontImplementation(string name)
		{
			Name = name;
		}

		public void Dispose()
		{
			foreach (var fs in fontSurfaces.Values)
				fs.Dispose();
		}

		public string Name { get; set; }

		public IReadOnlyDictionary<FontSettings, FontSurface> FontItems => fontSurfaces;

		public void AddFontSurface(FontSettings settings, FontSurface fontSurface)
		{
			Require.ArgumentNotNull(fontSurface, nameof(fontSurface));

			fontSurfaces[settings] = fontSurface;
		}

		public FontSurface GetFontSurface(int size, FontStyles fontStyles)
		{
			return GetFontSurface(new FontSettings(size, fontStyles));
		}
		public FontSurface GetFontSurface(FontSettings settings)
		{
			return fontSurfaces[settings];
		}

		public int FontHeight(FontState state)
		{
			var surface = FontSurface(state);
			return surface.FontHeight(state);
		}

		int MaxSize(FontStyles style)
		{
			var keys = fontSurfaces.Keys.Where(x => x.Style == style);
			if (keys.Any())
				return keys.Max(x => x.Size);
			else
				return -1;
		}

		#region --- Finding correctly sized font ---

		public FontSurface FontSurface(FontState state)
		{
			var settings = GetClosestFontSettings(state.Settings);
			var result = fontSurfaces[settings];

			var ratio = state.Settings.Size / (double)settings.Size;

			state.ScaleHeight = ratio;
			state.ScaleWidth = ratio;

			return result;
		}

		internal FontSettings GetClosestFontSettings(int size, FontStyles style)
		{
			return GetClosestFontSettings(new FontSettings(size, style));
		}
		public FontSettings GetClosestFontSettings(FontSettings settings)
		{
			if (fontSurfaces.ContainsKey(settings))
				return settings;

			int maxSize = MaxSize(settings.Style);

			// this happens if we have no font surfaces of this style.
			if (maxSize <= 0)
			{
				FontStyles newStyle;

				// OK remove styles until we find an actual font.
				if (TryRemoveStyle(settings.Style, FontStyles.Strikeout, out newStyle))
					return GetClosestFontSettings(settings.Size, newStyle);
				if (TryRemoveStyle(settings.Style, FontStyles.Italic, out newStyle))
					return GetClosestFontSettings(settings.Size, newStyle);
				if (TryRemoveStyle(settings.Style, FontStyles.Underline, out newStyle))
					return GetClosestFontSettings(settings.Size, newStyle);
				if (TryRemoveStyle(settings.Style, FontStyles.Bold, out newStyle))
					return GetClosestFontSettings(settings.Size, newStyle);
				else
				{
					Debug.Assert(fontSurfaces.Count == 0);
					throw new AgateException("There are no font styles defined.");
				}
			}

			if (settings.Size > maxSize)
				return GetClosestFontSettings(maxSize, settings.Style);

			for (int i = settings.Size; i <= maxSize; i++)
			{
				settings.Size = i;

				if (fontSurfaces.ContainsKey(settings))
					return settings;
			}

			throw new AgateException("Could not find a valid font.");
		}


		#endregion

		private bool TryRemoveStyle(FontStyles value, FontStyles remove, out FontStyles result)
		{
			if ((value & remove) == remove)
			{
				result = ~(~value | remove);
				return true;
			}
			else
			{
				result = 0;
				return false;
			}
		}

		public void DrawText(FontState state, string text)
		{
			FontSurface(state).DrawText(state, text);
		}
		public void DrawText(FontState state, Point dest, string text)
		{
			FontSurface(state).DrawText(state, dest, text);
		}
		public void DrawText(FontState state, int x, int y, string text)
		{
			FontSurface(state).DrawText(state, x, y, text);
		}
		public void DrawText(FontState state, int x, int y, string text, params object[] parameters)
		{
			FontSurface(state).DrawText(state, x, y, text, parameters);
		}
		public void DrawText(FontState state, double x, double y, string text)
		{
			FontSurface(state).DrawText(state, x, y, text);
		}
		public void DrawText(FontState state, PointF dest, string text)
		{
			FontSurface(state).DrawText(state, dest, text);
		}

		public Size MeasureString(FontState state, string text)
		{
			return FontSurface(state).MeasureString(state, text);
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
