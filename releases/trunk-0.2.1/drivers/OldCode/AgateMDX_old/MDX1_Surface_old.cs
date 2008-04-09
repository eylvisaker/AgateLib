using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Direct3D = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using CustomVertex = Microsoft.DirectX.Direct3D.CustomVertex;
using ERY.AgateGL.ImplBase;
using ERY.AgateGL.Utility;

namespace ERY.AgateGL.MDXold
{
    public class MDX1_Surface_old : SurfaceImpl, MDX1_IRenderTarget_old 
    {
        #region --- Private Variables ---

        MDX1_Display_old mDisplay;
        Direct3D.Device mDevice;

        Ref<Texture> mTexture;
        RenderToSurface mRenderToSurface;

        string mFileName;

        Rectangle mSrcRect;
        Size mTextureSize;

        CustomVertex.TransformedColoredTextured[] verts = new CustomVertex.TransformedColoredTextured[4];

        #endregion

        #region --- Creation / Destruction ---

        public MDX1_Surface_old()
        {

            mDisplay = Display.Impl as MDX1_Display_old;
            mDevice = mDisplay.D3D_Device;

        }
        public MDX1_Surface_old(string fileName)
        {
            mFileName = fileName;

            mDisplay = Display.Impl as MDX1_Display_old;
            mDevice = mDisplay.D3D_Device;

            LoadFromFile();

            mDevice.DeviceReset += new EventHandler(mDevice_DeviceReset);
        }
        public MDX1_Surface_old(Size size)
        {
            mSrcRect = new Rectangle(new Point(0, 0), size);

            mDisplay = Display.Impl as MDX1_Display_old;
            mDevice = mDisplay.D3D_Device;
            /*
            Bitmap bitmap = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.Dispose();
            */
            //mTexture = Texture.FromBitmap(mDevice, bitmap, Usage.None, Pool.Managed);
            mTexture = new Ref<Texture>(new Texture(mDevice, size.Width, size.Height, 1, Usage.None ,
                Format.A8R8G8B8, Pool.Managed));

            mRenderToSurface = new RenderToSurface(mDevice, size.Width, size.Height, 
                Format.A8R8G8B8, true, DepthFormat.D16 );

            mRenderToSurface.BeginScene(mTexture.Value.GetSurfaceLevel(0));
            mDevice.Clear(ClearFlags.Target, Color.FromArgb(0, 0, 0, 0).ToArgb(), 1.0f, 0);
            mRenderToSurface.EndScene(Filter.None);

            mRenderToSurface.Dispose();
            mRenderToSurface = null;

            mTextureSize = mSrcRect.Size;
        }
        public MDX1_Surface_old(Ref<Texture> texture, Rectangle sourceRect)
        {
            mSrcRect = sourceRect;

            mDisplay = Display.Impl as MDX1_Display_old;
            mDevice = mDisplay.D3D_Device;

            mTexture = new Ref<Texture>(texture);

            mTextureSize = new Size(mTexture.Value.GetSurfaceLevel(0).Description.Width,
                mTexture.Value.GetSurfaceLevel(0).Description.Height);
           

        }
        public override void Dispose()
        {
            if (mTexture.IsDisposed == false)
            {
                mTexture.Dispose();
            }
        }


        public void LoadFromFile()
        {
            if (string.IsNullOrEmpty(mFileName))
                return;

            string path = mFileName;
            Bitmap bitmap = new Bitmap(path);

            mSrcRect = new Rectangle(Point.Empty, new Size(bitmap.Size));
            /*
            // this is the speed issue fix in the debugger found on the net (thezbuffer.com has it documented)
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);

            stream.Position = 0;
            
            mTexture = Texture.FromStream(mDevice, stream, Usage.None, Pool.Managed);
             * */
            //mTexture = new Texture(mDevice, bitmap, Usage.None, Pool.Managed);
            Format format;

            switch (mDevice.DisplayMode.Format)
            {
                case Format.X8R8G8B8:
                    format = Format.A8R8G8B8;
                    break;

                case Format.X8B8G8R8:
                    format = Format.A8B8G8R8;
                    break;

                default:
                    System.Diagnostics.Debug.Assert(false);
                    throw new Exception("What format do I use?");
                    
            }

            mTexture = new Ref<Texture>(TextureLoader.FromFile(mDevice, path, 0, 0, 1, Usage.None,
                 format, Pool.Managed, Filter.None, Filter.None, 0x00000000));

            mTextureSize = new Size(mTexture.Value.GetSurfaceLevel(0).Description.Width,
                mTexture.Value.GetSurfaceLevel(0).Description.Height);

            bitmap.Dispose();
        }

        #endregion
        #region --- Events and event handlers ---

        public void mDevice_DeviceReset(object sender, EventArgs e)
        {
            LoadFromFile();
        }

        #endregion

        #region --- Drawing Helper functions ---

        protected void RotatePointInPlace(ref PointF pt, PointF rotation)
        {
            //PointF local = new PointF(pt.X - rotation.X, pt.Y - rotation.Y);
            float localX = pt.X - rotation.X;
            float localY = pt.Y - rotation.Y;

            double cos = Math.Cos(RotationAngle);
            double sin = Math.Sin(RotationAngle);

            pt.X = (int)(cos * localX + sin * localY);
            pt.Y = (int)(-sin * localX + cos * localY);

            pt.X += rotation.X;
            pt.Y += rotation.Y;

        }
        protected void RotatePointInPlace(ref CustomVertex.TransformedColoredTextured pt, PointF rotation)
        {
            //PointF local = new PointF(pt.X - rotation.X, pt.Y - rotation.Y);
            float localX = pt.X - rotation.X;
            float localY = pt.Y - rotation.Y;

            double cos = Math.Cos(RotationAngle);
            double sin = Math.Sin(RotationAngle);

            pt.X = (float)(cos * localX + sin * localY);
            pt.Y = (float)(-sin * localX + cos * localY);

            pt.X += rotation.X;
            pt.Y += rotation.Y;

        }
        protected void TranslatePointInPlace(ref PointF[] pt, PointF origin)
        {
            for (int i = 0; i < pt.Length; i++)
            {
                pt[i].X -= origin.X;
                pt[i].Y -= origin.Y;
            }
        }

        protected void TranslatePointInPlace(ref PointF pt, PointF origin)
        {
            pt.X -= origin.X;
            pt.Y -= origin.Y;
        }
        protected void TranslatePointInPlace(ref CustomVertex.TransformedColoredTextured pt, PointF origin)
        {
            pt.X -= origin.X;
            pt.Y -= origin.Y;
        }

        protected void DrawWithoutVB(float destX, float destY, bool alphaBlend)
        {
            // find center
           PointF rotation = Origin.CalcF(RotationCenter, DisplaySize);


           DrawWithoutVB(destX, destY, rotation.X, rotation.Y, alphaBlend);
        }
        protected void DrawWithoutVB(float destX, float destY, float rotationCenterX, float rotationCenterY, bool alphaBlend)
        {
            PointF centerpt = Origin.CalcF(DisplayAlignment, DisplaySize);
            PointF rotation = new PointF(rotationCenterX, rotationCenterY);

            rotation.X += destX;
            rotation.Y += destY;

            float leftBias = 0.0f;
            float topBias = 0.0f;
            float rightBias = 0.0f;
            float bottomBias = 0.0f;

            leftBias /= DisplayWidth;
            rightBias /= DisplayWidth; 
            topBias /= DisplayHeight;
            bottomBias /= DisplayHeight;

            float uLeft = mSrcRect.Left / (float)mTextureSize.Width + leftBias;
            float vTop = mSrcRect.Top / (float)mTextureSize.Height + topBias;
            float uRight = mSrcRect.Right / (float)mTextureSize.Width + rightBias;
            float vBottom = mSrcRect.Bottom / (float)mTextureSize.Height + bottomBias;

            int color = Color.ToArgb();
/*
            CustomVertex.TransformedColoredTextured[] verts = new CustomVertex.TransformedColoredTextured[4]
            {
                new CustomVertex.TransformedColoredTextured
                    (destX , destY, 0.5f, 1.0f, color, uLeft, vTop),

                new CustomVertex.TransformedColoredTextured
                    (destX + DisplayWidth, destY, 0.5f, 1.0f, color, uRight, vTop),

                new CustomVertex.TransformedColoredTextured
                    (destX, destY + DisplayHeight, 0.5f, 1.0f, color, uLeft, vBottom),

                new CustomVertex.TransformedColoredTextured
                    (destX + DisplayWidth, destY + DisplayHeight, 0.5f, 1.0f, color, uRight, vBottom),

            };

            */
            // avoiding the use of the constructor, as in the commented code above, results
            // in a surprisingly large performance increase.
            verts[0].X = destX;
            verts[0].Y = destY;
            verts[0].Z = 0.5f;
            verts[0].Rhw = 1.0f;
            verts[0].Color = color;
            verts[0].Tu = uLeft;
            verts[0].Tv = vTop;

            verts[1].X = destX + DisplayWidth ;
            verts[1].Y = destY;
            verts[1].Z = 0.5f;
            verts[1].Rhw = 1.0f;
            verts[1].Color = color;
            verts[1].Tu = uRight;
            verts[1].Tv = vTop;

            verts[2].X = destX;
            verts[2].Y = destY + DisplayHeight;
            verts[2].Z = 0.5f;
            verts[2].Rhw = 1.0f;
            verts[2].Color = color;
            verts[2].Tu = uLeft;
            verts[2].Tv = vBottom;

            verts[3].X = destX + DisplayWidth ;
            verts[3].Y = destY + DisplayHeight ;
            verts[3].Z = 0.5f;
            verts[3].Rhw = 1.0f;
            verts[3].Color = color;
            verts[3].Tu = uRight;
            verts[3].Tv = vBottom;
           

            if (DisplayWidth < 0)
            {
                // swap points
                for (int i = 0; i < 4; i += 2)
                {
                    CustomVertex.TransformedColoredTextured t = verts[i];
                    verts[i] = verts[i + 1];
                    verts[i + 1] = t;
                }

                for (int i = 0; i < 4; i++)
                    verts[i].X -= DisplayWidth;
            }
            if (DisplayHeight < 0)
            {
                // swap points
                for (int i = 0; i < 2; i += 1)
                {
                    CustomVertex.TransformedColoredTextured t = verts[i];
                    verts[i] = verts[i + 2];
                    verts[i + 2] = t;
                }

                for (int i = 0; i < 4; i++)
                    verts[i].Y -= DisplayHeight;

            }

            // do rotation and translation
            if (RotationAngle != 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    RotatePointInPlace(ref verts[i], rotation);
                    TranslatePointInPlace(ref verts[i], centerpt);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    TranslatePointInPlace(ref verts[i], centerpt);
                }
            }

            
            mDisplay.SetDeviceStateTexture(mTexture.Value, alphaBlend);

            mDevice.VertexFormat = CustomVertex.TransformedColoredTextured.Format;
            mDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, verts);
        }
        
        protected void DrawWithoutVBNoRotation(Rectangle destRect, bool alphaBlend)
        {
            DrawWithoutVBNoRotation(new Rectangle(new Point(0, 0), SurfaceSize), destRect, alphaBlend);
        }
        protected void DrawWithoutVBNoRotation(Rectangle srcRect, Rectangle destRect, bool alphaBlend)
        {
            //CustomVertex.TransformedColoredTextured[] verts = new CustomVertex.TransformedColoredTextured[4];
            int startIndex = 0;

            AddRectToVB(verts, startIndex, srcRect, destRect);

            mDisplay.SetDeviceStateTexture(mTexture.Value, alphaBlend);

            mDevice.VertexFormat = CustomVertex.TransformedColoredTextured.Format;
            mDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, verts);

        }

        private void AddRectToVB(CustomVertex.TransformedColoredTextured[] verts, int startIndex, 
                                Rectangle srcRect, Rectangle destRect)
        {
            // find center
            PointF centerpt = Origin.CalcF(DisplayAlignment, DisplaySize);

            float left = destRect.Left;
            float top = destRect.Top;
            float right = destRect.Right;// +(float)Math.Floor(destRect.Width / (float)srcRect.Width);
            float bottom = destRect.Bottom;// +(float)Math.Floor(destRect.Height / (float)srcRect.Height);

            PointF[] corners = new PointF[4] 
            {   
                new PointF(left, top),
                new PointF(right, top),
                new PointF(left, bottom),
                new PointF(right, bottom) 
            };

            

            PointF[] uv = new PointF[4]
            {
                new PointF(mSrcRect.Left + srcRect.Left, mSrcRect.Top + srcRect.Top),
                new PointF(mSrcRect.Left + srcRect.Right, mSrcRect.Top + srcRect.Top),
                new PointF(mSrcRect.Left + srcRect.Left, mSrcRect.Top + srcRect.Bottom),
                new PointF(mSrcRect.Left + srcRect.Right, mSrcRect.Top + srcRect.Bottom) 
            };

            TranslatePointInPlace(ref corners, centerpt);


            for (int i = 0; i < 4; i++)
            {
                verts[startIndex + i] = new Direct3D.CustomVertex.TransformedColoredTextured(
                   corners[i].X, corners[i].Y, 0.5F, 1, Color.ToArgb(),
                   uv[i].X / (float)mTextureSize.Width, uv[i].Y / (float)mTextureSize.Height);
            }
        }

        [Obsolete("Drawing using the vertex buffer is much slower when there are only two primitives.  Use DrawWithoutVB")]
        private void DrawVB(float destX, float destY, bool alphaBlend)
        {

            // find center
            PointF centerpt = Origin.CalcF(DisplayAlignment, DisplaySize);
            PointF rotation = Origin.CalcF(RotationCenter, DisplaySize);

            rotation.X += destX;
            rotation.Y += destY;


            PointF[] corners = new PointF[4] 
            {   
                new PointF(destX, destY),
                new PointF(destX + DisplayWidth, destY),
                new PointF(destX, destY + DisplayHeight),
                new PointF(destX + DisplayWidth, destY + DisplayHeight) 
            };


            float tlBias = 0.0f;
            float brBias = 1.0f;


            // TODO: These need updating
            PointF[] uv = new PointF[4]
            {
                new PointF(tlBias, tlBias),
                new PointF(1 + brBias / SurfaceWidth, tlBias),
                new PointF( tlBias, 1 + brBias / SurfaceHeight),
                new PointF(1 + brBias/ SurfaceWidth, 1 + brBias / SurfaceHeight) 
            };

            if (DisplayWidth < 0)
            {
                // swap points
                for (int i = 0; i < 4; i += 2)
                {
                    PointF t = corners[i];
                    corners[i] = corners[i + 1];
                    corners[i + 1] = t;

                    PointF uvt = uv[i];
                    uv[i] = uv[i + 1];
                    uv[i + 1] = uvt;
                }

                for (int i = 0; i < 4; i++)
                    corners[i].X -= DisplayWidth;
            }
            if (DisplayHeight < 0)
            {
                // swap points
                for (int i = 0; i < 2; i += 1)
                {
                    PointF t = corners[i];
                    corners[i] = corners[i + 2];
                    corners[i + 2] = t;

                    PointF uvt = uv[i];
                    uv[i] = uv[i + 2];
                    uv[i + 2] = uvt;
                }

                for (int i = 0; i < 4; i++)
                    corners[i].Y -= DisplayHeight;

            }

            // do rotation and translation
            if (RotationAngle != 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    RotatePointInPlace(ref corners[i], rotation);
                    TranslatePointInPlace(ref corners[i], centerpt);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    TranslatePointInPlace(ref corners[i], centerpt);
                }
            }

            CustomVertex.TransformedColoredTextured[] verts = new CustomVertex.TransformedColoredTextured[4];

            for (int i = 0; i < 4; i++)
            {
                verts[i] = new Direct3D.CustomVertex.TransformedColoredTextured(
                   (float)corners[i].X, (float)corners[i].Y, 0.5F, 1, Color.ToArgb(), uv[i].X, uv[i].Y);
            }

            mDisplay.SetDeviceStateTexture(mTexture.Value, alphaBlend);
            mDisplay.WriteToSurfaceVBAndRender(PrimitiveType.TriangleStrip, 2, verts);

        }


        #endregion
        #region --- Drawing to screen functions ---

        public override void Draw(float destX, float destY)
        {
            DrawWithoutVB(destX, destY, true);            
        }
        public override void Draw(float destX, float destY, float rotationCenterX, float rotationCenterY)
        {
            DrawWithoutVB(destX, destY, rotationCenterX, rotationCenterY, true);
        }
        public override void Draw(Rectangle destRect)
        {
            Draw(new Rectangle(0, 0, mSrcRect.Width, mSrcRect.Height), destRect);
        }
        public override void Draw(Rectangle srcRect, Rectangle destRect)
        {
            DrawWithoutVBNoRotation(srcRect, destRect, true);
        }
        public override void DrawRects(Rectangle[] srcRects, Rectangle[] destRects)
        {
            CustomVertex.TransformedColoredTextured[] verts = 
                new CustomVertex.TransformedColoredTextured[srcRects.Length * 4];

            int startIndex = 0;

            for (int i = 0; i < srcRects.Length; i++)
            {
                AddRectToVB(verts, startIndex, srcRects[i], destRects[i]);

                startIndex += 4;
            }

            mDisplay.SetDeviceStateTexture(mTexture.Value, true);

            mDevice.VertexFormat = CustomVertex.TransformedColoredTextured.Format;
            mDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2 * srcRects.Length, verts);
        }

        #endregion
   
        #region --- Overriden public properties ---

        public override int SurfaceHeight
        {
            get { return mSrcRect.Height; }
        }
        public override int SurfaceWidth
        {
            get { return mSrcRect.Width; }
        }

        public override Size SurfaceSize
        {
            get { return mSrcRect.Size; }
        }
        #endregion


        public override bool IsSurfaceBlank()
        {
            return IsSurfaceBlank((int)(Display.AlphaThreshold * 255.0));
        }
        public override bool IsSurfaceBlank(int alphaThreshold)
        {
            for (int i = 0; i < SurfaceHeight; i++)
            {
                if (IsRowBlank(i) == false)
                    return false;
            }

            return true;

        }

        public override bool IsRowBlank(int row)
        {
            Direct3D.Surface surf = mTexture.Value.GetSurfaceLevel(0);

            int stride;
            GraphicsStream stm = surf.LockRectangle((System.Drawing.Rectangle)mSrcRect, LockFlags.ReadOnly, out stride);

            bool retval = this.IsRowBlankScanARGB(stm.InternalData, row, this.SurfaceWidth,
                stride, (int)(Display.AlphaThreshold * 255.0), 0xff000000, 24);
            
            surf.UnlockRectangle();

            return retval;
        }

        public override bool IsColumnBlank(int col)
        {
            Direct3D.Surface surf = mTexture.Value.GetSurfaceLevel(0);

            int stride;
            GraphicsStream stm = surf.LockRectangle((System.Drawing.Rectangle)mSrcRect, LockFlags.ReadOnly, out stride);

            bool retval = this.IsColBlankScanARGB(stm.InternalData, col, this.SurfaceHeight,
                stride, (int)(Display.AlphaThreshold * 255.0), 0xff000000, 24);

            surf.UnlockRectangle();

            return retval;
        }

        public override void SaveTo(string frameFile, ImageFileFormat format)
        {
            Direct3D.Surface surf = mTexture.Value.GetSurfaceLevel(0);
            Direct3D.ImageFileFormat d3dformat = Microsoft.DirectX.Direct3D.ImageFileFormat.Png;

            switch (format)
            {
                case ImageFileFormat.Bmp: d3dformat = Direct3D.ImageFileFormat.Bmp; break;
                case ImageFileFormat.Png: d3dformat = Direct3D.ImageFileFormat.Png; break;
                case ImageFileFormat.Jpg: d3dformat = Direct3D.ImageFileFormat.Jpg; break;
                case ImageFileFormat.Tga: d3dformat = Direct3D.ImageFileFormat.Tga; break;
            }

            SurfaceLoader.Save(frameFile, d3dformat, surf, (System.Drawing.Rectangle)mSrcRect);
        }


        #region --- MDX1_IRenderTarget Members ---

        public override void BeginRender()
        {
            // it looks like Direct3D creates a new surface.
            // so here we will create a new texture, and draw the current texture to it
            // then discard the old one.  
            Texture t = new Texture(mDevice, SurfaceWidth, SurfaceHeight, 1, Usage.None,
                Format.A8R8G8B8, Pool.Managed);

            Direct3D.Surface surfaceTarget = t.GetSurfaceLevel(0);

            mRenderToSurface = new RenderToSurface(mDevice, SurfaceWidth, SurfaceHeight,
                Format.A8R8G8B8, false, DepthFormat.D16);

            Viewport vp = new Viewport();

            vp.X = 0;
            vp.Y = 0;
            vp.Width = SurfaceWidth;
            vp.Height = SurfaceHeight;

            mRenderToSurface.BeginScene(surfaceTarget, vp);

            DrawWithoutVBNoRotation(new Rectangle(new Point(0, 0), SurfaceSize), false);

            mTexture.Dispose();
            mTexture = new Ref<Texture>(t);

        }
        public override void EndRender(bool waitVSync)
        {
            mRenderToSurface.EndScene(Filter.None);
        }

        #endregion


        public override SurfaceImpl CarveSubSurface(Surface surf, Rectangle srcRect)
        {
            Rectangle newSrcRect = new Rectangle(
                mSrcRect.Left + srcRect.Left,
                mSrcRect.Top + srcRect.Top,
                srcRect.Width,
                srcRect.Height);

            return new MDX1_Surface_old(mTexture, newSrcRect);
        }
        [Obsolete("Base class obsolete")]
        public override void SetSourceSurface(SurfaceImpl surf, Rectangle srcRect)
        {
            mTexture.Dispose();
            mTexture = new Ref<Texture>((surf as MDX1_Surface_old).mTexture);

            mSrcRect = srcRect;

            Direct3D.Surface d3dsurf = mTexture.Value.GetSurfaceLevel(0);

            mTextureSize = new Size(d3dsurf.Description.Width, d3dsurf.Description.Height);
        }
    }

}
