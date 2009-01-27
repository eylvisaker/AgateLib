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

    }
}