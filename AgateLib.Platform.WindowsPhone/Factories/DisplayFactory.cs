using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.Platform.WindowsPhone.DisplayImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsPhone.Factories
{
	class DisplayFactory : IDisplayFactory
	{
		private System.Windows.Controls.DrawingSurfaceBackgroundGrid renderTarget;
		SDX_Display mDisplayImpl;

		public DisplayFactory(SharpDX.SimpleInitializer.SharpDXContext context, System.Windows.Controls.DrawingSurfaceBackgroundGrid renderTarget)
		{
			mDisplayImpl = new SDX_Display(context, renderTarget);
			this.renderTarget = renderTarget;
		}

		public DisplayImpl DisplayImpl
		{
			get { return mDisplayImpl; }
		}

		public DisplayWindowImpl CreateDisplayWindow(DisplayWindow owner, CreateWindowParams windowParams)
		{
			return new SDX_DisplayWindow(owner, windowParams, renderTarget);
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
