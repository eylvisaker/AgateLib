using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Direct3D = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using CustomVertex = Microsoft.DirectX.Direct3D.CustomVertex;
using ERY.AgateGL.ImplBase;

namespace ERY.AgateGL.MDXold
{
    public class MDX1_Display_old : DisplayImpl
    {
        #region --- Private Variables ---

        private Device mDevice;

        private MDX1_IRenderTarget_old mRenderTarget;
        private Texture mLastTexture;
        private frmFullScreen mFullScreenWindow;

        private bool mInitialized = false;

        // variables for drawing primitives
        Direct3D.Line mLine;

        //Vector2[] mDrawLinePts = new Vector2[4];
        CustomVertex.TransformedColored[] mFillRectVerts = new CustomVertex.TransformedColored[6];
            
        VertexBuffer mSurfaceVB;
        const int NumVertices = 1000;
        int mSurfaceVBPointer = 0;

        readonly int SurfaceVBSize = NumVertices * CustomVertex.TransformedColoredTextured.StrideSize;

        #endregion

        #region --- Overriden Public Properties ---



        #endregion
        #region --- Implementation Specific Public Properties ---

        public Device D3D_Device
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
            mLine = new Line(mDevice);
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

        public override DisplayWindowImpl CreateDisplayWindow(string title, int clientWidth, int clientHeight, bool startFullScreen, bool allowResize)
        {
            return new MDX1_DisplayWindow_old(title, clientWidth, clientHeight, startFullScreen, allowResize);
        }
        public override DisplayWindowImpl CreateDisplayWindow(System.Windows.Forms.Control renderTarget)
        {
            return new MDX1_DisplayWindow_old(renderTarget);
        }
        public override SurfaceImpl CreateSurface(string fileName)
        {
            return new MDX1_Surface_old(fileName);
        }
        public override SurfaceImpl CreateSurface(ERY.AgateGL.Size surfaceSize)
        {
            return new MDX1_Surface_old(surfaceSize);
        }

        public override FontSurfaceImpl CreateFont(  string fontFamily, float sizeInPoints)
        {
            return new MDX1_FontSurface_old( fontFamily, sizeInPoints);
        }

        #endregion  
        #region --- Creation / Destruction ---

        public override void Initialize()
        {
            Sprite.UseSpriteCache = true;
        }

        
        internal void Initialize(MDX1_DisplayWindow_old window)
        {
            if (mInitialized)
                return;

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

            mDevice = new Device
                (0, dtype, window.RenderTarget.TopLevelControl.Handle,
                 flags, present);

            mDevice.DeviceLost += new EventHandler(mDevice_DeviceLost);
            mDevice.DeviceReset += new EventHandler(mDevice_DeviceReset);
            // create primitive objects
            mLine = new Direct3D.Line(mDevice);

            CreateSurfaceVB();

        }

        private void CreateSurfaceVB()
        {
            //mSurfaceVB = new VertexBuffer(mDevice, SurfaceVBSize,
             //    Usage.WriteOnly | Usage.Dynamic, CustomVertex.TransformedColoredTextured.Format,
             //     Pool.Default);
        }

        public override void Dispose()
        {
            if (mSurfaceVB != null)
            {
                mSurfaceVB.Dispose();
                mSurfaceVB = null;
            }

            if (mLine != null)
            {
                mLine.Dispose();
                mLine = null;
            }

            mDevice.Dispose();
        }

        #endregion

        #region --- BeginFrame stuff and DeltaTime ---

        protected override void OnBeginFrame()
        {
            mRenderTarget.BeginRender();

            SetClipRect(new Rectangle(new Point(0, 0), mRenderTarget.Size));

        }

        protected override void OnEndFrame(bool waitVSync)
        {
            while (mClipRects.Count > 0)
                PopClipRect();

            
            mRenderTarget.EndRender(waitVSync);

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

            Viewport view = new Viewport();

            view.X = newClipRect.X;
            view.Y = newClipRect.Y;
            view.Width = newClipRect.Width;
            view.Height = newClipRect.Height;

            mDevice.Viewport = view;
            mCurrentClipRect = newClipRect;

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
        #region --- Methods for direct modificiation of the back buffer ---

        public override void Clear()
        {
            mDevice.Clear(ClearFlags.Target, Color.Black.ToArgb(), 1.0f, 0);
        }
        public override void Clear(Color color)
        {
            mDevice.Clear(ClearFlags.Target, color.ToArgb(), 1.0f, 0);
        }
        public override void Clear(Color color, Rectangle rect)
        {
            System.Drawing.Rectangle[] rects = new System.Drawing.Rectangle[1];
            rects[0] = (System.Drawing.Rectangle)rect;

            mDevice.Clear(ClearFlags.Target, color.ToArgb(), 1.0f, 0, rects);
        }


        public override void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            DrawLine(new Point(x1, y1), new Point(x2, y2), color);
        }
        public override void DrawLine(Point a, Point b, Color color)
        {
            Vector2[] pts = new Vector2[2];

            pts[0] = new Vector2(a.X, a.Y);
            pts[1] = new Vector2(b.X, b.Y);

            mLine.Begin();
            mLine.Draw(pts, color.ToArgb());
            mLine.End();

        }
        public override void DrawLines(Point[] pt, Color color)
        {
            Vector2[] pts = new Vector2[pt.Length];

            for (int i = 0; i < pt.Length; i++)
                pts[i] = new Vector2(pt[i].X, pt[i].Y);

            mLine.Begin();
            mLine.Draw(pts, color.ToArgb());
            mLine.End();

        }
        public override void DrawRect(Rectangle rect, Color color)
        {            
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
            int clr = color.ToArgb();

            // defining our screen sized quad, note the Z value of 1f to place it in the background
            mFillRectVerts[0].Position = new Vector4(rect.Left, rect.Top, 0f, 1f);
            mFillRectVerts[0].Color = clr;

            mFillRectVerts[1].Position = new Vector4(rect.Right, rect.Top , 0f, 1f);
            mFillRectVerts[1].Color = clr;

            mFillRectVerts[2].Position = new Vector4(rect.Left , rect.Bottom, 0f, 1f);
            mFillRectVerts[2].Color = clr;

            mFillRectVerts[3] = mFillRectVerts[1];
            //vert[3].Position = new Vector4(rect.Right, rect.Top, 0f, 1f);
            //vert[3].Color = clr;

            mFillRectVerts[4].Position = new Vector4(rect.Right, rect.Bottom, 0f, 1f);
            mFillRectVerts[4].Color = clr;

            mFillRectVerts[5] = mFillRectVerts[2];
            //vert[5].Position = new Vector4(rect.Left, rect.Bottom, 0f, 1f);
            //vert[5].Color = clr;

            //backgroundIndices = new short[] { 0, 1, 2, 1, 3, 2 };

            //GraphicsBuffer buff = new GraphicsBuffer(6);
            //Generic.GraphicsBuffer<CustomVertex.TransformedColored> gbuff = new Microsoft.DirectX.Generic.GraphicsBuffer<Microsoft.DirectX.Direct3D.CustomVertex.TransformedColored>(6);
            //GraphicsStream gbuff = new GraphicsStream(backgroundVertices, 
            //    backgroundVertices.Length * CustomVertex.TransformedColored.StrideSize, ;
            //gbuff.Write(backgroundVertices);

            // render our gradient background quad
            //mDevice.SetStreamSource(0, null, 0);
            
            mLastTexture = null;

            mDevice.RenderState.AlphaBlendEnable = true;
            mDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            mDevice.RenderState.DestinationBlend = Blend.InvSourceAlpha;

            mDevice.SetTexture(0, null);

            mDevice.TextureState[0].AlphaArgument1 = TextureArgument.Diffuse;
            
            mDevice.VertexFormat = CustomVertex.TransformedColored.Format;
            mDevice.DrawUserPrimitives(PrimitiveType.TriangleList, 2, mFillRectVerts);
              
        }

        


        #endregion

        #region --- Registration with core library ---

        public static void Register()
        {
            Registrar.RegisterDisplayDriver(
                new DisplayDriverInfo(typeof(MDX1_Display_old), DisplayTypeID.Direct3D_MDX_1_1 + 20,
                "MDX 1.1", 5));
        }

        #endregion

        protected override void OnRenderTargetResize()
        {
            
        }
        protected override void OnRenderTargetChange(IRenderTarget oldRenderTarget)
        {
            mRenderTarget = RenderTarget.Impl as MDX1_IRenderTarget_old;
           // mRenderSurface = mRenderTarget.RenderSurface;
        }

        internal void CreateSwapChain(MDX1_DisplayWindow_old displayWindow, 
            int width, int height, int bpp, bool fullScreen)
        {
            bool wasFullScreen = displayWindow.IsFullScreen;

            if (fullScreen == true)
            {
                PresentParameters present = CreateFullScreenPresentParameters(displayWindow, width, height, bpp);

                OnDeviceAboutToReset();

                displayWindow.ReplaceForm(mFullScreenWindow, mFullScreenWindow);

                mFullScreenWindow.Activate();

                System.Diagnostics.Debug.Print("{0} Going to full screen...", DateTime.Now);
                mDevice.Reset(present);
                System.Diagnostics.Debug.Print("{0} Full screen success.", DateTime.Now);
                
                
                displayWindow.mSwap = mDevice.GetSwapChain(0);
                displayWindow.mBackBuffer = displayWindow.mSwap.GetBackBuffer(0, BackBufferType.Mono);

            }
            else
            {
                PresentParameters present = CreateWindowedPresentParameters(displayWindow, width, height);

                if (displayWindow.mSwap != null && displayWindow.IsFullScreen == false)
                {
                    displayWindow.mSwap.Dispose();
                }
                else if (wasFullScreen)
                {
                    // if we are in full screen mode already, we must
                    // reset the device before creating the windowed swap chain.

                    displayWindow.CreateWindowedDisplay();

                    present.BackBufferHeight = 1;
                    present.BackBufferWidth = 1;
                    present.DeviceWindowHandle = displayWindow.RenderTarget.TopLevelControl.Handle;

                    OnDeviceAboutToReset();

                    System.Diagnostics.Debug.Print("{0} Going to windowed mode...", DateTime.Now);
                    mDevice.Reset(present);
                    System.Diagnostics.Debug.Print("{0} Windowed mode success.", DateTime.Now);

                    //displayWindow.RenderTarget.TopLevelControl.Visible = true;

                    DisposeFullScreenWindow();


                    
                    present = CreateWindowedPresentParameters(displayWindow, width, height);

                    /*
                    // do this to force Windows.Forms to update the unmanaged
                    // Win32 resource for the window.  Otherwise there is a mismatch
                    // between the stored window position and the actual on-screen position.
                    displayWindow.RenderTarget.TopLevelControl.Left++;
                    displayWindow.RenderTarget.TopLevelControl.Left--;
                    displayWindow.RenderTarget.TopLevelControl.Width++;
                    displayWindow.RenderTarget.TopLevelControl.Width--;
                    */
                }

                displayWindow.mSwap = new Direct3D.SwapChain(mDevice, present);
                displayWindow.mBackBuffer = displayWindow.mSwap.GetBackBuffer(0, BackBufferType.Mono);

                if (width > 0 && height > 0)
                    displayWindow.Size = new Size(width, height);
            }

            
        }

        private PresentParameters CreateFullScreenPresentParameters(MDX1_DisplayWindow_old displayWindow,
            int width, int height, int bpp)
        {
            mFullScreenWindow = new frmFullScreen();
            mFullScreenWindow.Text = displayWindow.Title;

            mFullScreenWindow.KeyUp += new System.Windows.Forms.KeyEventHandler(mFullScreenWindow_KeyUp);
            mFullScreenWindow.KeyDown += new System.Windows.Forms.KeyEventHandler(mFullScreenWindow_KeyDown);
            mFullScreenWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(mFullScreenWindow_MouseUp);
            mFullScreenWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(mFullScreenWindow_MouseDown);
            mFullScreenWindow.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(mFullScreenWindow_MouseDoubleClick);
            
            PresentParameters present = new PresentParameters();

            present.BackBufferCount = 1;
            present.AutoDepthStencilFormat = DepthFormat.Unknown;
            present.EnableAutoDepthStencil = false;
            present.DeviceWindow = mFullScreenWindow;
            present.BackBufferWidth = width;
            present.BackBufferHeight = height;
            present.SwapEffect = SwapEffect.Discard;
            present.Windowed = false;
            present.PresentFlag = PresentFlag.LockableBackBuffer;

            SelectBestDisplayMode(present);

            return present;
        }


        void mFullScreenWindow_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Keyboard.Keys.SetWinFormsKey(e.KeyCode, false);
        }
        void mFullScreenWindow_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Keyboard.Keys.SetWinFormsKey(e.KeyCode, true);
        }

        void mFullScreenWindow_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Mouse.MouseButtons btn = MDX1_DisplayWindow_old.GetButtons(e.Button);

            Mouse.OnMouseDoubleClick(btn);
        }
        void mFullScreenWindow_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Mouse.MouseButtons btn = MDX1_DisplayWindow_old.GetButtons(e.Button);

            Mouse.Buttons[btn] = false;
        }
        void mFullScreenWindow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Mouse.MouseButtons btn = MDX1_DisplayWindow_old.GetButtons(e.Button);

            Mouse.Buttons[btn] = true;
        }
        void mFullScreenWindow_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Mouse.OnMouseMove();
        }

        private static PresentParameters CreateWindowedPresentParameters(MDX1_DisplayWindow_old displayWindow, 
            int width, int height)
        {
            PresentParameters present = new PresentParameters();

            present.BackBufferCount = 1;
            present.AutoDepthStencilFormat = DepthFormat.Unknown;
            present.EnableAutoDepthStencil = false;
            present.DeviceWindow = displayWindow.RenderTarget;
            present.BackBufferWidth = width;
            present.BackBufferHeight = height;
            present.BackBufferFormat = Format.Unknown;
            present.SwapEffect = SwapEffect.Copy;
            present.Windowed = true;
            present.PresentFlag = PresentFlag.LockableBackBuffer;
            present.PresentationInterval = PresentInterval.Immediate;

            return present;
        }
        private void SelectBestDisplayMode(PresentParameters present)
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
                        bits = 24;
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

        internal void DisposeFullScreenWindow()
        {
            if (mFullScreenWindow != null)
            {
                mFullScreenWindow.Dispose();
                mFullScreenWindow = null;
            }
        }

        #region --- Drawing Helper Functions ---

        internal void SetDeviceStateTexture(Texture texture, bool alphaBlend)
        {
            mDevice.RenderState.AlphaBlendEnable = alphaBlend;
            mDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            mDevice.RenderState.DestinationBlend = Blend.InvSourceAlpha;

            if (texture == mLastTexture)
                return;

            mDevice.SetTexture(0, texture);

            mDevice.SamplerState[0].AddressU = TextureAddress.Clamp;
            mDevice.SamplerState[0].AddressV = TextureAddress.Clamp;

            mDevice.TextureState[0].AlphaArgument1 = TextureArgument.TextureColor;
            mDevice.TextureState[0].AlphaArgument2 = TextureArgument.Diffuse;
            mDevice.TextureState[0].AlphaOperation = TextureOperation.Modulate;

            mLastTexture = texture;
        }

        internal void WriteToSurfaceVBAndRender
            (PrimitiveType primitiveType, int primCount, CustomVertex.TransformedColoredTextured[] verts)
        {
            GraphicsStream stm;

            if (mSurfaceVBPointer + verts.Length < NumVertices)
            {
                stm = mSurfaceVB.Lock(mSurfaceVBPointer, 
                    CustomVertex.TransformedColoredTextured.StrideSize * verts.Length , 
                    LockFlags.NoOverwrite);

            }
            else
            {
                mSurfaceVBPointer = 0;

                stm = mSurfaceVB.Lock(mSurfaceVBPointer,
                    CustomVertex.TransformedColoredTextured.StrideSize * verts.Length,
                    LockFlags.Discard);
            }

            stm.Write(verts);
            
            mSurfaceVB.Unlock();

            mDevice.SetStreamSource(0, mSurfaceVB, 0);
            mDevice.VertexFormat = CustomVertex.TransformedColoredTextured.Format;
            mDevice.DrawPrimitives(primitiveType, mSurfaceVBPointer, primCount);

            mSurfaceVBPointer += verts.Length;
        }

        #endregion



        public override Size MaxSurfaceSize
        {
            get
            {
                Size retval = new Size(mDevice.DeviceCaps.MaxTextureWidth, mDevice.DeviceCaps.MaxTextureHeight);

                if (retval.Width > 512)
                    retval.Width = 512;
                if (retval.Height > 512)
                    retval.Height = 512;

                return retval;
            }
        }
    }

}