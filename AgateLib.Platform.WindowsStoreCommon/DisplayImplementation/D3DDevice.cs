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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;
using SharpDX.Direct3D11;
using SharpDX;
using SharpDX.SimpleInitializer;

namespace AgateLib.Platform.WindowsStore.DisplayImplementation
{
	public class D3DDevice : IDisposable
	{
		InterpolationMode lastInterpolation;
		private SharpDXContext context;

		private Device mDevice { get { return context.D3DDevice; } }
		private DeviceContext mDeviceContext { get { return context.D3DContext; } }

		private Texture2D[] mLastTexture = new Texture2D[8];
		private SDX_FrameBuffer mRenderTarget;
		private DrawBuffer mDrawBuffer;

		private bool mAlphaBlend;
		private Matrix mWorld2D;


		//VertexBuffer mSurfaceVB;
		//const int NumVertices = 1000;
		//int mSurfaceVBPointer = 0;

		//readonly int SurfaceVBSize = NumVertices * CustomVertex.TransformedColoredTextured.StrideSize;

		public D3DDevice(SharpDXContext context)
		{
			if (context == null) throw new ArgumentNullException();

			this.context = context;
			
			mWorld2D = Matrix.Identity;
			
			mDrawBuffer = new DrawBuffer(this, context);
		}


		~D3DDevice()
		{
			Dispose(false);
		}

		//public void SetVertexDeclarationForSurfaces()
		//{
		//	mDevice.VertexDeclaration = mSurfaceDecl;
		//}
		//void mDevice_DeviceLost(object sender, EventArgs e)
		//{
		//	// set weird values which will indicate that the device's
		//	// render states need to be set.
		//	mAlphaBlend = false;
		//	mAlphaArgument1 = TextureArgument.Temp;
		//	mAlphaArgument2 = TextureArgument.Temp;
		//	mAlphaOperation = TextureOperation.Add;
		//}

		public void Dispose()
		{
			Dispose(true);
		}
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		public Device Device
		{
			get { return mDevice; }
		}
		public DeviceContext DeviceContext
		{
			get { return mDeviceContext; }
		}
		public SharpDXContext Context
		{
			get { return context; }
		}


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
						//mDevice.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Anisotropic);
						//mDevice.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Anisotropic);
						break;

					case InterpolationMode.Fastest:
						//mDevice.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Point);
						//mDevice.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Point);
						break;
				}

				lastInterpolation = value;
			}
		}

		public SDX_FrameBuffer RenderTarget
		{
			get { return mRenderTarget; }
			set { mRenderTarget = value; }
		}
		public DrawBuffer DrawBuffer
		{
			get { return mDrawBuffer; }
		}
		public bool AlphaBlend
		{
			get { return mAlphaBlend; }
			set
			{
				if (value != mAlphaBlend)
				{
					mAlphaBlend = value;
					//mDevice.SetRenderState(RenderState.AlphaBlendEnable, value);
				}
			}
		}
		//public TextureArgument AlphaArgument1
		//{
		//	get { return mAlphaArgument1; }
		//	set
		//	{
		//		if (value != mAlphaArgument1)
		//		{
		//			mAlphaArgument1 = value;
		//			mDevice.SetTextureStageState(0, TextureStage.AlphaArg1, value);
		//		}

		//	}
		//}
		//public TextureArgument AlphaArgument2
		//{
		//	get { return mAlphaArgument2; }
		//	set
		//	{
		//		if (value != mAlphaArgument2)
		//		{
		//			mAlphaArgument2 = value;
		//			mDevice.SetTextureStageState(0, TextureStage.AlphaArg2, value);
		//		}
		//	}
		//}
		//public TextureOperation AlphaOperation
		//{
		//	get { return mAlphaOperation; }
		//	set
		//	{
		//		if (value != mAlphaOperation)
		//		{
		//			mAlphaOperation = value;
		//			mDevice.SetTextureStageState(0, TextureStage.AlphaOperation, value);
		//		}
		//	}
		//}

		public void SetDeviceStateTexture(Texture2D texture)
		{
			SetDeviceStateTexture(texture, 0);
		}
		public void SetDeviceStateTexture(Texture2D texture, int index)
		{
			if (texture == mLastTexture[index])
				return;

			//mDevice.SetTexture(index, texture);

			mLastTexture[index] = texture;

			if (texture != null)
			{
				//AlphaArgument1 = TextureArgument.Texture;
				//AlphaArgument2 = TextureArgument.Diffuse;
				//AlphaOperation = TextureOperation.Modulate;
			}
		}

		public Size MaxSurfaceSize
		{
			get { return new Size(16384, 16384); }
		}

		public void Clear(AgateLib.Geometry.Color color, float zdepth, int stencil)
		{
			mDeviceContext.ClearDepthStencilView(mRenderTarget.DepthStencilView, 
				DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil,
				zdepth, (byte)stencil);
	
			mDeviceContext.ClearRenderTargetView(mRenderTarget.RenderTargetView, color.ToColor4());
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


		/*
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
		 * */

	}
}