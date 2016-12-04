using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DefaultAssets;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.Platform.Test.Fakes.Display;

namespace AgateLib.Platform.Test
{
	public class FakeDisplayFactory : IDisplayFactory
	{
		public FakeDisplayFactory()
		{
			DisplayImpl = new FakeDisplayDriver();
		}

		public FakeDisplayDriver DisplayImpl { get; private set; }

		DisplayImpl IDisplayFactory.DisplayImpl {  get { return DisplayImpl; } }

		public DisplayWindowImpl CreateDisplayWindow(AgateLib.DisplayLib.DisplayWindow owner, AgateLib.DisplayLib.CreateWindowParams windowParams)
		{
			return new FakeDisplayWindow(owner, windowParams);
		}

		public SurfaceImpl CreateSurface(IReadFileProvider fileProvider, string fileName)
		{
			return new FakeSurface(new Size(10, 10));
		}

		public SurfaceImpl CreateSurface(System.IO.Stream fileStream)
		{
			throw new NotImplementedException();
		}

		public SurfaceImpl CreateSurface(Size surfaceSize)
		{
			return new FakeSurface(surfaceSize);
		}

		public FontSurfaceImpl CreateFont(string fontFamily, float sizeInPoints, AgateLib.DisplayLib.FontStyles style)
		{
			throw new NotImplementedException();
		}

		public FontSurfaceImpl CreateFont(BitmapFontOptions bitmapOptions)
		{
			throw new NotImplementedException();
		}

		public FrameBufferImpl CreateFrameBuffer(AgateLib.Geometry.Size size)
		{
			throw new NotImplementedException();
		}


		public SurfaceImpl CreateSurface(DisplayLib.PixelBuffer pixels)
		{
			return new FakeSurface(pixels.Size);
		}
		
		public Task InitializeDefaultResourcesAsync(DefaultResources res)
		{
			res.Dispose();

			res.AgateSans = new FakeFont("AgateSans");
			res.AgateSerif = new FakeFont("AgateSerif");
			res.AgateMono = new FakeFont("AgateMono");

			return Task.FromResult(0);
		}
	}

}
