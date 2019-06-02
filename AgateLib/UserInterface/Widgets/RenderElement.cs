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
using System.Reflection;
using System.Text;

namespace AgateLib.UserInterface
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
        /// Gets the parent of this element.
        /// </summary>
        IRenderElement Parent { get; }

        /// <summary>
        /// Gets the type identifier used to identify this render element type to the styling
        /// engine.
        /// </summary>
        string StyleTypeId { get; }

        /// <summary>
        /// Gets the class name used to identify this render element to the styling
        /// </summary>
        string StyleClass { get; }

        /// <summary>
        /// Gets the name of the render element. This is used as the StyleId of the rendered element.
        /// </summary>
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
        /// Gets the props object for the render element.
        /// </summary>
        RenderElementProps Props { get; }

        /// <summary>
        /// Gets the state object for the render element.
        /// </summary>
        RenderElementState State { get; }

        /// <summary>
        /// Used to track references by the display system.
        /// </summary>
        ElementReference Ref { get; set; }

        /// <summary>
        /// Events for the element.
        /// </summary>
        RenderElementEvents Events { get; }

        /// <summary>
        /// Draws the content of the widget.
        /// To draw children, call <c >renderContext.DrawChildren</c>.
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="offset"></param>
        void Draw(IUserInterfaceRenderContext renderContext, Rectangle clientArea);

        /// <summary>
        /// Updates the widget.
        /// </summary>
        /// <param name="renderContext"></param>
        void Update(IUserInterfaceRenderContext renderContext);

        /// <summary>
        /// Compute the ideal size of the content of the widget.
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        Size CalcIdealContentSize(IUserInterfaceRenderContext renderContext, Size maxSize);

        /// <summary>
        /// Compute the minimum size of the content of the widget, given
        /// a constraint on its height or width.
        /// </summary>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
        Size CalcMinContentSize(int? widthConstraint, int? heightConstraint);

        /// <summary>
        /// Instructs the element to prepare its layout.
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="contentAreaSize"></param>
        void DoLayout(IUserInterfaceRenderContext renderContext, Size contentAreaSize);

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
        void OnUserInterfaceAction(UserInterfaceActionEventArgs action);

        /// <summary>
        /// Event called by a child component when it receives an input event it cannot handle.
        /// This is usually a navigation event.
        /// </summary>
        /// <param name="menuItemElement"></param>
        /// <param name="btn"></param>
        void OnChildAction(IRenderElement child, UserInterfaceActionEventArgs action);

        /// <summary>
        /// Called by the rendering system when the collection of children is updated.
        /// </summary>
        void OnChildrenUpdated();

        /// <summary>
        /// Called by the rendering system right before the component is removed from
        /// the render tree.
        /// </summary>
        void OnWillUnmount();

        /// <summary>
        /// Called by the rendering system right after the component
        /// is added to the render tree.
        /// </summary>
        void OnDidMount();

        /// <summary>
        /// Called after reconciliation is complete.
        /// </summary>
        void OnReconciliationCompleted();

        /// <summary>
        /// Sets the props of the render element. This must match the props type used by the render element.
        /// </summary>
        /// <param name="props"></param>
        void SetProps(RenderElementProps props);

        /// <summary>
        /// Sets the state of the render element. This must match the state type used by the render element.
        /// </summary>
        /// <param name="state"></param>
        void SetState(RenderElementState state);
    }

    public abstract class RenderElement<TProps> : RenderElement<TProps, RenderElementState> where TProps : RenderElementProps
    {
        public RenderElement(TProps props) : base(props)
        {
        }
    }

    public abstract class RenderElement<TProps, TState> : IRenderElement where TProps : RenderElementProps where TState:RenderElementState

    {
        public RenderElement(TProps props)
        {
            this.Props = props;

            Display = new RenderElementDisplay(props);

            ReceiveRenderElementProps();

            EventData = new UserInterfaceEvent(this);
        }

        #region --- Props ---

        public TProps Props { get; private set; }
        RenderElementProps IRenderElement.Props => Props;

        void IRenderElement.SetProps(RenderElementProps props)
        {
            Props = (TProps)props;
            Display.SetProps(props);

            OnReceiveProps();
        }

        #endregion
        #region --- State ---

        private TState state;

        public TState State => state;
        RenderElementState IRenderElement.State => State;

        protected void ReplaceState(Func<TState, TState> stateMutator)
        {
            SetState(stateMutator(state));
        }

        protected void SetState(Action<TState> stateMutator)
        {
            stateMutator(state);

            //NeedsRender?.Invoke(this);
        }

        protected void SetState(TState newState)
        {
            this.state = newState;

            //NeedsRender?.Invoke(this);
        }

        void IRenderElement.SetState(RenderElementState newState)
        {
            if (State == null && newState == null)
                return;

            if (newState == null)
                throw new ArgumentNullException("State should not be set to null after it has been set to a value.");

            if (newState is TState typedState)
            {
                SetState(typedState);
                OnReceiveState();
            }
            else
            {
                throw new ArgumentException("NewState is not correct type");
            }
        }

        #endregion
        #region --- Rendering Widgets ---

        Action<IRenderable> IRenderable.NeedsRender { get => NeedsRender; set => NeedsRender = value; }
        protected Action<IRenderable> NeedsRender { get; private set; }

        public IRenderElement Parent => Display.System.ParentOf(this);

        protected IEnumerable<IRenderElement> Finalize(IEnumerable<IRenderable> renderables)
        {
            return renderables.Where(x => x != null).Select(FinalizeRendering);
        }

        protected IRenderElement FinalizeRendering(IRenderable renderable)
        {
            if (renderable == null)
                return null;

            return renderable.FinalizeRendering(e => NeedsRender?.Invoke(e));
        }

        #endregion

        /// <summary>
        /// Gets the event container for the render element.
        /// </summary>
        public virtual RenderElementEvents Events { get; } = new RenderElementEvents();

        /// <summary>
        /// Gets the display object for the render element. This contains everything the
        /// rendering system needs to display the render element.
        /// </summary>
        public RenderElementDisplay Display { get; }

        /// <summary>
        /// Gets or sets the children of the render element. This should only 
        /// be modified by the constructor of a render element.
        /// </summary>
        public virtual IList<IRenderElement> Children { get; protected set; }

        /// <summary>
        /// Gets the style of the render element. This contains things like the default font for
        /// the render element.
        /// </summary>
        public IRenderElementStyle Style => Display.Style;

        public virtual string StyleTypeId => GetType().Name;

        public string StyleClass => Props.StyleClass;

        public string Name => Props.Name;

        /// <summary>
        /// Gets whether or not the render element can hold input focus.
        /// </summary>
        public virtual bool CanHaveFocus => false;

        public ElementReference Ref { get; set; }

        protected UserInterfaceEvent EventData { get; }

        public virtual Size CalcMinContentSize(int? widthConstraint, int? heightConstraint)
        {
            return new Size(1, 1);
        }

        public abstract Size CalcIdealContentSize(IUserInterfaceRenderContext renderContext, Size maxSize);

        public abstract void Draw(IUserInterfaceRenderContext renderContext, Rectangle clientArea);

        public abstract void DoLayout(IUserInterfaceRenderContext renderContext, Size size);


        public virtual void Update(IUserInterfaceRenderContext renderContext)
        {
        }

        public void DrawChildren(IUserInterfaceRenderContext renderContext, Rectangle clientArea)
        {
            renderContext.DrawChildren(clientArea, Children);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(StyleTypeId);

            if (!string.IsNullOrWhiteSpace(StyleClass))
                result.Append($".{StyleClass}");

            if (!string.IsNullOrWhiteSpace(Name))
                result.Append($"#{Name}");

            return result.ToString();
        }

        IRenderable IRenderable.Render() => this;

        public virtual void OnUserInterfaceAction(UserInterfaceActionEventArgs args)
        {
            if (args.Action == UserInterfaceAction.Accept)
            {
                OnAccept(args);
            }
            else if (args.Action == UserInterfaceAction.Cancel)
            {
                OnCancel(args);
            }

            if (!args.Handled)
            {
                Parent?.OnChildAction(this, args);
            }
        }

        public virtual void OnAccept(UserInterfaceActionEventArgs args)
        {
        }

        public virtual void OnCancel(UserInterfaceActionEventArgs args)
        {
        }

        public virtual void OnBlur()
        {
            Props.OnBlur?.Invoke(EventData);
        }

        public virtual void OnFocus()
        {
            Props.OnFocus?.Invoke(EventData);
        }

        /// <summary>
        /// Event called when a child component receives an input event it can't handle.
        /// Override this method to handle that event, otherwise the event will be passed to 
        /// this element's parent.
        /// </summary>
        /// <param name="child"></param>
        /// <param name="button"></param>
        public virtual void OnChildAction(IRenderElement child, UserInterfaceActionEventArgs action)
        {
            if (!action.Handled)
            {
                Parent?.OnChildAction(this, action);
            }
        }

        public virtual void OnWillUnmount()
        {
            Props.OnWillUnmount?.Invoke(EventData);
        }

        public virtual void OnDidMount()
        {
            Props.OnDidMount?.Invoke(EventData);
        }

        public virtual void OnChildrenUpdated()
        {
        }

        /// <summary>
        /// Called when props are received. If overriden, you should not
        /// modify the Children collection.
        /// </summary>
        protected virtual void OnReceiveProps()
        {
            ReceiveRenderElementProps();
        }

        protected virtual void OnReceiveState()
        {

        }

        protected void ReceiveRenderElementProps()
        {
            Display.IsVisible = Props.Visible;

            Display.PseudoClasses.SetIf("disabled", !Props.Enabled);
        }

        public virtual void OnReconciliationCompleted()
        {
        }

        protected string FirstNotNullOrWhitespace(params string[] values)
        {
            for (int i = 0; i < values.Length; i++)
                if (!string.IsNullOrWhiteSpace(values[i]))
                    return values[i];

            return null;
        }

        protected void DoLayoutForSingleChild(IUserInterfaceRenderContext renderContext, Size size, IRenderElement child)
        {
            child.Display.MarginRect = new Rectangle(Point.Zero, size);

            var contentSize = child.Display.ContentRect.Size;

            child.DoLayout(renderContext, contentSize);
        }

        void IRenderable.OnRenderResult(IRenderElement result)
        {
        }
    }

    public class RenderElementProps
    {
        private InlineElementStyle _defaultStyle;
        private InlineElementStyle _style;

        /// <summary>
        /// Gets or sets the name of the theme the styling engine should 
        /// use to style the owning widget.
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// The ID value used in matching styles.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The class value used in matching styles.
        /// </summary>
        public string StyleClass { get; set; }

        /// <summary>
        /// Style elements specified by the parent. Styles specified here have the highest priority.
        /// </summary>
        public InlineElementStyle Style
        {
            get => _style;
            set
            {
                if (value != null)
                    value.Specificity = 1000;

                _style = value;
            }
        }

        /// <summary>
        /// The default style for this element. Styles specified here have the lowest priority.
        /// </summary>
        public InlineElementStyle DefaultStyle
        {
            get => _defaultStyle;
            set
            {
                if (value != null)
                    value.Specificity = -1000;

                _defaultStyle = value;
            }
        }

        /// <summary>
        /// Key which is used to match the render element during reconciliation.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Event raised when a render element is mounted.
        /// </summary>
        public UserInterfaceEventHandler OnDidMount { get; set; }

        /// <summary>
        /// Event raised when a render element is unmounted.
        /// </summary>
        public UserInterfaceEventHandler OnWillUnmount { get; set; }

        /// <summary>
        /// Event raised when a render element receives the focus.
        /// </summary>
        public UserInterfaceEventHandler OnFocus { get; set; }

        /// <summary>
        /// Event raised when a render element loses the focus.
        /// </summary>
        public UserInterfaceEventHandler OnBlur { get; set; }

        /// <summary>
        /// Gets or sets whether the element is visible. Defaults to true.
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the element is enabled for interaction
        /// with the user. Defaults to true.
        /// </summary>
        public bool Enabled { get; set; } = true;

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

            var properties = GetType().GetTypeInfo().DeclaredProperties;

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

    public class RenderElementState
    {

    }

    public static class RenderElementExtensions
    {
        /// <summary>
        /// Compute the ideal size of an element's margin rectangle.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="renderContext"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public static Size CalcIdealMarginSize(this IRenderElement element, IUserInterfaceRenderContext renderContext, Size maxSize)
        {
            var contentSize = element.CalcIdealContentSize(renderContext, maxSize);

            var marginSize = element.Display.Region.MarginToContentOffset.Expand(contentSize);

            return marginSize;
        }

        /// <summary>
        /// Computes the minimum size of an element's margin rectangle.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
        public static Size CalcMinMarginSize(this IRenderElement element,
                                             int? widthConstraint,
                                             int? heightConstraint)
        {
            var contentSize = element.CalcMinContentSize(widthConstraint, heightConstraint);

            var marginSize = element.Display.Region.MarginToContentOffset.Expand(contentSize);

            return marginSize;
        }

        /// <summary>
        /// Copies standard WidgetProps members to the RenderElementProps structure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elementProps"></param>
        /// <param name="props"></param>
        /// <returns></returns>
        public static T CopyFromWidgetProps<T>(this T elementProps, WidgetProps props)
            where T : RenderElementProps
        {
            elementProps.Name = props.Name;
            elementProps.Theme = props.Theme;
            elementProps.Style = props.Style;
            elementProps.StyleClass = props.StyleClass;
            elementProps.DefaultStyle = props.DefaultStyle;
            elementProps.Visible = props.Visible;

            return elementProps;
        }
    }
}
