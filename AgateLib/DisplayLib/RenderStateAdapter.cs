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

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which gets or sets render states for the Display.
	/// </summary>
	public sealed class RenderStateAdapter
	{
		void CheckDisplayInitialized()
		{
			if (Display.Impl == null)
				throw new AgateException("Display has not been initialized.");
		}

		/// <summary>
		/// Gets or sets whether the vertical blank should be waited for on each frame.
		/// </summary>
		public bool WaitForVerticalBlank
		{
			get
			{
				CheckDisplayInitialized();
				return Display.Impl.GetRenderState(RenderStateBool.WaitForVerticalBlank);
			}
			set
			{
				CheckDisplayInitialized();
				Display.Impl.SetRenderState(RenderStateBool.WaitForVerticalBlank, value);
			}
		}
		/// <summary>
		/// Gets or sets whether alpha blending is enabled.
		/// </summary>
		public bool AlphaBlend
		{
			get
			{
				CheckDisplayInitialized();
				return Display.Impl.GetRenderState(RenderStateBool.AlphaBlend);
			}
			set
			{
				CheckDisplayInitialized();
				Display.Impl.SetRenderState(RenderStateBool.AlphaBlend, value);
			}
		}
		/// <summary>
		/// Gets or sets whether to test the z-buffer when writing pixels, if it is available.
		/// </summary>
		public bool ZBufferTest
		{
			get
			{
				CheckDisplayInitialized();
				return Display.Impl.GetRenderState(RenderStateBool.ZBufferTest);
			}
			set
			{
				CheckDisplayInitialized();
				Display.Impl.SetRenderState(RenderStateBool.ZBufferTest, value);
			}
		}
		/// <summary>
		/// Gets or sets whether to write to the z-buffer when writing pixels, if it is available.
		/// </summary>
		public bool ZBufferWrite
		{
			get
			{
				CheckDisplayInitialized();
				return Display.Impl.GetRenderState(RenderStateBool.ZBufferWrite);
			}
			set
			{
				CheckDisplayInitialized();
				Display.Impl.SetRenderState(RenderStateBool.ZBufferWrite, value);
			}
		}
		/// <summary>
		/// Gets or sets whether to test the stencil buffer when writing pixels.
		/// </summary>
		public bool StencilBufferTest
		{
			get
			{
				CheckDisplayInitialized();
				return Display.Impl.GetRenderState(RenderStateBool.StencilBufferTest);
			}
			set
			{
				CheckDisplayInitialized();
				Display.Impl.SetRenderState(RenderStateBool.StencilBufferTest, value);
			}
		}
	}

	/// <summary>
	/// Enum describing boolean render state values.
	/// </summary>
	public enum RenderStateBool
	{
		/// <summary>
		/// VSync
		/// </summary>
		WaitForVerticalBlank,
		/// <summary>
		/// Alpha blending
		/// </summary>
		AlphaBlend,
		/// <summary>
		/// Z Buffer Testing
		/// </summary>
		ZBufferTest,
		/// <summary>
		/// Z buffer writing
		/// </summary>
		ZBufferWrite,
		/// <summary>
		/// Stencil buffer testing
		/// </summary>
		StencilBufferTest,
	}
}
