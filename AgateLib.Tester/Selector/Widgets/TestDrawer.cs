using AgateLib.UserInterface;
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
            return new Window(new WindowProps
            {
                Children = Props.Tests.Select(CreateButton).ToList()
            });

            //return new FlexBox(new FlexBoxProps
            //{
            //    StyleId = "test-drawer",
            //    Children = categories
            //              .Select(x => CreateMenu(x.Value))
            //              .ToArray()
            //});
        }

        private Window CreateMenu(ITest[] value)
        {
            return new Window(new WindowProps
            {
                Children = value.Select(CreateButton).ToList(),
            });
        }

        private IRenderable CreateButton(ITest test)
        {
            return new Button(new ButtonProps
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
