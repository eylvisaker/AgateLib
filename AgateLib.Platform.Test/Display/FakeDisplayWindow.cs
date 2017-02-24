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
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Platform.Test.Display
{
	public class FakeDisplayWindow : DisplayWindowImpl
	{
		private CreateWindowParams windowParams;
		private bool isDisposed;

		public FakeDisplayWindow(Size size)
		{
			FrameBuffer = new FakeFrameBuffer(this);

			Resolution = new Resolution(size);
		}

		public FakeDisplayWindow(DisplayWindow owner, CreateWindowParams windowParams)
			: this(windowParams.Resolution?.Size ?? Size.Empty)
		{
			// TODO: Complete member initialization
			Owner = owner;
			Resolution = windowParams.Resolution?.Clone() ?? new Resolution(10, 10);
			this.windowParams = windowParams;

			Screen = windowParams.TargetScreen;
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

		public sealed override IResolution Resolution { get; set; }

		public override Size PhysicalSize => Resolution?.Size ?? Size.Empty;

		public override string Title { get; set; }

		public override ScreenInfo Screen { get; }
	}
}