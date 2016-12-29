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
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;

namespace AgateLib.Platform.Test.Display
{
	public class FakeSurface : SurfaceImpl
	{
		private readonly Size size;

		public FakeSurface(Size size)
		{
			this.size = size;
		}

		protected override void Dispose(bool disposing)
		{
		}

		public override void Draw(DisplayLib.SurfaceState state)
		{
		}

		public override void SaveTo(string filename, DisplayLib.ImageFileFormat format)
		{
			throw new NotImplementedException();
		}

		public override SurfaceImpl CarveSubSurface(Rectangle srcRect)
		{
			throw new NotImplementedException();
		}

		public override void SetSourceSurface(SurfaceImpl surf, Rectangle srcRect)
		{
			throw new NotImplementedException();
		}

		public override DisplayLib.PixelBuffer ReadPixels(DisplayLib.PixelFormat format, Rectangle rect)
		{
			throw new NotImplementedException();
		}

		public override void WritePixels(DisplayLib.PixelBuffer buffer)
		{
			throw new NotImplementedException();
		}

		public override Size SurfaceSize
		{
			get { return size; }
		}

		public override bool IsLoaded
		{
			get { throw new NotImplementedException(); }
		}

		public override event EventHandler LoadComplete;
	}
}
