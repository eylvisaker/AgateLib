using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Direct3D = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace ERY.GameLibrary
{

    public class MDX2_DisplayWindow : DisplayWindowImpl
    {
        System.Windows.Forms.Form frm;
        System.Windows.Forms.Control mRenderTarget;
        bool mClosed = false;
        bool mChooseFullscreen = false;

        internal SwapChain mSwap;
        internal Direct3D.Surface mBackBuffer;

        MDX2_Display mDisplay;

        public MDX2_DisplayWindow(string title, int clientWidth, int clientHeight, 
            bool startFullscreen, bool allowResize)
        {
            InitializeWindowsForm(out frm, out mRenderTarget, title, clientWidth, clientHeight, startFullscreen, allowResize);

            mChooseFullscreen = startFullscreen;


            // show the form
            frm.Show();

            mDisplay = Display.Impl as MDX2_Display;
            mDisplay.Initialize(this);

            // attach events
            AttachEvents();

            // and create the back buffer
            OnResize();
        }
        public MDX2_DisplayWindow(System.Windows.Forms.Control renderTarget)
        {
            mRenderTarget = renderTarget;

            mDisplay = Display.Impl as MDX2_Display;
            mDisplay.Initialize(this);

            AttachEvents();
            OnResize();
        }

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
            Keyboard.Keys[e.KeyCode] = false;
        }
        void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Keyboard.Keys[e.KeyCode] = true;
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

        public override System.Windows.Forms.Control RenderTarget
        {
            get { return mRenderTarget; }
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
            OnResize();
        }

        private void OnResize()
        {
            // create swap chain
            //mDisplay.CreateSwapChain(this, mChooseFullscreen);
            mDisplay.CreateSwapChain(this, false);
        }


        public override bool Closed
        {
            get { return mClosed; }
        }

        public override Size Size
        {
            get
            {
                return mRenderTarget.ClientSize;
            }
            set
            {
                if (frm != null)
                    frm.ClientSize = value;
            }
        }

        public override Point MousePosition
        {
            get
            {
                return frm.PointToClient(System.Windows.Forms.Cursor.Position);
            }
            set
            {
                System.Windows.Forms.Cursor.Position = frm.PointToScreen(value);
            }
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


        public override bool IsFullScreen
        {
            get { return !mSwap.PresentParameters.IsWindowed; }
        }

        public override void ToggleFullScreen()
        {
            mChooseFullscreen = !mChooseFullscreen;

            OnResize();
        }
    }
}
