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

using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;

namespace AgateLib.Platform.Test.Display
{
	public class FakeDisplayWindow : DisplayWindowImpl
	{
		private CreateWindowParams windowParams;
		private bool isDisposed;

		public FakeDisplayWindow(Size size)
		{
			FrameBuffer = new FakeFrameBuffer(this);

			Size = size;
		}

		public FakeDisplayWindow(DisplayWindow owner, CreateWindowParams windowParams)
			: this(windowParams.Resolution.Size)
		{
			// TODO: Complete member initialization
			Owner = owner;
			this.windowParams = windowParams;
		}

		protected override void Dispose(bool disposing)
		{
			isDisposed = true;

			base.Dispose(disposing);
		}

		public DisplayWindow Owner { get; set; }

		public override bool IsClosed => isDisposed;

		public override bool IsFullScreen => false;

		public override FrameBufferImpl FrameBuffer { get; }

		public override IResolution Resolution { get; set; }

		public override string Title { get; set; }
	}
}