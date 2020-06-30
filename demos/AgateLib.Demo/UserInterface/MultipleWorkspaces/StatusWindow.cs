using AgateLib.UserInterface;

namespace AgateLib.Demo.UserInterface.MultipleWorkspaces
{
    public class StatusWindow : Widget<StatusWindowProps>
    {
        public StatusWindow(StatusWindowProps props) : base(props)
        {
        }

        public override IRenderable Render() =>
            new Window(new WindowProps
            {
                Children =
                {
                    new Label(new LabelProps
                    {
                        Text = Props.Text,
                        Ref = Props.StatusLabelRef,
                    }),
                }
            });
    }

    public class StatusWindowProps : WidgetProps
    {
        public string Text { get; set; }

        public ElementReference StatusLabelRef { get; set; }
    }
}
