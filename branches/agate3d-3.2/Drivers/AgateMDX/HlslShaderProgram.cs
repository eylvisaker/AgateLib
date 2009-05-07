using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;

namespace AgateMDX
{
	class HlslShaderProgram : ShaderProgram 
	{
		public override PixelShader PixelShader
		{
			get { throw new NotImplementedException(); }
		}

		public override void SetUniform(string name, AgateLib.Geometry.Matrix4 matrix)
		{
			throw new NotImplementedException();
		}

		public override void SetUniform(string name, params int[] v)
		{
			throw new NotImplementedException();
		}

		public override void SetUniform(string name, params float[] v)
		{
			throw new NotImplementedException();
		}

		public override VertexShader VertexShader
		{
			get { throw new NotImplementedException(); }
		}
	}

	class HlslPixelShader : PixelShader
	{
	}

	class HlslVertexShader : VertexShader
	{ }
}
