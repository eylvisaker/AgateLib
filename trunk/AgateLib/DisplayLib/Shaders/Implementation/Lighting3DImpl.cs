using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib.Shaders.Implementation
{
	public abstract class Lighting3DImpl : AgateShaderImpl  
	{
		public Lighting3DImpl()
		{
			AmbientLight = Color.White;
			EnableLighting = true;
		}
		public abstract Matrix4x4 Projection { get; set; }
		public abstract Matrix4x4 View { get; set; }
		public abstract Matrix4x4 World { get; set; }

		public abstract Light[] Lights { get; }
		public abstract Color AmbientLight { get; set; }
		public virtual bool EnableLighting { get; set; }

	}
}
