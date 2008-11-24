// Some of the code used here is based off NeHe tutorials.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using Tao.OpenGl;
using Tao.Platform.Windows;


namespace ERY.GameLibrary
{
    public class WGL_Display : DisplayImpl
    {
        string mPath;

        WGL_DisplayWindow mOGLWindow;

        public override void Initialize()
        {
            Kernel.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);

            Gl.glShadeModel(Gl.GL_SMOOTH);                                      // Enable Smooth Shading
            Gl.glClearColor(0, 0, 0, 1.0f);                                     // Black Background
            Gl.glClearDepth(1);                                                 // Depth Buffer Setup
            Gl.glEnable(Gl.GL_DEPTH_TEST);                                      // Enables Depth Testing
            Gl.glDepthFunc(Gl.GL_LEQUAL);                                       // The Type Of Depth Testing To Do
            Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);         // Really Nice Perspective Calculations

        }

        public static void Register()
        {
            Registrar.RegisterDisplayDriver(
                new DisplayDriverInfo(typeof(WGL_Display), DisplayTypeID.WGL,
                "OpenGL for Windows", 50));
        }

        public override void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string ImagePath
        {
            get { return mPath; }
            set { mPath = value; }
        }

        protected override void OnCurrentWindowChange()
        {
            mOGLWindow = mCurrentWindow.Impl as WGL_DisplayWindow;

            // Try To Activate The Rendering Context
            if (!Wgl.wglMakeCurrent(mOGLWindow.hDC, mOGLWindow.hRC))
            {                                 
                mOGLWindow.KillGLWindow();      // Reset The Display
                throw new Exception("Can't Activate The GL Rendering Context.");
            }
 
        }

        public override DisplayWindowImpl CreateDisplayWindow(string title, int clientWidth, int clientHeight, bool startFullScreen, bool allowResize)
        {
            WGL_DisplayWindow retval = new WGL_DisplayWindow(title, clientWidth, clientHeight, startFullScreen, allowResize);


            retval.InitializeGL();

            return retval;
        }
        public override DisplayWindowImpl CreateDisplayWindow(System.Windows.Forms.Control renderTarget)
        {
            return new WGL_DisplayWindow(renderTarget);
        }

        public override SurfaceImpl CreateSurface(Surface owner, string fileName)
        {
            return new WGL_Surface(owner, fileName);
        }
        public override SurfaceImpl CreateSurface(Surface owner, System.Drawing.Size surfaceSize)
        {
            return new WGL_Surface(owner, surfaceSize);
        }

        public override FontSurfaceImpl CreateFont(FontSurface owner, System.Drawing.Font font)
        {
            return new WGL_FontSurface(owner, font);
        }


        public override void BeginFrame()
        {
            DateTime now = System.DateTime.Now;

            if (mRanOnce == false)
            {
                mRanOnce = true;

                mDeltaTime = 0;
                mLastTime = now;

                mFPSStart = now;
                mFrames = 0;

            }
            else
            {
                TimeSpan delta = now - mLastTime;
                mDeltaTime = delta.TotalMilliseconds;
                mLastTime = now;

                TimeSpan framesTime = now - mFPSStart;

                if (framesTime.TotalMilliseconds > 100)
                {
                    double time = framesTime.TotalSeconds;

                    // average current framerate with that of the last update
                    mFPS = 0.5 * (mFrames / time + mFPS);

                    mFPSStart = now;
                    mFrames = 0;

                }
            }


            //SetClipRect(new Rectangle(new Point(0, 0), mD3DWindow.Size));
            Gl.glMatrixMode(Gl.GL_PROJECTION); 
            Gl.glLoadIdentity(); 
            Glu.gluOrtho2D(0, mOGLWindow.Width, mOGLWindow.Height, 0);

            Gl.glEnable(Gl.GL_TEXTURE_2D);

            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
        }

        public override void EndFrame(bool waitVSync)
        {
            while (mClipRects.Count > 0)
                PopClipRect();

            Gdi.SwapBuffers(mOGLWindow.hDC);                                   // Swap Buffers (Double Buffering)

            mFrames++;
        }

        public override double DeltaTime
        {
            get
            {
                return mDeltaTime;
            }
        }
        public override void SetDeltaTime(double deltaTime)
        {
            mDeltaTime = deltaTime;
        }
        public override double FramesPerSecond
        {
            get { return mFPS; }
        }
        private double mDeltaTime;
        private DateTime mLastTime;
        private bool mRanOnce = false;

        private DateTime mFPSStart;
        private int mFrames;
        private double mFPS = 0;

        private Stack<Rectangle> mClipRects = new Stack<Rectangle>();

        public override void SetClipRect(System.Drawing.Rectangle newClipRect)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void PushClipRect(System.Drawing.Rectangle newClipRect)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void PopClipRect()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DrawLine(int x1, int y1, int x2, int y2, System.Drawing.Color color)
        {
            SetGLColor(color);

            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glBegin(Gl.GL_LINES);

            Gl.glVertex2d(x1, y1);
            Gl.glVertex2d(x2, y2);

            Gl.glEnd();
            Gl.glEnable(Gl.GL_TEXTURE_2D);
        }

        public override void DrawLine(System.Drawing.Point a, System.Drawing.Point b, System.Drawing.Color color)
        {
            SetGLColor(color);

            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glBegin(Gl.GL_LINE);

            Gl.glVertex2d(a.X, a.Y);
            Gl.glVertex2d(b.X, b.Y);
            Gl.glEnable(Gl.GL_TEXTURE_2D);

            Gl.glEnd();          
        }

        public override void DrawRect(System.Drawing.Rectangle rect, System.Drawing.Color color)
        {
            SetGLColor(color);


            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glBegin(Gl.GL_LINES);

            Gl.glVertex2d(rect.Left, rect.Top);
            Gl.glVertex2d(rect.Right, rect.Top);

            Gl.glVertex2d(rect.Right, rect.Top);
            Gl.glVertex2d(rect.Right, rect.Bottom);

            Gl.glVertex2d(rect.Right, rect.Bottom);
            Gl.glVertex2d(rect.Left, rect.Bottom);

            Gl.glVertex2d(rect.Left, rect.Bottom);
            Gl.glVertex2d(rect.Left, rect.Top);

            Gl.glEnd();
            Gl.glEnable(Gl.GL_TEXTURE_2D);
        }

        public override void FillRect(System.Drawing.Rectangle rect, System.Drawing.Color color)
        {
            SetGLColor(color);

            Gl.glDisable(Gl.GL_TEXTURE_2D);

            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3f(rect.Left, rect.Top, 0);                                        // Top Left
            Gl.glVertex3f(rect.Right, rect.Top, 0);                                         // Top Right
            Gl.glVertex3f(rect.Right, rect.Bottom, 0);                                        // Bottom Right
            Gl.glVertex3f(rect.Left, rect.Bottom, 0);                                       // Bottom Left
            Gl.glEnd();                                                         // Done Drawing The Quad

            Gl.glEnable(Gl.GL_TEXTURE_2D);
        }

        
        public override void Clear(System.Drawing.Color color)
        {
            Gl.glClearColor(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, 0.5f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

        }

        public override void Clear(System.Drawing.Color color, System.Drawing.Rectangle dest)
        {
            DrawRect(dest, color);
        }


        public void SetGLColor(Color color)
        {
            Gl.glColor4f(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
        }

    }
}