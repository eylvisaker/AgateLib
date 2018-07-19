using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.FF6.Widgets
{
    public class FF6MainMenu : Widget<FF6MainMenuProps, WidgetState>
    {
        public FF6MainMenu(FF6MainMenuProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new FlexBox(new FlexBoxProps
            {
                Children =
                {
                    new PartyStatusWindow(new PartyStatusWindowProps
                    {
                        Model = Props.Model,
                        Name = "status",
                        Enabled = false
                    }),
                    new Menu(new MenuProps
                    {
                        AllowNavigate = false,
                        Name = "main",
                        MenuItems =
                        {
                            new MenuItem(new MenuItemProps { Text = "Items", OnAccept = Props.Items  }),
                            new MenuItem(new MenuItemProps { Text = "Skills", OnAccept = Props.Skills }),
                            new MenuItem(new MenuItemProps { Text = "Equip", OnAccept = Props.Equip  }),
                            new MenuItem(new MenuItemProps { Text = "Relic", OnAccept = Props.Relic  }),
                            new MenuItem(new MenuItemProps { Text = "Status", OnAccept = Props.Status }),
                            new MenuItem(new MenuItemProps { Text = "Config", OnAccept = Props.Config }),
                            new MenuItem(new MenuItemProps { Text = "Save", OnAccept = Props.Save   }),
                        }
                    }),
                },
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.Row,
                    }
                },
                StyleId = Props.Name,
            });
        }
    }

    public class FF6MainMenuProps : WidgetProps
    {
        public FF6Model Model { get; set; }
        public UserInterfaceEventHandler Items { get; set; }
        public UserInterfaceEventHandler Skills { get; set; }
        public UserInterfaceEventHandler Equip { get; set; }
        public UserInterfaceEventHandler Relic { get; set; }
        public UserInterfaceEventHandler Status { get; set; }
        public UserInterfaceEventHandler Config { get; set; }
        public UserInterfaceEventHandler Save { get; set; }
    }
}
