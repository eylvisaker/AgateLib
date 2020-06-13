using AgateLib.Mathematics.Geometry;
using AgateLib.Tests;
using AgateLib.UserInterface.Content.Commands;
using AgateLib.UserInterface.Content.LayoutItems;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace AgateLib.UserInterface.Content
{
    public class ContentLayoutUnitTests
    {
        [Fact]
        public void LayoutTextAlignRight()
        {
            var text = @"Enemies with shields can block your attacks.
Try crouching with `Color Yellow`Down`Color White` and slashing with `Color Yellow`Sword`Color White` to beat enemies with shields.
Be careful! Some enemies may anticipate this!";

            var lines = new[] {
                "Enemies with shields can block your attacks.",
                "Try crouching with Down and slashing with Sword to beat enemies with shields.",
                "Be careful! Some enemies may anticipate this!",
            };

            var fontProvider = CommonMocks.FontProvider("temp");

            var layoutEngine = new ContentLayoutEngine(fontProvider.Object);
            var options = new ContentLayoutOptions();
            var result = (ContentLayout)layoutEngine.LayoutContent(text, options);

            const int maxWidth = 500;

            result.MaxWidth = maxWidth;
            result.TextAlign = TextAlign.Right;
            result.Draw(Vector2.Zero);

            var items = result.Items.Cast<ContentText>().ToList();

            int wordIndex = 0;
            Point expected = new Point();
            int lineIndex = 0;

            foreach (string line in lines)
            {
                lineIndex++;
                string[] words = line.Split(' ');

                int lineLength = line.Length * 5;
                expected.X = maxWidth - lineLength;

                foreach (var word in words)
                {
                    ContentText content = items[wordIndex];
                    wordIndex++;

                    content.Text.Should().Be(word);

                    content.Position.Y.Should().Be(expected.Y, $"word '{word}' in line {lineIndex} should be at {expected}");
                    content.Position.X.Should().Be(expected.X, $"word '{word}' in line {lineIndex} should be at {expected}");

                    // Extra +1 here to account for space after word.
                    expected.X += 5 * (word.Length + 1);
                }

                expected.Y += 10;
                expected.X = 0;
            }
        }

        [Theory]
        [InlineData(null, 52, 0)]
        [InlineData(250, 118, 65)]
        public void LayoutTextAndImageCentered(int? maxWidth, int expectedImageX, int textShift)
        {
            var fontProvider = CommonMocks.FontProvider("temp");

            var layoutEngine = new ContentLayoutEngine(fontProvider.Object);

            var textureCommand = new Mock<IContentCommand>();

            textureCommand
                .Setup(x => x.Execute(It.IsAny<LayoutContext>(), "south"))
                .Callback<LayoutContext, string>((context, arg) =>
                {
                    var item = new Mock<ContentLayoutItem>();
                    item.SetupAllProperties();

                    item.Setup(x => x.Size).Returns(context.ScaleToLineHeight(new Size(4, 4)));
                    item.Setup(x => x.Count).Returns(1);

                    context.Add(item.Object);
                })
                .Verifiable();

            layoutEngine.AddCommand("button", textureCommand.Object);

            var layoutOptions = new ContentLayoutOptions
            {
                FontLookup = new FontStyleProperties
                {
                    Size = 14,
                    Color = Color.White,
                }
            };

            string text = "`button south`\nSteel Short Sword";

            var result = (ContentLayout)layoutEngine.LayoutContent(text, layoutOptions);

            result.MaxWidth = maxWidth;
            result.TextAlign = TextAlign.Center;
            result.Draw(Vector2.Zero);

            result.Items.Count.Should().Be(4);
            result.Items[0].Bounds.Should().Be(new Rectangle(expectedImageX, 0, 14, 14));
            result.Items[1].Bounds.Should().Be(new Rectangle(textShift + 0, 14, 35, 14));
            result.Items[2].Bounds.Should().Be(new Rectangle(textShift + 42, 14, 35, 14));
            result.Items[3].Bounds.Should().Be(new Rectangle(textShift + 84, 14, 35, 14));

            textureCommand.Verify();
        }
    }
}
