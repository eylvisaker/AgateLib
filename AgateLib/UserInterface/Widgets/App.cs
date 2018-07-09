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
                    AlignItems = AlignItems.Start,
                    JustifyContent = JustifyContent.SpaceEvenly,
                },
                Padding = new LayoutBox(100, 50, 100, 50),
            },
            StyleTypeId = "workspace",
            StyleId = Props.StyleId,
            StyleClass = Props.StyleClass,
            Style = Props.Style,
            Cancel = Props.Cancel,
            Children = Props.Children,
        });
    }

    public class AppProps : WidgetProps
    {
        public InlineElementStyle DefaultStyle { get; set; }

        public IList<IRenderable> Children { get; set; } = new List<IRenderable>();

        public Action Cancel { get; set; }
    }
}
