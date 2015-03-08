using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgateLib.DisplayLib.ImplementationBase;

namespace AgateLib.Platform.Test
{
    public class FakeSurface : SurfaceImpl
    {
        protected override void Dispose(bool disposing)
        {
        }

        public override void Draw(DisplayLib.SurfaceState state)
        {
            throw new NotImplementedException();
        }

        public override void SaveTo(string filename, DisplayLib.ImageFileFormat format)
        {
            throw new NotImplementedException();
        }

        public override SurfaceImpl CarveSubSurface(Geometry.Rectangle srcRect)
        {
            throw new NotImplementedException();
        }

        public override void SetSourceSurface(SurfaceImpl surf, Geometry.Rectangle srcRect)
        {
            throw new NotImplementedException();
        }

        public override DisplayLib.PixelBuffer ReadPixels(DisplayLib.PixelFormat format, Geometry.Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public override void WritePixels(DisplayLib.PixelBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public override Geometry.Size SurfaceSize
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsLoaded
        {
            get { throw new NotImplementedException(); }
        }

        public override event EventHandler LoadComplete;
    }
}
