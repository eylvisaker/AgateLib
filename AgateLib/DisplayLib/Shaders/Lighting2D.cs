//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

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
		public Vector3f Position { get; set; }
		public Color DiffuseColor { get; set; }
		public Color SpecularColor { get; set; }
		public Color AmbientColor { get; set; }

		public float AttenuationConstant { get; set; }
		public float AttenuationLinear { get; set; }
		public float AttenuationQuadratic { get; set; }
	}

}
