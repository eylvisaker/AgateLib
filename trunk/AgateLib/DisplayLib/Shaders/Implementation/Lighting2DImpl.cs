using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib.Shaders.Implementation
{
	public abstract class Lighting2DImpl : AgateShaderImpl 
	{
		public abstract Light[] Lights { get; }
		public abstract Color AmbientLight { get; set; }

	}
}
