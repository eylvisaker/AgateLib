using System;
using System.Collections.Generic;
using Drawing = System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using ERY.AgateLib;
using ERY.AgateLib.Drivers;
using ERY.AgateLib.Geometry;
using ERY.AgateLib.ImplBase;
using ERY.AgateLib.Utility;

using OpenTK.OpenGL;
using OpenTK.OpenGL.Enums;
using OTKPixelFormat = OpenTK.OpenGL.Enums.PixelFormat;

namespace ERY.AgateLib.OpenGL
{
    public sealed class GL_Surface : SurfaceImpl, GL_IRenderTarget
    {
        GL_Display mDisplay;
        GLState mState;

        string mFilename;

        /// <summary>
        /// Refrence counting for the texture id's.
        /// </summary>
        static Dictionary<int, int> mTextureIDs = new Dictionary<int, int>();
        int mTextureID;

        Rectangle mSourceRect;

        /// <summary>
        /// OpenGL's texture size (always a power of 2).
        /// </summary>
        Size mTextureSize;

        TextureCoordinates mTexCoord;

        float mRotationCos = 1.0f;
        float mRotationSin = 0.0f;

        public GL_Surface(string filename)
        {
            mDisplay = Display.Impl as GL_Display;
            mState = mDisplay.State;

            mFilename = filename;

            Load();
        }
        public GL_Surface(Stream st)
        {
            mDisplay = Display.Impl as GL_Display;
            mState = mDisplay.State;

            // Load The Bitmap
            Drawing.Bitmap sourceImage = new Drawing.Bitmap(st);

            LoadFromBitmap(sourceImage);
        }
        public GL_Surface(Size size)
        {
            mDisplay = Display.Impl as GL_Display;
            mState = mDisplay.State;

            mSourceRect = new Rectangle(Point.Empty, size);

            mTextureSize = new Size(NextPowerOfTwo(size.Width), NextPowerOfTwo(size.Height));

            //int[] array = new int[1];
            //GL.GenTextures(1, array);
            int textureID;
            GL.GenTextures(1, out textureID);
            
            AddTextureRef(textureID);

            IntPtr fake = IntPtr.Zero;

            try
            {
                fake = Marshal.AllocHGlobal(mTextureSize.Width * mTextureSize.Height * Marshal.SizeOf(typeof(int)));

                // Typical Texture Generation Using Data From The Bitmap
                GL.BindTexture(TextureTarget.Texture2d, mTextureID);
                GL.TexImage2D(TextureTarget.Texture2d, 0, PixelInternalFormat.Rgba,
                    mTextureSize.Width, mTextureSize.Height, 0, OTKPixelFormat.Rgba,
                    PixelType.UnsignedByte, fake);

                GL.TexParameter(TextureTarget.Texture2d,
                                TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2d,
                                TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                mTexCoord = GetTextureCoords(mSourceRect);
            }
            finally
            {
                if (fake != IntPtr.Zero)
                    Marshal.FreeHGlobal(fake);
            }
        }
       
        private GL_Surface(int textureID, Rectangle sourceRect, Size textureSize)
        {
            mDisplay = Display.Impl as GL_Display;
            mState = mDisplay.State;

            AddTextureRef(textureID);

            mSourceRect = sourceRect;
            mTextureSize = textureSize;

            mTexCoord = GetTextureCoords(mSourceRect);

        }

        private void AddTextureRef(int textureID)
        {
            mTextureID = textureID;

            if (mTextureIDs.ContainsKey(mTextureID))
                mTextureIDs[mTextureID] += 1;
            else
                mTextureIDs.Add(mTextureID, 1);
        }
        private void ReleaseTextureRef()
        {
            if (mTextureID == 0)
                return;

            mTextureIDs[mTextureID]--;

            if (mTextureIDs[mTextureID] == 0)
            {
                int[] array = new int[1];
                array[0] = mTextureID;

                GL.DeleteTextures(1, array);

                mTextureIDs.Remove(mTextureID);
            }

            mTextureID = 0;
        }
        public override void Dispose()
        {
            ReleaseTextureRef();
           
        }



        public override double RotationAngle
        {
            get
            {
                return base.RotationAngle;
            }
            set
            {
                base.RotationAngle = value;

                mRotationCos = (float)Math.Cos(RotationAngle);
                mRotationSin = (float)Math.Sin(RotationAngle);

            }
        }


        public override void Draw(Rectangle destRect)
        {
            Draw(mSourceRect, destRect);
        }
        public override void Draw(RectangleF srcRect, RectangleF destRect)
        {
            srcRect.X += mSourceRect.X;
            srcRect.Y += mSourceRect.Y;
            
            TextureCoordinates texcoords = GetTextureCoords(srcRect);
            RectangleF dest = new RectangleF(destRect.X, destRect.Y, destRect.Width, destRect.Height);

            if (TesselateFactor == 1)
            {
                mState.DrawBuffer.AddQuad(mTextureID, Color, texcoords, dest);
            }
            else
            {
                float texWidth = texcoords.Right - texcoords.Left;
                float texHeight = texcoords.Bottom - texcoords.Top;

                for (int j = 0; j < TesselateFactor; j++)
                {
                    RectangleF subRect = dest;
                    TextureCoordinates coords = texcoords;

                    subRect.Y = dest.Top + j * dest.Height / (float)TesselateFactor;
                    subRect.Height = dest.Height / (float)TesselateFactor;

                    coords.Top = texcoords.Top + texHeight * j / TesselateFactor;
                    coords.Bottom = coords.Top + texHeight / TesselateFactor;

                    for (int i = 0; i < TesselateFactor; i++)
                    {
                        subRect.X = dest.Left + i * dest.Width / (float)TesselateFactor;
                        subRect.Width = dest.Width / (float)TesselateFactor;

                        coords.Left = texcoords.Left + texWidth * i / TesselateFactor;
                        coords.Right = coords.Left + texWidth / TesselateFactor;

                        mState.DrawBuffer.AddQuad(mTextureID, Color, coords, subRect);
                    }
                }
            }
            
        }
        public override void Draw(float destX, float destY)
        {
            Point rotatePoint = Origin.Calc(RotationCenter, DisplaySize);

            Draw(destX, destY, rotatePoint.X, rotatePoint.Y);
        }


        public override void Draw(float destX, float destY, float rotationCenterX, float rotationCenterY)
        {
            Draw(destX, destY, mSourceRect, rotationCenterX, rotationCenterY);
        }

        public override void Draw(float destX, float destY, Rectangle srcRect, float rotationCenterX, float rotationCenterY)
        {
            SizeF dispSize = new SizeF(
                srcRect.Width * (float)ScaleWidth,
                srcRect.Height * (float)ScaleHeight);
            
            if (DisplaySize.Width < 0)
                destX -= dispSize.Width;

            if (DisplaySize.Height < 0)
                destY -= dispSize.Height;

            mTexCoord = GetTextureCoords(srcRect);

            if (TesselateFactor == 1)
            {
                BufferQuad(destX, destY, rotationCenterX, rotationCenterY,
                    dispSize.Width, dispSize.Height, mTexCoord, ColorGradient);
            }
            else
            {
                TextureCoordinates texCoord = new TextureCoordinates();
                float texWidth = mTexCoord.Right - mTexCoord.Left;
                float texHeight = mTexCoord.Bottom - mTexCoord.Top;

                float displayWidth = DisplayWidth / (float)TesselateFactor;
                float displayHeight = DisplayHeight / (float)TesselateFactor;

                for (int j = 0; j < TesselateFactor; j++)
                {
                    texCoord.Top = mTexCoord.Top + j * texHeight / TesselateFactor;
                    texCoord.Bottom = mTexCoord.Top + (j + 1) * texHeight / TesselateFactor;

                    for (int i = 0; i < TesselateFactor; i++)
                    {
                        texCoord.Left = mTexCoord.Left + i * texWidth / TesselateFactor;
                        texCoord.Right = mTexCoord.Left + (i+1) * texWidth / TesselateFactor;

                        float dx = destX + i * displayWidth * mRotationCos + j * displayHeight * mRotationSin;
                        float dy = destY - i * displayWidth * mRotationSin + j * displayHeight * mRotationCos;

                        double cx = i / (double)TesselateFactor;
                        double cy = j / (double)TesselateFactor;

                        Gradient color = new Gradient(
                            ColorGradient.Interpolate(cx, cy),
                            ColorGradient.Interpolate(cx + 1.0 / TesselateFactor, cy),
                            ColorGradient.Interpolate(cx, cy + 1.0 / TesselateFactor),
                            ColorGradient.Interpolate(cx + 1.0 / TesselateFactor, cy + 1.0 / TesselateFactor));

                        BufferQuad(dx, dy, 
                            rotationCenterX, rotationCenterY,
                            displayWidth, displayHeight, texCoord, color);

                    }
                }
            }
            //GL.PushMatrix();

            //GL.Translatef(-translatePoint.X, -translatePoint.Y, 0);
            //GL.Rotatef((float)-RotationAngleDegrees, 0.0f, 0.0f, 1.0f);


            // restore the matrix
            //GL.PopMatrix();
        }

        private void BufferQuad(float destX, float destY, float rotationCenterX, float rotationCenterY,
            float displayWidth, float displayHeight, TextureCoordinates texCoord, Gradient color)
        {

            // order is 
            //  1 -- 2
            //  |    |
            //  4 -- 3
            PointF[] pt = new PointF[4];

            SetPoints(pt, destX, destY,
                rotationCenterX, rotationCenterY, displayWidth, displayHeight);

            //RectangleF destRect = new RectangleF(new PointF(-rotationCenterX, -rotationCenterY),
            //                     new SizeF(displayWidth, displayHeight));


            mState.DrawBuffer.AddQuad(mTextureID, color, texCoord, pt);
        }

        private void SetPoints(PointF[] pt, float destX, float destY, float rotationCenterX, float rotationCenterY, 
                               float destWidth, float destHeight)
        {
            const int index = 0;
            PointF centerPoint = Origin.CalcF(DisplayAlignment, new SizeF(destWidth, destHeight));

            destX += rotationCenterX - centerPoint.X;
            destY += rotationCenterY - centerPoint.Y;

            // Point at (0, 0) local coordinates
            pt[index].X = mRotationCos * (-rotationCenterX) +
                         mRotationSin * (-rotationCenterY) + destX;

            pt[index].Y = -mRotationSin * (-rotationCenterX) +
                          mRotationCos * (-rotationCenterY) + destY;

            // Point at (DisplayWidth, 0) local coordinates
            pt[index + 1].X = mRotationCos * (-rotationCenterX + destWidth) +
                         mRotationSin * (-rotationCenterY) + destX;

            pt[index + 1].Y = -mRotationSin * (-rotationCenterX + destWidth) +
                          mRotationCos * (-rotationCenterY) + destY;

            // Point at (DisplayWidth, DisplayHeight) local coordinates
            pt[index + 2].X = mRotationCos * (-rotationCenterX + destWidth) +
                         mRotationSin * (-rotationCenterY + destHeight) + destX;

            pt[index + 2].Y = -mRotationSin * (-rotationCenterX + destWidth) +
                          mRotationCos * (-rotationCenterY + destHeight) + destY;

            // Point at (0, DisplayHeight) local coordinates
            pt[index + 3].X = mRotationCos * (-rotationCenterX) +
                         mRotationSin * (-rotationCenterY + destHeight) + destX;

            pt[index + 3].Y = (-mRotationSin * (-rotationCenterX) +
                           mRotationCos * (-rotationCenterY + destHeight)) + destY;

        }

        public override void SaveTo(string filename, ImageFileFormat format)
        {
            PixelBuffer buffer = ReadPixels(PixelFormat.Any);
            buffer.SaveTo(filename, format);
        }

        public override SurfaceImpl CarveSubSurface(Surface surface, Rectangle srcRect)
        {
            srcRect.X += mSourceRect.X;
            srcRect.Y += mSourceRect.Y;

            return new GL_Surface(mTextureID, srcRect, mTextureSize);
        }

        public override void SetSourceSurface(SurfaceImpl surf, Rectangle srcRect)
        {
            ReleaseTextureRef();
            AddTextureRef((surf as GL_Surface).mTextureID);

            mTextureSize = (surf as GL_Surface).mTextureSize;
            mSourceRect = srcRect;

            mTexCoord = GetTextureCoords(mSourceRect);

        }

        public override PixelBuffer ReadPixels(PixelFormat format)
        {
            return ReadPixels(format, new Rectangle(Point.Empty, SurfaceSize));
        }
        public override PixelBuffer ReadPixels(PixelFormat format, Rectangle rect)
        {
            if (format == PixelFormat.Any)
                format = PixelFormat.RGBA8888;

            rect.X += mSourceRect.X;
            rect.Y += mSourceRect.Y;

            int pixelStride = 4;
            int size = mTextureSize.Width * mTextureSize.Height * pixelStride;
            int memStride = pixelStride * mTextureSize.Width;
            IntPtr memory = Marshal.AllocHGlobal(size);

            GL.BindTexture(TextureTarget.Texture2d, mTextureID);
            GL.GetTexImage(TextureTarget.Texture2d, 0, OTKPixelFormat.Rgba,
                 PixelType.UnsignedByte, memory);

            byte[] data = new byte[rect.Width * rect.Height * pixelStride];

            unsafe
            {
                for (int i = rect.Top; i < rect.Bottom; i++)
                {
                    int dataIndex = (i - rect.Top) * pixelStride * rect.Width;
                    IntPtr memPtr = (IntPtr)((byte*)memory + i * memStride + rect.Left * pixelStride);

                    Marshal.Copy(memPtr, data, dataIndex, pixelStride * rect.Width);
                }
            }

            Marshal.FreeHGlobal(memory);

            if (format == PixelFormat.RGBA8888)
                return new PixelBuffer(format, SurfaceSize, data);
            else
                return new PixelBuffer(format, SurfaceSize, data, PixelFormat.RGBA8888);
        }

        public override void WritePixels(PixelBuffer buffer)
        {
            if (buffer.PixelFormat != PixelFormat.RGBA8888 ||
                buffer.Size.Equals(mTextureSize) == false)
            {
                buffer = buffer.ConvertTo(PixelFormat.RGBA8888, mTextureSize);
            }

            unsafe
            {
                fixed (byte* ptr = buffer.Data)
                {
                    // Typical Texture Generation Using Data From The Bitmap
                    GL.BindTexture(TextureTarget.Texture2d, mTextureID);
                    GL.TexImage2D(TextureTarget.Texture2d, 0, PixelInternalFormat.Rgba,
                        mTextureSize.Width, mTextureSize.Height, 0, OTKPixelFormat.Rgba,//, GL.GL_BGRA, 
                        PixelType.UnsignedByte, (IntPtr)ptr);

                    GL.TexParameter(TextureTarget.Texture2d,
                                     TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2d,
                                     TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                }
            }
        }
        public override void WritePixels(PixelBuffer buffer, Point startPoint)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override Size SurfaceSize
        {
            get { return mSourceRect.Size; }
        }

        public override bool IsSurfaceBlank()
        {
            return false;
        }
        public override bool IsSurfaceBlank(int alphaThreshold)
        {
            return false;
        }
        public override bool IsRowBlank(int row)
        {
            return false;
        }
        public override bool IsColumnBlank(int col)
        {
            return false;
        }

        public override void BeginRender()
        {
            GL.Viewport(0, 0, SurfaceWidth, SurfaceHeight);

            mDisplay.SetupGLOrtho(Rectangle.FromLTRB(0, SurfaceHeight, SurfaceWidth, 0));


            // clear the framebuffer and draw this texture to it.
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | 
                     ClearBufferMask.DepthBufferBit);

            GL.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);

            Draw();

            GL.TexParameter(TextureTarget.Texture2d,
                             TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2d,
                             TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
        }
        public override void EndRender()
        {
           // GL.Disable(EnableCap.Texture2d);
            GL.BindTexture(TextureTarget.Texture2d, mTextureID);

            GL.CopyTexSubImage2D(TextureTarget.Texture2d,
                0, 0, 0, 0, 0, mSourceRect.Width, mSourceRect.Height);
            //GL.CopyTexImage2D(TextureTarget.Texture2d, 0, PixelInternalFormat.Rgba8,
            //    0, 0, mSourceRect.Width, mSourceRect.Height, 0);
            
            GL.TexParameter(TextureTarget.Texture2d,
                             TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2d,
                             TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
        }

        #region GL_IRenderTarget Members

        public void MakeCurrent()
        {
            
        }

        #endregion


        private void Load()
        {
            if (mFilename == "")
                return;

            
            // Load The Bitmap
            Drawing.Bitmap sourceImage = new Drawing.Bitmap(mFilename);

            LoadFromBitmap(sourceImage);

            sourceImage.Dispose();
        }

        private void LoadFromBitmap(Drawing.Bitmap sourceImage)
        {

            mSourceRect.Size = new Size(sourceImage.Size);

            Size newSize = GetOGLSize(sourceImage);

            // create a new bitmap of the size OpenGL expects, and copy the source image to it.
            Drawing.Bitmap textureImage = new Drawing.Bitmap(newSize.Width, newSize.Height);
            Drawing.Graphics g = Drawing.Graphics.FromImage(textureImage);

            g.Transform = new System.Drawing.Drawing2D.Matrix();
            g.Clear(Drawing.Color.FromArgb(0, 0, 0, 0));
            g.DrawImage(sourceImage, new Drawing.Rectangle(new Drawing.Point(0, 0), sourceImage.Size));
            g.Dispose();

            mTextureSize = new Size(textureImage.Size);

            //textureImage.Save(@"temp.bmp", ImageFormat.Bmp);

            mTexCoord = GetTextureCoords(mSourceRect);


            // Rectangle For Locking The Bitmap In Memory
            Rectangle rectangle = new Rectangle(0, 0, textureImage.Width, textureImage.Height);

            // Get The Bitmap's Pixel Data From The Locked Bitmap
            BitmapData bitmapData = textureImage.LockBits((Drawing.Rectangle)rectangle,
                ImageLockMode.ReadOnly, Drawing.Imaging.PixelFormat.Format32bppArgb);

            // use a pixelbuffer to do format conversion.
            PixelBuffer buffer = new PixelBuffer(PixelFormat.RGBA8888, mTextureSize,
                bitmapData.Scan0, PixelFormat.BGRA8888, bitmapData.Stride);

            // Create The GL Texture object
            int textureID;
            
            GL.GenTextures(1, out textureID);
            AddTextureRef(textureID);

            WritePixels(buffer);

            textureImage.UnlockBits(bitmapData);                     // Unlock The Pixel Data From Memory
            textureImage.Dispose();                                  // Dispose The Bitmap
        }
        
        private Size GetOGLSize(System.Drawing.Bitmap image)
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
        private TextureCoordinates GetTextureCoords(Rectangle srcRect)
        {
            TextureCoordinates coords = new TextureCoordinates(
                (srcRect.Left) / (float)mTextureSize.Width,
                (srcRect.Top) / (float)mTextureSize.Height,
                (srcRect.Right ) / (float)mTextureSize.Width,
                (srcRect.Bottom ) / (float)mTextureSize.Height);

            return coords;
        }
        private TextureCoordinates GetTextureCoords(RectangleF srcRect)
        {
            TextureCoordinates coords = new TextureCoordinates(
                (srcRect.Left) / (float)mTextureSize.Width,
                (srcRect.Top) / (float)mTextureSize.Height,
                (srcRect.Right) / (float)mTextureSize.Width,
                (srcRect.Bottom) / (float)mTextureSize.Height);

            return coords;
        }

        /*
       GL_Display mDisplay;
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

       public GL_Surface(Surface owner, string filename)
       {
           mDisplay = Display.Impl as WGL_Display;
           mSurface = owner;

           mFilename = filename;


           Load();
       }

       public GL_Surface(Surface owner, Size size)
       {
           mDisplay = Display.Impl as WGL_Display;
           mSurface = owner;


       }

      
       private Size GetOGLSize(System.Drawing.Bitmap image)
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

           GL.glBindTexture(GL.GL_Texture2d, mTextureID);

           GL.glTranslatef(-translatePoint.X, -translatePoint.Y, 0);
           GL.glRotatef((float)-mSurface.RotationAngleDegrees, 0.0f, 0.0f, 1.0f);

           GL.glBegin(GL.GL_QUADS);

           GL.glTexCoord2f(mTexCoord.Left, mTexCoord.Top); GL.glVertex2f(destRect.Left, destRect.Top);
           GL.glTexCoord2f(mTexCoord.Right, mTexCoord.Top); GL.glVertex2f(destRect.Right, destRect.Top);
           GL.glTexCoord2f(mTexCoord.Right, mTexCoord.Bottom); GL.glVertex2f(destRect.Right, destRect.Bottom);
           GL.glTexCoord2f(mTexCoord.Left, mTexCoord.Bottom); GL.glVertex2f(destRect.Left, destRect.Bottom);

           GL.glEnd();

           // restore the matrix
           GL.glRotatef((float)mSurface.RotationAngleDegrees, 0.0f, 0.0f, 1.0f);
           GL.glTranslatef(translatePoint.X, translatePoint.Y, 0);
       }

       public override void DrawRects(System.Drawing.Rectangle[] src_rects, System.Drawing.Rectangle[] dest_rects)
       {
           throw new Exception("The method or operation is not implemented.");
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
           //GL.glDeleteTextures(1, new int[] { mTextureID } );
       }
*/

    }
}
