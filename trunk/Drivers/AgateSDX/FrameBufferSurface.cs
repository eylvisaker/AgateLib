using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D9;
using Direct3D = SlimDX.Direct3D9;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.ImplementationBase;

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
		
		public FrameBufferSurface(Size size)
		{
			mDisplay = Display.Impl as SDX_Display;
			mDevice = mDisplay.D3D_Device;
			mSize = size;

			mTexture = new Texture(mDevice.Device, mSize.Width, mSize.Height,
				0, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default);
			
			mRenderTarget = mTexture.GetSurfaceLevel(0);

			mAgateSurface = new SDX_Surface(new AgateLib.Utility.Ref<Texture>(mTexture),
				new Rectangle(Point.Empty, Size));

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

		public override bool CanAccessBackBuffer
		{
			get
			{
				return true;
			}
		}
		public override SurfaceImpl BackBuffer
		{
			get
			{
				return mAgateSurface;
			}
		}
	}
}
