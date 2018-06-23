using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Styling.Themes.Model;
using FluentAssertions;
using Xunit;
using Microsoft.Xna.Framework;
using Moq;

namespace AgateLib.UserInterface.Styling.Themes
{
    public class ThemeStylerTest
    {
        [Fact]
        public void ApplyDefaultTheme()
        {
            var fontProvider = CommonMocks.FontProvider("default");
            ThemeCollection themes = new ThemeCollection
            {
                ["default"] = Theme.CreateDefaultTheme(),
                ["xyz"] = CreateTestTheme(),
            };

            ThemeStyler styler = new ThemeStyler(fontProvider.Object, themes);

            var widget = CommonMocks.Widget("widget");

            styler.ApplyStyle(widget.Object, "xyz");

            widget.Object.Display.Style.Padding.Left.Should().Be(14);
            widget.Object.Display.Font.Color.Should().Be(Color.Yellow);
        }

        private ITheme CreateTestTheme()
        {
            var data = new ThemeData
            {
                new ThemeStyle
                {
                    Pattern = "*",
                    Background = new ThemeWidgetBackground { Color = Color.Blue, },
                    Padding = LayoutBox.SameAllAround(14),
                    Font = new FontStyleProperties { Color = Color.Yellow, },
                },

                new ThemeStyle
                {
                    Pattern = "menu.*",
                    Padding = LayoutBox.SameAllAround(14),
                },

                new ThemeStyle
                {
                    Pattern = "menu.*",
                    WidgetState = "selected",
                    Padding = LayoutBox.SameAllAround(14),
                    Background = new ThemeWidgetBackground { Color = Color.White },
                    Font = new FontStyleProperties { Color = Color.Black }
                }
            };

            return new Theme(data);
        }
    }
}
