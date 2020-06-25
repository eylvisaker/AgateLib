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
using AgateLib.Quality;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AgateLib.UserInterface
{
    /// <summary>
    /// Interface for a render element.
    /// </summary>
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
        /// Gets whether or not the element currently has input focus.
        /// </summary>
        bool HasFocus { get; }
        /// <summary>
        /// Gets whether or not the element participates in layout.
        /// </summary>
        bool ParticipateInLayout { get; }

        /// <summary>
        /// Gets the aggregated style of the render element.
        /// </summary>
        IRenderElementStyle Style { get; }

        /// <summary>
        /// Gets the props object for the render element.
        /// </summary>
        RenderElementProps Props { get; }

        /// <summary>
        /// Used to track references by the display system.
        /// </summary>
        ElementReference Ref { get; set; }

        /// <summary>
        /// Events for the element.
        /// </summary>
        RenderElementEvents Events { get; }

        ChildReconciliationMode ChildReconciliationMode { get; }

        /// <summary>
        /// Draws the background and border of the element.
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="clientDest"></param>
        void DrawBackgroundAndBorder(IUserInterfaceRenderContext renderContext, Rectangle clientArea);

        /// <summary>
        /// Draws the content of the widget.
        /// To draw children, call <c>renderContext.DrawChildren</c>.
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
        /// <param name="layoutContext"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        Size CalcIdealContentSize(IUserInterfaceLayoutContext layoutContext, Size maxSize);

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
        /// <param name="layoutContext"></param>
        /// <param name="contentAreaSize"></param>
        void DoLayout(IUserInterfaceLayoutContext layoutContext, Size contentAreaSize);

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
        /// Called when a button is pressed.
        /// </summary>
        /// <param name="args"></param>
        void OnButtonDown(ButtonStateEventArgs args);

        /// <summary>
        /// Called when a button is released.
        /// </summary>
        /// <param name="args"></param>
        void OnButtonUp(ButtonStateEventArgs args);

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
        void ReconcileChildren(IRenderElement newNode);

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
    }

    public abstract class RenderElement<TProps> : RenderElement<TProps, object> where TProps : RenderElementProps
    {
        public RenderElement(TProps props) : base(props)
        {
        }
    }

    public abstract class RenderElement<TProps, TState> : IRenderElement where TProps : RenderElementProps
    {
        public RenderElement(TProps props)
        {
            this.Props = props;

            Display = new RenderElementDisplay(this, props);

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
            TryFinalizeChildren();
        }

        #endregion
        #region --- State ---

        private TState state;
        private IUserInterfaceAppContext appContext;

        public TState State => state;

        protected void ReplaceState(Func<TState, TState> stateMutator)
        {
            SetState(stateMutator(state));
        }

        protected void SetState(Action<TState> stateMutator)
        {
            stateMutator(state);

            OnReceiveState();

            //NeedsRender?.Invoke(this);
        }

        protected void SetState(TState newState)
        {
            this.state = newState;

            OnReceiveState();

            //NeedsRender?.Invoke(this);
        }

        #endregion
        #region --- AppContext ---

        public IUserInterfaceAppContext AppContext
        {
            get => appContext;
            set
            {
                Require.ArgumentNotNull(value, nameof(AppContext));

                if (this.appContext == value)
                {
                    return;
                }

                appContext = value;

                TryFinalizeChildren();
                OnReceivedAppContext();
            }
        }

        protected virtual void OnReceivedAppContext()
        {

        }

        #endregion
        #region --- Rendering ---

        Action<IRenderable> IRenderable.NeedsRender { get => NeedsRender; set => NeedsRender = value; }
        protected Action<IRenderable> NeedsRender { get; private set; }

        public IRenderElement Parent => Display.System?.ParentOf(this);

        public virtual void DrawBackgroundAndBorder(IUserInterfaceRenderContext renderContext, Rectangle clientArea)
        {
            renderContext.UserInterfaceRenderer.DrawBackground(renderContext, Display, clientArea);
            renderContext.UserInterfaceRenderer.DrawFrame(renderContext, Display, clientArea);
        }

        public abstract void Draw(IUserInterfaceRenderContext renderContext, Rectangle clientArea);

        /// <summary>
        /// Draws the set of children.
        /// </summary>
        /// <param name="renderContext">The render context service.</param>
        /// <param name="clientArea">The client area of the element which is the parent of the children to draw.</param>
        /// <param name="children">The list of children to draw. If this is null, it will use the <c>Children</c> property.</param>
        protected void DrawChildren(IUserInterfaceRenderContext renderContext,
                                    Rectangle clientArea,
                                    IEnumerable<IRenderElement> children = null)
        {
            children = children ?? Children;

            renderContext.DrawChildren(clientArea, children);
        }

        /// <summary>
        /// Draws a single child of the current element.
        /// </summary>
        /// <param name="renderContext">The render context service.</param>
        /// <param name="clientArea">The client area of the element which is the parent of the child to draw.</param>
        /// <param name="child">The child to draw.</param>
        protected void DrawChild(IUserInterfaceRenderContext renderContext,
                                 Rectangle clientArea,
                                 IRenderElement child)
        {
            renderContext.DrawChild(clientArea, child);
        }

        #endregion
        #region --- Handling Children ---

        /// <summary>
        /// Gets or sets the children of the render element. This should only 
        /// be modified by the constructor of a render element.
        /// </summary>
        public virtual IList<IRenderElement> Children { get; protected set; }

        protected virtual ChildReconciliationMode ChildReconciliationMode => ChildReconciliationMode.System;
        ChildReconciliationMode IRenderElement.ChildReconciliationMode => ChildReconciliationMode;

        protected void TryFinalizeChildren()
        {
            if (AppContext == null)
                return;

            OnFinalizeChildren();
        }

        /// <summary>
        /// Called when the render element is ready to have its children finalized.
        /// </summary>
        protected virtual void OnFinalizeChildren() { }

        /// <summary>
        /// Finalizes rendering of a set of child widgets. This must not be called in the constructor
        /// because it requires the AppContext property be set. Instead call in OnReceiveAppContext.
        /// </summary>
        /// <param name="renderables"></param>
        /// <returns></returns>
        protected IEnumerable<IRenderElement> FinalizeRendering(IEnumerable<IRenderable> renderables)
        {
            Require.That(AppContext != null, "AppContext must not be null to finalize.");

            return renderables.Where(x => x != null).Select(FinalizeRendering);
        }

        /// <summary>
        /// Finalizes rendering of a child widget. This must not be called in the constructor
        /// because it requires the AppContext property be set. Instead call in OnReceiveAppContext.
        /// </summary>
        /// <param name="renderable"></param>
        /// <returns></returns>
        protected IRenderElement FinalizeRendering(IRenderable renderable)
        {
            if (renderable == null)
                return null;

            Require.That(AppContext != null, "AppContext must not be null to finalize.");

            renderable.AppContext = AppContext;

            return renderable.FinalizeRendering(e => NeedsRender?.Invoke(e));
        }

        #endregion
        #region --- Events ---

        /// <summary>
        /// Gets the event container for the render element.
        /// </summary>
        public virtual RenderElementEvents Events { get; } = new RenderElementEvents();

        protected UserInterfaceEvent EventData { get; }

        #endregion
        #region --- Services ---

        /// <summary>
        /// Gets the display object for the render element. This contains everything the
        /// rendering system needs to display the render element.
        /// </summary>
        public RenderElementDisplay Display { get; }

        /// <summary>
        /// Gets the style of the render element. This contains things like the default font for
        /// the render element.
        /// </summary>
        public IRenderElementStyle Style => Display.Style;

        #endregion
        #region --- Identification Properties ---

        /// <summary>
        /// Gets the type identifier of this render element.
        /// </summary>
        public virtual string StyleTypeId => GetType().Name.ToLowerInvariant();

        public string StyleClass => Props.StyleClass;

        public string Name => Props.Name;

        public ElementReference Ref { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(StyleTypeId);

            if (!string.IsNullOrWhiteSpace(StyleClass))
            {
                result.Append($".{StyleClass}");
            }

            if (!string.IsNullOrWhiteSpace(Name))
            {
                result.Append($"#{Name}");
            }

            return result.ToString();
        }

        #endregion
        #region --- Focus ---

        /// <summary>
        /// Gets whether or not the render element can hold input focus.
        /// </summary>
        public virtual bool CanHaveFocus => false;

        public bool HasFocus { get; private set; }

        /// <summary>
        /// Called when the element loses focus.
        /// </summary>
        /// <remarks>
        /// If overriding this, be sure to call base.OnBlur() so HasFocus and the Props.OnFocus event get called correctly.
        /// </remarks>
        public virtual void OnBlur()
        {
            HasFocus = false;
            Props.OnBlur?.Invoke(EventData);
        }

        /// <summary>
        /// Called when the element gains focus.
        /// </summary>
        /// <remarks>
        /// If overriding this, be sure to call base.OnFocus() so HasFocus and the Props.OnFocus event get called correctly.
        /// </remarks>
        public virtual void OnFocus()
        {
            HasFocus = true;
            Props.OnFocus?.Invoke(EventData);
        }
        #endregion
        #region --- Layout ---

        /// <summary>
        /// Gets whether or not the element participates in layout.
        /// </summary>
        public virtual bool ParticipateInLayout => true;

        public virtual Size CalcMinContentSize(int? widthConstraint, int? heightConstraint)
        {
            return new Size(1, 1);
        }

        public abstract Size CalcIdealContentSize(IUserInterfaceLayoutContext layoutContext, Size maxSize);

        public abstract void DoLayout(IUserInterfaceLayoutContext layoutContext, Size size);

        protected void DoLayoutForSingleChild(IUserInterfaceLayoutContext layoutContext, Size size, IRenderElement child)
        {
            child.Display.MarginRect = new Rectangle(Point.Zero, size);

            var contentSize = child.Display.ContentRect.Size;

            child.DoLayout(layoutContext, contentSize);
        }

        /// <summary>
        /// Instructs each visible child to PerformLayout for its internals.
        /// Call this method after setting the content/margin sizes for each child.
        /// </summary>
        /// <param name="layoutContext">The layout context service.</param>
        /// <param name="children">The children to PerformLayout on. If this is null,
        /// the <c>Children</c> collection will be used.</param>
        protected void PerformLayoutForChildren(IUserInterfaceLayoutContext layoutContext, IEnumerable<IRenderElement> children = null)
        {
            children = children ?? Children;

            foreach (var item in children.Where(x => x.Display.IsVisible))
            {
                item.DoLayout(layoutContext, item.Display.ContentRect.Size);
            }
        }

        #endregion
        #region --- Updates ---

        public virtual void Update(IUserInterfaceRenderContext renderContext)
        {
            Props.OnUpdate?.Invoke(renderContext);
        }

        #endregion
        #region --- Input ---

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

        /// <summary>
        /// Called when a button is pressed.
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnButtonDown(ButtonStateEventArgs args) { }

        /// <summary>
        /// Called when a button is released.
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnButtonUp(ButtonStateEventArgs args) { }

        public virtual void OnAccept(UserInterfaceActionEventArgs args) { }

        public virtual void OnCancel(UserInterfaceActionEventArgs args) { }

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

        #endregion
        #region --- System Events ---

        IRenderable IRenderable.Render() => this;

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

            TryFinalizeChildren();
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
            {
                if (!string.IsNullOrWhiteSpace(values[i]))
                {
                    return values[i];
                }
            }

            return null;
        }

        void IRenderable.OnRenderResult(IRenderElement result)
        {
        }

        protected virtual void ReconcileChildren(IRenderElement other) { }
        void IRenderElement.ReconcileChildren(IRenderElement other) => ReconcileChildren(other);

        #endregion
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
                {
                    value.Specificity = 1000;
                }

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
                {
                    value.Specificity = -1000;
                }

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
        /// Event raised when a render element receives its update event.
        /// </summary>
        public Action<IUserInterfaceRenderContext> OnUpdate { get; set; }

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
        /// Used by the PropertiesEqual method to determine which properties to skip
        /// when comparing.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual bool CanDoValueComparison(PropertyInfo property)
        {
            return property.Name != "Children";
        }

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
            {
                return false;
            }

            PropertyInfo[] properties = GetType().GetProperties();

            foreach (PropertyInfo prop in properties.Where(x => CanDoValueComparison(x)))
            {
                object myValue = prop.GetValue(this);
                object otherValue = prop.GetValue(other);

                if (myValue == null && otherValue == null)
                {
                    continue;
                }

                if (myValue == null || otherValue == null)
                {
                    return false;
                }

                if (!myValue.Equals(otherValue))
                {
                    return false;
                }
            }

            return true;
        }
    }

    [Obsolete("This class does nothing, references to it are useless.", true)]
    public class RenderElementState
    {

    }

    public static class RenderElementExtensions
    {
        /// <summary>
        /// Compute the ideal size of an element's margin rectangle.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="layoutContext"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public static Size CalcIdealMarginSize(this IRenderElement element,
                                               IUserInterfaceLayoutContext layoutContext,
                                               Size maxSize)
        {
            var contentSize = element.CalcIdealContentSize(layoutContext, maxSize);

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
        public static T CopyFromWidgetProps<T>(this T elementProps, WidgetProps props, bool overwriteExisting = true)
            where T : RenderElementProps
        {
            if (overwriteExisting)
            {
                elementProps.Key = props.Key ?? elementProps.Key;
                elementProps.Name = props.Name ?? elementProps.Name;
                elementProps.Theme = props.Theme ?? elementProps.Theme;
                elementProps.Style = props.Style ?? elementProps.Style;
                elementProps.StyleClass = props.StyleClass ?? elementProps.StyleClass;
                elementProps.DefaultStyle = props.DefaultStyle ?? elementProps.DefaultStyle;
                elementProps.Visible = props.Visible;
            }
            else
            {
                elementProps.Key = props.Key ?? elementProps.Key;
                elementProps.Name = elementProps.Name ?? props.Name;
                elementProps.Theme = elementProps.Theme ?? props.Theme;
                elementProps.Style = elementProps.Style ?? props.Style;
                elementProps.StyleClass = elementProps.StyleClass ?? props.StyleClass;
                elementProps.DefaultStyle = elementProps.DefaultStyle ?? props.DefaultStyle;
                // Not overwriting value for Visible.
            }

            return elementProps;
        }
    }
}
