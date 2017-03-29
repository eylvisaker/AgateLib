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
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib.Shaders
{
	/// <summary>
	/// The default 2D shader.  This shader supports no effects, and must be implemented
	/// by every AgateLib display driver.
	/// </summary>
	public class Basic2DShader : AgateInternalShader, IShader2D 
	{
		/// <summary>
		/// Constructs a 2D shader.
		/// </summary>
		public Basic2DShader()
		{

		}

		/// <summary>
		/// Returns the implementation.
		/// </summary>
		protected new Basic2DImpl Impl => (Basic2DImpl)base.Impl;

		/// <summary>
		/// Gets the BuiltInShaderType enum corresponding to this built in shader.
		/// </summary>
		protected override BuiltInShader BuiltInShaderType => BuiltInShader.Basic2DShader;

		/// <summary>
		/// Gets or sets the coordinate system used for drawing.
		/// The default for any render target is to use a one-to-one
		/// mapping for pixels.
		/// </summary>
		public Rectangle CoordinateSystem
		{
			get { return Impl.CoordinateSystem; }
			set { Impl.CoordinateSystem = value; }
		}
	}
}
