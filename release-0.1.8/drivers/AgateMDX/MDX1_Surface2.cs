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
using System.Text;

namespace ERY.AgateLib.MDX
{
    /*
        /// <summary>
        /// MDX2_Surface2 class, using Direct3DX.Sprite class.  Isn't as flexible as MDX2_Surface 
        /// but may be faster.
        /// </summary>
        public class MDX2_Surface2 : SurfaceImpl
        {
            MDX2_Display mOwner;
            Device mDevice;

            Texture mTexture;
            Bitmap mGDISurface;
            Direct3D.Sprite mSprite;

            string mFileName;

            Size mSize;

            #region --- Creation / Destruction ---

            public MDX2_Surface2(MDX2_Display owner)
            {
                mOwner = owner;
                mDevice = mOwner.MDX2_Device;
                mTexture = null;
                mSprite = null;

            }
            public MDX2_Surface2(MDX2_Display owner, string filename)
            {
                mOwner = owner;
                mDevice = mOwner.MDX2_Device;
                mFileName = filename;

                mGDISurface = Bitmap.FromFile(mFileName) as Bitmap;
                mTexture = new Texture(mDevice, mFileName);

                mSize = new Size(mTexture.GetLevelDescription(0).Width, mTexture.GetLevelDescription(0).Height);

                mSprite = new Direct3D.Sprite(mDevice);
            }

            public override void Dispose()
            {
                if (mTexture != null)
                {
                    mTexture.Dispose();
                    mGDISurface.Dispose();
                    mSprite.Dispose();

                    mTexture = null;
                }
            }

            #endregion

            protected Rectangle getSrcRect()
            {
                return new Rectangle(new Point(0, 0), mSize);
            }

            #region --- Drawing to screen functions ---

            public override void DrawPoints(Point[] dest_pts)
            {
                Point rotationCenter = Origin.Calc(mRotationSpot, DisplaySize);

                //mDevice.RenderState.AlphaBlendEnable = true;
                //mDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                //mDevice.RenderState.DestinationBlend = Blend.InvSourceAlpha;

                //mDevice.SetTexture(0, mTexture);
                //mDevice.TextureState[0].AlphaArgument1 = TextureArgument.TextureColor;
                //mDevice.TextureState[0].AlphaArgument2 = TextureArgument.Diffuse;
                //mDevice.TextureState[0].AlphaOperation = TextureOperation.Modulate;

                mSprite.Begin(SpriteFlags.AlphaBlend);

                mSprite.Transform = Matrix.Scaling((float) mScaleX, (float) mScaleY, 1.0f);

                for (int i = 0; i < dest_pts.Length; i++)
                {
                    Point drawPoint = Origin.Calc(mAlignment, DisplaySize);

                    drawPoint.X = dest_pts[i].X - drawPoint.X;
                    drawPoint.Y = dest_pts[i].Y - drawPoint.Y;

                    mSprite.Draw(mTexture, getSrcRect(), Vector3.Empty, 
                        new Vector3(drawPoint.X, drawPoint.Y, 0), mColor);
                
                }

                mSprite.End();
            }
            public override void Draw()
            {
                Draw(0, 0);
            }
            public override void Draw(Point destPt)
            {

                Point rotationCenter = Origin.Calc(mRotationSpot, DisplaySize);
            
                //mDevice.RenderState.AlphaBlendEnable = true;
                //mDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                //mDevice.RenderState.DestinationBlend = Blend.InvSourceAlpha;

                //mDevice.SetTexture(0, mTexture);
                //mDevice.TextureState[0].AlphaArgument1 = TextureArgument.Texture;
                //mDevice.TextureState[0].AlphaArgument2 = TextureArgument.Diffuse;
                //mDevice.TextureState[0].AlphaOperation = TextureOperation.Modulate;

                mSprite.Begin(SpriteFlags.AlphaBlend);

                mSprite.Transform = Matrix.Scaling((float)mScaleX, (float)mScaleY, 1.0f);

                Point drawPoint = Origin.Calc(mAlignment, DisplaySize);

                drawPoint.X = destPt.X  - drawPoint.X;
                drawPoint.Y = destPt.Y - drawPoint.Y;

                mSprite.Draw(mTexture, getSrcRect(), Vector3.Empty,
                    new Vector3(drawPoint.X, drawPoint.Y, 0), mColor);

                mSprite.End();

            }
            public override void Draw(Rectangle dest_rect)
            {

                mSprite.Begin(SpriteFlags.AlphaBlend);

                float scaleX = (float)dest_rect.Width / mSize.Width;
                float scaleY = (float)dest_rect.Height / mSize.Height;

                mSprite.Transform = Matrix.Scaling(scaleX, scaleY, 1.0f);

                PointF drawPoint = Origin.CalcF(mAlignment, DisplaySize);

                drawPoint.X = (dest_rect.X - (float)drawPoint.X) / scaleX;
                drawPoint.Y = (dest_rect.Y - (float)drawPoint.Y) / scaleY;

                mSprite.Draw(mTexture, getSrcRect(), Vector3.Empty,
                    new Vector3(drawPoint.X, drawPoint.Y, 0), mColor);

                mSprite.End();

            }
            public override void Draw(Rectangle src_rect, Rectangle dest_rect)
            {

                mSprite.Begin(SpriteFlags.AlphaBlend);

                float fudgeX = (float)(Math.Round((float)dest_rect.Width / (float)src_rect.Width - 1));
                float fudgeY = (float)(Math.Round((float)dest_rect.Height / (float)src_rect.Height - 1));
            
                if (fudgeX < 0) fudgeX = 0;
                if (fudgeY < 0) fudgeY = 0;

                float scaleX = ((float)dest_rect.Width + fudgeX ) / src_rect.Width;
                float scaleY = ((float)dest_rect.Height + fudgeY ) / src_rect.Height;

                mSprite.Transform = Matrix.Scaling(scaleX, scaleY, 1.0f);

                PointF drawPoint = Origin.CalcF(mAlignment, DisplaySize);

                drawPoint.X = (dest_rect.X - (float)drawPoint.X) / scaleX;
                drawPoint.Y = (dest_rect.Y - (float)drawPoint.Y) / scaleY;

                //Rectangle srcRect = new Rectangle(src_rect.X + (int)fudgeX, src_rect.Y + (int)fudgeY, src_rect.Width, src_rect.Height);

                mSprite.Draw(mTexture, src_rect, Vector3.Empty,
                    new Vector3(drawPoint.X, drawPoint.Y, 0), mColor);

                mSprite.End(); 
            }
            public override void DrawRects(Rectangle[] src_rects, Rectangle[] dest_rects)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            #endregion
            #region --- Drawing to canvas functions ---

            public override void Draw(Point destPt, CanvasImpl c)
            {
                MDX2_Canvas canvas = (MDX2_Canvas)c;

                canvas.Graphics.DrawImage(mGDISurface, new Rectangle(destPt, mSize));
            }
            public override void Draw(Rectangle dest_rect, CanvasImpl c)
            {
                MDX2_Canvas canvas = (MDX2_Canvas)c;

                canvas.Graphics.DrawImage(mGDISurface, dest_rect);
            }
            public override void Draw(Rectangle src_rect, Rectangle dest_rect, CanvasImpl c)
            {
                MDX2_Canvas canvas = (MDX2_Canvas)c;

                canvas.Graphics.DrawImage(mGDISurface, dest_rect, src_rect, GraphicsUnit.Pixel);
            }
            public override void DrawPoints(Point[] dest_pts, CanvasImpl c)
            {
                base.DrawPoints(dest_pts, c);
            }
            public override void DrawRects(Rectangle[] src_rects, Rectangle[] dest_rects, CanvasImpl c)
            {
                for (int i = 0; i < src_rects.Length; i++)
                    Draw(src_rects[i], dest_rects[i], c);
            }

            #endregion

            public override int SurfaceHeight
            {
                get { return mSize.Height; }
            }
            public override int SurfaceWidth
            {
                get { return mSize.Width; }
            }
            public override Size SurfaceSize
            {
                get
                {
                    return mSize;
                }
            }

            public override CanvasImpl LockSurface()
            {
                throw new Exception("The method or operation is not implemented.");
            }


            //public bool IsRowTransparent(int row, sbyte alphaThreshold)
            //{
            //    Direct3D.Surface surf = mTexture.GetSurfaceLevel(0);

            //    GraphicsBuffer buff = surf.Lock(null, LockFlags.ReadOnly);
            //    Microsoft.DirectX.Generic.GraphicsBuffer<int> ibuff =
            //        buff.GetBuffer<int>();

            //    ibuff.
            //}

            public override bool IsSurfaceBlank()
            {
                return IsSurfaceBlank((int)(mOwner.AlphaThreshold * 255.0));
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

            public override void SaveTo(string frameFile)
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
        */
}
