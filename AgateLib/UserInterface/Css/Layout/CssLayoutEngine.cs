//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Layout
{
    public class CssLayoutEngine : IGuiLayoutEngine
    {
        private CssAdapter mAdapter;

        public CssLayoutEngine(CssAdapter adapter)
        {
            this.mAdapter = adapter;
        }
        public void UpdateLayout(Gui gui)
        {
            UpdateLayout(gui, Display.Coordinates.Size);
        }
        public void UpdateLayout(Gui gui, Size renderTargetSize)
        {
            bool totalRefresh = false;

            totalRefresh |= gui.Desktop.Width != renderTargetSize.Width;
            totalRefresh |= gui.Desktop.Height != renderTargetSize.Height;
            totalRefresh |= gui.Desktop.LayoutDirty;

            gui.Desktop.Width = renderTargetSize.Width;
            gui.Desktop.Height = renderTargetSize.Height;

            SetDesktopAnimatorProperties(gui.Desktop);

            RedoLayout(gui.Desktop, totalRefresh);

            if (totalRefresh || gui.Desktop.Descendants.Any(x => x.LayoutDirty))
            {
                RedoFixedLayout(gui.Desktop);

                gui.Desktop.LayoutDirty = false;
                foreach (var w in gui.Desktop.Descendants)
                    w.LayoutDirty = false;
            }
        }

        private void SetDesktopAnimatorProperties(Desktop desktop)
        {
            var style = mAdapter.GetStyle(desktop);

            style.Widget.ClientRect = new Rectangle(0, 0, desktop.Width, desktop.Height);
        }

        private void RedoFixedLayout(Desktop desktop)
        {
            var deskStyle = mAdapter.GetStyle(desktop);
            desktop.ClientRect = new Rectangle(0, 0, desktop.Width, desktop.Height);

            foreach (var child in desktop.Descendants.Where(IsFixedPosition))
            {
                var style = mAdapter.GetStyle(child);
                var sz = style.Widget.ClientRect.Size;
                var box = style.BoxModel;

                switch (style.Data.Position)
                {
                    case CssPosition.Absolute:
                    case CssPosition.Fixed:
                        SetFixedCoordinates(style, box, sz, desktop.ClientRect);
                        break;
                }
            }
        }

        private bool IsFixedPosition(Widget widget)
        {
            var pos = mAdapter.GetStyle(widget).Data.Position;
            return pos == CssPosition.Absolute || pos == CssPosition.Fixed;
        }

        private void SetFixedCoordinates(CssStyle style, CssBoxModel box, Size sz, Rectangle desktopBounds)
        {
            var position = style.Data.PositionData;
            var parentStyle = mAdapter.GetStyle(style.Widget.ParentCoordinateSystem);
            var widget = style.Widget;

            Rectangle clientRect = widget.ClientRect;
            Size widgetSize = widget.WidgetSize;

            if (position.Left.Automatic == false)
            {
                var targetLeft = ConvertDistance(style.Widget, position.Left, true, false).Value;

                clientRect.X = targetLeft + box.Left;
            }
            if (position.Right.Automatic == false)
            {
                int targetRight = ConvertDistance(style.Widget, position.Right, true, false).Value;
                targetRight = parentStyle.Widget.ClientRect.Width - targetRight;

                clientRect.X = targetRight - clientRect.Width - box.Right;
            }
            if (position.Top.Automatic == false)
            {
                int targetTop = ConvertDistance(style.Widget, position.Top, false, false).Value;

                clientRect.Y = targetTop + box.Top;
            }
            if (position.Bottom.Automatic == false)
            {
                int targetBottom = ConvertDistance(style.Widget, position.Bottom, false, false).Value;
                targetBottom = parentStyle.Widget.ClientRect.Height - targetBottom;

                clientRect.Y = targetBottom - clientRect.Height - box.Bottom;
            }

            widget.ClientRect = clientRect;

            int maxBottom = desktopBounds.Bottom - box.Margin.Bottom;

            if (widget.WidgetRect.Bottom > maxBottom)
            {
                int excess = widget.WidgetRect.Bottom - maxBottom;

                clientRect.Height -= excess;
                widgetSize.Height -= excess;
            }

            widget.WidgetSize = widgetSize;

        }

        public CssAdapter Adapter { get { return mAdapter; } }

        private void RedoLayout(Container container, bool forceRefresh = false)
        {
            var containerStyle = mAdapter.GetStyle(container);
            CssBoxModel containerBox = containerStyle.BoxModel;

            if (forceRefresh == false)
            {
                if (container.LayoutDirty == false &&
                    container.Descendants.Any(x => x.LayoutDirty) == false)
                {
                    return;
                }
            }

            Rectangle originalContainerClient = container.ClientRect;
            Rectangle containerClientRect = container.ClientRect;
            containerClientRect.X = 0;
            containerClientRect.Y = 0;

            int maxContWidth = ComputeMaxWidthForContainer(containerStyle);
            int maxContBottom = ComputeMaxHeightForContainer(containerStyle);
            Point nextPos = Point.Empty;
            int largestHeight = 0;

            maxContWidth -= containerBox.Left + containerBox.Right;

            int largestWidth = 0;
            int bottom = 0;

            int? fixedContainerWidth = ConvertDistance(container, containerStyle.Data.PositionData.Width, true);
            int? fixedContainerHeight = ConvertDistance(container, containerStyle.Data.PositionData.Height, true);
            if (fixedContainerWidth != null)
                maxContWidth = (int)fixedContainerWidth;

            containerClientRect.Width = maxContWidth;

            bool resetNextPosition = false;

            if (container is Desktop == false)
                container.ClientRect = containerClientRect;

            if (container.ManualLayout)
            {
                containerClientRect.Size = originalContainerClient.Size;
                fixedContainerWidth = originalContainerClient.Width;
                fixedContainerHeight = originalContainerClient.Height;
            }

            foreach (var child in container.Children)
            {
                var style = mAdapter.GetStyle(child);
                SetStaticProperties(child);
                Rectangle originalClient = child.ClientRect;

                if (child.Visible == false)
                    continue;
                if (style.Data.Display == CssDisplay.None)
                    continue;

                style.IncludeInLayout = true;

                switch (style.Data.Position)
                {
                    case CssPosition.Absolute:
                        style.IncludeInLayout = false;
                        child.ParentCoordinateSystem = TopLevelWidget(child, x => mAdapter.GetStyle(x).Data.Position != CssPosition.Static);
                        break;

                    case CssPosition.Fixed:
                        style.IncludeInLayout = false;
                        child.ParentCoordinateSystem = TopLevelWidget(child, x => x is Desktop);
                        break;
                }

                int? maxWidth = ConvertDistance(child, style.Data.PositionData.MaxWidth, true, true);
                int? maxHeight = ConvertDistance(child, style.Data.PositionData.MaxHeight, true, true);

                if (style.IncludeInLayout)
                {
                    maxWidth = maxContWidth - nextPos.X;
                }

                var sz = ComputeSize(child, containerStyle, forceRefresh, maxWidth, maxHeight);

                var box = style.BoxModel;
                int? fixedWidth = ConvertDistance(child, style.Data.PositionData.Width, true);
                int? fixedHeight = ConvertDistance(child, style.Data.PositionData.Height, false);

                if (fixedWidth != null) sz.Width = (int)fixedWidth;
                if (fixedHeight != null) sz.Height = (int)fixedHeight;

                int? minWidth = ConvertDistance(child, style.Data.PositionData.MinWidth, true, true);
                int? minHeight = ConvertDistance(child, style.Data.PositionData.MinHeight, true, true);

                if (minWidth != null && sz.Width < (int)minWidth) sz.Width = (int)minWidth;
                if (minHeight != null && sz.Height < (int)minHeight) sz.Height = (int)minHeight;

                bool resetPosition = false;

                switch (containerStyle.Data.Layout.Kind)
                {
                    case CssLayoutKind.Flow:
                        if (resetNextPosition)
                        {
                            resetPosition = true;
                            resetNextPosition = false;
                        }
                        if (style.Data.Display == CssDisplay.Block)
                        {
                            resetPosition = true;
                            resetNextPosition = true;
                        }

                        if (nextPos.X + sz.Width + style.BoxModel.Left + style.BoxModel.Right > maxContWidth)
                            resetPosition = true;

                        if (resetPosition)
                        {
                            nextPos.X = 0;
                            nextPos.Y += largestHeight;
                            largestHeight = 0;
                        }
                        break;
                }

                Rectangle clientRect = new Rectangle();

                clientRect.Size = sz;

                if (style.IncludeInLayout)
                {
                    clientRect.X = nextPos.X + box.Left;
                    clientRect.Y = nextPos.Y + box.Top;
                }

                int bottomMargin = box.Bottom;

                if (clientRect.Bottom + bottomMargin > maxContBottom)
                {
                    clientRect.Height -= clientRect.Bottom - maxContBottom - bottomMargin;
                }

                style.Widget.ClientWidgetOffset = new Point(
                    box.Padding.Left + box.Border.Left,
                    box.Padding.Top + box.Border.Top);

                style.Widget.WidgetSize = new Size(
                    clientRect.Width + box.Padding.Left + box.Border.Left + box.Padding.Right + box.Border.Right,
                    clientRect.Height + box.Padding.Top + box.Border.Top + box.Padding.Bottom + box.Border.Bottom);

                if (style.IncludeInLayout)
                {
                    switch (containerStyle.Data.Layout.Kind)
                    {
                        case CssLayoutKind.Flow:
                            nextPos.X += clientRect.Width + box.Left + box.Right;
                            largestWidth = Math.Max(largestWidth, nextPos.X);
                            break;

                        case CssLayoutKind.Column:
                            nextPos.X = 0;
                            nextPos.Y += clientRect.Height + box.Top + box.Bottom;
                            largestWidth = Math.Max(largestWidth, clientRect.Width + box.Right + box.Left);
                            break;

                        case CssLayoutKind.Row:
                            nextPos.X += clientRect.Width + box.Left + box.Right;
                            largestWidth = Math.Max(largestWidth, nextPos.X);
                            break;
                    }

                    largestHeight = Math.Max(largestHeight, clientRect.Height + box.Top + box.Bottom);
                    bottom = Math.Max(bottom, clientRect.Y + clientRect.Height + box.Bottom);  // only add box.Bottom here, because box.Top is taken into account in child.Y.
                }

                if (container.ManualLayout)
                {
                    clientRect = originalClient;
                    style.IncludeInLayout = false;
                }

                style.Widget.ClientRect = clientRect;
            }

            containerClientRect.Width = Math.Min(largestWidth, maxContWidth);
            containerClientRect.Height = bottom;

            if (fixedContainerWidth != null)
                containerClientRect.Width = (int)fixedContainerWidth;
            if (fixedContainerHeight != null)
                containerClientRect.Height = (int)fixedContainerHeight;

            switch (containerStyle.Data.Layout.Kind)
            {
                case CssLayoutKind.Column:
                    foreach (var child in container.Children)
                    {
                        var style = mAdapter.GetStyle(child);
                        var box = style.BoxModel;
                        int width = containerClientRect.Width - box.Left - box.Right;

                        if (style.IncludeInLayout == false)
                            continue;

                        if (style.Data.PositionData.MinWidth.Automatic == false)
                        {
                            int minwidth = mAdapter.CssDistanceToPixels(containerStyle, style.Data.PositionData.MinWidth, true);
                            width = Math.Max(width, minwidth);
                        }
                        if (style.Data.PositionData.MaxWidth.Automatic == false)
                        {
                            int maxwidth = mAdapter.CssDistanceToPixels(containerStyle, style.Data.PositionData.MaxWidth, true);
                            width = Math.Min(width, maxwidth);
                        }

                        style.Widget.ClientRect = new Rectangle(
                            style.Widget.ClientRect.X,
                            style.Widget.ClientRect.Y,
                            width,
                            style.Widget.ClientRect.Height);
                    }
                    break;
            }

            if (containerClientRect.Bottom > maxContBottom)
                containerClientRect.Height += maxContBottom - containerClientRect.Bottom;

            if (container is Desktop == false)
                container.ClientRect = containerClientRect;
        }

        private void SetStaticProperties(Widget child)
        {
            var style = mAdapter.GetStyle(child);
            var container = child as Container;

            child.Font = style.Font;

            switch (style.Data.Overflow)
            {
                case CssOverflow.Scroll:
                    if (container != null)
                        container.AllowScroll = ScrollAxes.Both;
                    break;
            }
        }

        private Container TopLevelWidget(Widget child)
        {
            return TopLevelWidget(child, x => true);
        }
        private Container TopLevelWidget(Widget child, Func<Widget, bool> validMatch)
        {
            var retval = child.Parent;

            if (retval == null)
                return null;

            if (validMatch(retval))
                return retval;

            return TopLevelWidget(retval, validMatch);
        }

        private int ComputeMaxWidthForContainer(CssStyle style)
        {
            Container container = (Container)style.Widget;
            CssStyleData styleData = style.Data;

            if (container.Parent == null)
                return container.Width;

            var parentStyle = mAdapter.GetStyle(container.Parent);

            int availableWidth = container.Parent.ClientRect.Width - container.X;

            if (styleData.PositionData.MaxWidth.Automatic)
                return availableWidth;
            else
            {
                int maxWidth = mAdapter.CssDistanceToPixels(style, styleData.PositionData.MaxWidth, true);

                return Math.Min(availableWidth, maxWidth);
            }
        }

        private int ComputeMaxHeightForContainer(CssStyle style)
        {
            Container container = (Container)style.Widget;
            CssStyleData styleData = style.Data;

            if (container.Parent == null)
                return container.Height;

            var parentStyle = mAdapter.GetStyle(container.Parent);

            int availableHeight = ComputeMaxHeightForContainer(mAdapter.GetStyle(container.Parent)) - container.Y;
            var box = style.BoxModel;

            if (styleData.PositionData.MaxHeight.Automatic)
            {
                return availableHeight - box.Top - box.Bottom;
            }
            else
            {
                int maxHeight = mAdapter.CssDistanceToPixels(style, styleData.PositionData.MaxHeight, false);

                return Math.Min(availableHeight, maxHeight);
            }
        }
        private Size ComputeSize(Widget control, CssStyle parentStyle, bool forceRefresh, int? maxWidth, int? maxHeight)
        {
            return ComputeSize(control, parentStyle.Data, forceRefresh, maxWidth, maxHeight);
        }
        private Size ComputeSize(Widget control, CssStyleData parentStyle, bool forceRefresh, int? maxWidth, int? maxHeight)
        {
            if (control is Container)
                return ComputeContainerSize((Container)control, forceRefresh);

            mAdapter.SetFont(control);
            var style = mAdapter.GetStyle(control);

            int? minWidth = ConvertDistance(control, style.Data.PositionData.MinWidth, true);
            int? minHeight = ConvertDistance(control, style.Data.PositionData.MinHeight, false);
            int? styleMaxWidth = ConvertDistance(control, style.Data.PositionData.MaxWidth, true);
            int? styleMaxHeight = ConvertDistance(control, style.Data.PositionData.MaxHeight, false);

            if (maxWidth == null) maxWidth = styleMaxWidth;
            else if (maxWidth != null && styleMaxWidth != null)
                maxWidth = Math.Min(maxWidth.Value, styleMaxWidth.Value);

            if (maxHeight == null) maxHeight = styleMaxHeight;
            else if (maxHeight != null && styleMaxHeight != null)
                maxWidth = Math.Min(maxHeight.Value, styleMaxHeight.Value);

            return control.ComputeSize(minWidth, minHeight, maxWidth, maxHeight);
        }

        private int? ConvertDistance(Widget control, CssDistance cssDistance, bool width, bool autoIsNull = true)
        {
            if (cssDistance.Automatic && autoIsNull)
                return null;

            return mAdapter.CssDistanceToPixels(control, cssDistance, width);
        }

        private Size ComputeContainerSize(Container container, bool forceRefresh)
        {
            RedoLayout(container, forceRefresh);

            return container.ClientRect.Size;
        }
    }
}
