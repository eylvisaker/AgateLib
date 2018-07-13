﻿using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.FF6.Widgets
{
    public class PartyStatusWindow : Widget<PartyStatusWindowProps, WidgetState>
    {
        public PartyStatusWindow(PartyStatusWindowProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new Menu(new MenuProps
            {
                Enabled = Props.Enabled,
                MenuItems = Props.Model.Party.Characters.Select(
                    x => new MenuItem(new MenuItemProps { Text = x.Name })).ToList(),
            });
        }
    }

    public class PartyStatusWindowProps : WidgetProps
    {
        public FF6Model Model { get; set; }

        public bool Enabled { get; set; } = true;
    }
}
