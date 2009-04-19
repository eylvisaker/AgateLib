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
		static ZipFileProvider mProcionoProvider = new ZipFileProvider("Prociono", DataResources.Prociono);
		static AgateResourceCollection mFontResources;

		static Surface mPoweredBy;
		static FontSurface mProciono11, mProciono14;

		private static void LoadFonts()
		{
			mFontResources = new AgateResourceCollection(mProcionoProvider);
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

		internal static FontSurface Prociono11
		{
			get
			{
				LoadFonts();

				if (mProciono11 == null)
					mProciono11 = new FontSurface(mFontResources, "Prociono-11");

				return mProciono11;
			}
		}

		internal static FontSurface Prociono14
		{
			get
			{
				LoadFonts();

				if (mProciono14 == null)
					mProciono14 = new FontSurface(mFontResources, "Prociono-14");

				return mProciono14;
			}
		}

	}
}
