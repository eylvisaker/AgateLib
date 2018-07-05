using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Widgets;
using FluentAssertions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AgateLib.Tests.UserInterface.Widgets
{
    public class FlexBoxTest
    {
        IWidgetRenderContext renderContext = CommonMocks.RenderContext().Object;

        [Fact]
        public void ColumnLayout()
        {
            Window box = new Window(new WindowProps
            {
                StyleId = "thewindow",
                Children = {
                    new Label(new LabelProps { Text = "HelloA" }),
                    new Label(new LabelProps { Text = "HelloB" }),
                    new Label(new LabelProps { Text = "HelloC" }),
                    new Label(new LabelProps { Text = "HelloD" }),
                }
            });

            TestUIDriver driver = new TestUIDriver(box);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            elements[0].Display.ContentRect.Should().Be(new Rectangle(0, 0, 30, 10));
            elements[1].Display.ContentRect.Should().Be(new Rectangle(0, 10, 30, 10));
            elements[2].Display.ContentRect.Should().Be(new Rectangle(0, 20, 30, 10));
            elements[3].Display.ContentRect.Should().Be(new Rectangle(0, 30, 30, 10));
        }
    }
}
