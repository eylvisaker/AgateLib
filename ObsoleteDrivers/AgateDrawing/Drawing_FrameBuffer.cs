//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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
		internal AgateLib.DisplayLib.DisplayWindow mAttachedWindow;
		SurfaceImpl mRenderTarget;

		public Drawing_FrameBuffer(Size size)
		{
			backBuffer = new Bitmap(size.Width, size.Height);
		}
		public Drawing_FrameBuffer(Bitmap bmp)
		{
			backBuffer = bmp;
		}

		public override bool HasDepthBuffer
		{
			get { return false; } 
		}
		public override bool HasStencilBuffer
		{
			get { return false; }
		}
		public override void Dispose()
		{
			if (mRenderTarget == null)
				backBuffer.Dispose();
		}

		public override Size Size
		{
			get { return AgateLib.WinForms.Interop.Convert(backBuffer.Size); }
		}

		public System.Drawing.Bitmap BackBufferBitmap
		{
			get { return backBuffer; }
			set
			{
				if (value == null)
					throw new ArgumentNullException();

				backBuffer = value;
			}
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
			get
			{
				if (mRenderTarget == null)
					mRenderTarget = new Drawing_Surface(backBuffer);

				return mRenderTarget;
			}
		}
		
		public override AgateLib.DisplayLib.DisplayWindow AttachedWindow
		{
			get { return mAttachedWindow; }
		}

		public event EventHandler EndRenderEvent;
	}
}
