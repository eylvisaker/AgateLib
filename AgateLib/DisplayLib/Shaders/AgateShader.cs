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

namespace AgateLib.DisplayLib.Shaders
{
	/// <summary>
	/// Base class for a shader.
	/// </summary>
	public class AgateShader
	{
		AgateShaderImpl mImpl;

		/// <summary>
		/// Sets the implementation.  If the implementation is already set, then
		/// an exception is thrown.
		/// </summary>
		/// <param name="impl"></param>
		protected internal void SetImpl(AgateShaderImpl impl)
		{
			if (this.mImpl != null)
				throw new InvalidOperationException("Cannot set impl on an object which already has one.");

			this.mImpl = impl;
		}

		/// <summary>
		/// Gets the implementation.
		/// </summary>
		public AgateShaderImpl Impl
		{
			get { return mImpl; }
		}
		/// <summary>
		/// Returns true if this shader has an implementation.
		/// </summary>
		public bool IsValid
		{
			get { return mImpl != null; }
		}

		internal void BeginInternal()
		{
			mImpl.Begin();
		}
		internal void EndInternal()
		{
			mImpl.End();
		}

		/// <summary>
		/// Returns true if this is the currently active shader.
		/// </summary>
		public bool IsActive
		{
			get { return Display.Shader == this; }
		}
		/// <summary>
		/// Activates this shader.
		/// </summary>
		public void Activate()
		{
			Display.Shader = this;
		}
		
	}
}
