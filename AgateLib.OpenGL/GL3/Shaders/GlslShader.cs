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
using AgateLib.DisplayLib.Shaders;
using OpenTK.Graphics.OpenGL;

namespace AgateLib.OpenGL.GL3.Shaders
{
	class GlslShader
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
		struct AttributeInfo
		{
			public string Name;
			public int Location;
			public ActiveAttribType Type;
			public int Size;

			public override string ToString()
			{
				return "Uniform: " + Name + " | " + Type.ToString();
			}
		}

		List<UniformInfo> mUniforms = new List<UniformInfo>();
		List<AttributeInfo> mAttributes = new List<AttributeInfo>();
		List<string> mAttributeNames;

		List<string> mSampler2DUniforms = new List<string>();
		int programHandle;

		GlslVertexProgram vertex;
		GlslFragmentProgram pixel;

		public GlslShader(int handle, GlslVertexProgram vert, GlslFragmentProgram frag)
		{
			programHandle = handle;
			this.vertex = vert;
			this.pixel = frag;

			LoadUniforms();
			LoadAttributes();
		}

		private void LoadAttributes()
		{
			int count;
			GL.GetProgram(programHandle, GetProgramParameterName.ActiveAttributes, out count);

			StringBuilder b = new StringBuilder(1000);
			for (int i = 0; i < count; i++)
			{
				int length;
				int size;
				ActiveAttribType type;
				GL.GetActiveAttrib(programHandle, i, 1000, out length, out size, out type, b);
				string name = b.ToString();

				int loc = GL.GetAttribLocation(programHandle, name);

				// ignore active attributes that we aren't interested in because we don't set them
				// with glVertexAttribPointer
				if (loc == -1)
					continue;

				AttributeInfo info = new AttributeInfo();

				info.Name = name;
				info.Location = loc;
				info.Type = type;
				info.Size = size;

				mAttributes.Add(info);
			}

			mAttributeNames = mAttributes.Select(x => x.Name).ToList();
		}
		private void LoadUniforms()
		{
			int count;
			GL.GetProgram(programHandle, GetProgramParameterName.ActiveUniforms, out count);

			StringBuilder b = new StringBuilder(1000);
			for (int i = 0; i < count; i++)
			{
				int length;
				int size;
				ActiveUniformType type;
				GL.GetActiveUniform(programHandle, i, 1000, out length, out size, out type, b);
				string name = b.ToString();

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

				mUniforms.Add(info);
			}

			mSampler2DUniforms = mUniforms
				.Where(x => x.Type == ActiveUniformType.Sampler2D)
				.Select(x => x.Name)
				.ToList();
		}

		public IList<string> Attributes
		{
			get { return mAttributeNames; }
		}
		public IList<string> Sampler2DUniforms
		{
			get
			{
				return mSampler2DUniforms;
			}
		}

		public GlslFragmentProgram PixelShader
		{
			get { return pixel; }
		}
		public GlslVertexProgram VertexShader
		{
			get { return vertex; }
		}

		public int Handle
		{
			get { return programHandle; }
		}

		private int GetUniformLocation(string name)
		{
			if (mUniforms.Any(x => x.Name == name))
				return mUniforms.First(x => x.Name == name).Location;

			int loc = GL.GetUniformLocation(programHandle, name);

			if (loc != -1)
				return loc;
			else
				throw new AgateLib.AgateException("Could not find uniform {0} in the GLSL program.", name);
		}
		internal int GetAttribLocation(string name)
		{
			if (mAttributes.Any(x => x.Name == name))
				return mAttributes.First(x => x.Name == name).Location;

			int loc = GL.GetAttribLocation(programHandle, name);

			if (loc != -1)
				return loc;
			else
				throw new AgateLib.AgateException("Could not find attribute {0} in the GLSL program.", name);
		}

		public void SetUniform(string name, params float[] v)
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
		public void SetUniform(string name, params int[] v)
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

		public void SetUniform(string name, AgateLib.Mathematics.Matrix4x4 matrix)
		{
			int loc = GetUniformLocation(name);

			unsafe
			{
				GL.UniformMatrix4(loc, 16, true, (float*)&matrix);
			}
		}
	}
}
