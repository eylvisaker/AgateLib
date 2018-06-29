using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManualTests.AgateLib.Selector.Widgets
{
    public class TestDrawer : Widget<TestDrawerProps, TestDrawerState>
    {
        private Dictionary<string, ITest[]> categories;

        public TestDrawer(TestDrawerProps props) : base(props)
        {
            categories = props.Tests.GroupBy(x => x.Category).ToDictionary(x => x.Key, x => x.ToArray());
        }

        public override IRenderElement Render()
        {
            return new FlexBox(new FlexContainerProps
            {
                StyleId = "test-drawer",
                Children = categories
                          .Select(x => CreateMenu(x.Value))
                          .Select(x => x.Render())
                          .ToArray()
            });
        }

        private Menu CreateMenu(ITest[] value)
        {
            return new Menu(new MenuProps
            {
                Children = value.Select(x => new MenuItem(new MenuItemProps
                {
                    Text = x.Name,
                    OnAccept = () => Props.OnAcceptTest(x),
                })).ToList(),
            });
        }

    }

    public class TestDrawerProps : WidgetProps
    {
        public ITest[] Tests { get; set; }

        public Action<ITest> OnAcceptTest { get; set; }
    }

    public class TestDrawerState : WidgetState
    {
    }
}
