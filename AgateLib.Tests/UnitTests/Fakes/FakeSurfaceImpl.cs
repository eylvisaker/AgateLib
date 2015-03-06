using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;

namespace AgateLib.UnitTests.Fakes
{
    public class FakeSurfaceImpl : SurfaceImpl
    {
        Size size;
        PixelBuffer data;

        public SurfaceState LastDraw { get; private set; }

        public FakeSurfaceImpl(Size size)
        {
            this.size = size;
            data = new PixelBuffer(PixelFormat.ABGR8888, size);
        }
        protected override void Dispose(bool disposing)
        {
            data = null;
        }

        public override void Draw(SurfaceState state)
        {
            LastDraw = state.Clone();
        }

        public override void SaveTo(string filename, ImageFileFormat format)
        {
            throw new NotImplementedException();
        }

        public override SurfaceImpl CarveSubSurface(Rectangle srcRect)
        {
            var result = new FakeSurfaceImpl(srcRect.Size);
            result.data.CopyFrom(data, srcRect, new Point(), false);

            return result;
        }

        public override void SetSourceSurface(SurfaceImpl surf, Rectangle srcRect)
        {
            throw new NotImplementedException();
        }

        public override PixelBuffer ReadPixels(PixelFormat format, Rectangle rect)
        {
            PixelBuffer result = new PixelBuffer(format, rect.Size);

            result.CopyFrom(data, rect, Point.Empty, false, false);

            return result;
        }

        public override void WritePixels(PixelBuffer buffer)
        {
            data.CopyFrom(buffer, new Rectangle(Point.Empty, buffer.Size), Point.Empty,true);
        }

        public override Size SurfaceSize
        {
            get { return size; }
        }

        public override bool IsLoaded
        {
            get { return true; }
        }

        public override event EventHandler LoadComplete;
    }
}
