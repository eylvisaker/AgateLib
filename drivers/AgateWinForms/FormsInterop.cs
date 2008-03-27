//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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
