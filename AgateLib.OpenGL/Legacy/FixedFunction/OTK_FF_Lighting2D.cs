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
using OpenTK.Graphics.OpenGL;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Geometry;

namespace AgateLib.OpenGL.Legacy.FixedFunction
{
	class OTK_FF_Lighting2D : Lighting2DImpl 
	{
		Rectangle coords;

		Color mAmbientLight;

		public OTK_FF_Lighting2D()
		{
		}
		
		
		public override Rectangle CoordinateSystem
		{
			get
			{
				return coords;
			}
			set
			{
				coords = value;
			}
		}
		
		
		public override int MaxActiveLights
		{
			get
			{
				int maxLights;
				GL.GetInteger(GetPName.MaxLights, out maxLights);
				return maxLights;
			}
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
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(coords.Left, coords.Right, coords.Bottom, coords.Top, -1, 1);
			
			float[] array = new float[4];

			GL.Enable(EnableCap.Lighting);

			SetArray(array, AmbientLight);
			GL.LightModel(LightModelParameter.LightModelAmbient, array);

			GL.Enable(EnableCap.ColorMaterial);
			GL.ColorMaterial(MaterialFace.FrontAndBack,
							 ColorMaterialParameter.AmbientAndDiffuse);

			int i;
			for (i = 0; i < Lights.Count && i < MaxActiveLights; i++)
			{
				EnableCap lightID = (EnableCap)((int)EnableCap.Light0 + i);
				LightName lightName = (LightName)((int)LightName.Light0 + i);

				if (Lights[i] == null || Lights[i].Enabled == false)
				{
					GL.Disable(lightID);
					continue;
				}

				GL.Enable(lightID);

				SetArray(array, Lights[i].DiffuseColor);
				GL.Light(lightName, LightParameter.Diffuse, array);

				//SetArray(array, mLights[i]);
				//GL.Lightv(lightName, LightParameter.Ambient, array);

				SetArray(array, Lights[i].Position);
				GL.Light(lightName, LightParameter.Position, array);

				GL.Light(lightName, LightParameter.ConstantAttenuation, Lights[i].AttenuationConstant);
				GL.Light(lightName, LightParameter.LinearAttenuation, Lights[i].AttenuationLinear);
				GL.Light(lightName, LightParameter.QuadraticAttenuation, Lights[i].AttenuationQuadratic);
			}
			for (; i < MaxActiveLights; i++)
			{
				EnableCap lightID = (EnableCap)((int)EnableCap.Light0 + i);
				GL.Disable(lightID);
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
