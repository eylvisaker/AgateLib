using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Display;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;

namespace AgateLib.Tests.UserInterface.JustifyContentTest
{
    public class JustifyContentApp : Widget<JustifyContentAppProps, JustifyContentAppState>
    {
        public JustifyContentApp(JustifyContentAppProps props) : base(props)
        {
            SetState(new JustifyContentAppState());
        }

        public override IRenderable Render()
        {
            var labelStyle = new InlineElementStyle
            {
                Background = new BackgroundStyle { Color = ColorX.FromArgb("e34234") },
                Padding = LayoutBox.SameAllAround(8)
            };

            return new App(new AppProps
            {
                Children =
                {
                    new Window(new WindowProps
                    {
                        Children =
                        {
                            new Label(new LabelProps { Text = "Justify Content Value"}),
                            new RadioMenu(new RadioMenuProps
                            {
                                Buttons = {
                                    new RadioButton(new RadioButtonProps {
                                        Text = "Start", OnAccept = e => Set(JustifyContent.Start),  Checked = true, }),
                                    new RadioButton(new RadioButtonProps {
                                        Text = "End", OnAccept = e => Set(JustifyContent.End)}),
                                    new RadioButton(new RadioButtonProps {
                                        Text = "Center", OnAccept = e => Set(JustifyContent.Center)}),
                                    new RadioButton(new RadioButtonProps {
                                        Text = "Space Between", OnAccept = e => Set(JustifyContent.SpaceBetween)}),
                                    new RadioButton(new RadioButtonProps {
                                        Text = "Space Around", OnAccept = e => Set(JustifyContent.SpaceAround)}),
                                    new RadioButton(new RadioButtonProps {
                                        Text = "Space Evenly", OnAccept = e => Set(JustifyContent.SpaceEvenly)}),
                                },
                                OnCancel = Props.OnCancel,
                            })

                        }
                    }),

                    new Window(new WindowProps
                    {
                        Style = new InlineElementStyle
                        {
                            Padding = LayoutBox.SameAllAround(12),
                        },
                        Children =
                        {
                            new Label(new LabelProps { Text = "Really long text to setup some horizontal space." }),
                            new Window(new WindowProps
                            {
                                Name = "magic-window",
                                Style = new InlineElementStyle
                                {
                                    Flex = new FlexStyle
                                    {
                                        Direction = FlexDirection.Row,
                                        JustifyContent = State.JustifyContent,
                                    },
                                    Background = new BackgroundStyle{ Color = Color.DimGray }
                                },
                                Children =
                                {
                                    new Label(new LabelProps{ Style = labelStyle, Text = "This", }),
                                    new Label(new LabelProps{ Style = labelStyle, Text = "is"}),
                                    new Label(new LabelProps{ Style = labelStyle, Text = "an"}),
                                    new Label(new LabelProps{ Style = labelStyle, Text = "example"}),
                                }
                            }),
                        }
                    })
                }
            });
        }

        private void Set(JustifyContent value)
        {
            SetState(state => state.JustifyContent = value);
        }
    }

    public class JustifyContentAppProps : WidgetProps
    {
        public UserInterfaceEventHandler OnCancel { get; set; }
    }

    public class JustifyContentAppState : WidgetState
    {
        public JustifyContent JustifyContent { get; set; }
    }
}
