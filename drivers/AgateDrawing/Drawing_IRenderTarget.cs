using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib.SystemDrawing
{
    public interface Drawing_IRenderTarget : IRenderTargetImpl
    {
        Bitmap BackBuffer { get; }
    }
}
