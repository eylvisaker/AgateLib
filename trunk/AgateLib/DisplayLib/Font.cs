using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.DisplayLib
{
	public class Font : IDisposable
	{
		Dictionary<FontSettings, FontSurface> mFontSurfaces = new Dictionary<FontSettings, FontSurface>();
		FontSettings mSettings;
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

		public void AddFont(FontSurface fontSurface, int size, FontStyles style)
		{
			AddFont(new FontSettings(size, style), fontSurface);
		}
		public void AddFont(FontSettings settings, FontSurface fontSurface)
		{
			if (Size == 0)
				mSettings = settings;

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
			if (keys.Count() > 0)
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
			var retval = mFontSurfaces[settings];

			var ratio = fontSettings.Size / (double)settings.Size;

			retval.SetScale(ratio, ratio);

			return retval;
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

		public Size MeasureString(string text)
		{
			return FontSurface.MeasureString(text);
		}

		[Obsolete("This needs to be thought through more carefully.")]
		internal static Font Create(string name, int minSize, int maxSize, FontStyles styles)
		{
			Font retval = new Font(name);

			int inc = FontSizeStep(minSize);
			bool bold = (styles & FontStyles.Bold) == FontStyles.Bold;
			bool italic = (styles & FontStyles.Italic) == FontStyles.Italic;

			for (int size = minSize; size <= maxSize; size += inc)
			{
				retval.AddFont(new FontSurface(name, size, FontStyles.None), size, FontStyles.None);

				if (bold)
					retval.AddFont(new FontSurface(name, size, FontStyles.Bold), size, FontStyles.Bold);
				if (italic)
					retval.AddFont(new FontSurface(name, size, FontStyles.Italic), size, FontStyles.Italic);
				if (bold && italic)
					retval.AddFont(new FontSurface(name, size, FontStyles.Bold | FontStyles.Italic), size, FontStyles.Bold | FontStyles.Italic);
			}

			return retval;
		}

		private static int FontSizeStep(int minSize)
		{
			if (minSize < 18)
				return 2;
			else
				return 4;
		}
	}

}
