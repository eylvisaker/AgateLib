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
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib.DefaultAssets;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Drivers;
using AgateLib.IO;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Platform.Test.Display
{
	public class FakeDisplayFactory : IDisplayFactory
	{
		public FakeDisplayFactory()
		{
			DisplayCore = new FakeDisplayDriver();
		}

		public FakeDisplayDriver DisplayCore { get; private set; }

		DisplayImpl IDisplayFactory.DisplayCore => DisplayCore;

		public DisplayWindowImpl CreateDisplayWindow(AgateLib.DisplayLib.DisplayWindow owner, AgateLib.DisplayLib.CreateWindowParams windowParams)
		{
			return new FakeDisplayWindow(owner, windowParams);
		}

		public SurfaceImpl CreateSurface(IReadFileProvider fileProvider, string fileName)
		{
			using (var fileStream = fileProvider.OpenRead(fileName))
			{
				return new FakeSurfaceImpl(new Size(10, 10));
			}
		}

		public SurfaceImpl CreateSurface(System.IO.Stream fileStream)
		{
			return new FakeSurfaceImpl(new Size(10, 10));
		}

		public SurfaceImpl CreateSurface(Size surfaceSize)
		{
			return new FakeSurfaceImpl(surfaceSize);
		}

		public FontSurfaceImpl CreateFont(string fontFamily, float sizeInPoints, AgateLib.DisplayLib.FontStyles style)
		{
			return new FakeFontSurface(fontFamily, sizeInPoints, style);
		}

		public FontSurfaceImpl CreateFont(BitmapFontOptions bitmapOptions)
		{
			throw new NotImplementedException();
		}

		public FrameBufferImpl CreateFrameBuffer(Size size)
		{
			throw new NotImplementedException();
		}


		public SurfaceImpl CreateSurface(DisplayLib.PixelBuffer pixels)
		{
			return new FakeSurfaceImpl(pixels.Size);
		}
		
		public Task InitializeDefaultResourcesAsync(DefaultResources res)
		{
			res.Dispose();

			res.AgateSans = BuildFont("AgateSans");
			res.AgateSerif = BuildFont("AgateSerif");
			res.AgateMono = BuildFont("AgateMono");

			return Task.FromResult(0);
		}

		private Font BuildFont(string name)
		{
			var builder = new FontBuilder(name);

			builder.AddFontSurface(
				new FontSettings(6, FontStyles.None),
				FontSurface.FromImpl(CreateFont(name, 6, FontStyles.None)));
			builder.AddFontSurface(
				new FontSettings(10, FontStyles.None),
				FontSurface.FromImpl(CreateFont(name, 10, FontStyles.None)));
			builder.AddFontSurface(
				new FontSettings(22, FontStyles.None),
				FontSurface.FromImpl(CreateFont(name, 22, FontStyles.None)));

			return builder.Build();
		}
	}
}
