using AgateLib.UserInterface.Widgets;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace AgateLib.UserInterface
{
    public class VisualTreeReconcileUnitTests
    {
        private class TestApp : Widget<TestAppProps, TestAppState>
        {
            public TestApp(TestAppProps props) : base(props)
            {
                SetState(new TestAppState { ItemCount = props.InitialItemCount });
            }
            
            public override IRenderable Render()
            {
                return new Window(new WindowProps
                {
                    Children =
                    {
                        new Window(new WindowProps
                        {
                            Children = Enumerable.Range(0, 5).Select(i => new Button(new ButtonProps {
                                Text = $"Other window has {i} items",
                                OnAccept = e => {
                                    SetState(state => state.ItemCount = i);
                                }
                            })).ToList<IRenderable>()
                        }),
                        new Window(new WindowProps
                        {
                            Ref = Props.ManipWindowRef,
                            Children = Enumerable.Range(0, State.ItemCount).Select(i => new Label(new LabelProps {
                                Text = $"Item {i}",
                            })).ToList<IRenderable>()
                        }),
                    }
                });
            }
        }

        public class TestAppProps : WidgetProps
        {
            public int InitialItemCount { get; set; }
            public ElementReference ManipWindowRef { get; set; } = new ElementReference();
        }

        public class TestAppState
        {
            public int ItemCount { get; set; }
        }

        [Fact]
        public void RemoveAllItemsFromWindow()
        {
            var props = new TestAppProps { InitialItemCount = 4 };

            UserInterfaceTestDriver driver = new UserInterfaceTestDriver(new TestApp(props));

            props.ManipWindowRef.Current.Children.Count.Should().Be(4);

            driver.Press(Microsoft.Xna.Framework.Input.Buttons.A);

            props.ManipWindowRef.Current.Children.Count.Should().Be(0);
        }

        [Fact]
        public void AddItemsToBlankWindow()
        {
            var props = new TestAppProps { InitialItemCount = 0 };

            UserInterfaceTestDriver driver = new UserInterfaceTestDriver(new TestApp(props));

            props.ManipWindowRef.Current.Children.Count.Should().Be(0);

            driver.Press(Microsoft.Xna.Framework.Input.Buttons.DPadDown, repeatCount: 4);
            driver.Press(Microsoft.Xna.Framework.Input.Buttons.A);

            props.ManipWindowRef.Current.Children.Count.Should().Be(4);
        }
    }

}
