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
            DefaultStyle = new InlineElementStyle
            {
                Flex = new FlexStyle
                {
                    Direction = FlexDirection.Row,
                    AlignItems = AlignItems.Start,
                    JustifyContent = JustifyContent.SpaceEvenly,
                }
            },
            StyleTypeId = "workspace",
            StyleId = Props.StyleId,
            StyleClass = Props.StyleClass,
            Style = Props.Style,
            Children = Props.Children,
        });
    }

    public class AppProps : WidgetProps
    {
        public IList<IRenderable> Children { get; set; } = new List<IRenderable>();
    }
}
