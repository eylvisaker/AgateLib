using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using AgateLib.ImplementationBase;
using OpenTK.Graphics.OpenGL;

namespace AgateOTK
{
	class ArbShaderCompiler : ShaderCompilerImpl
	{
		public ArbShaderCompiler()
		{
		}

		public OtkShader CompileShader(ShaderLanguage language, string vertexShaderSource, string pixelShaderSource)
		{
			if (language != ShaderLanguage.Glsl)
				throw new NotSupportedException("AgateOTK can only compile and use GLSL shaders.");

			GlslVertexProgram vert = CompileVertexProgram(vertexShaderSource);
			GlslFragmentProgram frag = CompileFragmentProgram(pixelShaderSource);

			return LinkPrograms(vert, frag);
		}

		private OtkShader LinkPrograms(GlslVertexProgram vert, GlslFragmentProgram frag)
		{
			int program = GL.Arb.CreateProgramObject();

			GL.Arb.AttachObject(program, vert.ShaderHandle);
			GL.Arb.AttachObject(program, frag.ShaderHandle);

			GL.Arb.LinkProgram(program);

			return new ArbShader(program, vert, frag);
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

			return shaderHandle;
		}

		public override Effect CompileEffect(ShaderLanguage language, string effectSource)
		{
			throw new NotImplementedException();
		}
	}
}
