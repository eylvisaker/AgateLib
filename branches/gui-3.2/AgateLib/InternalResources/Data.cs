using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Resources;
using AgateLib.Utility;

namespace AgateLib.InternalResources
{
	internal static class Data
	{
		static TgzFileProvider mProvider = new TgzFileProvider("images", DataResources.images);
		static ZipFileProvider mFontProvider = new ZipFileProvider("Fonts", DataResources.Fonts);
		static AgateResourceCollection mFontResources;

		static Surface mPoweredBy;
		static Dictionary<int, FontSurface> mGentium = new Dictionary<int, FontSurface>();
		static Dictionary<int, FontSurface> mAndika = new Dictionary<int, FontSurface>();

		static Data()
		{
			Display.DisposeDisplay += new Display.DisposeDisplayHandler(Display_DisposeDisplay);
		}

		static void Display_DisposeDisplay()
		{
			mPoweredBy.Dispose();
			mPoweredBy = null;

			foreach (var font in mAndika)
				font.Value.Dispose();

			foreach (var font in mGentium)
				font.Value.Dispose();

			mAndika.Clear();
			mGentium.Clear();
		}

		private static void LoadFonts()
		{
			mFontResources = new AgateResourceCollection(mFontProvider);
		}

		internal static Surface PoweredBy
		{
			get
			{
				if (mPoweredBy != null && mPoweredBy.IsDisposed == false)
					return mPoweredBy;

				mPoweredBy = new Surface(mProvider, "agate-powered.png");

				return mPoweredBy;
			}
		}

		private static FontSurface GetFont(Dictionary<int, FontSurface> dictionary, int size, string resourceName)
		{
			LoadFonts();

			if (dictionary.ContainsKey(size) == false)
			{
				FontSurface font = new FontSurface(mFontResources, resourceName);
				dictionary[size] = font;
			}

			return dictionary[size];
		}

		internal static FontSurface Gentium10
		{
			get { return GetFont(mGentium, 10, "Gentium-10"); }
		}

		internal static FontSurface Gentium12
		{
			get { return GetFont(mGentium, 12, "Gentium-12"); }
		}
		internal static FontSurface Gentium14
		{
			get { return GetFont(mGentium, 14, "Gentium-14"); }
		}

		internal static FontSurface Andika09
		{
			get { return GetFont(mGentium, 9, "Andika-09"); }
		}
		internal static FontSurface Andika10
		{
			get { return GetFont(mGentium, 10, "Andika-10"); }
		}
		internal static FontSurface Andika12
		{
			get { return GetFont(mGentium, 12, "Andika-12"); }
		}
		internal static FontSurface Andika14
		{
			get { return GetFont(mGentium, 14, "Andika-14"); }
		}
	}
}