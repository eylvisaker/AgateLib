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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib.SystemDrawing
{
    class Drawing_Surface : SurfaceImpl, Drawing_IRenderTarget
    {
        #region --- Private variables ---

        Drawing_Display mDisplay;
        Bitmap mImage;

        #endregion

        #region --- Creation / Destruction ---

        public Drawing_Surface(string fileName)
        {
            mDisplay = Display.Impl as Drawing_Display;

            mImage = (Bitmap)Image.FromFile(fileName);

            System.Diagnostics.Debug.Assert(mImage != null);
        }
        public Drawing_Surface(Bitmap image, Rectangle sourceRect)
        {
            mDisplay = Display.Impl as Drawing_Display;

            // copy the pixels from the srcRect
            mImage = new Bitmap(sourceRect.Width, sourceRect.Height);

            Graphics g = Graphics.FromImage(mImage);

            g.DrawImage(image,
                new System.Drawing.Rectangle(0, 0, sourceRect.Width, sourceRect.Height),
                (System.Drawing.Rectangle)sourceRect, GraphicsUnit.Pixel);

            g.Dispose();

        }
        public Drawing_Surface(Size sz)
        {
            mDisplay = Display.Impl as Drawing_Display;

            mImage = new Bitmap(sz.Width, sz.Height);

            System.Diagnostics.Debug.Assert(mImage != null);
        }

        public override void Dispose()
        {
            if (mImage != null)
            {
                mImage.Dispose();
            }

            mImage = null;
        }

        #endregion
        #region --- Protected Drawing helper methods ---

        protected Rectangle SrcRect
        {
            get { return new Rectangle(new Point(0, 0), new Size(mImage.Size)); }
        }
        protected Rectangle DestRect(int dest_x, int dest_y)
        {
            return new Rectangle(dest_x, dest_y, DisplayWidth, DisplayHeight);
        }
        #endregion
        #region --- Draw to Screen Methods ---

        public override void Draw(float destX, float destY, float rotationCenterX, float rotationCenterY)
        {
            mDisplay.CheckInFrame("Surface.Draw");

            PointF destPt = new PointF(destX, destY);


            System.Diagnostics.Debug.Assert(mImage != null);

            Drawing_Display disp = Display.Impl as Drawing_Display;
            Graphics g = disp.FrameGraphics;
            GraphicsState state = g.Save();
            PointF translatePoint = Origin.CalcF(DisplayAlignment, DisplaySize);


            if (DisplaySize.Width < 0)
                translatePoint.X += DisplaySize.Width;

            if (DisplaySize.Height < 0)
                translatePoint.Y += DisplaySize.Height;

            // translate to rotation point, rotate, and translate back.
            // System.Drawing rotates Clockwise!  So we must reverse the
            // rotation angle.
            g.TranslateTransform(-rotationCenterX, -rotationCenterY, MatrixOrder.Append);
            g.RotateTransform(-(float)RotationAngleDegrees, MatrixOrder.Append);
            g.TranslateTransform(rotationCenterX, rotationCenterY, MatrixOrder.Append);


            g.TranslateTransform(destPt.X - translatePoint.X,
                                 destPt.Y - translatePoint.Y, MatrixOrder.Append);

            if (Color != Color.White)
            {
                ImageAttributes imageAttributes = new ImageAttributes();

                ColorMatrix colorMatrix = new ColorMatrix(new float[][]{
                   new float[] { Color.R / 255.0f, 0.0f, 0.0f, 0.0f, 0.0f },
                   new float[] { 0.0f, Color.G / 255.0f, 0.0f, 0.0f, 0.0f },
                   new float[] { 0.0f, 0.0f, Color.B / 255.0f, 0.0f, 0.0f },
                   new float[] { 0.0f, 0.0f, 0.0f, (float)Alpha, 0.0f },
                   new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 1.0f} });

                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                g.DrawImage(mImage, (System.Drawing.Rectangle)DestRect(0, 0),
                    SrcRect.X,
                    SrcRect.Y,
                    SrcRect.Width,
                    SrcRect.Height,
                    GraphicsUnit.Pixel,
                    imageAttributes);

            }
            else
                g.DrawImage(mImage, (System.Drawing.Rectangle)DestRect(0, 0),
                    (System.Drawing.Rectangle)SrcRect, GraphicsUnit.Pixel);

            g.Restore(state);
        }
        public override void Draw(float destX, float destY)
        {
            PointF rotatePoint = Origin.CalcF(RotationCenter, DisplaySize);

            Draw(destX, destY, rotatePoint.X, rotatePoint.Y);

        }
        public override void Draw(Rectangle destRect)
        {
            Draw(SrcRect, destRect);
        }
        public override void Draw(Rectangle srcRect, Rectangle destRect)
        {
            mDisplay.CheckInFrame("Surface.Draw");
            System.Diagnostics.Debug.Assert(mImage != null);

            Graphics g = mDisplay.FrameGraphics;

            g.DrawImage(mImage, (System.Drawing.Rectangle)destRect,
                (System.Drawing.Rectangle)srcRect, GraphicsUnit.Pixel);
        }
        public override void DrawRects(Rectangle[] src_rects, Rectangle[] dest_rects)
        {
            mDisplay.CheckInFrame("Surface.Draw");
            System.Diagnostics.Debug.Assert(mImage != null);

            if (src_rects.Length > dest_rects.Length)
                return;

            for (int i = 0; i < src_rects.Length; i++)
                Draw(src_rects[i], dest_rects[i]);
        }
        public override void DrawPoints(Point[] destPts)
        {
            mDisplay.CheckInFrame("Surface.Draw");
            System.Diagnostics.Debug.Assert(mImage != null);

            Drawing_Display disp = Display.Impl as Drawing_Display;
            Graphics g = disp.FrameGraphics;

            System.Drawing.Point[] pts = new System.Drawing.Point[destPts.Length];

            for (int i = 0; i < pts.Length; i++)
                pts[i] = (System.Drawing.Point)destPts[i];

            g.DrawImage(mImage, pts);
        }

        #endregion
        #region --- Public overriden properties ---

        public override Size SurfaceSize
        {
            get { return new Size(mImage.Size); }
        }

        #endregion


        public override SurfaceImpl CarveSubSurface(Surface surf, Rectangle srcRect)
        {
            return new Drawing_Surface(mImage, srcRect);
        }

        public override bool IsSurfaceBlank()
        {
            return IsSurfaceBlank((int)(Display.AlphaThreshold * 255.0));
        }
        public override bool IsSurfaceBlank(int alphaThreshold)
        {
            for (int i = 0; i < mImage.Height; i++)
            {
                if (IsRowBlank(i) == false)
                    return false;
            }

            return true;
        }

        public override bool IsRowBlank(int row)
        {
            BitmapData bmp = mImage.LockBits(new System.Drawing.Rectangle(0, 0, mImage.Width, mImage.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);


            bool retval = IsRowBlankScanARGB(bmp.Scan0, row, bmp.Width, bmp.Stride,
                (int)(Display.AlphaThreshold * 255.0), 0xff000000, 24);

            mImage.UnlockBits(bmp);

            return retval;
        }
        public override bool IsColumnBlank(int col)
        {
            BitmapData bmp = mImage.LockBits(new System.Drawing.Rectangle(0, 0, mImage.Width, mImage.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);


            bool retval = IsColBlankScanARGB(bmp.Scan0, col, bmp.Height, bmp.Stride,
                (int)(Display.AlphaThreshold * 255.0), 0xff000000, 24);

            mImage.UnlockBits(bmp);

            return retval;
        }

        public override void SaveTo(string filename, ImageFileFormat format)
        {
            ImageFormat drawformat = ImageFormat.Png;

            switch (format)
            {
                case  ImageFileFormat.Png:
                    drawformat = ImageFormat.Png;
                    break;

                case ImageFileFormat.Bmp:
                    drawformat = ImageFormat.Bmp;
                    break;

                case ImageFileFormat.Jpg:
                    drawformat = ImageFormat.Jpeg;
                    break;

            }

            mImage.Save(filename, drawformat);
        }


        #region --- Drawing_IRenderTarget Members ---

        public override void BeginRender()
        {
        }

        public override void EndRender(bool waitVSync)
        {
        }

        public Bitmap BackBuffer
        {
            get { return mImage; }
        }

        #endregion


        [Obsolete("duh")]
        public override void SetSourceSurface(SurfaceImpl surf, Rectangle srcRect)
        {
            mImage.Dispose();

            mImage = new Bitmap(srcRect.Width, srcRect.Height);
            Graphics g = Graphics.FromImage(mImage);

            g.DrawImage((surf as Drawing_Surface).mImage,
                new System.Drawing.Rectangle(System.Drawing.Point.Empty, (System.Drawing.Size)srcRect.Size),
                (System.Drawing.Rectangle)srcRect, GraphicsUnit.Pixel);

            g.Dispose();

        }
    }
}