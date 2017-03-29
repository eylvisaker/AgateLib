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
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.OpenGL
{
	public abstract class GL_FrameBuffer: FrameBufferImpl 
	{
		GLDrawBuffer drawBuffer;
		protected bool hasDepth;
		protected bool hasStencil;

		protected GL_FrameBuffer(ICoordinateSystem coords) : base(coords)
		{
		}

		public event EventHandler RenderComplete;

		public override bool HasDepthBuffer => hasDepth;

		public override bool HasStencilBuffer => hasStencil;

		public GLDrawBuffer DrawBuffer => drawBuffer;

		public override DisplayWindow AttachedWindow => MyAttachedWindow;

		public DisplayWindow MyAttachedWindow { get; set; }

		public abstract void MakeCurrent();

		protected void InitializeDrawBuffer()
		{
			drawBuffer = ((IGL_Display)Display.Impl).CreateDrawBuffer();
		}

		protected void OnRenderComplete()
		{
			RenderComplete?.Invoke(this, EventArgs.Empty);
		}
	}
}
