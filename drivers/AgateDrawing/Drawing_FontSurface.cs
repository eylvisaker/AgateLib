using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib.SystemDrawing
{
    class Drawing_FontSurface : FontSurfaceImpl
    {
        Font mFont;

        public Drawing_FontSurface(  string fontFamily, float sizeInPoints)
        {
            mFont = new Font(fontFamily, sizeInPoints);
        }
        public override void Dispose()
        {
            mFont = null;
        }

        public override void DrawText(Point dest_pt, string text)
        {
            DrawText(new PointF((float)dest_pt.X, (float)dest_pt.Y), text);

        }
        public override void DrawText(PointF dest_pt, string text)
        {
            PointF dest = Origin.CalcF(DisplayAlignment, StringDisplaySize(text));

            dest_pt.X -= dest.X;
            dest_pt.Y -= dest.Y;

            Drawing_Display disp = Display.Impl as Drawing_Display;
            Graphics g = disp.FrameGraphics;

            GraphicsState state = g.Save();
            double scalex, scaley;

            GetScale(out scalex, out scaley);

            g.TranslateTransform(dest_pt.X, dest_pt.Y);
            g.ScaleTransform((float)scalex, (float)scaley);

            g.DrawString(text, mFont, new SolidBrush((System.Drawing.Color)Color), System.Drawing.Point.Empty);

            g.Restore(state);

        }
        public override void DrawText(int dest_x, int dest_y, string text)
        {
            DrawText(new PointF((float)dest_x, (float)dest_y), text);
        }
        public override void DrawText(double dest_x, double dest_y, string text)
        {
            DrawText(new PointF((float)dest_x, (float)dest_y), text);
        }
        /*
        public override void DrawText(int dest_x, int dest_y, string text, CanvasImpl c)
        {
            Drawing_Canvas canvas = c as Drawing_Canvas;

            canvas.Graphics.DrawString(text, mFont, new SolidBrush(mOwner.Color), (float)dest_x, (float)dest_y);
        }

        public override void DrawText(Point dest_pt, string text, CanvasImpl c)
        {
            DrawText(dest_pt.X, dest_pt.Y, text, c);
        }
        */

        public override int StringDisplayWidth(string text)
        {
            return StringDisplaySize(text).Width;
        }
        public override int StringDisplayHeight(string text)
        {
            return StringDisplaySize(text).Height;
        }
        public override Size StringDisplaySize(string text)
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

            return new Size((int)result.Width, (int)result.Height);
        }

    }

}
