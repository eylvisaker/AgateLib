using AgateLib.Tests.UserInterface.Scrolling;
using FluentAssertions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AgateLib.UserInterface.Scrolling
{
    public class ScrollingAppUnitTests
    {
        [Fact]
        public void ScrollingAppFirstWindowLayoutTest()
        {
            var appRef = new ElementReference();

            ScrollingApp app = new ScrollingApp(new ScrollingAppProps
            {
                Ref = appRef,
            });

            var driver = new UserInterfaceTestDriver(app);

            driver.DoLayout();

            IRenderElement rootElement = appRef.Current;

            var scrollWindow1 = rootElement.Children.First();
            
            rootElement.Display.MarginRect.Should().Be(new Rectangle(0, 0, 1280, 720));
            scrollWindow1.Display.ContentRect.Should().Be(new Rectangle(254, 0, 159, 620));

            IRenderElement header = scrollWindow1.Children.First();

            header.Display.MarginRect.Should().Be(new Rectangle(0, 0, 159, 36));

            List<ButtonElement> buttons = scrollWindow1.Children
                                         .Skip(1)
                                         .Cast<ButtonElement>()
                                         .ToList();

            buttons.Count.Should().Be(50);

            int y = header.Display.MarginRect.Bottom;

            foreach(var button in buttons)
            {
                button.Display.MarginRect.Height.Should().Be(26);
                button.Display.MarginRect.Top.Should().Be(y);

                var label = button.Children.First();

                label.Display.ContentRect.Height.Should().Be(10);

                y += button.Display.MarginRect.Height;
            }

            scrollWindow1.Display.HasOverflow.Should().Be(HasOverflow.Y);
        }

        [Fact]
        public void ScrollingAppSecondWindowLayoutTest()
        {
            var appRef = new ElementReference();

            ScrollingApp app = new ScrollingApp(new ScrollingAppProps
            {
                Ref = appRef,
            });

            var driver = new UserInterfaceTestDriver(app);

            driver.DoLayout();

            IRenderElement rootElement = appRef.Current;

            var scrollWindow2 = rootElement.Children.Last();

            rootElement.Display.MarginRect.Should().Be(new Rectangle(0, 0, 1280, 720));
            scrollWindow2.Display.ContentRect.Should().Be(new Rectangle(667, 0, 159, 620));

            IRenderElement header = scrollWindow2.Children.First();

            header.Display.MarginRect.Should().Be(new Rectangle(0, 0, 159, 36));

            IRenderElement box = scrollWindow2.Children.Last();

            List<ButtonElement> buttons = box.Children
                                         .Cast<ButtonElement>()
                                         .ToList();

            buttons.Count.Should().Be(50);

            int y = 0;

            foreach (var button in buttons)
            {
                button.Display.MarginRect.Height.Should().Be(26);
                button.Display.MarginRect.Top.Should().Be(y);

                var label = button.Children.First();

                label.Display.ContentRect.Height.Should().Be(10);

                y += button.Display.MarginRect.Height;
            }

            box.Display.HasOverflow.Should().Be(HasOverflow.Y);
            scrollWindow2.Display.HasOverflow.Should().Be(HasOverflow.None);
        }
    }
}
