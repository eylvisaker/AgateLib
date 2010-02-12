
#if XNA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using Microsoft.Xna.Framework.Graphics;
using Color = AgateLib.Geometry.Color;

namespace AgateLib.Xna
{
	class XnaDisplay : DisplayImpl 
	{
		PresentationParameters mPresentParams;
		GraphicsDevice mDevice;

		public override void Initialize()
		{

		}
		internal void CreateWindow(CreateWindowParams p, IntPtr handle)
		{
			if (mDevice != null)
				throw new AgateException("Cannot create more than one window.");

			mPresentParams = new PresentationParameters();

			mPresentParams.BackBufferFormat = SurfaceFormat.Bgr32;
			mPresentParams.BackBufferWidth = p.Width;
			mPresentParams.BackBufferHeight = p.Height;
			mPresentParams.SwapEffect = SwapEffect.Discard;

			mPresentParams.AutoDepthStencilFormat = DepthFormat.Depth24Stencil8;
			mPresentParams.EnableAutoDepthStencil = true;
			mPresentParams.PresentationInterval = PresentInterval.Default;
			mPresentParams.PresentOptions = PresentOptions.DiscardDepthStencil;

			mPresentParams.IsFullScreen = true;

#if !XBOX360
			mPresentParams.IsFullScreen = false;
#endif

			mDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, DeviceType.Hardware, handle, mPresentParams);
		}

		public override void Dispose()
		{
			mDevice.Dispose();
		}

		public override DisplayWindowImpl CreateDisplayWindow(CreateWindowParams windowParams)
		{
#if XBOX360
			CreateWindow(windowParams, IntPtr.Zero);
			return new XnaDisplayWindow(this, mDevice);
#else
			System.Windows.Forms.Form form = new System.Windows.Forms.Form();
			form.ClientSize = new System.Drawing.Size(windowParams.Width, windowParams.Height);

			CreateWindow(windowParams, form.Handle);

			return new XnaDisplayWindow(this, mDevice, form);
#endif

		}

		public override SurfaceImpl CreateSurface(string fileName)
		{
			throw new NotImplementedException();
		}

		public override SurfaceImpl CreateSurface(System.IO.Stream fileStream)
		{
			throw new NotImplementedException();
		}

		public override SurfaceImpl CreateSurface(Size surfaceSize)
		{
			throw new NotImplementedException();
		}

		public override FontSurfaceImpl CreateFont(string fontFamily, float sizeInPoints, FontStyle style)
		{
			throw new NotImplementedException();
		}

		public override FontSurfaceImpl CreateFont(AgateLib.BitmapFont.BitmapFontOptions bitmapOptions)
		{
			throw new NotImplementedException();
		}


		public override bool CapsBool(DisplayBoolCaps caps)
		{
			throw new NotImplementedException();
		}

		public override Size CapsSize(DisplaySizeCaps displaySizeCaps)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<AgateLib.DisplayLib.Shaders.ShaderLanguage> SupportedShaderLanguages
		{
			get { throw new NotImplementedException(); }
		}

		public override PixelFormat DefaultSurfaceFormat
		{
			get { throw new NotImplementedException(); }
		}

		protected override void OnRenderTargetChange(FrameBuffer oldRenderTarget)
		{
			
		}

		protected override void OnRenderTargetResize()
		{
			throw new NotImplementedException();
		}

		protected override void OnBeginFrame()
		{
		}

		protected override void OnEndFrame()
		{
			mDevice.Present();
		}

		public override void SetClipRect(Rectangle newClipRect)
		{
			throw new NotImplementedException();
		}

		public override void Clear(Color color)
		{
			mDevice.Clear(Convert(color));
		}

		public override void Clear(Color color, Rectangle dest)
		{
			throw new NotImplementedException();
		}

		public override void FillPolygon(PointF[] pts, int startIndex, int length, Color color)
		{
			throw new NotImplementedException();
		}

		public override void DrawLine(Point a, Point b, Color color)
		{
			throw new NotImplementedException();
		}

		public override void DrawRect(Rectangle rect, Color color)
		{
			throw new NotImplementedException();
		}

		public override void DrawRect(RectangleF rect, Color color)
		{
			throw new NotImplementedException();
		}

		public override void FillRect(Rectangle rect, Color color)
		{
			throw new NotImplementedException();
		}

		public override void FillRect(Rectangle rect, Gradient color)
		{
			throw new NotImplementedException();
		}

		public override void FillRect(RectangleF rect, Color color)
		{
			throw new NotImplementedException();
		}

		public override void FillRect(RectangleF rect, Gradient color)
		{
			throw new NotImplementedException();
		}

		public override void FlushDrawBuffer()
		{
		}

		protected internal override void ProcessEvents()
		{
#if !XBOX360
			System.Windows.Forms.Application.DoEvents();
#endif
		}

		protected internal override void ShowCursor()
		{
			throw new NotImplementedException();
		}

		protected internal override void HideCursor()
		{
			throw new NotImplementedException();
		}

		protected internal override AgateLib.DisplayLib.Shaders.Implementation.AgateShaderImpl CreateBuiltInShader(AgateLib.DisplayLib.Shaders.Implementation.BuiltInShader BuiltInShaderType)
		{
			switch (BuiltInShaderType)
			{
				case AgateLib.DisplayLib.Shaders.Implementation.BuiltInShader.Basic2DShader:
					return new Shaders.XnaBasic2D();

				case AgateLib.DisplayLib.Shaders.Implementation.BuiltInShader.Lighting2D:
				case AgateLib.DisplayLib.Shaders.Implementation.BuiltInShader.Lighting3D:

				default:
					return null;
			}
		}

		protected internal override FrameBufferImpl CreateFrameBuffer(Size size)
		{
			throw new NotImplementedException();
		}

		protected internal override bool GetRenderState(RenderStateBool renderStateBool)
		{
			throw new NotImplementedException();
		}

		protected internal override void SetRenderState(RenderStateBool renderStateBool, bool value)
		{
			switch (renderStateBool)
			{
				case RenderStateBool.AlphaBlend:
					mDevice.RenderState.AlphaBlendEnable = value;
					break;

				default:
					throw new NotSupportedException();
			}
		}



		internal Microsoft.Xna.Framework.Graphics.Color Convert(Color color)
		{
			return new Microsoft.Xna.Framework.Graphics.Color(color.R, color.G, color.B, color.A);
		}

	}
}

#endif