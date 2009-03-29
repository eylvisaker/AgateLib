using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Utility;

namespace AgateLib.InternalResources
{
    internal static class Data
    {
        static TgzFileProvider mProvider = new TgzFileProvider("images", DataResources.images);

        static Surface mPoweredBy;

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
    }
}
