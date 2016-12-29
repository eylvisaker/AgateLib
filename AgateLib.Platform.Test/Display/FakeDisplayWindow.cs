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
	public class FakeDisplayWindow : DisplayWindowImpl
	{
		private DisplayWindow owner;
		private CreateWindowParams windowParams;
		private FrameBufferImpl frameBuffer;
		private bool isDisposed;

		public FakeDisplayWindow(Size size)
		{
			frameBuffer = new FakeFrameBuffer(this);

			this.Size = size;
		}
		public FakeDisplayWindow(DisplayWindow owner, CreateWindowParams windowParams)
			:this(windowParams.Size)
		{
			// TODO: Complete member initialization
			this.owner = owner;
			this.windowParams = windowParams;
		}

		public DisplayWindow Owner
		{
			get { return owner; }
			set { owner = value; }
		}
		public override void Dispose()
		{
			isDisposed = true;
		}

		public override bool IsClosed
		{
			get { return isDisposed; }
		}

		public override bool IsFullScreen
		{
			get { return false; }
		}

		public override FrameBufferImpl FrameBuffer
		{
			get { return frameBuffer; }
		}

		public override Size Size { get;set;}

		public override string Title { get;set;}
	}
}
