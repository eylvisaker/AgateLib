using System;
using System.Collections.Generic;
using System.Text;
using Draw = System.Drawing;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.WinForms
{
    public static class FormsInterop
    {
        public static Draw.Color ConvertColor(Color clr)
        {
            return Draw.Color.FromArgb(clr.ToArgb());
        }
        public static Color ConvertColor(Draw.Color clr)
        {
            return Color.FromArgb(clr.ToArgb());
        }

        public static Draw.Rectangle ConvertRectangle(Rectangle rect)
        {
            return new Draw.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
        public static Rectangle ConvertRectangle(Draw.Rectangle rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Draw.RectangleF ConvertRectangleF(RectangleF rect)
        {
            return new Draw.RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        }
        public static RectangleF ConvertRectangleF(Draw.RectangleF rect)
        {
            return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Point ConvertPoint(Draw.Point pt)
        {
            return new Point(pt.X, pt.Y);
        }
        public static Draw.Point ConvertPoint(Point pt)
        {
            return new Draw.Point(pt.X, pt.Y);
        }

        public static PointF ConvertPointF(Draw.PointF pt)
        {
            return new PointF(pt.X, pt.Y);
        }
        public static Draw.PointF ConvertPointF(PointF pt)
        {
            return new Draw.PointF(pt.X, pt.Y);
        }


        public static Size ConvertSize(Draw.Size pt)
        {
            return new Size(pt.Width, pt.Height);
        }
        public static Draw.Size ConvertSize(Size pt)
        {
            return new Draw.Size(pt.Width, pt.Height);
        }

        public static SizeF ConvertSizeF(Draw.SizeF pt)
        {
            return new SizeF(pt.Width, pt.Height);
        }
        public static Draw.SizeF ConvertSizeF(SizeF pt)
        {
            return new Draw.SizeF(pt.Width, pt.Height);
        }

    }
}
