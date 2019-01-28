using AgateLib.Tests;
using AgateLib.UserInterface.Content.LayoutItems;
using FluentAssertions;
using Microsoft.Xna.Framework;
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

            foreach(var line in lines)
            {
                string[] words = line.Split(' ');

                int lineLength = line.Length * 5;
                expected.X = maxWidth - lineLength;

                foreach(var word in words)
                {
                    ContentText content = items[wordIndex];
                    wordIndex++;

                    content.Text.Should().Be(word);

                    content.Position.Y.Should().Be(expected.Y);
                    content.Position.X.Should().Be(expected.X);

                    // Extra +1 here to account for space after word.
                    expected.X += 5 * (word.Length + 1);
                }

                expected.Y += 10;
                expected.X = 0;
            }
        }
    }
}
