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

namespace AgateLib.UserInterface
{
    public abstract class Widget<TProps, TState> : IRenderable
        where TProps : WidgetProps
    {
        private TProps props;
        private TState state;

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

    public abstract class Widget<TProps> : Widget<TProps, WidgetState> where TProps : WidgetProps
    {
        public Widget(TProps props) : base(props)
        {
        }
    }

    public class WidgetProps
    {
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
    }

    public class WidgetState
    {
    }
}
