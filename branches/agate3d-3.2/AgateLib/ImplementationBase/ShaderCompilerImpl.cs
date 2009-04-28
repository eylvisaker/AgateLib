using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;

namespace AgateLib.ImplementationBase
{
	public abstract class ShaderCompilerImpl
	{
		public abstract ShaderProgram CompileShader(ShaderLanguage language, string vertexShaderSource, string pixelShaderSource);
	}
}
