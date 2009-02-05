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
using System.Text;
using Direct3D = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

using AgateLib.BitmapFont;
using AgateLib.DisplayLib;
using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.ImplementationBase;
using AgateLib.WinForms;

using Vector2 = Microsoft.DirectX.Vector2;
using ImageFileFormat = AgateLib.DisplayLib.ImageFileFormat;

namespace AgateMDX
{
    public class MDX1_Display : DisplayImpl, IDisplayCaps, AgateLib.PlatformSpecific.IPlatformServices 
    {
        #region --- Private Variables ---

        private D3DDevice mDevice;

        private MDX1_IRenderTarget mRenderTarget;

        private bool mInitialized = false;

        // variables for drawing primitives
        Direct3D.Line mLine;

        //Vector2[] mDrawLinePts = new Vector2[4];
        CustomVertex.PositionColored[] mFillRectVerts = new CustomVertex.PositionColored[6];

        private bool mVSync = true;

        #endregion
        #region --- Creation / Destruction ---

        public override void Initialize()
        {
            Report("Managed DirectX 1.1 driver instantiated for display.");

            AgateLib.Sprites.Old.Sprite.UseSpriteCache = true;
        }


        internal void Initialize(MDX1_DisplayWindow window)
        {
            if (mInitialized)
                return;

            if (window.RenderTarget.TopLevelControl == null)
                throw new ArgumentException("The specified render target does not have a Form object yet.  " +
                    "It's TopLevelControl property is null.  You may not create DisplayWindow objects before " +
                    "the control to render to is added to the Form.");

            mInitialized = true;

            // ok, create D3D device
            PresentParameters present = CreateWindowedPresentParameters(window, 0, 0);

            present.BackBufferWidth = 1;
            present.BackBufferHeight = 1;

            DeviceType dtype = DeviceType.Hardware;

            int adapterOrdinal = Direct3D.Manager.Adapters.Default.Adapter;

            Direct3D.Caps caps = Direct3D.Manager.GetDeviceCaps(adapterOrdinal, Direct3D.DeviceType.Hardware);
            Direct3D.CreateFlags flags = Direct3D.CreateFlags.SoftwareVertexProcessing;

            // Is there support for hardware vertex processing? If so, replace
            // software vertex processing.
            if (caps.DeviceCaps.SupportsHardwareTransformAndLight)
                flags = Direct3D.CreateFlags.HardwareVertexProcessing;

            // Does the device support a pure device?
            if (caps.DeviceCaps.SupportsPureDevice)
                flags |= Direct3D.CreateFlags.PureDevice;

            Device device = new Device
                (0, dtype, window.RenderTarget.TopLevelControl.Handle,
                 flags, present);

            device.DeviceLost += new EventHandler(mDevice_DeviceLost);
            device.DeviceReset += new EventHandler(mDevice_DeviceReset);

            mDevice = new D3DDevice(device);

            // create primitive objects
            mLine = new Direct3D.Line(device);

            //CreateSurfaceVB();

        }

        public override void Dispose()
        {
            if (mLine != null)
            {
                mLine.Dispose();
                mLine = null;
            }

            mDevice.Dispose();
        }

        #endregion

        #region --- Implementation Specific Public Properties ---

        public D3DDevice D3D_Device
        {
            get { return mDevice; }
        }

        #endregion

        #region --- Events ---

        public event EventHandler DeviceReset;
        public event EventHandler DeviceLost;
        public event EventHandler DeviceAboutToReset;

        private void OnDeviceReset()
        {
            mLine = new Line(mDevice.Device);
            System.Diagnostics.Debug.Print("{0} Device Reset", DateTime.Now);

            if (DeviceReset != null)
                DeviceReset(this, EventArgs.Empty);
        }
        private void OnDeviceLost()
        {
            if (mLine != null)
            {
                mLine.Dispose();
                mLine = null;
            }

            System.Diagnostics.Debug.Print("{0} Device Lost", DateTime.Now);

            if (DeviceLost != null)
                DeviceLost(this, EventArgs.Empty);
        }
        private void OnDeviceAboutToReset()
        {
            if (mLine != null)
            {
                mLine.Dispose();
                mLine = null;
            }


            System.Diagnostics.Debug.Print("{0} Device About to Reset", DateTime.Now);

            if (DeviceAboutToReset != null)
                DeviceAboutToReset(this, EventArgs.Empty);
        }


        #endregion
        #region --- Event Handlers ---

        void mDevice_DeviceReset(object sender, EventArgs e)
        {
            OnDeviceReset();
        }

        void mDevice_DeviceLost(object sender, EventArgs e)
        {
            OnDeviceLost();
        }


        #endregion

        #region --- Creation of objects ---

        //public override DisplayWindowImpl CreateDisplayWindow(string title, int clientWidth, int clientHeight,
        //    string iconFile, bool startFullScreen, bool allowResize)
        //{
        //    return new MDX1_DisplayWindow(title, clientWidth, clientHeight, iconFile, startFullScreen, allowResize);
        //}
        //public override DisplayWindowImpl CreateDisplayWindow(System.Windows.Forms.Control renderTarget)
        //{
        //    return new MDX1_DisplayWindow(renderTarget);
        //}
        public override DisplayWindowImpl CreateDisplayWindow(CreateWindowParams windowParams)
        {
            return new MDX1_DisplayWindow(windowParams);
        }
        public override SurfaceImpl CreateSurface(string fileName)
        {
            return new MDX1_Surface(fileName);
        }
        public override SurfaceImpl CreateSurface(Size surfaceSize)
        {
            return new MDX1_Surface(surfaceSize);
        }
        public override SurfaceImpl CreateSurface(System.IO.Stream fileStream)
        {
            return new MDX1_Surface(fileStream);   
        }

        public override FontSurfaceImpl CreateFont(string fontFamily, float sizeInPoints, FontStyle style)
        {
            BitmapFontOptions options = new BitmapFontOptions(fontFamily, sizeInPoints, style);

            return BitmapFontUtil.ConstructFromOSFont(options);
        }
        public override FontSurfaceImpl CreateFont(BitmapFontOptions bitmapOptions)
        {
            return BitmapFontUtil.ConstructFromOSFont(bitmapOptions);
        }

        #endregion

        #region --- BeginFrame stuff and DeltaTime ---

        protected override void OnBeginFrame()
        {
            mRenderTarget.BeginRender();

            SetClipRect(new Rectangle(new Point(0, 0), mRenderTarget.Size));

            mDevice.Set2DDrawState();
            
        }
        protected override void OnEndFrame()
        {
            mDevice.DrawBuffer.Flush();

            while (mClipRects.Count > 0)
                PopClipRect();


            mRenderTarget.EndRender();

        }


        #endregion

        #region --- Clip Rect stuff ---

        public override void SetClipRect(Rectangle newClipRect)
        {
            if (newClipRect.X < 0)
            {
                newClipRect.Width += newClipRect.X;
                newClipRect.X = 0;
            }
            if (newClipRect.Y < 0)
            {
                newClipRect.Height += newClipRect.Y;
                newClipRect.Y = 0;
            }
            if (newClipRect.Right > mRenderTarget.Width)
            {
                newClipRect.Width -= newClipRect.Right - mRenderTarget.Width;
            }
            if (newClipRect.Bottom > mRenderTarget.Height)
            {
                newClipRect.Height -= newClipRect.Bottom - mRenderTarget.Height;
            }

            if (mRenderTarget.Width == 0 || mRenderTarget.Height == 0)
                return;
            
            Viewport view = new Viewport();

            view.X = newClipRect.X;
            view.Y = newClipRect.Y;
            view.Width = newClipRect.Width;
            view.Height = newClipRect.Height;

            mDevice.Device.Viewport = view;
            mCurrentClipRect = newClipRect;

            SetOrthoProjection(newClipRect);
        }
        public override void PushClipRect(Rectangle newClipRect)
        {
            mClipRects.Push(mCurrentClipRect);
            SetClipRect(newClipRect);
        }
        public override void PopClipRect()
        {
            if (mClipRects.Count == 0)
            {
#if DEBUG
                throw new Exception("You have popped the cliprect too many times.");
#endif
            }
            else
            {
                SetClipRect(mClipRects.Pop());
            }
        }

        private Stack<Rectangle> mClipRects = new Stack<Rectangle>();
        private Rectangle mCurrentClipRect;

        #endregion
        #region --- Methods for drawing to the back buffer ---

        public override void Clear(Color color)
        {
            mDevice.DrawBuffer.Flush();

            mDevice.Clear(ClearFlags.Target, color.ToArgb(), 1.0f, 0);
        }
        public override void Clear(Color color, Rectangle rect)
        {
            mDevice.DrawBuffer.Flush();

            System.Drawing.Rectangle[] rects = new System.Drawing.Rectangle[1];
            rects[0] = Interop.Convert(rect);

            mDevice.Clear(ClearFlags.Target, color.ToArgb(), 1.0f, 0, rects);
        }


        public override void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            DrawLine(new Point(x1, y1), new Point(x2, y2), color);
        }
        public override void DrawLine(Point a, Point b, Color color)
        {
            mDevice.DrawBuffer.Flush();

            Vector2[] pts = new Vector2[2];

            pts[0] = new Vector2(a.X, a.Y);
            pts[1] = new Vector2(b.X, b.Y);


            mLine.Begin();
            mLine.Draw(pts, color.ToArgb());
            mLine.End();

        }
        public override void DrawLines(Point[] pt, Color color)
        {
            mDevice.DrawBuffer.Flush();

            Vector2[] pts = new Vector2[pt.Length];

            for (int i = 0; i < pt.Length; i++)
                pts[i] = new Vector2(pt[i].X, pt[i].Y);


            mLine.Begin();
            mLine.Draw(pts, color.ToArgb());
            mLine.End();

        }
        public override void DrawRect(Rectangle rect, Color color)
        {
            DrawRect(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), color);
        }
        public override void DrawRect(RectangleF rect, Color color)
        {
            mDevice.DrawBuffer.Flush();

            Vector2[] pts = new Vector2[5];

            pts[0] = new Vector2(rect.X, rect.Y);
            pts[1] = new Vector2(rect.X + rect.Width, rect.Y);
            pts[2] = new Vector2(rect.X + rect.Width, rect.Y + rect.Height);
            pts[3] = new Vector2(rect.X, rect.Y + rect.Height);
            pts[4] = pts[0];
             
            mLine.Begin();
            mLine.Draw(pts, color.ToArgb());
            mLine.End();
        }

        public override void FillRect(Rectangle rect, Color color)
        {
            FillRect(rect, new Gradient(color));
        }
        public override void FillRect(RectangleF rect, Color color)
        {
            FillRect(rect, new Gradient(color));
        }
        public override void FillRect(Rectangle rect, Gradient color)
        {
            FillRect(new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), color);
        }
        public override void FillRect(RectangleF rect, Gradient color)
        {
            // defining our screen sized quad, note the Z value of 1f to place it in the background
            mFillRectVerts[0].Position = new Microsoft.DirectX.Vector3(rect.Left, rect.Top, 0f);
            mFillRectVerts[0].Color = color.TopLeft.ToArgb();

            mFillRectVerts[1].Position = new Microsoft.DirectX.Vector3(rect.Right, rect.Top, 0f);
            mFillRectVerts[1].Color = color.TopRight.ToArgb();

            mFillRectVerts[2].Position = new Microsoft.DirectX.Vector3(rect.Left, rect.Bottom, 0f);
            mFillRectVerts[2].Color = color.BottomLeft.ToArgb();

            mFillRectVerts[3] = mFillRectVerts[1];
            //vert[3].Position = new Vector4(rect.Right, rect.Top, 0f, 1f);
            //vert[3].Color = clr;

            mFillRectVerts[4].Position = new Microsoft.DirectX.Vector3(rect.Right, rect.Bottom, 0f);
            mFillRectVerts[4].Color = color.BottomRight.ToArgb();

            mFillRectVerts[5] = mFillRectVerts[2];
            //vert[5].Position = new Vector4(rect.Left, rect.Bottom, 0f, 1f);
            //vert[5].Color = clr;

            mDevice.DrawBuffer.Flush();

            mDevice.AlphaBlend = true;

            mDevice.SetDeviceStateTexture(null);
            mDevice.AlphaArgument1 = TextureArgument.Diffuse;

            mDevice.VertexFormat = CustomVertex.PositionColored.Format;
            mDevice.Device.DrawUserPrimitives(PrimitiveType.TriangleList, 2, mFillRectVerts);

        }




        #endregion

        #region --- Registration with core library ---

        public static void Register()
        {
            Registrar.RegisterDisplayDriver(
                new DriverInfo<DisplayTypeID>(typeof(MDX1_Display), DisplayTypeID.Direct3D_MDX_1_1,
                "Managed DirectX 1.1", 200));
        }

        #endregion

        #region --- Display Mode changing stuff ---

        protected override void OnRenderTargetResize()
        {

        }
        protected override void OnRenderTargetChange(IRenderTarget oldRenderTarget)
        {
            mRenderTarget = RenderTarget.Impl as MDX1_IRenderTarget;
            mDevice.RenderTarget = mRenderTarget;
        }

        internal SwapChain CreateSwapChain(MDX1_DisplayWindow displayWindow,
            int width, int height, int bpp, bool fullScreen)
        {
            if (fullScreen == true)
            {
                PresentParameters present = CreateFullScreenPresentParameters(displayWindow, width, height, bpp);

                OnDeviceAboutToReset();

                System.Diagnostics.Debug.Print("{0} Going to full screen...", DateTime.Now);
                mDevice.Device.Reset(present);
                System.Diagnostics.Debug.Print("{0} Full screen success.", DateTime.Now);

                return mDevice.Device.GetSwapChain(0);
            }
            else
            {
                PresentParameters present = CreateWindowedPresentParameters(displayWindow, width, height);

                if (displayWindow.mSwap != null && displayWindow.IsFullScreen == true)
                {
                    // if we are in full screen mode already, we must
                    // reset the device before creating the windowed swap chain.
                    present.BackBufferHeight = 1;
                    present.BackBufferWidth = 1;
                    present.DeviceWindowHandle = displayWindow.RenderTarget.TopLevelControl.Handle;

                    OnDeviceAboutToReset();

                    System.Diagnostics.Debug.Print("{0} Going to windowed mode...", DateTime.Now);
                    mDevice.Device.Reset(present);
                    System.Diagnostics.Debug.Print("{0} Windowed mode success.", DateTime.Now);

                    
                    present = CreateWindowedPresentParameters(displayWindow, width, height);

                }

                return new Direct3D.SwapChain(mDevice.Device, present);
            }
        }

        private PresentParameters CreateFullScreenPresentParameters(MDX1_DisplayWindow displayWindow,
            int width, int height, int bpp)
        {
            PresentParameters present = new PresentParameters();

            present.AutoDepthStencilFormat = DepthFormat.Unknown;
            present.EnableAutoDepthStencil = false;
            present.DeviceWindowHandle = displayWindow.RenderTarget.Handle;
            present.BackBufferWidth = width;
            present.BackBufferHeight = height;
            present.SwapEffect = SwapEffect.Flip;
            present.Windowed = false;
            present.PresentFlag = PresentFlag.None;

            if (VSync)
                present.PresentationInterval = PresentInterval.Default;
            else
                present.PresentationInterval = PresentInterval.Immediate;

            SelectBestDisplayMode(present, bpp);

            return present;
        }

        private PresentParameters CreateWindowedPresentParameters(MDX1_DisplayWindow displayWindow,
            int width, int height)
        {
            PresentParameters present = new PresentParameters();

            present.BackBufferCount = 1;
            present.AutoDepthStencilFormat = DepthFormat.Unknown;
            present.EnableAutoDepthStencil = false;
            present.DeviceWindowHandle = displayWindow.RenderTarget.Handle;
            present.BackBufferWidth = width;
            present.BackBufferHeight = height;
            present.BackBufferFormat = Format.Unknown;
            present.SwapEffect = SwapEffect.Copy;
            present.Windowed = true;
            present.PresentFlag = PresentFlag.LockableBackBuffer;
            
            if (VSync)
                present.PresentationInterval = PresentInterval.Default;
            else
                present.PresentationInterval = PresentInterval.Immediate;

            return present;
        }
        public override ScreenMode[] EnumScreenModes()
        {
            List<ScreenMode> modes = new List<ScreenMode>();

            DisplayModeCollection dxmodes = Direct3D.Manager.Adapters[0].SupportedDisplayModes;

            foreach (DisplayMode mode in dxmodes)
            {
                int bits;

                switch (mode.Format)
                {
                    case Format.A8B8G8R8:
                    case Format.X8B8G8R8:
                    case Format.X8R8G8B8:
                    case Format.A8R8G8B8:
                        bits = 32;
                        break;

                    case Format.R8G8B8:
                        bits = 24;
                        break;

                    case Format.R5G6B5:
                    case Format.X4R4G4B4:
                    case Format.X1R5G5B5:
                        bits = 16;
                        break;

                    default:
                        continue;
                }

                modes.Add(new ScreenMode(mode.Width, mode.Height, bits));
            }

            return modes.ToArray();
        }
        private void SelectBestDisplayMode(PresentParameters present, int bpp)
        {
            DisplayModeCollection modes = Direct3D.Manager.Adapters[0].SupportedDisplayModes;

            DisplayMode selected = new DisplayMode();
            int diff = 0;

            foreach (DisplayMode mode in modes)
            {
                if (mode.Width < present.BackBufferWidth)
                    continue;

                if (mode.Height < present.BackBufferHeight)
                    continue;

                int thisDiff = Math.Abs(present.BackBufferWidth - mode.Width)
                    + Math.Abs(present.BackBufferHeight - mode.Height);

                int bits = 0;

                switch (mode.Format)
                {
                    case Format.A8B8G8R8:
                    case Format.X8B8G8R8:
                        thisDiff += 10;
                        goto case Format.X8R8G8B8;

                    case Format.X8R8G8B8:
                    case Format.A8R8G8B8:
                        bits = 32;
                        break;

                    case Format.R5G6B5:
                    case Format.X4R4G4B4:
                    case Format.X1R5G5B5:
                        bits = 16;
                        break;

                    default:
                        System.Diagnostics.Debug.Print("Unknown backbuffer format {0}.", mode.Format);
                        continue;
                }

                thisDiff += Math.Abs(bits - bpp);

                // first mode by default, or any mode which is a better match.
                if (selected.Height == 0 || thisDiff < diff)
                {
                    selected = mode;
                    diff = thisDiff;
                }

            }


            present.BackBufferFormat = selected.Format;
            present.BackBufferWidth = selected.Width;
            present.BackBufferHeight = selected.Height;
            //present.FullScreenRefreshRateInHz = selected.RefreshRate;

        }


        #endregion
 
        #region --- Drawing Helper Functions ---

        #endregion

        protected override void ProcessEvents()
        {
            System.Windows.Forms.Application.DoEvents();
        }

        internal event EventHandler VSyncChanged;
        private void OnVSyncChanged()
        {
            if (VSyncChanged != null)
                VSyncChanged(this, EventArgs.Empty);
        }
        public override bool VSync
        {
            get
            {
                return mVSync;
            }
            set
            {
                mVSync = value;

                OnVSyncChanged();
            }
        }
        public override Size MaxSurfaceSize
        {
            get { return mDevice.MaxSurfaceSize; }
        }

        internal int GetPixelPitch(Format format)
        {
            switch (format)
            {
                case Format.A8R8G8B8:
                case Format.A8B8G8R8:
                case Format.X8B8G8R8:
                    return 4;

                case Format.R8G8B8:
                    return 3;

                default:
                    throw new NotSupportedException("Format not supported.");
            }
        }
        internal PixelFormat GetPixelFormat(Format format)
        {
            switch (format)
            {
                case Format.A8R8G8B8: return PixelFormat.BGRA8888;
                case Format.A8B8G8R8: return PixelFormat.RGBA8888;
                case Format.X8B8G8R8: return PixelFormat.RGBA8888; // TODO: fix this one
                case Format.X8R8G8B8: return PixelFormat.BGRA8888; // TODO: fix this one

                default:
                    throw new NotSupportedException("Format not supported.");

            }
        }
        public override PixelFormat DefaultSurfaceFormat
        {
            get { return PixelFormat.RGBA8888; }
        }

        public override void FlushDrawBuffer()
        {
            mDevice.DrawBuffer.Flush();
        }

        public override void SetOrthoProjection(Rectangle region)
        {
            mDevice.SetOrthoProjection(region);
        }

        public override void DoLighting(LightManager lights)
        {
            FlushDrawBuffer();
            mDevice.DoLighting(lights);
        }

        protected override void SavePixelBuffer(PixelBuffer pixelBuffer, string filename, ImageFileFormat format)
        {
            FormUtil.SavePixelBuffer(pixelBuffer, filename, format);
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
                Caps c = Direct3D.Manager.GetDeviceCaps(0, DeviceType.Hardware);

                return c.MaxActiveLights;
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
            get { return true;}
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

        #region IPlatformServices Members

        protected override AgateLib.PlatformSpecific.IPlatformServices GetPlatformServices()
        {
            return this;
        }
        AgateLib.Utility.PlatformType AgateLib.PlatformSpecific.IPlatformServices.PlatformType
        {
            get { return AgateLib.Utility.PlatformType.Windows; }
        }

        #endregion
    }
}