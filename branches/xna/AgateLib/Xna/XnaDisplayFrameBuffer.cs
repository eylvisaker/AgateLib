
#if XNA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;

namespace AgateLib.Xna
{
	class XnaDisplayFrameBuffer : FrameBufferImpl 
	{
		Microsoft.Xna.Framework.Graphics.GraphicsDevice mDevice;

		public XnaDisplayFrameBuffer(Microsoft.Xna.Framework.Graphics.GraphicsDevice device)
		{
			mDevice = device;
		}

		public override void Dispose()
		{
		}

		public override AgateLib.Geometry.Size Size
		{
			get { return new Size(mDevice.PresentationParameters.BackBufferWidth, mDevice.PresentationParameters.BackBufferHeight); }
		}

		public override void BeginRender()
		{
		}

		public override void EndRender()
		{
			mDevice.Present();
		}
	}
}

#endif