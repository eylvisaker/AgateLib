// Some of the code used here is based off NeHe tutorials.

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ERY.AgateLib;
using ERY.AgateLib.Drivers;
using ERY.AgateLib.Geometry;
using ERY.AgateLib.ImplBase;

using OpenTK.OpenGL;
using Gl = OpenTK.OpenGL.GL;

namespace ERY.AgateLib.OpenGL
{
    public sealed class GL_Display : DisplayImpl, IDisplayCaps 
    {
        GL_IRenderTarget mRenderTarget;
        GLState mState;
        Stack<Rectangle> mClipRects = new Stack<Rectangle>();
        Rectangle mCurrentClip = Rectangle.Empty;
        private bool mVSync = true;
        private int mMaxLightsUsed = 0;

        protected override void OnRenderTargetChange(IRenderTarget oldRenderTarget)
        {
            mRenderTarget = RenderTarget.Impl as GL_IRenderTarget;
            mRenderTarget.MakeCurrent();

            OnRenderTargetResize();
        }

        protected override void OnRenderTargetResize()
        {

        }

        public override PixelFormat DefaultSurfaceFormat
        {
            get { return PixelFormat.RGBA8888; }
        }
        public override DisplayWindowImpl CreateDisplayWindow(string title, int clientWidth, int clientHeight, 
            string iconFile, bool startFullscreen, bool allowResize)
        {
            return new GL_DisplayWindow(title, clientWidth, clientHeight,
                iconFile, startFullscreen, allowResize);
        }

        public override DisplayWindowImpl CreateDisplayWindow(System.Windows.Forms.Control renderTarget)
        {
            return new GL_DisplayWindow(renderTarget);
        }

        public override SurfaceImpl CreateSurface(string fileName)
        {
            return new GL_Surface(fileName);
        }

        public override SurfaceImpl CreateSurface(Size surfaceSize)
        {
            return new GL_Surface(surfaceSize);
        }

        public override FontSurfaceImpl CreateFont(string fontFamily, float sizeInPoints, FontStyle style)
        {
            return BitmapFontImpl.FromOSFont(fontFamily, sizeInPoints, style);
        }

        internal void SetupGLOrtho(Rectangle ortho)
        {
            SetOrthoProjection(ortho);

            Gl.Enable(Enums.EnableCap.TEXTURE_2D);

            Gl.Enable(Enums.EnableCap.BLEND);
            Gl.BlendFunc(Enums.BlendingFactorSrc.SRC_ALPHA, Enums.BlendingFactorDest.ONE_MINUS_SRC_ALPHA);

            Gl.MatrixMode(Enums.MatrixMode.MODELVIEW);
            Gl.LoadIdentity();
        }
        protected override void OnBeginFrame()
        {
            mRenderTarget.BeginRender();
        }

        protected override void OnEndFrame()
        {
            mState.DrawBuffer.Flush();

            mRenderTarget.EndRender();
        }

        internal GLState State
        {
            get { return mState; }
        }

        // TODO: Make this not hardcoded
        public override Size MaxSurfaceSize
        {
            get { return new Size(1024, 1024); }
        }

        // TODO: Test clip rect stuff.
        public override void SetClipRect(Rectangle newClipRect)
        {
            Gl.Viewport(newClipRect.X, mRenderTarget.Height - newClipRect.Bottom,
                newClipRect.Width, newClipRect.Height );

            SetupGLOrtho(newClipRect);

            mCurrentClip = newClipRect;
        }

        public override void PushClipRect(Rectangle newClipRect)
        {
            mClipRects.Push(mCurrentClip);

            SetClipRect(newClipRect);
        }

        public override void PopClipRect()
        {
            SetClipRect(mClipRects.Peek());

            mClipRects.Pop();
        }


        public override void FlushDrawBuffer()
        {
            mState.DrawBuffer.Flush();
        }

        public override void SetOrthoProjection(Rectangle region)
        {
            Gl.MatrixMode(Enums.MatrixMode.PROJECTION);
            Gl.LoadIdentity();
            Glu.Ortho2D(region.Left, region.Right, region.Bottom, region.Top);

        }
        public override void Clear(Color color)
        {
            mState.DrawBuffer.Flush();

            Gl.ClearColor(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, 1.0f);
            Gl.Clear(Enums.ClearBufferMask.DEPTH_BUFFER_BIT | Enums.ClearBufferMask.COLOR_BUFFER_BIT);   
        }

        public override void Clear(Color color, Rectangle dest)
        {
            mState.DrawBuffer.Flush();

            DrawRect(dest, Color.FromArgb(255, color));
        }

        public override void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            mState.DrawBuffer.Flush();

            mState.SetGLColor(color);

            Gl.Disable(Enums.EnableCap.TEXTURE_2D);
            Gl.Begin(Enums.BeginMode.LINES);

            Gl.Vertex2d(x1, y1);
            Gl.Vertex2d(x2, y2);

            Gl.End();
            Gl.Enable(Enums.EnableCap.TEXTURE_2D);
        }

        public override void DrawLine(Point a, Point b, Color color)
        {
            mState.DrawBuffer.Flush();

            DrawLine(a.X, a.Y, b.X, b.Y, color);
        }

        public override void DrawRect(Rectangle rect, Color color)
        {
            mState.DrawBuffer.Flush();
            mState.SetGLColor(color);

            // hacks here to make it come out right?
            // rect.Y++ and rect.Right +1 down below.
            rect.Y++;

            Gl.Disable(Enums.EnableCap.TEXTURE_2D);
            Gl.Begin(Enums.BeginMode.LINES);

            Gl.Vertex2d(rect.Left, rect.Top);
            Gl.Vertex2d(rect.Right+1, rect.Top);

            Gl.Vertex2d(rect.Right, rect.Top);
            Gl.Vertex2d(rect.Right, rect.Bottom);

            Gl.Vertex2d(rect.Right, rect.Bottom);
            Gl.Vertex2d(rect.Left, rect.Bottom);

            Gl.Vertex2d(rect.Left, rect.Bottom);
            Gl.Vertex2d(rect.Left, rect.Top);

            Gl.End();
            Gl.Enable(Enums.EnableCap.TEXTURE_2D);
        }

        public override void FillRect(Rectangle rect, Color color)
        {
            mState.DrawBuffer.Flush();

            mState.SetGLColor(color);

            Gl.Disable(Enums.EnableCap.TEXTURE_2D);

            Gl.Begin(Enums.BeginMode.QUADS);
            Gl.Vertex3f(rect.Left, rect.Top, 0);                                        // Top Left
            Gl.Vertex3f(rect.Right, rect.Top, 0);                                         // Top Right
            Gl.Vertex3f(rect.Right, rect.Bottom, 0);                                        // Bottom Right
            Gl.Vertex3f(rect.Left, rect.Bottom, 0);                                       // Bottom Left
            Gl.End();                                                         // Done Drawing The Quad

            Gl.Enable(Enums.EnableCap.TEXTURE_2D);
        }


        public override bool VSync
        {
            get { return mVSync; }
            set { mVSync = value; }
        }

        public override void Initialize()
        {
            mState = new GLState();

            Report("OpenTK / OpenGL driver instantiated for display.");
        }
        internal void Initialize(GL_DisplayWindow gL_DisplayWindow)
        {
            Gl.ShadeModel(Enums.ShadingModel.SMOOTH);                         // Enable Smooth Shading
            Gl.ClearColor(0, 0, 0, 1.0f);                                     // Black Background
            Gl.ClearDepth(1);                                                 // Depth Buffer Setup
            Gl.Enable(Enums.EnableCap.DEPTH_TEST);                            // Enables Depth Testing
            Gl.DepthFunc(Enums.DepthFunction.LEQUAL);                         // The Type Of Depth Testing To Do
            Gl.Hint(Enums.HintTarget.PERSPECTIVE_CORRECTION_HINT,             // Really Nice Perspective Calculations
                Enums.HintMode.NICEST);

        }
 
        public override void Dispose()
        {
        }

        public static void Register()
        {
            Registrar.RegisterDisplayDriver(
                new DriverInfo<DisplayTypeID>(typeof(GL_Display),
                DisplayTypeID.OpenGL, "OpenGL with OpenTK", 120));
        }

        public override void DoLighting(LightManager lights)
        {
            FlushDrawBuffer();

            if (lights.Enabled == false)
            {
                GL.Disable(Enums.EnableCap.LIGHTING);
                return;
            }

            float[] array = new float[4];

            GL.Enable(Enums.EnableCap.LIGHTING);

            SetArray(array, lights.Ambient);
            GL.LightModelfv(Enums.LightModelParameter.LIGHT_MODEL_AMBIENT, array);

            GL.Enable(Enums.EnableCap.COLOR_MATERIAL);
            GL.ColorMaterial(Enums.MaterialFace.FRONT_AND_BACK, 
                             Enums.ColorMaterialParameter.AMBIENT_AND_DIFFUSE);

            for (int i = 0; i < lights.Count || i < mMaxLightsUsed; i++)
            {
                Enums.EnableCap lightID = (Enums.EnableCap)((int)Enums.EnableCap.LIGHT0 + i);
                Enums.LightName lightName = (Enums.LightName)((int)Enums.LightName.LIGHT0 + i);

                if (i >= lights.Count)
                {
                    GL.Disable(lightID);
                    continue;
                }

                if (lights[i].Enabled == false)
                {
                    GL.Disable(lightID);
                    continue;
                }

                GL.Enable(lightID);

                SetArray(array, lights[i].Diffuse);
                GL.Lightfv(lightName, Enums.LightParameter.DIFFUSE, array);

                SetArray(array, lights[i].Ambient);
                GL.Lightfv(lightName, Enums.LightParameter.AMBIENT, array);

                SetArray(array, lights[i].Position);
                GL.Lightfv(lightName, Enums.LightParameter.POSITION, array);

                GL.Lightf(lightName, Enums.LightParameter.CONSTANT_ATTENUATION, lights[i].AttenuationConstant);
                GL.Lightf(lightName, Enums.LightParameter.LINEAR_ATTENUATION, lights[i].AttenuationLinear);
                GL.Lightf(lightName, Enums.LightParameter.QUADRATIC_ATTENUATION, lights[i].AttenuationQuadratic);

            }

            mMaxLightsUsed = lights.Count;

        }
        private void SetArray(float[] array, Vector3 vec)
        {
            array[0] = vec.X;
            array[1] = vec.Y;
            array[2] = vec.Z;
        }
        private void SetArray(float[] array,Color color)
        {
            array[0] = color.R / 255.0f;
            array[1] = color.G / 255.0f;
            array[2] = color.B / 255.0f;
            array[3] = color.A / 255.0f;
        } 

        public override IDisplayCaps Caps
        {
            get { return this; }
        }

        #region IDisplayCaps Members

        bool IDisplayCaps.SupportsScaling
        {
            get { return true; }
        }

        bool IDisplayCaps.SupportsRotation
        {
            get { return true; }
        }

        bool IDisplayCaps.SupportsColor
        {
            get { return true; }
        }

        bool IDisplayCaps.SupportsSurfaceAlpha
        {
            get { return true; }
        }

        bool IDisplayCaps.SupportsPixelAlpha
        {
            get { return true; }
        }

        bool IDisplayCaps.SupportsLighting
        {
            get { return true; }
        }

        int IDisplayCaps.MaxLights
        {
            get
            {
                int[] max = new int[1];
                GL.GetIntegerv(Enums.GetPName.MAX_LIGHTS, max);

                return max[0];
            }
        }

        bool IDisplayCaps.IsHardwareAccelerated
        {
            get { return true; }
        }

        bool IDisplayCaps.Supports3D
        {
            get { return false; }
        }
        bool IDisplayCaps.SupportsFullScreen
        {
            get { return true; }
        }
        bool IDisplayCaps.SupportsFullScreenModeSwitching
        {
            get { return true; }
        }

        #endregion
    }
}