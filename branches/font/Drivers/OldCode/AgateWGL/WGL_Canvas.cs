/*
 * C# code is based off C++ code taken from Paul's projects:
 * http://www.paulsprojects.net/opengl/rtotex/rtotex.html
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Tao.OpenGl;
using Tao.Platform.Windows;

namespace ERY.GameLibrary
{

    /*    
    public class WGL_Canvas : CanvasImpl
    {
        WGL_Surface mOwner;
        Size mSize;

        IntPtr hDC;
        IntPtr hRC;

        //HPBUFFERARB hBuffer;
        IntPtr hBuffer;

        public WGL_Canvas(WGL_Surface owner, Size size)
        {
            mOwner = owner;
            mSize = size;


            Initialize();

        }


        void Initialize()
            //(	int newWidth, int newHeight,
            //            int newColorBits, int newDepthBits, int newStencilBits,
            //            int numExtraIAttribs, int * extraIAttribList, int * flags)
            
        {
            int colorBits = 32;
            int numExtraIAttribs = 0;
            
            //Check for pbuffer support
            // I don't know how to convert this yet.  These are marked as extern bool
            // in Paul's code, which is probably not supported by Tao.Platform.Windows.Wgl
            // (or maybe by C# at all?)
           
            //if(	!WGL_ARB_extensions_string_supported ||
            //    !WGL_ARB_pixel_format_supported ||
            //    !WGL_ARB_pbuffer_supported)
            //{
            //    throw new Exception("Extension required for pbuffer unsupported");
            //    return false;
            //}


            //Get the current device context
            IntPtr hCurrentDC = Wgl.wglGetCurrentDC();
            if (hCurrentDC == null)
            {
                throw new Exception("Unable to get current Device Context");
            }


            //choose pixel format
            int pixelFormat;

            //const int standardIAttribList[]={	WGL_DRAW_TO_PBUFFER_ARB, 1,
            //                                    WGL_COLOR_BITS_ARB, colorBits,
            //                                    WGL_ALPHA_BITS_ARB, colorBits==32 ? 8 : 0,
            //                                    WGL_DEPTH_BITS_ARB, depthBits,
            //                                    WGL_STENCIL_BITS_ARB, stencilBits};


            int[] standardIAttribList = new int[] {
            Wgl.WGL_DRAW_TO_PBUFFER_ARB, 1,
            Wgl.WGL_COLOR_BITS_ARB, colorBits,
                                            Wgl.WGL_ALPHA_BITS_ARB, colorBits==32 ? 8 : 0,
                                            Wgl.WGL_DEPTH_BITS_ARB, 0,
                                            Wgl.WGL_STENCIL_BITS_ARB, 0 };

            float[] fAttribList = new float[] { 0 };

            //add the extraIAttribList to the standardIAttribList
            int[] iAttribList = new int[standardIAttribList.Length + numExtraIAttribs * 2 + 1];

            //memcpy(iAttribList, standardIAttribList, sizeof(standardIAttribList));
            //memcpy(iAttribList + sizeof(standardIAttribList) / sizeof(int),
            //        extraIAttribList, numExtraIAttribs * 2 * sizeof(int) + sizeof(int));

            standardIAttribList.CopyTo(iAttribList, 0);
            //extraIAttribList.CopyTo(iAttribList, standardIAttribList.Length);

            //Choose pixel format
            uint numFormats;

            if (!Wgl.wglChoosePixelFormatARB(hCurrentDC, ref iAttribList, ref fAttribList, 1,
                                             pixelFormat, out numFormats))
            {
                throw new Exception("Unable to find a pixel format for the pbuffer");
            }

            //Create the pbuffer
            hBuffer = Wgl.wglCreatePbufferARB(hCurrentDC, pixelFormat, mSize.Width, mSize.Height, flags);
            if (hBuffer == null)
            {
                throw new Exception("Unable to create pbuffer");
            }

            //Get the pbuffer's device context
            hDC = Wgl.wglGetPbufferDCARB(hBuffer);
            if (hDC == null)
            {
                throw new Exception("Unable to get pbuffer's device context");
            }

            //Create a rendering context for the pbuffer
            hRC = Wgl.wglCreateContext(hDC);
            if (!hRC)
            {
                throw new Exception("Unable to create pbuffer's rendering context");
            }

            //Set and output the actual pBuffer dimensions
            Wgl.wglQueryPbufferARB(hBuffer, WGL_PBUFFER_WIDTH_ARB, &width);
            Wgl.wglQueryPbufferARB(hBuffer, WGL_PBUFFER_HEIGHT_ARB, &height);
            

            // Success!
        }

        void Shutdown()
        {
            if (hRC)													//have a rendering context?
            {
                if (!Wgl.wglDeleteContext(hRC))							//try to delete RC
                {
                    throw new Exception("Release of Pbuffer Rendering Context Failed.");
                }

                hRC = null;											//set RC to null
            }

            if (hDC && !Wgl.wglReleasePbufferDCARB(hBuffer, hDC))		//Are we able to release DC?
            {
                throw new Exception("Release of Pbuffer Device Context Failed.");
                hDC = null;
            }

            if (!Wgl.wglDestroyPbufferARB(hBuffer))
            {
                throw new Exception("Unable to destroy pbuffer");
            }
        }


        bool MakeCurrent()
        {
            if (!Wgl.wglMakeCurrent(hDC, hRC))
            {
                throw new Exception("Unable to change current context");
                return false;
            }

            return true;
        }


        public override System.Drawing.Size Size
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override void Clear(System.Drawing.Color color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Clear(System.Drawing.Color color, System.Drawing.Rectangle dest)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DrawEllipse(System.Drawing.Rectangle target, System.Drawing.Color color, float thickness)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DrawRect(System.Drawing.Rectangle target, System.Drawing.Color color, float thickness)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void FillRect(System.Drawing.Rectangle rect, System.Drawing.Color color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void FillEllipse(System.Drawing.Rectangle rect, System.Drawing.Color color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
    */
}