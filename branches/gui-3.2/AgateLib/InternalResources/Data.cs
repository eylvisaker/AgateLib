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
		static FontSurface mGentium10,mGentium12,mGentium14;
		static FontSurface mAndika09, mAndika10, mAndika12, mAndika14;

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


		internal static FontSurface Gentium10
		{
			get
			{
				LoadFonts();

				if (mGentium10 == null)
					mGentium10 = new FontSurface(mFontResources, "Gentium-10");

				return mGentium10;
			}
		}
		internal static FontSurface Gentium12
		{
			get
			{
				LoadFonts();

				if (mGentium12 == null)
					mGentium12 = new FontSurface(mFontResources, "Gentium-12");

				return mGentium12;
			}
		}
		internal static FontSurface Gentium14
		{
			get
			{
				LoadFonts();

				if (mGentium14 == null)
					mGentium14 = new FontSurface(mFontResources, "Gentium-14");

				return mGentium14;
			}
		}

		internal static FontSurface Andika09
		{
			get
			{
				LoadFonts();

				if (mAndika09 == null)
					mAndika09 = new FontSurface(mFontResources, "Andika-09");

				return mAndika09;
			}
		}
		internal static FontSurface Andika10
		{
			get
			{
				LoadFonts();

				if (mAndika10 == null)
					mAndika10 = new FontSurface(mFontResources, "Andika-10");

				return mAndika10;
			}
		}
		internal static FontSurface Andika12
		{
			get
			{
				LoadFonts();

				if (mAndika12 == null)
					mAndika12 = new FontSurface(mFontResources, "Andika-12");

				return mAndika12;
			}
		}
		internal static FontSurface Andika14
		{
			get
			{
				LoadFonts();

				if (mAndika14 == null)
					mAndika14 =new FontSurface(mFontResources, "Andika-14");


				return mAndika14;
			}
		}
	}
}
