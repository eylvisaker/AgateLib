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
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// This interface is implemented by the FrameBuffer class. Its main purpose is
	/// to allow you to create a fake object implementing the interface in order to 
	/// write unit tests for drawing code.
	/// </summary>
	public interface IFrameBuffer : IDisposable
	{
		/// <summary>
		/// Height of the IFrameBuffer object.
		/// </summary>
		int Height { get; }
		/// <summary>
		/// Width of the IFrameBuffer object.
		/// </summary>
		int Width { get; }
		/// <summary>
		/// Size of the IFrameBuffer object. Should equal new Size(Width, Height).
		/// </summary>
		Size Size { get; }

		/// <summary>
		/// Gets or sets the coordinate system for the render target.
		/// </summary>
		ICoordinateSystem CoordinateSystem { get; set; }
	}
}
