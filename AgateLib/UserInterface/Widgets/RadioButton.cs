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

namespace AgateLib.UserInterface
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
                Checked = Props.Checked,
                Enabled = Props.Enabled,
                Children = { new Label(new LabelProps { Text = Props.Text }) }
            }.CopyFromWidgetProps(Props));
        }
    }

    public class RadioButtonProps : WidgetProps
    {
        /// <summary>
        /// Specifies whether the button should be enabled for the user to interact with.
        /// Defaults to true.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Text that should be shown for the radio button.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Event raised when the radio button is selected in the menu.
        /// </summary>
        public UserInterfaceEventHandler OnSelect { get; set; }

        /// <summary>
        /// Event raised when the radio button is accepted by the user, as in hitting the A button or Enter key.
        /// </summary>
        public UserInterfaceEventHandler OnAccept { get; set; }

        /// <summary>
        /// Gets or sets the initial check state of the radio button.
        /// </summary>
        public bool Checked { get; set; }
    }

    public class RadioButtonElement : RenderElement<RadioButtonElementProps>
    {
        private IRenderElement child;
        private bool isChecked;

        public RadioButtonElement(RadioButtonElementProps props) : base(props)
        {
            if (props.Children.Count == 1)
            {
                child = Finalize(props.Children.First());
            }
            else
            {
                child = new FlexBox(new FlexBoxProps { Children = props.Children });
            }

            IsChecked = Props.Checked;

            Children = new List<IRenderElement> { child };
        }

        private void OnButtonPress(UserInterfaceAction btn)
        {
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

        public override void OnAccept(UserInterfaceActionEventArgs args)
        {
            var parent = Display.System.ParentOf(this);

            if (parent == null)
                return;

            foreach (var child in parent.Children.OfType<RadioButtonElement>().Where(x => x != this))
                child.IsChecked = false;

            IsChecked = true;
            args.Handled = true;

            Props.OnAccept?.Invoke(EventData.Reset(this));
        }

        public override string StyleTypeId => "radiobutton";

        public override bool CanHaveFocus => Props.Enabled;

        public override void DoLayout(IUserInterfaceRenderContext renderContext, Size size)
            => DoLayoutForSingleChild(renderContext, size, child);

        public override Size CalcIdealContentSize(IUserInterfaceRenderContext renderContext, Size maxSize)
            => child.CalcIdealMarginSize(renderContext, maxSize);

        public override void Draw(IUserInterfaceRenderContext renderContext, Rectangle clientArea)
            => renderContext.DrawChild(clientArea, child);
    }

    public class RadioButtonElementProps : RenderElementProps
    {
        public string Text { get; set; }

        public IList<IRenderable> Children { get; set; } = new List<IRenderable>();

        public UserInterfaceEventHandler OnAccept { get; set; }

        public bool Checked { get; set; }
    }
}
