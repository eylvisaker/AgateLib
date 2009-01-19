//     The contents of this file are subject to the Mozilla Public License
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
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.ImplementationBase;
using AgateLib.Utility;
using AgateLib.WinForms;

using Drawing = System.Drawing;
using ImageFileFormat = AgateLib.DisplayLib.ImageFileFormat;
using Direct3D = Microsoft.DirectX.Direct3D;
using Surface = AgateLib.DisplayLib.Surface;

namespace AgateMDX
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

        PositionColorNormalTexture[] mVerts = new PositionColorNormalTexture[4];
        short[] mIndices = new short[] { 0, 2, 1, 1, 2, 3 };

        PositionColorNormalTexture[] mExtraVerts = new PositionColorNormalTexture[4];
        short[] mExtraIndices = new short[] { 0, 2, 1, 1, 2, 3 };

        #endregion

        #region --- TextureCoordinates structure

        struct TextureCoordinates
        {
            public float Left;
            public float Top;
            public float Right;
            public float Bottom;

            public TextureCoordinates(float left, float top, float right, float bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }
        }

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

            if (mDevice == null)
            {
                throw new Exception("Error: It appears that AgateLib has not been initialized yet.  Have you created a DisplayWindow?");
            }

            LoadFromFile();

            mDevice.Device.DeviceReset += new EventHandler(mDevice_DeviceReset);

            InitVerts();
        }
        public MDX1_Surface(Stream stream)
        {
            mDisplay = Display.Impl as MDX1_Display;
            mDevice = mDisplay.D3D_Device;

            if (mDevice == null)
            {
                throw new Exception("Error: It appears that AgateLib has not been initialized yet.  Have you created a DisplayWindow?");
            }

            LoadFromStream(stream);

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
        public void LoadFromStream(Stream st)
        {
            Drawing.Bitmap bitmap = new Drawing.Bitmap(st);

            mSrcRect = new Rectangle(Point.Empty, Interop.Convert(bitmap.Size));

            // this is the speed issue fix in the debugger found on the net (thezbuffer.com has it documented)
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);

            stream.Position = 0;
            
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

            mTexture = new Ref<Texture>(TextureLoader.FromStream(mDevice.Device, 
                stream, 0, 0, 1, Usage.None,
                format, Pool.Managed, Filter.None, Filter.None, 0x00000000));

            mTextureSize = new Size(mTexture.Value.GetSurfaceLevel(0).Description.Width,
                mTexture.Value.GetSurfaceLevel(0).Description.Height);

            bitmap.Dispose();
        }
        public void LoadFromFile()
        {
            if (string.IsNullOrEmpty(mFileName))
                return;

            string path = mFileName;
            Drawing.Bitmap bitmap = new Drawing.Bitmap(path);

            mSrcRect = new Rectangle(Point.Empty, Interop.Convert(bitmap.Size));
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

        public override Gradient ColorGradient
        {
            get
            {
                return base.ColorGradient;
            }
            set
            {
                base.ColorGradient = value;

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

           DrawWithoutVB(destX, destY, mSrcRect, rotation.X, rotation.Y, alphaBlend);
        }
        protected void DrawWithoutVB(float destX, float destY, Rectangle srcRect,
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

            if (TesselateFactor == 1)
            {
                SetVertsTextureCoordinates(mVerts, 0, srcRect);
                SetVertsColor(mVerts, 0, 4);
                SetVertsPosition(mVerts, 0, 
                    new RectangleF(destX, destY, 
                                   srcRect.Width * (float)ScaleWidth, 
                                   srcRect.Height * (float)ScaleHeight),
                    rotationCenterX, rotationCenterY);

                mDevice.DrawBuffer.CacheDrawIndexedTriangles(mVerts, mIndices, mTexture.Value, alphaBlend);
            }
            else
            {
                TextureCoordinates texCoords = GetTextureCoordinates(mSrcRect);
                float texWidth = texCoords.Right - texCoords.Left;
                float texHeight = texCoords.Bottom - texCoords.Top;

                float displayWidth = DisplayWidth / (float)TesselateFactor;
                float displayHeight = DisplayHeight / (float)TesselateFactor;

                for (int j = 0; j < TesselateFactor; j++)
                {
                    TextureCoordinates coords = texCoords;
                    coords.Top = texCoords.Top + j * texHeight / TesselateFactor;
                    coords.Bottom = coords.Top + texHeight / TesselateFactor;

                    for (int i = 0; i < TesselateFactor; i++)
                    {
                        coords.Left = texCoords.Left + i * texWidth / TesselateFactor;
                        coords.Right = coords.Left + texWidth / TesselateFactor;

                        float dx = destX + i * displayWidth * mRotationCos + j * displayHeight * mRotationSin;
                        float dy = destY - i * displayWidth * mRotationSin + j * displayHeight * mRotationCos; 

                        SetVertsPosition(mExtraVerts, 0,
                            new RectangleF(dx, dy,
                                           displayWidth, displayHeight),
                                           rotationCenterX, rotationCenterY);
                        SetVertsColor(mExtraVerts, 0, 4,
                            i / (double)TesselateFactor, j / (double)TesselateFactor, 1.0 / TesselateFactor, 1.0 / TesselateFactor);

                        SetVertsTextureCoordinates(mExtraVerts, 0, coords);

                        mDevice.DrawBuffer.CacheDrawIndexedTriangles(
                            mExtraVerts, mIndices, mTexture.Value, alphaBlend);
                    }
                }
            }

        }

        private void SetVertsTextureCoordinates(PositionColorNormalTexture[] verts, int startIndex,
            Rectangle srcRect)
        {
            TextureCoordinates texCoords = GetTextureCoordinates(srcRect);

            SetVertsTextureCoordinates(verts, startIndex, texCoords);
        }

        private void SetVertsTextureCoordinates(PositionColorNormalTexture[] verts, int startIndex,
            RectangleF srcRect)
        {
            TextureCoordinates texCoords = GetTextureCoordinates(srcRect);

            SetVertsTextureCoordinates(verts, startIndex, texCoords);
        }
        private void SetVertsTextureCoordinates(PositionColorNormalTexture[] verts, int startIndex, 
            TextureCoordinates texCoords)
        {
            verts[startIndex].Tu = texCoords.Left;
            verts[startIndex].Tv = texCoords.Top;

            verts[startIndex + 1].Tu = texCoords.Right;
            verts[startIndex + 1].Tv = texCoords.Top;

            verts[startIndex + 2].Tu = texCoords.Left;
            verts[startIndex + 2].Tv = texCoords.Bottom;

            verts[startIndex + 3].Tu = texCoords.Right;
            verts[startIndex + 3].Tv = texCoords.Bottom;
        }

        private TextureCoordinates GetTextureCoordinates(Rectangle srcRect)
        {
            return GetTextureCoordinates(new RectangleF(
                srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height));
        }
        
        private TextureCoordinates GetTextureCoordinates(RectangleF srcRect)
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

            TextureCoordinates texCoords = new TextureCoordinates(uLeft, vTop, uRight, vBottom);
            return texCoords;
        }

        private void SetVertsColor(PositionColorNormalTexture[] verts, int startIndex, int count)
        {
            verts[startIndex].Color = ColorGradient.TopLeft.ToArgb();
            verts[startIndex + 1].Color = ColorGradient.TopRight.ToArgb();
            verts[startIndex + 2].Color = ColorGradient.BottomLeft.ToArgb();
            verts[startIndex + 3].Color = ColorGradient.BottomRight.ToArgb();
        }
        private void SetVertsColor(PositionColorNormalTexture[] verts, int startIndex, int count,
            double x, double y, double width, double height)
        {
            verts[startIndex].Color = ColorGradient.Interpolate(x, y).ToArgb();
            verts[startIndex + 1].Color = ColorGradient.Interpolate(x + width, y).ToArgb();
            verts[startIndex + 2].Color = ColorGradient.Interpolate(x, y + height).ToArgb();
            verts[startIndex + 3].Color = ColorGradient.Interpolate(x + width, y + height).ToArgb();
        }
        private void SetVertsPosition(PositionColorNormalTexture[] verts, int index,
            Rectangle dest, float rotationCenterX, float rotationCenterY)
        {
            SetVertsPosition(verts, index, new RectangleF(dest.X, dest.Y, dest.Width, dest.Height),
                rotationCenterX, rotationCenterY);
        }

        private void SetVertsPosition(PositionColorNormalTexture[] verts, int index,
            RectangleF dest, float rotationCenterX, float rotationCenterY)
        {
            float destX = dest.X -0.5f;
            float destY = dest.Y -0.5f;
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
            verts[index + 2].X = mRotationCos * (-rotationCenterX) +
                         mRotationSin * (-rotationCenterY + destHeight) + destX;

            verts[index + 2].Y = (-mRotationSin * (-rotationCenterX) +
                           mRotationCos * (-rotationCenterY + destHeight)) + destY;

            // Point at (DisplayWidth, DisplayHeight) local coordinates
            verts[index + 3].X = mRotationCos * (-rotationCenterX + destWidth) +
                         mRotationSin * (-rotationCenterY + destHeight) + destX;

            verts[index + 3].Y = -mRotationSin * (-rotationCenterX + destWidth) +
                          mRotationCos * (-rotationCenterY + destHeight) + destY;

            for (int i = 0; i < 4; i++)
            {
                verts[index + i].nx = 0;
                verts[index + i].ny = 0;
                verts[index + i].nz = -1;
            }
        }

        [Obsolete("Old DX method.")]
        protected void DrawWithoutVBNoRotation(RectangleF destRect, bool alphaBlend)
        {
            DrawWithoutVBNoRotation(new RectangleF(new PointF(0, 0), (SizeF)SurfaceSize),
                destRect, alphaBlend);
        }
        [Obsolete("Old DX method.")]
        protected void DrawWithoutVBNoRotation(RectangleF srcRect, RectangleF destRect, bool alphaBlend)
        {
            //CustomVertex.TransformedColoredTextured[] verts = new CustomVertex.TransformedColoredTextured[4];
            int startIndex = 0;

            AddRectToVB(mVerts, startIndex, srcRect, destRect);

            mDevice.SetDeviceStateTexture(mTexture.Value);
            mDevice.AlphaBlend =  alphaBlend;

            mDevice.Device.VertexFormat = CustomVertex.PositionColoredTextured.Format;
            mDevice.Device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, mVerts);

        }
        
        
        [Obsolete("Old DX method.")]
        private void AddRectToVB(PositionColorNormalTexture[] verts, int startIndex, 
                                RectangleF srcRect, RectangleF destRect)
        {
            // find center
            PointF centerpt = Origin.CalcF(DisplayAlignment, DisplaySize);

            float left = destRect.Left - 0.5f;
            float top = destRect.Top - 0.5f;
            float right = destRect.Right - 0.5f;// +(float)Math.Floor(destRect.Width / (float)srcRect.Width);
            float bottom = destRect.Bottom - 0.5f;// +(float)Math.Floor(destRect.Height / (float)srcRect.Height);

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
                verts[startIndex + i] = new PositionColorNormalTexture(
                   corners[i].X, corners[i].Y, 0.0F, Color.ToArgb(),
                   uv[i].X / (float)mTextureSize.Width, uv[i].Y / (float)mTextureSize.Height,
                   0, 0, -1);
            }
        }
        

        #endregion
        #region --- Drawing to screen functions ---

        public override void Draw(float destX, float destY)
        {
            DrawWithoutVB(destX, destY, true);            
        }

        public override void Draw(float destX, float destY, Rectangle srcRect, float rotationCenterX, float rotationCenterY)
        {
            DrawWithoutVB(destX, destY, srcRect, rotationCenterX, rotationCenterY, true);
        }
        public override void Draw(float destX, float destY, float rotationCenterX, float rotationCenterY)
        {
            DrawWithoutVB(destX, destY, mSrcRect, rotationCenterX, rotationCenterY, true);
        }
        public override void Draw(Rectangle destRect)
        {
            Draw(new Rectangle(0, 0, mSrcRect.Width, mSrcRect.Height), destRect);
        }
        public override void Draw(RectangleF srcRect, RectangleF destRect)
        {
            srcRect.X += mSrcRect.X;
            srcRect.Y += mSrcRect.Y;

            //DrawWithoutVBNoRotation(srcRect, destRect, true);
            //if (mRotationCos != 1.0f)
            //{
            //    float oldcos = mRotationCos;
            //    float oldsin = mRotationSin;

            //    SetVertsColor(mExtraVerts, 0, 4);
            //    SetVertsTextureCoordinates(mExtraVerts, 0, srcRect);
            //    SetVertsPosition(mExtraVerts, 0, destRect, 0, 0);

            //    mRotationCos = oldcos;
            //    mRotationSin = oldsin;
            //}
            //else
            //{
            if (TesselateFactor == 1)
            {
                SetVertsColor(mExtraVerts, 0, 4);
                SetVertsTextureCoordinates(mExtraVerts, 0, srcRect);
                SetVertsPosition(mExtraVerts, 0, destRect, 0, 0);

                mDevice.DrawBuffer.CacheDrawIndexedTriangles(mExtraVerts, mExtraIndices, mTexture.Value, true);
            }
            else
            {
                SetVertsColor(mExtraVerts, 0, 4);

                RectangleF src = new RectangleF();
                RectangleF dest = new RectangleF();

                for (int j = 0; j < TesselateFactor; j++)
                {
                    src.Y = srcRect.Top + j * srcRect.Height / (float)TesselateFactor;
                    src.Height = srcRect.Height / (float)TesselateFactor;

                    dest.Y = destRect.Top + j * destRect.Height / (float)TesselateFactor;
                    dest.Height = destRect.Height / (float)TesselateFactor;

                    for (int i = 0; i < TesselateFactor; i++)
                    {
                        src.X = srcRect.X + i * srcRect.Width / (float)TesselateFactor;
                        src.Width = srcRect.Width / (float)TesselateFactor;

                        dest.X = destRect.X + i * destRect.Width / (float)TesselateFactor;
                        dest.Width = destRect.Width / (float)TesselateFactor;


                        SetVertsColor(mExtraVerts, 0, 4, 
                            i / (double)TesselateFactor, j / (double)TesselateFactor, 
                            1.0 / TesselateFactor, 1.0 / TesselateFactor);

                        SetVertsTextureCoordinates(mExtraVerts, 0, src);
                        SetVertsPosition(mExtraVerts, 0, dest, 0, 0);

                        mDevice.DrawBuffer.CacheDrawIndexedTriangles(mExtraVerts, mExtraIndices, mTexture.Value, true);
                    }
                }
            }
        }
        
        /// <summary>
        /// This needs to be updated to use the same approach as Draw(Rectangle, Rectangle)
        /// </summary>
        /// <param name="srcRects"></param>
        /// <param name="destRects"></param>
        public override void DrawRects(RectangleF[] srcRects, RectangleF[] destRects, int start, int length)
        {

            PositionColorNormalTexture[] verts =
                new PositionColorNormalTexture[srcRects.Length * 4];
            short[] indices = new short[srcRects.Length * 6];

            int startIndex = 0;
            int indexIndex = 0;

            for (int i = start; i < start + length; i++)
            {
                AddRectToVB(verts, startIndex, srcRects[i], destRects[i]);

                indices[indexIndex] = (short)startIndex;
                indices[indexIndex + 1] = (short)(startIndex + 1);
                indices[indexIndex + 2] = (short)(startIndex + 2);
                indices[indexIndex + 3] = (short)(startIndex + 2);
                indices[indexIndex + 4] = (short)(startIndex + 1);
                indices[indexIndex + 5] = (short)(startIndex + 3);


                startIndex += 4;
                indexIndex += 6;
            }

            mDevice.DrawBuffer.CacheDrawIndexedTriangles(verts, indices, mTexture.Value, true);

            //mDevice.SetDeviceStateTexture(mTexture.Value);
            //mDevice.AlphaBlend = true;

            //mDevice.VertexFormat = CustomVertex.TransformedColoredTextured.Format;
            ////mDevice.Device.DrawUserPrimitives(PrimitiveType.TriangleList, 2 * srcRects.Length, verts);
            //mDevice.Device.DrawIndexedUserPrimitives(PrimitiveType.TriangleList, 0, indexIndex,
            //    srcRects.Length * 2, indices, true, verts);
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
            GraphicsStream stm = surf.LockRectangle(
                Interop.Convert(mSrcRect), LockFlags.ReadOnly, out stride);

            bool retval = this.IsRowBlankScanARGB(stm.InternalData, row, this.SurfaceWidth,
                stride, (int)(Display.AlphaThreshold * 255.0), 0xff000000, 24);
            
            surf.UnlockRectangle();

            return retval;
        }

        public override bool IsColumnBlank(int col)
        {
            Direct3D.Surface surf = mTexture.Value.GetSurfaceLevel(0);

            int stride;
            GraphicsStream stm = surf.LockRectangle(Interop.Convert(mSrcRect),
                LockFlags.ReadOnly, out stride);

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

            SurfaceLoader.Save(frameFile, d3dformat, surf, Interop.Convert(mSrcRect));
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

            Display.Clear();

            Draw();

            mTexture.Dispose();
            mTexture = new Ref<Texture>(t);

        }
        public override void EndRender()
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


        public override PixelBuffer ReadPixels(PixelFormat format, Rectangle rect)
        {
            Direct3D.Surface surf = mTexture.Value.GetSurfaceLevel(0);

            rect.X += mSrcRect.X;
            rect.Y += mSrcRect.Y;

            int stride;
            int pixelPitch = mDisplay.GetPixelPitch(surf.Description.Format);

            PixelFormat pixelFormat = mDisplay.GetPixelFormat(surf.Description.Format);

            if (format == PixelFormat.Any)
                format = pixelFormat;

            GraphicsStream stm = surf.LockRectangle(
                new Drawing.Rectangle(0, 0, mTextureSize.Width, mTextureSize.Height),
                LockFlags.ReadOnly, out stride);

            byte[] array = new byte[SurfaceWidth * SurfaceHeight * pixelPitch];
            int length = SurfaceWidth * pixelPitch;
            int index = 0;

            unsafe
            {
                byte* ptr = (byte*)stm.InternalDataPointer;

                for (int i = rect.Top; i < rect.Bottom; i++)
                {
                    // hack if the size requested is too large.
                    if (i >= mTextureSize.Height)
                        break;

                    //IntPtr ptr = (IntPtr)((int)stm.InternalData + i * stride + rect.Left * pixelPitch);
                    IntPtr mptr = (IntPtr)(ptr + i * stride + rect.Left * pixelPitch);

                    Marshal.Copy(mptr, array, index, length);

                    index += length;
                }
            }

            surf.UnlockRectangle();
            surf.Dispose();

            return new PixelBuffer(format, rect.Size, array, pixelFormat);

        }

        public override void WritePixels(PixelBuffer buffer)
        {
            Direct3D.Surface surf = mTexture.Value.GetSurfaceLevel(0);

            int pitch;
            int pixelPitch = mDisplay.GetPixelPitch(surf.Description.Format);
            PixelFormat pixelFormat = mDisplay.GetPixelFormat(surf.Description.Format);

            surf.Dispose();

            GraphicsStream stm = mTexture.Value.LockRectangle(0, 0, out pitch);

            if (buffer.PixelFormat != pixelFormat)
                buffer = buffer.ConvertTo(pixelFormat);

            unsafe
            {
                for (int i = 0; i < SurfaceHeight; i++)
                {
                    int startIndex = buffer.GetPixelIndex(0, i);
                    int rowStride = buffer.RowStride;
                    IntPtr dest = (IntPtr)((byte*)stm.InternalData + i * pitch);

                    Marshal.Copy(buffer.Data, startIndex, dest, rowStride);
                }
            }

            mTexture.Value.UnlockRectangle(0);
            
        }
        // TODO: Test this method:
        public override void WritePixels(PixelBuffer buffer, Point startPoint)
        {
            Direct3D.Surface surf = mTexture.Value.GetSurfaceLevel(0);
            Rectangle updateRect = new Rectangle(startPoint, buffer.Size);

            int pitch;
            int pixelPitch = mDisplay.GetPixelPitch(surf.Description.Format);
            PixelFormat pixelFormat = mDisplay.GetPixelFormat(surf.Description.Format);

            surf.Dispose();

            GraphicsStream stm = mTexture.Value.LockRectangle(0,Interop.Convert(updateRect), 0, out pitch);

            if (buffer.PixelFormat != pixelFormat)
                buffer = buffer.ConvertTo(pixelFormat);

            unsafe
            {
                for (int i = updateRect.Top; i < updateRect.Bottom; i++)
                {
                    int startIndex = buffer.GetPixelIndex(0, i);
                    int rowStride = buffer.RowStride;
                    IntPtr dest = (IntPtr)((byte*)stm.InternalData + i * pitch + updateRect.Left * pixelPitch);

                    Marshal.Copy(buffer.Data, startIndex, dest, rowStride);
                }
            }

            mTexture.Value.UnlockRectangle(0);
        }

    }

}
