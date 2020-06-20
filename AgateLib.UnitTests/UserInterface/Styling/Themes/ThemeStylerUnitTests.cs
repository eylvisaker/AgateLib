﻿using AgateLib.UserInterface;
using AgateLib.UserInterface.Styling.Themes;
using AgateLib.UserInterface.Styling.Themes.Model;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Tests.UserInterface.Styling.Themes
{
    public class ThemeStylerUnitTests
    {
        [Fact]
        public void ApplyDefaultTheme()
        {
            ThemeCollection themes = new ThemeCollection
            {
                ["default"] = Theme.CreateDefaultTheme(),
                ["xyz"] = CreateTestTheme(),
            };

            ThemeStyler styler = new ThemeStyler(themes);

            (var widget, var element) = CommonMocks.Widget("widget");

            styler.Apply(element.Object, "xyz");

            element.Object.Style.Update(1);
            element.Object.Display.Style.Padding.Left.Should().Be(14);
            element.Object.Style.Font.Color.Should().Be(Color.Yellow);
        }

        private ITheme CreateTestTheme()
        {
            var data = new ThemeData
            {
                new ThemeStyle
                {
                    Selector = "*",
                    Background = new BackgroundStyle { Color = Color.Blue, },
                    Padding = LayoutBox.SameAllAround(14),
                    Font = new FontStyleProperties { Color = Color.Yellow, },
                },

                new ThemeStyle
                {
                    Selector = "menuitem",
                    Padding = LayoutBox.SameAllAround(14),
                },

                new ThemeStyle
                {
                    Selector = "menuitem:selected",
                    Padding = LayoutBox.SameAllAround(14),
                    Background = new BackgroundStyle { Color = Color.White },
                    Font = new FontStyleProperties { Color = Color.Black }
                }
            };

            return new Theme(data);
        }
    }
}
