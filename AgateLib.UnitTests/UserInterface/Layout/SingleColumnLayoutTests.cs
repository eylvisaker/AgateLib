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
    public class SingleColumnLayoutTests : ListLayoutTests
    {
        SingleColumnLayout layout = new SingleColumnLayout();

        public SingleColumnLayoutTests()
        {
        }

        [Fact]
        public void SCL_SizeMetrics()
        {
            TestSizeMetrics(new Size(100, 200));
        }

        [Fact]
        public void SCL_SizeMetricsWithHiddenItems()
        {
            for (int i = 0; i < 10; i += 2)
                layout[i].Display.IsVisible = false;

            TestSizeMetrics(new Size(100, 100));
        }

        [Fact]
        public void SCL_SizeMetricsWithMargins()
        {
            for (int i = 0; i < 10; i += 2)
            {
                layout[i].Display.Region.Style.Margin
                    = new LayoutBox(1, 2, 3, 4);
            }

            TestSizeMetrics(new Size(104, 230));
        }

        [Fact]
        public void SCL_UpdateLayoutWithHiddenItems()
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

                dest.Y += 20 + widgetRegion.MarginToContentOffset.Height;
            }
        }

        [Fact]
        public void SCL_UpdateLayoutWithMarginAndPadding()
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

            for(int i = 0; i < 10; i++)
            {
                var widgetRegion = layout[i].Display.Region;
                var thisDest = dest;

                thisDest.X += widgetRegion.MarginToContentOffset.Left;
                thisDest.Y += widgetRegion.MarginToContentOffset.Top;

                widgetRegion.ContentRect.Location.Should().Be(thisDest);

                dest.Y += 20 + widgetRegion.MarginToContentOffset.Height;
            }
        }

        protected override IListLayout ListLayout => layout;

        protected override bool EnableWrapping
        {
            get => layout.TopBottomWrap;
            set => layout.TopBottomWrap = value;
        }

        protected override void NextItem()
        {
            Send(MenuInputButton.Down);
        }

        protected override void PreviousItem()
        {
            Send(MenuInputButton.Up);
        }

    }
}
