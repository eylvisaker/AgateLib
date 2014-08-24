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
using System.Windows.Controls;
using System.Windows.Input;
using AgateLib.InputLib.Legacy;
using System.Diagnostics;

namespace AgateLib.Platform.WindowsPhone.DisplayImplementation
{
	public class SDX_DisplayWindow : DisplayWindowImpl
	{
		DisplayWindow mOwner;
		DrawingSurfaceBackgroundGrid mRenderTarget;
		bool mIsClosed = false;

		FrameBufferWindow mFrameBuffer;

		SDX_Display mDisplay;

		#region --- Creation / Destruction ---

		public SDX_DisplayWindow(DisplayWindow owner, CreateWindowParams windowParams,
			DrawingSurfaceBackgroundGrid renderTarget)
		{
			mOwner = owner;
			mRenderTarget = renderTarget;
			mDisplay = (SDX_Display)Display.Impl;

			AttachEvents();
			mFrameBuffer = new FrameBufferWindow(
				renderTarget.RenderSize.ToAgateSize(),
				null, owner, 
				mDisplay.D3D_Device.Context,
				windowParams.Coordinates);
			//CreateBackBuffer(mIsFullscreen);
		}

		public override void Dispose()
		{
			mFrameBuffer.Dispose();
			mIsClosed = true;
		}

		#endregion

		#region --- Event handlers ---

		private void DetachEvents()
		{
			if (mRenderTarget == null)
				return;

			throw new NotImplementedException();
		}

		private void AttachEvents()
		{
			mRenderTarget.SizeChanged += mRenderTarget_SizeChanged;

			mRenderTarget.MouseWheel += mRenderTarget_MouseWheel;
			mRenderTarget.MouseMove += mRenderTarget_MouseMove;
			mRenderTarget.MouseLeftButtonDown += mRenderTarget_MouseLeftButtonDown;
			mRenderTarget.MouseLeftButtonUp += mRenderTarget_MouseLeftButtonUp;
			mRenderTarget.DoubleTap += mRenderTarget_DoubleTap;

			mRenderTarget.KeyDown += mRenderTarget_KeyDown;
			mRenderTarget.KeyUp += mRenderTarget_KeyUp;

		}

		void mRenderTarget_KeyUp(object sender, KeyEventArgs e)
		{
			//Input.QueueInputEvent(AgateInputEventArgs.KeyUp(
			//	TransformKey(e.Key), e))
			//Keyboard.Keys[FormUtil.TransformWinFormsKey(e.KeyCode)] = false;
		}

		void mRenderTarget_KeyDown(object sender, KeyEventArgs e)
		{
			//Keyboard.Keys[FormUtil.TransformWinFormsKey(e.KeyCode)] = true;
		}

		void mRenderTarget_DoubleTap(object sender, GestureEventArgs e)
		{
			Mouse.OnMouseDoubleClick(MouseButton.Primary);
		}

		void mRenderTarget_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Debug.WriteLine("Mouse down at: {0}", e.GetPosition(mRenderTarget));
			Input.QueueInputEvent(AgateInputEventArgs.MouseDown(
				this, e.GetPosition(mRenderTarget).ToAgatePoint(), MouseButton.Primary));
		}
		void mRenderTarget_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Input.QueueInputEvent(AgateInputEventArgs.MouseUp(
				this, e.GetPosition(mRenderTarget).ToAgatePoint(), MouseButton.Primary));
		}
		void mRenderTarget_MouseMove(object sender, MouseEventArgs e)
		{
			Input.QueueInputEvent(AgateInputEventArgs.MouseMove(
				this, e.GetPosition(mRenderTarget).ToAgatePoint()));
		}


		void mRenderTarget_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
		{
			mFrameBuffer.SetSize(Size);
		}

		void mRenderTarget_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			Mouse.OnMouseWheel(-(e.Delta * 100) / 120);
		}


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
				return mRenderTarget.RenderSize.ToAgateSize();
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


		public override Point MousePosition { get;set;}
	}
}
