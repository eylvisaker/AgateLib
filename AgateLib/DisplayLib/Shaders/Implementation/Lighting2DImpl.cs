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
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib.Shaders.Implementation
{
	/// <summary>
	/// Base class for the implementation of the Lighting2D shader.
	/// </summary>
	public abstract class Lighting2DImpl : AgateShaderImpl 
	{
		/// <summary>
		/// Constructs a Lighting2DImpl object.
		/// </summary>
		protected Lighting2DImpl()
		{
			Lights = new List<Light>();
		}

		/// <summary>
		/// Gets the maximum number of lights.
		/// </summary>
		public abstract int MaxActiveLights { get; }
		/// <summary>
		/// Gets the list of lights.
		/// </summary>
		public List<Light> Lights { get; private set; }
		/// <summary>
		/// Sets the ambient light color.
		/// </summary>
		public abstract Color AmbientLight { get; set; }
		
		/// <summary>
		/// Gets or sets the coordinate system used (orthogonal projection).
		/// </summary>
		public abstract Rectangle CoordinateSystem { get; set; }
	}
}
