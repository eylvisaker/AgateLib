using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using OpenTK.Graphics;

namespace AgateOTK
{
	class GlslShader : ShaderProgram
	{
		struct UniformInfo
		{
			public int Location;
			public ActiveUniformType Type;
			public int Size;
		}

		Dictionary<string, UniformInfo> mUniforms = new Dictionary<string, UniformInfo>();
		int programHandle;

		public GlslShader(int handle, GlslVertexProgram vert, GlslFragmentProgram frag)
		{
			programHandle = handle;
			this.vertex = vert;
			this.pixel = frag;

			int count;
			GL.GetProgram(programHandle, ProgramParameter.ActiveUniforms, out count);

			StringBuilder b = new StringBuilder(1000);
			for (int i = 0; i < count; i++)
			{
				int length;
				int size;
				ActiveUniformType type;
				GL.GetActiveUniform(programHandle, i, 1000, out length, out size, out type, b);

				UniformInfo info = new UniformInfo();

				info.Location = GetUniformLocation(b.ToString());
				info.Type = type;
				info.Size = size;

				mUniforms.Add(b.ToString(), info);
			}
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

		public int Handle
		{
			get { return programHandle; }
		}

		private int GetUniformLocation(string name)
		{
			if (mUniforms.ContainsKey(name))
				return mUniforms[name].Location;

			int loc = GL.GetUniformLocation(programHandle, name);

			if (loc != -1)
			{
				return loc;
			}
			else
				throw new AgateLib.AgateException("Could not find uniform {0} in the GLSL program.", name);
		}

		public override void SetUniform(string name, params float[] v)
		{
			int loc = GetUniformLocation(name);

			switch (v.Length)
			{
				case 0: throw new AgateLib.AgateException("A value for the uniform must be specified.");
				case 1:
					GL.Uniform1(loc, v[0]);
					break;

				case 2:
					GL.Uniform2(loc, v[0], v[1]);
					break;

				case 3:
					GL.Uniform3(loc, v[0], v[1], v[2]);
					break;

				case 4:
					GL.Uniform4(loc, v[0], v[1], v[2], v[3]);
					break;

				default:
					throw new AgateLib.AgateException("Too many parameters to SetUniform.");
			}
		}
		public override void SetUniform(string name, params int[] v)
		{
			int loc = GetUniformLocation(name);

			switch (v.Length)
			{
				case 0: throw new AgateLib.AgateException("Must specify a value.");
				case 1:
					GL.Uniform1(loc, v[0]);
					break;

				case 2:
					GL.Uniform2(loc, v[0], v[1]);
					break;

				case 3:
					GL.Uniform3(loc, v[0], v[1], v[2]);
					break;

				case 4:
					GL.Uniform4(loc, v[0], v[1], v[2], v[3]);
					break;

				default:
					throw new AgateLib.AgateException("Too many parameters to SetUniform.");
			}
		}

		public override void SetUniform(string name, AgateLib.Geometry.Matrix4 matrix)
		{
			int loc = GetUniformLocation(name);

			unsafe
			{
				GL.UniformMatrix4(loc, 16, true, (float*)&matrix);
			}
		}
	}
}
