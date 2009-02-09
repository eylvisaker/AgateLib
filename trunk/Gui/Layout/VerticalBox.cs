using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.Gui.Layout
{
    public class VerticalBox : BoxLayoutBase
    {
        protected override void DoLayoutInternal()
        {
            DoBoxLayout(false);
        }

        protected override Size RecalcMinSizeInternal()
        {
            return RecalcMinSizeBox(false);
        }

        public override bool AcceptInputKey(AgateLib.InputLib.KeyCode keyCode)
        {
            switch (keyCode)
            {
                case AgateLib.InputLib.KeyCode.Up:
                case AgateLib.InputLib.KeyCode.Down:
                    return true;

                default:
                    return false;
            }
        }

        public override Widget CanMoveFocus(Container container, Widget currentFocus, Direction direction)
        {
            if (direction == Direction.Right || direction == Direction.Left)
                return null;

            GuiRoot root = Root(container);
            int index = GetParentIndex(container, root.FocusControl);

            switch (direction)
            {
                case Direction.Up:
                    return GetNextChild(container, index, -1);

                case Direction.Down:
                    return GetNextChild(container, index, 1);

            }

            throw new InvalidOperationException();
        }
    }
}