using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using AgateLib.ImplementationBase;

namespace AgateMDX
{
	class HlslCompiler : ShaderCompilerImpl 
	{
		MDX1_Display mDisplay;

		public HlslCompiler(MDX1_Display display)
		{
			mDisplay = display;
		}

		public override ShaderProgram CompileShader(ShaderLanguage language, string vertexShaderSource, string pixelShaderSource)
		{
			throw new NotImplementedException();

			//return new HlslShaderProgram();
		}
	}
}
