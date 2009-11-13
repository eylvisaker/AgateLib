using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib.Shaders
{
	public class Lighting3D : Implementation.AgateInternalShader
	{
		protected override BuiltInShader BuiltInShaderType
		{
			get { return BuiltInShader.Lighting3D; }
		}

		protected new Lighting3DImpl Impl { get { return (Lighting3DImpl)base.Impl;}}

		public Matrix4x4 Projection { get { return Impl.Projection; } set { Impl.Projection = value; } }
		public Matrix4x4 View { get { return Impl.View; } set { Impl.View = value; } }
		public Matrix4x4 World { get { return Impl.World; } set { Impl.World= value; } }

		public Light[] Lights { get { return Impl.Lights; } }
		public Color AmbientLight { get { return Impl.AmbientLight; } set { Impl.AmbientLight = value; } }
	}
}
