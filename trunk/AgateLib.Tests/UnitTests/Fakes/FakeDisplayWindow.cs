using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UnitTests.Fakes
{
	class FakeDisplayWindow : DisplayWindowImpl
	{
		private DisplayWindow owner;
		private CreateWindowParams windowParams;
		private FrameBufferImpl frameBuffer;
		private bool isDisposed;

		public FakeDisplayWindow(Size size)
		{
			frameBuffer = new FakeFrameBuffer(this);

			this.Size = size;
		}
		public FakeDisplayWindow(DisplayWindow owner, CreateWindowParams windowParams)
			:this(windowParams.Size)
		{
			// TODO: Complete member initialization
			this.owner = owner;
			this.windowParams = windowParams;
		}

		public DisplayWindow Owner
		{
			get { return owner; }
			set { owner = value; }
		}
		public override void Dispose()
		{
			isDisposed = true;
		}

		public override bool IsClosed
		{
			get { return isDisposed; }
		}

		public override bool IsFullScreen
		{
			get { return false; }
		}

		public override FrameBufferImpl FrameBuffer
		{
			get { return frameBuffer; }
		}

		public override Size Size { get;set;}

		public override string Title { get;set;}

		public override Point MousePosition { get;set;}
	}
}
