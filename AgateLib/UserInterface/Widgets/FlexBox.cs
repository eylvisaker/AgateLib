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
    /// <summary>
    /// Render element that can be used for automatic positioning of child elements
    /// based on a flexible flow layout.
    /// </summary>
    public class FlexBox : RenderElement<FlexBoxProps>
    {
        #region --- Axis Interfaces ---

        abstract class AxisSpace
        {
            protected abstract int MainAxis(Point itemSize);
            protected abstract int CrossAxis(Point itemSize);
            protected abstract Size SetCrossSize(Size itemSize, int amount);
            protected abstract Size SetSize(int axisSize, int crossSize);

            protected abstract Point IncMain(Point dest, int amount);
            protected abstract Point IncCross(Point dest, int amount);

            protected Rectangle IncMain(Rectangle dest, int amount)
            {
                var loc = IncMain(dest.Location, amount);
                return new Rectangle(loc, dest.Size);
            }

            protected Rectangle IncCross(Rectangle dest, int amount)
            {
                var loc = IncCross(dest.Location, amount);
                return new Rectangle(loc, dest.Size);
            }

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

                    switch (style.Flex?.AlignItems ?? AlignItems.Default)
                    {
                        case AlignItems.End:
                            itemDest = IncCross(itemDest, CrossAxis(size) - CrossAxis(marginSize));
                            break;

                        case AlignItems.Center:
                            itemDest = IncCross(itemDest, (CrossAxis(size) - CrossAxis(marginSize)) / 2);
                            break;

                        case AlignItems.Stretch:
                            marginSize = SetCrossSize(marginSize, CrossAxis(size));
                            break;
                    }

                    item.Display.MarginRect = new Rectangle(itemDest, marginSize);
                    item.DoLayout(renderContext, item.Display.ContentRect.Size);

                    dest = IncMain(dest, MainAxis((Size)item.Display.MarginRect.Size));
                }

                var extraSpace = MainAxis(size) - MainAxis(dest);

                switch (style.Flex?.JustifyContent ?? JustifyContent.Default)
                {
                    case JustifyContent.Center:
                        foreach (var item in children)
                        {
                            item.Display.MarginRect = IncMain(item.Display.MarginRect, extraSpace / 2);
                        }
                        break;

                    case JustifyContent.End:
                        foreach (var item in children)
                        {
                            item.Display.MarginRect = IncMain(item.Display.MarginRect, extraSpace);
                        }
                        break;

                    case JustifyContent.SpaceBetween:
                        DistributeSpaceBetween(children, extraSpace);
                        break;

                    case JustifyContent.SpaceAround:
                        DistributeSpaceAround(children, extraSpace);
                        break;

                    case JustifyContent.SpaceEvenly:
                        DistributeSpaceEvenly(children, extraSpace);
                        break;
                }
            }

            private void DistributeSpaceBetween(IEnumerable<IRenderElement> children, int extraSpace)
            {
                int betweenSpaces = children.Count() - 1;
                float step = extraSpace / (float)betweenSpaces;
                int index = 0;

                foreach (var item in children)
                {
                    item.Display.MarginRect = IncMain(item.Display.MarginRect, (int)(step * index));
                    index++;
                }
            }

            private void DistributeSpaceAround(IEnumerable<IRenderElement> children, int extraSpace)
            {
                int betweenSpaces = children.Count() * 2;
                float step = extraSpace / (float)betweenSpaces;
                int index = 1;

                foreach (var item in children)
                {
                    item.Display.MarginRect = IncMain(item.Display.MarginRect, (int)(step * index));
                    index += 2;
                }
            }

            private void DistributeSpaceEvenly(IEnumerable<IRenderElement> children, int extraSpace)
            {
                int betweenSpaces = children.Count() + 1;
                float step = extraSpace / (float)betweenSpaces;
                int index = 1;

                foreach (var item in children)
                {
                    item.Display.MarginRect = IncMain(item.Display.MarginRect, (int)(step * index));
                    index++;
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

                    idealCrossSize = Math.Max(idealCrossSize, CrossAxis(itemIdealMarginSize));
                    idealAxisSize += MainAxis(itemIdealMarginSize);
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

            protected override int MainAxis(Point itemSize)
            {
                return itemSize.X;
            }
            protected override int CrossAxis(Point itemSize)
            {
                return itemSize.Y;
            }
            protected override Size SetCrossSize(Size itemSize, int amount)
            {
                itemSize.Height = amount;
                return itemSize;
            }

            protected override Point IncMain(Point dest, int amount)
            {
                dest.X += amount;
                return dest;
            }

            protected override Point IncCross(Point dest, int amount)
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

            protected override int MainAxis(Point itemSize)
            {
                return itemSize.Y;
            }
            protected override int CrossAxis(Point itemSize)
            {
                return itemSize.X;
            }
            protected override Size SetCrossSize(Size itemSize, int amount)
            {
                itemSize.Width = amount;
                return itemSize;
            }

            protected override Point IncMain(Point dest, int amount)
            {
                dest.Y += amount;
                return dest;
            }

            protected override Point IncCross(Point dest, int amount)
            {
                dest.X += amount;
                return dest;
            }
        }

        static AxisSpace horizontal = new HorizontalAxisSpace();
        static AxisSpace vertical = new VerticalAxisSpace();

        #endregion

        private readonly List<IRenderElement> children;
        private readonly List<IRenderElement> focusChildren;
        private int focusIndex;

        public FlexBox(FlexBoxProps props) : base(props)
        {
            children = Finalize(props.Children).ToList();
            focusChildren = children.Where(x => CanChildHaveFocus(x)).ToList();
        }

        public FlexDirection Direction => Style.Flex?.Direction ?? FlexDirection.Column;

        public int SelectedIndex => focusIndex;

        public override string StyleTypeId => Props.StyleTypeId ?? "flexbox";

        public override bool CanHaveFocus => Children.Any(c => CanChildHaveFocus(c));

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

        public override void OnFocus()
        {
            Display.System.Focus = focusChildren[focusIndex];
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
            var index = focusChildren.IndexOf(child);
            var newIndex = index;

            if (Direction == FlexDirection.Column || Direction == FlexDirection.ColumnReverse)
            {
                if (button == MenuInputButton.Up)
                {
                    do
                    {
                        newIndex--;
                    } while (newIndex >= 0 && !CanChildHaveFocus(focusChildren[newIndex]));
                }
                if (button == MenuInputButton.Down)
                {
                    do
                    {
                        newIndex++;
                    } while (newIndex < focusChildren.Count && !CanChildHaveFocus(focusChildren[newIndex]));
                }
            }
            else
            {
                if (button == MenuInputButton.Left)
                {
                    do
                    {
                        newIndex--;
                    } while (newIndex >= 0 && !CanChildHaveFocus(focusChildren[newIndex]));
                }
                if (button == MenuInputButton.Right)
                {
                    do
                    {
                        newIndex++;
                    } while (newIndex < focusChildren.Count && !CanChildHaveFocus(focusChildren[newIndex]));
                }
            }

            if (newIndex == index || newIndex < 0 || newIndex >= focusChildren.Count)
            {
                base.OnChildNavigate(this, button);
            }
            else
            {
                focusIndex = newIndex;
                Display.System.Focus = focusChildren[newIndex];
            }
        }

        private bool CanChildHaveFocus(IRenderElement renderElement)
        {
            return renderElement.Display.IsVisible && renderElement.CanHaveFocus;
        }
    }

    public class FlexBoxProps : RenderElementProps
    {
        public FlexBoxProps()
        {
            DefaultStyle = new InlineElementStyle
            {
                Flex = new FlexStyle
                {
                    AlignItems = AlignItems.Stretch,
                    Direction = FlexDirection.Column
                }
            };
        }

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
