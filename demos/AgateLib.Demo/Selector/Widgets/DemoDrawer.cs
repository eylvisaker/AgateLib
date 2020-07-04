using AgateLib.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.Demo.Selector.Widgets
{
    public class DemoDrawer : Widget<DemoDrawerProps>
    {
        private Dictionary<string, IDemo[]> categories;

        public DemoDrawer(DemoDrawerProps props) : base(props)
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

        private Window CreateMenu(IDemo[] value)
        {
            return new Window(new WindowProps
            {
                Children = value.Select(CreateButton).ToList(),
            });
        }

        private IRenderable CreateButton(IDemo test)
        {
            return new Button(new ButtonProps
            {
                Text = test.Name,
                OnAccept = e => Props.OnAcceptTest?.Invoke(test),
            });
        }
    }

    public class DemoDrawerProps : WidgetProps
    {
        public List<IDemo> Tests { get; set; }

        public Action<IDemo> OnAcceptTest { get; set; }
    }
}
