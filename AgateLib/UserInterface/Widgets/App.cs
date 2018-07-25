﻿using System;
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
            Name = Props.Name,
            StyleClass = Props.StyleClass,
            Style = Props.Style,
            OnCancel = Props.OnCancel,
            Children = Props.Children,
        });
    }

    public class AppProps : WidgetProps
    {
        public InlineElementStyle DefaultStyle { get; set; }

        public IList<IRenderable> Children { get; set; } = new List<IRenderable>();

        public UserInterfaceEventHandler OnCancel { get; set; }
    }
}
