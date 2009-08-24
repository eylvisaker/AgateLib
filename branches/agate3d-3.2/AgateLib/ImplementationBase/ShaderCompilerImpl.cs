using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;

namespace AgateLib.ImplementationBase
{
	public abstract class ShaderCompilerImpl
	{
		public abstract Effect CompileEffect(ShaderLanguage language, string effectSource);
	}
}
