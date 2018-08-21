using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moq;
using AgateLib.Display;
using FluentAssertions;
using AgateLib.UserInterface.Content;

namespace AgateLib.Tests.UserInterface.Content
{
    public class ContentLayoutTest
    {
        [Fact]
        public void LayoutText()
        {
            var text = @"Enemies with shields can block your attacks.
Try crouching with {Color Yellow}Down{Color White} and slashing with {Color Yellow}Sword{Color White} to beat enemies with shields.
Be careful! Some enemies may anticipate this!";

            var fontProvider =  CommonMocks.FontProvider("temp");

            ContentLayoutEngine layoutEngine = new ContentLayoutEngine(fontProvider.Object);

            var result = (ContentLayout)layoutEngine.LayoutContent(text, 200);

            result.Draw(Vector2.Zero);

            var items = result.Items.Cast<ContentText>().ToList();

            ValidateItem(items, 0, new Vector2(0, 0), "Enemies with shields can block your");
            ValidateItem(items, 1, new Vector2(0, 10), "attacks.");
            ValidateItem(items, 2, new Vector2(0, 20), "Try crouching with ");
            ValidateItem(items, 3, new Vector2(95, 20), "Down");
            ValidateItem(items, 4, new Vector2(115, 20), " and slashing");
        }

        private void ValidateItem(IReadOnlyList<ContentText> items,
            int index, Vector2 location, string text)
        {
            var item = items[index];

            (location - item.Location).Length().Should().BeLessThan(1e-4f,
                $"Item {index}: Wrong position. Expected {location}, actual {item.Location}");
            item.Text.Should().Be(text);
        }
    }
}
