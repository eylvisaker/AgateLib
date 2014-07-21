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
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Geometry;
using OpenTK.Graphics.OpenGL;
using AgateLib.GL.GL3.Shaders;

namespace AgateOTK.GL3.Shaders
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


		public override void SetTexture(AgateLib.DisplayLib.Shaders.EffectTexture tex, string variableName)
		{
			throw new NotImplementedException();
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
		public void SetVertexAttributes(AgateLib.Geometry.VertexTypes.VertexLayout layout)
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

		private int CountOf(AgateLib.Geometry.VertexTypes.VertexElementDataType vertexElementDataType)
		{
			switch (vertexElementDataType)
			{
				case AgateLib.Geometry.VertexTypes.VertexElementDataType.Float1:
					return 1;

				case AgateLib.Geometry.VertexTypes.VertexElementDataType.Float2:
					return 2;
				case AgateLib.Geometry.VertexTypes.VertexElementDataType.Float3:
					return 3;
				case AgateLib.Geometry.VertexTypes.VertexElementDataType.Float4:
				case AgateLib.Geometry.VertexTypes.VertexElementDataType.Int:
					return 4;

				default:
					throw new AgateLib.AgateException("Unrecognized data type.");
			}
		}
		private VertexAttribPointerType AttribTypeOf(AgateLib.Geometry.VertexTypes.VertexElementDataType vertexElementDataType)
		{
			switch (vertexElementDataType)
			{
				case AgateLib.Geometry.VertexTypes.VertexElementDataType.Float1:
				case AgateLib.Geometry.VertexTypes.VertexElementDataType.Float2:
				case AgateLib.Geometry.VertexTypes.VertexElementDataType.Float3:
				case AgateLib.Geometry.VertexTypes.VertexElementDataType.Float4:
					return VertexAttribPointerType.Float;

				case AgateLib.Geometry.VertexTypes.VertexElementDataType.Int:
					return VertexAttribPointerType.Byte;

				default:
					throw new AgateLib.AgateException("Unrecognized data type.");
			}
		}

		string VaryingName(AgateLib.Geometry.VertexTypes.VertexElementDesc vertexElementDesc)
		{
			switch (vertexElementDesc.ElementType)
			{
				case AgateLib.Geometry.VertexTypes.VertexElement.Position:
					return "position";
				case AgateLib.Geometry.VertexTypes.VertexElement.DiffuseColor:
					return "color";
				case AgateLib.Geometry.VertexTypes.VertexElement.Texture:
					return "texCoord";
				default:
					return null;
			}
		}
		#endregion
	}
}