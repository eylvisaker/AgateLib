using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib.ImplementationBase;
using SlimDX.Direct3D9;

namespace AgateSDX
{
	public abstract class SDX_FrameBuffer: FrameBufferImpl 
	{
		bool mHasDepth;
		bool mHasStencil;

		protected void SetHasDepthStencil(Format fmt)
		{
			switch (fmt)
			{
				case Format.D16:
				case Format.D16Lockable:
				case Format.D24X8:
				case Format.D32:
				case Format.D32Lockable:
				case Format.D32SingleLockable:
					mHasDepth = true;
					break;

				case Format.D15S1:
				case Format.D24S8:
				case Format.D24SingleS8:
				case Format.D24X4S4:
					mHasDepth = true;
					mHasStencil = true;
					break;

				case Format.S8Lockable:
					mHasStencil = true;
					break;
			}
		}

		public override bool HasDepthBuffer
		{
			get { return mHasDepth; }
		}
		public override bool HasStencilBuffer
		{
			get { return mHasStencil; }
		}
	}
}
