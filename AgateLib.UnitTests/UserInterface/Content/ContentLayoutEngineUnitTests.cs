using AgateLib.Mathematics.Geometry;
using AgateLib.Tests;
using AgateLib.UserInterface.Content.LayoutItems;
using FluentAssertions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AgateLib.UserInterface.Content
{
    public class ContentLayoutEngineUnitTests
    {
        [Fact]
        public void LayoutTextWithBackTicks()
        {
            var text = @"Enemies with shields can block your attacks.
Try crouching with `Color Yellow`Down`Color White` and slashing with `Color Yellow`Sword`Color White` to beat enemies with shields.
Be careful! Some enemies may anticipate this!";
            var displayedText = @"Enemies with shields can block your attacks.
Try crouching with Down and slashing with Sword to beat enemies with shields.
Be careful! Some enemies may anticipate this!";

            var fontProvider = CommonMocks.FontProvider("temp");

            ContentLayoutEngine layoutEngine = new ContentLayoutEngine(fontProvider.Object)
            {
                CommandStart = '`',
                CommandEnd = '`',
            };

            var result = (ContentLayout)layoutEngine.LayoutContent(text);

            result.MaxWidth = 200;
            result.Draw(Vector2.Zero);

            var items = result.Items.Cast<ContentText>().ToList();
            var coloredItems = new int[] { 10, 14 };

            ValidateDisplayedText(items, displayedText);
            ValidateColoredText(items, coloredItems);
        }

        [Fact]
        public void LayoutTextWithBraces()
        {
            var text = @"Enemies with shields can block your attacks.
Try crouching with {Color Yellow}Down{Color White} and slashing with {Color Yellow}Sword{Color White} to beat enemies with shields.
Be careful! Some enemies may anticipate this!";
            var displayedText = @"Enemies with shields can block your attacks.
Try crouching with Down and slashing with Sword to beat enemies with shields.
Be careful! Some enemies may anticipate this!";

            var fontProvider = CommonMocks.FontProvider("temp");

            ContentLayoutEngine layoutEngine = new ContentLayoutEngine(fontProvider.Object)
            {
                CommandStart = '{',
                CommandEnd = '}',
            };

            var result = (ContentLayout)layoutEngine.LayoutContent(text);

            result.MaxWidth = 200;
            result.Draw(Vector2.Zero);

            var items = result.Items.Cast<ContentText>().ToList();
            var coloredItems = new int[] { 10, 14 };

            ValidateDisplayedText(items, displayedText);
            ValidateColoredText(items, coloredItems);
        }

        private void ValidateDisplayedText(List<ContentText> items, string text)
        {
            int textIndex = 0;

            string FindNextWord()
            {
                bool whiteSpace = false;
                int start = textIndex;

                for (; textIndex < text.Length; textIndex++)
                {
                    if (" \r\n\t".Contains(text[textIndex]))
                    {
                        whiteSpace = true;
                    }
                    else if (whiteSpace)
                    {
                        return text.Substring(start, textIndex - start);
                    }
                }

                return text.Substring(start);
            }

            for (int i = 0; i < items.Count; i++)
            {
                string word = FindNextWord();
                string trimmedWord = word.Trim();

                int newLines = word.Count(x => x == '\n');
                int spaces = word.Count(x => x == ' ');

                items[i].Text.Should().Be(trimmedWord);
                items[i].Size.Should().Be(new Size(trimmedWord.Length * 5, 10));
                items[i].NewLinesAfter.Should().Be(newLines);
                items[i].ExtraWhiteSpace.Should().Be(5 * spaces);
            }
        }

        private static void ValidateColoredText(List<ContentText> items, int[] coloredItems)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (coloredItems.Contains(i))
                {
                    items[i].Font.Color.Should().Be(Color.Yellow);
                }
                else
                {
                    items[i].Font.Color.Should().Be(Color.White);
                }
            }
        }

        [Fact]
        public void TextWithDash()
        {
            var text = @"Weapon - Knife";

            var fontProvider = CommonMocks.FontProvider("temp");

            ContentLayoutEngine layoutEngine = new ContentLayoutEngine(fontProvider.Object)
            {
                CommandStart = '`',
                CommandEnd = '`',
            };

            var result = (ContentLayout)layoutEngine.LayoutContent(text);

            result.Items.Count.Should().Be(3);
        }

    }
}
