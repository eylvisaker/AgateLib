//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
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
using AgateLib.Geometry;

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
				return new FakeSurface(new Size(10, 10));
			}
		}

		public SurfaceImpl CreateSurface(System.IO.Stream fileStream)
		{
			return new FakeSurface(new Size(10, 10));
		}

		public SurfaceImpl CreateSurface(Size surfaceSize)
		{
			return new FakeSurface(surfaceSize);
		}

		public FontSurfaceImpl CreateFont(string fontFamily, float sizeInPoints, AgateLib.DisplayLib.FontStyles style)
		{
			return new FakeFontSurface(fontFamily, sizeInPoints, style);
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
