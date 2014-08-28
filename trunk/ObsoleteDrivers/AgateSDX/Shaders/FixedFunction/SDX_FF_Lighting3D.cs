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

namespace AgateSDX.Shaders.FixedFunction
{
	class SDX_FF_Lighting3D : Lighting3DImpl
	{
		Color mAmbientLight;
		AgateLib.DisplayLib.Shaders.Light[] mLights;
		Matrix4x4 mProjection;
		Matrix4x4 mView;
		Matrix4x4 mWorld;

		SDX_Display mDisplay;

		public SDX_FF_Lighting3D()
		{
			mDisplay = (SDX_Display)AgateLib.DisplayLib.Display.Impl;
			//mLights = new AgateLib.DisplayLib.Shaders.Light[mDisplay.D3D_Device.Device.Capabilities.MaxActiveLights];
			mLights = new AgateLib.DisplayLib.Shaders.Light[8];
		}
		public override Color AmbientLight
		{
			get
			{
				return mAmbientLight;
			}
			set
			{
				mAmbientLight = value;
			}
		}

		public override AgateLib.DisplayLib.Shaders.Light[] Lights
		{
			get { return mLights; }
		}

		public override Matrix4x4 Projection
		{
			get { return mProjection; }
			set { mProjection = value; }
		}
		public override Matrix4x4 View
		{
			get { return mView; }
			set { mView = value; }
		}
		public override Matrix4x4 World
		{
			get { return mWorld; }
			set { mWorld = value; }
		}
		public override void Begin()
		{
			int index = 0;

			mDisplay.D3D_Device.Device.SetTransform(
				TransformState.Projection, GeoHelper.TransformAgateMatrix(mProjection.Transpose()));
			mDisplay.D3D_Device.Device.SetTransform(
				TransformState.View, GeoHelper.TransformAgateMatrix(mView.Transpose()));
			mDisplay.D3D_Device.Device.SetTransform(
				TransformState.World, GeoHelper.TransformAgateMatrix(mWorld.Transpose()));

			if (EnableLighting == false)
			{
				mDisplay.D3D_Device.Device.SetRenderState(RenderState.Lighting, false);
				return;
			}

			mDisplay.D3D_Device.Device.SetRenderState(RenderState.Lighting, true);
			mDisplay.D3D_Device.Device.SetRenderState(RenderState.Ambient, mAmbientLight.ToArgb());

			Material material = new Material();
			material.Diffuse = new SlimDX.Color4(Color.White.ToArgb());
			material.Ambient = new SlimDX.Color4(Color.White.ToArgb());

			mDisplay.D3D_Device.Device.Material = material;

			for (int i = 0; i < Lights.Length; i++)
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

		public override void SetTexture(AgateLib.DisplayLib.Shaders.EffectTexture tex, string variableName)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, Color color)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, Matrix4x4 matrix)
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
