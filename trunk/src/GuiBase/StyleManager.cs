using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.Gui
{
    public abstract class StyleManager
    {
        public abstract void ConnectStyle(Type componentType, Component target);
    }
}
