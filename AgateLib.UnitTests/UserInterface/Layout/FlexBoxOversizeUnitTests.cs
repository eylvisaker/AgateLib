using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Styling.Themes;
using AgateLib.UserInterface;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Tests.UserInterface.Widgets
{
    public class FlexBoxTest
    {
        private ThemeStyler styleConfigurator;
        private IUserInterfaceRenderContext renderContext = CommonMocks.RenderContext().Object;

        public FlexBoxTest()
        {
            var themes = new ThemeCollection();
            themes["default"] = new Theme();

            styleConfigurator = new ThemeStyler(themes);
        }

        [Fact]
        public void FlexOversizeWindowWithLabels()
        {
            const int buttonCount = 200;

            Window box = new Window(new WindowProps
            {
                Name = "thewindow",
                Children = Enumerable.Range(0, buttonCount).Select(i =>
                    new Label(new LabelProps { Text = $"Label  {i + 1:000}" }))
                    .ToList<IRenderable>(),
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            root.Display.ContentRect.Should().Be(
                new Rectangle(615, 0, 50, Math.Min(720, buttonCount * 10)));

            for (int i = 0; i < buttonCount; i++)
            {
                int expected_y = i * 10;

                elements[i].Display.ContentRect.Should().Be(
                    new Rectangle(0, expected_y, 50, 10));
            }
        }

        [Fact]
        public void FlexOversizeWindowWithButtons()
        {
            const int buttonCount = 200;

            Window box = new Window(new WindowProps
            {
                Name = "thewindow",
                Children = Enumerable.Range(0, buttonCount).Select(i =>
                    new Button(new ButtonProps { Text = $"Button {i+1:000}"}))
                    .ToList<IRenderable>(),
            });

            TestUIDriver driver = new TestUIDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            root.Display.ContentRect.Should().Be(
                new Rectangle(615, 0, 50, Math.Min(720, buttonCount * 10)));

            for (int i = 0; i < buttonCount; i++)
            {
                int expected_y = i * 10;

                elements[i].Display.ContentRect.Should().Be(
                    new Rectangle(0, expected_y, 50, 10));
            }
        }

        private IRenderable CreateApp(IRenderable contents)
        {
            return new App(new AppProps { Children = new[] { contents } });
        }
    }

}
