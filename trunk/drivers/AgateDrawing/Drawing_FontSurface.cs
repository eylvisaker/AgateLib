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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

using AgateLib.ImplBase;

namespace AgateLib.Display.SystemDrawing
{
    using AgateLib.Display;
    using WinForms;

    class Drawing_FontSurface : FontSurfaceImpl
    {
        Font mFont;

        public Drawing_FontSurface(string fontFamily, float sizeInPoints, FontStyle style)
        {
            System.Drawing.FontStyle drawingStyle = System.Drawing.FontStyle.Regular;

            if ((style & FontStyle.Bold) > 0)
                drawingStyle |= System.Drawing.FontStyle.Bold;
            if ((style & FontStyle.Italic) > 0)
                drawingStyle |= System.Drawing.FontStyle.Italic;
            if ((style & FontStyle.Strikeout) > 0)
                drawingStyle |= System.Drawing.FontStyle.Strikeout;
            if ((style & FontStyle.Underline) > 0)
                drawingStyle |= System.Drawing.FontStyle.Underline;

            mFont = new Font(fontFamily, sizeInPoints, drawingStyle);
        }
        public override void Dispose()
        {
            mFont = null;
        }

        public override void DrawText(Geometry.Point dest_pt, string text)
        {
            DrawText(new Geometry.PointF((float)dest_pt.X, (float)dest_pt.Y), text);

        }
        public override void DrawText(Geometry.PointF dest_pt, string text)
        {
            Geometry.PointF dest = Origin.CalcF(DisplayAlignment, StringDisplaySize(text));

            dest_pt.X -= dest.X;
            dest_pt.Y -= dest.Y;

            Drawing_Display disp = Display.Impl as Drawing_Display;
            Graphics g = disp.FrameGraphics;

            GraphicsState state = g.Save();
            double scalex, scaley;

            GetScale(out scalex, out scaley);

            g.TranslateTransform(dest_pt.X, dest_pt.Y);
            g.ScaleTransform((float)scalex, (float)scaley);

            g.DrawString(text, mFont, 
                new SolidBrush(Interop.Convert(Color)), Point.Empty);

            g.Restore(state);

        }
        public override void DrawText(int dest_x, int dest_y, string text)
        {
            DrawText(new Geometry.PointF((float)dest_x, (float)dest_y), text);
        }
        public override void DrawText(double dest_x, double dest_y, string text)
        {
            DrawText(new Geometry.PointF((float)dest_x, (float)dest_y), text);
        }

        public override int StringDisplayWidth(string text)
        {
            return StringDisplaySize(text).Width;
        }
        public override int StringDisplayHeight(string text)
        {
            return StringDisplaySize(text).Height;
        }
        public override Geometry.Size StringDisplaySize(string text)
        {
            Drawing_Display disp = Display.Impl as Drawing_Display;
            Graphics g = disp.FrameGraphics;
            bool disposeGraphics = false;

            if (g == null)
            {

                g = Graphics.FromImage((disp.RenderTarget.Impl as Drawing_IRenderTarget).BackBuffer);

                disposeGraphics = true;
            }

            SizeF result = new SizeF(g.MeasureString(text, mFont));
            double scalex, scaley;

            if (disposeGraphics)
                g.Dispose();

            GetScale(out scalex, out scaley);

            result.Height *= (float)scalex;
            result.Width *= (float)scaley;

            return new Geometry.Size((int)result.Width, (int)result.Height);
        }

    }

}
