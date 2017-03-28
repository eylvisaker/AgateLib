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
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Mathematics.Geometry;
using OpenTK.Graphics.OpenGL;

namespace AgateLib.OpenGL.Legacy.FixedFunction
{
	class OTK_FF_Basic2DShader : Basic2DImpl  
	{
		Rectangle coords;

		void SetProjection ()
		{
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(coords.Left, coords.Right, coords.Bottom, coords.Top, -1, 1);
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
				SetProjection();
			}
		}

		public override void Begin()
		{
			SetProjection();
			
			GL.Disable(EnableCap.Lighting);

			GL.Enable(EnableCap.Texture2D);

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
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
