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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using ERY.AgateLib.Drivers;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib.SystemDrawing
{
    public class Drawing_Display : DisplayImpl, IDisplayCaps 
    {
        #region --- Private variables ---
            
        private Graphics mGraphics;
        private Drawing_IRenderTarget mRenderTarget;

        private bool mInFrame = false;

        private Stack<Geometry.Rectangle> mClipRects = new Stack<Geometry.Rectangle>();
        private Geometry.Rectangle mCurrentClipRect;

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
            Report("System.Drawing driver instantiated for display.");
        }
        public override void Dispose() 
        {
        }

        public override SurfaceImpl CreateSurface(String fileName)
        {
            return new Drawing_Surface(fileName);

        }
        public override SurfaceImpl CreateSurface(Geometry.Size surfaceSize)
        {
            return new Drawing_Surface(surfaceSize);
        }
        public override DisplayWindowImpl CreateDisplayWindow(string title, int clientWidth, int clientHeight, string iconFile, bool startFullScreen, bool allowResize)
        {
            return new Drawing_DisplayWindow(title, clientWidth, clientHeight, iconFile, startFullScreen, allowResize );
        }
        public override DisplayWindowImpl CreateDisplayWindow(System.Windows.Forms.Control renderTarget)
        {
            return new Drawing_DisplayWindow(renderTarget);
        }
        public override FontSurfaceImpl CreateFont(string fontFamily, float sizeInPoints, FontStyle style)
        {
            return new Drawing_FontSurface(fontFamily, sizeInPoints, style);
        }

        #endregion 
        #region --- Direct modification of the back buffer ---

        public override void Clear(Geometry.Color color)
        {
            CheckInFrame("Clear");

            mGraphics.Clear((Color)color);
        }
        public override void Clear(Geometry.Color color, Geometry.Rectangle dest_rect)
        {
            CheckInFrame("Clear");

            mGraphics.FillRectangle(
                new SolidBrush((Color)color), (Rectangle)dest_rect);
        }

        public override void DrawLine(int x1, int y1, int x2, int y2, Geometry.Color color)
        {
            CheckInFrame("DrawLine");

            mGraphics.DrawLine(new Pen((Color)color), x1, y1, x2, y2);
        }
        public override void DrawLine(Geometry.Point a, Geometry.Point b, Geometry.Color color)
        {
            CheckInFrame("DrawLine");

            mGraphics.DrawLine(new Pen((Color)color), (Point)a, (Point)b);
        }

        public override void DrawRect(Geometry.Rectangle rect, Geometry.Color color)
        {
            CheckInFrame("DrawRect");

            mGraphics.DrawRectangle(new Pen((Color)color), (Rectangle)rect);
        }
        public override void FillRect(Geometry.Rectangle rect, Geometry.Color color)
        {
            CheckInFrame("FillRect");

            mGraphics.FillRectangle(new SolidBrush((Color)color), (Rectangle)rect);
        }

        #endregion
        #region --- Begin/End Frame and DeltaTime ---

        protected override void OnBeginFrame()
        {
            mGraphics = Graphics.FromImage(mRenderTarget.BackBuffer);
        }
        protected override void OnEndFrame()
        {
            mGraphics.Dispose();
            mGraphics = null;

            while (mClipRects.Count > 0)
                PopClipRect();

            Drawing_IRenderTarget renderTarget = RenderTarget.Impl as Drawing_IRenderTarget;
            renderTarget.EndRender();

        }
        #endregion
        #region --- Clip Rect Stuff ---

        public override void SetClipRect(Geometry.Rectangle newClipRect)
        {
            mGraphics.SetClip((Rectangle)newClipRect);
            mCurrentClipRect = newClipRect;
        }
        public override void PushClipRect(Geometry.Rectangle newClipRect)
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

        
        #endregion 

    
        public override PixelFormat DefaultSurfaceFormat
        {
            get { return PixelFormat.BGRA8888; }
        }
    
        public static void Register()
        {
            Registrar.RegisterDisplayDriver(
                new DriverInfo<DisplayTypeID>(typeof(Drawing_Display), DisplayTypeID.Reference,
                "System.Drawing", 0));
        }

        public override Geometry.Size MaxSurfaceSize
        {
            get { return new Geometry.Size(1024, 1024); }
        }

        public override void FlushDrawBuffer()
        {
        }

        public override void SetOrthoProjection(ERY.AgateLib.Geometry.Rectangle region)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override void DoLighting(LightManager lights)
        {
            throw new InvalidOperationException("Lighting is not supported.");
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
            get { return false; }
        }

        int IDisplayCaps.MaxLights
        {
            get { return 0; }
        }

        bool IDisplayCaps.IsHardwareAccelerated
        {
            get { return false; }
        }

        bool IDisplayCaps.Supports3D
        {
            get { return false; }
        }
        bool IDisplayCaps.SupportsFullScreen
        {
            get { return false; }
        }
        bool IDisplayCaps.SupportsFullScreenModeSwitching
        {
            get { return false; }
        }
        #endregion
    }

    
}