using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib.Shaders.Implementation
{
	public abstract class Lighting2DImpl : AgateShaderImpl 
	{
		public Lighting2DImpl()
		{
			Lights = new List<Light>();
		}

		public abstract int MaxActiveLights { get; }
		public List<Light> Lights { get; private set; }
		public abstract Color AmbientLight { get; set; }

	}
}
