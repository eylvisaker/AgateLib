using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;

namespace AgateOTK
{
	public abstract class OtkShader : ShaderProgram
	{
		public abstract int Handle { get; }
	}
}
