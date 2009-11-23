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
					return new OTK_FF_Basic2DShader();
				case BuiltInShader.Lighting2D:
					return new OTK_FF_Lighting2D();
				case BuiltInShader.Lighting3D:
					return new OTK_FF_Lighting3D();

				default:
					return null;
			}
		}
	}
}
