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
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;

namespace AgateLib.Platform.Test.Display
{
	public class FakeSurfaceImpl : SurfaceImpl
	{
		Size size;
		PixelBuffer data;

		public SurfaceState LastDraw { get; private set; }

		public FakeSurfaceImpl(Size size)
		{
			this.size = size;
			data = new PixelBuffer(PixelFormat.ABGR8888, size);
		}
		protected override void Dispose(bool disposing)
		{
			data = null;
		}

		public override void Draw(SurfaceState state)
		{
			LastDraw = state.Clone();
		}

		public override void SaveTo(string filename, ImageFileFormat format)
		{
			throw new NotImplementedException();
		}

		public override SurfaceImpl CarveSubSurface(Rectangle srcRect)
		{
			var result = new FakeSurfaceImpl(srcRect.Size);
			result.data.CopyFrom(data, srcRect, new Point(), false);

			return result;
		}

		public override void SetSourceSurface(SurfaceImpl surf, Rectangle srcRect)
		{
			throw new NotImplementedException();
		}

		public override PixelBuffer ReadPixels(PixelFormat format, Rectangle rect)
		{
			PixelBuffer result = new PixelBuffer(format, rect.Size);

			result.CopyFrom(data, rect, Point.Empty, false, false);

			return result;
		}

		public override void WritePixels(PixelBuffer buffer)
		{
			data.CopyFrom(buffer, new Rectangle(Point.Empty, buffer.Size), Point.Empty, true);
		}

		public override Size SurfaceSize
		{
			get { return size; }
		}

		public override bool IsLoaded
		{
			get { return true; }
		}

		public override event EventHandler LoadComplete;
	}
}
