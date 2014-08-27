using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.Platform.WindowsStoreCommon.DisplayImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsStoreCommon.Factories
{
	public class DisplayFactory : IDisplayFactory
	{
		IRenderTargetAdapter mRenderTarget;
		SDX_Display mDisplayImpl;

		public DisplayFactory(SharpDX.SimpleInitializer.SharpDXContext context, IRenderTargetAdapter renderTarget)
		{
			mDisplayImpl = new SDX_Display(context, renderTarget);
			this.mRenderTarget = renderTarget;
		}

		public DisplayImpl DisplayImpl
		{
			get { return mDisplayImpl; }
		}

		public DisplayWindowImpl CreateDisplayWindow(DisplayWindow owner, CreateWindowParams windowParams)
		{
			return new SDX_DisplayWindow(owner, windowParams, mRenderTarget);
		}
		public SurfaceImpl CreateSurface(IReadFileProvider provider, string fileName)
		{
			return new SDX_Surface(provider.ResolveFile(fileName));
		}
		public SurfaceImpl CreateSurface(Size surfaceSize)
		{
			return new SDX_Surface(surfaceSize);
		}
		public SurfaceImpl CreateSurface(System.IO.Stream fileStream)
		{
			return new SDX_Surface(fileStream);
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

	}
}
