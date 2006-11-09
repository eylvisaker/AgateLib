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
    public class GL_DisplayWindow : DisplayWindowImpl, GL_IRenderTarget
    {
        Form frm;
        Control mRenderTarget;
        GLContext mContext;
        GL_Display mDisplay;
        Drawing.Icon mIcon;
        bool mClosed = false;

        string mTitle;
        bool mChooseFullscreen;
        int mChooseWidth;
        int mChooseHeight;
        bool mChooseResize;

        public GL_DisplayWindow(string title, int clientWidth, int clientHeight,
            string iconFile, bool startFullscreen, bool allowResize)
        {
            if (iconFile != null)
                mIcon = new Drawing.Icon(iconFile);

            mTitle = title;
            mChooseFullscreen = startFullscreen;
            mChooseWidth = clientWidth;
            mChooseHeight = clientHeight;
            mChooseResize = allowResize;

            CreateWindowedDisplay();

            mDisplay = Display.Impl as GL_Display;
            mDisplay.Initialize(this);

            // and create the back buffer
            //OnResize();
        }


        public GL_DisplayWindow(System.Windows.Forms.Control renderTarget)
        {
            mRenderTarget = renderTarget;

            mChooseFullscreen = false;
            mChooseWidth = renderTarget.ClientSize.Width;
            mChooseHeight = renderTarget.ClientSize.Height;

            mDisplay = Display.Impl as GL_Display;
            mDisplay.Initialize(this);

            mContext = GLContext.Create(mRenderTarget, new OpenTK.OpenGL.ColorDepth(8, 8, 8, 8), 16, 0);

            AttachEvents();
            //OnResize();
        }


        private void CreateWindowedDisplay()
        {
            Form myform;
            Control myRenderTarget;

            InitializeWindowsForm(out myform, out myRenderTarget, 
                mTitle, mChooseWidth, mChooseHeight, mChooseFullscreen, mChooseResize);


            frm = myform;
            mRenderTarget = myRenderTarget;

            frm.Icon = mIcon;

            frm.Show();
            AttachEvents();

            mContext = GLContext.Create(mRenderTarget, new OpenTK.OpenGL.ColorDepth(8, 8, 8, 8), 16, 0);
        }


        public override void Dispose()
        {
            if (frm != null)
            {
                frm.Close();
                frm = null;
            }

            if (mContext != null)
            {
                mContext.Dispose();
                mContext = null;
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
            mRenderTarget.Resize += new EventHandler(mRenderTarget_Resize);
            mRenderTarget.Disposed += new EventHandler(mRenderTarget_Disposed);

            mRenderTarget.MouseMove += new System.Windows.Forms.MouseEventHandler(pct_MouseMove);
            mRenderTarget.MouseDown += new System.Windows.Forms.MouseEventHandler(pct_MouseDown);
            mRenderTarget.MouseUp += new System.Windows.Forms.MouseEventHandler(pct_MouseUp);
            //mRenderTarget.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(pct_MouseDoubleClick);
            mRenderTarget.DoubleClick += new EventHandler(mRenderTarget_DoubleClick);
            System.Windows.Forms.Form form = (mRenderTarget.TopLevelControl as System.Windows.Forms.Form);

            form.KeyPreview = true;
            form.KeyDown += new System.Windows.Forms.KeyEventHandler(form_KeyDown);
            form.KeyUp += new System.Windows.Forms.KeyEventHandler(form_KeyUp);

            //form.FormClosed += new System.Windows.Forms.FormClosedEventHandler(form_FormClosed);
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
            mClosed = true;
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
            mClosed = true;
        }


        void form_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            mClosed = true;
        }
        void form_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Keyboard.Keys.SetWinFormsKey(e.KeyCode, false);
        }
        void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Keyboard.Keys.SetWinFormsKey(e.KeyCode, true);
        }
        public override bool Closed
        {
            get { return mClosed; }
        }

        public override bool IsFullScreen
        {
            get { return mContext.IsFullscreen;}
        }

        public override void ToggleFullScreen()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void ToggleFullScreen(int width, int height, int bpp)
        {
            throw new Exception("The method or operation is not implemented.");
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

            mDisplay.SetupGLOrtho(new Rectangle(0, 0, Width, Height));
        }

        public override void EndRender(bool waitVSync)
        {
            mContext.SwapBuffers();
        }


        /*
        Form frm;
        Control mRenderTarget;

        bool mClosed = false;
        bool mIsFullscreen = false;

        internal IntPtr hDC;
        internal IntPtr hRC;

        public GL_DisplayWindow(string title, int clientWidth, int clientHeight, 
            bool startFullscreen, bool allowResize)
        {
            mIsFullscreen = startFullscreen;

            InitializeWindowsForm(out frm, out mRenderTarget, title, clientWidth, clientHeight, startFullscreen, allowResize);

            // show the form
            frm.Show();

            mDisplay = Display.Impl as WGL_Display;
            InitializeGL();

            // attach events
            AttachEvents();

            // and create the back buffer
            OnResize();
        }

        public GL_DisplayWindow(System.Windows.Forms.Control renderTarget)
        {
            mRenderTarget = renderTarget;

            mDisplay = Display.Impl as WGL_Display;
            InitializeGL();

            AttachEvents();
            OnResize();
        }


        internal void InitializeGL()
        {
            bool fullscreen = false;
            int bitdepth = 16;
            int pixelFormat;

            Gdi.PIXELFORMATDESCRIPTOR pfd = new Gdi.PIXELFORMATDESCRIPTOR();    // pfd Tells Windows How We Want Things To Be
            pfd.nSize = (short)Marshal.SizeOf(pfd);                             // Size Of This Pixel Format Descriptor
            pfd.nVersion = 1;                                                   // Version Number
            pfd.dwFlags = Gdi.PFD_DRAW_TO_WINDOW |                              // Format Must Support Window
                Gdi.PFD_SUPPORT_OPENGL |                                        // Format Must Support OpenGL
                Gdi.PFD_DOUBLEBUFFER;                                           // Format Must Support Double Buffering
            pfd.iPixelType = (byte)Gdi.PFD_TYPE_RGBA;                           // Request An RGBA Format
            pfd.cColorBits = (byte)bitdepth;                                    // Select Our Color Depth
            pfd.cRedBits = 0;                                                   // Color Bits Ignored
            pfd.cRedShift = 0;
            pfd.cGreenBits = 0;
            pfd.cGreenShift = 0;
            pfd.cBlueBits = 0;
            pfd.cBlueShift = 0;
            pfd.cAlphaBits = 0;                                                 // No Alpha Buffer
            pfd.cAlphaShift = 0;                                                // Shift Bit Ignored
            pfd.cAccumBits = 0;                                                 // No Accumulation Buffer
            pfd.cAccumRedBits = 0;                                              // Accumulation Bits Ignored
            pfd.cAccumGreenBits = 0;
            pfd.cAccumBlueBits = 0;
            pfd.cAccumAlphaBits = 0;
            pfd.cDepthBits = 16;                                                // 16Bit Z-Buffer (Depth Buffer)
            pfd.cStencilBits = 0;                                               // No Stencil Buffer
            pfd.cAuxBuffers = 0;                                                // No Auxiliary Buffer
            pfd.iLayerType = (byte)Gdi.PFD_MAIN_PLANE;                         // Main Drawing Layer
            pfd.bReserved = 0;                                                  // Reserved
            pfd.dwLayerMask = 0;                                                // Layer Masks Ignored
            pfd.dwVisibleMask = 0;
            pfd.dwDamageMask = 0;

            // Attempt To Get A Device Context
            hDC = User.GetDC(mRenderTarget.Handle);                      
            if (hDC == IntPtr.Zero)
            {                                            
                // no device context
                KillGLWindow();                                                 
                throw new Exception("Can't Create A GL Device Context.");
            }

            // Attempt To Find An Appropriate Pixel Format
            pixelFormat = Gdi.ChoosePixelFormat(hDC, ref pfd);
            if (pixelFormat == 0)
            {                      
                // couldn't find a matching pixel format (wtf does this mean, anyway?)
                KillGLWindow();                                                 
                throw new Exception("Can't Find A Suitable PixelFormat.");
            }

            if (!Gdi.SetPixelFormat(hDC, pixelFormat, ref pfd))
            {                
                // unable to set the pixel format
                KillGLWindow();                  
                throw new Exception("Can't Set The PixelFormat.");
            }

            // Attempt To Get The Rendering Context
            hRC = Wgl.wglCreateContext(hDC);     

            if (hRC == IntPtr.Zero)
            {                                            
                KillGLWindow();                                                 // Reset The Display
                throw new Exception("Can't Create A GL Rendering Context.");
            }


            // Set Up Our Perspective GL Screen
            ReSizeGLScene(mRenderTarget.Width, mRenderTarget.Height);

            
        }

        public override void Dispose()
        {
            KillGLWindow();

            frm.Dispose();

            mClosed = true;
        }

        /// <summary>
        ///     Properly kill the window.
        /// </summary>
        internal void KillGLWindow()
        {
            if (mIsFullscreen)
            {                                                    // Are We In Fullscreen Mode?
                User.ChangeDisplaySettings(IntPtr.Zero, 0);                     // If So, Switch Back To The Desktop
                Cursor.Show();                                                  // Show Mouse Pointer
            }

            if (hRC != IntPtr.Zero)
            {                                            // Do We Have A Rendering Context?
                if (!Wgl.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero))
                {             // Are We Able To Release The DC and RC Contexts?
                    MessageBox.Show("Release Of DC And RC Failed.", "SHUTDOWN ERROR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (!Wgl.wglDeleteContext(hRC))
                {                                // Are We Able To Delete The RC?
                    MessageBox.Show("Release Rendering Context Failed.", "SHUTDOWN ERROR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                hRC = IntPtr.Zero;                                              // Set RC To Null
            }

            if (hDC != IntPtr.Zero)
            {                                            // Do We Have A Device Context?
                if (frm != null && !frm.IsDisposed)
                {                          // Do We Have A Window?
                    if (mRenderTarget.Handle != IntPtr.Zero)
                    {                            // Do We Have A Window Handle?
                        if (!User.ReleaseDC(mRenderTarget.Handle, hDC))
                        {                 // Are We Able To Release The DC?
                            MessageBox.Show("Release Device Context Failed.", "SHUTDOWN ERROR",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                hDC = IntPtr.Zero;                                              // Set DC To Null
            }

            frm.Dispose();
        }

        /// <summary>
        ///     Resizes and initializes the GL window.
        /// </summary>
        /// <param name="width">
        ///     The new window width.
        /// </param>
        /// <param name="height">
        ///     The new window height.
        /// </param>
        private static void ReSizeGLScene(int width, int height)
        {
            if (height == 0)
            {                                                   // Prevent A Divide By Zero...
                height = 1;                                                     // By Making Height Equal To One
            }

            Gl.glViewport(0, 0, width, height);                                 // Reset The Current Viewport
            Gl.glMatrixMode(Gl.GL_PROJECTION);                                  // Select The Projection Matrix
            Gl.glLoadIdentity();                                                // Reset The Projection Matrix
            Glu.gluPerspective(45, width / (double)height, 0.1, 100);          // Calculate The Aspect Ratio Of The Window
            Gl.glMatrixMode(Gl.GL_MODELVIEW);                                   // Select The Modelview Matrix
            Gl.glLoadIdentity();                                                // Reset The Modelview Matrix
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

        public Form Form
        {
            get { return frm; }
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
            get { return false; }
        }

        public override void ToggleFullScreen()
        {
            mIsFullscreen = !mIsFullscreen;

            OnResize();
        }
        */
    }
}
