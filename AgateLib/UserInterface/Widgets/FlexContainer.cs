using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public class FlexContainer : RenderElement<FlexContainerProps>
    {
        private readonly List<IRenderElement> children;
        FlexDirection Direction = FlexDirection.Column;

        public FlexContainer(FlexContainerProps props) : base(props)
        {
            children = props.Children.Select(c => c.Render()).ToList();
        }


        public override IEnumerable<IRenderElement> Children => children;

        public override Size ComputeIdealSize(IWidgetRenderContext renderContext, Size maxSize)
        {
            switch(Direction)
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

            foreach(var item in Children)
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

            int itemWidth = clientArea.Width;

            foreach(var item in children)
            {
                if (!item.Display.IsVisible)
                    continue;

                item.Display.ParentFont = Style.Font;

                var itemDest = dest;
                var marginToContent = item.Display.Region.MarginToContentOffset;
                var idealSize = item.ComputeIdealSize(renderContext, clientArea.Size);

                itemDest.X += marginToContent.Left;
                itemDest.Y += marginToContent.Top;

                item.Display.Region.Position = itemDest;
                var contentSize = item.Display.Region.Size.ComputeContentSize();

                contentSize.Width = itemWidth - item.Display.Region.MarginToContentOffset.Width;
                item.Display.Region.ContentSize = contentSize;

                dest.Y += item.Display.Region.MarginRect.Height;

                item.Display.HasFocus = item == Focus;
            }
        }
    }

    public class FlexContainerProps : RenderElementProps
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
