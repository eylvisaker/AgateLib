using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using Size = AgateLib.Geometry.Size;

namespace AgateDrawing
{
	class Drawing_FrameBuffer: FrameBufferImpl 
	{
		Bitmap backBuffer;

		public Drawing_FrameBuffer(Size size)
		{
			backBuffer = new Bitmap(size.Width, size.Height);
		}
		public Drawing_FrameBuffer(Bitmap bmp)
		{
			backBuffer = bmp;
		}

		public override void Dispose()
		{
			throw new NotImplementedException();
		}

		public override Size Size
		{
			get { return AgateLib.WinForms.Interop.Convert(backBuffer.Size); }
		}

		public System.Drawing.Bitmap BackBufferBitmap
		{
			get { return backBuffer; }
		}

		public override void BeginRender()
		{
		}
		public override void EndRender()
		{
			if (EndRenderEvent != null)
				EndRenderEvent(this, EventArgs.Empty);
		}
		public override bool CanAccessRenderTarget
		{
			get
			{
				return true;
			}
		}
		public override SurfaceImpl RenderTarget
		{
			get { return new Drawing_Surface(backBuffer, new System.Drawing.Rectangle(System.Drawing.Point.Empty, backBuffer.Size)); }
		}


		public event EventHandler EndRenderEvent;
	}
}
