using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using AgateLib.ImplementationBase;
using OpenTK.Graphics;

namespace AgateOTK
{
	class GlslShaderCompiler : ShaderCompilerImpl
	{
		public GlslShaderCompiler()
		{
		}

		public  OtkShader CompileShader(ShaderLanguage language, string vertexShaderSource, string pixelShaderSource)
		{
			if (language != ShaderLanguage.Glsl)
				throw new NotSupportedException("AgateOTK can only compile and use GLSL shaders.");

			GlslVertexProgram vert = CompileVertexProgram(vertexShaderSource);
			GlslFragmentProgram frag = CompileFragmentProgram(pixelShaderSource);

			return LinkPrograms(vert, frag);
		}

		private OtkShader LinkPrograms(GlslVertexProgram vert, GlslFragmentProgram frag)
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
				string info;
				GL.GetProgramInfoLog(program, out info);

				throw new AgateLib.AgateException("Failed to validate GLSL shader program.\n{0}", info);
			}

			return new GlslShader(program, vert, frag);
		}

		private GlslVertexProgram CompileVertexProgram(string vertexShaderSource)
		{
			return new GlslVertexProgram(CompileShader(ShaderType.VertexShader, vertexShaderSource), vertexShaderSource);
		}

		private GlslFragmentProgram CompileFragmentProgram(string pixelShaderSource)
		{
			return new GlslFragmentProgram(CompileShader(ShaderType.FragmentShader, pixelShaderSource), pixelShaderSource);
		}

		private int CompileShader(ShaderType type, string source)
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

		public override Effect CompileEffect(ShaderLanguage language, string effectSource)
		{
			throw new NotImplementedException();
		}
	}
}
