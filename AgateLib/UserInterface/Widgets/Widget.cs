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

        public string StyleId { get; set; }

        public string StyleClass { get; set; }

        public InlineElementStyle Style { get; set; }
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

        Action<IRenderable> IRenderable.NeedsRender { get => NeedsRender; set => NeedsRender = value; }
        protected Action<IRenderable> NeedsRender { get; private set; }

        /// <summary>
        /// Read-only props. Do not modify props, instead call SetProps method.
        /// Props should not be modified within a widget, instead they should
        /// only be updated by the widget's owner.
        /// </summary>
        protected TProps Props => props;

        public void SetProps(TProps props)
        {
            this.props = props;

            NeedsRender?.Invoke(this);
        }

        public void UpdateProps(Action<TProps> propsUpdater)
        {
            propsUpdater(props);

            NeedsRender?.Invoke(this);
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

            NeedsRender?.Invoke(this);
        }

        protected void SetState(TState newState)
        {
            this.state = newState;

            NeedsRender?.Invoke(this);
        }

        internal IDisplaySystem DisplaySystem { get; set; }

        #endregion

        public string Name => props.Name;

        public virtual void Initialize()
        {
        }

        public abstract IRenderable Render();

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


        Action<IRenderable> IRenderable.NeedsRender { get; set; }

        public event EventHandler FocusGained;
        public event EventHandler FocusLost;
        public event EventHandler<MenuInputButton> ButtonDown;
        public event EventHandler<MenuInputButton> ButtonUp;
        public event EventHandler<Rectangle> AfterDraw;

        /// <summary>
        /// Gets or sets the name of the widget.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the WidgetDisplay object that controls how the widget is
        /// displayed on screen.
        /// </summary>
        public RenderElementDisplay Display { get; } = new RenderElementDisplay(new RenderElementProps());

        /// <summary>
        /// Gets the readonly collection of children.
        /// </summary>
        public virtual IList<IRenderElement> Children => null;

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

        string IRenderElement.StyleTypeId => StyleTypeIdentifier;

        #endregion

        public virtual bool CanHaveFocus => false;

        public IRenderElementStyle Style => throw new NotImplementedException();

        public string StyleClass => throw new NotImplementedException();

        public string StyleId => throw new NotImplementedException();

        public RenderElementProps Props => throw new NotImplementedException();

        /// <summary>
        /// Compute the ideal size of the content of the widget.
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public abstract Size CalcIdealContentSize(
            IWidgetRenderContext renderContext,
            Size maxSize);

        public abstract void Draw(IWidgetRenderContext renderContext, Rectangle clientArea);

        public abstract void Update(IWidgetRenderContext renderContext);

        public virtual void Initialize() { }

        public virtual void OnInputEvent(InputEventArgs widgetEventArgs)
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
                    OnDrawComplete(widgetEventArgs.Area);
                    break;
            }
        }

        private void OnDrawComplete(Rectangle location)
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

        public virtual IRenderable Render()
        {
            return this;
        }

        public void OnBlur()
        {
            throw new NotImplementedException();
        }

        void IRenderElement.OnFocus()
        {
            throw new NotImplementedException();
        }

        public void OnAccept()
        {
            throw new NotImplementedException();
        }

        public void DoLayout(IWidgetRenderContext renderContext, Size size)
        {
            throw new NotImplementedException();
        }

        public void OnSelect()
        {
            throw new NotImplementedException();
        }

        public void OnChildNavigate(IRenderElement child, MenuInputButton button)
        {
            throw new NotImplementedException();
        }

        public void SetProps(RenderElementProps props)
        {
            throw new NotImplementedException();
        }

        public void OnWillUnmount()
        {
            throw new NotImplementedException();
        }

        public void OnDidMount()
        {
            throw new NotImplementedException();
        }

        public void OnChildrenUpdated()
        {
            throw new NotImplementedException();
        }

        public void OnReconciliationCompleted()
        {
            throw new NotImplementedException();
        }
    }


    public abstract class Widget<TProps> : Widget<TProps, WidgetState> where TProps : WidgetProps
    {
        public Widget(TProps props) : base(props)
        {
        }
    }

    public static class WidgetExtensions
    {
        /// <summary>
        /// Recalculates the size of a child widget.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="renderContext"></param>
        /// <param name="parentMaxSize"></param>
        [Obsolete("This should be moved somewhere else.")]
        public static Size RecalculateSize(this IRenderElement element, IWidgetRenderContext renderContext, Size parentMaxSize)
        {
            element.Display.Region.Size.ParentMaxSize = parentMaxSize;

            element.Display.Region.Size.IdealContentSize
                = element.CalcIdealContentSize(renderContext, parentMaxSize);

            return element.Display.Region.Size.IdealContentSize;
        }
    }
}
