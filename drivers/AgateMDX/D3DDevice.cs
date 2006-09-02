using System;
using System.Collections.Generic;
using System.Text;
using Direct3D = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using CustomVertex = Microsoft.DirectX.Direct3D.CustomVertex;

namespace ERY.AgateLib.MDX
{
    public class D3DDevice : IDisposable
    {
        private Device mDevice;
        private Texture mLastTexture;
        private MDX1_IRenderTarget mRenderTarget;
        private DrawBuffer mDrawBuffer;

        private VertexFormats mVertexFormat;
        private bool mAlphaBlend;
        private TextureArgument mAlphaArgument1;
        private TextureArgument mAlphaArgument2;
        private TextureOperation mAlphaOperation;

        private Matrix mWorld2D;

        //VertexBuffer mSurfaceVB;
        //const int NumVertices = 1000;
        //int mSurfaceVBPointer = 0;

        //readonly int SurfaceVBSize = NumVertices * CustomVertex.TransformedColoredTextured.StrideSize;

        public D3DDevice(Device device)
        {
            mDevice = device;

            mDrawBuffer = new DrawBuffer(this);
        }
        ~D3DDevice()
        {
            Dispose(false);
        }



        public void Dispose()
        {
            Dispose(true);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
                GC.SuppressFinalize(this);

            if (mDevice != null)
            {
                mDevice.Dispose();
                mDevice = null;
            }
            //if (mSurfaceVB != null)
            //{
            //    mSurfaceVB.Dispose();
            //    mSurfaceVB = null;
            //}
        }

        //private void CreateSurfaceVB()
        //{
        //    //mSurfaceVB = new VertexBuffer(mDevice, SurfaceVBSize,
        //    //    Usage.WriteOnly | Usage.Dynamic, CustomVertex.TransformedColoredTextured.Format,
        //    //     Pool.Default);
        //}
        public Device Device
        {
            get { return mDevice; }
        }

        public MDX1_IRenderTarget RenderTarget
        {
            get { return mRenderTarget; }
            set { mRenderTarget = value; }
        }
        public DrawBuffer DrawBuffer
        {
            get { return mDrawBuffer; }
        }
        public VertexFormats VertexFormat
        {
            get { return mVertexFormat; }
            set
            {
                if (mVertexFormat != value)
                {
                    mVertexFormat = value;
                    mDevice.VertexFormat = value;
                }
            }
        }
        public bool AlphaBlend
        {
            get { return mAlphaBlend; }
            set
            {
                if (value != mAlphaBlend)
                {
                    mAlphaBlend = value;
                    mDevice.RenderState.AlphaBlendEnable = value;
                }
            }
        }
        public TextureArgument AlphaArgument1
        {
            get { return mAlphaArgument1; }
            set
            {
                if (value != mAlphaArgument1)
                {
                    mAlphaArgument1 = value;
                    mDevice.TextureState[0].AlphaArgument1 = value;
                }

            }
        }
        public TextureArgument AlphaArgument2
        {
            get { return mAlphaArgument2; }
            set
            {
                if (value != mAlphaArgument2)
                {
                    mAlphaArgument2 = value;
                    mDevice.TextureState[0].AlphaArgument2 = value;
                }
            }
        }
        public TextureOperation AlphaOperation
        {
            get { return mAlphaOperation; }
            set
            {
                if (value != mAlphaOperation)
                {
                    mAlphaOperation = value;
                    mDevice.TextureState[0].AlphaOperation = value;
                }
            }
        }
        public void Set2DDrawState()
        {
            mDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            mDevice.RenderState.DestinationBlend = Blend.InvSourceAlpha;

            mDevice.SamplerState[0].AddressU = TextureAddress.Clamp;
            mDevice.SamplerState[0].AddressV = TextureAddress.Clamp;

            SetView2D();
        }
        private void DefaultWorldMatrix()
        {
            mWorld2D = Matrix.Translation(-RenderTarget.Width / 2, -RenderTarget.Height * 0.5f, 0);
        }


        public void SetView2D()
        {
            Matrix world = mWorld2D;
            Matrix orthoProj = Matrix.OrthoRH(RenderTarget.Width, -RenderTarget.Height, -1, 1);


            mDevice.SetRenderState(RenderStates.CullMode, (int)Cull.None);
            mDevice.SetRenderState(RenderStates.Lighting, false);

            mDevice.SetTransform(TransformType.Projection, orthoProj);
            mDevice.SetTransform(TransformType.World, world);
            mDevice.SetTransform(TransformType.View, Matrix.Identity);

        }
        public void SetFontRenderState()
        {
            mLastTexture = null;
            mVertexFormat = VertexFormats.PointSize;
        }

        public void SetDeviceStateTexture(Texture texture)
        {

            if (texture == mLastTexture)
                return;

            mDevice.SetTexture(0, texture);

            mLastTexture = texture;

            if (texture != null)
            {
                AlphaArgument1 = TextureArgument.TextureColor;
                AlphaArgument2 = TextureArgument.Diffuse;
                AlphaOperation = TextureOperation.Modulate;
            }
        }

        public void SetOrigin(float x, float y, float z)
        {
            Matrix world = Matrix.Translation(x, y, z) * mWorld2D;

            mDevice.SetTransform(TransformType.World, world);
        }

        public Size MaxSurfaceSize
        {
            get
            {
                Size retval = new Size(mDevice.DeviceCaps.MaxTextureWidth, mDevice.DeviceCaps.MaxTextureHeight);
                
                if (retval.Width > 512)                    retval.Width = 512;
                if (retval.Height > 512)                    retval.Height = 512;
                
                return retval;
            }
        }

        public void Clear(ClearFlags flags, int color, float zdepth, int stencil)
        {
            mDevice.Clear(flags, color, zdepth, stencil);
        }

        public void Clear(ClearFlags flags, int color, float zdepth, int stencil, System.Drawing.Rectangle[] rects)
        {
            mDevice.Clear(flags, color, zdepth, stencil, rects);
        }

        //public void WriteToSurfaceVBAndRender
        //    (PrimitiveType primitiveType, int primCount, CustomVertex.TransformedColoredTextured[] verts)
        //{
        //    GraphicsStream stm;

        //    if (mSurfaceVBPointer + verts.Length < NumVertices)
        //    {
        //        stm = mSurfaceVB.Lock(mSurfaceVBPointer,
        //            CustomVertex.TransformedColoredTextured.StrideSize * verts.Length,
        //            LockFlags.NoOverwrite);

        //    }
        //    else
        //    {
        //        mSurfaceVBPointer = 0;

        //        stm = mSurfaceVB.Lock(mSurfaceVBPointer,
        //            CustomVertex.TransformedColoredTextured.StrideSize * verts.Length,
        //            LockFlags.Discard);
        //    }

        //    stm.Write(verts);

        //    mSurfaceVB.Unlock();

        //    mDevice.SetStreamSource(0, mSurfaceVB, 0);
        //    mDevice.VertexFormat = CustomVertex.TransformedColoredTextured.Format;
        //    mDevice.DrawPrimitives(primitiveType, mSurfaceVBPointer, primCount);

        //    mSurfaceVBPointer += verts.Length;
        //}


    }
}