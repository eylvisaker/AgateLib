using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.Selector.Widgets
{
    public class TestSelectorApp : Widget<TestSelectorProps, TestSelectorState>
    {
        private Label choose;
        private TestDrawer carousel;

        public TestSelectorApp(TestSelectorProps props) : base(props)
        {
            choose = new Label(new LabelProps { Text = "Choose a test" });
            carousel = new TestDrawer(new TestDrawerProps
            {
                Tests = props.Tests,
                OnAcceptTest = props.OnAcceptTest,
            });
        }

        public override IRenderable Render()
        {
            return new App(new AppProps
            {
                Children = {
                    new Window(new WindowProps
                    {
                        Name = "test-selector-app",
                        Children = { choose, carousel }
                    })
                }
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
