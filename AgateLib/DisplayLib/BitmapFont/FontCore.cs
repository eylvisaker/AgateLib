using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Quality;

namespace AgateLib.DisplayLib.BitmapFont
{
	internal class FontCore : IFontCore
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

		public FontCore(string name)
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

		public void DrawText(FontState state, Vector2 dest, string text)
		{
			FontSurface(state).DrawText(state, dest, text);
		}

		public void DrawText(FontState state, Vector2 dest, string text, params object[] parameters)
		{
			FontSurface(state).DrawText(state, dest, text, parameters);
		}

		public Size MeasureString(FontState state, string text)
		{
			return FontSurface(state).MeasureString(state, text);
		}
	}

	internal interface IFontCore : IDisposable
	{
		string Name { get; }

		IReadOnlyDictionary<FontSettings, FontSurface> FontItems { get; }

		int FontHeight(FontState state);

		void DrawText(FontState state, string text);
		void DrawText(FontState state, Vector2 dest, string text);
		void DrawText(FontState state, Vector2 dest, string text, params object[] parameters);

		Size MeasureString(FontState state, string text);

		void AddFontSurface(FontSettings settings, FontSurface fontSurface);

		FontSettings GetClosestFontSettings(FontSettings settings);

		FontSurface FontSurface(FontState fontState);

	}
}