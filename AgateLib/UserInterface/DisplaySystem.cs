using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface
{
    public interface IDisplaySystem
    {
        IRenderElement Focus { get; set; }

        IFontProvider Fonts { get; }

        IInstructions Instructions { get; }

    }
}
