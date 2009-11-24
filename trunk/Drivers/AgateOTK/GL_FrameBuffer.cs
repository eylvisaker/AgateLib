using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.ImplementationBase;

namespace AgateOTK
{
	class GL_FrameBufferExt: FrameBufferImpl 
	{
		public GL_FrameBufferExt(Size size)
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
