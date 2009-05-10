using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using OpenTK.Graphics;

namespace AgateOTK
{
	class ArbShader : OtkShader
	{
		int programHandle;
		Dictionary<string, int> mUniforms = new Dictionary<string, int>();

		public ArbShader(int handle, GlslVertexProgram vert, GlslFragmentProgram frag)
		{
			programHandle = handle;
			this.vertex = vert;
			this.pixel = frag;
		}

		GlslVertexProgram vertex;
		GlslFragmentProgram pixel;

		public override PixelShader PixelShader
		{
			get { return pixel; }
		}
		public override VertexShader VertexShader
		{
			get { return vertex; }
		}

		public override int Handle
		{
			get { return programHandle; }
		}

		private int GetUniformLocation(string name)
		{
			if (mUniforms.ContainsKey(name))
				return mUniforms[name];

			return GL.Arb.GetUniformLocation(programHandle, name);
		}

		public override void SetUniform(string name, params float[] v)
		{
			int loc = GetUniformLocation(name);

		}
		public override void SetUniform(string name, params int[] v)
		{
		}

		public override void SetUniform(string name, AgateLib.Geometry.Matrix4 matrix)
		{
		}

		public override void Render(RenderHandler handler, object obj)
		{
			throw new NotImplementedException();
		}
	}
}
