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
		private static int FontSizeStep(int minSize)
		{
			if (minSize < 18)
				return 2;
			else
				return 4;
		}

		public static IFont AgateSans => DefaultAssets.Fonts.AgateSans;
		public static IFont AgateSerif => DefaultAssets.Fonts.AgateSerif;
		public static IFont AgateMono => DefaultAssets.Fonts.AgateMono;
		
		Dictionary<FontSettings, FontSurface> mFontSurfaces = new Dictionary<FontSettings, FontSurface>();
		FontSettings mSettings = new FontSettings(12, FontStyles.None);
		FontState mState = new FontState();

		public Font(string name)
		{
			Name = name;
		}

		public void Dispose()
		{
			foreach (var fs in mFontSurfaces.Values)
				fs.Dispose();
		}

		public string Name { get; set; }
		public int Size { get { return mSettings.Size; } set { mSettings.Size = value; } }
		public FontStyles Style { get { return mSettings.Style; } set { mSettings.Style = value; } }

		public IReadOnlyDictionary<FontSettings, FontSurface> FontItems => mFontSurfaces;

		public void AddFont(FontSurface fontSurface, int size, FontStyles style)
		{
			Condition.RequireArgumentNotNull(fontSurface, nameof(fontSurface));

			AddFont(new FontSettings(size, style), fontSurface);
		}
		public void AddFont(FontSettings settings, FontSurface fontSurface)
		{
			Condition.RequireArgumentNotNull(fontSurface, nameof(fontSurface));

			mFontSurfaces[settings] = fontSurface;
		}

		public FontSurface GetFontSurface(int size, FontStyles fontStyles)
		{
			return GetFontSurface(new FontSettings(size, fontStyles));
		}
		public FontSurface GetFontSurface(FontSettings settings)
		{
			return mFontSurfaces[settings];
		}


		public int FontHeight { get { return FontSurface.FontHeight; } }

		int MaxSize(FontStyles style)
		{
			var keys = mFontSurfaces.Keys.Where(x => x.Style == style);
			if (keys.Any())
				return keys.Max(x => x.Size);
			else
				return -1;
		}

		public FontSurface FontSurface
		{
			get
			{
				var font = GetClosestFont(mSettings);
				font.State = mState;
				return font;
			}
		}

		#region --- Finding correctly sized font ---

		public FontSurface GetClosestFont(int size, FontStyles style)
		{
			return GetClosestFont(new FontSettings(size, style));
		}
		public FontSurface GetClosestFont(FontSettings fontSettings)
		{
			var settings = GetClosestFontSettings(fontSettings);
			var result = mFontSurfaces[settings];

			var ratio = fontSettings.Size / (double)settings.Size;

			result.SetScale(ratio, ratio);

			return result;
		}

		internal FontSettings GetClosestFontSettings(int size, FontStyles style)
		{
			return GetClosestFontSettings(new FontSettings(size, style));
		}
		internal FontSettings GetClosestFontSettings(FontSettings settings)
		{
			if (mFontSurfaces.ContainsKey(settings))
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
					Debug.Assert(mFontSurfaces.Count == 0);
					throw new AgateException("There are no font styles defined.");
				}
			}

			if (settings.Size > maxSize)
				return GetClosestFontSettings(maxSize, settings.Style);

			for (int i = settings.Size; i <= maxSize; i++)
			{
				settings.Size = i;

				if (mFontSurfaces.ContainsKey(settings))
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



		public double Alpha
		{
			get { return mState.Alpha; }
			set { mState.Alpha = value; }
		}
		public Color Color
		{
			get { return mState.Color; }
			set { mState.Color = value; }
		}
		public OriginAlignment DisplayAlignment
		{
			get { return mState.DisplayAlignment; }
			set { mState.DisplayAlignment = value; }
		}
		public TextImageLayout TextImageLayout
		{
			get { return mFontSurfaces.Values.First().TextImageLayout; }
			set
			{
				foreach (var fs in mFontSurfaces.Values)
					fs.TextImageLayout = value;
			}
		}

		public void DrawText(string text)
		{
			FontSurface.DrawText(text);
		}
		public void DrawText(Point dest, string text)
		{
			FontSurface.DrawText(dest, text);
		}
		public void DrawText(int x, int y, string text)
		{
			FontSurface.DrawText(x, y, text);
		}
		public void DrawText(int x, int y, string text, params object[] Parameters)
		{
			FontSurface.DrawText(x, y, text, Parameters);
		}
		public void DrawText(double x, double y, string text)
		{
			FontSurface.DrawText(x, y, text);
		}
		public void DrawText(PointF dest, string text)
		{
			FontSurface.DrawText(dest, text);
		}

		public Size MeasureString(string text)
		{
			return FontSurface.MeasureString(text);
		}

	}

}
