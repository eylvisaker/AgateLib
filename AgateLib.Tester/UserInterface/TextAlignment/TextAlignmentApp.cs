using AgateLib.UserInterface;
using Microsoft.Xna.Framework;

namespace AgateLib.Tests.UserInterface.TextAlignment
{
    public class TextAlignmentApp : Widget<TextAlignmentAppProps, TextAlignmentAppState>
    {
        public TextAlignmentApp(TextAlignmentAppProps props) : base(props)
        {
            SetState(new TextAlignmentAppState());
        }

        public object SetAlignmentOnProps { get; private set; }

        public override IRenderable Render() => new App(new AppProps
        {
            Children =
            {
                new Window(new WindowProps
                {
                    OnCancel = Props.OnCancel,
                    Children =
                    {
                        new Label(new LabelProps { Text = "Some really long text to set the maximum width of the window. This is used to make sure the grid below has plenty of space to adjust the layout of its items as the TextAlign property is changed."}),
                        new RadioMenu(new RadioMenuProps
                        {
                            Buttons =
                            {
                                new RadioButton(new RadioButtonProps
                                {
                                    Text = "Align Left",
                                    OnAccept = e => SetAlign(TextAlign.Left),
                                    Checked = true
                                }),
                                new RadioButton(new RadioButtonProps
                                {
                                    Text = "Align Center",
                                    OnAccept = e => SetAlign(TextAlign.Center),
                                }),
                                new RadioButton(new RadioButtonProps
                                {
                                    Text = "Align Right" ,
                                    OnAccept = e => SetAlign(TextAlign.Right),
                                }),
                            }
                        }),
                        new Label(new LabelProps
                        {
                            Text = "Not in a grid!",
                            Style = new InlineElementStyle
                            {
                                Border = BorderStyle.Solid(Color.Blue, 1),
                                Margin = LayoutBox.SameAllAround(8),
                                TextAlign = State.TextAlign,
                            },
                        }),
                        new Grid(new GridProps
                        {
                            Style = new InlineElementStyle
                            {
                                Border = BorderStyle.Solid(Color.Yellow, 1),
                                Margin = LayoutBox.SameAllAround(8),
                            },
                            Columns = 2,
                        }
                            .Add("Strength").Add("110", SetAlignOnProps)
                            .Add("Dexterity").Add("8", SetAlignOnProps)
                            .Add("Intelligence").Add("32", SetAlignOnProps)
                            .Add("Endurance").Add("1002", SetAlignOnProps)
                        ),
                    }
                }),
            }
        });

        private void SetAlignOnProps(LabelProps labelProps)
        {
            labelProps.Style = new InlineElementStyle
            {
                TextAlign = State.TextAlign
            };
        }

        private void SetAlign(TextAlign align)
        {
            SetState(state => state.TextAlign = align);
        }
    }

    public class TextAlignmentAppProps : WidgetProps
    {
        public UserInterfaceEventHandler OnCancel { get; set; }
    }

    public class TextAlignmentAppState
    {
        public TextAlign TextAlign { get; set; }
    }
}
