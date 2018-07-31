using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Styling
{
    public class FlexItemStyle
    {
        public static bool Equals(FlexItemStyle a, FlexItemStyle b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;

            if (a.Grow != b.Grow) return false;

            return true;
        }

        public int Grow { get; set; }
    }
}
