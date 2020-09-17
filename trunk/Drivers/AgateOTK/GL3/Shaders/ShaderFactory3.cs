using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders.Implementation;

namespace AgateOTK.GL3.Shaders
{
	class ShaderFactory3 : ShaderFactory 
	{
		
		protected override AgateShaderImpl CreateBuiltInShaderImpl(BuiltInShader builtInShader)
		{
			switch (builtInShader)
			{
				case BuiltInShader.Basic2DShader:
					return new GL3_Basic2DShader();

				default:
					return null;
			}
		}
	}
}
