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

using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Platform.Test.Display
{
	public class FakeDisplayWindow : DisplayWindowImpl
	{
		private CreateWindowParams windowParams;
		private bool isDisposed;

		public FakeDisplayWindow(Size size)
		{
			FrameBuffer = new FakeFrameBuffer(this);

			Resolution = new Resolution(size);
		}

		public FakeDisplayWindow(DisplayWindow owner, CreateWindowParams windowParams)
			: this(windowParams.Resolution?.Size ?? Size.Empty)
		{
			// TODO: Complete member initialization
			Owner = owner;
			Resolution = windowParams.Resolution?.Clone() ?? new Resolution(10, 10);
			this.windowParams = windowParams;

			Screen = windowParams.TargetScreen;
		}

		protected override void Dispose(bool disposing)
		{
			isDisposed = true;

			base.Dispose(disposing);
		}

		public DisplayWindow Owner { get; set; }

		public override bool IsClosed => isDisposed;

		public override bool IsFullScreen => false;

		public override FrameBufferImpl FrameBuffer { get; }

		public sealed override IResolution Resolution { get; set; }

		public override Size PhysicalSize => Resolution?.Size ?? Size.Empty;

		public override string Title { get; set; }

		public override ScreenInfo Screen { get; }
	}
}