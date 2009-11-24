using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib.Shaders
{
	public class Lighting2D : AgateInternalShader
	{
		protected override BuiltInShader BuiltInShaderType
		{
			get { return BuiltInShader.Lighting2D; }
		}

		protected new Lighting2DImpl Impl
		{
			get { return (Lighting2DImpl)base.Impl; }
		}

		public List<Light> Lights
		{
			get { return Impl.Lights; }
		}
		public Color AmbientLight
		{
			get { return Impl.AmbientLight; }
			set { Impl.AmbientLight = value; }
		}
		public int MaxActiveLights
		{
			get { return Impl.MaxActiveLights; }
		}

		public void AddLight(Light ptLight)
		{
			for (int i = 0; i < Lights.Count; i++)
			{
				if (Lights[i] == null)
				{
					Lights[i] = ptLight;
					return;
				}
			}

			for (int i = 0; i < Lights.Count; i++)
			{
				if (Lights[i].Enabled == false)
				{
					Lights[i] = ptLight;
					return;
				}
			}
		}
	}

	public class Light
	{
		public Light()
		{
			Enabled = true;
		}

		public bool Enabled { get; set; }
		public Vector3 Position { get; set; }
		public Color DiffuseColor { get; set; }
		public Color SpecularColor { get; set; }
		public Color AmbientColor { get; set; }

		public float AttenuationConstant { get; set; }
		public float AttenuationLinear { get; set; }
		public float AttenuationQuadratic { get; set; }
	}

}
