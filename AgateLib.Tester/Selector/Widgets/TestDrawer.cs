using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.Selector.Widgets
{
    public class TestDrawer : Widget<TestDrawerProps, TestDrawerState>
    {
        private Dictionary<string, ITest[]> categories;

        public TestDrawer(TestDrawerProps props) : base(props)
        {
            categories = props.Tests.GroupBy(x => x.Category).ToDictionary(x => x.Key, x => x.ToArray());
        }

        public override IRenderable Render()
        {
            return new Menu(new MenuProps
            {
                MenuItems = Props.Tests.Select(CreateMenuItem).ToList()
            });

            //return new FlexBox(new FlexBoxProps
            //{
            //    StyleId = "test-drawer",
            //    Children = categories
            //              .Select(x => CreateMenu(x.Value))
            //              .ToArray()
            //});
        }

        private Menu CreateMenu(ITest[] value)
        {
            return new Menu(new MenuProps
            {
                MenuItems = value.Select(CreateMenuItem).ToList(),
            });
        }

        private MenuItem CreateMenuItem(ITest test)
        {
            return new MenuItem(new MenuItemProps
            {
                Text = test.Name,
                OnAccept = e => Props.OnAcceptTest?.Invoke(test),
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
