using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib
{
    /// <summary>
    /// Class which contains information about an available screen mode.
    /// </summary>
    public sealed class ScreenMode
    {
        private int mWidth;
        private int mHeight;
        private int mBpp;

        /// <summary>
        /// Constructs a ScreenMode object.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="bpp"></param>
        public ScreenMode(int width, int height, int bpp)
        {
            mWidth = width;
            mHeight = height;
            mBpp = bpp;
        }

        /// <summary>
        /// Width of the screen mode in pixels.
        /// </summary>
        public int Width
        {
            get { return mWidth; }
        }
        /// <summary>
        /// Height of the screen mode in pixels.
        /// </summary>
        public int Height
        {
            get { return mHeight; }
        }
        /// <summary>
        /// Bits per pixel of the screen mode.
        /// </summary>
        public int Bpp
        {
            get { return mBpp; }
        }

        /// <summary>
        /// Searches through the available screen resolutions and selects the one
        /// which is the closest match for the parameters passed in.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="bpp"></param>
        /// <returns>null if no screen mode could be found, otherwise the appropriate
        /// ScreenMode structure.</returns>
        public static ScreenMode SelectBestMode(int width, int height, int bpp)
        {
            ScreenMode[] modes = Display.EnumScreenModes();

            ScreenMode selected = null;
            int diff = 0;

            if (modes == null || modes.Length == 0)
                return null;

            foreach (ScreenMode mode in modes)
            {
                if (mode.Width < width)
                    continue;

                if (mode.Height < height)
                    continue;

                int thisDiff = Math.Abs(width - mode.Width) + Math.Abs(height - mode.Height);
                thisDiff *= Math.Abs(mode.Bpp - bpp) / 2;


                // first mode by default, or any mode which is a better match.
                if (selected == null || thisDiff < diff)
                {
                    selected = mode;
                    diff = thisDiff;
                }

            }

            return selected;
        }
    }
}
