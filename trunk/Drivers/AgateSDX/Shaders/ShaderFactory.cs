using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;

namespace AgateSDX.Shaders
{
	abstract class ShaderFactory
	{
		static ShaderFactory inst = new FixedFunction.FixedFunctionShaderFactory();

		public static AgateShaderImpl CreateBuiltInShader(BuiltInShader BuiltInShaderType)
		{
			return inst.CreateBuiltInShaderImpl(BuiltInShaderType);
		}

		protected abstract AgateShaderImpl CreateBuiltInShaderImpl(BuiltInShader BuiltInShaderType);

	}
}
