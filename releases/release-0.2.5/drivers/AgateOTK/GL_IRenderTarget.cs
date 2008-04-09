using System;
using System.Collections.Generic;
using System.Text;

using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib.OpenGL
{
    interface GL_IRenderTarget : IRenderTargetImpl
    {
        void MakeCurrent();
    }
}
