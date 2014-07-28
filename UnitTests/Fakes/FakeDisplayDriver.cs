using AgateLib.DisplayLib.ImplementationBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UnitTests.Fakes
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

		public override DisplayWindowImpl CreateDisplayWindow(AgateLib.DisplayLib.DisplayWindow owner, AgateLib.DisplayLib.CreateWindowParams windowParams)
		{
			return new FakeDisplayWindow(owner, windowParams); 
		}

		public override SurfaceImpl CreateSurface(string fileName)
		{
			throw new NotImplementedException();
		}

		public override SurfaceImpl CreateSurface(System.IO.Stream fileStream)
		{
			throw new NotImplementedException();
		}

		public override SurfaceImpl CreateSurface(AgateLib.Geometry.Size surfaceSize)
		{
			throw new NotImplementedException();
		}

		public override FontSurfaceImpl CreateFont(string fontFamily, float sizeInPoints, AgateLib.DisplayLib.FontStyle style)
		{
			throw new NotImplementedException();
		}

		public override FontSurfaceImpl CreateFont(AgateLib.BitmapFont.BitmapFontOptions bitmapOptions)
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

		protected internal override void ShowCursor()
		{
			throw new NotImplementedException();
		}

		protected internal override void HideCursor()
		{
			throw new NotImplementedException();
		}

		protected internal override AgateLib.DisplayLib.Shaders.Implementation.AgateShaderImpl CreateBuiltInShader(AgateLib.DisplayLib.Shaders.Implementation.BuiltInShader builtInShaderType)
		{
			return null;
		}

		protected internal override FrameBufferImpl CreateFrameBuffer(AgateLib.Geometry.Size size)
		{
			throw new NotImplementedException();
		}

		protected internal override bool GetRenderState(AgateLib.DisplayLib.RenderStateBool renderStateBool)
		{
			throw new NotImplementedException();
		}

		protected internal override void SetRenderState(AgateLib.DisplayLib.RenderStateBool renderStateBool, bool value)
		{
			throw new NotImplementedException();
		}

		public override void Initialize()
		{
		}

		public override void Dispose()
		{
		}
	}
}
