using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DefaultAssets;

namespace AgateLib.Testing.Fakes
{
	class FakeDisplayDriver : DisplayImpl 
	{
		public override bool CapsBool(AgateLib.DisplayLib.DisplayBoolCaps caps)
		{
			throw new NotImplementedException();
		}
		public override double CapsDouble(DisplayLib.DisplayDoubleCaps caps)
		{
			throw new NotImplementedException();
		}

		public override AgateLib.Geometry.Size CapsSize(AgateLib.DisplayLib.DisplaySizeCaps displaySizeCaps)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<AgateLib.DisplayLib.Shaders.ShaderLanguage> SupportedShaderLanguages
		{
			get { throw new NotImplementedException(); }
		}

		public override AgateLib.DisplayLib.PixelFormat DefaultSurfaceFormat
		{
			get { throw new NotImplementedException(); }
		}

		protected override void OnRenderTargetChange(AgateLib.DisplayLib.FrameBuffer oldRenderTarget)
		{
		}

		protected override void OnRenderTargetResize()
		{
			throw new NotImplementedException();
		}


		protected override void OnBeginFrame()
		{
			throw new NotImplementedException();
		}

		protected override void OnEndFrame()
		{
			throw new NotImplementedException();
		}

		public override void SetClipRect(AgateLib.Geometry.Rectangle newClipRect)
		{
			throw new NotImplementedException();
		}

		public override void Clear(AgateLib.Geometry.Color color)
		{
			throw new NotImplementedException();
		}

		public override void Clear(AgateLib.Geometry.Color color, AgateLib.Geometry.Rectangle destRect)
		{
			throw new NotImplementedException();
		}

		public override void FillPolygon(AgateLib.Geometry.PointF[] pts, int startIndex, int length, AgateLib.Geometry.Color color)
		{
			throw new NotImplementedException();
		}

		public override void DrawLine(AgateLib.Geometry.Point a, AgateLib.Geometry.Point b, AgateLib.Geometry.Color color)
		{
			throw new NotImplementedException();
		}

		public override void DrawRect(AgateLib.Geometry.Rectangle rect, AgateLib.Geometry.Color color)
		{
			throw new NotImplementedException();
		}

		public override void DrawRect(AgateLib.Geometry.RectangleF rect, AgateLib.Geometry.Color color)
		{
			throw new NotImplementedException();
		}

		public override void FillRect(AgateLib.Geometry.Rectangle rect, AgateLib.Geometry.Color color)
		{
			throw new NotImplementedException();
		}

		public override void FillRect(AgateLib.Geometry.Rectangle rect, AgateLib.Geometry.Gradient color)
		{
			throw new NotImplementedException();
		}

		public override void FillRect(AgateLib.Geometry.RectangleF rect, AgateLib.Geometry.Color color)
		{
			throw new NotImplementedException();
		}

		public override void FillRect(AgateLib.Geometry.RectangleF rect, AgateLib.Geometry.Gradient color)
		{
			throw new NotImplementedException();
		}

		public override void FlushDrawBuffer()
		{
			throw new NotImplementedException();
		}

		public override void Initialize()
		{
		}

		protected override void ShowCursor()
		{
		}

		protected override void HideCursor()
		{
		}

		protected override bool GetRenderState(DisplayLib.RenderStateBool renderStateBool)
		{
			return true;
		}

		protected override void SetRenderState(DisplayLib.RenderStateBool renderStateBool, bool value)
		{
		}

		protected override DisplayLib.Shaders.Implementation.AgateShaderImpl CreateBuiltInShader(DisplayLib.Shaders.Implementation.BuiltInShader builtInShaderType)
		{
			return null;
		}
	}

	class FakeDisplayFactory : IDisplayFactory
	{
		public FakeDisplayFactory()
		{
			DisplayImpl = new FakeDisplayDriver();
		}

		public DisplayImpl DisplayImpl { get; private set;}

		public DisplayWindowImpl CreateDisplayWindow(AgateLib.DisplayLib.DisplayWindow owner, AgateLib.DisplayLib.CreateWindowParams windowParams)
		{
			return new FakeDisplayWindow(owner, windowParams);
		}

		public SurfaceImpl CreateSurface(IReadFileProvider fileProvider, string fileName)
		{
		    return new FakeSurface();
		}

		public SurfaceImpl CreateSurface(System.IO.Stream fileStream)
		{
			throw new NotImplementedException();
		}

		public SurfaceImpl CreateSurface(AgateLib.Geometry.Size surfaceSize)
		{
			throw new NotImplementedException();
		}

		public FontSurfaceImpl CreateFont(string fontFamily, float sizeInPoints, AgateLib.DisplayLib.FontStyles style)
		{
			throw new NotImplementedException();
		}

		public FontSurfaceImpl CreateFont(BitmapFontOptions bitmapOptions)
		{
			throw new NotImplementedException();
		}

		public FrameBufferImpl CreateFrameBuffer(AgateLib.Geometry.Size size)
		{
			throw new NotImplementedException();
		}


		public SurfaceImpl CreateSurface(DisplayLib.PixelBuffer pixels)
		{
			throw new NotImplementedException();
		}


		public Task InitializeDefaultResourcesAsync(DefaultResources res)
		{
			return Task.Run(() => { });
		}
	}

}
