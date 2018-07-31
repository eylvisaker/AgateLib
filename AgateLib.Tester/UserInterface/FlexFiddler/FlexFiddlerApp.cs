using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Widgets;

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
                            new Menu(new MenuProps
                            {
                                OnCancel = Props.OnCancel,
                                Name = "growWindow",
                                Style = new InlineElementStyle
                                {
                                    Padding = new LayoutBox(24, 12, 24, 12),
                                },
                                MenuItems = {
                                    new MenuItem(new MenuItemProps {
                                        Text = "Increase Grow",
                                        OnAccept = e =>
                                        {
                                            SetState(state => state.FirstWindowGrow ++);
                                        },
                                    }),
                                    new MenuItem(new MenuItemProps {
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
                            new Menu(new MenuProps
                            {
                                Name = "growWindow",
                                Style = new InlineElementStyle
                                {
                                    Padding = new LayoutBox(12, 6, 12, 6),
                                },
                                MenuItems =
                                {
                                    new MenuItem(new MenuItemProps {
                                        Text = "Increase Grow",
                                        OnAccept = e =>
                                        {
                                            SetState(state => state.SecondWindowGrow ++);
                                        },
                                    }),
                                    new MenuItem(new MenuItemProps {
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