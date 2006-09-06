//     ``The contents of this file are subject to the Mozilla Public License
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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Direct3D = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib.MDX
{

    public class MDX1_DisplayWindow : DisplayWindowImpl, MDX1_IRenderTarget
    {
        Form frm;
        Control mRenderTarget;
        bool mClosed = false;
        bool mIsFullscreen = false;

        internal SwapChain mSwap;
        internal Direct3D.Surface mBackBuffer;

        int mChooseWidth;
        int mChooseHeight;
        int mChooseBitDepth;
        bool mChooseFullscreen = false;
        bool mChooseResize = false;
        string mTitle = "";

        MDX1_Display mDisplay;

        #region --- Creation / Destruction ---

        public MDX1_DisplayWindow(string title, int clientWidth, int clientHeight, 
            bool startFullscreen, bool allowResize)
        {
            mTitle = title;
            mChooseFullscreen = startFullscreen;
            mChooseWidth = clientWidth;
            mChooseHeight = clientHeight;
            mChooseResize = allowResize;

            CreateWindowedDisplay();

            mDisplay = Display.Impl as MDX1_Display;
            mDisplay.Initialize(this);

            // and create the back buffer
            OnResize();
        }

        public MDX1_DisplayWindow(System.Windows.Forms.Control renderTarget)
        {
            mRenderTarget = renderTarget;

            mChooseFullscreen = false;
            mChooseWidth = renderTarget.ClientSize.Width;
            mChooseHeight = renderTarget.ClientSize.Height;
            
            mDisplay = Display.Impl as MDX1_Display;
            mDisplay.Initialize(this);

            AttachEvents();
            OnResize();
        }

        public override void Dispose()
        {
            if (frm != null)
            {
                frm.Dispose();
                frm = null;
            }

            mClosed = true;
        }

        #endregion

        #region --- Event handlers ---

        private void AttachEvents()
        {

            mRenderTarget.Resize += new EventHandler(frm_Resize);

            mRenderTarget.MouseMove += new System.Windows.Forms.MouseEventHandler(pct_MouseMove);
            mRenderTarget.MouseDown += new System.Windows.Forms.MouseEventHandler(pct_MouseDown);
            mRenderTarget.MouseUp += new System.Windows.Forms.MouseEventHandler(pct_MouseUp);
            mRenderTarget.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(pct_MouseDoubleClick);

            System.Windows.Forms.Form form = (mRenderTarget.TopLevelControl as System.Windows.Forms.Form);

            form.KeyPreview = true;
            form.KeyDown += new System.Windows.Forms.KeyEventHandler(form_KeyDown);
            form.KeyUp += new System.Windows.Forms.KeyEventHandler(form_KeyUp);

            form.FormClosing += new System.Windows.Forms.FormClosingEventHandler(form_FormClosing);
            form.FormClosed += new System.Windows.Forms.FormClosedEventHandler(form_FormClosed);

        }

        void form_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            mClosed = true;
        }
        void form_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {

        }
        void form_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Keyboard.Keys.SetWinFormsKey(e.KeyCode, false);
        }
        void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Keyboard.Keys.SetWinFormsKey(e.KeyCode, true);
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
            Mouse.OnMouseMove();
        }
        void renderTarget_Disposed(object sender, EventArgs e)
        {
            mClosed = true;
        }


        void frm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            mClosed = true;
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

        bool creatingSwapChain = false;

        private void OnResize()
        {
            if (creatingSwapChain)
                return;

            // create swap chain
            creatingSwapChain = true;

            mDisplay.CreateSwapChain(this, mChooseWidth, mChooseHeight, mChooseBitDepth, mChooseFullscreen);

            creatingSwapChain = false;
        }

        #endregion

        internal void CreateWindowedDisplay()
        {
            Form myform;
            Control myRenderTarget;

            InitializeWindowsForm(out myform, out myRenderTarget, mTitle, mChooseWidth, mChooseHeight, mChooseFullscreen, mChooseResize);

            ReplaceForm(myform, myRenderTarget);
        }

        public System.Windows.Forms.Control RenderTarget
        {
            get { return mRenderTarget; }
        }
        public override bool Closed
        {
            get { return mClosed; }
        }
        public override Size Size
        {
            get
            {
                return new Size(mRenderTarget.ClientSize);
            }
            set
            {
                if (frm != null)
                    frm.ClientSize = (System.Drawing.Size)value;
            }
        }
        public override Point MousePosition
        {
            get
            {
                return new Point(mRenderTarget.PointToClient((System.Drawing.Point)System.Windows.Forms.Cursor.Position));
            }
            set
            {
                System.Windows.Forms.Cursor.Position = mRenderTarget.PointToScreen((System.Drawing.Point)value);
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

        public override void ToggleFullScreen()
        {
            Keyboard.ReleaseAllKeys(true);

            bool wasFullscreen = IsFullScreen;
            Size size = new Size( frm.ClientSize);

            mChooseFullscreen = !mChooseFullscreen;

            OnResize();


            mIsFullscreen = !mSwap.PresentParameters.Windowed;


        }
        public override void ToggleFullScreen(int width, int height, int bpp)
        {
            mChooseWidth = width;
            mChooseHeight = height;
            mChooseBitDepth = bpp;

            ToggleFullScreen();
        }

        #region --- MDX1_IRenderTarget Members ---

        public override void BeginRender()
        {
            mDisplay.D3D_Device.Device.SetRenderTarget(0, mBackBuffer);
            mDisplay.D3D_Device.Device.BeginScene();
        }
        public override void EndRender(bool waitVSync)
        {
            mDisplay.D3D_Device.Device.EndScene();

            try
            {
                //mSwap.Present(Present.DoNotWait);

                if (waitVSync == false)
                {
                    mSwap.PresentParameters.PresentationInterval = PresentInterval.Immediate;
                    mSwap.Present(Present.DoNotWait);
                }
                else
                {
                    mSwap.PresentParameters.PresentationInterval = PresentInterval.Default;
                    mSwap.Present();
                }

            }
            catch
            {
            }
        }

        public Microsoft.DirectX.Direct3D.Surface RenderSurface
        {
            get { return mBackBuffer; }
        }

        #endregion


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

        internal void ReplaceForm(Form newform, Control renderTarget)
        {
            if (frm != null)
                frm.Dispose();

            frm = newform;
            mRenderTarget = renderTarget;

            // show the form and attach events.
            frm.Show();
            AttachEvents();
        }
    }
}
