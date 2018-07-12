using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Styling.Themes;
using AgateLib.UserInterface.Widgets;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Tests.UserInterface.Widgets
{
    public class FlexBoxTest
    {
        private ThemeStyler styleConfigurator;
        private IWidgetRenderContext renderContext = CommonMocks.RenderContext().Object;

        public FlexBoxTest()
        {
            var themes = new ThemeCollection();
            themes["default"] = new Theme();

            styleConfigurator = new ThemeStyler(themes);
        }

        #region --- Flex Direction Column Tests ---

        [Fact]
        public void FlexColumnDefaults()
        {
            Window box = new Window(new WindowProps
            {
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "AHe" }),
                    new Label(new LabelProps { Text = "BHel" }),
                    new Label(new LabelProps { Text = "CHell" }),
                    new Label(new LabelProps { Text = "DHello" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(0, 0,  30, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(0, 10, 30, 10));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(0, 20, 30, 10));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(0, 30, 30, 10));
        }

        [Fact]
        public void FlexColumnDefaultsItemsWithPadding()
        {
            var style = new InlineElementStyle
            {
                Padding = LayoutBox.SameAllAround(4)
            };

            Window box = new Window(new WindowProps
            {
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Style = style, Text = "AHe" }),
                    new Label(new LabelProps { Style = style, Text = "BHel" }),
                    new Label(new LabelProps { Style = style, Text = "CHell" }),
                    new Label(new LabelProps { Style = style, Text = "DHello" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.MarginRect.Should().Be(new Rectangle(0,  0, 38, 18));
            elements[1].Display.MarginRect.Should().Be(new Rectangle(0, 18, 38, 18));
            elements[2].Display.MarginRect.Should().Be(new Rectangle(0, 36, 38, 18));
            elements[3].Display.MarginRect.Should().Be(new Rectangle(0, 54, 38, 18));

            elements[0].Display.ContentRect.Should().Be(new Rectangle(4,  4, 30, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(4, 22, 30, 10));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(4, 40, 30, 10));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(4, 58, 30, 10));
        }

        [Fact]
        public void FlexColumnAlignItemsStart()
        {
            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        AlignItems = AlignItems.Start
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "AHe" }),
                    new Label(new LabelProps { Text = "BHel" }),
                    new Label(new LabelProps { Text = "CHell" }),
                    new Label(new LabelProps { Text = "DHello" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(0, 0, 15, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(0, 10, 20, 10));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(0, 20, 25, 10));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(0, 30, 30, 10));
        }


        [Fact]
        public void FlexColumnAlighItemsStartWithPadding()
        {
            var style = new InlineElementStyle
            {
                Padding = LayoutBox.SameAllAround(4)
            };

            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        AlignItems = AlignItems.Start
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Style = style, Text = "AHe" }),
                    new Label(new LabelProps { Style = style, Text = "BHel" }),
                    new Label(new LabelProps { Style = style, Text = "CHell" }),
                    new Label(new LabelProps { Style = style, Text = "DHello" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.MarginRect.Should().Be(new Rectangle(0, 0, 23, 18));
            elements[1].Display.MarginRect.Should().Be(new Rectangle(0, 18, 28, 18));
            elements[2].Display.MarginRect.Should().Be(new Rectangle(0, 36, 33, 18));
            elements[3].Display.MarginRect.Should().Be(new Rectangle(0, 54, 38, 18));

            elements[0].Display.ContentRect.Should().Be(new Rectangle(4, 4, 15, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(4, 22, 20, 10));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(4, 40, 25, 10));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(4, 58, 30, 10));
        }

        [Fact]
        public void FlexColumnAlignItemsStretch()
        {
            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        AlignItems = AlignItems.Stretch,
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "AHe" }),
                    new Label(new LabelProps { Text = "BHel" }),
                    new Label(new LabelProps { Text = "CHell" }),
                    new Label(new LabelProps { Text = "DHello" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(0, 0, 30, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(0, 10, 30, 10));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(0, 20, 30, 10));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(0, 30, 30, 10));
        }

        [Fact]
        public void FlexColumnAlignItemsEnd()
        {
            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        AlignItems = AlignItems.End,
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "AHe" }),
                    new Label(new LabelProps { Text = "BHel" }),
                    new Label(new LabelProps { Text = "CHell" }),
                    new Label(new LabelProps { Text = "DHello" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(15, 0, 15, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(10, 10, 20, 10));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(5, 20, 25, 10));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(0, 30, 30, 10));
        }

        #endregion
        #region --- Flex Direction Column Reverse Tests ---

        [Fact]
        public void FlexColumnReverseDefaults()
        {
            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.ColumnReverse
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "AHe" }),
                    new Label(new LabelProps { Text = "BHel" }),
                    new Label(new LabelProps { Text = "CHell" }),
                    new Label(new LabelProps { Text = "DHello" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(0, 30, 30, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(0, 20, 30, 10));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(0, 10, 30, 10));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(0, 0, 30, 10));
        }

        [Fact]
        public void FlexColumnReverseAlignItemsStart()
        {
            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.ColumnReverse,
                        AlignItems = AlignItems.Start
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "AHe" }),
                    new Label(new LabelProps { Text = "BHel" }),
                    new Label(new LabelProps { Text = "CHell" }),
                    new Label(new LabelProps { Text = "DHello" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(0, 30, 15, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(0, 20, 20, 10));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(0, 10, 25, 10));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(0, 0, 30, 10));
        }

        [Fact]
        public void FlexColumnReverseAlignItemsStretch()
        {
            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.ColumnReverse,
                        AlignItems = AlignItems.Stretch,
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "AHe" }),
                    new Label(new LabelProps { Text = "BHel" }),
                    new Label(new LabelProps { Text = "CHell" }),
                    new Label(new LabelProps { Text = "DHello" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(0, 30, 30, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(0, 20, 30, 10));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(0, 10, 30, 10));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(0, 0, 30, 10));
        }

        [Fact]
        public void FlexColumnReverseAlignItemsEnd()
        {
            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.ColumnReverse,
                        AlignItems = AlignItems.End,
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "AHe" }),
                    new Label(new LabelProps { Text = "BHel" }),
                    new Label(new LabelProps { Text = "CHell" }),
                    new Label(new LabelProps { Text = "DHello" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(15, 30, 15, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(10, 20, 20, 10));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(5, 10, 25, 10));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(0, 0, 30, 10));
        }

        #endregion

        #region --- Flex Direction Row Tests ---

        [Fact]
        public void FlexRowDefaults()
        {
            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.Row,
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "Abc" }),
                    new Label(new LabelProps { Text = "Defg\nHijk" }),
                    new Label(new LabelProps { Text = "Lmnop\nLmnop\nLmnop" }),
                    new Label(new LabelProps { Text = "Qrstuv\na\nb\nc" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(0, 0, 15, 40));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(15, 0, 20, 40));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(35, 0, 25, 40));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(60, 0, 30, 40));
        }

        [Fact]
        public void FlexRowDefaultsItemsWithPadding()
        {
            var style = new InlineElementStyle
            {
                Padding = LayoutBox.SameAllAround(4)
            };

            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.Row,
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Style = style, Text = "Abc" }),
                    new Label(new LabelProps { Style = style, Text = "Defg\nHijk" }),
                    new Label(new LabelProps { Style = style, Text = "Lmnop\nLmnop\nLmnop" }),
                    new Label(new LabelProps { Style = style, Text = "Qrstuv\na\nb\nc" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.MarginRect.Should().Be(new Rectangle(0, 0, 23, 48));
            elements[1].Display.MarginRect.Should().Be(new Rectangle(23, 0, 28, 48));
            elements[2].Display.MarginRect.Should().Be(new Rectangle(51, 0, 33, 48));
            elements[3].Display.MarginRect.Should().Be(new Rectangle(84, 0, 38, 48));

            elements[0].Display.ContentRect.Should().Be(new Rectangle(4, 4, 15, 40));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(27, 4, 20, 40));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(55, 4, 25, 40));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(88, 4, 30, 40));
        }

        [Fact]
        public void FlexRowAlignItemsStart()
        {
            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.Row,
                        AlignItems = AlignItems.Start
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "Abc" }),
                    new Label(new LabelProps { Text = "Defg\nHijk" }),
                    new Label(new LabelProps { Text = "Lmnop\nLmnop\nLmnop" }),
                    new Label(new LabelProps { Text = "Qrstuv\na\nb\nc" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(0, 0, 15, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(15, 0, 20, 20));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(35, 0, 25, 30));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(60, 0, 30, 40));
        }

        [Fact]
        public void FlexRowAlignItemsStretch()
        {
            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.Row,
                        AlignItems = AlignItems.Stretch,
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "Abc" }),
                    new Label(new LabelProps { Text = "Defg\nHijk" }),
                    new Label(new LabelProps { Text = "Lmnop\nLmnop\nLmnop" }),
                    new Label(new LabelProps { Text = "Qrstuv\na\nb\nc" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(0, 0, 15, 40));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(15, 0, 20, 40));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(35, 0, 25, 40));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(60, 0, 30, 40));
        }

        [Fact]
        public void FlexRowAlignItemsEnd()
        {
            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.Row,
                        AlignItems = AlignItems.End,
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "Abc" }),
                    new Label(new LabelProps { Text = "Defg\nHijk" }),
                    new Label(new LabelProps { Text = "Lmnop\nLmnop\nLmnop" }),
                    new Label(new LabelProps { Text = "Qrstuv\na\nb\nc" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(0, 30, 15, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(15, 20, 20, 20));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(35, 10, 25, 30));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(60, 0, 30, 40));
        }

        [Fact]
        public void FlexRowJustifyContentCenter()
        {
            Window masterBox = new Window(new WindowProps
            {
                Children =
                {
                    new Label(new LabelProps { Text = "Long label to create additional horizontal space"}),
                    new Window(new WindowProps
                    {
                        Style = new InlineElementStyle
                        {
                            Flex = new FlexStyle
                            {
                                Direction = FlexDirection.Row,
                                JustifyContent = JustifyContent.Center
                            }
                        },
                        StyleId = "thewindow",
                        Children = {
                            new Label(new LabelProps { Text = "AHe" }),
                            new Label(new LabelProps { Text = "BHel" }),
                            new Label(new LabelProps { Text = "CHell" }),
                            new Label(new LabelProps { Text = "DHello" }),
                        }
                    })
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(masterBox), styleConfigurator);

            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(75, 0, 15, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(90, 0, 20, 10));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(110, 0, 25, 10));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(135, 0, 30, 10));
        }

        [Fact]
        public void FlexRowJustifyContentEnd()
        {
            Window masterBox = new Window(new WindowProps
            {
                Children =
                {
                    new Label(new LabelProps { Text = "Long label to create additional horizontal space"}),
                    new Window(new WindowProps
                    {
                        Style = new InlineElementStyle
                        {
                            Flex = new FlexStyle
                            {
                                Direction = FlexDirection.Row,
                                JustifyContent = JustifyContent.End
                            }
                        },
                        StyleId = "thewindow",
                        Children = {
                            new Label(new LabelProps { Text = "AHe" }),
                            new Label(new LabelProps { Text = "BHel" }),
                            new Label(new LabelProps { Text = "CHell" }),
                            new Label(new LabelProps { Text = "DHello" }),
                        }
                    })
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(masterBox), styleConfigurator);

            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(150, 0, 15, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(165, 0, 20, 10));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(185, 0, 25, 10));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(210, 0, 30, 10));
        }

        [Fact]
        public void FlexRowJustifyContentSpaceBetween()
        {
            Window masterBox = new Window(new WindowProps
            {
                Children =
                {
                    new Label(new LabelProps { Text = "Long label to create additional horizontal space"}),
                    new Window(new WindowProps
                    {
                        Style = new InlineElementStyle
                        {
                            Flex = new FlexStyle
                            {
                                Direction = FlexDirection.Row,
                                JustifyContent = JustifyContent.SpaceBetween
                            }
                        },
                        StyleId = "thewindow",
                        Children = {
                            new Label(new LabelProps { Text = "AHe" }),
                            new Label(new LabelProps { Text = "BHel" }),
                            new Label(new LabelProps { Text = "CHell" }),
                            new Label(new LabelProps { Text = "DHello" }),
                        }
                    })
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(masterBox), styleConfigurator);

            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(0, 0, 15, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(65, 0, 20, 10));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(135, 0, 25, 10));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(210, 0, 30, 10));
        }

        [Fact]
        public void FlexRowJustifyContentSpaceAround()
        {
            Window masterBox = new Window(new WindowProps
            {
                Children =
                {
                    new Label(new LabelProps { Text = "Long label to create additional horizontal space"}),
                    new Window(new WindowProps
                    {
                        Style = new InlineElementStyle
                        {
                            Flex = new FlexStyle
                            {
                                Direction = FlexDirection.Row,
                                JustifyContent = JustifyContent.SpaceAround
                            }
                        },
                        StyleId = "thewindow",
                        Children = {
                            new Label(new LabelProps { Text = "AHe" }),
                            new Label(new LabelProps { Text = "BHel" }),
                            new Label(new LabelProps { Text = "CHell" }),
                            new Label(new LabelProps { Text = "DHello" }),
                        }
                    })
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(masterBox), styleConfigurator);

            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(18, 0, 15, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(71, 0, 20, 10));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(128, 0, 25, 10));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(191, 0, 30, 10));
        }

        [Fact]
        public void FlexRowJustifyContentSpaceEvenly()
        {
            Window masterBox = new Window(new WindowProps
            {
                Children =
                {
                    new Label(new LabelProps { Text = "Long label to create additional horizontal space"}),
                    new Window(new WindowProps
                    {
                        Style = new InlineElementStyle
                        {
                            Flex = new FlexStyle
                            {
                                Direction = FlexDirection.Row,
                                JustifyContent = JustifyContent.SpaceEvenly
                            }
                        },
                        StyleId = "thewindow",
                        Children = {
                            new Label(new LabelProps { Text = "AHe" }),
                            new Label(new LabelProps { Text = "BHel" }),
                            new Label(new LabelProps { Text = "CHell" }),
                            new Label(new LabelProps { Text = "DHello" }),
                        }
                    })
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(masterBox), styleConfigurator);

            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(30, 0, 15, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(75, 0, 20, 10));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(125, 0, 25, 10));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(180, 0, 30, 10));
        }

        #endregion
        #region --- Flex Direction Row Reverse Tests ---

        [Fact]
        public void FlexRowReverseDefaults()
        {
            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.RowReverse
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "Abc" }),
                    new Label(new LabelProps { Text = "Defg\nHijk" }),
                    new Label(new LabelProps { Text = "Lmnop\nLmnop\nLmnop" }),
                    new Label(new LabelProps { Text = "Qrstuv\na\nb\nc" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(75, 0, 15, 40));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(55, 0, 20, 40));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(30, 0, 25, 40));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(0, 0, 30, 40));
        }

        [Fact]
        public void FlexRowReverseAlignItemsStart()
        {
            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.RowReverse,
                        AlignItems = AlignItems.Start
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "Abc" }),
                    new Label(new LabelProps { Text = "Defg\nHijk" }),
                    new Label(new LabelProps { Text = "Lmnop\nLmnop\nLmnop" }),
                    new Label(new LabelProps { Text = "Qrstuv\na\nb\nc" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(75, 0, 15, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(55, 0, 20, 20));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(30, 0, 25, 30));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(0, 0, 30, 40));
        }

        [Fact]
        public void FlexRowReverseAlignItemsStretch()
        {
            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.RowReverse,
                        AlignItems = AlignItems.Stretch,
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "Abc" }),
                    new Label(new LabelProps { Text = "Defg\nHijk" }),
                    new Label(new LabelProps { Text = "Lmnop\nLmnop\nLmnop" }),
                    new Label(new LabelProps { Text = "Qrstuv\na\nb\nc" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(75, 0, 15, 40));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(55, 0, 20, 40));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(30, 0, 25, 40));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(0, 0, 30, 40));
        }

        [Fact]
        public void FlexRowReverseAlignItemsEnd()
        {
            Window box = new Window(new WindowProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.RowReverse,
                        AlignItems = AlignItems.End,
                    }
                },
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "Abc" }),
                    new Label(new LabelProps { Text = "Defg\nHijk" }),
                    new Label(new LabelProps { Text = "Lmnop\nLmnop\nLmnop" }),
                    new Label(new LabelProps { Text = "Qrstuv\na\nb\nc" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(75, 30, 15, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(55, 20, 20, 20));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(30, 10, 25, 30));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(0, 0, 30, 40));
        }

        #endregion

        private IWidget CreateApp(IWidget contents)
        {
            return new App(new AppProps { Children = new[] { contents } });
        }
    }

}
