using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using ERY.AgateLib;
using ERY.AgateLib.Geometry;

using OpenTK.OpenGL;

namespace ERY.AgateLib.OpenGL
{
    class GLState
    {
        #region --- Private variables for state management ---

        private GLDrawBuffer mDrawBuffer;

        #endregion

        public GLState()
        {
             mDrawBuffer = new GLDrawBuffer(this);
        }


        public GLDrawBuffer DrawBuffer
        {
            get { return mDrawBuffer; }
        }

        public void SetGLColor(Color color)
        {
            GL.Color4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
        }


    }
}
