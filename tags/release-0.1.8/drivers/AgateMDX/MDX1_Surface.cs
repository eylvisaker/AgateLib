//     ``The contents of this file are subject to the Mozilla Public License
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
using System.Runtime.InteropServices;
using System.Text;
using Direct3D = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using CustomVertex = Microsoft.DirectX.Direct3D.CustomVertex;
using ERY.AgateLib.ImplBase;
using ERY.AgateLib.Utility;

namespace ERY.AgateLib.MDX
{
    public class MDX1_Surface : SurfaceImpl, MDX1_IRenderTarget 
    {
        #region --- Private Variables ---

        MDX1_Display mDisplay;
        D3DDevice mDevice;

        Ref<Texture> mTexture;
        RenderToSurface mRenderToSurface;

        string mFileName;

        Rectangle mSrcRect;
        Size mTextureSize;
        PointF mCenterPoint;
        float mRotationCos = 1.0f;
        float mRotationSin = 0.0f;

        CustomVertex.TransformedColoredTextured[] mVerts = new CustomVertex.TransformedColoredTextured[4];
        short[] mIndices = new short[] { 0, 1, 2, 1, 2, 3 };

        CustomVertex.TransformedColoredTextured[] mExtraVerts = new CustomVertex.TransformedColoredTextured[4];
        short[] mExtraIndices = new short[] { 0, 1, 2, 1, 2, 3 };

        #endregion

        #region --- Creation / Destruction ---

        public MDX1_Surface()
        {
            mDisplay = Display.Impl as MDX1_Display;
            mDevice = mDisplay.D3D_Device;

            InitVerts();
        }

        public MDX1_Surface(string fileName)
        {
            mFileName = fileName;

            mDisplay = Display.Impl as MDX1_Display;
            mDevice = mDisplay.D3D_Device;

            LoadFromFile();

            mDevice.Device.DeviceReset += new EventHandler(mDevice_DeviceReset);

            InitVerts();
        }
        public MDX1_Surface(Size size)
        {
            mSrcRect = new Rectangle(new Point(0, 0), size);

            mDisplay = Display.Impl as MDX1_Display;
            mDevice = mDisplay.D3D_Device;
            /*
            Bitmap bitmap = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.Dispose();
            */
            //mTexture = Texture.FromBitmap(mDevice, bitmap, Usage.None, Pool.Managed);
            mTexture = new Ref<Texture>(new Texture(mDevice.Device, size.Width, size.Height, 1, Usage.None ,
                Format.A8R8G8B8, Pool.Managed));

            mRenderToSurface = new RenderToSurface(mDevice.Device, size.Width, size.Height, 
                Format.A8R8G8B8, true, DepthFormat.D16 );

            mRenderToSurface.BeginScene(mTexture.Value.GetSurfaceLevel(0));
            mDevice.Clear(ClearFlags.Target, Color.FromArgb(0, 0, 0, 0).ToArgb(), 1.0f, 0);
            mRenderToSurface.EndScene(Filter.None);

            mRenderToSurface.Dispose();
            mRenderToSurface = null;

            mTextureSize = mSrcRect.Size;

            InitVerts();
        }
        public MDX1_Surface(Ref<Texture> texture, Rectangle sourceRect)
        {
            mSrcRect = sourceRect;

            mDisplay = Display.Impl as MDX1_Display;
            mDevice = mDisplay.D3D_Device;

            mTexture = new Ref<Texture>(texture);

            mTextureSize = new Size(mTexture.Value.GetSurfaceLevel(0).Description.Width,
                mTexture.Value.GetSurfaceLevel(0).Description.Height);

            InitVerts();
        }
        public override void Dispose()
        {
            if (mTexture.IsDisposed == false)
            {
                mTexture.Dispose();
            }
        }


        private void InitVerts()
        {
            SetVertsTextureCoordinates(mVerts, 0, mSrcRect);
            SetVertsColor(mVerts, 0, 4);
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

            switch (mDevice.Device.DisplayMode.Format)
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

            mTexture = new Ref<Texture>(TextureLoader.FromFile(mDevice.Device, path, 0, 0, 1, Usage.None,
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

        #region --- Overriden base class methods ---

        public override Color Color
        {
            get
            {
                return base.Color;
            }
            set
            {

                base.Color = value;

                SetVertsColor(mVerts, 0, 4);
            }
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

        #endregion

        #region --- Drawing Helper functions ---

        #region --- Old ---
        [Obsolete("Old DX method.")]
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
        [Obsolete("Old DX method.")]
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
        [Obsolete("Old DX method.")]
        protected void TranslatePointInPlace(ref PointF[] pt, PointF origin)
        {
            for (int i = 0; i < pt.Length; i++)
            {
                pt[i].X -= origin.X;
                pt[i].Y -= origin.Y;
            }
        }

        [Obsolete("Old DX method.")]
        protected void TranslatePointInPlace(ref PointF pt, PointF origin)
        {
            pt.X -= origin.X;
            pt.Y -= origin.Y;
        }
        [Obsolete("Old DX method.")]
        protected void TranslatePointInPlace(ref CustomVertex.TransformedColoredTextured pt, PointF origin)
        {
            pt.X -= origin.X;
            pt.Y -= origin.Y;
        }


        #endregion

        protected void DrawWithoutVB(float destX, float destY, bool alphaBlend)
        {
            // find center
           PointF rotation = Origin.CalcF(RotationCenter, DisplaySize);

           DrawWithoutVB(destX, destY, rotation.X, rotation.Y, alphaBlend);
        }
        protected void DrawWithoutVB(float destX, float destY, 
            float rotationCenterX, float rotationCenterY, bool alphaBlend)
        {   
            if (DisplayWidth < 0)
            {
                destX -= DisplayWidth;
            }
            if (DisplayHeight < 0)
            {
                destY -= DisplayHeight;
            }

            SetVertsPosition(mVerts, 0, new RectangleF(destX, destY, DisplayWidth, DisplayHeight), 
                rotationCenterX, rotationCenterY);

            //mDevice.SetDeviceStateTexture(mTexture.Value);
            //mDevice.AlphaBlend = alphaBlend;

            //mDevice.VertexFormat = CustomVertex.TransformedColoredTextured.Format;
            //mDevice.Device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, verts);
            //mDevice.Device.DrawIndexedUserPrimitives(PrimitiveType.TriangleList, 0, 6, 2, indices, true, verts);
            mDevice.DrawBuffer.CacheDrawIndexedTriangles(mVerts, mIndices, mTexture.Value, alphaBlend);


        }

        private void SetVertsTextureCoordinates(CustomVertex.TransformedColoredTextured[] verts, int startIndex,
            Rectangle srcRect)
        {
            // if you change these, besure to uncomment the divisions below.
            const float leftBias = 0.0f;
            const float topBias = 0.0f;
            const float rightBias = 0.0f;
            const float bottomBias = 0.0f;
            /*
            leftBias /= DisplayWidth;
            rightBias /= DisplayWidth;
            topBias /= DisplayHeight;
            bottomBias /= DisplayHeight;
            */
            float uLeft = srcRect.Left / (float)mTextureSize.Width + leftBias;
            float vTop = srcRect.Top / (float)mTextureSize.Height + topBias;
            float uRight = srcRect.Right / (float)mTextureSize.Width + rightBias;
            float vBottom = srcRect.Bottom / (float)mTextureSize.Height + bottomBias;

            verts[0].Tu = uLeft;
            verts[0].Tv = vTop;

            verts[1].Tu = uRight;
            verts[1].Tv = vTop;

            verts[2].Tu = uLeft;
            verts[2].Tv = vBottom;

            verts[3].Tu = uRight;
            verts[3].Tv = vBottom;

            for (int i = 0; i < 4; i++)
                verts[i].Rhw = 1.0f;
        }

        private void SetVertsColor(CustomVertex.TransformedColoredTextured[] verts, int startIndex, int count)
        {
            int color = Color.ToArgb();

            for (int i = startIndex; i < count; i++)
            {
                verts[i].Color = color;
            }
        }
        private void SetVertsPosition(CustomVertex.TransformedColoredTextured[] verts, int index,
            Rectangle dest, float rotationCenterX, float rotationCenterY)
        {
            SetVertsPosition(verts, index, new RectangleF(dest.X, dest.Y, dest.Width, dest.Height),
                rotationCenterX, rotationCenterY);
        }
        
        private void SetVertsPosition(CustomVertex.TransformedColoredTextured[] verts, int index,
            RectangleF dest, float rotationCenterX, float rotationCenterY)
        {
            float destX = dest.X;
            float destY = dest.Y;
            float destWidth = dest.Width;
            float destHeight = dest.Height;

            mCenterPoint = Origin.CalcF(DisplayAlignment, dest.Size);
            
            destX += rotationCenterX - mCenterPoint.X;
            destY += rotationCenterY - mCenterPoint.Y;

            // Point at (0, 0) local coordinates
            verts[index].X = mRotationCos * (-rotationCenterX) + 
                         mRotationSin * (-rotationCenterY) + destX;

            verts[index].Y = -mRotationSin * (-rotationCenterX) + 
                          mRotationCos * (-rotationCenterY) + destY;

            // Point at (DisplayWidth, 0) local coordinates
            verts[index + 1].X = mRotationCos * (-rotationCenterX + destWidth) + 
                         mRotationSin * (-rotationCenterY) + destX;

            verts[index + 1].Y = -mRotationSin * (-rotationCenterX + destWidth) + 
                          mRotationCos * (-rotationCenterY) + destY;

            // Point at (0, DisplayHeight) local coordinates
            verts[index+2].X = mRotationCos * (-rotationCenterX) +
                         mRotationSin * (-rotationCenterY + destHeight) + destX;

            verts[index+2].Y = (-mRotationSin * (-rotationCenterX) +
                           mRotationCos * (-rotationCenterY + destHeight)) + destY;

            // Point at (DisplayWidth, DisplayHeight) local coordinates
            verts[index + 3].X = mRotationCos * (-rotationCenterX + destWidth) +
                         mRotationSin * (-rotationCenterY + destHeight) + destX;

            verts[index + 3].Y = -mRotationSin * (-rotationCenterX + destWidth) +
                          mRotationCos * (-rotationCenterY + destHeight) + destY;

        }
        [Obsolete("Old DX method.")]
        protected void DrawWithoutVBNoRotation(Rectangle destRect, bool alphaBlend)
        {
            DrawWithoutVBNoRotation(new Rectangle(new Point(0, 0), SurfaceSize), destRect, alphaBlend);
        }
        [Obsolete("Old DX method.")]
        protected void DrawWithoutVBNoRotation(Rectangle srcRect, Rectangle destRect, bool alphaBlend)
        {
            //CustomVertex.TransformedColoredTextured[] verts = new CustomVertex.TransformedColoredTextured[4];
            int startIndex = 0;

            AddRectToVB(mVerts, startIndex, srcRect, destRect);

            mDevice.SetDeviceStateTexture(mTexture.Value);
            mDevice.AlphaBlend =  alphaBlend;

            mDevice.Device.VertexFormat = CustomVertex.TransformedColoredTextured.Format;
            mDevice.Device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, mVerts);

        }

        [Obsolete("Old DX method.")]
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
                   corners[i].X, corners[i].Y, 0.5F, 1.0f, Color.ToArgb(),
                   uv[i].X / (float)mTextureSize.Width, uv[i].Y / (float)mTextureSize.Height);
            }
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
            //DrawWithoutVBNoRotation(srcRect, destRect, true);
            if (mRotationCos != 1.0f)
            {
                float oldcos = mRotationCos;
                float oldsin = mRotationSin;

                SetVertsColor(mExtraVerts, 0, 4);
                SetVertsTextureCoordinates(mExtraVerts, 0, srcRect);
                SetVertsPosition(mExtraVerts, 0, destRect, 0, 0);

                mRotationCos = oldcos;
                mRotationSin = oldsin;
            }
            else
            {
                SetVertsColor(mExtraVerts, 0, 4);
                SetVertsTextureCoordinates(mExtraVerts, 0, srcRect);
                SetVertsPosition(mExtraVerts, 0, destRect, 0, 0);

            }

            //mDevice.SetDeviceStateTexture(mTexture.Value);
            //mDevice.AlphaBlend = alphaBlend;

            //mDevice.VertexFormat = CustomVertex.TransformedColoredTextured.Format;
            //mDevice.Device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, verts);
            //mDevice.Device.DrawIndexedUserPrimitives(PrimitiveType.TriangleList, 0, 6, 2, indices, true, verts);
            mDevice.DrawBuffer.CacheDrawIndexedTriangles(mExtraVerts, mExtraIndices, mTexture.Value, true);

        }
        /// <summary>
        /// This needs to be updated to use the same approach as Draw(Rectangle, Rectangle)
        /// </summary>
        /// <param name="srcRects"></param>
        /// <param name="destRects"></param>
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

            mDevice.SetDeviceStateTexture(mTexture.Value);
            mDevice.AlphaBlend = true;

            mDevice.VertexFormat = CustomVertex.TransformedColoredTextured.Format;
            mDevice.Device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2 * srcRects.Length, verts);
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

        #region --- Surface querying ---

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

        #endregion

        #region --- MDX1_IRenderTarget Members ---

        public override void BeginRender()
        {
            // it looks like Direct3D creates a new surface.
            // so here we will create a new texture, and draw the current texture to it
            // then discard the old one.  
            Texture t = new Texture(mDevice.Device, SurfaceWidth, SurfaceHeight, 1, Usage.None,
                Format.A8R8G8B8, Pool.Managed);

            Direct3D.Surface surfaceTarget = t.GetSurfaceLevel(0);

            mRenderToSurface = new RenderToSurface(mDevice.Device, SurfaceWidth, SurfaceHeight,
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

        #region --- SubSurface stuff ---

        public override SurfaceImpl CarveSubSurface(Surface surf, Rectangle srcRect)
        {
            Rectangle newSrcRect = new Rectangle(
                mSrcRect.Left + srcRect.Left,
                mSrcRect.Top + srcRect.Top,
                srcRect.Width,
                srcRect.Height);

            return new MDX1_Surface(mTexture, newSrcRect);
        }
        public override void SetSourceSurface(SurfaceImpl surf, Rectangle srcRect)
        {
            mTexture.Dispose();
            mTexture = new Ref<Texture>((surf as MDX1_Surface).mTexture);

            mSrcRect = srcRect;

            Direct3D.Surface d3dsurf = mTexture.Value.GetSurfaceLevel(0);

            mTextureSize = new Size(d3dsurf.Description.Width, d3dsurf.Description.Height);

            SetVertsTextureCoordinates(mVerts, 0, mSrcRect);
        }

        #endregion

    }

}