//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib.Shaders
{
	/// <summary>
	/// Lighting2D is the Basic2DShader with lighting effects added.
	/// </summary>
	public class Lighting2D : AgateInternalShader, IShader2D
	{
		protected override BuiltInShader BuiltInShaderType
		{
			get { return BuiltInShader.Lighting2D; }
		}

		protected new Lighting2DImpl Impl
		{
			get { return (Lighting2DImpl)base.Impl; }
		}

		/// <summary>
		/// Gets the list of lights.
		/// </summary>
		public List<Light> Lights
		{
			get { return Impl.Lights; }
		}
		/// <summary>
		/// Gets or sets the ambient light value.
		/// </summary>
		public Color AmbientLight
		{
			get { return Impl.AmbientLight; }
			set { Impl.AmbientLight = value; }
		}
		/// <summary>
		/// Gets the maximum number of active lights.
		/// </summary>
		public int MaxActiveLights
		{
			get { return Impl.MaxActiveLights; }
		}
		/// <summary>
		/// Adds a light to the list.
		/// </summary>
		/// <param name="ptLight"></param>
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

			Lights.Add(ptLight);
		}
	

		#region IShader2D implementation
		
		public Rectangle CoordinateSystem 
		{
			get	{ return Impl.CoordinateSystem; }
			set 
			{
				Impl.CoordinateSystem = value;
			}
		}
		#endregion
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
