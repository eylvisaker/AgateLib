using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Direct3D = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using CustomVertex = Microsoft.DirectX.Direct3D.CustomVertex;
using Generic = Microsoft.DirectX.Generic;

namespace ERY.GameLibrary
{
    public class MDX2_Surface : SurfaceImpl
    {
        #region --- Private Variables ---

        Surface mOwner;
        MDX2_Display mDisplay;
        Direct3D.Device mDevice;

        Texture mTexture;
        Bitmap mGDISurface;
        string mFileName;

        VertexBuffer mVertices;
        int mVertexBufferLength;

        Size mSize;

        #endregion

        #region --- Creation / Destruction ---

        public MDX2_Surface(Surface owner)
        {
            mOwner = owner;
            mTexture = null;

            mDisplay = Display.Impl as MDX2_Display;
            mDevice = mDisplay.D3D_Device;
        }
        public MDX2_Surface(Surface owner, string fileName)
        {
            mOwner = owner;
            mFileName = fileName;

            mDisplay = Display.Impl as MDX2_Display;
            mDevice = mDisplay.D3D_Device;

            load();

            mDevice.DeviceReset += new EventHandler(mDevice_DeviceReset);
        }
        public MDX2_Surface(Surface owner, Size size)
        {
            mOwner = owner;
            mSize = size;

            mDisplay = Display.Impl as MDX2_Display;
            mDevice = mDisplay.D3D_Device;

            mGDISurface = new Bitmap(size.Width, size.Height);
            mTexture = Texture.FromBitmap(mDevice, mGDISurface, Usage.None, Pool.Managed);

            //mVertices = CreateVertexBuffer(0, 0);

        }

        public override void Dispose()
        {
            if (mTexture != null)
            {
                mTexture.Dispose();
            }
            if (mVertices != null)
            {
                mVertices.Dispose();
            }
        }


        public void load()
        {
            if (string.IsNullOrEmpty(mFileName))
                return;

            mGDISurface = new Bitmap(System.IO.Path.Combine(Display.ImagePath, mFileName));
            mTexture = new Texture(mDevice, System.IO.Path.Combine(Display.ImagePath, mFileName));


            mSize = mGDISurface.Size;


        }

        #endregion
        #region --- Events and event handlers ---

        public void mDevice_DeviceReset(object sender, EventArgs e)
        {
            load();
        }

        #endregion

        #region --- Drawing Helper functions ---

        protected void RotatePointInPlace(ref Point pt, Point rotation)
        {

            Point local = new Point(pt.X - rotation.X, pt.Y - rotation.Y);

            double cos = Math.Cos(mOwner.RotationAngle);
            double sin = Math.Sin(mOwner.RotationAngle);

            pt.X = (int)(cos * local.X + sin * local.Y);
            pt.Y = (int)(-sin * local.X + cos * local.Y);

            pt.X += rotation.X;
            pt.Y += rotation.Y;

        }
        protected void TranslatePointInPlace(ref Point pt, Point origin)
        {
            pt.X -= origin.X;
            pt.Y -= origin.Y;
        }
        protected void TranslatePointInPlace(ref PointF pt, Point origin)
        {
            pt.X -= origin.X;
            pt.Y -= origin.Y;
        }

        protected VertexBuffer CreateVertexBuffer(int dest_x, int dest_y)
        {
            mDevice.VertexFormat = CustomVertex.TransformedColoredTextured.Format;


            CustomVertex.TransformedColoredTextured[] verts =
                new CustomVertex.TransformedColoredTextured[4];

            verts[0] = new CustomVertex.TransformedColoredTextured(
                   (float)dest_x, (float)dest_y, 0.5F, 1, mOwner.Color.ToArgb(), 0, 0);

            verts[1] = new CustomVertex.TransformedColoredTextured(
                   (float)dest_x + mOwner.DisplayWidth, (float)dest_y, 0.5F, 1,
                   mOwner.Color.ToArgb(), 1, 0);

            verts[2] = new CustomVertex.TransformedColoredTextured(
                   (float)dest_x, (float)dest_y + mOwner.DisplayHeight, 0.5F, 1,
                   mOwner.Color.ToArgb(), 0, 1);

            verts[3] = new CustomVertex.TransformedColoredTextured(
                   (float)dest_x + mOwner.DisplayWidth, (float)dest_y + mOwner.DisplayHeight, 0.5F, 1,
                   mOwner.Color.ToArgb(), 1, 1);

            /*
            VertexBuffer buf = new VertexBuffer(
                mDevice,
                typeof(Direct3D.CustomVertex.TransformedColoredTextured), 
                verts.Length, 
                mDevice, 
                0,
                Direct3D.CustomVertex.TransformedColoredTextured.Format, 
                Pool.Default
                );
            */

            mVertexBufferLength = verts.Length * VertexInformation.GetFormatSize(
                CustomVertex.TransformedColoredTextured.Format);
            /*
            VertexBuffer buf = new VertexBuffer(
                mDevice,
                mVertexBufferLength,
                0,
                CustomVertex.TransformedColoredTextured.Format,
                Pool.Default,
                null ); 
            */
            VertexBuffer buf = VertexBuffer.CreateGeneric<CustomVertex.TransformedColoredTextured>
                (mDevice, verts.Length, 0, CustomVertex.TransformedColoredTextured.Format,
                Pool.Default, null);

            GraphicsBuffer stm = buf.Lock(0, 0, 0);
            stm.GetBuffer<CustomVertex.TransformedColoredTextured>().Write(verts);
            buf.Unlock();

            return buf;
        }
        protected void UpdateVertexBuffer(int dest_x, int dest_y)
        {
            if (mVertices == null)
            {
                mVertices = CreateVertexBuffer(0, 0);
            }

            Generic.GraphicsBuffer<CustomVertex.TransformedColoredTextured> verts =
                mVertices.Lock(0, mVertexBufferLength, 0).GetBuffer<CustomVertex.TransformedColoredTextured>();

            /*Direct3D.CustomVertex.TransformedColoredTextured[] verts =
                new Direct3D.CustomVertex.TransformedColoredTextured[4];
            */
            // find center
            Point centerpt = Origin.Calc(mOwner.DisplayAlignment, mOwner.DisplaySize);
            Point rotation = Origin.Calc(mOwner.RotationCenter, mOwner.DisplaySize);

            rotation.X += dest_x;
            rotation.Y += dest_y;


            Point[] corners = new Point[4] 
            {   
                new Point(dest_x, dest_y),
                new Point(dest_x + mOwner.DisplayWidth, dest_y),
                new Point(dest_x, dest_y + mOwner.DisplayHeight),
                new Point(dest_x + mOwner.DisplayWidth, dest_y + mOwner.DisplayHeight) 
            };

            PointF[] uv = new PointF[4]
            {
                new PointF(0, 0),
                new PointF(1, 0),
                new PointF(0, 1),
                new PointF(1, 1),
            };

            if (mOwner.DisplayWidth < 0)
            {
                // swap points
                for (int i = 0; i < 4; i += 2)
                {
                    Point t = corners[i];
                    corners[i] = corners[i + 1];
                    corners[i + 1] = t;

                    PointF uvt = uv[i];
                    uv[i] = uv[i + 1];
                    uv[i + 1] = uvt;
                }

                for (int i = 0; i < 4; i++)
                    corners[i].X -= mOwner.DisplayWidth;
            }
            if (mOwner.DisplayHeight < 0)
            {
                // swap points
                for (int i = 0; i < 2; i += 1)
                {
                    Point t = corners[i];
                    corners[i] = corners[i + 2];
                    corners[i + 2] = t;

                    PointF uvt = uv[i];
                    uv[i] = uv[i + 2];
                    uv[i + 2] = uvt;
                }

                for (int i = 0; i < 4; i++)
                    corners[i].Y -= mOwner.DisplayHeight;

            }

            // do rotation and translation
            if (mOwner.RotationAngle != 0)
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


            for (int i = 0; i < 4; i++)
            {
                verts[i] = new Direct3D.CustomVertex.TransformedColoredTextured(
                   (float)corners[i].X, (float)corners[i].Y, 0.5F, 1, mOwner.Color.ToArgb(), uv[i].X, uv[i].Y);
            }

            //write.Write(verts);

            mVertices.Unlock();
        }
        protected void UpdateVertexBufferNoRotation(Rectangle dest_rect)
        {
            UpdateVertexBufferNoRotation(new Rectangle(new Point(0, 0), SurfaceSize), dest_rect);
        }
        protected void UpdateVertexBufferNoRotation(Rectangle src_rect, Rectangle dest_rect)
        {
            Generic.GraphicsBuffer
                <CustomVertex.TransformedColoredTextured> verts =
                mVertices.Lock(0, mVertexBufferLength, 0).
                GetBuffer<CustomVertex.TransformedColoredTextured>();

            /*Direct3D.CustomVertex.TransformedColoredTextured[] verts =
                new Direct3D.CustomVertex.TransformedColoredTextured[4];
            */
            // find center
            Point centerpt = Origin.Calc(mOwner.DisplayAlignment, mOwner.DisplaySize);

            float left = dest_rect.Left;
            float top = dest_rect.Top;
            float right = dest_rect.Right + (float)Math.Round(dest_rect.Width / (float)src_rect.Width);
            float bottom = dest_rect.Bottom + (float)Math.Round(dest_rect.Height / (float)src_rect.Height);

            PointF[] corners = new PointF[4] 
            {   
                new PointF(left, top),
                new PointF(right, top),
                new PointF(left, bottom),
                new PointF(right, bottom) 
            };

            float tlBias = 0.75f;
            float brBias = -0.75f;

            PointF[] uv = new PointF[4]
            {
                new PointF(src_rect.Left + tlBias, src_rect.Top + tlBias),
                new PointF(src_rect.Right + brBias, src_rect.Top + tlBias),
                new PointF(src_rect.Left + tlBias, src_rect.Bottom + brBias),
                new PointF(src_rect.Right + brBias, src_rect.Bottom + brBias) 
            };


            for (int i = 0; i < 4; i++)
            {
                TranslatePointInPlace(ref corners[i], centerpt);
            }



            for (int i = 0; i < 4; i++)
            {
                verts[i] = new Direct3D.CustomVertex.TransformedColoredTextured(
                   corners[i].X - 0.5f, corners[i].Y - 0.5f, 0.5F, 1, mOwner.Color.ToArgb(),
                   uv[i].X / (float)mSize.Width, uv[i].Y / (float)mSize.Height);
            }

            //write.Write(verts);

            mVertices.Unlock();
        }

        #endregion
        #region --- Drawing to screen functions ---

        public override void Draw(Point dest_pt)
        {
            UpdateVertexBuffer(dest_pt.X, dest_pt.Y);

            mDevice.RenderState.AlphaBlendEnable = true;
            mDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            mDevice.RenderState.DestinationBlend = Blend.InvSourceAlpha;

            mDevice.SetTexture(0, mTexture);

            mDevice.TextureState[0].AlphaArgument1 = TextureArgument.Texture;
            mDevice.TextureState[0].AlphaArgument2 = TextureArgument.Diffuse;
            mDevice.TextureState[0].AlphaOperation = TextureOperation.Modulate;

            mDevice.SetStreamSource(0, mVertices, 0);
            mDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);

        }
        public override void Draw(Rectangle dest_rect)
        {
            Draw(new Rectangle(0, 0, mSize.Width, mSize.Height), dest_rect);
        }
        public override void Draw(Rectangle src_rect, Rectangle dest_rect)
        {
            UpdateVertexBufferNoRotation(src_rect, dest_rect);

            mDevice.RenderState.AlphaBlendEnable = true;
            mDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            mDevice.RenderState.DestinationBlend = Blend.InvSourceAlpha;

            mDevice.SetTexture(0, mTexture);
            mDevice.TextureState[0].AlphaArgument1 = TextureArgument.Texture;
            mDevice.TextureState[0].AlphaArgument2 = TextureArgument.Diffuse;
            mDevice.TextureState[0].AlphaOperation = TextureOperation.Modulate;

            mDevice.SetStreamSource(0, mVertices, 0);
            mDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
        }
        public override void DrawRects(Rectangle[] src_rects, Rectangle[] dest_rects)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
        #region --- Drawing to another surface functions ---
        /*
        public override void Draw(int dest_x, int dest_y, Canvas c)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override void Draw(Point dest_pt, Canvas c)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override void Draw(Rectangle dest_rect, Canvas c)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override void Draw(Rectangle src_rect, Rectangle dest_rect, Canvas c)
        {
            D3D_Canvas canvas = (D3D_Canvas)c;

            canvas.Graphics.DrawImage(mGDISurface, dest_rect, src_rect, GraphicsUnit.Pixel);

        }
        public override void DrawPoints(Point[] dest_pts, Canvas c)
        {
            base.DrawPoints(dest_pts, c);
        }
        public override void DrawRects(Rectangle[] src_rects, Rectangle[] dest_rects, Canvas c)
        {
            throw new Exception("The method or operation is not implemented.");
        }
*/
        public override void Draw(Point destPt, CanvasImpl c)
        {
            MDX2_Canvas canvas = (MDX2_Canvas)c;

            canvas.Graphics.DrawImage(mGDISurface, new Rectangle(new Point(destPt.X, destPt.Y), mSize));
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
        #region --- Surface locking ---

        public override CanvasImpl LockSurface()
        {
            if (mGDISurface == null)
            {
                mGDISurface = new Bitmap(mSize.Width, mSize.Height);
            }


            return new MDX2_Canvas(this, Graphics.FromImage(mGDISurface));
        }
        public override void UnlockSurface(CanvasImpl surfaceCanvas)
        {
            //mTexture = new Texture(mDevice, mGDISurface, Usage.Dynamic, Pool.Default);
            mTexture = Texture.FromBitmap(mDevice, mGDISurface, Usage.None, Pool.Managed);
        }

        #endregion

        #region --- Overriden public properties ---

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
            get { return mSize; }
        }
        #endregion

        // do these:
        public override bool IsSurfaceBlank()
        {
            return IsSurfaceBlank((int)(Display.AlphaThreshold * 255.0));
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

        public override void SaveTo(string frameFile)
        {
            return;
        }

    }

}
