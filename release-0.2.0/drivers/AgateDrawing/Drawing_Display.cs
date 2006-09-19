//     ``The contents of this file are subject to the Mozilla Public License
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib.SystemDrawing
{
    class Drawing_Display : DisplayImpl
    {
        #region --- Private variables ---

        private Graphics mGraphics;
        private Drawing_IRenderTarget mRenderTarget;

        private bool mInFrame = false;

        #endregion 

        #region --- Events and Event Handlers ---

        protected override void OnRenderTargetChange(IRenderTarget oldRenderTarget)
        {
            if (mInFrame)
                throw new InvalidOperationException(
                    "Cannot change the current render target inside BeginFrame..EndFrame block!");

            System.Diagnostics.Debug.Assert(mGraphics == null);

            mRenderTarget = RenderTarget.Impl as Drawing_IRenderTarget;

            OnRenderTargetResize();
        }

        protected override void OnRenderTargetResize()
        {
        }

        #endregion
        #region --- Public Overridden properties ---


        #endregion 
        #region --- Public Properties ---

        public Graphics FrameGraphics
        {
            get { return mGraphics; }
        }

        #endregion

        #region --- Creation / Destruction ---

        public override void Initialize()
        {
        }
        public override void Dispose() 
        {
        }

        public override SurfaceImpl CreateSurface(String fileName)
        {
            return new Drawing_Surface(fileName);

        }
        public override SurfaceImpl CreateSurface( Size surfaceSize)
        {
            return new Drawing_Surface( surfaceSize);   
        }
        public override DisplayWindowImpl CreateDisplayWindow(string title, int clientWidth, int clientHeight, string iconFile, bool startFullScreen, bool allowResize)
        {
            return new Drawing_DisplayWindow(title, clientWidth, clientHeight, iconFile, startFullScreen, allowResize );
        }
        public override DisplayWindowImpl CreateDisplayWindow(System.Windows.Forms.Control renderTarget)
        {
            return new Drawing_DisplayWindow(renderTarget);
        }
        public override FontSurfaceImpl CreateFont(  string fontFamily, float sizeInPoints)
        {
            return new Drawing_FontSurface( fontFamily, sizeInPoints);
        }

        #endregion 
        #region --- Direct modification of the back buffer ---

        public override void Clear(Color color)
        {
            CheckInFrame("Clear");

            mGraphics.Clear((System.Drawing.Color)color);
        }
        public override void Clear(Color color, Rectangle dest_rect)
        {
            CheckInFrame("Clear");

            mGraphics.FillRectangle(new SolidBrush((System.Drawing.Color)color), (System.Drawing.Rectangle)dest_rect);
        }

        public override void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            CheckInFrame("DrawLine");

            mGraphics.DrawLine(new Pen((System.Drawing.Color)color), x1, y1, x2, y2);
        }
        public override void DrawLine(Point a, Point b, Color color)
        {
            CheckInFrame("DrawLine");

            mGraphics.DrawLine(new Pen((System.Drawing.Color)color), (System.Drawing.Point)a, (System.Drawing.Point)b);
        }

        public override void DrawRect(Rectangle rect, Color color)
        {
            CheckInFrame("DrawRect");

            mGraphics.DrawRectangle(new Pen((System.Drawing.Color)color), (System.Drawing.Rectangle)rect);
        }
        public override void FillRect(Rectangle rect, Color color)
        {
            CheckInFrame("FillRect");

            mGraphics.FillRectangle(new SolidBrush((System.Drawing.Color)color), (System.Drawing.Rectangle)rect);
        }

        #endregion
        #region --- Begin/End Frame and DeltaTime ---

        protected override void OnBeginFrame()
        {
            mGraphics = Graphics.FromImage(mRenderTarget.BackBuffer);
        }
        protected override void OnEndFrame(bool waitVSync)
        {
            mGraphics.Dispose();
            mGraphics = null;

            while (mClipRects.Count > 0)
                PopClipRect();

            Drawing_IRenderTarget renderTarget = RenderTarget.Impl as Drawing_IRenderTarget;
            renderTarget.EndRender(waitVSync);

        }
        #endregion
        #region --- Clip Rect Stuff ---

        public override void SetClipRect(Rectangle newClipRect)
        {
            mGraphics.SetClip((System.Drawing.Rectangle)newClipRect);
            mCurrentClipRect = newClipRect;
        }
        public override void PushClipRect(Rectangle newClipRect)
        {
            mClipRects.Push(mCurrentClipRect);
            SetClipRect(newClipRect);
        }
        public override void PopClipRect()
        {
#if DEBUG
            if (mClipRects.Count == 0)
                throw new Exception("You have popped the cliprect too many times.");
#endif
            
            SetClipRect(mClipRects.Pop());
        }

        private Stack<Rectangle> mClipRects = new Stack<Rectangle>();
        private Rectangle mCurrentClipRect;

        #endregion 

    
    
        public static void Register()
        {
            Registrar.RegisterDisplayDriver(
                new DisplayDriverInfo(typeof(Drawing_Display), DisplayTypeID.Reference,
                "System.Drawing", 0));
        }
    
        public override Size MaxSurfaceSize
        {
            get { return new Size(1024, 1024); }
        }
    }

    
}
