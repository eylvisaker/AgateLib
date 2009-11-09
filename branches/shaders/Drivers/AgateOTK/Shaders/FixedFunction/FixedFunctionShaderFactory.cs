using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;

namespace AgateOTK.Shaders.FixedFunction
{
	class FixedFunctionShaderFactory : ShaderFactory 
	{
		protected override AgateShaderImpl CreateBuiltInShaderImpl(BuiltInShader buildInShader)
		{
			switch (buildInShader)
			{
				case BuiltInShader.Basic2DShader:
					return new OTK_Basic2DShader_FF();
				case BuiltInShader.Lighting2D:
					return new OTK_Lighting2D_FF();
					
				default:
					return null;
			}
		}
	}
}
