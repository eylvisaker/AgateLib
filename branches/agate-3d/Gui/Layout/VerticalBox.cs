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
    }
}