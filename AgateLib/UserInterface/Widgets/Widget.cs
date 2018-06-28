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
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public interface IWidget : IRenderable
    {
        string Name { get; }

        void Initialize();

        void Update(IWidgetRenderContext renderContext);
    }

    [Obsolete]
    public interface IRenderWidget : IWidget, IRenderElement
    {
    }

    public class WidgetProps
    {
        public string Name { get; set; }
    }

    public class WidgetState
    {
        public bool IsDirty { get; protected set; }
    }

    public abstract class Widget<TProps, TState> : IWidget
        where TProps : WidgetProps where TState : WidgetState
    {
        private TProps props;

        public Widget(TProps props)
        {
            this.props = props;
        }

        #region --- Props Management ---

        /// <summary>
        /// Read-only props. Do not modify props, instead call SetProps method.
        /// Props should not be modified within a widget, instead they should
        /// only be updated by the widget's owner.
        /// </summary>
        protected TProps Props => props;

        public void SetProps(TProps props)
        {
            this.props = props;
        }

        public void UpdateProps(Action<TProps> propsUpdater)
        {
            propsUpdater(props);
        }

        #endregion
        #region --- State Management ---

        private TState state;

        protected TState State => state;

        protected void ReplaceState(Func<TState, TState> stateMutator)
        {
            SetState(stateMutator(state));
        }
        protected void UpdateState(Action<TState> stateMutator)
        {
            stateMutator(state);
        }

        protected void SetState(TState newState)
        {
            this.state = newState;

            NeedsRender = true;
        }

        internal bool NeedsRender { get; private set; }

        #endregion

        public string Name => props.Name;

        public virtual void Initialize()
        {
        }

        public abstract IRenderElement Render();

        public virtual void Update(IWidgetRenderContext renderContext)
        {
        }
    }

    [Obsolete]
    public abstract class RenderWidget : IRenderWidget
    {
        /// <summary>
        /// Initializes a widget and sets its name.
        /// </summary>
        /// <param name="name">If name is null, it is replaced by the empty string.</param>
        protected RenderWidget(string name = "")
        {
            Name = name ?? "";
        }

        public event EventHandler FocusGained;
        public event EventHandler FocusLost;
        public event EventHandler<MenuInputButton> ButtonDown;
        public event EventHandler<MenuInputButton> ButtonUp;
        public event EventHandler<Point> AfterDraw;

        /// <summary>
        /// Gets or sets the name of the widget.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the WidgetDisplay object that controls how the widget is
        /// displayed on screen.
        /// </summary>
        public WidgetDisplay Display { get; } = new WidgetDisplay();

        /// <summary>
        /// Gets the readonly collection of children.
        /// </summary>
        public virtual IEnumerable<IRenderElement> Children => null;

        /// <summary>
        /// Gets the child of this widget that has focus.
        /// </summary>
        public virtual IRenderWidget Focus
        {
            get => null;
            set => throw new InvalidOperationException();
        }

        #region --- Style Type Identifier ---

        protected virtual string StyleTypeIdentifier => GetType().Name;

        string IRenderElement.StyleTypeIdentifier => StyleTypeIdentifier;

        #endregion

        public virtual bool CanHaveFocus => false;

        public object Tag { get; set; }
        IRenderElement IRenderElement.Focus { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IRenderElementStyle Style => throw new NotImplementedException();

        /// <summary>
        /// Compute the ideal size of the content of the widget.
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public abstract Size ComputeIdealSize(
            IWidgetRenderContext renderContext,
            Size maxSize);

        public abstract void Draw(IWidgetRenderContext renderContext, Rectangle clientArea);

        public abstract void Update(IWidgetRenderContext renderContext);

        public virtual void Initialize() { }

        public virtual void ProcessEvent(WidgetEventArgs widgetEventArgs)
        {
            switch (widgetEventArgs.EventType)
            {
                case WidgetEventType.ButtonDown:
                    OnButtonDown(widgetEventArgs.Button);
                    break;

                case WidgetEventType.ButtonUp:
                    OnButtonUp(widgetEventArgs.Button);
                    break;

                case WidgetEventType.FocusLost:
                    OnFocusLost();
                    break;

                case WidgetEventType.FocusGained:
                    OnFocusGained();
                    break;

                case WidgetEventType.DrawComplete:
                    OnDrawComplete(widgetEventArgs.Location);
                    break;
            }
        }

        private void OnDrawComplete(Point location)
        {
            AfterDraw?.Invoke(this, location);
        }

        protected virtual void OnButtonDown(MenuInputButton button)
        {
            ButtonDown?.Invoke(this, button);
        }

        protected virtual void OnButtonUp(MenuInputButton button)
        {
            ButtonUp?.Invoke(this, button);
        }

        protected virtual void OnFocusLost()
        {
            FocusLost?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnFocusGained()
        {
            FocusGained?.Invoke(this, EventArgs.Empty);
        }

        public override string ToString()
        {
            return $"{GetType().Name}: {Name}";
        }

        public virtual IRenderElement Render()
        {
            return this;
        }
    }

    public static class WidgetExtensions
    {
        /// <summary>
        /// Recalculates the size of a child widget.
        /// </summary>
        /// <param name="widget"></param>
        /// <param name="renderContext"></param>
        /// <param name="parentMaxSize"></param>
        public static Size RecalculateSize(this IRenderElement widget, IWidgetRenderContext renderContext, Size parentMaxSize)
        {
            widget.Display.Region.Size.ParentMaxSize = parentMaxSize;

            widget.Display.Region.Size.IdealContentSize
                = widget.ComputeIdealSize(renderContext, parentMaxSize);

            return widget.Display.Region.Size.IdealContentSize;
        }

        /// <summary>
        /// Recalculates the size of a widget's own layout.
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="owner"></param>
        /// <param name="renderContext"></param>
        /// <param name="parentMaxSize"></param>
        public static Size RecalculateSize(this IWidgetLayout layout, IRenderElement owner, IWidgetRenderContext renderContext, Size parentMaxSize)
        {
            owner.Display.Region.Size.ParentMaxSize = parentMaxSize;

            return layout.ComputeIdealSize(parentMaxSize, renderContext);
        }
    }
}
