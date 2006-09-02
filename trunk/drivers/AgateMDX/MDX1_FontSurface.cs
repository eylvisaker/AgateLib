using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Direct3D = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib.MDX
{
    public class MDX1_FontSurface : FontSurfaceImpl
    {
        #region --- Private Variables ---

        MDX1_Display mDisplay;

        private Direct3D.Font mD3DFont;
        private System.Drawing.Font mWinFont;

        private Direct3D.Sprite mSprite;

        #endregion

        #region --- Construction / Destruction ---

        public MDX1_FontSurface( )
        {
        }
        public MDX1_FontSurface( string fontFamily, float sizeInPoints)
        {
            mDisplay = Display.Impl as MDX1_Display;
            mWinFont = new System.Drawing.Font(fontFamily, sizeInPoints);

            CreateD3DFont();

            //Color = Color.FromArgb(255, Color.Black);

            mDisplay.DeviceLost += new EventHandler(D3D_Device_DeviceLost);
            mDisplay.DeviceReset += new EventHandler(D3D_Device_DeviceReset);
            mDisplay.DeviceAboutToReset += new EventHandler(D3D_Device_DeviceLost);
        }


        public override void Dispose()
        {
            if (mD3DFont != null)
            {
                mD3DFont.Dispose();
                mD3DFont = null;
            }
            if (mSprite != null)
            {
                mSprite.Dispose();
                mSprite = null;
            }
        }

        private void CreateD3DFont()
        {
            mD3DFont = new Direct3D.Font(mDisplay.D3D_Device.Device, mWinFont);
        }

        #endregion
        #region --- Event Handlers ---

        void D3D_Device_DeviceReset(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Print("{0} Creating D3DFont object...", DateTime.Now); 
            CreateD3DFont();
        }
        void D3D_Device_DeviceLost(object sender, EventArgs e)
        {
            if (mD3DFont != null)
            {
                System.Diagnostics.Debug.Print("{0} Disposing of D3DFont object...", DateTime.Now);
                mD3DFont.Dispose();
                mD3DFont = null;
            }
            if (mSprite != null)
            {
                mSprite.Dispose();
                mSprite = null;
            }
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
            Rectangle result = new Rectangle(mD3DFont.MeasureString(null, text, 
                DrawTextFormat.SingleLine, Color.ToArgb()));
            
            double scalex, scaley;

            GetScale(out scalex, out scaley);

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

            Point dest = Origin.Calc(DisplayAlignment, StringDisplaySize(text));


            dest_pt.X -= dest.X;
            dest_pt.Y -= dest.Y;

            double scalex, scaley;
            GetScale(out scalex, out scaley);

            mDisplay.D3D_Device.DrawBuffer.Flush();
            mDisplay.D3D_Device.SetFontRenderState();

            mSprite.Begin(SpriteFlags.AlphaBlend);
            mSprite.Transform = Matrix.Scaling((float)scalex, (float)scaley, 1.0f)
                * Matrix.Translation(dest_pt.X, dest_pt.Y, 0);

            mD3DFont.DrawText(mSprite, text, new System.Drawing.Point(0, 0), Color.ToArgb());

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
    }
}
