using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.Gui.Layout
{
    public class Grid : ILayoutPerformer
    {

        public bool DoingLayout
        {
            get { return false;  }
        }

        public void DoLayout(Container container)
        {
            return;
        }


        public Size RecalcMinSize(Container container)
        {
            return container.Size;
        }


        public bool AcceptInputKey(AgateLib.InputLib.KeyCode keyCode)
        {
            return false;
        }


        public Widget CanMoveFocus(Container container, Widget currentFocus, Direction direction)
        {
            return null;
        }

    }
}
