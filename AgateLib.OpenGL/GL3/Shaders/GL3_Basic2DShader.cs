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
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using OpenTK.Graphics.OpenGL;

namespace AgateLib.OpenGL.GL3.Shaders
{
	class GL3_Basic2DShader : Basic2DImpl, IGL3Shader
	{
		GlslShader shader;
		Rectangle coords;

		public GL3_Basic2DShader()
		{
			shader = GlslShaderCompiler.CompileShader(
				ShaderSources.Basic2D_vert, ShaderSources.Basic2D_pixel);
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


		public override void Begin()
		{
			GL.UseProgram(shader.Handle);

			shader.SetUniform("transform", Matrix4x4.Ortho((RectangleF)coords, -1, 1));

		}

		public override void BeginPass(int passIndex)
		{
			throw new NotImplementedException();
		}

		public override void EndPass()
		{
			throw new NotImplementedException();
		}

		public override void End()
		{
		}
		
		public override void SetVariable(string name, params float[] v)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, params int[] v)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, Matrix4x4 matrix)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, Color color)
		{
			throw new NotImplementedException();
		}

		public override int Passes
		{
			get { throw new NotImplementedException(); }
		}


		#region IGL3Shader Members

		public void SetTexture(int value)
		{
			shader.SetUniform("texture", value );
		}
		public void SetVertexAttributes(AgateLib.Mathematics.Geometry.VertexTypes.VertexLayout layout)
		{
			for (int i = 0; i < layout.Count; i++)
			{
				var desc = layout[i];

				int pos = shader.GetAttribLocation(VaryingName(desc));
				int count = CountOf(desc.DataType);
				VertexAttribPointerType type = AttribTypeOf(desc.DataType);

				GL.EnableVertexAttribArray(pos);

				GL.VertexAttribPointer(pos, count, type, false, layout.VertexSize,
					layout.ElementByteIndex(desc.ElementType));
				
			}
		}

		private int CountOf(AgateLib.Mathematics.Geometry.VertexTypes.VertexElementDataType vertexElementDataType)
		{
			switch (vertexElementDataType)
			{
				case AgateLib.Mathematics.Geometry.VertexTypes.VertexElementDataType.Float1:
					return 1;

				case AgateLib.Mathematics.Geometry.VertexTypes.VertexElementDataType.Float2:
					return 2;
				case AgateLib.Mathematics.Geometry.VertexTypes.VertexElementDataType.Float3:
					return 3;
				case AgateLib.Mathematics.Geometry.VertexTypes.VertexElementDataType.Float4:
				case AgateLib.Mathematics.Geometry.VertexTypes.VertexElementDataType.Int:
					return 4;

				default:
					throw new AgateLib.AgateException("Unrecognized data type.");
			}
		}
		private VertexAttribPointerType AttribTypeOf(AgateLib.Mathematics.Geometry.VertexTypes.VertexElementDataType vertexElementDataType)
		{
			switch (vertexElementDataType)
			{
				case AgateLib.Mathematics.Geometry.VertexTypes.VertexElementDataType.Float1:
				case AgateLib.Mathematics.Geometry.VertexTypes.VertexElementDataType.Float2:
				case AgateLib.Mathematics.Geometry.VertexTypes.VertexElementDataType.Float3:
				case AgateLib.Mathematics.Geometry.VertexTypes.VertexElementDataType.Float4:
					return VertexAttribPointerType.Float;

				case AgateLib.Mathematics.Geometry.VertexTypes.VertexElementDataType.Int:
					return VertexAttribPointerType.Byte;

				default:
					throw new AgateLib.AgateException("Unrecognized data type.");
			}
		}

		string VaryingName(AgateLib.Mathematics.Geometry.VertexTypes.VertexElementDesc vertexElementDesc)
		{
			switch (vertexElementDesc.ElementType)
			{
				case AgateLib.Mathematics.Geometry.VertexTypes.VertexElement.Position:
					return "position";
				case AgateLib.Mathematics.Geometry.VertexTypes.VertexElement.DiffuseColor:
					return "color";
				case AgateLib.Mathematics.Geometry.VertexTypes.VertexElement.Texture:
					return "texCoord";
				default:
					return null;
			}
		}
		#endregion
	}
}