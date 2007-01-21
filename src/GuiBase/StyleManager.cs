using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.GuiBase
{
    public abstract class StyleManager
    {
        public abstract void ConnectStyle(string componentType, Component target);
    }
}
