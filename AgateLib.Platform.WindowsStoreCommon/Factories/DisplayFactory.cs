using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.Platform.WindowsStore.DisplayImplementation;
using AgateLib.Platform.WindowsStore.PlatformImplementation;
using AgateLib.Resources.Legacy;
using SharpDX.SimpleInitializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DefaultAssets;

namespace AgateLib.Platform.WindowsStore.Factories
{
	public class DisplayFactory : IDisplayFactory
	{
		SDX_Display mDisplayImpl;

		public DisplayFactory()
		{
			mDisplayImpl = new SDX_Display();
		}

		public DisplayImpl DisplayImpl
		{
			get { return mDisplayImpl; }
		}
		public IRenderTargetAdapter RenderTargetAdapter
		{
			get { return mDisplayImpl.RenderTargetAdapter; }
			set { mDisplayImpl.ResetRenderTarget(value); }
		}

		public DisplayWindowImpl CreateDisplayWindow(DisplayWindow owner, CreateWindowParams windowParams)
		{
			return new SDX_DisplayWindow(owner, windowParams, RenderTargetAdapter);
		}
		public SurfaceImpl CreateSurface(IReadFileProvider provider, string filename)
		{
			return new SDX_Surface(provider, filename);
		}
		public SurfaceImpl CreateSurface(Size surfaceSize)
		{
			return new SDX_Surface(surfaceSize);
		}
		public SurfaceImpl CreateSurface(System.IO.Stream fileStream)
		{
			return new SDX_Surface(fileStream);
		}
		public SurfaceImpl CreateSurface(PixelBuffer pixels)
		{
			return new SDX_Surface(pixels);
		}

		public FontSurfaceImpl CreateFont(string fontFamily, float sizeInPoints, FontStyles style)
		{
			BitmapFontOptions options = new BitmapFontOptions(fontFamily, sizeInPoints, style);

			throw new NotImplementedException();
			//return BitmapFontUtil.ConstructFromOSFont(options);
		}
		public FontSurfaceImpl CreateFont(BitmapFontOptions bitmapOptions)
		{
			throw new NotImplementedException();
			//return BitmapFontUtil.ConstructFromOSFont(bitmapOptions);
		}
		public FrameBufferImpl CreateFrameBuffer(Size size)
		{
			return new FrameBufferSurface(size);
		}

		AgateResourceCollection resources;

		public async Task InitializeDefaultResourcesAsync(DefaultResources res)
		{
			if (Display.Impl == null) return;
			var display = (SDX_Display)Display.Impl;
			if (display.D3D_Device == null) return;

			if (resources == null)
			{
				var assets = new WindowsStoreAssetFileProvider("AgateLib.Platform.WindowsStoreCommon/Assets");

				resources = new AgateResourceCollection();
				AgateResourceLoader.LoadResources(resources, 
					await assets.OpenReadAsync("Resources.xml").ConfigureAwait(false));

				resources.FileProvider = assets;
			}
	
			res.AgateSans = new Font("AgateSans");
			res.AgateSerif = new Font("AgateSerif");
			res.AgateMono = new Font("AgateMono");

			res.AgateSans.AddFont(new FontSurface(resources, "AgateSans-10"), 10, FontStyles.None);
			res.AgateSans.AddFont(new FontSurface(resources, "AgateSans-14"), 14, FontStyles.None);
			res.AgateSans.AddFont(new FontSurface(resources, "AgateSans-24"), 24, FontStyles.None);

			res.AgateSerif.AddFont(new FontSurface(resources, "AgateSerif-10"), 10, FontStyles.None);
			res.AgateSerif.AddFont(new FontSurface(resources, "AgateSerif-14"), 14, FontStyles.None);

			res.AgateMono.AddFont(new FontSurface(resources, "AgateMono-10"), 10, FontStyles.None);
		}
	}
}
