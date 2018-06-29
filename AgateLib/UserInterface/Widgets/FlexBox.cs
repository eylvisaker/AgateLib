using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public class FlexBox : RenderElement<FlexBoxProps>
    {
        private readonly List<IRenderElement> children;
        FlexDirection Direction = FlexDirection.Column;

        public FlexBox(FlexBoxProps props) : base(props)
        {
            children = props.Children.Select(c => c.Render()).ToList();
        }


        public override IEnumerable<IRenderElement> Children => children;

        public override Size CalcIdealContentSize(IWidgetRenderContext renderContext, Size maxSize)
        {
            switch (Direction)
            {
                case FlexDirection.Column:
                case FlexDirection.ColumnReverse:
                    return ComputeIdealSizeColumn(renderContext, maxSize);

                case FlexDirection.Row:
                case FlexDirection.RowReverse:
                    return ComputeIdealSizeRow(renderContext, maxSize);

                default:
                    throw new InvalidOperationException();
            }
        }

        private Size ComputeIdealSizeRow(IWidgetRenderContext renderContext, Size maxSize)
        {
            throw new NotImplementedException();
        }

        private Size ComputeIdealSizeColumn(IWidgetRenderContext renderContext, Size maxSize)
        {
            int idealWidth = 0;
            int idealHeight = 0;

            foreach (var item in Children)
            {
                if (!item.Display.IsVisible)
                    continue;

                var itemBox = item.Display.Region.MarginToContentOffset;

                var itemMaxSize =
                    LayoutMath.ItemContentMaxSize(itemBox, maxSize);

                item.RecalculateSize(renderContext, maxSize);

                var itemIdealContentSize = item.Display.Region.Size.IdealContentSize;

                idealWidth = Math.Max(idealWidth, itemIdealContentSize.Width + itemBox.Width);

                idealHeight += itemIdealContentSize.Height + itemBox.Height;
            }

            return new Size(idealWidth, idealHeight);
        }

        public override void Draw(IWidgetRenderContext renderContext, Rectangle clientArea)
        {
            PerformLayout(renderContext, clientArea);

            DrawChildren(renderContext, clientArea);
        }

        private void PerformLayout(IWidgetRenderContext renderContext, Rectangle clientArea)
        {
            switch (Direction)
            {
                case FlexDirection.Column:
                    PerformColumnLayout(renderContext, clientArea, Children);
                    break;

                case FlexDirection.ColumnReverse:
                    PerformColumnLayout(renderContext, clientArea, Children.Reverse());
                    break;

                case FlexDirection.Row:
                    PerformRowLayout(renderContext, clientArea, Children);
                    break;

                case FlexDirection.RowReverse:
                    PerformRowLayout(renderContext, clientArea, Children.Reverse());
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        private void PerformRowLayout(IWidgetRenderContext renderContext, Rectangle clientArea, IEnumerable<IRenderElement> children)
        {
            throw new NotImplementedException();
        }

        private void PerformColumnLayout(IWidgetRenderContext renderContext, Rectangle clientArea, IEnumerable<IRenderElement> children)
        {
            Point dest = new Point(0, 0);

            foreach (var item in children)
            {
                if (!item.Display.IsVisible)
                    continue;

                item.Display.ParentFont = Style.Font;

                var itemDest = dest;
                var marginToContent = item.Display.Region.MarginToContentOffset;

                CalcIdealSize(renderContext, clientArea.Size, item);

                itemDest.X += marginToContent.Left;
                itemDest.Y += marginToContent.Top;

                var contentSize = item.Display.Region.Size.ComputeContentSize();

                int itemWidth = contentSize.Width;
                int left = 0;

                switch (Style.Flex?.AlignItems ?? AlignItems.Start)
                {
                    case AlignItems.End:
                        left = clientArea.Width - contentSize.Width - marginToContent.Width;
                        break;

                    case AlignItems.Center:
                        left = (clientArea.Width - contentSize.Width - marginToContent.Width) / 2;
                        break;

                    case AlignItems.Stretch:
                        itemWidth = clientArea.Width;
                        break;
                }

                itemDest.X += left;

                contentSize.Width = itemWidth - item.Display.Region.MarginToContentOffset.Width;

                item.Display.ContentRect = new Rectangle(itemDest, contentSize);

                dest.Y += item.Display.Region.MarginRect.Height;
            }
        }

        private static void CalcIdealSize(IWidgetRenderContext renderContext, Size maxSize, IRenderElement item)
        {
            var idealSize = item.CalcIdealContentSize(renderContext, maxSize);
            item.Display.Region.Size.IdealContentSize = idealSize;
        }
    }

    public class FlexBoxProps : RenderElementProps
    {
        public IList<IRenderable> Children { get; set; } = new List<IRenderable>();
    }

    public enum FlexDirection
    {
        /// <summary>
        /// Top to Bottom
        /// </summary>
        Column,

        /// <summary>
        /// Left to Right 
        /// </summary>
        Row,

        /// <summary>
        /// Bottom to Top
        /// </summary>
        ColumnReverse,

        /// <summary>
        /// Right to Left
        /// </summary>
        RowReverse,
    }
}
