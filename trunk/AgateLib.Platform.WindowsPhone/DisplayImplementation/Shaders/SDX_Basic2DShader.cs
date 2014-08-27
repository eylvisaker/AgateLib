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
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Geometry;
using SharpDX.Direct3D11;

namespace AgateLib.Platform.WindowsPhone.DisplayImplementation.Shaders
{
	class SDX_Basic2DShader : Basic2DImpl  
	{
		Rectangle mCoords;

		SDX_Display mDisplay;
		D3DDevice mDevice;

		VertexShader mVertexShader;
		PixelShader mPixelShader;
		SharpDX.Direct3D11.Buffer mConstantBuffer;

		SamplerState mSampler;

		public SDX_Basic2DShader()
		{
			mDisplay = (SDX_Display)DisplayLib.Display.Impl;
			mDisplay.DeviceReset += mDisplay_DeviceReset;

			mDevice = mDisplay.D3D_Device;

			InitializeShaders();
		}

		void mDisplay_DeviceReset(object sender, EventArgs e)
		{
			InitializeShaders();
		}

		private void InitializeShaders()
		{
			if (mDevice.Device == null)
				return;

			var vs = (byte[])ShaderResources.ResourceManager.GetObject("Basic2Dvert");
			var ps = (byte[])ShaderResources.ResourceManager.GetObject("Basic2Dpixel");

			mVertexShader = new VertexShader(mDevice.Device, vs);
			mPixelShader = new PixelShader(mDevice.Device, ps);

			mConstantBuffer = new SharpDX.Direct3D11.Buffer(
				mDevice.Device,
				SharpDX.Utilities.SizeOf<SharpDX.Matrix>(),
				ResourceUsage.Default,
				BindFlags.ConstantBuffer,
				CpuAccessFlags.None,
				ResourceOptionFlags.None,
				0);

			mSampler = new SamplerState(mDevice.Device, new SamplerStateDescription()
			{
				Filter = Filter.MinMagMipLinear,
				AddressU = TextureAddressMode.Wrap,
				AddressV = TextureAddressMode.Wrap,
				AddressW = TextureAddressMode.Wrap,
				BorderColor = SharpDX.Color.Black,
				ComparisonFunction = Comparison.Never,
				MaximumAnisotropy = 16,
				MipLodBias = 0,
				MinimumLod = -float.MaxValue,
				MaximumLod = float.MaxValue
			});
		}

		public override AgateLib.Geometry.Rectangle CoordinateSystem
		{
			get { return mCoords;  }
			set
			{
				mCoords = value;
				SetOrthoProjection();
			}
		}

		public void Set2DDrawState()
		{
			//mDevice = (AgateLib.DisplayLib.Display.Impl as SDX_Display).D3D_Device.Device;

			//mDevice.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
			//mDevice.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);

			//mDevice.SetSamplerState(0, SamplerState.AddressU, TextureAddress.Clamp);
			//mDevice.SetSamplerState(0, SamplerState.AddressV, TextureAddress.Clamp);

			//mDevice.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Anisotropic);
			//mDevice.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Anisotropic);

			//mDevice.SetRenderState(RenderState.CullMode, Cull.None);
			//mDevice.SetRenderState(RenderState.Lighting, false);

			//mDevice.SetTransform(TransformState.World, SlimDX.Matrix.Identity);
			//mDevice.SetTransform(TransformState.View, SlimDX.Matrix.Identity);

			SetOrthoProjection();
		}

		private void SetOrthoProjection()
		{
			SharpDX.Matrix orthoProj = SharpDX.Matrix.OrthoOffCenterRH(
						 mCoords.Left, mCoords.Right, mCoords.Bottom, mCoords.Top, -1, 1);

			mDevice.DeviceContext.UpdateSubresource(ref orthoProj, mConstantBuffer, 0);
			//// TODO: figure out why this method sometimes gets called when mDevice is null?
			//if (mDevice != null)
			//{
			//	try
			//	{
			//		mDevice.SetTransform(TransformState.Projection, orthoProj);
			//	}
			//	catch (NullReferenceException e)
			//	{
			//		System.Diagnostics.Debug.Print("NullReferenceException when setting transformation.");
			//	}
			//}
		}

		public override void Begin()
		{
			mDevice.DeviceContext.VertexShader.SetConstantBuffer(0, mConstantBuffer);
			mDevice.DeviceContext.VertexShader.Set(mVertexShader);

			mDevice.DeviceContext.PixelShader.Set(mPixelShader);
			mDevice.DeviceContext.PixelShader.SetSampler(0, mSampler);

			//SDX_Display mDisplay = (SDX_Display)AgateLib.DisplayLib.Display.Impl;

			//Set2DDrawState();
		}

		public override void BeginPass(int passIndex)
		{
		}

		public override void End()
		{
		}

		public override void EndPass()
		{
		}

		public override int Passes
		{
			get { return 1; }
		}

		public override void SetTexture(EffectTexture tex, string variableName)
		{
			//mDevice.DeviceContext.PixelShader.SetShaderResource();
		}


		public override void SetVariable(string name, AgateLib.Geometry.Color color)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, AgateLib.Geometry.Matrix4x4 matrix)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, params int[] v)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, params float[] v)
		{
			throw new NotImplementedException();
		}
	}
}
