using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SDL = SdlDotNet;

namespace ERY.GameLibrary
{
    class SDL_Display : DisplayImpl
    {
        System.Windows.Forms.Control mParent;
        string mImagePath;
        SDL.Surface mBackBuffer;

        #region --- Construction / Destruction ---

        public override bool Initialize(DisplayInitInfo displayType)
        {
            SDL.Video.Initialize();

            mParent = displayType.Parent;

            if (displayType.FullScreen)
            {
                SDL.Video.SetVideoMode(displayType.Width, displayType.Height, displayType.BPP);

            }
            else
            {
                displayType.Width = mParent.ClientRectangle.Width;
                displayType.Height = mParent.ClientRectangle.Height;
            }
            
            mBackBuffer = new SDL.Surface(displayType.Width, displayType.Height);

            return true;
        }
        public override void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetRenderTarget(System.Windows.Forms.Control control)
        {
            mParent = control;
        }

        #endregion
        #region --- Overriden Public Properties ---

        public override string ImagePath
        {
            get
            {
                return mImagePath;
            }
            set
            {
                mImagePath = value;
            }
        }

        public override System.Drawing.Size RenderTargetSize
        {
            get { return mParent.ClientRectangle.Size; }
        }

        #endregion

        #region --- Creation of Surfaces ---

        public override SurfaceImpl CreateSurface(string fileName)
        {
            return new SDL_Surface(this, fileName);
        }
        public override SurfaceImpl CreateSurface(System.Drawing.Size surfaceSize)
        {
            return new SDL_Surface(this, surfaceSize);
        }
        public override FontSurfaceImpl CreateFont(System.Drawing.Font font)
        {
            return new SDL_FontSurface(this, font);
        }

        #endregion
        #region --- Modification of Backbuffer data ---

        public override void Clear(System.Drawing.Color color)
        {
            mBackBuffer.Fill(color);
        }
        public override void Clear(System.Drawing.Color color, System.Drawing.Rectangle dest)
        {
            mBackBuffer.Fill(dest, color);
        }

        public override CanvasImpl LockSurface()
        {
            return new SDL_Canvas(mBackBuffer);
        }

        #endregion

        #region --- Begin / End Scene and DeltaTime stuff ---

        private double mDeltaTime;
        private DateTime mLastTime;
        private bool mRanOnce = false;

        private DateTime mFPSStart;
        private int mFrames;
        private double mFPS = 0;


        public override void BeginScene()
        {
            DateTime now = System.DateTime.Now;

            if (mRanOnce)
            {
                TimeSpan delta = now - mLastTime;
                mDeltaTime = delta.TotalMilliseconds;
                mLastTime = now;

                TimeSpan framesTime = now - mFPSStart;

                if (framesTime.TotalMilliseconds > 100)
                {
                    double time = framesTime.TotalSeconds;

                    // average current framerate with that of the last update
                    mFPS = 0.5 * (mFrames / time + mFPS);

                    mFPSStart = now;
                    mFrames = 0;

                }
            }
            else
            {
                mDeltaTime = 0;
                mLastTime = now;

                mFPSStart = now;
                mFrames = 0;

                mRanOnce = true;
            }
          
        }
        public override void EndScene()
        {
            EndScene(true);
        }
        public override void EndScene(bool waitVSync)
        {
            Graphics gr = mParent.CreateGraphics();

            gr.DrawImageUnscaled(mBackBuffer.Bitmap, new Point(0, 0));
            gr.Dispose();

            mFrames++;
        }


        public override double DeltaTime
        {
            get
            {
                return mDeltaTime;
            }
        }
        public override void SetDeltaTime(double deltaTime)
        {
            mDeltaTime = deltaTime;
        }
        public override double FramesPerSecond
        {
            get { return mFPS; }
        }

        #endregion
        #region --- ClipRect stuff ---

        public override void SetClipRect(Rectangle newClipRect)
        {
            if (newClipRect.X < 0)
            {
                newClipRect.Width += newClipRect.X;
                newClipRect.X = 0;
            }
            if (newClipRect.Y < 0)
            {
                newClipRect.Height += newClipRect.Y;
                newClipRect.Y = 0;
            }
            if (newClipRect.Right > mParent.ClientRectangle.Right)
            {
                newClipRect.Width -= newClipRect.Right - mParent.ClientRectangle.Right;
            }
            if (newClipRect.Bottom > mParent.ClientRectangle.Bottom)
            {
                newClipRect.Height -= newClipRect.Bottom - mParent.ClientRectangle.Bottom;
            }

            mCurrentClipRect = newClipRect;
            mBackBuffer.ClipRectangle = mCurrentClipRect;

        }
        public override void PushClipRect(Rectangle newClipRect)
        {
            mClipRects.Push(mCurrentClipRect);
            SetClipRect(newClipRect);
        }
        public override void PopClipRect()
        {
            if (mClipRects.Count == 0)
            {
#if DEBUG
                throw new Exception("You have popped the cliprect too many times.");
#endif
            }
            else
            {
                SetClipRect(mClipRects.Pop());
            }
        }

        private Stack<Rectangle> mClipRects = new Stack<Rectangle>();
        private Rectangle mCurrentClipRect;

        #endregion

        #region --- Events ---

        public override event EventHandler DeviceLost;
        public override event EventHandler DeviceReset;
        public override event EventHandler Resize;

        private void DummyMethod()
        {
            DeviceLost(this, EventArgs.Empty);
            DeviceReset(this, EventArgs.Empty);
            Resize(this, EventArgs.Empty);
        }
        #endregion --- Events ---

        #region --- SDL Specific functions and properties ---

        public SDL.Surface BackBuffer
        {
            get { return mBackBuffer; }
        }
        /// <summary>
        /// Clips the rectangles passed. 
        /// Returns true if the rectangles still specify a valid region of space
        /// for doing blitting, ie. Width > 0 and Height > 0.
        /// </summary>
        /// <param name="srcRect">The source Rectangle to modify</param>
        /// <param name="destRect">The dest rectangle to modify</param>
        /// <returns>True if blitting should occur with the new rectangles.</returns>
        public bool Clip(ref Rectangle srcRect, ref Rectangle destRect)
        {
           // mBackBuffer.ClipRectangle

            if (destRect.Width > 0 && destRect.Height > 0)
                return true;
            else
                return false;

        }
        #endregion
    }

    class SDL_Surface : SurfaceImpl
    {
        SDL_Display mOwner;
        SDL.Surface mSurface;
        SDL.Surface mBackBuffer;

        public SDL_Surface(SDL_Display owner, string fileName)
        {
            mOwner = owner;
            mBackBuffer = mOwner.BackBuffer;

            mSurface = new SdlDotNet.Surface(fileName);
            
            mSurface.AlphaBlending = true;
            mSurface.Alpha = 255;

            
            //BlankTransparentPixels();
        }

        public SDL_Surface(SDL_Display owner, Size surfaceSize)
        {
            mOwner = owner;
            mBackBuffer = mOwner.BackBuffer;

            mSurface = new SdlDotNet.Surface(surfaceSize);
            mSurface = mSurface.Convert(owner.BackBuffer);

            //mSurface.TransparentColor = Color.FromArgb(255, 255, 0, 255);
            
            mSurface.AlphaBlending = true;

        }

        public SDL.Surface SDLSurface
        {
            get { return mSurface; }
        }

        // HACK:
        public void MakeCompatibleWidth(SDL_Surface surf)
        {
            mSurface = mSurface.Convert(surf.SDLSurface);
            mSurface.AlphaBlending = true;
        }

        public override void Draw(System.Drawing.Rectangle dest_rect)
        {
            mBackBuffer.Blit(mSurface, dest_rect, new Rectangle(new Point(0, 0), SurfaceSize));
        }
        public override void Draw(System.Drawing.Rectangle src_rect, System.Drawing.Rectangle dest_rect)
        {
            mBackBuffer.Blit(mSurface, dest_rect, src_rect);
        }
        public override void Draw(int dest_x, int dest_y)
        {
            Draw(new Point(dest_x, dest_y));
        }
        public override void Draw(System.Drawing.Point dest_pt)
        {
            Rectangle destRect = new Rectangle(dest_pt, DisplaySize);

            Draw(destRect);
        }
        public override void Draw(System.Drawing.Rectangle dest_rect, CanvasImpl c)
        {
            
        }
        public override void Draw(System.Drawing.Rectangle src_rect, System.Drawing.Rectangle dest_rect, CanvasImpl c)
        {
            SDL_Canvas canvas = c as SDL_Canvas;

            canvas.Surface.Blit(mSurface, dest_rect, src_rect);
        }
        public override void Draw(int dest_x, int dest_y, CanvasImpl c)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override void Draw(System.Drawing.Point dest_pt, CanvasImpl c)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DrawRects(System.Drawing.Rectangle[] src_rects, System.Drawing.Rectangle[] dest_rects, CanvasImpl c)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override void DrawRects(System.Drawing.Rectangle[] src_rects, System.Drawing.Rectangle[] dest_rects)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int SurfaceWidth
        {
            get { return mSurface.Size.Width; }
        }
        public override int SurfaceHeight
        {
            get { return mSurface.Size.Height; }
        }
        public override Size SurfaceSize
        {
            get { return mSurface.Size; }
        }

        public override CanvasImpl LockSurface()
        {
            return new SDL_Canvas(mSurface);
        }

        public override void Dispose()
        {
            if (mSurface != null)
                mSurface.Dispose();

            mOwner = null;
            mSurface = null;
            mBackBuffer = null;
        }

        public override DisplayImpl Display
        {
            get { return mOwner;  }
        }

        public override bool IsSurfaceBlank()
        {
            return IsSurfaceBlank((int)(mOwner.AlphaThreshold * 255.0));
        }
        public override bool IsSurfaceBlank(int alphaThreshold)
        {
            mSurface.Lock();

            unsafe
            {
                uint* ptr = (uint*)mSurface.Pixels;

                int pixels = mSurface.Width * mSurface.Height;

                for (int i = 0; i < pixels; i++)
                {
                    uint pixel = ptr[i];
                    byte alpha = (byte)(pixel & mSurface.AlphaMask >> mSurface.AlphaShift);

                    if (alpha > alphaThreshold)
                    {
                        mSurface.Unlock();

                        return false;
                    }
                }
            }

            mSurface.Unlock();

            return true;
        }
        /*
        private void BlankTransparentPixels()
        {
            mSurface.Lock();

            uint transparentColor = (uint) mSurface.TransparentColor.ToArgb();

            unsafe
            {
                uint* ptr = (uint*)mSurface.Pixels;

                int pixels = mSurface.Width * mSurface.Height;

                for (int i = 0; i < pixels; i++)
                {
                    uint pixel = ptr[i];
                    byte alpha = (byte)(pixel >> 24);

                    if (alpha < mOwner.AlphaThreshold)
                    {
                        ptr[i] = transparentColor;
                    }
                }
            }

            mSurface.Unlock();

        }*/
        public override bool IsRowBlank(int row)
        {
           if (row < 0 || row >= mSurface.Height)
                throw new ArgumentOutOfRangeException("row", row, "Row must be between 0 and Height!");
            
            mSurface.Lock();

            
            int alphaThreshold = (int)(mOwner.AlphaThreshold * 255 + 0.5);

            bool retval = IsRowBlankScanARGB(mSurface.Pixels, row, mSurface.Width, 
                mSurface.Pitch, alphaThreshold, (uint)mSurface.AlphaMask, mSurface.AlphaShift);

            mSurface.Unlock();

            return retval;
        }

        

        public override bool IsColumnBlank(int col)
        {
           
            if (col < 0 || col >= mSurface.Width)
                throw new ArgumentOutOfRangeException("col", col, "col must be between 0 and Width!");

            mSurface.Lock();

            int alphaThreshold = (int)(mOwner.AlphaThreshold * 255 + 0.5);

            bool retval = false;

            retval = IsColBlankScanARGB(mSurface.Pixels, col, mSurface.Height,
                mSurface.Pitch, alphaThreshold, (uint)mSurface.AlphaMask, mSurface.AlphaShift);

            mSurface.Unlock();

            return retval;
        }


        public override void SaveTo(string filename)
        {
            string ext = System.IO.Path.GetExtension(filename);
            string tempfile = System.IO.Path.GetTempFileName() + ".bmp";
            mSurface.SaveBmp(tempfile);

            Bitmap bmp = (Bitmap)Bitmap.FromFile(tempfile);

            System.Drawing.Imaging.ImageFormat format = null;

            switch (ext)
            {
                case ".png":
                    format = System.Drawing.Imaging.ImageFormat.Png;
                    break;

                case ".bmp":
                    format = System.Drawing.Imaging.ImageFormat.Bmp;
                    break;

                case ".jpg":
                case ".jpeg":
                    format = System.Drawing.Imaging.ImageFormat.Jpeg;
                    break;

            }

            bmp.Save(filename, format);

            bmp.Dispose();
            System.IO.File.Delete(tempfile);

        }
    }
    class SDL_FontSurface : FontSurfaceImpl
    {
        SDL_Display mOwner;
        SDL.Font mFont;
        SDL.Surface mBackBuffer;
        SDL.Sprites.TextSprite mSprite;

        public SDL_FontSurface(SDL_Display owner, System.Drawing.Font font)
        {
            // TODO: Fix this somehow.
            mFont = new SdlDotNet.Font(@"C:\Windows\Fonts\micross.ttf", (int)(font.SizeInPoints+0.5));

            mOwner = owner;
            mBackBuffer = mOwner.BackBuffer;

            mSprite = new SdlDotNet.Sprites.TextSprite("text", mFont);
        }

        public override int StringDisplayWidth(string text)
        {
            return mFont.SizeText(text).Width;
        }
        public override int StringDisplayHeight(string text)
        {
            return mFont.SizeText(text).Height;
        }
        public override System.Drawing.Size StringDisplaySize(string text)
        {
            return mFont.SizeText(text);
        }

        public override void DrawText(int dest_x, int dest_y, string text)
        {
            mSprite.Position = new Point(dest_x, dest_y);
            mSprite.Text = text;
            mSprite.Color = FontColor;
            
            mBackBuffer.Blit(mSprite);
        }
        public override void DrawText(double dest_x, double dest_y, string text)
        {
            DrawText((int)dest_x, (int)dest_y, text);
        }
        public override void DrawText(System.Drawing.Point dest_pt, string text)
        {
            DrawText(dest_pt.X, dest_pt.Y, text);
        }
        public override void DrawText(System.Drawing.PointF dest_pt, string text)
        {
            DrawText((int)dest_pt.X, dest_pt.Y, text);
        }
        public override void DrawText(string text)
        {
            DrawText(0, 0, text);
        }

        public override void Dispose()
        {
            if (mFont != null)            mFont.Dispose();
            if (mSprite != null) mSprite.Dispose();

            mOwner = null;
            mFont = null;
            mBackBuffer = null;
            mSprite = null;
        }

        public override void DrawText(int dest_x, int dest_y, string text, CanvasImpl c)
        {
            SDL_Canvas canvas = c as SDL_Canvas;
            
            mSprite.Position = new Point(dest_x, dest_y);
            mSprite.Text = text;
            mSprite.Color = FontColor;
            
            canvas.Surface.Blit(mSprite);

            
        }

        public override void DrawText(Point dest_pt, string text, CanvasImpl c)
        {
            DrawText(dest_pt.X, dest_pt.Y, text, c);
        }
    }
    class SDL_Canvas : CanvasImpl
    {
        SDL.Surface mSurface;

        public SDL_Canvas(SDL.Surface surface)
        {
            mSurface = surface;
        }
        public override System.Drawing.Size Size
        {
            get { return mSurface.Size; }
        }

        public override void Clear(System.Drawing.Color color)
        {
            mSurface.Fill(color);
        }
        public override void Clear(System.Drawing.Color color, System.Drawing.Rectangle dest)
        {
            mSurface.Fill(dest, color);
        }

        public override void DrawEllipse(System.Drawing.Rectangle target, System.Drawing.Color color, 
            float thickness)
        {
            SDL.Ellipse ellipse = new SDL.Ellipse(target.Location, target.Size);

            mSurface.DrawEllipse(ellipse, color);
        }
        public override void DrawRect(System.Drawing.Rectangle target, System.Drawing.Color color, 
            float thickness)
        {
            mSurface.DrawBox(target, color);

        }

        public override void FillRect(System.Drawing.Rectangle rect, System.Drawing.Color color)
        {
            mSurface.DrawFilledBox(rect, color);
        }
        public override void FillEllipse(System.Drawing.Rectangle rect, System.Drawing.Color color)
        {
            SDL.Ellipse ellipse = new SDL.Ellipse(rect.Location, rect.Size);

            mSurface.DrawFilledEllipse(ellipse, color);
        }

        public override void Dispose()
        {
            
        }

        public SDL.Surface Surface
        {
            get { return mSurface; }
        }
    }

}
