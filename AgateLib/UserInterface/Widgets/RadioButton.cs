using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Widgets
{
    public class RadioButton : Widget<RadioButtonProps>
    {
        public RadioButton(RadioButtonProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new RadioButtonElement(new RadioButtonElementProps
            {
                Text = Props.Text,
                OnFocus = Props.OnSelect,
                OnAccept = Props.OnAccept,
                Children = { new Label(new LabelProps { Text = Props.Text }) }
            });
        }
    }

    public class RadioButtonProps : WidgetProps
    {
        /// <summary>
        /// Text that should be shown for the radio button.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Event raised when the radio button is selected in the menu.
        /// </summary>
        public Action OnSelect { get; set; }

        /// <summary>
        /// Event raised when the radio button is accepted by the user, as in hitting the A button or Enter key.
        /// </summary>
        public Action OnAccept { get; set; }
    }

    public class RadioButtonElement : RenderElement<RadioButtonElementProps>
    {
        private IRenderElement child;
        private bool isChecked;

        private ButtonPress<MenuInputButton> buttonPress = new ButtonPress<MenuInputButton>();

        public RadioButtonElement(RadioButtonElementProps props) : base(props)
        {
            if (props.Children.Count == 1)
            {
                child = props.Children.First().Finalize();
            }
            else
            {
                child = new FlexBox(new FlexBoxProps { Children = props.Children });
            }

            Children = new List<IRenderElement> { child };

            buttonPress.Press += OnButtonPress;
        }

        private void OnButtonPress(MenuInputButton btn)
        {
            if (btn == MenuInputButton.Accept)
            {
                OnAccept();
            }
            else
            {
                Parent.OnChildNavigate(this, btn);
            }
        }

        public bool IsChecked
        {
            get => isChecked;
            private set
            {
                isChecked = value;

                if (isChecked)
                {
                    Display.PseudoClasses.Add("checked");
                }
                else
                {
                    Display.PseudoClasses.Remove("checked");
                }
            }
        }

        public override void OnAccept()
        {
            base.OnAccept();

            var parent = Display.System.ParentOf(this);

            if (parent == null)
                return;

            foreach (var child in parent.Children.OfType<RadioButtonElement>().Where(x => x != this))
                child.IsChecked = false;

            IsChecked = true;

            Props.OnAccept?.Invoke();
        }

        public override void OnFocus()
        {
            Props.OnFocus?.Invoke();
        }

        public override void OnBlur()
        {
            base.OnBlur();
            buttonPress.Clear();
        }

        public override void OnSelect()
        {
            Props.OnFocus?.Invoke();
        }

        public override void DoLayout(IWidgetRenderContext renderContext, Size size)
        {
            child.DoLayout(renderContext, size);
        }

        public override string StyleTypeId => "radiobutton";

        public override bool CanHaveFocus => true;
        
        public override Size CalcIdealContentSize(IWidgetRenderContext renderContext, Size maxSize)
            => child.CalcIdealContentSize(renderContext, maxSize);

        public override void Draw(IWidgetRenderContext renderContext, Rectangle clientArea)
            => renderContext.DrawChild(clientArea, child);

        public override void OnInputEvent(InputEventArgs input)
        {
            buttonPress.HandleInputEvent(input);
        }
    }

    public class RadioButtonElementProps : RenderElementProps
    {
        public string Text { get; set; }

        public IList<IRenderable> Children { get; set; } = new List<IRenderable>();

        public Action OnAccept { get; set; }

        public Action OnFocus { get; set; }
    }
}
