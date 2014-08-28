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
using Drawing = System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.WinForms;
using SlimDX;
using SlimDX.Direct3D9;
using Direct3D = SlimDX.Direct3D9;

namespace AgateSDX
{
	public class SDX_DisplayWindow : DisplayWindowImpl
	{
		DisplayWindow mOwner;
		Form frm;
		Control mRenderTarget;
		bool mIsClosed = false;
		bool mIsFullscreen = false;

		FrameBufferWindow mFrameBuffer;

		int mChooseWidth;
		int mChooseHeight;
		int mChooseBitDepth = 32;
		System.Drawing.Icon mIcon;
		bool mChooseResize = false;
		WindowPosition mChoosePosition;
		bool mHasFrame = true;
		string mTitle = "";

		SDX_Display mDisplay;

		#region --- Creation / Destruction ---

		public SDX_DisplayWindow(DisplayWindow owner, CreateWindowParams windowParams)
		{
			mChoosePosition = windowParams.WindowPosition;
			mOwner = owner;

			if (windowParams.RenderToControl)
			{
				if (typeof(Control).IsAssignableFrom(windowParams.RenderTarget.GetType()) == false)
					throw new ArgumentException("The specified render target does not derive from System.Windows.Forms.Control");

				mRenderTarget = (Control)windowParams.RenderTarget;

				mIsFullscreen = false;
				mChooseWidth = mRenderTarget.ClientSize.Width;
				mChooseHeight = mRenderTarget.ClientSize.Height;

				mDisplay = Display.Impl as SDX_Display;
				mDisplay.Initialize(this, windowParams);
				mDisplay.VSyncChanged += new EventHandler(mDisplay_VSyncChanged);

			}
			else
			{
				if (string.IsNullOrEmpty(windowParams.IconFile) == false)
					mIcon = new Drawing.Icon(windowParams.IconFile);

				mTitle = windowParams.Title;
				mIsFullscreen = windowParams.IsFullScreen;
				mChooseWidth = windowParams.Width;
				mChooseHeight = windowParams.Height;
				mChooseResize = windowParams.IsResizable;
				mHasFrame = windowParams.HasFrame;

				CreateWindow(mIsFullscreen);

				mDisplay = Display.Impl as SDX_Display;
				mDisplay.Initialize(this, windowParams);
				mDisplay.VSyncChanged += new EventHandler(mDisplay_VSyncChanged);
			}

			AttachEvents();
			CreateBackBuffer(mIsFullscreen);
		}

		public SDX_DisplayWindow(System.Windows.Forms.Control renderTarget)
		{

		}

		public override void Dispose()
		{
			if (IsFullScreen)
			{
				SetWindowed();
			}

			mFrameBuffer.Dispose();

			if (frm != null && frm is frmFullScreen == false)
			{
				frm.Dispose();
			}

			frm = null;
			mIsClosed = true;
		}

		#endregion

		#region --- Event handlers ---

		private void AttachEvents()
		{
			mRenderTarget.Resize += new EventHandler(frm_Resize);

			mRenderTarget.MouseWheel += new MouseEventHandler(mRenderTarget_MouseWheel);
			mRenderTarget.MouseMove += new System.Windows.Forms.MouseEventHandler(pct_MouseMove);
			mRenderTarget.MouseDown += new System.Windows.Forms.MouseEventHandler(pct_MouseDown);
			mRenderTarget.MouseUp += new System.Windows.Forms.MouseEventHandler(pct_MouseUp);
			mRenderTarget.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(pct_MouseDoubleClick);

			System.Windows.Forms.Form form;

			if (mRenderTarget is System.Windows.Forms.Form)
				form = mRenderTarget as System.Windows.Forms.Form;
			else
				form = (mRenderTarget.TopLevelControl as System.Windows.Forms.Form);

			if (form == null)
			{
				throw new ArgumentException("Could not find a top-level Form.  It appears the render target is not attached to a Form yet.");
			}

			form.KeyPreview = true;
			form.KeyDown += new System.Windows.Forms.KeyEventHandler(form_KeyDown);
			form.KeyUp += new System.Windows.Forms.KeyEventHandler(form_KeyUp);

			form.FormClosing += new System.Windows.Forms.FormClosingEventHandler(form_FormClosing);
			form.FormClosed += new System.Windows.Forms.FormClosedEventHandler(form_FormClosed);

		}

		void mRenderTarget_MouseWheel(object sender, MouseEventArgs e)
		{
			Mouse.OnMouseWheel(-(e.Delta * 100) / 120);
		}
		private void DetachEvents()
		{
			if (mRenderTarget == null)
				return;

			mRenderTarget.Resize -= new EventHandler(frm_Resize);

			mRenderTarget.MouseMove -= new System.Windows.Forms.MouseEventHandler(pct_MouseMove);
			mRenderTarget.MouseDown -= new System.Windows.Forms.MouseEventHandler(pct_MouseDown);
			mRenderTarget.MouseUp -= new System.Windows.Forms.MouseEventHandler(pct_MouseUp);
			mRenderTarget.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(pct_MouseDoubleClick);

			System.Windows.Forms.Form form = (mRenderTarget.TopLevelControl as System.Windows.Forms.Form);

			form.KeyDown -= new System.Windows.Forms.KeyEventHandler(form_KeyDown);
			form.KeyUp -= new System.Windows.Forms.KeyEventHandler(form_KeyUp);

			form.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(form_FormClosing);
			form.FormClosed -= new System.Windows.Forms.FormClosedEventHandler(form_FormClosed);
		}


		void mDisplay_VSyncChanged(object sender, EventArgs e)
		{
			CreateBackBuffer(mIsFullscreen);
		}


		void form_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
		{
			mIsClosed = true;
			OnClosed();

		}
		void form_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			bool cancel = e.Cancel;
			OnClosing(ref cancel);

			e.Cancel = cancel;
		}

		void form_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			Keyboard.Keys[FormUtil.TransformWinFormsKey(e.KeyCode)] = false;
		}
		void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			Keyboard.Keys[FormUtil.TransformWinFormsKey(e.KeyCode)] = true;
		}

		void pct_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Mouse.MouseButtons btn = GetButtons(e.Button);

			Mouse.OnMouseDoubleClick(btn);
		}
		void pct_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Mouse.MouseButtons btn = GetButtons(e.Button);

			Mouse.Buttons[btn] = false;
		}
		void pct_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Mouse.MouseButtons btn = GetButtons(e.Button);

			Mouse.Buttons[btn] = true;
		}
		void pct_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			SetInternalMousePosition(Interop.Convert(e.Location));
		}
		void renderTarget_Disposed(object sender, EventArgs e)
		{
			mIsClosed = true;
		}


		void frm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
		{
			mIsClosed = true;
		}
		void frm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{

		}
		void frm_Resize(object sender, EventArgs e)
		{
			//if (mIsFullscreen)
			//    return;


			mChooseWidth = RenderTarget.ClientSize.Width;
			mChooseHeight = RenderTarget.ClientSize.Height;

			OnResize();
		}


		private void OnResize()
		{
			if (mChooseWidth == 0 || mChooseHeight == 0)
				return;

			CreateBackBuffer(mIsFullscreen);

		}

		#endregion

		public override FrameBufferImpl FrameBuffer
		{
			get { return mFrameBuffer; }
		}

		/// <summary>
		/// Creates a window.  Events are not attached or detached in this method,
		/// and the old form is not disposed.
		/// </summary>
		/// <param name="fullScreenForm"></param>
		private void CreateWindow(bool fullScreenForm)
		{
			if (fullScreenForm)
			{
				frm = new frmFullScreen();
				frm.Show();

				frm.Text = mTitle;
				frm.Icon = mIcon;
				frm.TopLevel = true;

				mRenderTarget = frm;

				frm.Location = System.Drawing.Point.Empty;
			}
			else
			{
				FormUtil.InitializeWindowsForm(out frm, out mRenderTarget, mChoosePosition, mTitle,
					mChooseWidth, mChooseHeight, mIsFullscreen, mChooseResize, mHasFrame);

				if (mIcon != null)
					frm.Icon = mIcon;

				frm.Show();
			}

			frm.ClientSize = new System.Drawing.Size(mChooseWidth, mChooseHeight);
		}
		private void CreateWindowedDisplay()
		{
			DetachEvents();

			Form oldForm = frm;

			CreateWindow(false);
			CreateBackBuffer(false);

			if (oldForm != null)
				oldForm.Dispose();

			AttachEvents();

			Core.IsActive = true;
			mIsFullscreen = false;
		}

		private void CreateFullScreenDisplay()
		{
			DetachEvents();

			if (frm is frmFullScreen == false)
			{
				if (mFrameBuffer != null)
					mFrameBuffer.Dispose();

				frm.Dispose();

				CreateWindow(true);
			}

			frm.Activate();
			frm.Refresh();

			CreateBackBuffer(true);

			frm.ClientSize = new System.Drawing.Size(mChooseWidth, mChooseHeight);

			System.Threading.Thread.Sleep(500);


			frm.Activate();

			AttachEvents();
			mIsFullscreen = true;
			Core.IsActive = true;
		}


		private void CreateBackBuffer(bool fullScreen)
		{
			if (mFrameBuffer != null)
				mFrameBuffer.Dispose();

			// Created swap chain will be owned by the FrameBufferWindow object, and
			// disposal will be handled by that object.
			SwapChain swap = mDisplay.CreateSwapChain(this, mChooseWidth, mChooseHeight,
				mChooseBitDepth, fullScreen);
			
			Direct3D.Surface backBuffer = swap.GetBackBuffer(0);

			Direct3D.Surface backDepthStencil = Direct3D.Surface.CreateDepthStencil(
				mDisplay.D3D_Device.Device,
				mChooseWidth, mChooseHeight, 
				mDisplay.DepthStencilFormat,
				MultisampleType.None, 0, true);

			mFrameBuffer = new FrameBufferWindow(Size, swap, mOwner, backBuffer, backDepthStencil);

			mRenderTarget.Resize += new EventHandler(mRenderTarget_Resize);
		}

		void mRenderTarget_Resize(object sender, EventArgs e)
		{
			mFrameBuffer.SetSize(Size);
		}


		public System.Windows.Forms.Control RenderTarget
		{
			get { return mRenderTarget; }
		}
		public override bool IsClosed
		{
			get { return mIsClosed; }
		}
		public override Size Size
		{
			get
			{
				return Interop.Convert(mRenderTarget.ClientSize);
			}
			set
			{
				if (frm != null)
					frm.ClientSize = Interop.Convert(value);
			}
		}
		public override Point MousePosition
		{
			get
			{
				return Interop.Convert(mRenderTarget.PointToClient(
					System.Windows.Forms.Cursor.Position));
			}
			set
			{
				System.Windows.Forms.Cursor.Position = mRenderTarget.PointToScreen(
					Interop.Convert(value));
			}
		}
		internal static Mouse.MouseButtons GetButtons(System.Windows.Forms.MouseButtons buttons)
		{
			Mouse.MouseButtons retval = Mouse.MouseButtons.None;

			if ((buttons & System.Windows.Forms.MouseButtons.Left) != 0)
				retval |= Mouse.MouseButtons.Primary;
			if ((buttons & System.Windows.Forms.MouseButtons.Right) != 0)
				retval |= Mouse.MouseButtons.Secondary;
			if ((buttons & System.Windows.Forms.MouseButtons.Middle) != 0)
				retval |= Mouse.MouseButtons.Middle;
			if ((buttons & System.Windows.Forms.MouseButtons.XButton1) != 0)
				retval |= Mouse.MouseButtons.ExtraButton1;
			if ((buttons & System.Windows.Forms.MouseButtons.XButton2) != 0)
				retval |= Mouse.MouseButtons.ExtraButton2;

			return retval;
		}


		public override bool IsFullScreen
		{
			get { return mIsFullscreen; }
		}

		public override void SetFullScreen()
		{
			SetFullScreen(mChooseWidth, mChooseHeight, mChooseBitDepth);
		}
		public override void SetFullScreen(int width, int height, int bpp)
		{
			if (frm == null)
				throw new InvalidOperationException("This DisplayWindow was created on a " +
					"System.Windows.Forms.Control object, and cannot be set to full screen.");

			
			//return;

			ScreenMode mode = ScreenMode.SelectBestMode(width, height, bpp);

			if (mode == null)
				return;

			mChooseWidth = mode.Width;
			mChooseHeight = mode.Height;
			mChooseBitDepth = mode.Bpp;

			mIsFullscreen = true;

			CreateFullScreenDisplay();
			Keyboard.ReleaseAllKeys();

		}

		public override void SetWindowed()
		{
			CreateWindowedDisplay();

			mIsFullscreen = false;
		}

		public override string Title
		{
			get
			{
				if (frm != null)
					return frm.Text;
				else
					return "";
			}
			set
			{
				if (frm != null)
					frm.Text = value;
			}
		}

	}
}
