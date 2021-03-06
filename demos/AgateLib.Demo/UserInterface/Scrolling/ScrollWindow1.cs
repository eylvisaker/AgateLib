﻿using AgateLib.UserInterface;
using System.Collections.Generic;

namespace AgateLib.Demo.UserInterface.Scrolling
{
    public class ScrollWindow1 : Widget<ScrollWindow1Props>
    {
        public ScrollWindow1(ScrollWindow1Props props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            List<IRenderable> buttons = new List<IRenderable>
            {
                new Label(new LabelProps { Text = "There should be 50 items in\nthe list below this label." })
            };

            for (int i = 0; i < 50; i++)
            {
                buttons.Add(new Button(new ButtonProps { Text = $"Item {i + 1}" }));
            }

            return new Window(new WindowProps
            {
                OnCancel = Props.OnCancel,
                Children = buttons,
            });
        }
    }

    public class ScrollWindow1Props : WidgetProps
    {
        public UserInterfaceEventHandler OnCancel { get; set; }
    }
}
