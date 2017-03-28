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

namespace AgateLib.DisplayLib.Shaders
{
	/// <summary>
	/// Lighting3D is the basic 3D shader.  Any driver with support for 3D must implement
	/// this shader.
	/// </summary>
	public class Lighting3D : Implementation.AgateInternalShader
	{
		protected override BuiltInShader BuiltInShaderType
		{
			get { return BuiltInShader.Lighting3D; }
		}
		/// <summary>
		/// Returns the implementation.
		/// </summary>
		protected new Lighting3DImpl Impl { get { return (Lighting3DImpl)base.Impl;}}

		/// <summary>
		/// Projection matrix for 3D view.  Best obtained by Matrix4x4.Projection.
		/// </summary>
		public Matrix4x4 Projection { get { return Impl.Projection; } set { Impl.Projection = value; } }
		/// <summary>
		/// View matrix for 3D view.  Best obtained by Matrix4x4.ViewLookAt.
		/// </summary>
		public Matrix4x4 View { get { return Impl.View; } set { Impl.View = value; } }
		/// <summary>
		/// World matrix for 3D view.
		/// </summary>
		public Matrix4x4 World { get { return Impl.World; } set { Impl.World= value; } }

		/// <summary>
		/// Set to true to enable lighting effects.
		/// </summary>
		public bool EnableLighting { get { return Impl.EnableLighting; } set { Impl.EnableLighting = value; } }
		public Light[] Lights { get { return Impl.Lights; } }
		public Color AmbientLight { get { return Impl.AmbientLight; } set { Impl.AmbientLight = value; } }
	}
}
