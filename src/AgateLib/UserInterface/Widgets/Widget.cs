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

using AgateLib.Quality;
using System;

namespace AgateLib.UserInterface
{
    public abstract class Widget<TProps, TState> : IRenderable
        where TProps : WidgetProps
    {
        private TProps props;
        private TState state;
        private IUserInterfaceAppContext appContext;

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

        public void SetProps(Action<TProps> propsUpdater)
        {
            propsUpdater(props);

            NeedsRender?.Invoke(this);
        }

        #endregion
        #region --- State Management ---

        protected TState State => state;

        protected void ReplaceState(Func<TState, TState> stateMutator)
        {
            SetState(stateMutator(state));
        }

        protected void SetState(Action<TState> stateMutator)
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
        #region --- AppContext Management ---

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

                this.appContext = value;

                OnReceiveAppContext();
            }
        }

        /// <summary>
        /// Method called when the app context is set.
        /// </summary>
        protected virtual void OnReceiveAppContext()
        {

        }

        #endregion

        public abstract IRenderable Render();

        void IRenderable.OnRenderResult(IRenderElement result)
        {
            if (Props.Ref != null)
            {
                Props.Ref.Current = result;
                result.Ref = Props.Ref;
            }
        }
    }

    public abstract class Widget<TProps> : Widget<TProps, object> where TProps : WidgetProps
    {
        public Widget(TProps props) : base(props)
        {
        }
    }

    public class WidgetProps
    {
        /// <summary>
        /// The key used during reconciliation to match this element between updates.
        /// </summary>
        public string Key { get; set; }

        public string Name { get; set; }

        public string Theme { get; set; }

        public string StyleClass { get; set; }

        public InlineElementStyle DefaultStyle { get; set; }

        public InlineElementStyle Style { get; set; }

        public bool Visible { get; set; } = true;

        /// <summary>
        /// Set to an ElementReference to capture a reference to the render element 
        /// when this widget is rendered.
        /// </summary>
        public ElementReference Ref { get; set; }

        public bool PlaySounds { get; set; } = true;
    }

    [Obsolete("This class serves no purpose.", true)]
    public class WidgetState
    {
    }

    public static class WidgetExtensions
    {
        /// <summary>
        /// Copies standard WidgetProps members from another WidgetProps structure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="widgetProps"></param>
        /// <param name="props"></param>
        /// <returns></returns>
        public static T CopyFrom<T>(this T widgetProps, WidgetProps props)
            where T : WidgetProps
        {
            widgetProps.Key = widgetProps.Key ?? props.Key;
            widgetProps.Name = widgetProps.Name ?? props.Name;
            widgetProps.Theme = widgetProps.Theme ?? props.Theme;
            widgetProps.Style = widgetProps.Style ?? props.Style;
            widgetProps.StyleClass = widgetProps.StyleClass ?? props.StyleClass;
            widgetProps.DefaultStyle = widgetProps.DefaultStyle ?? props.DefaultStyle;
            widgetProps.Visible = props.Visible;

            return widgetProps;
        }
    }
}
