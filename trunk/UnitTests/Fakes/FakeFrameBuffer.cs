using AgateLib.DisplayLib.ImplementationBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UnitTests.Fakes
{
	class FakeFrameBuffer : FrameBufferImpl
	{
		public FakeFrameBuffer(FakeDisplayWindow owner)
		{
			this.Owner = owner;
		}
		public override void Dispose()
		{
			throw new NotImplementedException();
		}

		public override Geometry.Size Size
		{
			get { throw new NotImplementedException(); }
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
