using AgateLib.UserInterface.Styling.Themes;
using FluentAssertions;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Xunit;

namespace AgateLib.UserInterface.Widgets
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

            var driver = new UserInterfaceTestDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            root.Display.ContentRect.Should().Be(
                new Rectangle(515, 0, 50, Math.Min(620, buttonCount * 10)));

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
                    new Button(new ButtonProps { Text = $"Button {i + 1:000}" }))
                    .ToList<IRenderable>(),
            });

            var driver = new UserInterfaceTestDriver(CreateApp(box), styleConfigurator);
            driver.DoLayout();

            var root = driver.Desktop.ActiveWorkspace.VisualTree.Find("#thewindow").First();
            var elements = root.Children.ToList();

            root.Display.ContentRect.Should().Be(
                new Rectangle(515, 0, 50, Math.Min(620, buttonCount * 10)));

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
