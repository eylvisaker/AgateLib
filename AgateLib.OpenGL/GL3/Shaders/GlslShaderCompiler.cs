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
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.DisplayLib.Shaders;
using OpenTK.Graphics.OpenGL;

namespace AgateLib.OpenGL.GL3.Shaders
{
	static class GlslShaderCompiler
	{
		static public GlslShader CompileShader(string vertexShaderSource, string pixelShaderSource)
		{
			GlslVertexProgram vert = CompileVertexProgram(vertexShaderSource);
			GlslFragmentProgram frag = CompileFragmentProgram(pixelShaderSource);

			return LinkPrograms(vert, frag);
		}

		static private GlslShader LinkPrograms(GlslVertexProgram vert, GlslFragmentProgram frag)
		{
			int program = GL.CreateProgram();

			GL.AttachShader(program, vert.ShaderHandle);
			GL.AttachShader(program, frag.ShaderHandle);

			GL.LinkProgram(program);

			GL.ValidateProgram(program);

			int status;
			GL.GetProgram(program, GetProgramParameterName.ValidateStatus, out status);

			if (status == 0)
			{
				string info = GL.GetProgramInfoLog(program);

				throw new AgateLib.AgateException("Failed to validate GLSL shader program.\n{0}", info);
			}

			return new GlslShader(program, vert, frag);
		}

		static private GlslVertexProgram CompileVertexProgram(string vertexShaderSource)
		{
			return new GlslVertexProgram(CompileShader(ShaderType.VertexShader, vertexShaderSource), vertexShaderSource);
		}
		static private GlslFragmentProgram CompileFragmentProgram(string pixelShaderSource)
		{
			return new GlslFragmentProgram(CompileShader(ShaderType.FragmentShader, pixelShaderSource), pixelShaderSource);
		}

		static private int CompileShader(ShaderType type, string source)
		{
			int shaderHandle;

			shaderHandle = GL.CreateShader(type);

			GL.ShaderSource(shaderHandle, source);
			GL.CompileShader(shaderHandle);

			int status;
			GL.GetShader(shaderHandle, ShaderParameter.CompileStatus, out status);

			if (status == 0)
			{
				string info;
				GL.GetShaderInfoLog(shaderHandle, out info);

				throw new AgateLib.AgateException("Failed to compile {0} shader.\n{1}",
					type, info);
			}

			return shaderHandle;
		}
	}
}
