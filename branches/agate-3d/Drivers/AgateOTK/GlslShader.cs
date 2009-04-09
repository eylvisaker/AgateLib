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
			public string Name;
			public int Location;
			public ActiveUniformType Type;
			public int Size;

			public override string ToString()
			{
				return "Uniform: " + Name + " | " + Type.ToString();
			}
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
				string name;
				GL.GetActiveUniform(programHandle, i, 1000, out length, out size, out type, b);
				name = b.ToString();

				// Apparently OpenGL reports not just user uniforms, but also built-in uniforms
				// that are determined "active" and accessible in program execution.  Built-in uniforms
				// won't return a location because they cannot be directly modified by the OpenGL client.
				int loc = GL.GetUniformLocation(programHandle, name);
				if (loc == -1)
					continue;

				UniformInfo info = new UniformInfo();

				info.Name = name;
				info.Location = loc;
				info.Type = type;
				info.Size = size;

				mUniforms.Add(info.Name, info);
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
