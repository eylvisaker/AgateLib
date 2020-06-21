using AgateLib.UserInterface;

namespace AgateLib.Tests.UserInterface.FF6.Widgets
{
    public class FF6EspersMenu : Widget<FF6EspersMenuProps>
    {
        public FF6EspersMenu(FF6EspersMenuProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new Window(new WindowProps
            {
                Name = "Espers",
                OnCancel = Props.OnCancel,
                Children = {
                    new Button(new ButtonProps { Text = "Does nothing" })
                }
            });
        }
    }

    public class FF6EspersMenuProps : WidgetProps
    {
        public UserInterfaceEventHandler OnCancel { get; set; }
    }
}
