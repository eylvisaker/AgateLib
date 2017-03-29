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
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Platform.Test.Display
{
	public class FakeSurfaceImpl : SurfaceImpl
	{
		Size size;
		PixelBuffer data;

		public SurfaceState LastDraw { get; private set; }

		public FakeSurfaceImpl(Size size)
		{
			this.size = size;
			data = new PixelBuffer(PixelFormat.ABGR8888, size);
		}
		protected override void Dispose(bool disposing)
		{
			data = null;
		}

		public override void Draw(SurfaceState state)
		{
			LastDraw = state.Clone();
		}

		public override void SaveTo(string filename, ImageFileFormat format)
		{
			throw new NotImplementedException();
		}

		public override SurfaceImpl CarveSubSurface(Rectangle srcRect)
		{
			var result = new FakeSurfaceImpl(srcRect.Size);
			result.data.CopyFrom(data, srcRect, new Point(), false);

			return result;
		}

		public override void SetSourceSurface(SurfaceImpl surf, Rectangle srcRect)
		{
			throw new NotImplementedException();
		}

		public override PixelBuffer ReadPixels(PixelFormat format, Rectangle rect)
		{
			PixelBuffer result = new PixelBuffer(format, rect.Size);

			result.CopyFrom(data, rect, Point.Zero, false, false);

			return result;
		}

		public override void WritePixels(PixelBuffer buffer)
		{
			data.CopyFrom(buffer, new Rectangle(Point.Zero, buffer.Size), Point.Zero, true);
		}

		public override Size SurfaceSize => size;

		public override bool IsLoaded => true;

		public override event EventHandler LoadComplete
		{
			add { value?.Invoke(this, EventArgs.Empty); }
			remove { }
		}
	}
}
