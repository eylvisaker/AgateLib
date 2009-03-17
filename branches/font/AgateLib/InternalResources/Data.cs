using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Utility;

namespace AgateLib.InternalResources
{
    public static class Data
    {
        static TgzFileProvider mProvider = new TgzFileProvider("images", DataResources.images);

        static Surface mPoweredBy;

        public static Surface PoweredBy
        {
            get
            {
                if (mPoweredBy != null)
                    return mPoweredBy;

                mPoweredBy = new Surface(mProvider, "agate-powered.png");

                return mPoweredBy;
            }
        }
    }
}
