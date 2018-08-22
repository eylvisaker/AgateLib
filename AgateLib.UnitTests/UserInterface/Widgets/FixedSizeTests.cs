using AgateLib.Mathematics.Geometry;
using AgateLib.Tests.UserInterface.Content;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Styling.Themes;
using AgateLib.UserInterface;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Tests.UserInterface.Widgets
{
    public class FixedSizeTests
    {
        private ThemeStyler styleConfigurator;
        private ContentLayoutEngineLogger contentLayoutEngine;
        private IUserInterfaceRenderContext renderContext;

        public FixedSizeTests()
        {
            var themes = new ThemeCollection();
            themes["default"] = new Theme();

            styleConfigurator = new ThemeStyler(themes);

            contentLayoutEngine = new ContentLayoutEngineLogger(
                                    new ContentLayoutEngine(
                                        CommonMocks.FontProvider().Object));
        }

        // TODO: Implement fixed size as fractions of parent size.
        //[Fact]
        //public void FixedSizePassedToChildren()
        //{
        //    ElementReference label = new ElementReference();

        //    Window box = new Window(new WindowProps
        //    {
        //        Name = "thewindow",
        //        Style = new InlineElementStyle
        //        {
        //            Size = new SizeConstraints
        //            {
        //                Width = 100,
        //                Height = 200,
        //            }
        //        },
        //        Children =
        //        {
        //            new Label(new LabelProps
        //            {
        //                Text = "This is some really long text that should be forced to wrap.",
        //                Ref = label,
        //            }),
        //        }
        //    });

        //    TestUIDriver driver = new TestUIDriver(CreateApp(box),
        //                                           styleConfigurator,
        //                                           contentLayoutEngine: contentLayoutEngine);
        //    driver.DoLayout();

        //    label.Current.Display.Region.IdealContentSize.Should().Be(new Size(95, 40));
        //}

        [Theory]
        [InlineData(300, 200)]
        [InlineData(200, 100)]
        [InlineData(350, 50)]
        public void RowSingleChildFixedWidthSecondChildResized(int mainWidth, int leftWidth)
        {
            ElementReference fixedLabel = new ElementReference();
            ElementReference fillLabel = new ElementReference();

            Window box = new Window(new WindowProps
            {
                Name = "thewindow",
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.Row,
                        AlignItems= AlignItems.Stretch,
                    },
                    Size = new SizeConstraints
                    {
                        MinWidth = mainWidth,
                        MaxWidth = mainWidth,
                        MinHeight = 400,
                        MaxHeight = 400,
                    }
                },
                Children =
                {
                    new Label(new LabelProps
                    {
                        Style = new InlineElementStyle
                        {
                            Size = new SizeConstraints
                            {
                                MinWidth = leftWidth,
                                MaxWidth = leftWidth,
                            }
                        },
                        Text = $"This is a fixed size label. It will only be {leftWidth} pixels wide.",
                        Ref = fixedLabel,
                    }),
                    new Label(new LabelProps
                    {
                        Text = "This is some really long text that should be forced to wrap. There should be some extra juicy details in this text string that will require wrapping for the user to enjoy reading them.",
                        Ref = fillLabel,
                    }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), 
                                                   styleConfigurator, 
                                                   contentLayoutEngine: contentLayoutEngine);
            driver.DoLayout();

            fixedLabel.Current.Display.ContentRect.Should().Be(new Rectangle(0, 0, leftWidth, 400));
            fillLabel.Current.Display.ContentRect.Should().Be(new Rectangle(leftWidth, 0, mainWidth - leftWidth, 400));
        }

        private IRenderable CreateApp(IRenderable contents)
        {
            return new App(new AppProps { Children = new[] { contents } });
        }
    }
}
