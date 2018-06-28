using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManualTests.AgateLib.Selector.Widgets
{
    public class TestSelectorApp : Widget<TestSelectorProps, TestSelectorState>
    {
        private NewLabel choose;
        private TestDrawer carousel;

        public TestSelectorApp(TestSelectorProps props) : base(props)
        {
            choose = new NewLabel(new LabelProps { Text = "Choose a test" });
            carousel = new TestDrawer(new TestDrawerProps { Tests = props.Tests });
        }

        public override IRenderElement Render()
        {
            return new FlexContainer(new FlexContainerProps
            {
                Children = { choose, carousel }
            });
        }
    }

    public class TestSelectorState : WidgetState
    {
    }

    public class TestSelectorProps : WidgetProps
    {
        public ITest[] Tests { get; set; }

        public Action<ITest> OnAcceptTest { get; set; }
    }
}
