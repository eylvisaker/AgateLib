using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;

namespace AgateLib.ImplementationBase
{
	/// <summary>
	/// 
	/// </summary>
	[Obsolete]
	public abstract class ShaderCompilerImpl
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="language"></param>
		/// <param name="effectSource"></param>
		/// <returns></returns>
		public abstract Effect CompileEffect(ShaderLanguage language, string effectSource);
	}
}
