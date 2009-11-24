using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.ImplementationBase;

namespace AgateSDX
{
	class SDX_FrameBuffer: FrameBufferImpl 
	{
		public SDX_FrameBuffer(Size size)
		{
		}

		public override void Dispose()
		{
			throw new NotImplementedException();
		}

		public override Size Size
		{
			get { throw new NotImplementedException(); }
		}
	}
}
