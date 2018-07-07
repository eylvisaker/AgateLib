﻿//
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
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public interface IRenderElement : IRenderable
    {
        /// <summary>
        /// Gets the widget display object that contains all the data needed to
        /// render styling and animation of the widget on screen.
        /// </summary>
        RenderElementDisplay Display { get; }

        /// <summary>
        /// Gets a read-only collection of children of this element.
        /// </summary>
        IReadOnlyList<IRenderElement> Children { get; }

        /// <summary>
        /// Gets the type identifier used to identify this widget type to the styling
        /// engine.
        /// </summary>
        string StyleTypeId { get; }

        /// <summary>
        /// Gets the class name used to identify this widget to the styling
        /// </summary>
        string StyleClass { get; }

        /// <summary>
        /// Gets the identifier of this widget for styling.
        /// </summary>
        string StyleId { get; }        

        /// <summary>
        /// Gets the name of the widget.
        /// </summary>
        [Obsolete("Use StyleId instead?")]
        string Name { get; }

        /// <summary>
        /// Gets whether or not the element can receive input focus.
        /// </summary>
        bool CanHaveFocus { get; }

        /// <summary>
        /// Gets the aggregated style of the render element.
        /// </summary>
        IRenderElementStyle Style { get; }

        /// <summary>
        /// Event called by a child component when it receives an input event it cannot handle.
        /// This is usually a navigation event.
        /// </summary>
        /// <param name="menuItemElement"></param>
        /// <param name="btn"></param>
        void OnChildNavigate(IRenderElement child, MenuInputButton button);

        /// <summary>
        /// Draws the content of the widget.
        /// To draw children, call <c >renderContext.DrawChildren</c>.
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="offset"></param>
        void Draw(IWidgetRenderContext renderContext, Rectangle clientArea);

        /// <summary>
        /// Updates the widget.
        /// </summary>
        /// <param name="renderContext"></param>
        void Update(IWidgetRenderContext renderContext);

        /// <summary>
        /// Compute the ideal size of the content of the widget.
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        Size CalcIdealContentSize(IWidgetRenderContext renderContext, Size maxSize);

        /// <summary>
        /// Instructs the element to prepare its layout.
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="clientArea"></param>
        void DoLayout(IWidgetRenderContext renderContext, Size size);

        /// <summary>
        /// Called when the element loses focus.
        /// </summary>
        void OnBlur();

        /// <summary>
        /// Called when the element gains focus.
        /// </summary>
        void OnFocus();

        /// <summary>
        /// Called when the widget receives an input event.
        /// </summary>
        /// <param name="widgetEventArgs"></param>
        void OnInputEvent(InputEventArgs input);

        /// <summary>
        /// Called when the user accepts an item. Accept can be a button press, enter on the keyboard, or a mouse click (not yet supported).
        /// </summary>
        void OnAccept();

        /// <summary>
        /// Called when the user selects an item in its parent.
        /// </summary>
        void OnSelect();
    }

    public abstract class RenderElement<TProps> : IRenderElement where TProps : RenderElementProps
    {
        public RenderElement(TProps props)
        {
            this.Props = props;

            Display = new RenderElementDisplay(Props.Style, Props.DefaultStyle);
        }

        protected IRenderElement Parent => Display.System.ParentOf(this);

        public RenderElementDisplay Display { get; }

        public virtual IReadOnlyList<IRenderElement> Children { get; protected set; }
        
        public IRenderElementStyle Style => Display.Style;

        public virtual string StyleTypeId => GetType().Name;

        public string StyleClass => Props.StyleClass;

        public string StyleId => Props.StyleId;

        public string Name => Props.StyleId;

        public virtual bool CanHaveFocus => false;

        public TProps Props { get; }

        public abstract Size CalcIdealContentSize(IWidgetRenderContext renderContext, Size maxSize);

        public abstract void Draw(IWidgetRenderContext renderContext, Rectangle clientArea);

        public abstract void DoLayout(IWidgetRenderContext renderContext, Size size);

        public virtual void OnInputEvent(InputEventArgs input)
        {
        }

        public virtual void Update(IWidgetRenderContext renderContext)
        {
        }

        public void DrawChildren(IWidgetRenderContext renderContext, Rectangle clientArea)
        {
            renderContext.DrawChildren(clientArea, Children);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(StyleTypeId);

            if (!string.IsNullOrWhiteSpace(StyleClass))
                result.Append($".{StyleClass}");

            if (!string.IsNullOrWhiteSpace(StyleId))
                result.Append($"#{StyleId}");

            return result.ToString();
        }

        IRenderable IRenderable.Render() => this;

        public virtual void OnBlur()
        {
        }

        public virtual void OnFocus()
        {
        }

        public virtual void OnAccept()
        {
        }

        public virtual void OnSelect()
        {
        }

        /// <summary>
        /// Event called when a child component receives an input event it can't handle.
        /// Override this method to handle that event, otherwise the event will be passed to 
        /// this element's parent.
        /// </summary>
        /// <param name="child"></param>
        /// <param name="button"></param>
        public virtual void OnChildNavigate(IRenderElement child, MenuInputButton button)
        {
            Parent?.OnChildNavigate(this, button);
        }
    }

    public class RenderElementProps
    {
        public string StyleId { get; set; }

        public string StyleClass { get; set; }

        /// <summary>
        /// Style elements specified by the parent. Styles specified here have the highest priority.
        /// </summary>
        public InlineElementStyle Style { get; set; }

        /// <summary>
        /// The default style for this element. Styles specified here have the lowest priority.
        /// </summary>
        public InlineElementStyle DefaultStyle { get; set; } 
    }
}
