using AgateLib.DefaultAssets;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.Test
{
    public class FakeDisplayFactory : IDisplayFactory
    {
        public FakeDisplayFactory()
        {
            DisplayImpl = new FakeDisplayDriver();
        }

        public DisplayImpl DisplayImpl { get; private set; }

        public DisplayWindowImpl CreateDisplayWindow(AgateLib.DisplayLib.DisplayWindow owner, AgateLib.DisplayLib.CreateWindowParams windowParams)
        {
            return new FakeDisplayWindow(owner, windowParams);
        }

        public SurfaceImpl CreateSurface(IReadFileProvider fileProvider, string fileName)
        {
            return new FakeSurface();
        }

        public SurfaceImpl CreateSurface(System.IO.Stream fileStream)
        {
            throw new NotImplementedException();
        }

        public SurfaceImpl CreateSurface(AgateLib.Geometry.Size surfaceSize)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }


        public Task InitializeDefaultResourcesAsync(DefaultResources res)
        {
            return Task.Run(() => { });
        }
    }

}
