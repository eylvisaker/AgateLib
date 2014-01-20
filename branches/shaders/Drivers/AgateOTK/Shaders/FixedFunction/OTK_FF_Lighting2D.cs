﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Geometry;

namespace AgateOTK.Shaders.FixedFunction
{
	class OTK_FF_Lighting2D : Lighting2DImpl 
	{
		Light[] mLights;
		Color mAmbientLight;

		public OTK_FF_Lighting2D()
		{
		}
		public override Light[] Lights
		{
			get
			{
				if (mLights == null)
					InitializeLightsArray();

				return mLights;
			}
		}

		private void InitializeLightsArray()
		{
			int maxLights;
			GL.GetInteger(GetPName.MaxLights, out maxLights);

			mLights = new Light[maxLights];
		}

		public override Color AmbientLight
		{
			get { return mAmbientLight; }
			set { mAmbientLight = value; }
		}

		private void SetArray(float[] array, Vector3 vec)
		{
			array[0] = vec.X;
			array[1] = vec.Y;
			array[2] = vec.Z;
		}
		private void SetArray(float[] array, Color color)
		{
			array[0] = color.R / 255.0f;
			array[1] = color.G / 255.0f;
			array[2] = color.B / 255.0f;
			array[3] = color.A / 255.0f;
		}

		public override void Begin()
		{
			GL.Enable(EnableCap.Lighting);

			float[] array = new float[4];

			GL.Enable(EnableCap.Lighting);

			SetArray(array, AmbientLight);
			GL.LightModel(LightModelParameter.LightModelAmbient, array);

			GL.Enable(EnableCap.ColorMaterial);
			GL.ColorMaterial(MaterialFace.FrontAndBack,
							 ColorMaterialParameter.AmbientAndDiffuse);

			for (int i = 0; i < mLights.Length; i++)
			{
				EnableCap lightID = (EnableCap)((int)EnableCap.Light0 + i);
				LightName lightName = (LightName)((int)LightName.Light0 + i);

				if (mLights[i] == null || mLights[i].Enabled == false)
				{
					GL.Disable(lightID);
					continue;
				}

				GL.Enable(lightID);

				SetArray(array, mLights[i].DiffuseColor);
				GL.Light(lightName, LightParameter.Diffuse, array);

				//SetArray(array, mLights[i]);
				//GL.Lightv(lightName, LightParameter.Ambient, array);

				SetArray(array, mLights[i].Position);
				GL.Light(lightName, LightParameter.Position, array);

				GL.Light(lightName, LightParameter.ConstantAttenuation, mLights[i].AttenuationConstant);
				GL.Light(lightName, LightParameter.LinearAttenuation, mLights[i].AttenuationLinear);
				GL.Light(lightName, LightParameter.QuadraticAttenuation, mLights[i].AttenuationQuadratic);
			}
		}

		public override void BeginPass(int passIndex)
		{
			throw new NotImplementedException();
		}

		public override void End()
		{
			GL.Disable(EnableCap.Lighting);
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
		}

		public override void SetVariable(string name, AgateLib.Geometry.Color color)
		{
		}

		public override void SetVariable(string name, AgateLib.Geometry.Matrix4x4 matrix)
		{
		}

		public override void SetVariable(string name, params int[] v)
		{
		}

		public override void SetVariable(string name, params float[] v)
		{
		}
	}
}