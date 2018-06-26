using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public class MenuItem : Widget<MenuItemProps, MenuItemState>
    {
        public MenuItem(MenuItemProps props) : base(props)
        {
        }

        public override IRenderElement Render()
        {
            return new LabelElement(new LabelElementProps(Props.Text));
        }
    }

    public class MenuItemProps : WidgetProps
    {
        public string Text { get; set; }

        public Action OnAccept { get; set; }
    }

    public class MenuItemState : WidgetState
    {
    }
}
