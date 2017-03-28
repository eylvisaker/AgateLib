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

namespace AgateLib.DisplayLib.Shaders
{
	/// <summary>
	/// Static class containing AgateLib built in shaders.
	/// </summary>
	public static class AgateBuiltInShaders
	{
		internal static void InitializeShaders()
		{
			Basic2DShader = new Basic2DShader();
			Lighting2D = new Lighting2D();
			Lighting3D = new Lighting3D();
		}
		internal static void DisposeShaders()
		{
			Basic2DShader = null;
			Lighting2D = null;
			Lighting3D = null;
		}

		/// <summary>
		/// Gets an object implementing the Basic2DShader class.
		/// This should always be available.
		/// </summary>
		public static Basic2DShader Basic2DShader { get; private set; }
		/// <summary>
		/// Gets an object implementing the Lighting2D class.
		/// </summary>
		public static Lighting2D Lighting2D { get; private set; }
		/// <summary>
		/// Gets and object implementing the Lighting3D class.
		/// </summary>
		public static Lighting3D Lighting3D { get; private set; }

	}
}
