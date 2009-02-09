using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.Gui.Layout
{
    public class HorizontalBox : BoxLayoutBase
    {
        protected override void DoLayoutInternal()
        {
            DoBoxLayout(true);
        }

        protected override Size RecalcMinSizeInternal()
        {
            return RecalcMinSizeBox(true);
        }

        public override bool AcceptInputKey(AgateLib.InputLib.KeyCode keyCode)
        {
            switch (keyCode)
            {
                case AgateLib.InputLib.KeyCode.Right:
                case AgateLib.InputLib.KeyCode.Left:
                    return true;

                default:
                    return false;
            }
        }

        public override Widget CanMoveFocus(Container container, Widget currentFocus, Direction direction)
        {
            if (direction == Direction.Up || direction == Direction.Down)
                return null;

            GuiRoot root = Root(container);
            int index = GetParentIndex(container, currentFocus);


            switch (direction)
            {
                case Direction.Left:
                    return GetNextChild(container, index, -1);

                case Direction.Right:
                    return GetNextChild(container, index, 1);
            }

            throw new InvalidOperationException();
        }


    }
}