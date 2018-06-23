using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AgateLib.UserInterface.Layout
{
    public class AnchoredLayoutTests : LayoutTests
    {
        AnchoredLayout layout;

        public AnchoredLayoutTests()
        {
            layout = new AnchoredLayout();
        }

        protected override IWidgetLayout WidgetLayout => layout;
    }
}
