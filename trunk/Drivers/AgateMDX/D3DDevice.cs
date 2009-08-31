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
using Direct3D = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.WinForms;

namespace AgateMDX
{
	public class D3DDevice : IDisposable
	{
		private Device mDevice;
		private Texture[] mLastTexture = new Texture[8];
		private MDX1_IRenderTarget mRenderTarget;
		private DrawBuffer mDrawBuffer;

		private VertexFormats mVertexFormat;
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

			mDevice.DeviceLost += new EventHandler(mDevice_DeviceLost);
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
			mVertexFormat = VertexFormats.None;
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
						mDevice.SamplerState[0].MinFilter = TextureFilter.Anisotropic;
						mDevice.SamplerState[0].MagFilter = TextureFilter.Anisotropic;
						break;

					case InterpolationMode.Fastest:
						mDevice.SamplerState[0].MinFilter = TextureFilter.Point;
						mDevice.SamplerState[0].MagFilter = TextureFilter.Point;
						break;
				}

				lastInterpolation = value;
			}
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

			mDevice.SamplerState[0].MagFilter = TextureFilter.Linear;
			mDevice.SamplerState[0].MinFilter = TextureFilter.Linear;

			SetView2D();
		}


		public void SetOrthoProjection(Rectangle region)
		{
			Matrix orthoProj = Matrix.OrthoOffCenterRH(
				region.Left, region.Right, region.Bottom, region.Top, -1, 1);

			mDevice.SetTransform(TransformType.Projection, orthoProj);
		}

		public void SetView2D()
		{
			Matrix world = mWorld2D;
			//Matrix orthoProj = Matrix.OrthoRH(RenderTarget.Width, -RenderTarget.Height, -1, 1);
			SetOrthoProjection(new Rectangle(0, 0, RenderTarget.Width, RenderTarget.Height));

			mDevice.RenderState.CullMode = Cull.None;
			mDevice.RenderState.Lighting = false;



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

				if (retval.Width > 512) retval.Width = 512;
				if (retval.Height > 512) retval.Height = 512;

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
				mDevice.SetRenderState(RenderStates.Lighting, false);
				return;
			}

			mDevice.RenderState.Lighting = true;
			mDevice.RenderState.DiffuseMaterialSource = ColorSource.Color1;
			mDevice.RenderState.AmbientMaterialSource = ColorSource.Color1;

			mDevice.RenderState.AmbientColor = lights.Ambient.ToArgb();
			
			//Material mat = new Material();
			//mat.Diffuse = System.Drawing.Color.White;

			//mDevice.Material = mat;

			for (int i = 0; i < mMaxLightsUsed || i < lights.Count; i++)
			{
				if (i >= lights.Count)
				{
					mDevice.Lights[i].Enabled = false;
					mDevice.Lights[i].Update();

					continue;
				}
				if (lights[i].Enabled == false)
				{
					mDevice.Lights[i].Enabled = false;
					mDevice.Lights[i].Update();

					continue;
				}

				mDevice.Lights[i].Type = LightType.Point;

				mDevice.Lights[i].Attenuation0 = lights[i].AttenuationConstant;
				mDevice.Lights[i].Attenuation1 = lights[i].AttenuationLinear;
				mDevice.Lights[i].Attenuation2 = lights[i].AttenuationQuadratic;

				mDevice.Lights[i].Diffuse = Interop.Convert(lights[i].Diffuse);
				mDevice.Lights[i].Ambient = Interop.Convert(lights[i].Ambient);
				//mDevice.Lights[i].Specular = (System.Drawing.Color)lights[i].Specular;

				mDevice.Lights[i].Position = new Microsoft.DirectX.Vector3(
					lights[i].Position.X, lights[i].Position.Y, lights[i].Position.Z);

				mDevice.Lights[i].Range = lights[i].Range;

				mDevice.Lights[i].Enabled = true;
				mDevice.Lights[i].Update();
			}

			mMaxLightsUsed = lights.Count;
		}
	}
}