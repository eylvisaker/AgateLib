using AgateLib.Tests.UserInterface.Widgets;
using AgateLib.UserInterface;
using Xunit;

namespace AgateLib.Tests.UserInterface
{
    public class VisualTreeTests
    {
        private class TestApp : Widget<TestAppProps, TestAppState>
        {
            public TestApp(TestAppProps props) : base(props)
            {
                SetState(new TestAppState());
            }

            public ElementReference MenuA { get; private set; }
            public ElementReference MenuB { get; private set; }

            public override IRenderable Render()
            {
                MenuA = new ElementReference();
                MenuB = new ElementReference();

                return new Window(new WindowProps
                {
                    Children =
                    {
                        new Label(new LabelProps { Text = State.Switches.ToString() }),
                        new Window(new WindowProps
                        {
                            Ref = MenuA,
                            Children =
                            {
                                new Button(new ButtonProps {
                                    Text = "Go directly to jail.",
                                    OnAccept = e =>{ SetState(state => state.Switches++); e.System.SetFocus(MenuB); },
                                }),
                            }
                        }),
                        new Window(new WindowProps
                        {
                            Ref = MenuB,
                            Children =
                            {
                                new Button(new ButtonProps {
                                    Text = "Do not collect $200.",
                                    OnAccept = e =>{ SetState(state => state.Switches++); e.System.SetFocus(MenuA); },
                                }),
                            }
                        }),
                    }
                });
            }
        }

        public class TestAppProps : WidgetProps
        {
        }

        public class TestAppState
        {
            public int Switches { get; set; }
        }

        [Fact]
        public void ReferenceStaysAliveAfterReconciliation()
        {
            TestUIDriver driver = new TestUIDriver(new TestApp(new TestAppProps()));

            driver.Press(Microsoft.Xna.Framework.Input.Buttons.A);
            driver.Press(Microsoft.Xna.Framework.Input.Buttons.A);
        }
    }

}
