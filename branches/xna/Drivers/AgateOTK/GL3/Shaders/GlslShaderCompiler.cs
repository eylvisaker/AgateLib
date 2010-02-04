using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.DisplayLib.Shaders;
using OpenTK.Graphics.OpenGL;

namespace AgateOTK.GL3.Shaders
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
			GL.GetProgram(program, ProgramParameter.ValidateStatus, out status);

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
