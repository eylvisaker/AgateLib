using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DirectDraw = Microsoft.DirectX.DirectDraw;

namespace EYGL
{

    class DDraw_Display : Display 
    {
        private DirectDraw.Device mDevice;
        private System.Windows.Forms.Control mParent;
        private DirectDraw.Clipper mClipper;

        private DDraw_Surface mPrimarySurface;
        private DDraw_Surface mBackBuffer;

        private string mPath = ".";
        String Display.imagePath
        {
            get { return mPath; }
            set { mPath = value; }
        }

        public DirectDraw.Device DirectDrawDevice
        {
            get
            {
                return mDevice;
            }
        }

        public DDraw_Surface screen
        {
            get
            {
                return mPrimarySurface;
            }
        }
        public DDraw_Surface backBuffer
        {
            get
            {
                return mBackBuffer;
            }
        }

        bool Display.initialize(DisplayInitInfo displayInfo)
        {
            // Create the direct draw device
            DirectDraw.CreateFlags createFlags = DirectDraw.CreateFlags.Default;

            switch (displayInfo.displayType)
            {
                case DisplayTypes.Default:
                    createFlags = Microsoft.DirectX.DirectDraw.CreateFlags.Default;
                    break;

                case DisplayTypes.HardwareIfAvailable:
                    createFlags = Microsoft.DirectX.DirectDraw.CreateFlags.HardwareOnly;
                    break;

                case DisplayTypes.SoftwareAlways:
                    createFlags = Microsoft.DirectX.DirectDraw.CreateFlags.EmulationOnly;
                    break;

            }
            try
            {
                mDevice = new DirectDraw.Device(createFlags);
            }
            catch (Exception e)
            {
                displayInfo.errorString = e.Message;

                return false;
            }

            // resize target control
            mParent = displayInfo.parent;

            if (displayInfo.autoResize)
            {
                mParent.Size = new Size(displayInfo.width, displayInfo.height);
            }
            else if (!displayInfo.fullScreen)
            {
                displayInfo.width = mParent.Width;
                displayInfo.height = mParent.Height;
            }

            // set cooperative level
            DirectDraw.CooperativeLevelFlags cooperativeFlags 
                = Microsoft.DirectX.DirectDraw.CooperativeLevelFlags.Normal;

            if (displayInfo.fullScreen)
                cooperativeFlags = Microsoft.DirectX.DirectDraw.CooperativeLevelFlags.FullscreenExclusive;

            try
            {
                mDevice.SetCooperativeLevel(displayInfo.parent, cooperativeFlags);

                if (displayInfo.fullScreen)
                {
                    mDevice.SetDisplayMode(displayInfo.width, displayInfo.height,
                        displayInfo.bpp, displayInfo.refreshRate, displayInfo.VGAmode);
                }
            }
            catch (Exception e)
            {
                displayInfo.errorString = e.Message;

                return false;
            }

            // create primary surface
            DirectDraw.Surface surf;
            DirectDraw.SurfaceDescription desc = new Microsoft.DirectX.DirectDraw.SurfaceDescription();

            desc.Clear();
            desc.SurfaceCaps.Clear();
            desc.SurfaceCaps.PrimarySurface = true;

            surf = new Microsoft.DirectX.DirectDraw.Surface(desc, mDevice);
            mPrimarySurface = new DDraw_Surface(this, surf);

            // create backbuffer
            desc = new Microsoft.DirectX.DirectDraw.SurfaceDescription();

            desc.Clear();
            desc.SurfaceCaps.Clear();

            desc.Width = displayInfo.width;
            desc.Height = displayInfo.height;
            desc.SurfaceCaps.OffScreenPlain = true;

            surf = new Microsoft.DirectX.DirectDraw.Surface(desc, mDevice);
            mBackBuffer = new DDraw_Surface(this, surf);

            // create clipper
            mClipper = new Microsoft.DirectX.DirectDraw.Clipper(mDevice);

            return true;
        }
        void Display.dispose()
        {
            mDevice.Dispose();
        }

        void Display.beginScene() { }
        void Display.endScene()
        {
            ((Display)this).endScene(true);
        }
        void Display.endScene(bool waitVSync)
        {
            if (waitVSync)
                mDevice.WaitForVerticalBlank(Microsoft.DirectX.DirectDraw.WaitVbFlags.BlockBegin);

            mPrimarySurface.drawFast(mBackBuffer, mParent.PointToScreen(new Point(0, 0)));
        }

        void Display.clear()
        {
            mBackBuffer.clear();
        }
        void Display.clear(Color color)
        {
            mBackBuffer.clear(color);
        }
        void Display.clear(Color color, Rectangle rect)
        {
            mBackBuffer.clear(color, rect);
        }

        Surface Display.createSurface(String fileName)
        {
            if (mDevice == null) return null;

            DirectDraw.Surface surf;
            DirectDraw.SurfaceDescription desc = new Microsoft.DirectX.DirectDraw.SurfaceDescription();
            Image image = Image.FromFile(System.IO.Path.Combine(mPath, fileName));

            if (image == null) return null;

            desc.Clear();
            surf = new DirectDraw.Surface((Bitmap)image, desc, mDevice);

            return new DDraw_Surface(this, surf);


        }
        FontSurface Display.createFont(System.Drawing.Font font)
        {
            return null;
        }

    }

    class DDraw_Surface : Surface
    {
        DDraw_Display mOwner;
        DirectDraw.Device mDevice;
        DirectDraw.Surface mSurface;

        private DirectDraw.DrawEffects mDrawEffects = new Microsoft.DirectX.DirectDraw.DrawEffects();

        public DDraw_Surface(DDraw_Display owner)
        {
            mOwner = owner;
            mDevice = mOwner.DirectDrawDevice;
            mSurface = null;
        }
        public DDraw_Surface(DDraw_Display owner, DirectDraw.Surface surface)
        {
            mOwner = owner;
            mDevice = mOwner.DirectDrawDevice;
            mSurface = surface;
        }

        protected DirectDraw.Surface getSurface()
        {
            return mSurface;
        }
        protected Rectangle getDestRect(int dest_x, int dest_y)
        {
            Point origin = Origin.Calc(mAlignment, displaySize);

            return new Rectangle
                (dest_x - origin.X, dest_y - origin.Y, displayWidth, displayHeight);

        }
        protected Rectangle getSrcRect()
        {
            return new Rectangle(0, 0, surfaceWidth, surfaceHeight);
        }

        protected DirectDraw.DrawFlags getFlags(DDraw_Surface dest)
        {
            DirectDraw.DrawFlags flags = Microsoft.DirectX.DirectDraw.DrawFlags.DoNotWait;

            if (alpha < 1.0)            flags |= Microsoft.DirectX.DirectDraw.DrawFlags.AlphaSource;
            if (dest.alpha < 1.0)       flags |= Microsoft.DirectX.DirectDraw.DrawFlags.AlphaDestination;
            //if (src.rotationAngle != 0.0)   flags |= Microsoft.DirectX.DirectDraw.DrawFlags.RotationAngle;

            //mDrawEffects.RotationAngle = (int) src.rotationAngleDegrees;
            mDrawEffects.AlphaSourceConstant = (int) (alpha * 255);
            

            return flags;
        }

        public void drawFast(DDraw_Surface iSrc, Point dest)
        {
            mSurface.DrawFast(dest.X, dest.Y, iSrc.mSurface, Microsoft.DirectX.DirectDraw.DrawFastFlags.Wait);
        }
        public void drawFast(DDraw_Surface iSrc, int dest_x, int dest_y)
        {
            mSurface.DrawFast(dest_x, dest_y, iSrc.mSurface, Microsoft.DirectX.DirectDraw.DrawFastFlags.Wait);
        }

        public override void draw(Rectangle dest_rect) 
        {
            DDraw_Surface dest = mOwner.backBuffer;
            DirectDraw.DrawFlags flags = getFlags(dest);

            Rectangle src_rect = getSrcRect();
            dest.clipRects(ref dest_rect, ref src_rect);

            dest.mSurface.Draw(dest_rect, mSurface, src_rect, flags, mDrawEffects);
        }
        public override void draw(Rectangle src_rect, Rectangle dest_rect)
        {
            DDraw_Surface dest = mOwner.backBuffer;
            DirectDraw.DrawFlags flags = getFlags(dest);

            dest.clipRects(ref dest_rect, ref src_rect);

            dest.mSurface.Draw(dest_rect, mSurface, src_rect, flags, mDrawEffects);
            
        }
        public override void draw(int dest_x, int dest_y) 
        {
            DDraw_Surface dest = mOwner.backBuffer;
            DirectDraw.DrawFlags flags = getFlags(dest);

            Rectangle dest_rect = getDestRect(dest_x, dest_y);
            Rectangle src_rect = getSrcRect();
            dest.clipRects(ref dest_rect, ref src_rect);

            dest.mSurface.Draw(dest_rect, mSurface, src_rect, flags, mDrawEffects);
        }
        public override void draw(Point dest_pt) 
        {
            draw(dest_pt.X, dest_pt.Y);
        }
        public override void draw()
        {
            DDraw_Surface dest = mOwner.backBuffer;
            DirectDraw.DrawFlags flags = getFlags(dest);

            Rectangle dest_rect = getDestRect(0, 0);
            Rectangle src_rect = getSrcRect();
            dest.clipRects(ref dest_rect, ref src_rect);

            dest.mSurface.Draw(dest_rect, mSurface, src_rect, flags, mDrawEffects);
        }

        public override void drawRects(Rectangle[] src_rects, Rectangle[] dest_rects)
        {
            DDraw_Surface dest = mOwner.backBuffer;
            DirectDraw.DrawFlags flags = getFlags(dest);

            if (src_rects.Length != dest_rects.Length)
            {
                return;
            }

            for (int i = 0; i < src_rects.Length; i++)
            {
                dest.clipRects(ref dest_rects[i], ref src_rects[i]);

                dest.mSurface.Draw(dest_rects[i], mSurface, src_rects[i], flags, mDrawEffects);
            }
        }

        protected bool clipRects(ref Rectangle dest_rect, ref Rectangle src_rect)
        {
            double ScaleX = (double)dest_rect.Width / (double)src_rect.Width;
            double ScaleY = (double)dest_rect.Height / (double)src_rect.Height;

            if (dest_rect.Left < 0)
            {
                int pixels = -dest_rect.Left;

                dest_rect.X += pixels;
                dest_rect.Width -= pixels;

                src_rect.X += (int)(pixels / ScaleX);
                src_rect.Width -= (int)(pixels / ScaleX);
            }
            if (dest_rect.Top < 0)
            {
                int pixels = -dest_rect.Top;

                dest_rect.Y += pixels;
                dest_rect.Height -= pixels;

                src_rect.Y += (int)(pixels / ScaleY);
                src_rect.Height -= (int)(pixels / ScaleY);
            }
            if (dest_rect.Right > surfaceWidth)
            {
                int pixels = dest_rect.Right - surfaceWidth;

                dest_rect.Width -= pixels;
                src_rect.Width -= (int)(pixels / ScaleX);
            }
            if (dest_rect.Bottom > surfaceHeight)
            {
                int pixels = dest_rect.Bottom - surfaceHeight;

                dest_rect.Height -= pixels;
                src_rect.Height -= (int)(pixels / ScaleY);
            }

            if (dest_rect.Width > 0 && dest_rect.Height > 0)
                return true;
            else
                return false;

        }

        public override int surfaceWidth
        {
            get { return mSurface.SurfaceDescription.Width;  }
        }
        public override int surfaceHeight 
        {
            get { return mSurface.SurfaceDescription.Height; }
        }

        public override double alpha
        {
            get { return mAlpha; }
            set
            {
                if (value < 0) value = 0;
                if (value > 1.0) value = 1.0;

                mAlpha = value;
            }
        }
        public override double rotationAngle
        {
            get { return mRotation; }
            set { mRotation = value; }
        }


        public override void clear(Color color)
        {
            mSurface.ColorFill(color);
        }
        public override void clear(Color color, Rectangle dest)
        {
            mSurface.ColorFill(dest, color);
        }



    }
}
