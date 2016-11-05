using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.Css.Layout.Defaults
{
    public class DesktopDefaultStyle : BlankDefaultStyle
    {
        public override void SetDefaultStyle(CssStyle style)
        {
            style.Data.Overflow = Overflow.Disallow;
        }
    }
}
