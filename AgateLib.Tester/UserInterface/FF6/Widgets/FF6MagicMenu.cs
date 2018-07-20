using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.FF6.Widgets
{
    public class FF6MagicMenu : Widget<FF6MagicMenuProps>
    {
        public FF6MagicMenu(FF6MagicMenuProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new Menu(new MenuProps
            {
                Name = "Magic",
                OnCancel = Props.OnCancel,
                MenuItems = { new MenuItem(new MenuItemProps { Text = "Does nothing" })}
            });
        }
    }

    public class FF6MagicMenuProps : WidgetProps
    {
        public UserInterfaceEventHandler OnCancel { get; set; }
    }
}
