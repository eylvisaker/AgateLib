using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Testing.Fakes
{
	public class FakeFrameBuffer : FrameBufferImpl
	{
		public FakeFrameBuffer(FakeDisplayWindow owner)
			: base(new NativeCoordinates())
		{
			this.Owner = owner;
		}
		public override void Dispose()
		{
			throw new NotImplementedException();
		}

		public override Geometry.Size Size
		{
			get { return Owner.Size; }
		}

		public override void BeginRender()
		{
			throw new NotImplementedException();
		}

		public override void EndRender()
		{
			throw new NotImplementedException();
		}

		public override bool HasDepthBuffer
		{
			get { throw new NotImplementedException(); }
		}

		public override bool HasStencilBuffer
		{
			get { throw new NotImplementedException(); }
		}

		public override DisplayLib.DisplayWindow AttachedWindow
		{
			get { return Owner.Owner; }
		}

		public FakeDisplayWindow Owner { get; private set; }
	}
}
