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
		bool arb = false;

		public GlslShaderCompiler()
		{
			double version = double.Parse(GL.GetString(StringName.Version).Substring(0, 3));

			if (version < 2.0)
				arb = true;
		}

		public override ShaderProgram CompileShader(ShaderLanguage language, string vertexShaderSource, string pixelShaderSource)
		{
			if (language != ShaderLanguage.Glsl)
				throw new NotSupportedException("AgateOTK can only compile and use GLSL shaders.");

			GlslVertexProgram vert = CompileVertexProgram(vertexShaderSource);
			GlslFragmentProgram frag = CompileFragmentProgram(pixelShaderSource);

			return LinkPrograms(vert, frag);
		}

		private ShaderProgram LinkPrograms(GlslVertexProgram vert, GlslFragmentProgram frag)
		{
			if (arb)
			{
				int program = GL.Arb.CreateProgramObject();

				GL.Arb.AttachObject(program, vert.ShaderHandle);
				GL.Arb.AttachObject(program, frag.ShaderHandle);

				GL.Arb.LinkProgram(program);

				return new ArbShader(program, vert, frag);
			}
			else
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
					throw new AgateLib.AgateException("Failed to validate GLSL shader program.");
				}

				return new GlslShader(program, vert, frag);
			}
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
			if (arb)
			{
				if (type == ShaderType.VertexShader)
					shaderHandle = GL.Arb.CreateShaderObject((ArbShaderObjects)0x8B31);
				else
					shaderHandle = GL.Arb.CreateShaderObject((ArbShaderObjects)0x8B30);

				string[] src = new string[1] { source };

				unsafe
				{
					GL.Arb.ShaderSource(shaderHandle, 1, src, (int*)IntPtr.Zero);
				}
				GL.Arb.CompileShader(shaderHandle);
			}
			else
			{
				shaderHandle = GL.CreateShader(type);

				GL.ShaderSource(shaderHandle, source);
				GL.CompileShader(shaderHandle);

				int status;
				GL.GetShader(shaderHandle, ShaderParameter.CompileStatus, out status);

				if (status == 0)
				{
					string info;
					GL.GetShaderInfoLog(shaderHandle, out info);

					throw new AgateLib.AgateException("Failed to compile {0} shader.  {1}",
						type, info);
				}
			}
			return shaderHandle;
		}
	}
}
