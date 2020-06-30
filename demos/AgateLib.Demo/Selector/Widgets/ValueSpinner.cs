using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Demo.Selector.Widgets
{
    public class ValueSpinner<T> : RenderElement<ValueSpinnerProps<T>, ValueSpinnerState>
    {
        Dictionary<SpinnerValue<T>, IRenderElement> mappedChildren = new Dictionary<SpinnerValue<T>, IRenderElement>();
        List<IRenderElement> children = new List<IRenderElement>();
        UserInterfaceEvent<T> valueChangedEvent = new UserInterfaceEvent<T>();

        public ValueSpinner(ValueSpinnerProps<T> props) : base(props)
        {
            SetState(new ValueSpinnerState { CurrentValueIndex = props.InitialValueIndex });
        }

        public override string StyleTypeId => "valuespinner";

        public override IList<IRenderElement> Children
        {
            get => children;
            protected set => throw new InvalidOperationException();
        }

        public override bool CanHaveFocus => true;

        private int ArrowWidth => (int)Math.Ceiling(10 * Display.VisualScaling);

        public override Size CalcIdealContentSize(IUserInterfaceLayoutContext layoutContext, Size maxSize)
        {
            int maxWidth = 0;
            int maxHeight = 0;
            int arrowWidth = ArrowWidth;

            Size childMaxSize = new Size(maxSize.Width - arrowWidth * 2, maxSize.Height);

            foreach (IRenderElement child in children)
            {
                var idealSize = child.CalcIdealMarginSize(layoutContext, childMaxSize);

                maxWidth = Math.Max(maxWidth, idealSize.Width);
                maxHeight = Math.Max(maxHeight, idealSize.Height);
            }

            return new Size(maxWidth + arrowWidth * 2, maxHeight);
        }

        public override void DoLayout(IUserInterfaceLayoutContext layoutContext, Size size)
        {
            Size widgetSize = new Size(size.Width - ArrowWidth * 2, size.Height);
            Rectangle childMarginArea = new Rectangle(new Point(ArrowWidth, 0), widgetSize);

            foreach (var child in children)
            {
                child.Display.MarginRect = childMarginArea;
            }
        }

        public override void Draw(IUserInterfaceRenderContext renderContext, Rectangle clientArea)
        {
            SpinnerValue<T> currentValue = Props.Values[State.CurrentValueIndex];

            DrawChild(renderContext, clientArea, mappedChildren[currentValue]);
        }

        public override void OnUserInterfaceAction(UserInterfaceActionEventArgs args)
        {
            switch (args.Action)
            {
                case UserInterfaceAction.Left:
                    MovePrevious();
                    args.Handled = true;
                    break;

                case UserInterfaceAction.Right:
                    MoveNext();
                    args.Handled = true;
                    break;
            }

            base.OnUserInterfaceAction(args);
        }

        private void MoveNext()
        {
            SetState(s =>
            {
                s.CurrentValueIndex++;

                if (s.CurrentValueIndex >= Props.Values.Count)
                {
                    s.CurrentValueIndex = 0;
                }
            });

            OnValueChanged();
        }


        private void MovePrevious()
        {
            SetState(s =>
            {
                s.CurrentValueIndex--;

                if (s.CurrentValueIndex < 0)
                {
                    s.CurrentValueIndex = Props.Values.Count - 1;
                }
            });

            OnValueChanged();
        }

        private void OnValueChanged()
        {
            Props?.OnValueChanged(valueChangedEvent.Reset(this, Props.Values[State.CurrentValueIndex].Value));
        }

        protected override void OnFinalizeChildren()
        {
            children.Clear();

            foreach (SpinnerValue<T> value in Props.Values)
            {
                IRenderable display = value.Display ?? new Label(value.Value.ToString());
                IRenderElement element = FinalizeRendering(display);

                mappedChildren[value] = element;
                children.Add(element);
            }
        }
    }

    public class ValueSpinnerProps<T> : RenderElementProps
    {
        public List<SpinnerValue<T>> Values { get; set; } = new List<SpinnerValue<T>>();

        public UserInterfaceEventHandler<T> OnValueChanged { get; set; }

        public int InitialValueIndex { get; set; }

    }

    public class ValueSpinnerState
    {
        public int CurrentValueIndex { get; set; }
    }

    public class SpinnerValue<T>
    {
        public SpinnerValue() { }

        public SpinnerValue(T value)
        {
            Value = value;
        }

        public static implicit operator SpinnerValue<T>(T value)
        {
            return new SpinnerValue<T> { Value = value };
        }

        public T Value { get; set; }

        public IRenderable Display { get; set; }
    }
}
