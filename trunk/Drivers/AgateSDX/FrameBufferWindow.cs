using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D9;
using Direct3D = SlimDX.Direct3D9;
using AgateLib.Geometry;
using AgateLib.ImplementationBase;

namespace AgateSDX
{
	class FrameBufferWindow : SDX_FrameBuffer 
	{
		SDX_Display mDisplay;
		SwapChain mSwap;
		Direct3D.Surface mBackBuffer;
		Direct3D.Surface mBackDepthStencil;
		RenderToSurface mRenderToSurface;


		public FrameBufferWindow(SwapChain swap, Direct3D.Surface backBuffer, Direct3D.Surface backDepthStencil)
		{
			mDisplay = (SDX_Display)AgateLib.DisplayLib.Display.Impl;

			mSwap = swap;
			mBackBuffer = backBuffer;
			mBackDepthStencil = backDepthStencil;
		}

		public override void Dispose()
		{
			mSwap.Dispose();
			mBackBuffer.Dispose();
			mBackDepthStencil.Dispose();
		}

		public override Size Size
		{
			get { return new Size(mBackBuffer.Description.Width, mBackBuffer.Description.Height); }
		}

		public override void BeginRender()
		{
			mDisplay.D3D_Device.Device.SetRenderTarget(0, mBackBuffer);
			mDisplay.D3D_Device.Device.DepthStencilSurface = mBackDepthStencil;
			mDisplay.D3D_Device.Device.BeginScene();
		}

		public override void EndRender()
		{
			mDisplay.D3D_Device.Device.EndScene();

			mSwap.Present(Present.None);
		}
	}
}
