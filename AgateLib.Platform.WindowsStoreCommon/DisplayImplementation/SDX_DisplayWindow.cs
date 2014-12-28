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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using AgateLib.InputLib;
using System.Windows.Input;
using AgateLib.InputLib.Legacy;
using System.Diagnostics;
using AgateLib.Platform.WindowsStore;

namespace AgateLib.Platform.WindowsStore.DisplayImplementation
{
	public class SDX_DisplayWindow : DisplayWindowImpl
	{
		DisplayWindow mOwner;
		IRenderTargetAdapter mRenderTarget;
		bool mIsClosed = false;

		FrameBufferWindow mFrameBuffer;

		SDX_Display mDisplay;

		#region --- Creation / Destruction ---

		public SDX_DisplayWindow(DisplayWindow owner, CreateWindowParams windowParams,
			IRenderTargetAdapter renderTarget)
		{
			mOwner = owner;
			mRenderTarget = renderTarget;
			mDisplay = (SDX_Display)Display.Impl;

			AttachEvents();
			mFrameBuffer = new FrameBufferWindow(
				renderTarget.Size,
				null, owner, 
				mDisplay.D3D_Device.Context,
				renderTarget,
				windowParams.Coordinates);
			//CreateBackBuffer(mIsFullscreen);
		}

		private void AttachEvents()
		{
			mRenderTarget.Disposed += renderTarget_Disposed;
		}

		public override void Dispose()
		{
			mFrameBuffer.Dispose();
			mIsClosed = true;
		}

		#endregion

		#region --- Event handlers ---

		void renderTarget_Disposed(object sender, EventArgs e)
		{
			mIsClosed = true;
		}


		#endregion

		public override FrameBufferImpl FrameBuffer
		{
			get { return mFrameBuffer; }
		}

		public override bool IsClosed
		{
			get { return mIsClosed; }
		}
		public override Size Size
		{
			get
			{
				return mRenderTarget.Size;
			}
			set
			{
				throw new NotImplementedException();
			}
		}


		public override bool IsFullScreen
		{
			get { return true; }
		}

		public override string Title { get;set;}

		[Obsolete]
		public override Point MousePosition { get;set;}
	}
}
