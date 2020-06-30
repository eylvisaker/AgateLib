using AgateLib.UserInterface;

namespace AgateLib.Demo.UserInterface.MultipleWorkspaces
{
    public class FirstWorkspaceApp : Widget<FirstWorkspaceAppProps>
    {
        public FirstWorkspaceApp(FirstWorkspaceAppProps props) : base(props)
        {
        }

        public override IRenderable Render() =>
            new App(new AppProps
            {
                DefaultStyle = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        JustifyContent = JustifyContent.Start,
                        AlignItems = AlignItems.Start,
                        Direction = FlexDirection.Row,
                    },
                },
                Children =
                {
                    new Window(new WindowProps
                    {
                        Children =
                        {
                            new Button(new ButtonProps
                            {
                                Text = "Pancakes",
                                OnFocus = e => Props.Status(e, "Order some pancakes."),
                            }),
                            new Button(new ButtonProps
                            {
                                Text = "Waffles",
                                OnFocus = e => Props.Status(e, "Eat some waffles.")
                            })
                        },
                        OnCancel = e => e.System.PopWorkspace(),
                    })
                }
            });
    }

    public class FirstWorkspaceAppProps : WidgetProps
    {
        public UserInterfaceEventHandler2<string> Status { get; internal set; }
    }
}
