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

        [Fact]
        public void AddItemInMiddle()
        {
            var app = new ElementReference();
            var b = new ElementReference();
            var c = new ElementReference();

            var appWidget = new App(new AppProps
            {
                Children = {
                    new Separator(new SeparatorProps { Key = "a" }),
                    new Window (new WindowProps { Key = "c", Ref = b })
                },
                Ref = app,
            });

            var driver = new UserInterfaceTestDriver(appWidget);

            app.Current.Children.Count.Should().Be(2);

            appWidget.SetProps(new AppProps
            {
                Children = {
                    new Separator(new SeparatorProps { Key = "a" }),
                    new Window (new WindowProps { Key = "b", Ref = b }),
                    new Window (new WindowProps { Key = "c", Ref = c }),
                }
            });

            app.Current.Children.Count.Should().Be(3);

            b.Current.Should().NotBeNull();

            app.Current.Children[1].Should().Be(b.Current, $"b should be in middle, but found {app.Current.Children[1].Props.Key}");
            app.Current.Children[2].Should().Be(c.Current, $"c should be at end, but found {app.Current.Children[2].Props.Key}");
        }

        [Fact]
        public void RemoveItemFromMiddle()
        {
            var app = new ElementReference();
            var b = new ElementReference();
            var c = new ElementReference();
            var buttonB = new ElementReference();
            var buttonC = new ElementReference();

            var appWidget = new App(new AppProps
            {
                Children = {
                    new Separator(new SeparatorProps { Key = "a" }),
                    new Window (new WindowProps { Key = "b", Ref = b, Children = { new Button(new ButtonProps { Ref = buttonB, Name="ButtonB" }) } }),
                    new Window (new WindowProps { Key = "c", Ref = c, Children = { new Button(new ButtonProps { Ref = buttonC, Name="ButtonC" }) } }),
                },
                Ref = app,
            });

            var driver = new UserInterfaceTestDriver(appWidget);

            app.Current.Children.Count.Should().Be(3);
            driver.Focus.Should().Be(buttonB.Current);

            appWidget.SetProps(new AppProps
            {
                Children = {
                    new Separator(new SeparatorProps { Key = "a" }),
                    new Window (new WindowProps { Key = "c", Ref = c, Children = { new Button(new ButtonProps { Ref = buttonC, Name = "ButtonC" }) } }),
                },
                Ref = app,
            });

            app.Current.Children.Count.Should().Be(2);

            b.Current.Should().NotBeNull();

            app.Current.Children[1].Should().Be(c.Current, $"c should be at end, but found {app.Current.Children[1].Props.Key}");
            driver.Focus.Should().Be(buttonC.Current, $"focus should be C, but found {driver.Focus.Props.Name}");
        }
    }
}
