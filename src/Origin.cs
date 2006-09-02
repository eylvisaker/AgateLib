using System;
using System.Collections.Generic;

namespace ERY.AgateLib
{
    /// <summary>
    /// OriginAlignment enum.  Used to specify how
    /// points should be interpreted.
    /// </summary>
    public enum OriginAlignment
    {
        /// <summary>
        /// Point indicates top-left.
        /// </summary>
        TopLeft = 0x11,
        /// <summary>
        /// Point indicates top-center.
        /// </summary>
        TopCenter = 0x12,
        /// <summary>
        /// Point indicates top-right.
        /// </summary>
        TopRight = 0x13,

        /// <summary>
        /// Point indicates center-left.
        /// </summary>
        CenterLeft = 0x21,
        /// <summary>
        /// Point indicates center.
        /// </summary>
        Center = 0x22,
        /// <summary>
        /// Point indicates center-right.
        /// </summary>
        CenterRight = 0x23,

        /// <summary>
        /// Point indicates bottom-left.
        /// </summary>
        BottomLeft = 0x31,
        /// <summary>
        /// Point indicates bottom-center.
        /// </summary>
        BottomCenter = 0x32,
        /// <summary>
        /// Point indicates bottom-right.
        /// </summary>
        BottomRight = 0x33,
    }

    /// <summary>
    /// Static class which performs necessary calculates based on OriginAlignment
    /// values.
    /// </summary>
    public static class Origin
    {
        /// <summary>
        /// Returns a point which should be subtracted from the interpreted
        /// point to get the top-left position.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Point Calc(OriginAlignment origin, Size size)
        {
            Point retval = new Point();
            int tb = ((int)origin) & 0xf0;
            int lr = ((int)origin) & 0x0f;
            int width = Math.Abs(size.Width);
            int height = Math.Abs(size.Height);

            if (lr == 0x01)
                retval.X = 0;
            else if (lr == 0x02)
                retval.X = width / 2;
            else if (lr == 0x03)
            {
                retval.X = width;

            }

            if (tb == 0x10)
                retval.Y = 0;
            else if (tb == 0x20)
                retval.Y = height / 2;
            else if (tb == 0x30)
            {
                retval.Y = height;
            }

            return retval;
        }
        /// <summary>
        /// Returns a point which should be subtracted from the interpreted
        /// point to get the top-left position.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static PointF CalcF(OriginAlignment origin, Size size)
        {
            Point retval = Calc(origin, size);

            return new PointF(retval.X, retval.Y);
        }
        /// <summary>
        /// Returns a point which should be subtracted from the interpreted
        /// point to get the top-left position.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static PointF CalcF(OriginAlignment origin, SizeF size)
        {
            PointF retval = new PointF();
            int tb = ((int)origin) & 0xf0;
            int lr = ((int)origin) & 0x0f;

            if (lr == 0x01)
                retval.X = 0;
            else if (lr == 0x02)
                retval.X = size.Width / 2;
            else if (lr == 0x03)
                retval.X = size.Width;

            if (tb == 0x10)
                retval.Y = 0;
            else if (tb == 0x20)
                retval.Y = size.Height / 2;
            else if (tb == 0x30)
                retval.Y = size.Height;

            return retval;
        }
        /// <summary>
        /// Returns a point which should be subtracted from the interpreted
        /// point to get the top-left position.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="size"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Calc(OriginAlignment origin, Size size, out int x, out int y)
        {
            Point ret = Calc(origin, size);

            x = ret.X;
            y = ret.Y;

        }
        /// <summary>
        /// Modifies the rectangle by taking its Location and converting it
        /// using Calc so that the rectangle outlines the actual destination.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Rectangle CalcRect(OriginAlignment origin, Rectangle rect)
        {
            return CalcRect(origin, rect, rect.Size);
        }
        /// <summary>
        /// Modifies the rectangle by taking its Location and converting it
        /// using Calc so that the rectangle outlines the actual destination.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="rect"></param>
        /// <param name="effectiveSize"></param>
        /// <returns></returns>
        public static Rectangle CalcRect(OriginAlignment origin, Rectangle rect, Size effectiveSize)
        {
            Point pt = Origin.Calc(origin, effectiveSize);
            Rectangle retval = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);

            retval.X -= pt.X;
            retval.Y -= pt.Y;

            return retval;
        }
    };

}
