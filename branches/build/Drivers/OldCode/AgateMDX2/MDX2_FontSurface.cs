using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Direct3D = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace ERY.GameLibrary
{
    public class MDX2_FontSurface : FontSurfaceImpl
    {
        #region --- Private Variables ---

        MDX2_Display mDisplay;
        FontSurface mOwner;

        protected Direct3D.Font mD3DFont;
        protected System.Drawing.Font mWinFont;

        protected Direct3D.Sprite mSprite;

        #endregion

        #region --- Construction / Destruction ---

        public MDX2_FontSurface(FontSurface owner)
        {
            mOwner = owner;
        }
        public MDX2_FontSurface(FontSurface owner, System.Drawing.Font winfont)
        {
            mDisplay = Display.Impl as MDX2_Display;
            mOwner = owner;
            mWinFont = winfont;
            mD3DFont = new Direct3D.Font(mDisplay.D3D_Device, winfont);

            mColor = Color.FromArgb(255, Color.Black);
        }
        public override void Dispose()
        {
            mD3DFont.Dispose();
            mSprite.Dispose();
        }

        #endregion

        public override int StringDisplayHeight(string text)
        {
            return StringDisplaySize(text).Height;
        }
        public override int StringDisplayWidth(string text)
        {
            return StringDisplaySize(text).Width;
        }
        public override Size StringDisplaySize(string text)
        {
            Rectangle result = mD3DFont.MeasureString(null, text, DrawStringFormat.SingleLine, mOwner.Color);

            double scalex, scaley;

            mOwner.GetScale(out scalex, out scaley);

            result.Height = (int)(scalex * result.Height);
            result.Width = (int)(scaley * result.Width);

            return result.Size;
        }


        public override void DrawText(Point dest_pt, string text)
        {
            if (mSprite == null)
            {
                mSprite = new Microsoft.DirectX.Direct3D.Sprite(mD3DFont.Device);
            }

            Point dest = Origin.Calc(mOwner.DisplayAlignment, StringDisplaySize(text));


            dest_pt.X -= dest.X;
            dest_pt.Y -= dest.Y;

            double scalex, scaley;
            mOwner.GetScale(out scalex, out scaley);

            mSprite.Begin(SpriteFlags.AlphaBlend);
            mSprite.Transform = Matrix.Scaling((float)scalex, (float)scaley, 1.0f)
                * Matrix.Translation(dest_pt.X, dest_pt.Y, 0);

            mD3DFont.DrawString(mSprite, text, new Point(0, 0), mOwner.Color);

            mSprite.End();
        }
        public override void DrawText(PointF dest_pt, string text)
        {
            DrawText(new Point((int)dest_pt.X, (int)dest_pt.Y), text);

        }
        public override void DrawText(int dest_x, int dest_y, string text)
        {
            DrawText(new Point(dest_x, dest_y), text);
        }
        public override void DrawText(double dest_x, double dest_y, string text)
        {
            DrawText(new Point((int)dest_x, (int)dest_y), text);
        }
        public override void DrawText(string text)
        {
            DrawText(0, 0, text);
        }


        public override void DrawText(int dest_x, int dest_y, string text, CanvasImpl c)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DrawText(Point dest_pt, string text, CanvasImpl c)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
