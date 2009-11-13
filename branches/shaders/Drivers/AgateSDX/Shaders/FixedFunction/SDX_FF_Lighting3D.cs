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
			mLights = new AgateLib.DisplayLib.Shaders.Light[mDisplay.D3D_Device.Device.Capabilities.MaxActiveLights];
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
			mDisplay.D3D_Device.Device.SetTransform(
				TransformState.Projection, GeoHelper.TransformAgateMatrix(mProjection.Transpose()));
			mDisplay.D3D_Device.Device.SetTransform(
				TransformState.View, GeoHelper.TransformAgateMatrix(mView.Transpose()));
			mDisplay.D3D_Device.Device.SetTransform(
				TransformState.World, GeoHelper.TransformAgateMatrix(mWorld.Transpose()));
		}

		public override void BeginPass(int passIndex)
		{
			throw new NotImplementedException();
		}

		public override void End()
		{
		}

		public override void EndPass()
		{
			throw new NotImplementedException();
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
