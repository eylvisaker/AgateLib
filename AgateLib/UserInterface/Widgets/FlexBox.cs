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
        #region --- Axis Interfaces ---

        abstract class AxisSpace
        {
            protected abstract int AxisSize(Size itemSize);
            protected abstract int CrossSize(Size itemSize);
            protected abstract Size SetCrossSize(Size itemSize, int amount);
            protected abstract Size SetSize(int axisSize, int crossSize);

            protected abstract Point AdvanceAxis(Point dest, int amount);
            protected abstract Point AdvanceCross(Point dest, int amount);

            public void PerformLayout(
                IWidgetRenderContext renderContext,
                Size size,
                IRenderElementStyle style,
                IEnumerable<IRenderElement> children)
            {
                Point dest = new Point(0, 0);

                foreach (var item in children)
                {
                    if (!item.Display.IsVisible)
                        continue;

                    item.Display.ParentFont = style.Font;

                    var itemDest = dest;

                    CalcItemIdealSize(renderContext, size, item);

                    var contentSize = item.Display.Region.Size.ComputeContentSize();
                    var marginSize = new Size(contentSize.Width + item.Display.Region.MarginToContentOffset.Width,
                                              contentSize.Height + item.Display.Region.MarginToContentOffset.Height);

                    switch (style.Flex?.AlignItems ?? AlignItems.Stretch)
                    {
                        case AlignItems.End:
                            itemDest = AdvanceCross(itemDest, CrossSize(size) - CrossSize(marginSize));
                            break;

                        case AlignItems.Center:
                            itemDest = AdvanceCross(itemDest, (CrossSize(size) - CrossSize(marginSize)) / 2);
                            break;

                        case AlignItems.Stretch:
                            marginSize = SetCrossSize(marginSize, CrossSize(size));
                            break;
                    }

                    item.Display.MarginRect = new Rectangle(itemDest, marginSize);
                    item.DoLayout(renderContext, item.Display.ContentRect.Size);

                    dest = AdvanceAxis(dest, AxisSize(item.Display.MarginRect.Size));
                }
            }

            public Size CalcIdealSize(IWidgetRenderContext renderContext, Size maxSize, List<IRenderElement> children)
            {
                int idealCrossSize = 0;
                int idealAxisSize = 0;

                foreach (var item in children)
                {
                    if (!item.Display.IsVisible)
                        continue;

                    var itemBox = item.Display.Region.MarginToContentOffset;

                    var itemMaxSize = LayoutMath.ItemContentMaxSize(itemBox, maxSize);

                    item.Display.Region.Size.ParentMaxSize = maxSize;

                    item.Display.Region.Size.IdealContentSize
                        = item.CalcIdealContentSize(renderContext, maxSize);

                    var itemIdealContentSize = item.Display.Region.Size.IdealContentSize;
                    var itemIdealMarginSize = itemBox.Expand(itemIdealContentSize);

                    idealCrossSize = Math.Max(idealCrossSize, CrossSize(itemIdealMarginSize));
                    idealAxisSize += AxisSize(itemIdealMarginSize);
                }

                return SetSize(idealAxisSize, idealCrossSize);
            }

            private static void CalcItemIdealSize(IWidgetRenderContext renderContext, Size maxSize, IRenderElement item)
            {
                var idealSize = item.CalcIdealContentSize(renderContext, maxSize);
                item.Display.Region.Size.IdealContentSize = idealSize;
            }
        }

        class HorizontalAxisSpace : AxisSpace
        {
            protected override Size SetSize(int axisSize, int crossSize)
            {
                return new Size(axisSize, crossSize);
            }

            protected override int AxisSize(Size itemSize)
            {
                return itemSize.Width;
            }
            protected override int CrossSize(Size itemSize)
            {
                return itemSize.Height;
            }
            protected override Size SetCrossSize(Size itemSize, int amount)
            {
                itemSize.Height = amount;
                return itemSize;
            }

            protected override Point AdvanceAxis(Point dest, int amount)
            {
                dest.X += amount;
                return dest;
            }

            protected override Point AdvanceCross(Point dest, int amount)
            {
                dest.Y += amount;
                return dest;
            }
        }

        class VerticalAxisSpace : AxisSpace
        {
            protected override Size SetSize(int axisSize, int crossSize)
            {
                return new Size(crossSize, axisSize);
            }

            protected override int AxisSize(Size itemSize)
            {
                return itemSize.Height;
            }
            protected override int CrossSize(Size itemSize)
            {
                return itemSize.Width;
            }
            protected override Size SetCrossSize(Size itemSize, int amount)
            {
                itemSize.Width = amount;
                return itemSize;
            }

            protected override Point AdvanceAxis(Point dest, int amount)
            {
                dest.Y += amount;
                return dest;
            }

            protected override Point AdvanceCross(Point dest, int amount)
            {
                dest.X += amount;
                return dest;
            }
        }

        static AxisSpace horizontal = new HorizontalAxisSpace();
        static AxisSpace vertical = new VerticalAxisSpace();

        #endregion

        private readonly List<IRenderElement> children;

        public FlexBox(FlexBoxProps props) : base(props)
        {
            children = props.Children.Select(c => c.Finalize()).ToList();
        }

        public FlexDirection Direction => Style.Flex?.Direction ?? FlexDirection.Column;

        public override string StyleTypeId => Props.StyleTypeId;

        public override IReadOnlyList<IRenderElement> Children => children;

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
                    return vertical.CalcIdealSize(renderContext, maxSize, children);

                case FlexDirection.Row:
                case FlexDirection.RowReverse:
                    return horizontal.CalcIdealSize(renderContext, maxSize, children);

                default:
                    throw new InvalidOperationException();
            }
        }


        public override void Draw(IWidgetRenderContext renderContext, Rectangle clientArea)
        {
            DrawChildren(renderContext, clientArea);
        }

        private void PerformLayout(IWidgetRenderContext renderContext, Size size)
        {
            switch (Direction)
            {
                case FlexDirection.Column:
                    vertical.PerformLayout(renderContext, size, Style, Children);
                    break;

                case FlexDirection.ColumnReverse:
                    vertical.PerformLayout(renderContext, size, Style, Children.Reverse());
                    break;

                case FlexDirection.Row:
                    horizontal.PerformLayout(renderContext, size, Style, Children);
                    break;

                case FlexDirection.RowReverse:
                    horizontal.PerformLayout(renderContext, size, Style, Children.Reverse());
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        public override void OnChildNavigate(IRenderElement child, MenuInputButton button)
        {
            var index = IndexOf(child);
            var newIndex = index;

            if (Direction == FlexDirection.Column || Direction == FlexDirection.ColumnReverse)
            {
                if (button == MenuInputButton.Up)
                {
                    do
                    {
                        newIndex--;
                    } while (newIndex >= 0 && !Children[newIndex].CanHaveFocus);
                }
                if (button == MenuInputButton.Down)
                {
                    do
                    {
                        newIndex++;
                    } while (newIndex < Children.Count && !Children[newIndex].CanHaveFocus);
                }
            }
            else
            {
                if (button == MenuInputButton.Left)
                {
                    do
                    {
                        newIndex--;
                    } while (newIndex >= 0 && !Children[newIndex].CanHaveFocus);
                }
                if (button == MenuInputButton.Right)
                {
                    do
                    {
                        newIndex++;
                    } while (newIndex < Children.Count && !Children[newIndex].CanHaveFocus);
                }
            }

            if (newIndex == index || newIndex < 0 || newIndex >= Children.Count)
            {
                base.OnChildNavigate(this, button);
            }
            else
            {
                Display.System.Focus = Children[newIndex];
            }
        }

        private int IndexOf(IRenderElement child)
        {
            return Children.ToList().IndexOf(child);
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
