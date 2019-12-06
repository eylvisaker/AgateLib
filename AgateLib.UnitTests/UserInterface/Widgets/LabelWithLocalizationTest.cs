using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using AgateLib.Tests;
using AgateLib.Tests.Fakes;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Content;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AgateLib.UserInterface.Widgets
{
    public class LabelWithLocalizationTest
    {
        private class TextRepository : ITextRepository
        {
            public Dictionary<string, string> LookupValues { get; set; }
                = new Dictionary<string, string>();

            public string Lookup(string key)
            {
                if (LookupValues.TryGetValue(key, out string value))
                    return value;

                return key;
            }
        }

        private readonly FakeFontCore fontCore;
        private readonly Font font;
        private readonly FontProvider fontProvider;
        private readonly TextRepository textRepo;

        private readonly IContentLayoutEngine contentLayout;
        private readonly Mock<ICanvas> canvas;
        private readonly Mock<IUserInterfaceRenderContext> context;
        private readonly Mock<IDisplaySystem> displaySystem;

        public LabelWithLocalizationTest()
        {
            fontCore = new FakeFontCore("default");
            font = new Font(fontCore);

            fontProvider = new FontProvider();
            fontProvider.Add("default", font);

            textRepo = new TextRepository();

            contentLayout = new LocalizedContentLayoutEngine(
                new ContentLayoutEngine(fontProvider), textRepo);

            canvas = new Mock<ICanvas>();

            context = CommonMocks.RenderContext(contentLayout, canvas.Object);

            canvas.Setup(x => x.Draw(It.IsAny<IContentLayout>(), It.IsAny<Vector2>()))
                .Callback<IContentLayout, Vector2>((content, dest) =>
                {
                    content.Draw(dest);
                });

            displaySystem = CommonMocks.DisplaySystem(fontProvider);
        }

        [Fact]
        public void LabelBasicTextNoWrap()
        {
            string text = "These aren't the droids you're looking for.";
            string textKey = "droids";
            textRepo.LookupValues.Add(textKey, text);

            int expectedWidth = 215;
            int expectedHeight = 10;

            var label = new Label(new LabelProps { Text = textKey }) { AppContext = new UserInterfaceAppContext() };
            var labelElement = (LabelElement)label.FinalizeRendering(null);

            labelElement.Display.System = displaySystem.Object;
            labelElement.Style.Update();

            Size idealSize = labelElement.CalcIdealContentSize(context.Object, new Size(1000, 1000));
            labelElement.Draw(context.Object, new Rectangle(40, 60, 1000, 1000));

            labelElement.Props.Text.Should().Be(textKey);

            idealSize.Width.Should().Be(expectedWidth);
            idealSize.Height.Should().Be(expectedHeight);

            fontCore.DrawCalls.Count.Should().Be(7);

            string[] words = text.Split(' ');
            Vector2 dest = new Vector2(40, 60);

            for (int i = 0; i < words.Length; i++)
            {
                var drawCall = fontCore.DrawCalls[i];

                drawCall.Text.Should().Be(words[i]);
                drawCall.Parameters.Should().Match(p => p == null || p.Count() == 0);
                drawCall.Color.Should().Be(Color.White);
                drawCall.Dest.Should().Be(dest, $"word index is {i}");

                // add 1 for the space after the word.
                dest.X += 5 * (1 + words[i].Length);
            }
        }
        [Fact]
        public void LabelBasicTextWithCarriageReturn()
        {
            string text = "This has a carriage\nreturn.";
            string textKey = "ret";
            textRepo.LookupValues.Add(textKey, text);

            int expectedWidth = 95;
            int expectedHeight = 20;

            var label = new Label(new LabelProps { Text = textKey }) { AppContext = new UserInterfaceAppContext() };
            var labelElement = (LabelElement)label.FinalizeRendering(null);
            labelElement.Display.System = displaySystem.Object;
            labelElement.Style.Update();

            Size idealSize = labelElement.CalcIdealContentSize(context.Object, new Size(1000, 1000));
            labelElement.Draw(context.Object, new Rectangle(40, 60, 1000, 1000));

            labelElement.Props.Text.Should().Be(textKey);

            idealSize.Width.Should().Be(expectedWidth);
            idealSize.Height.Should().Be(expectedHeight);

            fontCore.DrawCalls.Count.Should().Be(5);
            var firstDraw = fontCore.DrawCalls[0];
            var secondDraw = fontCore.DrawCalls[4];

            firstDraw.Text.Should().Be("This");
            firstDraw.Parameters.Should().Match(p => p == null || p.Count() == 0);
            firstDraw.Color.Should().Be(Color.White);
            firstDraw.Dest.Should().Be(new Vector2(40, 60));

            secondDraw.Text.Should().Be("return.");
            secondDraw.Parameters.Should().Match(p => p == null || p.Count() == 0);
            secondDraw.Color.Should().Be(Color.White);
            secondDraw.Dest.Should().Be(new Vector2(40, 70));
        }
    }
}
