﻿//     The contents of this file are subject to the Mozilla Public License
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
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using SharpDX.DXGI;
using SharpDX.SimpleInitializer;
using AgateLib.DisplayLib;
using SharpDX.Direct3D11;

namespace AgateLib.Platform.WindowsPhone.DisplayImplementation
{
	class FrameBufferWindow : SDX_FrameBuffer
	{
		DisplayWindow mAttachedWindow;
		SDX_Display mDisplay;
		Size mSize;
		SharpDXContext mContext;

		public FrameBufferWindow(Size size,
			SwapChain swap,
			DisplayWindow attachedWindow,
			SharpDXContext context)
		{
			if (context == null) throw new ArgumentNullException();

			mDisplay = (SDX_Display)AgateLib.DisplayLib.Display.Impl;
			mAttachedWindow = attachedWindow;

			mContext = context;

			mSize = size;
		}

		public override void Dispose()
		{
			//mSwap.Dispose();
			//mBackBuffer.Dispose();
			//mBackDepthStencil.Dispose();
		}

		public override Size Size
		{
			get { return mSize; }
		}

		public void SetSize(Size size)
		{
			mSize = size;
		}

		public override RenderTargetView RenderTargetView
		{
			get { return mContext.BackBufferView; }
			protected set
			{
				throw new NotImplementedException();
			}
		}
		public override DepthStencilView DepthStencilView
		{
			get
			{
				return mContext.DepthStencilView;
			}
			protected set
			{
				throw new NotImplementedException();
			}
		}

		public override void BeginRender()
		{
			//mDisplay.D3D_Device.Device.SetRenderTarget(0, mBackBuffer);
			//mDisplay.D3D_Device.Device.DepthStencilSurface = mBackDepthStencil;
			//mDisplay.D3D_Device.Device.BeginScene();
		}

		public override void EndRender()
		{
			//mDisplay.D3D_Device.Device.EndScene();

			//mSwap.Present(Present.None);
		}

		public override AgateLib.DisplayLib.DisplayWindow AttachedWindow
		{
			get { return mAttachedWindow; }
		}
	}
}