using AgateLib.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.FF6.Widgets
{
    public class FF6ItemTarget : Widget<FF6ItemTargetProps>
    {
        CharacterEvent characterEvent = new CharacterEvent();

        public FF6ItemTarget(FF6ItemTargetProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new App(new AppProps
            {
                OnCancel = Props.OnCancel,
                Children =
                {
                    new Window(new WindowProps
                    {
                        Children = Props.Characters.Select(c =>
                            new Button(new ButtonProps
                            {
                                Text = c.Name,
                                OnAccept = e => Props.OnAccept?.Invoke(characterEvent.Reset(e, c))
                            })).ToList<IRenderable>()
                    })
                }
            });
        }
    }

    public class FF6ItemTargetProps : WidgetProps
    {
        public IList<PlayerCharacter> Characters { get; set; }
        public UserInterfaceEventHandler OnCancel { get; set; }
        public UserInterfaceEventHandler<PlayerCharacter> OnAccept { get; set; }
    }

    public class CharacterEvent : UserInterfaceEvent<PlayerCharacter> { }
}
