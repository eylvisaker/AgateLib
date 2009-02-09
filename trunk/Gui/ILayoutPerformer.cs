using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.Gui
{
    public interface ILayoutPerformer
    {
        void DoLayout(Container container);

        Size RecalcMinSize(Container container);

        bool AcceptInputKey(AgateLib.InputLib.KeyCode keyCode);

        Widget CanMoveFocus(Container container, Widget currentFocus, Direction direction);
    }
}
