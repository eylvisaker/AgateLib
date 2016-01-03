using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;

namespace AgateLib.Platform.Test
{
	public class FakeSurface : SurfaceImpl
	{
		private readonly Size size;

		public FakeSurface(Size size)
		{
			this.size = size;
		}

		protected override void Dispose(bool disposing)
		{
		}

		public override void Draw(DisplayLib.SurfaceState state)
		{
		}

		public override void SaveTo(string filename, DisplayLib.ImageFileFormat format)
		{
			throw new NotImplementedException();
		}

		public override SurfaceImpl CarveSubSurface(Rectangle srcRect)
		{
			throw new NotImplementedException();
		}

		public override void SetSourceSurface(SurfaceImpl surf, Rectangle srcRect)
		{
			throw new NotImplementedException();
		}

		public override DisplayLib.PixelBuffer ReadPixels(DisplayLib.PixelFormat format, Rectangle rect)
		{
			throw new NotImplementedException();
		}

		public override void WritePixels(DisplayLib.PixelBuffer buffer)
		{
			throw new NotImplementedException();
		}

		public override Size SurfaceSize
		{
			get { return size; }
		}

		public override bool IsLoaded
		{
			get { throw new NotImplementedException(); }
		}

		public override event EventHandler LoadComplete;
	}
}
