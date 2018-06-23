using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Widgets;
using FluentAssertions;
using Xunit;
using Microsoft.Xna.Framework;
using Moq;

namespace AgateLib.UserInterface.Layout
{
    public class FixedGridLayoutTests : LayoutTests
    {
        private FixedGridLayout layout;
        private SizeMetrics gridParentMetrics;
        private Mock<IWidgetRenderContext> renderContext;

        public FixedGridLayoutTests()
        {
            layout = new FixedGridLayout(16, 8);

            gridParentMetrics = new SizeMetrics();
            gridParentMetrics.ParentMaxSize = new Size(1920, 1080);

            renderContext = CommonMocks.RenderContext();
        }

        [Fact]
        public void FG_SizeMetricsPassedToChild()
        {
            var widget = CommonMocks.Widget("test");

            layout.Add(widget.Object, new Point(3, 2));

            Size actualParentMaxSize = new Size();

            widget.Setup(x => x.ComputeIdealSize(renderContext.Object, It.IsAny<Size>()))
                .Returns<IWidgetRenderContext, Size>((_, maxSize) =>
                {
                    actualParentMaxSize = maxSize;
                    return new Size(10, 10);
                });

            gridParentMetrics.IdealContentSize = layout.ComputeIdealSize(
                gridParentMetrics.ParentMaxSize, renderContext.Object);
            layout.ApplyLayout(new Size(1920, 1080), renderContext.Object);

            actualParentMaxSize.Width.Should().Be(1920 / 16);
            actualParentMaxSize.Height.Should().Be(1080 / 8);
            widget.Object.Display.Region.ContentSize.Width.Should().Be(1920 / 16);
            widget.Object.Display.Region.ContentSize.Height.Should().Be(1080 / 8);
        }

        [Fact]
        public void FG_ChildSizeWithMarginAndBorder()
        {
            var widget = CommonMocks.Widget("test");

            layout.Add(widget.Object, new Rectangle(0, 0, 16, 8));

            widget.Object.Display.Style.Margin = new LayoutBox(2, 4, 6, 8);
            widget.Object.Display.Style.Border.Left.Width = 1;
            widget.Object.Display.Style.Border.Top.Width = 3;
            widget.Object.Display.Style.Border.Right.Width = 5;
            widget.Object.Display.Style.Border.Bottom.Width = 7;

            Size actualParentMaxSize = new Size();

            widget.Setup(x => x.ComputeIdealSize(renderContext.Object, It.IsAny<Size>()))
                .Returns<IWidgetRenderContext, Size>((_, maxSize) =>
                {
                    actualParentMaxSize = maxSize;

                    return new Size(10, 10);
                });

            gridParentMetrics.IdealContentSize = layout.ComputeIdealSize(
                gridParentMetrics.ParentMaxSize, renderContext.Object);
            layout.ApplyLayout(new Size(1920, 1080), renderContext.Object);

            actualParentMaxSize.Width.Should().Be(1920 - 14);
            actualParentMaxSize.Height.Should().Be(1080 - 22);

            widget.Object.Display.Region.ContentSize.Width.Should().Be(1920 - 14);
            widget.Object.Display.Region.ContentSize.Height.Should().Be(1080 - 22);

            widget.Object.Display.Region.Position.X.Should().Be(3);
            widget.Object.Display.Region.Position.Y.Should().Be(7);
        }

        [Fact]
        public void FG_SizeMetricsPassedToLargeChild()
        {
            var widget = CommonMocks.Widget("test");

            layout.Add(widget.Object, new Point(4, 4), new Size(3, 2));

            Size actualParentMaxSize = new Size();

            widget.Setup(x => x.ComputeIdealSize(renderContext.Object, It.IsAny<Size>()))
                .Returns<IWidgetRenderContext, Size>((_, maxSize) =>
                {
                    actualParentMaxSize = maxSize;
                    return new Size(10, 10);
                });

            gridParentMetrics.IdealContentSize = layout.ComputeIdealSize(
                gridParentMetrics.ParentMaxSize, renderContext.Object);

            actualParentMaxSize.Width.Should().Be(1920 / 16 * 3);
            actualParentMaxSize.Height.Should().Be(1080 / 8 * 2);
        }

        protected override IWidgetLayout WidgetLayout => layout;
    }
}
