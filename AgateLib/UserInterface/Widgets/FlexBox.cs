//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

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
            children = props.Children.Select(c => c.Finalize()).ToList();
        }

        public override string StyleTypeId => Props.StyleTypeId;

        public override IEnumerable<IRenderElement> Children => children;

        public override void DoLayout(IWidgetRenderContext renderContext, Size size)
        {
            PerformLayout(renderContext, size);
        }

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
            //PerformLayout(renderContext, clientArea);

            DrawChildren(renderContext, clientArea);
        }

        private void PerformLayout(IWidgetRenderContext renderContext, Size size)
        {
            switch (Direction)
            {
                case FlexDirection.Column:
                    PerformColumnLayout(renderContext, size, Children);
                    break;

                case FlexDirection.ColumnReverse:
                    PerformColumnLayout(renderContext, size, Children.Reverse());
                    break;

                case FlexDirection.Row:
                    PerformRowLayout(renderContext, size, Children);
                    break;

                case FlexDirection.RowReverse:
                    PerformRowLayout(renderContext, size, Children.Reverse());
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        private void PerformRowLayout(IWidgetRenderContext renderContext, Size size, IEnumerable<IRenderElement> children)
        {
            throw new NotImplementedException();
        }

        private void PerformColumnLayout(IWidgetRenderContext renderContext, Size size, IEnumerable<IRenderElement> children)
        {
            Point dest = new Point(0, 0);

            foreach (var item in children)
            {
                if (!item.Display.IsVisible)
                    continue;

                item.Display.ParentFont = Style.Font;

                var itemDest = dest;

                CalcIdealSize(renderContext, size, item);

                var contentSize = item.Display.Region.Size.ComputeContentSize();
                var marginSize = new Size(contentSize.Width + item.Display.Region.MarginToContentOffset.Width,
                                          contentSize.Height + item.Display.Region.MarginToContentOffset.Height);

                switch (Style.Flex?.AlignItems ?? AlignItems.Start)
                {
                    case AlignItems.End:
                        itemDest.X += size.Width - marginSize.Width;
                        break;

                    case AlignItems.Center:
                        itemDest.X += (size.Width - marginSize.Width) / 2;
                        break;

                    case AlignItems.Stretch:
                        marginSize.Width = size.Width;
                        break;
                }

                item.Display.MarginRect = new Rectangle(itemDest, marginSize);
                item.DoLayout(renderContext, item.Display.ContentRect.Size);

                dest.Y += item.Display.MarginRect.Height;
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
        public string StyleTypeId { get; set; }
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
