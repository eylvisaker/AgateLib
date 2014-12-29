using System;
using System.Diagnostics.Contracts;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib.ImplementationBase
{
	[ContractClassFor(typeof(DisplayWindowImpl))]
	internal abstract class DisplayWindowImplContract : DisplayWindowImpl
	{
		public override FrameBufferImpl FrameBuffer
		{
			get
			{
				Contract.Ensures(Contract.Result<FrameBufferImpl>() != null);

				throw new NotImplementedException();
			}
		}

		public override bool IsClosed
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override bool IsFullScreen
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		[Obsolete]
		public override Point MousePosition
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

		public override Size Size
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

		public override void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}