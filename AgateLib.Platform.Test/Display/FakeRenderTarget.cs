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
using AgateLib.Geometry;
using AgateLib.Geometry.CoordinateSystems;

namespace AgateLib.Platform.Test.Display
{
	public class FakeRenderTarget : IFrameBuffer
	{
		public FakeRenderTarget()
		{
			CoordinateSystem = new NativeCoordinates();
			CoordinateSystem.RenderTargetSize = Size;
		}

		public int Height { get { return 400; } }
		public int Width { get { return 640; } }
		public Size Size { get { return new Size(Width, Height); } }

		public ICoordinateSystem CoordinateSystem { get; set; }
	}
}
