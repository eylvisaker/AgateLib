using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface;
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
            return new Window(new WindowProps
            {
                DefaultStyle = new InlineElementStyle
                {
                    FlexItem = new FlexItemStyle { Grow = 1 },
                },
                Name = Props.Name,
                Enabled = Props.Enabled,
                Children = Props.Characters.Select(
                    x => new Button(new ButtonProps
                    {
                        Style = new InlineElementStyle
                        {
                            FlexItem = new FlexItemStyle { Grow = 1 },
                        },
                        Text = x.Name,
                        OnAccept = e => Props.OnSelectPC?.Invoke(
                            new UserInterfaceEvent<PlayerCharacter>().Reset(e, x))
                    })).ToList<IRenderable>(),
                OnCancel = Props.OnCancel
            });
        }
    }

    public class PartyStatusWindowProps : WidgetProps
    {
        internal UserInterfaceEventHandler<PlayerCharacter> OnSelectPC;

        public List<PlayerCharacter> Characters { get; set; } = new List<PlayerCharacter>();

        public bool Enabled { get; set; } = true;

        public UserInterfaceEventHandler OnCancel { get; set; }
    }
}
