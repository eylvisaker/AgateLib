using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.FF6.Widgets
{
    public class FF6SkillsMenu : Widget<FF6SkillsMenuProps>
    {
        public FF6SkillsMenu(FF6SkillsMenuProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new Menu(new MenuProps
            {
                Name = "skillType",
                MenuItems =
                {
                    new MenuItem(new MenuItemProps{ Text = "Magic",  }),
                    new MenuItem(new MenuItemProps{ Text = "Espers", }),
                    new MenuItem(new MenuItemProps{ Text = "Blitz",  }),
                    new MenuItem(new MenuItemProps{ Text = "SwdTech", }),
                    new MenuItem(new MenuItemProps{ Text = "Blue",  }),
                    new MenuItem(new MenuItemProps{ Text = "Rage",  }),
                },
                OnCancel = Props.OnCancel
            });

            //var layout = new FixedGridLayout(1, 1);
            //workspace.Layout = layout;

            //var menu = new Menu("SkillType");

            //menu.Add("Magic", StartMagicMenu);
            //menu.Add("Espers", StartEspersMenu);
            //menu.Add("Blitz", () => RecordEvent("Blitz"));
            //menu.Add("SwdTech", () => RecordEvent("SwdTech"));
            //menu.Add("Blue", () => RecordEvent("Blue"));
            //menu.Add("Rage", () => RecordEvent("Rage"));

            //menu.Exit += ReturnToDesktop;

            //layout.Add(menu, Point.Zero);
        }
    }

    public class FF6SkillsMenuProps : WidgetProps
    {
        public UserInterfaceEventHandler OnCancel { get; set; }
    }
}
