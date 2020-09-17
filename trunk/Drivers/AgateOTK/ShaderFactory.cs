using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;

namespace AgateOTK
{
	abstract class ShaderFactory
	{
		static ShaderFactory inst = new Legacy.FixedFunction.FixedFunctionShaderFactory();

		public static AgateShaderImpl CreateBuiltInShader(BuiltInShader builtInShader)
		{
			return inst.CreateBuiltInShaderImpl(builtInShader);
		}

		protected abstract AgateShaderImpl CreateBuiltInShaderImpl(BuiltInShader builtInShader);

		internal static void Initialize(bool gl3supported)
		{
			if (gl3supported)
			    inst = new GL3.Shaders.ShaderFactory3();
		}
	}
}
