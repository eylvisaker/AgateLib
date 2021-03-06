﻿using AgateLib.UserInterface.Styling.Themes.Model;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Moq;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace AgateLib.UserInterface.Styling.Themes
{
    public class ThemeDeserializationUnitTests
    {
        private FontProvider fonts;
        private Mock<IContentProvider> content;
        private ThemeLoader loader;

        public ThemeDeserializationUnitTests()
        {
            fonts = new FontProvider();
            content = new Mock<IContentProvider>();

            loader = new ThemeLoader(fonts, content.Object);
        }

        [Fact]
        public void TD_BorderBackgroundSimpleImage()
        {
            var theme = Parse(@"---
- selector: window
  border:
    image: imageFile
  background: 
    image: imageFile2  
");

            var item = theme.Styles.First();

            item.Border.Image.File.Should().Be("imageFile");
            item.Border.Image.SourceRect.Should().BeNull();

            item.Background.Image.File.Should().Be("imageFile2");
            item.Background.Image.SourceRect.Should().BeNull();
        }

        [Fact]
        public void TD_BorderBackgroundImageWithSourceRect()
        {
            var theme = Parse(@"---
- selector: window
  border:
    image: imageFile:rect(2 4 8 6)
  background: 
    image: imageFile2:rect(10 12 40 50)
");

            var item = theme.Styles.First();

            item.Border.Image.File.Should().Be("imageFile");
            item.Border.Image.SourceRect.Should().Be(new Rectangle(2, 4, 8, 6));

            item.Background.Image.File.Should().Be("imageFile2");
            item.Background.Image.SourceRect.Should().Be(new Rectangle(10, 12, 40, 50));
        }

        private ThemeModel Parse(string yamlTheme)
        {
            content.Setup(x => x.Open("default.atheme"))
                .Returns(new MemoryStream(Encoding.UTF8.GetBytes(yamlTheme)));

            return loader.LoadTheme("default").Model;
        }
    }
}
