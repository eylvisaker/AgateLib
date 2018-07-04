using AgateLib.UserInterface.Widgets;
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
            return new FlexBox(new FlexBoxProps
            {
                Children = Props.Model.Party.Characters.Select(x => new Label(new LabelProps { Text = x.Name })).ToList<IRenderable>(),
            });
        }
    }

    public class PartyStatusWindowProps : WidgetProps
    {
        public FF6Model Model { get; internal set; }
    }
}
