using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Widgets;
using FluentAssertions;
using Xunit;
using Microsoft.Xna.Framework;
using Moq;

namespace ManualTests.AgateLib.UserInterface.Layout
{
    public class SingleRowLayoutTests : ListLayoutTests
    {
        SingleRowLayout layout = new SingleRowLayout();

        [Fact]
        public void SRL_SizeMetrics()
        {
            TestSizeMetrics(new Size(1000, 20));
        }

        [Fact]
        public void SRL_SizeMetricsWithHiddenItems()
        {
            for (int i = 0; i < 10; i += 2)
                layout[i].Display.IsVisible = false;

            TestSizeMetrics(new Size(500, 20));
        }

        [Fact]
        public void SRL_SizeMetricsWithMargins()
        {
            for (int i = 0; i < 10; i += 2)
            {
                layout[i].Display.Region.Style.Margin 
                    = new LayoutBox(1, 2, 3, 4);
            }

            TestSizeMetrics(new Size(1020, 26));
        }

        [Fact]
        public void SRL_UpdateLayoutWithHiddenItems()
        {
            var renderContext = new Mock<IWidgetRenderContext>(MockBehavior.Strict);

            for (int i = 0; i < 10; i += 2)
            {
                layout[i].Display.IsVisible = false;
            }
            
            WidgetRegion region = new WidgetRegion(new WidgetStyle());

            region.Size.IdealContentSize = layout.ComputeIdealSize(
                region.Size.ParentMaxSize, renderContext.Object);

            layout.ApplyLayout(region.Size.IdealContentSize, renderContext.Object);

            var dest = new Point();

            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    continue;

                var widgetRegion = layout[i].Display.Region;
                var thisDest = dest;

                thisDest.X += widgetRegion.MarginToContentOffset.Left;
                thisDest.Y += widgetRegion.MarginToContentOffset.Top;

                widgetRegion.ContentRect.Location.Should().Be(thisDest, 
                    $"location test failed for item {i}");

                dest.X += 100 + widgetRegion.MarginToContentOffset.Width;
            }
        }

        [Fact]
        public void SRL_UpdateLayoutWithMarginAndPadding()
        {
            var renderContext = new Mock<IWidgetRenderContext>(MockBehavior.Strict);

            for (int i = 0; i < 10; i += 2)
            {
                layout[i].Display.Region.Style.Margin
                    = new LayoutBox(1, 2, 3, 4);
                layout[i].Display.Region.Style.Padding
                    = new LayoutBox(2, 4, 6, 8);
            }
            
            WidgetRegion region = new WidgetRegion(new WidgetStyle());
            
            region.Size.IdealContentSize = layout.ComputeIdealSize(
                region.Size.ParentMaxSize, renderContext.Object);

            layout.ApplyLayout(region.Size.IdealContentSize, renderContext.Object);

            var dest = new Point();

            for (int i = 0; i < 10; i++)
            {
                var widgetRegion = layout[i].Display.Region;
                var thisDest = dest;

                thisDest.X += widgetRegion.MarginToContentOffset.Left;
                thisDest.Y += widgetRegion.MarginToContentOffset.Top;

                widgetRegion.ContentRect.Location.Should().Be(thisDest);

                dest.X += 100 + widgetRegion.MarginToContentOffset.Width;
            }
        }

        protected override IListLayout ListLayout => layout;

        protected override bool EnableWrapping
        {
            get => layout.LeftRightWrap;
            set => layout.LeftRightWrap = value;
        }

        protected override void NextItem()
        {
            Send(MenuInputButton.Right);
        }

        protected override void PreviousItem()
        {
            Send(MenuInputButton.Left);
        }
    }
}
