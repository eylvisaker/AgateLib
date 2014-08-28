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
using SlimDX.Direct3D9;

namespace AgateSDX.Shaders
{
	class SDX_FF_Lighting2D : Lighting2DImpl
	{
		Device mDevice;
		Rectangle mCoords;

		public SDX_FF_Lighting2D()
		{

		}

		public override AgateLib.Geometry.Rectangle CoordinateSystem
		{
			get { return mCoords; }
			set
			{
				mCoords = value;
				SetOrthoProjection();
			}
		}

		public void Set2DDrawState()
		{
			mDevice = (AgateLib.DisplayLib.Display.Impl as SDX_Display).D3D_Device.Device;

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

			SetOrthoProjection();
		}

		private void SetOrthoProjection()
		{
			SlimDX.Matrix orthoProj = SlimDX.Matrix.OrthoOffCenterRH(
						 mCoords.Left, mCoords.Right, mCoords.Bottom, mCoords.Top, -1, 1);

			// TODO: figure out why this method sometimes gets called when mDevice is null?
			if (mDevice != null)
			{
				try
				{
					mDevice.SetTransform(TransformState.Projection, orthoProj);
				}
				catch (NullReferenceException e)
				{
					System.Diagnostics.Debug.Print("NullReferenceException when setting transformation.");
				}
			}
		}

		public override void Begin()
		{
			SDX_Display mDisplay = (SDX_Display)AgateLib.DisplayLib.Display.Impl;

			Set2DDrawState();

			mDisplay.D3D_Device.Device.SetRenderState(RenderState.Lighting, true);
			mDisplay.D3D_Device.Device.SetRenderState(RenderState.Ambient, AmbientLight.ToArgb());
			mDisplay.D3D_Device.Device.SetRenderState(RenderState.ColorVertex, true);

			TextureArgument arg0 = (TextureArgument)mDisplay.D3D_Device.Device.GetTextureStageState(0, TextureStage.ColorArg0);
			TextureArgument arg1 = (TextureArgument)mDisplay.D3D_Device.Device.GetTextureStageState(0, TextureStage.ColorArg1);
			TextureArgument arg2 = (TextureArgument)mDisplay.D3D_Device.Device.GetTextureStageState(0, TextureStage.ColorArg2);
			TextureOperation op = (TextureOperation)mDisplay.D3D_Device.Device.GetTextureStageState(0, TextureStage.ColorOperation);

			mDisplay.D3D_Device.Device.SetTextureStageState(0, TextureStage.ColorArg0, TextureArgument.Current);
			mDisplay.D3D_Device.Device.SetTextureStageState(0, TextureStage.ColorArg1, TextureArgument.Texture);
			mDisplay.D3D_Device.Device.SetTextureStageState(0, TextureStage.ColorArg2, TextureArgument.Current);
			mDisplay.D3D_Device.Device.SetTextureStageState(0, TextureStage.ColorOperation, TextureOperation.Modulate);
			mDisplay.D3D_Device.Device.SetRenderState(RenderState.DiffuseMaterialSource, ColorSource.Color1);
			mDisplay.D3D_Device.Device.SetRenderState(RenderState.AmbientMaterialSource, ColorSource.Color1);
			
			Material material = new Material();
			material.Diffuse = new SlimDX.Color4(Color.White.ToArgb());
			material.Ambient = new SlimDX.Color4(Color.White.ToArgb());
			
			
			mDisplay.D3D_Device.Device.Material = material;

			int index = 0;

			for (int i = 0; i < Lights.Count; i++)
			{
				var agateLight = Lights[i];

				if (agateLight == null)
					continue;
				if (agateLight.Enabled == false)
					continue;

				SlimDX.Direct3D9.Light l = new SlimDX.Direct3D9.Light();

				l.Ambient = new SlimDX.Color4(agateLight.AmbientColor.ToArgb());
				l.Attenuation0 = agateLight.AttenuationConstant;
				l.Attenuation1 = agateLight.AttenuationLinear;
				l.Attenuation2 = agateLight.AttenuationQuadratic;
				l.Diffuse = new SlimDX.Color4(agateLight.DiffuseColor.ToArgb());
				l.Type = LightType.Point;
				l.Direction = new SlimDX.Vector3(0, 0, 1);
				l.Range = 100;
				

				Vector3 pos = agateLight.Position;

				l.Position = new SlimDX.Vector3(pos.X, pos.Y, pos.Z);

				mDisplay.D3D_Device.Device.SetLight(index, l);
				mDisplay.D3D_Device.Device.EnableLight(index, true);

				index++;
			}
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

		public override int MaxActiveLights
		{
			get { return 8; }
		}

		public override Color AmbientLight { get; set; }
	}
}
