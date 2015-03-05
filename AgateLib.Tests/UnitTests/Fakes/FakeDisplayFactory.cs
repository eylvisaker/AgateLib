using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.Drivers;

namespace AgateLib.UnitTests.Fakes
{
    public class FakeDisplayFactory : IDisplayFactory
    {
        public AgateLib.DisplayLib.ImplementationBase.DisplayImpl DisplayImpl
        {
            get { throw new NotImplementedException(); }
        }

        public AgateLib.DisplayLib.ImplementationBase.DisplayWindowImpl CreateDisplayWindow(AgateLib.DisplayLib.DisplayWindow owner, AgateLib.DisplayLib.CreateWindowParams windowParams)
        {
            throw new NotImplementedException();
        }

        public AgateLib.DisplayLib.ImplementationBase.SurfaceImpl CreateSurface(IReadFileProvider provider, string filename)
        {
            throw new NotImplementedException();
        }

        public AgateLib.DisplayLib.ImplementationBase.SurfaceImpl CreateSurface(System.IO.Stream fileStream)
        {
            throw new NotImplementedException();
        }

        public AgateLib.DisplayLib.ImplementationBase.SurfaceImpl CreateSurface(AgateLib.Geometry.Size surfaceSize)
        {
            throw new NotImplementedException();
        }

        public AgateLib.DisplayLib.ImplementationBase.SurfaceImpl CreateSurface(AgateLib.DisplayLib.PixelBuffer pixels)
        {
            throw new NotImplementedException();
        }

        public AgateLib.DisplayLib.ImplementationBase.FrameBufferImpl CreateFrameBuffer(AgateLib.Geometry.Size size)
        {
            throw new NotImplementedException();
        }

        public Task InitializeDefaultResourcesAsync(DefaultAssets.DefaultResources res)
        {
            throw new NotImplementedException();
        }
    }
}
