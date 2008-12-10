using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Direct3D = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace ERY.GameLibrary
{
    public class MDX2_Canvas : CanvasImpl
    {
        MDX2_Surface mOwner;
        Direct3D.Surface mSurface;
        Graphics mGraphics;

        public MDX2_Canvas(MDX2_Surface owner, Graphics graphics)
        {
            mOwner = owner;
            mGraphics = graphics;
        }

        public Graphics Graphics
        {
            get { return mGraphics; }
        }

        public override Size Size
        {
            get { return new Size(mSurface.Description.Width, mSurface.Description.Height); }
        }
        public override void Dispose()
        {
            if (mSurface != null)
            {
                mSurface.ReleaseGraphics();
            }
            else
                mGraphics.Dispose();

            mOwner.UnlockSurface(this);
        }

        public override void Clear(Color color)
        {
            mGraphics.Clear(color);
        }
        public override void Clear(Color color, Rectangle dest)
        {
            mGraphics.FillRectangle(new SolidBrush(color), dest);
        }
        public override void DrawEllipse(Rectangle target, Color color, float thickness)
        {
            mGraphics.DrawEllipse(new Pen(color, thickness), target);
        }
        public override void DrawRect(Rectangle target, Color color, float thickness)
        {
            mGraphics.DrawRectangle(new Pen(color, thickness), target);
        }
        public override void FillRect(Rectangle rect, Color color)
        {
            mGraphics.FillRectangle(new SolidBrush(color), rect);
        }
        public override void FillEllipse(Rectangle rect, Color color)
        {
            mGraphics.FillEllipse(new SolidBrush(color), rect);
        }

    };

}
