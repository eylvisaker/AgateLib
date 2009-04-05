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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

using AgateLib.BitmapFont;
using AgateLib.DisplayLib;
using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.ImplementationBase;

using OpenTK.Graphics;
using PixelFormat = AgateLib.DisplayLib.PixelFormat;

namespace AgateOTK
{
    public sealed class GL_Display : DisplayImpl, IDisplayCaps, AgateLib.PlatformSpecific.IPlatformServices
    {
        GL_IRenderTarget mRenderTarget;
        GLState mState;
        Stack<Rectangle> mClipRects = new Stack<Rectangle>();
        Rectangle mCurrentClip = Rectangle.Empty;
        private bool mVSync = true;
        private int mMaxLightsUsed = 0;
        private bool mSupportsFramebuffer;
        private bool mNonPowerOf2Textures;
        private bool mSupportsShaders;

        public bool NonPowerOf2Textures
        {
            get { return mNonPowerOf2Textures; }
            private set { mNonPowerOf2Textures = value; }
        }

        public bool SupportsFramebuffer
        {
            get { return mSupportsFramebuffer; }
            set { mSupportsFramebuffer = value; }
        }

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
            return new GL_DisplayControl(windowParams);

            //if (windowParams.RenderToControl)
            //{
            //    return new GL_DisplayControl(windowParams);
            //}
            //else
            //{
            //    return new GL_GameWindow(windowParams);
            //}
        }
        public override SurfaceImpl CreateSurface(string fileName)
        {
            return new GL_Surface(fileName);
        }
        protected override VertexBufferImpl CreateVertexBuffer()
        {
            return new GL_VertexBuffer();
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

            return AgateLib.WinForms.BitmapFontUtil.ConstructFromOSFont(options);
        }
        public override FontSurfaceImpl CreateFont(BitmapFontOptions bitmapOptions)
        {
            return AgateLib.WinForms.BitmapFontUtil.ConstructFromOSFont(bitmapOptions);
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
                newClipRect.Width, newClipRect.Height);

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

        Matrix4 projection = Matrix4.Identity;
        Matrix4 world = Matrix4.Identity;
        Matrix4 view = Matrix4.Identity;

        public override Matrix4 MatrixProjection
        {
            get { return projection; }
            set
            {
                projection = value;
                SetProjection();
            }
        }
        public override Matrix4 MatrixView
        {
            get { return view; }
            set
            {
                view = value;
                SetModelview();
            }
        }
        public override Matrix4 MatrixWorld
        {
            get { return world; }
            set
            {
                world = value;
                SetModelview();
            }
        }

        private void SetModelview()
        {
            OpenTK.Math.Matrix4 modelview = ConvertAgateMatrix(view * world, false);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LoadMatrix(ref modelview);
        }
        private void SetProjection()
        {
            OpenTK.Math.Matrix4 otkProjection = ConvertAgateMatrix(projection, false);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.LoadMatrix(ref otkProjection);
        }

        private OpenTK.Math.Matrix4 ConvertAgateMatrix(Matrix4 matrix, bool invertY)
        {
            int sign = invertY ? -1 : 1;

            return new OpenTK.Math.Matrix4(
                new OpenTK.Math.Vector4(matrix[0, 0], sign * matrix[1, 0], matrix[2, 0], matrix[3, 0]),
                new OpenTK.Math.Vector4(matrix[0, 1], sign * matrix[1, 1], matrix[2, 1], matrix[3, 1]),
                new OpenTK.Math.Vector4(matrix[0, 2], sign * matrix[1, 2], matrix[2, 2], matrix[3, 2]),
                new OpenTK.Math.Vector4(matrix[0, 3], sign * matrix[1, 3], matrix[2, 3], matrix[3, 3]));
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

            GL.Vertex2(x1, y1 + 0.5);
            GL.Vertex2(x2, y2 + 0.5);

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
            DrawRect(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), color);
        }
        public override void DrawRect(RectangleF rect, Color color)
        {
            mState.DrawBuffer.Flush();
            mState.SetGLColor(color);

            GL.Disable(EnableCap.Texture2D);
            GL.Begin(BeginMode.Lines);

            GL.Vertex2(rect.Left, rect.Top);
            GL.Vertex2(rect.Right, rect.Top);

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
            FillRect(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), color);
        }
        public override void FillRect(RectangleF rect, Color color)
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
            FillRect(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), color);
        }
        public override void FillRect(RectangleF rect, Gradient color)
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

        bool glInitialized = false;

        internal void InitializeGL()
        {
            if (glInitialized)
                return;

            GL.ShadeModel(ShadingModel.Smooth);                         // Enable Smooth Shading
            GL.ClearColor(0, 0, 0, 1.0f);                                     // Black Background
            GL.ClearDepth(1);                                                 // Depth Buffer Setup
            GL.Enable(EnableCap.DepthTest);                            // Enables Depth Testing
            GL.DepthFunc(DepthFunction.Lequal);                         // The Type Of Depth Testing To Do
            GL.Hint(HintTarget.PerspectiveCorrectionHint,             // Really Nice Perspective Calculations
                HintMode.Nicest);

            string versionString = GL.GetString(StringName.Version);
            double version = double.Parse(versionString.Substring(0, 3));
            string ext = GL.GetString(StringName.Extensions);
            mSupportsShaders = false;

            // don't want stupid floating point comparision to improperly fail here.
            if (version >= 1.9999)  // version 2
            {
                mSupportsFramebuffer = true;
                mNonPowerOf2Textures = true;
                mSupportsShaders = true;

            }
            else if (version >= 1.49999) // version 1.5
            {
                mSupportsFramebuffer = true;
                mNonPowerOf2Textures = true;
            }
            else
            {
                mSupportsFramebuffer = GL.SupportsExtension("GL_EXT_FRAMEBUFFER_OBJECT");
                mNonPowerOf2Textures = GL.SupportsExtension("GL_ARB_NON_POWER_OF_TWO");
            }

            if (GL.SupportsExtension("GL_ARB_FRAGMENT_PROGRAM"))
            {
                mSupportsShaders = true;
            }

            if (mSupportsShaders)
                InitializeShaders();

            glInitialized = true;
            
        }

        GlslShader mCurrentShader;

        public override AgateLib.DisplayLib.Shaders.ShaderProgram Shader
        {
            get
            {
                return mCurrentShader;
            }
            set
            {
                if (value is GlslShader == false)
                    throw new AgateLib.AgateException(string.Format(
                        "Shader type is {0} but must be GlslShader.", typeof(ValueType)));

                mCurrentShader = (GlslShader)value;

                if (mCurrentShader == null)
                {
                    GL.UseProgram(0);
                }
                else
                {
                    GL.UseProgram(mCurrentShader.Handle);
                }

            }
        }
        public override void Dispose()
        {
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
            GL.LightModelv(LightModelParameter.LightModelAmbient, array);

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
        private void SetArray(float[] array, Color color)
        {
            array[0] = color.R / 255.0f;
            array[1] = color.G / 255.0f;
            array[2] = color.B / 255.0f;
            array[3] = color.A / 255.0f;
        }

        protected override void SavePixelBuffer(PixelBuffer pixelBuffer, string filename, ImageFileFormat format)
        {
            AgateLib.WinForms.FormUtil.SavePixelBuffer(pixelBuffer, filename, format);
        }

        public override IDisplayCaps Caps
        {
            get { return this; }
        }
        #region IPlatformServices Members

        protected override AgateLib.PlatformSpecific.IPlatformServices GetPlatformServices()
        {
            return this;
        }
        AgateLib.Utility.PlatformType AgateLib.PlatformSpecific.IPlatformServices.PlatformType
        {
            get
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.WinCE:
                    case PlatformID.Win32NT:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                        return AgateLib.Utility.PlatformType.Windows;

                    case PlatformID.Unix:
                        string kernel = DetectUnixKernel();

                        if (kernel == "Darwin")
                            return AgateLib.Utility.PlatformType.MacOS;
                        else
                            return AgateLib.Utility.PlatformType.Linux;
                }

                return AgateLib.Utility.PlatformType.Unknown;
            }
        }

        #region DetectUnixKernel()

        /// <summary>
        /// Borrowed from OpenTK source.  
        /// Executes "uname" which returns a string representing the name of the
        /// underlying Unix kernel.
        /// </summary>
        /// <returns>"Unix", "Linux", "Darwin" or null.</returns>
        /// <remarks>Source code from "Mono: A Developer's Notebook"</remarks>
        private static string DetectUnixKernel()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = "-s";
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            foreach (string unameprog in new string[] { "/usr/bin/uname", "/bin/uname", "uname" })
            {
                try
                {
                    startInfo.FileName = unameprog;
                    Process uname = Process.Start(startInfo);
                    StreamReader stdout = uname.StandardOutput;
                    return stdout.ReadLine().Trim();
                }
                catch (System.IO.FileNotFoundException)
                {
                    // The requested executable doesn't exist, try next one.
                    continue;
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    continue;
                }
            }
            return null;
        }

        #endregion

        #endregion

        #region --- Shaders ---

        protected override ShaderCompilerImpl CreateShaderCompiler()
        {
            if (this.Caps.SupportsShaders)
            {
                return new GlslShaderCompiler();
            }
            else
                return base.CreateShaderCompiler();
        }

        #endregion

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
            get { return true; }
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

        bool IDisplayCaps.SupportsShaders
        {
            get
            {
                return mSupportsShaders;
            }
        }

        AgateLib.DisplayLib.Shaders.ShaderLanguage IDisplayCaps.ShaderLanguage
        {
            get { return AgateLib.DisplayLib.Shaders.ShaderLanguage.Glsl; }
        }

        #endregion
    }
}