using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using Tao.OpenGl;


namespace ERY.GameLibrary
{
    public class WGL_Surface : SurfaceImpl 
    {
        WGL_Display mDisplay;
        string mFilename;

        Size mSize;

        /// <summary>
        /// Size of the actual texture in memory.. this will only be
        /// in powers of two.
        /// </summary>
        Size mTextureSize;

        // texture coordinates, since OGL requires textures
        // to be power of two..
        struct TextureCoordinates
        {
            public TextureCoordinates(float left, float top, float right, float bottom)
            {
                Top = top;
                Left = left;
                Bottom = bottom;
                Right = right;
            }
            public float Top;
            public float Bottom;
            public float Left;
            public float Right;
        }

        TextureCoordinates mTexCoord;

        // this is documented as "storage for one texture"
        // it really is just a texture identifier.
        int mTextureID;

        public WGL_Surface(Surface owner, string filename)
        {
            mDisplay = Display.Impl as WGL_Display;
            mSurface = owner;

            mFilename = filename;


            Load();
        }

        public WGL_Surface(Surface owner, Size size)
        {
            mDisplay = Display.Impl as WGL_Display;
            mSurface = owner;


        }

        private void Load()
        {
            if (mFilename == "")
                return;

            // Load The Bitmap
            Bitmap sourceImage = new Bitmap(
                System.IO.Path.Combine(Display.ImagePath, mFilename));

            mSize = sourceImage.Size;

            Size newSize = GetOGLSize(sourceImage);

            // create a new bitmap of the size OpenGL expects, and copy the source image to it.
            Bitmap textureImage = new Bitmap(newSize.Width, newSize.Height);
            Graphics g = Graphics.FromImage(textureImage);

            g.Transform = new System.Drawing.Drawing2D.Matrix();
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.DrawImage(sourceImage, new Rectangle(new Point(0, 0), sourceImage.Size));
            g.Dispose();

            mTextureSize = textureImage.Size;

            textureImage.Save(@"temp.bmp", ImageFormat.Bmp);

            TextureCoordinates coords = GetTextureCoords(new Rectangle(0, 0, sourceImage.Width, sourceImage.Height));

            mTexCoord = coords;

            // Rectangle For Locking The Bitmap In Memory
            Rectangle rectangle = new Rectangle(0, 0, textureImage.Width, textureImage.Height);

            // Get The Bitmap's Pixel Data From The Locked Bitmap
            BitmapData bitmapData = textureImage.LockBits(rectangle,
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            
            // Create The GL Texture object
            Gl.glGenTextures(1, out mTextureID);

            // Typical Texture Generation Using Data From The Bitmap
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, mTextureID);
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA8,
                textureImage.Width, textureImage.Height, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);


            textureImage.UnlockBits(bitmapData);                     // Unlock The Pixel Data From Memory
            textureImage.Dispose();                                  // Dispose The Bitmap

        }

        private TextureCoordinates GetTextureCoords(Rectangle srcRect)
        {
            TextureCoordinates coords = new TextureCoordinates(
                srcRect.Left / (float)mTextureSize.Width,
                srcRect.Top / (float)mTextureSize.Height,
                srcRect.Right / (float)mTextureSize.Width,
                srcRect.Bottom / (float)mTextureSize.Height);

            return coords;
        }

        private Size GetOGLSize(Bitmap image)
        {
            Size retval = new Size(
                NextPowerOfTwo(image.Width),
                NextPowerOfTwo(image.Height));

            return retval;
        }
        private int NextPowerOfTwo(int p)
        {
            return (int)Math.Pow(2, (int)(Math.Log(p) / Math.Log(2)) + 1);
        }

        public override void Draw(System.Drawing.Rectangle dest_rect)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Draw(System.Drawing.Rectangle src_rect, System.Drawing.Rectangle dest_rect)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Draw(System.Drawing.Point destPt)
        {
            Point rotatePoint = Origin.Calc(mSurface.RotationCenter, mSurface.DisplaySize);
            Point translatePoint = Origin.Calc(mSurface.DisplayAlignment, mSurface.DisplaySize);


            if (mSurface.DisplaySize.Width < 0)
                translatePoint.X += mSurface.DisplaySize.Width;

            if (mSurface.DisplaySize.Height < 0)
                translatePoint.Y += mSurface.DisplaySize.Height;

            translatePoint.X -= destPt.X + rotatePoint.X;
            translatePoint.Y -= destPt.Y + rotatePoint.Y;

            Rectangle destRect = new Rectangle(new Point(-rotatePoint.X, -rotatePoint.Y), mSurface.DisplaySize);

            mDisplay.SetGLColor(mSurface.Color);

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, mTextureID);

            Gl.glTranslatef(-translatePoint.X, -translatePoint.Y, 0);
            Gl.glRotatef((float)-mSurface.RotationAngleDegrees, 0.0f, 0.0f, 1.0f);

            Gl.glBegin(Gl.GL_QUADS);

            Gl.glTexCoord2f(mTexCoord.Left, mTexCoord.Top); Gl.glVertex2f(destRect.Left, destRect.Top);
            Gl.glTexCoord2f(mTexCoord.Right, mTexCoord.Top); Gl.glVertex2f(destRect.Right, destRect.Top);
            Gl.glTexCoord2f(mTexCoord.Right, mTexCoord.Bottom); Gl.glVertex2f(destRect.Right, destRect.Bottom);
            Gl.glTexCoord2f(mTexCoord.Left, mTexCoord.Bottom); Gl.glVertex2f(destRect.Left, destRect.Bottom);

            Gl.glEnd();

            // restore the matrix
            Gl.glRotatef((float)mSurface.RotationAngleDegrees, 0.0f, 0.0f, 1.0f);
            Gl.glTranslatef(translatePoint.X, translatePoint.Y, 0);
        }

        public override void DrawRects(System.Drawing.Rectangle[] src_rects, System.Drawing.Rectangle[] dest_rects)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Draw(System.Drawing.Rectangle dest_rect, CanvasImpl c)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Draw(System.Drawing.Rectangle src_rect, System.Drawing.Rectangle dest_rect, CanvasImpl c)
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

        public override CanvasImpl LockSurface()
        {
            throw new Exception("The method or operation is not implemented.");
            //WGL_Canvas retval = new WGL_Canvas(this);
        }

        public override void SaveTo(string filename)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override System.Drawing.Size SurfaceSize
        {
            get { return mSize; }
        }

        public override bool IsSurfaceBlank()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool IsSurfaceBlank(int alphaThreshold)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool IsRowBlank(int row)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool IsColumnBlank(int col)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Dispose()
        {
            //Gl.glDeleteTextures(1, new int[] { mTextureID } );
        }
    }
}
