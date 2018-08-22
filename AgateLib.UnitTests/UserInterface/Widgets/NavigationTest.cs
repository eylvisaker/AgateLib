using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Tests.UserInterface.DoubleRadioMenus;
using AgateLib.UserInterface;
using FluentAssertions;
using Microsoft.Xna.Framework.Input;
using Xunit;

namespace AgateLib.Tests.UserInterface.Widgets
{
    public class NavigationTest
    {
        [Fact]
        public void DoubleRadiosAcceptDisabledByDefault()
        {
            var app = new DoubleRadioMenusApp(new DoubleRadioMenusProps
            {
                LeftItems = new[]
                {
                    new ItemData { Name = "Foo", Description = "Food is good for you." },
                    new ItemData { Name = "Bar", Description = "Bars serve drinks." },
                    new ItemData { Name = "Gra", Description = "Gra is a nonsense word." },
                    new ItemData { Name = "Hoh", Description = "Hoh is one letter short." },
                },
                RightItems = new[]
                {
                    new ItemData { Name = "MegaFoo", Description = "Food is good for you." },
                    new ItemData { Name = "MegaBar", Description = "Bars serve drinks." },
                    new ItemData { Name = "MegaGra", Description = "Gra is a nonsense word." },
                    new ItemData { Name = "MegaHoh", Description = "Hoh is one letter short." },
                },
            });

            TestUIDriver driver = new TestUIDriver(app);

            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.DPadDown);

            // This 
            driver.Focus.Should().BeOfType(typeof(RadioButtonElement), "we shouldn't have gotten to the accept button.");
        }

        [Fact]
        public void DoubleRadiosEnabledAfterSelectingBoth()
        {
            var app = new DoubleRadioMenusApp(new DoubleRadioMenusProps
            {
                LeftItems = new[]
                {
                    new ItemData { Name = "Foo", Description = "Food is good for you." },
                    new ItemData { Name = "Bar", Description = "Bars serve drinks." },
                    new ItemData { Name = "Gra", Description = "Gra is a nonsense word." },
                    new ItemData { Name = "Hoh", Description = "Hoh is one letter short." },
                },
                RightItems = new[]
                {
                    new ItemData { Name = "MegaFoo", Description = "Food is good for you." },
                    new ItemData { Name = "MegaBar", Description = "Bars serve drinks." },
                    new ItemData { Name = "MegaGra", Description = "Gra is a nonsense word." },
                    new ItemData { Name = "MegaHoh", Description = "Hoh is one letter short." },
                },
            });

            TestUIDriver driver = new TestUIDriver(app);

            driver.Press(Buttons.A);
            driver.Press(Buttons.DPadRight);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.A);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.DPadDown);

            app.SelectedLeft.Name.Should().Be("Foo");
            app.SelectedRight.Name.Should().Be("MegaGra");

            driver.Focus.Should().BeOfType(typeof(ButtonElement));
        }

        [Fact]
        public void DoubleRadiosSelectingItems()
        {
            var app = new DoubleRadioMenusApp(new DoubleRadioMenusProps
            {
                LeftItems = new[]
                {
                    new ItemData { Name = "Foo", Description = "Food is good for you." },
                    new ItemData { Name = "Bar", Description = "Bars serve drinks." },
                    new ItemData { Name = "Gra", Description = "Gra is a nonsense word." },
                    new ItemData { Name = "Hoh", Description = "Hoh is one letter short." },
                },
                RightItems = new[]
                {
                    new ItemData { Name = "MegaFoo", Description = "Food is good for you." },
                    new ItemData { Name = "MegaBar", Description = "Bars serve drinks." },
                    new ItemData { Name = "MegaGra", Description = "Gra is a nonsense word." },
                    new ItemData { Name = "MegaHoh", Description = "Hoh is one letter short." },
                },
            });

            TestUIDriver driver = new TestUIDriver(app);

            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.A);
            driver.Press(Buttons.DPadRight);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.A);

            app.SelectedLeft.Name.Should().Be("Bar");
            app.SelectedRight.Name.Should().Be("MegaGra");
        }

        [Fact]
        public void DoubleRadiosAcceptButton()
        {
            ItemData selectedLeft = null;
            ItemData selectedRight = null;
            bool acceptFired = false;

            var app = new DoubleRadioMenusApp(new DoubleRadioMenusProps
            {
                LeftItems = new[]
                {
                    new ItemData { Name = "Foo", Description = "Food is good for you." },
                    new ItemData { Name = "Bar", Description = "Bars serve drinks." },
                    new ItemData { Name = "Gra", Description = "Gra is a nonsense word." },
                    new ItemData { Name = "Hoh", Description = "Hoh is one letter short." },
                },
                RightItems = new[]
                {
                    new ItemData { Name = "MegaFoo", Description = "Food is good for you." },
                    new ItemData { Name = "MegaBar", Description = "Bars serve drinks." },
                    new ItemData { Name = "MegaGra", Description = "Gra is a nonsense word." },
                    new ItemData { Name = "MegaHoh", Description = "Hoh is one letter short." },
                },
                OnAccept = (left, right) =>
                {
                    acceptFired = true;
                    selectedLeft = left;
                    selectedRight = right;
                }
            });

            TestUIDriver driver = new TestUIDriver(app);

            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.A);
            driver.Press(Buttons.DPadRight);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.A);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.A);

            acceptFired.Should().BeTrue("accept event was not fired.");

            selectedLeft.Name.Should().Be("Bar");
            selectedRight.Name.Should().Be("MegaGra");
        }

        [Fact]
        public void DoubleRadiosDownAndBackUp()
        {
            ItemData selectedLeft = null;
            ItemData selectedRight = null;
            bool acceptFired = false;

            var app = new DoubleRadioMenusApp(new DoubleRadioMenusProps
            {
                LeftItems = new[]
                {
                    new ItemData { Name = "Foo", Description = "Food is good for you." },
                    new ItemData { Name = "Bar", Description = "Bars serve drinks." },
                    new ItemData { Name = "Gra", Description = "Gra is a nonsense word." },
                    new ItemData { Name = "Hoh", Description = "Hoh is one letter short." },
                },
                RightItems = new[]
                {
                    new ItemData { Name = "MegaFoo", Description = "Food is good for you." },
                    new ItemData { Name = "MegaBar", Description = "Bars serve drinks." },
                    new ItemData { Name = "MegaGra", Description = "Gra is a nonsense word." },
                    new ItemData { Name = "MegaHoh", Description = "Hoh is one letter short." },
                },
                OnAccept = (left, right) =>
                {
                    acceptFired = true;
                    selectedLeft = left;
                    selectedRight = right;
                }
            });

            TestUIDriver driver = new TestUIDriver(app);

            driver.Press(Buttons.DPadRight);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.A);
            driver.Press(Buttons.DPadLeft);
            driver.Press(Buttons.A);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.DPadDown);
            driver.Press(Buttons.DPadUp);
            driver.Press(Buttons.A);

            acceptFired.Should().BeFalse("accept event was not fired.");

            app.SelectedLeft.Name.Should().Be("Hoh");
            app.SelectedRight.Name.Should().Be("MegaGra");
        }
    }
}
