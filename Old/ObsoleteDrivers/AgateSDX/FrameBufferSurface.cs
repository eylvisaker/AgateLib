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
using SlimDX;
using SlimDX.Direct3D9;
using Direct3D = SlimDX.Direct3D9;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;

namespace AgateSDX
{
	class FrameBufferSurface : SDX_FrameBuffer 
	{
		Size mSize;
		SDX_Display mDisplay;
		D3DDevice mDevice;
		SDX_Surface mAgateSurface;
		Direct3D.Texture mTexture;
		Direct3D.Surface mRenderTarget;
		bool mHasDepth;
		bool mHasStencil;

		public FrameBufferSurface(Size size)
		{
			mDisplay = Display.Impl as SDX_Display;
			mDevice = mDisplay.D3D_Device;
			mSize = size;

			try
			{
				mTexture = new Texture(mDevice.Device, mSize.Width, mSize.Height,
					0, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default);
			}
			catch
			{
				Size newSize = SDX_Surface.NextPowerOfTwo(mSize);

				mTexture = new Texture(mDevice.Device, newSize.Width, newSize.Height,
					0, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default);
			}

			mRenderTarget = mTexture.GetSurfaceLevel(0);

			mAgateSurface = new SDX_Surface(new AgateLib.Utility.Ref<Texture>(mTexture),
				new Rectangle(Point.Empty, Size));

			//SetHasDepthStencil(
		}

		public override void Dispose()
		{
			mRenderTarget.Dispose();
		}

		public override AgateLib.Geometry.Size Size
		{
			get { return mSize; }
		}

		public override void BeginRender()
		{
			mDevice.Device.SetRenderTarget(0, mRenderTarget);
			mDevice.Device.BeginScene();
		}

		public override void EndRender()
		{
			mDevice.Device.EndScene();
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
				return mAgateSurface;
			}
		}

		public override DisplayWindow AttachedWindow
		{
			get
			{
				return null;
			}
		}
	}
}
