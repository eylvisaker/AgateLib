using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.ImplementationBase;

namespace AgateLib.DisplayLib.Shaders
{
	public static class ShaderCompiler
	{
		static ShaderCompilerImpl impl;

		internal static void Initialize(AgateLib.ImplementationBase.ShaderCompilerImpl shaderCompilerImpl)
		{
			impl = shaderCompilerImpl;

			Display.DisposeDisplay += new Display.DisposeDisplayHandler(Display_DisposeDisplay);
		}

		static void Display_DisposeDisplay()
		{
			impl = null;
		}

		internal static void Disable()
		{
			impl = null;
		}

		public static ShaderProgram CompileEffect(ShaderLanguage language, string effectSource)
		{
			return impl.CompileEffect(language, effectSource);
		}
		public static ShaderProgram CompileShader(ShaderLanguage language, string vertexShaderSource, string pixelShaderSource)
		{
			return impl.CompileShader(language, vertexShaderSource, pixelShaderSource);
		}

	}
}
