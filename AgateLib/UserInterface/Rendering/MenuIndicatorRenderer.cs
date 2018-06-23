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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Collections.Generic;
using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Rendering
{
    public interface IMenuIndicatorRenderer
    {
        void DrawFocus(IWidgetRenderContext renderContext, Workspace workspace);
    }

    public abstract class MenuIndicatorRenderer : IMenuIndicatorRenderer
    {
        public virtual void TheDrawFocus(IWidgetRenderContext renderContext, IWidget widget, Rectangle destRect)
        {
        }

        public void DrawFocus(IWidgetRenderContext renderContext, Workspace workspace)
        {
            (IWidget focusWidget, Point offset) = FindFocus(workspace.Layout.Focus);

            if (focusWidget == null)
                return;

            if (!focusWidget.CanHaveFocus)
                return;

            TheDrawFocus(renderContext, focusWidget,
                new Rectangle(offset, focusWidget.Display.Region.ContentSize));
        }

        private (IWidget, Point) FindFocus(IWidget widget)
        {
            if (widget.Display.Animation.IsAnimating)
                return (null, Point.Zero);

            var child = widget.Focus;

            if (child == null)
                return (widget, ContentPositionOf(widget));

            Point offset = ContentPositionOf(widget);

            (var focus, Point nextOffset) = FindFocus(child);

            return (focus, offset + nextOffset);
        }

        private static Point ContentPositionOf(IWidget child)
        {
            return child.Display.Animation.BorderRect.Location
                                     + child.Display.Region.BorderToContentOffset.TopLeft;
        }
    }
}