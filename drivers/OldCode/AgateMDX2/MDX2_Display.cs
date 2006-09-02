using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Direct3D = Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using CustomVertex = Microsoft.DirectX.Direct3D.CustomVertex;
using Generic = Microsoft.DirectX.Generic;

namespace ERY.GameLibrary
{
    public class MDX2_Display : DisplayImpl
    {
        #region --- Private Variables ---

        private string mPath = ".";
        private Device mDevice;

        private MDX2_DisplayWindow mD3DWindow;

        private bool mInitialized = false;

        // variables for drawing primitives
        Direct3D.Line mLine;

        #endregion

        #region --- Overriden Public Properties ---


        public override String ImagePath
        {
            get { return mPath; }
            set { mPath = value; }
        }


        #endregion
        #region --- Implementation Specific Public Properties ---

        public Device D3D_Device
        {
            get { return mDevice; }
        }

        #endregion
        
        #region --- Events and Event Handlers ---


        #endregion

        #region --- Creation of objects ---

        public override DisplayWindowImpl CreateDisplayWindow(string title, int clientWidth, int clientHeight, bool startFullScreen, bool allowResize)
        {
            return new MDX2_DisplayWindow(title, clientWidth, clientHeight, startFullScreen, allowResize);
        }
        public override DisplayWindowImpl CreateDisplayWindow(System.Windows.Forms.Control renderTarget)
        {
            return new MDX2_DisplayWindow(renderTarget);
        }
        public override SurfaceImpl CreateSurface(Surface owner, string fileName)
        {
            return new MDX2_Surface(owner, fileName);
        }
        public override SurfaceImpl CreateSurface(Surface owner, Size surfaceSize)
        {
            return new MDX2_Surface(owner, surfaceSize);
        }

        public override FontSurfaceImpl CreateFont(FontSurface owner, System.Drawing.Font font)
        {
            return new MDX2_FontSurface(owner, font);
        }

        #endregion  
        #region --- Creation / Destruction ---

        public override void Initialize()
        {
            Sprite.UseSpriteCache = false;
        }

        
        internal void Initialize(MDX2_DisplayWindow window)
        {
            if (mInitialized)
                return;

            mInitialized = true;

            // ok, create D3D device
            PresentParameters present = CreatePresentParameters(window);
            
            present.BackBufferWidth = 1;
            present.BackBufferHeight = 1;
            
            DeviceType dtype = DeviceType.Hardware;
            
            mDevice = new Device
                (0, dtype, window.RenderTarget.Handle,
                 CreateFlags.SoftwareVertexProcessing, present);

            // create primitive objects
            mLine = new Direct3D.Line(mDevice);

            
        }
        public override void Dispose()
        {
            mDevice.Dispose();
        }

        #endregion

        #region --- BeginFrame stuff and DeltaTime ---

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


            SetClipRect(new Rectangle(new Point(0, 0), mD3DWindow.Size));

            mDevice.SetRenderTarget(0, mD3DWindow.mBackBuffer);
            mDevice.BeginScene();

            mLine.Begin();
        }
        public override void EndFrame(bool waitVSync)
        {
            while (mClipRects.Count > 0)
                PopClipRect();

            mLine.End();

            mDevice.EndScene();
            
            /*
             * if (mRenderTarget != null) 
                mRenderChain.Present();
            else
                mDevice.Present();
            */
            try
            {
                /*
                if (mRenderTarget != null)
                    mDevice.Present(new Rectangle(new Point(0, 0), mRenderTarget.Size),
                        new Rectangle(new Point(0, 0), mRenderTarget.Size), mRenderTarget.Handle);
                else
                 * */
                
                //mDevice.Present();
                mD3DWindow.mSwap.Present();

            }
            catch
            {
            }
            
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
            if (newClipRect.Right > mD3DWindow.Width)
            {
                newClipRect.Width -= newClipRect.Right - mD3DWindow.Width;
            }
            if (newClipRect.Bottom > mD3DWindow.Height)
            {
                newClipRect.Height -= newClipRect.Bottom - mD3DWindow.Height;
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
            mDevice.Clear(ClearFlags.Target, Color.Black, 1.0f, 0);
        }
        public override void Clear(Color color)
        {
            mDevice.Clear(ClearFlags.Target, color, 1.0f, 0);
        }
        public override void Clear(Color color, Rectangle rect)
        {
            Rectangle[] rects = new Rectangle[1];
            rects[0] = rect;

            mDevice.Clear(ClearFlags.Target, color, 1.0f, 0, rects);
        }


        public override void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            DrawLine(new Point(x1, y1), new Point(x2, y2), color);
        }
        public override void DrawLine(Point a, Point b, Color color)
        {
            /*
            Graphics g = mD3DWindow.mBackBuffer.GetGraphics();

            g.DrawLine(new Pen(color), a, b);

            mD3DWindow.mBackBuffer.ReleaseGraphics();  
           */

            Vector2[] pts = new Vector2[5];

            pts[0] = new Vector2(a.X, a.Y);
            pts[1] = new Vector2(b.X, b.Y);

            mLine.Draw(pts, color);
        }
        public override void DrawRect(Rectangle rect, Color color)
        {
            
            Graphics g = mD3DWindow.mBackBuffer.GetGraphics();

            g.DrawRectangle(new Pen(color), rect);

            mD3DWindow.mBackBuffer.ReleaseGraphics();  
             
            /*
            Vector2[] pts = new Vector2[5];

            pts[0] = new Vector2(rect.X, rect.Y);
            pts[1] = new Vector2(rect.X + rect.Width, rect.Y);
            pts[2] = new Vector2(rect.X + rect.Width, rect.Y + rect.Height);
            pts[3] = new Vector2(rect.X, rect.Y + rect.Height);
            pts[4] = pts[0];
            
            mLine.Draw(pts, color);
            */
        }
        public override void FillRect(Rectangle rect, Color color)
        {
            
            Graphics g = mD3DWindow.mBackBuffer.GetGraphics();

            g.FillRectangle(new SolidBrush(color), rect);

            mD3DWindow.mBackBuffer.ReleaseGraphics();
            /*
            CustomVertex.TransformedColored[] backgroundVertices = new CustomVertex.TransformedColored[6];		
		    //short[] backgroundIndices;

            // defining our screen sized quad, note the Z value of 1f to place it in the background
            backgroundVertices[0].Position = new Vector4(rect.Left, rect.Top, 1f, 1f);
            backgroundVertices[0].Color = color;

            backgroundVertices[1].Position = new Vector4(rect.Right, rect.Top , 1f, 1f);
            backgroundVertices[1].Color = color;

            backgroundVertices[2].Position = new Vector4(rect.Left , rect.Bottom, 1f, 1f);
            backgroundVertices[2].Color = color;

            backgroundVertices[3].Position = new Vector4(rect.Right, rect.Top, 1f, 1f);
            backgroundVertices[3].Color = color;

            backgroundVertices[4].Position = new Vector4(rect.Right, rect.Bottom, 1f, 1f);
            backgroundVertices[4].Color = color;

            backgroundVertices[5].Position = new Vector4(rect.Left, rect.Bottom, 1f, 1f);
            backgroundVertices[5].Color = color;

            //backgroundIndices = new short[] { 0, 1, 2, 1, 3, 2 };

            //GraphicsBuffer buff = new GraphicsBuffer(6);
            Generic.GraphicsBuffer<CustomVertex.TransformedColored> gbuff = new Microsoft.DirectX.Generic.GraphicsBuffer<Microsoft.DirectX.Direct3D.CustomVertex.TransformedColored>(6);
            gbuff.Write(backgroundVertices);

            // render our gradient background quad
            mDevice.VertexFormat = CustomVertex.TransformedColored.Format;
            //mDevice.DrawIndexedUserPrimitives(PrimitiveType.TriangleList, 0, 6, 2, backgroundIndices, true, backgroundVertices);
            mDevice.DrawUserPrimitives(PrimitiveType.TriangleList, 2, gbuff);
              */ 
        }

        
        //Public Sub DrawCircle(ByVal NumPoints As Integer, ByVal CenterX As Single, ByVal CenterY As Single, ByVal Radius As Single, ByVal Color As Integer, ByVal Thickness As Single)
        //Dim pts(NumPoints) As Vector2, pointStep As Single

        //pointStep = (Math.PI * 2) / NumPoints

        //For i As Integer = 0 To NumPoints
        //pts(i).X = CenterX + Math.Cos(pointStep * i) * Radius
        //pts(i).Y = CenterY + Math.Sin(pointStep * i) * Radius
        //Next

        //Try
        //mLine.Width = Thickness
        //mLine.Begin()
        //mLine.Draw(pts, Color)
        //mLine.End()
        //Catch Err As Exception

        //End Try
        //End Sub


        #endregion

        public static void Register()
        {
            Registrar.RegisterDisplayDriver(
                new DisplayDriverInfo(typeof(MDX2_Display), DisplayTypeID.Direct3D_MDX_2_0_Beta,
                "MDX 2.0 beta", 100));
        }
        
        protected override void OnCurrentWindowChange()
        {
            mD3DWindow = Display.CurrentWindow.Impl as MDX2_DisplayWindow ;
        }

        internal void CreateSwapChain(MDX2_DisplayWindow displayWindow, bool fullScreen)
        {
            PresentParameters present = CreatePresentParameters(displayWindow);

            present.IsWindowed = !fullScreen;

            if (displayWindow.mSwap != null)
            {
                displayWindow.mSwap.Dispose();
            }

            displayWindow.mSwap = new Direct3D.SwapChain(mDevice, present);
            displayWindow.mBackBuffer = displayWindow.mSwap.GetBackBuffer(0);
        }

        private static PresentParameters CreatePresentParameters(MDX2_DisplayWindow displayWindow)
        {
            PresentParameters present = new PresentParameters();

            present.BackBufferCount = 1;
            present.AutoDepthStencilFormat = DepthFormat.D16;
            present.EnableAutoDepthStencil = false;
            present.DeviceWindowHandle = displayWindow.RenderTarget.Handle;
            present.BackBufferWidth = displayWindow.Width;
            present.BackBufferHeight = displayWindow.Height;
            present.SwapEffect = SwapEffect.Copy;
            present.IsWindowed = true;

            return present;
        }
    }

}