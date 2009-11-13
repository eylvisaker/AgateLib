using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;

namespace AgateSDX.Shaders.FixedFunction
{
	class FixedFunctionShaderFactory : ShaderFactory 
	{
		protected override AgateLib.DisplayLib.Shaders.Implementation.AgateShaderImpl CreateBuiltInShaderImpl(AgateLib.DisplayLib.Shaders.Implementation.BuiltInShader BuiltInShaderType)
		{
			switch (BuiltInShaderType)
			{
				case BuiltInShader.Basic2DShader:
					return new SDX_FF_Basic2DShader();

				case BuiltInShader.Lighting3D:
					return new SDX_FF_Lighting3D();

				default:
					return null;
			}
		}
	}
}
