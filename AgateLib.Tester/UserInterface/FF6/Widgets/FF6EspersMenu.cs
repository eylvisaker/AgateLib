using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.FF6.Widgets
{
    public class FF6EspersMenu : Widget<FF6EspersMenuProps>
    {
        public FF6EspersMenu(FF6EspersMenuProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new Menu(new MenuProps
            {
                Name = "Espers",
                OnCancel = Props.OnCancel,
                MenuItems = { new MenuItem(new MenuItemProps { Text = "Does nothing" }) }
            });
        }
    }

    public class FF6EspersMenuProps : WidgetProps
    {
        public UserInterfaceEventHandler OnCancel { get; set; }
    }
}
