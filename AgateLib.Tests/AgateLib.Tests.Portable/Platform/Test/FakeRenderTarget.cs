using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Geometry.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.Test
{
	public class FakeRenderTarget : IFrameBuffer
	{
		public FakeRenderTarget()
		{
			CoordinateSystem = new NativeCoordinates();
			CoordinateSystem.RenderTargetSize = Size;
		}

		public int Height { get { return 400; } }
		public int Width { get { return 640; } }
		public Size Size { get { return new Size(Width, Height); } }

		public ICoordinateSystem CoordinateSystem { get; set; }
	}
}
