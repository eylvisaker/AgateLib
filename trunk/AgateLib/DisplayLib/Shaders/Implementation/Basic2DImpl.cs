using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib.Shaders.Implementation
{
	public abstract class Basic2DImpl : AgateShaderImpl
	{
		public abstract Rectangle CoordinateSystem { get; set; }
	}
}
