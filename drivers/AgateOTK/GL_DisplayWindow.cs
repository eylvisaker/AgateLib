using System;
using System.Collections.Generic;
using Drawing = System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using ERY.AgateLib;
using ERY.AgateLib.Drivers;
using ERY.AgateLib.Geometry;
using ERY.AgateLib.ImplBase;

using OpenTK.OpenGL;
using Gl = OpenTK.OpenGL.GL;

namespace ERY.AgateLib.OpenGL
{
    public sealed class GL_DisplayWindow : DisplayWindowImpl, GL_IRenderTarget
    {
        Form frm;
        Control mRenderTarget;
        GLContext mContext;
        GL_Display mDisplay;
        Drawing.Icon mIcon;
        bool mIsClosed = false;

        string mTitle;
        bool mChooseFullscreen;
        int mChooseWidth;
        int mChooseHeight;
        int mChooseBitDepth = 32;
        bool mChooseResize;
        WindowPosition mChoosePosition;

        bool mHasFrame = true;

        public GL_DisplayWindow(CreateWindowParams windowParams)
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
                mDisplay.Initialize(this);

                mContext = GLContext.Create(mRenderTarget, new OpenTK.OpenGL.ColorDepth(8, 8, 8, 8), 16, 0);

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
                mDisplay.Initialize(this);

            }
        }

        private void CreateFullScreenDisplay()
        {
            DetachEvents();

            Form oldForm = frm;
            GLContext oldcontext = mContext;
            
            mContext = null;
            
            frm = new frmFullScreen();
            frm.Show();

            frm.Text = mTitle;
            frm.Icon = mIcon;
            frm.TopLevel = true;

            mRenderTarget = frm;

            AttachEvents();

            mContext = GLContext.Create(frm, new OpenTK.OpenGL.ColorDepth(8, 8, 8, 8), 16, 0);
            mContext.SetFullScreen(mChooseWidth, mChooseHeight, new OpenTK.OpenGL.ColorDepth(8, 8, 8, 8));

            frm.Location = System.Drawing.Point.Empty;
            frm.ClientSize = new System.Drawing.Size(mChooseWidth, mChooseHeight);
            frm.Activate();

            System.Threading.Thread.Sleep(1000);

            if (oldcontext != null)
                oldcontext.Dispose();
            if (oldForm != null)
                oldForm.Dispose();

            Core.IsActive = true;
        }

        private void CreateWindowedDisplay()
        {
            DetachEvents();

            Form oldForm = frm;
            GLContext oldcontext = mContext;
            mContext = null;

            Form myform;
            Control myRenderTarget;

            InitializeWindowsForm(out myform, out myRenderTarget, mChoosePosition,
                mTitle, mChooseWidth, mChooseHeight, mChooseFullscreen, mChooseResize, mHasFrame);


            frm = myform;
            mRenderTarget = myRenderTarget;

            frm.Icon = mIcon;

            frm.Show();
            AttachEvents();

            mContext = GLContext.Create(mRenderTarget, 
                new OpenTK.OpenGL.ColorDepth(8, 8, 8, 8), 16, 0);

            if (oldcontext != null)
                oldcontext.Dispose();
            if (oldForm != null)
                oldForm.Dispose();

            Core.IsActive = true;
        }


        public override void Dispose()
        {
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
            mContext.MakeCurrent();

            Gl.Viewport(0, 0, Width, Height);

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
            Gl.Viewport(0, 0, mRenderTarget.Width, mRenderTarget.Height);
           
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
            Keyboard.Keys.SetWinFormsKey(e.KeyCode, false);
        }
        void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Keyboard.Keys.SetWinFormsKey(e.KeyCode, true);
        }
        public override bool IsClosed
        {
            get { return mIsClosed; }
        }

        public override bool IsFullScreen
        {
            get { return mContext.IsFullscreen;}
        }


        public override Size Size
        {
            get
            {
                return new Size(mRenderTarget.ClientSize);
            }
            set
            {
                mRenderTarget.ClientSize = (Drawing.Size)value;

                if (frm != null)
                    frm.ClientSize = (Drawing.Size)value;
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
                return new Point(mRenderTarget.PointToClient(Cursor.Position));
            }
            set
            {
                Cursor.Position = mRenderTarget.PointToScreen((Drawing.Point)value);
            }
        }

        public override void BeginRender()
        {
            mContext.MakeCurrent();

            mDisplay.SetClipRect(new Rectangle(0, 0, Width, Height));
            
        }

        public override void EndRender()
        {
            mContext.EnableVSync = mDisplay.VSync;
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
