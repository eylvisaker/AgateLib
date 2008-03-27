using System;
using System.Collections.Generic;
using System.Text;
using Draw = System.Drawing;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.WinForms
{
    public static class Interop
    {
        public static Draw.Color Convert(Color clr)
        {
            return Draw.Color.FromArgb(clr.ToArgb());
        }
        public static Color Convert(Draw.Color clr)
        {
            return Color.FromArgb(clr.ToArgb());
        }

        public static Draw.Rectangle Convert(Rectangle rect)
        {
            return new Draw.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
        public static Rectangle Convert(Draw.Rectangle rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Draw.RectangleF Convert(RectangleF rect)
        {
            return new Draw.RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        }
        public static RectangleF Convert(Draw.RectangleF rect)
        {
            return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Point Convert(Draw.Point pt)
        {
            return new Point(pt.X, pt.Y);
        }
        public static Draw.Point Convert(Point pt)
        {
            return new Draw.Point(pt.X, pt.Y);
        }

        public static PointF Convert(Draw.PointF pt)
        {
            return new PointF(pt.X, pt.Y);
        }
        public static Draw.PointF Convert(PointF pt)
        {
            return new Draw.PointF(pt.X, pt.Y);
        }


        public static Size Convert(Draw.Size pt)
        {
            return new Size(pt.Width, pt.Height);
        }
        public static Draw.Size Convert(Size pt)
        {
            return new Draw.Size(pt.Width, pt.Height);
        }

        public static SizeF Convert(Draw.SizeF pt)
        {
            return new SizeF(pt.Width, pt.Height);
        }
        public static Draw.SizeF Convert(SizeF pt)
        {
            return new Draw.SizeF(pt.Width, pt.Height);
        }

    }
}
