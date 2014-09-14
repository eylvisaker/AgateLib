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
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using SharpDX.Direct3D11;
using AgateLib.Geometry.CoordinateSystems;

namespace AgateLib.Platform.WindowsStore.DisplayImplementation
{
	class FrameBufferSurface : SDX_FrameBuffer
	{
		Size mSize;
		SDX_Display mDisplay;
		D3DDevice mDevice;
		SDX_Surface mAgateSurface;
		Texture2D mTexture;
		RenderTargetView mRenderTargetView;
		bool mHasDepth;
		bool mHasStencil;

		public FrameBufferSurface(Size size)
			: base(new NativeCoordinates())
		{
			mDisplay = Display.Impl as SDX_Display;
			mDevice = mDisplay.D3D_Device;
			mSize = size;

			mAgateSurface = new SDX_Surface(mSize);
			mTexture = mAgateSurface.D3dTexture;

			var rtvdesc = new RenderTargetViewDescription
			{
				Format = mTexture.Description.Format,
				Dimension = RenderTargetViewDimension.Texture2D,
			};

			mRenderTargetView = new RenderTargetView(mDevice.Device, mTexture, rtvdesc);
		}

		public override void Dispose()
		{
			mRenderTargetView.Dispose();
		}

		public override AgateLib.Geometry.Size Size
		{
			get { return mSize; }
		}

		public override void BeginRender()
		{
			mDevice.DeviceContext.OutputMerger.SetRenderTargets(mRenderTargetView);
		}

		public override void EndRender()
		{
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
