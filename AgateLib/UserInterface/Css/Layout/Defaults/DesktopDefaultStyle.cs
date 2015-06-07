using AgateLib.UserInterface.Css.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Layout.Defaults
{
    public class DesktopDefaultStyle : BlankDefaultStyle
    {
        public override void SetDefaultStyle(CssStyle style)
        {
            style.Data.Overflow = CssOverflow.Disallow;
        }
    }
}
