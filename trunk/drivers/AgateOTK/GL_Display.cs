// Some of the code used here is based off NeHe tutorials.

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ERY.AgateLib;
using ERY.AgateLib.BitmapFont;
using ERY.AgateLib.Drivers;
using ERY.AgateLib.Geometry;
using ERY.AgateLib.ImplBase;

using OpenTK.Graphics.OpenGL;

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

        internal event EventHandler ProcessEventsEvent;

        protected override void ProcessEvents()
        {
            if (ProcessEventsEvent != null)
                ProcessEventsEvent(this, EventArgs.Empty);
        }

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

        public override DisplayWindowImpl CreateDisplayWindow(CreateWindowParams windowParams)
        {
            if (windowParams.RenderToControl)
            {
                return new GL_DisplayControl(windowParams);
            }
            else
            {
                return new GL_GameWindow(windowParams);
            }
        }
        public override SurfaceImpl CreateSurface(string fileName)
        {
            return new GL_Surface(fileName);
        }

        public override SurfaceImpl CreateSurface(Size surfaceSize)
        {
            return new GL_Surface(surfaceSize);
        }
        public override SurfaceImpl CreateSurface(System.IO.Stream fileStream)
        {
            return new GL_Surface(fileStream);
        }
        public override FontSurfaceImpl CreateFont(string fontFamily, float sizeInPoints, FontStyle style)
        {
            BitmapFontOptions options = new BitmapFontOptions(fontFamily, sizeInPoints, style);

            return WinForms.BitmapFontUtil.ConstructFromOSFont(options);
        }
        public override FontSurfaceImpl CreateFont(BitmapFontOptions bitmapOptions)
        {
            return WinForms.BitmapFontUtil.ConstructFromOSFont(bitmapOptions);
        }

        internal void SetupGLOrtho(Rectangle ortho)
        {
            SetOrthoProjection(ortho);

            GL.Enable(EnableCap.Texture2D);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
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
            GL.Viewport(newClipRect.X, mRenderTarget.Height - newClipRect.Bottom,
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
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Glu.Ortho2D(region.Left, region.Right, region.Bottom, region.Top);

        }
        public override void Clear(Color color)
        {
            mState.DrawBuffer.Flush();

            GL.ClearColor(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, 1.0f);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);   
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

            GL.Disable(EnableCap.Texture2D);
            GL.Begin(BeginMode.Lines);

            GL.Vertex2(x1, y1+0.5);
            GL.Vertex2(x2, y2+0.5);

            GL.End();
            GL.Enable(EnableCap.Texture2D);
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

            GL.Disable(EnableCap.Texture2D);
            GL.Begin(BeginMode.Lines);

            GL.Vertex2(rect.Left, rect.Top);
            GL.Vertex2(rect.Right+1, rect.Top);

            GL.Vertex2(rect.Right, rect.Top);
            GL.Vertex2(rect.Right, rect.Bottom);

            GL.Vertex2(rect.Right, rect.Bottom);
            GL.Vertex2(rect.Left, rect.Bottom);

            GL.Vertex2(rect.Left, rect.Bottom);
            GL.Vertex2(rect.Left, rect.Top);

            GL.End();
            GL.Enable(EnableCap.Texture2D);
        }

        public override void FillRect(Rectangle rect, Color color)
        {
            mState.DrawBuffer.Flush();

            mState.SetGLColor(color);

            GL.Disable(EnableCap.Texture2D);

            GL.Begin(BeginMode.Quads);
            GL.Vertex3(rect.Left, rect.Top, 0);                                        // Top Left
            GL.Vertex3(rect.Right, rect.Top, 0);                                         // Top Right
            GL.Vertex3(rect.Right, rect.Bottom, 0);                                        // Bottom Right
            GL.Vertex3(rect.Left, rect.Bottom, 0);                                       // Bottom Left
            GL.End();                                                         // Done Drawing The Quad

            GL.Enable(EnableCap.Texture2D);
        }
        public override void FillRect(Rectangle rect, Gradient color)
        {
            mState.DrawBuffer.Flush();

            GL.Disable(EnableCap.Texture2D);

            GL.Begin(BeginMode.Quads);
            mState.SetGLColor(color.TopLeft);
            GL.Vertex3(rect.Left, rect.Top, 0);                                        // Top Left

            mState.SetGLColor(color.TopRight);
            GL.Vertex3(rect.Right, rect.Top, 0);                                         // Top Right

            mState.SetGLColor(color.BottomRight);
            GL.Vertex3(rect.Right, rect.Bottom, 0);                                        // Bottom Right

            mState.SetGLColor(color.BottomLeft);
            GL.Vertex3(rect.Left, rect.Bottom, 0);                                       // Bottom Left
            GL.End();                                                         // Done Drawing The Quad

            GL.Enable(EnableCap.Texture2D);
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
        internal void InitializeGL()
        {
            GL.ShadeModel(ShadingModel.Smooth);                         // Enable Smooth Shading
            GL.ClearColor(0, 0, 0, 1.0f);                                     // Black Background
            GL.ClearDepth(1);                                                 // Depth Buffer Setup
            GL.Enable(EnableCap.DepthTest);                            // Enables Depth Testing
            GL.DepthFunc(DepthFunction.Lequal);                         // The Type Of Depth Testing To Do
            GL.Hint(HintTarget.PerspectiveCorrectionHint,             // Really Nice Perspective Calculations
                HintMode.Nicest);
        }
 
        public override void Dispose()
        {
        }

        public static void Register()
        {
            Registrar.RegisterDisplayDriver(
                new DriverInfo<DisplayTypeID>(typeof(GL_Display),
                DisplayTypeID.OpenGL, "OpenGL with OpenTK 0.9.1", 1120));
        }

        public override void DoLighting(LightManager lights)
        {
            FlushDrawBuffer();

            if (lights.Enabled == false)
            {
                GL.Disable(EnableCap.Lighting);
                return;
            }

            float[] array = new float[4];

            GL.Enable(EnableCap.Lighting);

            SetArray(array, lights.Ambient);
            GL.LightModelv (LightModelParameter.LightModelAmbient, array);

            GL.Enable(EnableCap.ColorMaterial);
            GL.ColorMaterial(MaterialFace.FrontAndBack, 
                             ColorMaterialParameter.AmbientAndDiffuse);

            for (int i = 0; i < lights.Count || i < mMaxLightsUsed; i++)
            {
                EnableCap lightID = (EnableCap)((int)EnableCap.Light0 + i);
                LightName lightName = (LightName)((int)LightName.Light0 + i);

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
                GL.Lightv(lightName, LightParameter.Diffuse, array);

                SetArray(array, lights[i].Ambient);
                GL.Lightv(lightName, LightParameter.Ambient, array);

                SetArray(array, lights[i].Position);
                GL.Lightv(lightName, LightParameter.Position, array);

                GL.Light(lightName, LightParameter.ConstantAttenuation, lights[i].AttenuationConstant);
                GL.Light(lightName, LightParameter.LinearAttenuation, lights[i].AttenuationLinear);
                GL.Light(lightName, LightParameter.QuadraticAttenuation, lights[i].AttenuationQuadratic);

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

        #region --- IDisplayCaps Members ---

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
        bool IDisplayCaps.SupportsGradient
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
                GL.GetInteger(GetPName.MaxLights, max);

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

        bool IDisplayCaps.CanCreateBitmapFont
        {
            get { return true; }
        }

        #endregion
    }
}