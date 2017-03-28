//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform.Test.Display.Shaders;
using AgateLib.Quality;

namespace AgateLib.Platform.Test.Display
{
	public class FakeDisplayDriver : DisplayImpl, IPrimitiveRenderer
	{
		private FakeScreenConfiguration screens;

		public FakeDisplayDriver()
		{
			screens = new FakeScreenConfiguration();
		}

		public override IScreenConfiguration Screens => screens;

		public FakeScreenConfiguration ScreenConfiguration
		{
			get { return screens; }
			set
			{
				Require.ArgumentNotNull(value, nameof(ScreenConfiguration));

				screens = value;
			}
		}

		public override bool CapsBool(AgateLib.DisplayLib.DisplayBoolCaps caps)
		{
			throw new NotImplementedException();
		}
		public override double CapsDouble(DisplayLib.DisplayDoubleCaps caps)
		{
			throw new NotImplementedException();
		}

		public override Size CapsSize(AgateLib.DisplayLib.DisplaySizeCaps displaySizeCaps)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<ShaderLanguage> SupportedShaderLanguages
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
		}


		protected override void OnBeginFrame()
		{
		}

		protected override void OnEndFrame()
		{
		}

		public override void SetClipRect(Rectangle newClipRect)
		{
		}

		public override void Clear(Color color)
		{
		}

		public override void Clear(Color color, Rectangle destRect)
		{
		}

		public override void FlushDrawBuffer()
		{
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

		protected override AgateShaderImpl CreateBuiltInShader(BuiltInShader builtInShaderType)
		{
			switch (builtInShaderType)
			{
				case BuiltInShader.Basic2DShader:
					return new FakeBasic2DImpl();

				case BuiltInShader.Lighting2D:
					return new FakeLighting2DImpl();

				case BuiltInShader.Lighting3D:
					return new FakeLighting3DImpl();

				default:
					return null;
			}
		}

		public override IPrimitiveRenderer Primitives => this;

		public void DrawLines(LineType lineType, Color color, IEnumerable<Vector2f> points)
		{
		}

		public void FillConvexPolygon(Color color, IEnumerable<Vector2f> points)
		{
		}
	}
}
