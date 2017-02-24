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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Drivers;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
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

		protected override DisplayLib.Shaders.Implementation.AgateShaderImpl CreateBuiltInShader(DisplayLib.Shaders.Implementation.BuiltInShader builtInShaderType)
		{
			return null;
		}

		public override IPrimitiveRenderer Primitives => this;

		public void DrawLines(LineType lineType, Color color, IEnumerable<Vector2f> points)
		{
		}

		public void FillPolygon(Color color, IEnumerable<Vector2f> points)
		{
		}
	}

}
