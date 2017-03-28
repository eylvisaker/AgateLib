//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using OpenTK.Graphics.OpenGL;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

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

		private void SetArray(float[] array, Vector3f vec)
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

		public override void SetVariable(string name, AgateLib.DisplayLib.Color color)
		{
		}

		public override void SetVariable(string name, AgateLib.Mathematics.Matrix4x4 matrix)
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
