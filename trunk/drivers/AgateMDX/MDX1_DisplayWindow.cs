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
using Drawing = System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Direct3D = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

using ERY.AgateLib.Geometry;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib.MDX
{
    public class MDX1_DisplayWindow : DisplayWindowImpl, MDX1_IRenderTarget
    {
        Form frm;
        Control mRenderTarget;
        bool mIsClosed = false;
        bool mIsFullscreen = false;

        internal SwapChain mSwap;
        internal Direct3D.Surface mBackBuffer;

        int mChooseWidth;
        int mChooseHeight;
        int mChooseBitDepth = 32;
        System.Drawing.Icon mIcon;
        bool mChooseFullscreen = false;
        bool mChooseResize = false;
        string mTitle = "";

        MDX1_Display mDisplay;

        #region --- Creation / Destruction ---

        public MDX1_DisplayWindow(string title, int clientWidth, int clientHeight, 
            string iconFile, bool startFullscreen, bool allowResize)
        {
            if (iconFile != null)
                mIcon = new Drawing.Icon(iconFile);

            mTitle = title;
            mChooseFullscreen = startFullscreen;
            mChooseWidth = clientWidth;
            mChooseHeight = clientHeight;
            mChooseResize = allowResize;

            CreateWindow();

            mDisplay = Display.Impl as MDX1_Display;
            mDisplay.Initialize(this);
            mDisplay.VSyncChanged += new EventHandler(mDisplay_VSyncChanged);
            
            // and create the back buffer
            CreateBackBuffer();

        }

        public MDX1_DisplayWindow(System.Windows.Forms.Control renderTarget)
        {
            mRenderTarget = renderTarget;

            mChooseFullscreen = false;
            mChooseWidth = renderTarget.ClientSize.Width;
            mChooseHeight = renderTarget.ClientSize.Height;
            
            mDisplay = Display.Impl as MDX1_Display;
            mDisplay.Initialize(this);
            mDisplay.VSyncChanged += new EventHandler(mDisplay_VSyncChanged);

            AttachEvents();

            CreateBackBuffer();

        }

        public override void Dispose()
        {
            if (frm != null)
            {
                frm.Dispose();
                frm = null;
            }

            mIsClosed = true;
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

            System.Windows.Forms.Form form;

            if (mRenderTarget is System.Windows.Forms.Form)
                form = mRenderTarget as System.Windows.Forms.Form;
            else
                form = (mRenderTarget.TopLevelControl as System.Windows.Forms.Form);

            form.KeyPreview = true;
            form.KeyDown += new System.Windows.Forms.KeyEventHandler(form_KeyDown);
            form.KeyUp += new System.Windows.Forms.KeyEventHandler(form_KeyUp);

            form.FormClosing += new System.Windows.Forms.FormClosingEventHandler(form_FormClosing);
            form.FormClosed += new System.Windows.Forms.FormClosedEventHandler(form_FormClosed);

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
            CreateBackBuffer();
        }


        void form_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            mIsClosed = true;
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

            CreateBackBuffer();

        }

        #endregion

        private void CreateWindow()
        {
            InitializeWindowsForm(out frm, out mRenderTarget, mTitle,
                mChooseWidth, mChooseHeight, mChooseFullscreen, mChooseResize);

            frm.Icon = mIcon;

            frm.Show();
            AttachEvents();
        }

        private void CreateWindowedDisplay()
        {
            DetachEvents();

            Form oldForm = frm;

            CreateWindow();
            CreateBackBuffer();

            if (oldForm != null)
                oldForm.Dispose();

            Core.IsActive = true;
        }

        private void CreateFullScreenDisplay()
        {
            DetachEvents();

            Form oldForm = frm;

            frm = new frmFullScreen();
            frm.Show();

            frm.Text = mTitle;
            frm.Icon = mIcon;
            frm.TopLevel = true;
            frm.Activate();

            mRenderTarget = frm;

            AttachEvents();
            try
            {
                CreateBackBuffer();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("{0}", e.Message);
                System.Diagnostics.Debug.WriteLine("{0}", e.StackTrace);
                throw;
            }
            System.Diagnostics.Debug.WriteLine("Created Full Screen buffer.");

            frm.Location = System.Drawing.Point.Empty;
            System.Diagnostics.Debug.WriteLine("Moved Form.");
            
            frm.ClientSize = new System.Drawing.Size(mChooseWidth, mChooseHeight);
            System.Diagnostics.Debug.WriteLine("Resized Form.");
            
            frm.Activate();
            System.Diagnostics.Debug.WriteLine("Activated Form.");

            System.Threading.Thread.Sleep(1000);
            System.Diagnostics.Debug.WriteLine("Sleeping.");

            if (oldForm != null)
                oldForm.Dispose();
            System.Diagnostics.Debug.WriteLine("Disposed of old form.");

            Core.IsActive = true;
        }

        private void CreateBackBuffer()
        {
            SwapChain oldChain = mSwap;
            if (mBackBuffer != null)
                mBackBuffer.Dispose();

            mSwap = mDisplay.CreateSwapChain(this, mChooseWidth, mChooseHeight,
                mChooseBitDepth, mChooseFullscreen);

            mBackBuffer = mSwap.GetBackBuffer(0, BackBufferType.Mono);

            if (oldChain != null)
                oldChain.Dispose(); 
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
        /*
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
        */

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

            if (mode == null)
                return;

            mChooseWidth = mode.Width;
            mChooseHeight = mode.Height;
            mChooseBitDepth = mode.Bpp;

            mChooseFullscreen = true;

            CreateFullScreenDisplay();
        }

        public override void SetWindowed()
        {
            mChooseFullscreen = false;

            CreateWindowedDisplay();
        }

        #region --- MDX1_IRenderTarget Members ---

        public override void BeginRender()
        {
            mDisplay.D3D_Device.Device.SetRenderTarget(0, mBackBuffer);
            mDisplay.D3D_Device.Device.BeginScene();
        }
        public override void EndRender()
        {
            mDisplay.D3D_Device.Device.EndScene();

            //try
            //{
                //mSwap.Present(Present.DoNotWait);

                if (mDisplay.VSync == false)
                {
                    mSwap.PresentParameters.PresentationInterval = PresentInterval.Immediate;
                    mSwap.Present();
                }
                else
                {
                    mSwap.PresentParameters.PresentationInterval = PresentInterval.One;
                    mSwap.Present();
                }

            //}
            ////catch (Exception e)
            //{
            //    System.Diagnostics.Debug.WriteLine(
            //        "Present gave exception: " + e.ToString() + "\r\n" + e.Message);
            //}
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

    }
}
