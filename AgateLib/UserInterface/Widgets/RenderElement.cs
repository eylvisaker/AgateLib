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
        IList<IRenderElement> Children { get; }

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

        void SetProps(RenderElementProps props);

        /// <summary>
        /// Gets whether or not the element can receive input focus.
        /// </summary>
        bool CanHaveFocus { get; }

        /// <summary>
        /// Gets the aggregated style of the render element.
        /// </summary>
        IRenderElementStyle Style { get; }

        /// <summary>
        /// Gets the props object for the render element.
        /// </summary>
        RenderElementProps Props { get; }

        /// <summary>
        /// Event called by a child component when it receives an input event it cannot handle.
        /// This is usually a navigation event.
        /// </summary>
        /// <param name="menuItemElement"></param>
        /// <param name="btn"></param>
        void OnChildNavigate(IRenderElement child, MenuInputButton button);
        void OnChildrenUpdated();

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
        void OnWillUnmount();
        void OnDidMount();

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

            Display = new RenderElementDisplay(props);
        }

        #region --- Props ---

        public TProps Props { get; private set; }
        RenderElementProps IRenderElement.Props => Props;

        public void SetProps(RenderElementProps props)
        {
            Props = (TProps)props;
            Display.SetProps(props);

            OnReceiveProps();
        }

        #endregion
        #region --- Rendering Widgets ---

        Action<IRenderable> IRenderable.NeedsRender { get => NeedsRender; set => NeedsRender = value; }
        protected Action<IRenderable> NeedsRender { get; private set; }

        protected IRenderElement Parent => Display.System.ParentOf(this);

        protected IEnumerable<IRenderElement> Finalize(IEnumerable<IRenderable> renderables)
        {
            return renderables.Select(Finalize);
        }

        protected IRenderElement Finalize(IRenderable renderable)
        {
            return renderable.Finalize(e => NeedsRender?.Invoke(e));
        }

        #endregion

        public RenderElementDisplay Display { get; }

        public virtual IList<IRenderElement> Children { get; protected set; }

        public IRenderElementStyle Style => Display.Style;

        public virtual string StyleTypeId => GetType().Name;

        public string StyleClass => Props.StyleClass;

        public string StyleId => Props.StyleId;

        public string Name => Props.StyleId;

        public virtual bool CanHaveFocus => false;

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
            Props.Blur?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnFocus()
        {
            Props.Focus?.Invoke(this, EventArgs.Empty);
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

        public virtual void OnWillUnmount()
        {
            Props.WillUnmount?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnDidMount()
        {
            Props.DidMount?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnChildrenUpdated()
        {
        }

        protected virtual void OnReceiveProps()
        {
        }
    }

    public class RenderElementProps
    {
        /// <summary>
        /// The ID value used in matching styles.
        /// </summary>
        public string StyleId { get; set; }

        /// <summary>
        /// The class value used in matching styles.
        /// </summary>
        public string StyleClass { get; set; }

        /// <summary>
        /// Style elements specified by the parent. Styles specified here have the highest priority.
        /// </summary>
        public InlineElementStyle Style { get; set; }

        /// <summary>
        /// The default style for this element. Styles specified here have the lowest priority.
        /// </summary>
        public InlineElementStyle DefaultStyle { get; set; }

        /// <summary>
        /// Key which is used to match the render element during reconciliation.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Event raised when a render element is mounted.
        /// </summary>
        public EventHandler DidMount { get; set; }

        /// <summary>
        /// Event raised when a render element is unmounted.
        /// </summary>
        public EventHandler WillUnmount { get; set; }

        /// <summary>
        /// Event raised when a render element receives the focus.
        /// </summary>
        public EventHandler Focus { get; set; }

        /// <summary>
        /// Event raised when a render element loses the focus.
        /// </summary>
        public EventHandler Blur { get; set; }

        /// <summary>
        /// Compares two props objects to see if their property values are equal.
        /// By default, this uses reflection to check each individual property, except
        /// properties named "Children".
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool PropertiesEqual(RenderElementProps other)
        {
            if (other.GetType() != GetType())
                return false;

            var properties = GetType().GetProperties();

            foreach (var prop in properties.Where(x => x.Name != "Children"))
            {
                object myValue = prop.GetValue(this);
                object otherValue = prop.GetValue(other);

                if (myValue == null && otherValue == null) continue;
                if (myValue == null || otherValue == null) return false;

                if (!myValue.Equals(otherValue))
                    return false;
            }

            return true;
        }
    }
}
