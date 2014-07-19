using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
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

		public FakeDisplayWindow(DisplayWindow owner, CreateWindowParams windowParams)
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
		}

		public override bool IsClosed
		{
			get { throw new NotImplementedException(); }
		}

		public override bool IsFullScreen
		{
			get { throw new NotImplementedException(); }
		}

		public override FrameBufferImpl FrameBuffer
		{
			get { return new FakeFrameBuffer(this); }
		}

		public override void SetWindowed()
		{
			throw new NotImplementedException();
		}

		public override void SetFullScreen()
		{
			throw new NotImplementedException();
		}

		public override void SetFullScreen(int width, int height, int bpp)
		{
			throw new NotImplementedException();
		}

		public override Geometry.Size Size
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override string Title
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override Geometry.Point MousePosition
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}
}
