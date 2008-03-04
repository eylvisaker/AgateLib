using System;
using System.Collections.Generic;
using System.Text;
using Draw = System.Drawing;
using Geo = ERY.AgateLib.Geometry;

namespace ERY.AgateLib.WinForms
{
    public static class FormsInterop
    {
        public static Draw.Rectangle ToRectangle(Geo.Rectangle rect)
        {
            return new System.Drawing.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
