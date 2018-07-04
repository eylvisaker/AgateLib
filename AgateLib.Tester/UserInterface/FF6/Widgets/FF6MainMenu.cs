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
                    new Menu(new MenuProps
                    {
                        MenuItems =
                        {
                            new MenuItem(new MenuItemProps { Text = "Items ", OnAccept = Props.Items  }),
                            new MenuItem(new MenuItemProps { Text = "Skills", OnAccept = Props.Skills }),
                            new MenuItem(new MenuItemProps { Text = "Equip ", OnAccept = Props.Equip  }),
                            new MenuItem(new MenuItemProps { Text = "Relic ", OnAccept = Props.Relic  }),
                            new MenuItem(new MenuItemProps { Text = "Status", OnAccept = Props.Status }),
                            new MenuItem(new MenuItemProps { Text = "Config", OnAccept = Props.Config }),
                            new MenuItem(new MenuItemProps { Text = "Save  ", OnAccept = Props.Save   }),
                        }
                    }),
                    new PartyStatusWindow(new PartyStatusWindowProps{ Model = Props.Model })
                },
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {

                    }
                }
            });
        }
    }

    public class FF6MainMenuProps : WidgetProps
    {
        public FF6Model Model { get; internal set; }
        public Action Items { get; internal set; }
        public Action Skills { get; internal set; }
        public Action Equip { get; internal set; }
        public Action Relic { get; internal set; }
        public Action Status { get; internal set; }
        public Action Config { get; internal set; }
        public Action Save { get; internal set; }
    }
}
