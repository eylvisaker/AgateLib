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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using Direct3D = SlimDX.Direct3D9;
using SlimDX.Direct3D9;
using SlimDX;

using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.WinForms;

namespace AgateSDX
{
	public class D3DDevice : IDisposable
	{
		private Device mDevice;
		private Texture[] mLastTexture = new Texture[8];
		private SDX_IRenderTarget mRenderTarget;
		private DrawBuffer mDrawBuffer;

		private VertexFormat mVertexFormat;
		private bool mAlphaBlend;
		private TextureArgument mAlphaArgument1;
		private TextureArgument mAlphaArgument2;
		private TextureOperation mAlphaOperation;

		private Matrix mWorld2D;

		private int mMaxLightsUsed = 0;


		//VertexBuffer mSurfaceVB;
		//const int NumVertices = 1000;
		//int mSurfaceVBPointer = 0;

		//readonly int SurfaceVBSize = NumVertices * CustomVertex.TransformedColoredTextured.StrideSize;

		public D3DDevice(Device device)
		{
			mDevice = device;
			mWorld2D = Matrix.Identity;

			//mDevice.DeviceLost += new EventHandler(mDevice_DeviceLost);
			mDrawBuffer = new DrawBuffer(this);
		}

		~D3DDevice()
		{
			Dispose(false);
		}


		void mDevice_DeviceLost(object sender, EventArgs e)
		{
			// set weird values which will indicate that the device's
			// render states need to be set.
			mAlphaBlend = false;
			mVertexFormat = VertexFormat.None;
			mAlphaArgument1 = TextureArgument.Temp;
			mAlphaArgument2 = TextureArgument.Temp;
			mAlphaOperation = TextureOperation.Add;
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

		InterpolationMode lastInterpolation;

		public InterpolationMode Interpolation
		{
			get { return lastInterpolation; }
			set
			{
				if (value == lastInterpolation)
					return;

				DrawBuffer.Flush();

				switch (value)
				{
					case InterpolationMode.Default:
					case InterpolationMode.Nicest:
						mDevice.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Anisotropic);
						mDevice.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Anisotropic);
						break;

					case InterpolationMode.Fastest:
						mDevice.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Point);
						mDevice.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Point);
						break;
				}

				lastInterpolation = value;
			}
		}

		public SDX_IRenderTarget RenderTarget
		{
			get { return mRenderTarget; }
			set { mRenderTarget = value; }
		}
		public DrawBuffer DrawBuffer
		{
			get { return mDrawBuffer; }
		}
		public VertexFormat VertexFormat
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
					mDevice.SetRenderState(RenderState.AlphaBlendEnable, value);
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
					mDevice.SetTextureStageState(0, TextureStage.AlphaArg1, value);
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
					mDevice.SetTextureStageState(0, TextureStage.AlphaArg2, value);
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
					mDevice.SetTextureStageState(0, TextureStage.AlphaOperation, value);
				}
			}
		}
		public void Set2DDrawState()
		{
			mDevice.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
			mDevice.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);

			mDevice.SetSamplerState(0, SamplerState.AddressU, TextureAddress.Clamp);
			mDevice.SetSamplerState(0, SamplerState.AddressV, TextureAddress.Clamp);

			mDevice.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Anisotropic);
			mDevice.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Anisotropic);

			SetView2D();
		}


		public void SetOrthoProjection(Rectangle region)
		{
			Matrix orthoProj = Matrix.OrthoOffCenterRH(
				region.Left, region.Right, region.Bottom, region.Top, -1, 1);

			mDevice.SetTransform(TransformState.Projection, orthoProj);
		}

		public void SetView2D()
		{
			Matrix world = mWorld2D;
			//Matrix orthoProj = Matrix.OrthoRH(RenderTarget.Width, -RenderTarget.Height, -1, 1);
			SetOrthoProjection(new Rectangle(0, 0, RenderTarget.Width, RenderTarget.Height));

			mDevice.SetRenderState(RenderState.CullMode, Cull.None);
			mDevice.SetRenderState(RenderState.Lighting, false);

			mDevice.SetTransform(TransformState.World, world);
			mDevice.SetTransform(TransformState.View, Matrix.Identity);
		}
		public void SetFontRenderState()
		{
			mLastTexture = null;
			mVertexFormat = VertexFormat.PointSize;
		}

		public void SetDeviceStateTexture(Texture texture)
		{
			SetDeviceStateTexture(texture, 0);
		}
		public void SetDeviceStateTexture(Texture texture, int index)
		{
			if (texture == mLastTexture[index])
				return;

			mDevice.SetTexture(index, texture);

			mLastTexture[index] = texture;

			if (texture != null)
			{
				AlphaArgument1 = TextureArgument.Texture;
				AlphaArgument2 = TextureArgument.Diffuse;
				AlphaOperation = TextureOperation.Modulate;
			}
		}

		public void SetOrigin(float x, float y, float z)
		{
			Matrix world = Matrix.Translation(x, y, z) * mWorld2D;

			mDevice.SetTransform(TransformState.World, world);
		}

		public Size MaxSurfaceSize
		{
			get
			{
				Size retval = new Size(
					mDevice.Capabilities.MaxTextureWidth, mDevice.Capabilities.MaxTextureHeight);

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



		internal void DoLighting(LightManager lights)
		{
			if (lights.Enabled == false)
			{
				mDevice.SetRenderState(RenderState.Lighting, false);
				return;
			}

			mDevice.SetRenderState(RenderState.Lighting, true);
			mDevice.SetRenderState(RenderState.DiffuseMaterialSource, ColorSource.Color1);
			mDevice.SetRenderState(RenderState.AmbientMaterialSource, ColorSource.Color1);

			mDevice.SetRenderState(RenderState.Ambient, lights.Ambient.ToArgb());
			
			//Material mat = new Material();
			//mat.Diffuse = System.Drawing.Color.White;

			//mDevice.Material = mat;
			//Direct3D.Light light = new SlimDX.Direct3D9.Light();

			//for (int i = 0; i < mMaxLightsUsed || i < lights.Count; i++)
			//{
			//    if (i >= lights.Count)
			//    {
			//        mDevice.SetLight(i, Direct3D.Ligh
			//        mDevice.SetLight(i, [i].Enabled = false;
			//        mDevice.Lights[i].Update();

			//        continue;
			//    }
			//    if (lights[i].Enabled == false)
			//    {
			//        mDevice.Lights[i].Enabled = false;
			//        mDevice.Lights[i].Update();

			//        continue;
			//    }

			//    mDevice.Lights[i].Type = LightType.Point;

			//    mDevice.Lights[i].Attenuation0 = lights[i].AttenuationConstant;
			//    mDevice.Lights[i].Attenuation1 = lights[i].AttenuationLinear;
			//    mDevice.Lights[i].Attenuation2 = lights[i].AttenuationQuadratic;

			//    mDevice.Lights[i].Diffuse = Interop.Convert(lights[i].Diffuse);
			//    mDevice.Lights[i].Ambient = Interop.Convert(lights[i].Ambient);
			//    //mDevice.Lights[i].Specular = (System.Drawing.Color)lights[i].Specular;

			//    mDevice.Lights[i].Position = new SlimDX.Vector3(
			//        lights[i].Position.X, lights[i].Position.Y, lights[i].Position.Z);

			//    mDevice.Lights[i].Range = lights[i].Range;

			//    mDevice.Lights[i].Enabled = true;
			//    mDevice.Lights[i].Update();
			//}

			mMaxLightsUsed = lights.Count;
		}
	}
}