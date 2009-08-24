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

		public  GlslFragmentProgram PixelShader
		{
			get { return pixel; }
		}
		public  GlslVertexProgram VertexShader
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

		public  void SetUniform(string name, params float[] v)
		{
			int loc = GetUniformLocation(name);

		}
		public  void SetUniform(string name, params int[] v)
		{
		}

		public  void SetUniform(string name, AgateLib.Geometry.Matrix4 matrix)
		{
		}

		public  void Render<T>(RenderHandler<T> handler, T obj)
		{
			throw new NotImplementedException();
		}
	}
}
