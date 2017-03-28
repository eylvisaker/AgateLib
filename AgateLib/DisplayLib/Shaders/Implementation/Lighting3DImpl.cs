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
using AgateLib.Mathematics;

namespace AgateLib.DisplayLib.Shaders.Implementation
{
	/// <summary>
	/// Base class for implementing the Lighting3D shader.
	/// </summary>
	public abstract class Lighting3DImpl : AgateShaderImpl  
	{
		/// <summary>
		/// Constructs a Lighting3DImpl object.
		/// </summary>
		protected Lighting3DImpl()
		{
			AmbientLight = Color.White;
			EnableLighting = true;
		}

		/// <summary>
		/// Gets or sets the projection matrix.
		/// </summary>
		public abstract Matrix4x4 Projection { get; set; }
		/// <summary>
		/// Gets or sets the projection matrix.
		/// </summary>
		public abstract Matrix4x4 View { get; set; }
		/// <summary>
		/// Gets or sets the projection matrix.
		/// </summary>
		public abstract Matrix4x4 World { get; set; }

		/// <summary>
		/// Gets or sets the lights.
		/// </summary>
		public abstract Light[] Lights { get; }
		/// <summary>
		/// Gets or sets the ambient color.
		/// </summary>
		public abstract Color AmbientLight { get; set; }
		/// <summary>
		/// Gets or sets whether or not lighting should be enabled.
		/// </summary>
		public virtual bool EnableLighting { get; set; }

	}
}
