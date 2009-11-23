using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;

namespace AgateOTK.Shaders
{
	abstract class ShaderFactory
	{
		static ShaderFactory inst = new FixedFunction.FixedFunctionShaderFactory();

		public static AgateShaderImpl CreateBuiltInShader(BuiltInShader buildInShader)
		{
			return inst.CreateBuiltInShaderImpl(buildInShader);
		}

		protected abstract AgateShaderImpl CreateBuiltInShaderImpl(BuiltInShader buildInShader);
	}
}
