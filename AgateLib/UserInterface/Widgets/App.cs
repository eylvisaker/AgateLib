using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.UserInterface.Styling;

namespace AgateLib.UserInterface.Widgets
{
    public class App : Widget<AppProps>
    {
        public App(AppProps props) : base(props)
        {
        }

        public override IRenderable Render() => new FlexBox(new FlexBoxProps
        {
            DefaultStyle = Props.DefaultStyle ?? new InlineElementStyle
            {
                Flex = new FlexStyle
                {
                    Direction = FlexDirection.Row,
                    AlignItems = AlignItems.Center,
                    JustifyContent = JustifyContent.SpaceEvenly,
                },
                Padding = new LayoutBox(100, 50, 100, 50),
            },
            StyleTypeId = "workspace",
            Name = Props.Name,
            Theme = Props.Theme,
            StyleClass = Props.StyleClass,
            Style = Props.Style,
            OnCancel = Props.OnCancel,
            Children = Props.Children,
        });
    }

    public class AppProps : WidgetProps
    {
        public IList<IRenderable> Children { get; set; } = new List<IRenderable>();

        public UserInterfaceEventHandler OnCancel { get; set; }
    }
}
