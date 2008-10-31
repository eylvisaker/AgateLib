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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using Drawing = System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.ImplBase;
using AgateLib.Input;
using AgateLib.WinForms;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace AgateLib.Display.OpenGL
{
    public sealed class GL_DisplayControl : DisplayWindowImpl, GL_IRenderTarget
    {
        Form frm;
        Control mRenderTarget;
        GraphicsContext mContext;
        IWindowInfo mWindowInfo;

        GL_Display mDisplay;
        Drawing.Icon mIcon;
        bool mIsClosed = false;
        bool mIsFullScreen = false;

        string mTitle;
        bool mChooseFullscreen;
        int mChooseWidth;
        int mChooseHeight;
        int mChooseBitDepth = 32;
        bool mChooseResize;
        WindowPosition mChoosePosition;

        bool mHasFrame = true;

        public GL_DisplayControl(CreateWindowParams windowParams)
        {
            mChoosePosition = windowParams.WindowPosition;
            
            if (windowParams.RenderToControl)
            {
                if (typeof(Control).IsAssignableFrom(windowParams.RenderTarget.GetType()) == false)
                    throw new ArgumentException("The specified render target does not derive from System.Windows.Forms.Control");

                mRenderTarget = (Control)windowParams.RenderTarget;

                if (mRenderTarget.TopLevelControl == null)
                    throw new ArgumentException("The specified render target does not have a Form object yet.  " +
                        "It's TopLevelControl property is null.  You may not create DisplayWindow objects before " +
                        "the control to render to is added to the Form.");

                mChooseFullscreen = false;
                mChooseWidth = mRenderTarget.ClientSize.Width;
                mChooseHeight = mRenderTarget.ClientSize.Height;

                mDisplay = Display.Impl as GL_Display;

                mWindowInfo = OpenTK.Utilities.Interop.CreateWindowInfo(mRenderTarget);
                mContext = new GraphicsContext(
                    new GraphicsMode(new ColorFormat(32)), mWindowInfo);

                mContext.MakeCurrent(mWindowInfo);
                ((IGraphicsContextInternal)mContext).LoadAll();

                mDisplay.InitializeGL();

                AttachEvents();
            }
            else
            {
                if (string.IsNullOrEmpty(windowParams.IconFile) == false)
                    mIcon = new Drawing.Icon(windowParams.IconFile);

                mTitle = windowParams.Title;
                mChooseFullscreen = windowParams.IsFullScreen;
                mChooseWidth = windowParams.Width;
                mChooseHeight = windowParams.Height;
                mChooseResize = windowParams.IsResizable;
                mHasFrame = windowParams.HasFrame;

                if (mChooseFullscreen)
                    CreateFullScreenDisplay();
                else
                    CreateWindowedDisplay();

                mDisplay = Display.Impl as GL_Display;
                mDisplay.InitializeGL();

            }

            mDisplay.ProcessEventsEvent += new EventHandler(mDisplay_ProcessEventsEvent);
        }

        void mDisplay_ProcessEventsEvent(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.DoEvents();
        }

        private void CreateFullScreenDisplay()
        {
            DetachEvents();

            Form oldForm = frm;
            GraphicsContext oldcontext = mContext;
            IWindowInfo oldWindowInfo = mWindowInfo;

            mContext = null;
            
            frm = new frmFullScreen();
            frm.Show();

            frm.Text = mTitle;
            frm.Icon = mIcon;
            frm.TopLevel = true;

            mRenderTarget = frm;

            AttachEvents();

            mWindowInfo = OpenTK.Utilities.Interop.CreateWindowInfo(mRenderTarget);
            mContext = new GraphicsContext(
                new GraphicsMode(new ColorFormat(32)), mWindowInfo);
            mContext.MakeCurrent(mWindowInfo);
            ((IGraphicsContextInternal)mContext).LoadAll();

            DisplayResolution resolution = DisplayDevice.Default.SelectResolution(
                mChooseWidth, mChooseHeight, 32, 0);

            DisplayDevice.Default.ChangeResolution(resolution);

            frm.Location = System.Drawing.Point.Empty;
            frm.ClientSize = new System.Drawing.Size(mChooseWidth, mChooseHeight);
            frm.Activate();

            System.Threading.Thread.Sleep(1000);
            mIsFullScreen = true;

            if (oldWindowInfo != null) oldWindowInfo.Dispose();
            if (oldcontext != null)                oldcontext.Dispose();
            if (oldForm != null)                oldForm.Dispose();

            Core.IsActive = true;
        }

        private void CreateWindowedDisplay()
        {
            DetachEvents();

            Form oldForm = frm;
            GraphicsContext oldcontext = mContext;
            IWindowInfo oldWindowInfo = mWindowInfo;

            mContext = null;
            mIsFullScreen = false;

            Form myform;
            Control myRenderTarget;

            DisplayDevice.Default.RestoreResolution();

            WinForms.FormUtil.InitializeWindowsForm(out myform, out myRenderTarget, mChoosePosition,
                mTitle, mChooseWidth, mChooseHeight, mChooseFullscreen, mChooseResize, mHasFrame);

            frm = myform;
            mRenderTarget = myRenderTarget;

            if (mIcon != null)
                frm.Icon = mIcon;

            frm.Show();
            AttachEvents();

            mWindowInfo = OpenTK.Utilities.Interop.CreateWindowInfo(mRenderTarget);
            mContext = new GraphicsContext(
                new GraphicsMode(new ColorFormat(32)), mWindowInfo);
            mContext.MakeCurrent(mWindowInfo);
            ((IGraphicsContextInternal)mContext).LoadAll();

            if (oldWindowInfo != null) oldWindowInfo.Dispose();
            if (oldcontext != null) oldcontext.Dispose();
            if (oldForm != null) oldForm.Dispose();

            Core.IsActive = true;
        }


        public override void Dispose()
        {
            mDisplay.ProcessEventsEvent -= mDisplay_ProcessEventsEvent;

            if (mContext != null)
            {
                mContext.Dispose();
                mContext = null;
            }

            if (frm != null)
            {
                frm.Close();
                frm = null;
            }

        }

        public void MakeCurrent()
        {
            mContext.MakeCurrent(mWindowInfo);

            GL.Viewport(0, 0, Width, Height);

            mDisplay.SetupGLOrtho(Rectangle.FromLTRB(0, Height, Width, 0));

        }

        private void AttachEvents()
        {
            if (mRenderTarget == null)
                return;

            mRenderTarget.Resize += new EventHandler(mRenderTarget_Resize);
            mRenderTarget.Disposed += new EventHandler(mRenderTarget_Disposed);

            mRenderTarget.MouseMove += new System.Windows.Forms.MouseEventHandler(pct_MouseMove);
            mRenderTarget.MouseDown += new System.Windows.Forms.MouseEventHandler(pct_MouseDown);
            mRenderTarget.MouseUp += new System.Windows.Forms.MouseEventHandler(pct_MouseUp);
            mRenderTarget.DoubleClick += new EventHandler(mRenderTarget_DoubleClick);
            System.Windows.Forms.Form form = (mRenderTarget.TopLevelControl as System.Windows.Forms.Form);

            form.KeyPreview = true;
            form.KeyDown += new System.Windows.Forms.KeyEventHandler(form_KeyDown);
            form.KeyUp += new System.Windows.Forms.KeyEventHandler(form_KeyUp);

        }
        private void DetachEvents()
        {
            if (mRenderTarget == null)
                return;

            mRenderTarget.Resize -= new EventHandler(mRenderTarget_Resize);
            mRenderTarget.Disposed -= new EventHandler(mRenderTarget_Disposed);

            mRenderTarget.MouseMove -= new System.Windows.Forms.MouseEventHandler(pct_MouseMove);
            mRenderTarget.MouseDown -= new System.Windows.Forms.MouseEventHandler(pct_MouseDown);
            mRenderTarget.MouseUp -= new System.Windows.Forms.MouseEventHandler(pct_MouseUp);
            mRenderTarget.DoubleClick -= new EventHandler(mRenderTarget_DoubleClick);
            System.Windows.Forms.Form form = (mRenderTarget.TopLevelControl as System.Windows.Forms.Form);

            form.KeyDown -= new System.Windows.Forms.KeyEventHandler(form_KeyDown);
            form.KeyUp -= new System.Windows.Forms.KeyEventHandler(form_KeyUp);

        }

        Mouse.MouseButtons GetButtons(System.Windows.Forms.MouseButtons buttons)
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
        void mRenderTarget_Disposed(object sender, EventArgs e)
        {
            mIsClosed = true;
        }

        void mRenderTarget_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, mRenderTarget.Width, mRenderTarget.Height);
           
        }


        void mRenderTarget_DoubleClick(object sender, EventArgs e)
        {
            Mouse.OnMouseDoubleClick(Mouse.MouseButtons.Primary);
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
            Mouse.OnMouseMove();
        }
        void renderTarget_Disposed(object sender, EventArgs e)
        {
            mIsClosed = true;
        }


        void form_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            mIsClosed = true;
        }
        void form_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Keyboard.Keys[FormUtil.TransformWinFormsKey(e.KeyCode)] = false;
        }
        void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Keyboard.Keys[FormUtil.TransformWinFormsKey(e.KeyCode)] = true;
        }
        public override bool IsClosed
        {
            get { return mIsClosed; }
        }

        public override bool IsFullScreen
        {
            get
            {
                return mIsFullScreen;
            }
        }


        public override Size Size
        {
            get
            {
                return WinForms.Interop.Convert(mRenderTarget.ClientSize);
            }
            set
            {
                mRenderTarget.ClientSize = WinForms.Interop.Convert(value);

                if (frm != null)
                    frm.ClientSize = WinForms.Interop.Convert(value);
            }
        }

        public override string Title
        {
            get
            {
                if (frm != null)
                    return frm.Text;
                else
                    return null;
            }
            set
            {
                if (frm != null)
                    frm.Text = value;
            }
        }

        public override Point MousePosition
        {
            get
            {
                return WinForms.Interop.Convert(mRenderTarget.PointToClient(Cursor.Position));
            }
            set
            {
                Cursor.Position = mRenderTarget.PointToScreen(WinForms.Interop.Convert(value));
            }
        }

        public override void BeginRender()
        {
            mContext.MakeCurrent(mWindowInfo);

            mDisplay.SetClipRect(new Rectangle(0, 0, Width, Height));
            
        }

        public override void EndRender()
        {
            if (mContext.VSync != mDisplay.VSync)
                mContext.VSync = mDisplay.VSync;

            mContext.SwapBuffers();
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

            ScreenMode mode = ScreenMode.SelectBestMode(width, height, bpp);

            mChooseWidth = width;
            mChooseHeight = height;
            mChooseBitDepth = bpp;

            CreateFullScreenDisplay();
            Keyboard.ReleaseAllKeys();
        }
        public override void SetWindowed()
        {
            CreateWindowedDisplay();
            Keyboard.ReleaseAllKeys();
        }

    }
}
