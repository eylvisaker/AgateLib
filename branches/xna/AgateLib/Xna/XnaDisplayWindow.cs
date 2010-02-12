
#if XNA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Xna
{
	class XnaDisplayWindow : DisplayWindowImpl 
	{
		XnaDisplay mDisplay;
		GraphicsDevice mDevice;
		XnaDisplayFrameBuffer mFrameBuffer;

		public XnaDisplayWindow(XnaDisplay disp, GraphicsDevice device)
		{
			mDisplay = disp;
			mDevice = device;

			mFrameBuffer = new XnaDisplayFrameBuffer(mDevice);
		}

#if !XBOX360
		System.Windows.Forms.Form mForm;

		public XnaDisplayWindow(XnaDisplay disp, GraphicsDevice device, System.Windows.Forms.Form form) : this(disp, device)
		{
			mForm = form;

			mForm.Show();
		}

		public override void Dispose()
		{
			mForm.Dispose();
		}
		public override string Title
		{
			get
			{
				return mForm.Text;
			}
			set
			{
				mForm.Text = value;
			}
		}
		public override bool IsClosed
		{
			get
			{
				return mForm.IsDisposed;
			}
		}
#endif

#if XBOX360
		public override void Dispose()
		{
		}

		public override bool IsClosed
		{
			get { return false; }
		}

		public override string Title
		{
			get
			{
				return "AgateLib XNA Game";
			}
			set
			{
			}
		}
#endif

		

		public override bool IsFullScreen
		{
			get { return mDevice.PresentationParameters.IsFullScreen; }
		}

		public override FrameBufferImpl FrameBuffer
		{
			get { return mFrameBuffer; }
		}

		public override void SetWindowed()
		{
		}
		public override void SetFullScreen()
		{
		}
		public override void SetFullScreen(int width, int height, int bpp)
		{
		}

		public override AgateLib.Geometry.Size Size
		{
			get
			{
				return mFrameBuffer.Size;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public override AgateLib.Geometry.Point MousePosition
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

#endif
