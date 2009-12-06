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
		static Dictionary<int, FontSurface> mSans = new Dictionary<int, FontSurface>();
		static Dictionary<int, FontSurface> mSerif = new Dictionary<int, FontSurface>();
		static Dictionary<int, FontSurface> mMono = new Dictionary<int, FontSurface>();

		static Data()
		{
			Display.DisposeDisplay += new Display.DisposeDisplayHandler(Display_DisposeDisplay);
		}

		static void Display_DisposeDisplay()
		{
			if (mPoweredBy != null)
			{
				mPoweredBy.Dispose();
				mPoweredBy = null;
			}

			foreach (var font in mSans)
				font.Value.Dispose();

			foreach (var font in mSerif)
				font.Value.Dispose();

			foreach (var font in mMono)
				font.Value.Dispose();

			mSans.Clear();
			mSerif.Clear();
			mMono.Clear();

		}

		private static void LoadFonts()
		{
			if (mFontResources == null)
			{
				mFontResources = new AgateResourceCollection(mFontProvider);
			}
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

		internal static FontSurface AgateSans10
		{
			get { return GetFont(mSans, 10, "AgateSans-10"); }
		}
		internal static FontSurface AgateSans14
		{
			get { return GetFont(mSans, 14, "AgateSans-14"); }
		}
		internal static FontSurface AgateSans24
		{
			get { return GetFont(mSans, 24, "AgateSans-24"); }
		}

		internal static FontSurface AgateSerif10
		{
			get { return GetFont(mSerif, 10, "AgateSerif-10"); }
		}
		internal static FontSurface AgateSerif14
		{
			get { return GetFont(mSerif, 14, "AgateSerif-14"); }
		}

		internal static FontSurface AgateMono10
		{
			get { return GetFont(mMono, 10, "AgateMono-10"); }
		}
		

	}
}