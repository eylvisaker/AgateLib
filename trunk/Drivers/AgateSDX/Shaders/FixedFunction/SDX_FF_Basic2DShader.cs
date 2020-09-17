using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Geometry;
using SlimDX.Direct3D9;

namespace AgateSDX.Shaders
{
	class SDX_FF_Basic2DShader : Basic2DImpl  
	{
		Device mDevice;
		Rectangle mCoords;

		public SDX_FF_Basic2DShader()
		{
			mDevice = (AgateLib.DisplayLib.Display.Impl as SDX_Display).D3D_Device.Device;
		}

		public override AgateLib.Geometry.Rectangle CoordinateSystem
		{
			get { return mCoords;  }
			set { mCoords = value; }
		}

		public void Set2DDrawState()
		{
			mDevice.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
			mDevice.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);

			mDevice.SetSamplerState(0, SamplerState.AddressU, TextureAddress.Clamp);
			mDevice.SetSamplerState(0, SamplerState.AddressV, TextureAddress.Clamp);

			mDevice.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Anisotropic);
			mDevice.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Anisotropic);

			mDevice.SetRenderState(RenderState.CullMode, Cull.None);
			mDevice.SetRenderState(RenderState.Lighting, false);

			mDevice.SetTransform(TransformState.World, SlimDX.Matrix.Identity);
			mDevice.SetTransform(TransformState.View, SlimDX.Matrix.Identity);

			SlimDX.Matrix orthoProj = SlimDX.Matrix.OrthoOffCenterRH(
				mCoords.Left, mCoords.Right, mCoords.Bottom, mCoords.Top, -1, 1);

			mDevice.SetTransform(TransformState.Projection, orthoProj);
		}

		public override void Begin()
		{
			SDX_Display mDisplay = (SDX_Display)AgateLib.DisplayLib.Display.Impl;

			Set2DDrawState();
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
			throw new NotImplementedException();
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
