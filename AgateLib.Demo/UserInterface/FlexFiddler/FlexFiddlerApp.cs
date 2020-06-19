using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface;

namespace AgateLib.Tests.UserInterface.FlexFiddler
{
    public class FlexFiddlerApp : Widget<FlexFiddlerAppProps, FlexFiddlerAppState>
    {
        public FlexFiddlerApp(FlexFiddlerAppProps props) : base(props)
        {
            SetState(new FlexFiddlerAppState());
        }

        public override IRenderable Render()
        {
            return new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.Row,
                        AlignItems = AlignItems.Stretch,
                    }
                },
                Children =
                {
                    new Window(new WindowProps
                    {
                        Style = new InlineElementStyle
                        {
                            FlexItem = new FlexItemStyle
                            {
                                Grow = State.FirstWindowGrow,
                            }
                        },
                        Name = "FirstWindow",
                        Children =
                        {
                            new Label(new LabelProps { Text = $"Window Flex Grow: {State.FirstWindowGrow}"}),
                            new Panel(new PanelProps
                            {
                                OnCancel = Props.OnCancel,
                                Name = "growWindow",
                                Style = new InlineElementStyle
                                {
                                    Padding = new LayoutBox(24, 12, 24, 12),
                                },
                                Children = {
                                    new Button(new ButtonProps {
                                        Text = "Increase Grow",
                                        OnAccept = e =>
                                        {
                                            SetState(state => state.FirstWindowGrow ++);
                                        },
                                    }),
                                    new Button(new ButtonProps {
                                        Text = "Decrease Grow",
                                        OnAccept = e =>
                                        {
                                            SetState(state => state.FirstWindowGrow --);
                                        },
                                        Enabled = State.FirstWindowGrow > 0
                                    }),
                                },
                            }),
                        }
                    }),
                    new Window(new WindowProps
                    {
                        Style = new InlineElementStyle
                        {
                            FlexItem = new FlexItemStyle
                            {
                                Grow = State.SecondWindowGrow,
                            }
                        },
                        Name = "SecondWindow",
                        Children =
                        {
                            new Label(new LabelProps { Text = $"Window Flex Grow: {State.SecondWindowGrow}"}),
                            new Panel(new PanelProps
                            {
                                Name = "growWindow",
                                Style = new InlineElementStyle
                                {
                                    Padding = new LayoutBox(12, 6, 12, 6),
                                },
                                Children =
                                {
                                    new Button(new ButtonProps {
                                        Text = "Increase Grow",
                                        OnAccept = e =>
                                        {
                                            SetState(state => state.SecondWindowGrow ++);
                                        },
                                    }),
                                    new Button(new ButtonProps {
                                        Text = "Decrease Grow",
                                        OnAccept = e =>
                                        {
                                            SetState(state => state.SecondWindowGrow --);
                                        },
                                        Enabled = State.SecondWindowGrow > 0
                                    }),
                                },
                            }),
                        }
                    }),
                }
            });
        }
    }

    public class FlexFiddlerAppState
    {
        public FlexFiddlerAppState()
        {
        }

        public int FirstWindowGrow { get; set; }
        public int SecondWindowGrow { get; set; }
    }

    public class FlexFiddlerAppProps : WidgetProps
    {
        public UserInterfaceEventHandler OnCancel { get; set; }
    }
}