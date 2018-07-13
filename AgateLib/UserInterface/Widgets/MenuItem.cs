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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Widgets
{
    public class MenuItem : Widget<MenuItemProps>
    {
        public MenuItem(MenuItemProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new MenuItemElement(new MenuItemElementProps
            {
                Text = Props.Text,
                OnAccept = Props.OnAccept,
                Enabled = Props.Enabled,
                Children = { new Label(new LabelProps { Text = Props.Text }) }
            });
        }
    }

    public class MenuItemProps : WidgetProps
    {
        public string Text { get; set; }

        public Action OnAccept { get; set; }

        public Action OnSelect { get; set; }

        /// <summary>
        /// Specifies whether the button should be enabled for the user to interact with.
        /// Defaults to true.
        /// </summary>
        public bool Enabled { get; set; } = true;
    }

    public class MenuItemElement : RenderElement<MenuItemElementProps>
    {
        IRenderElement child;

        private ButtonPress<MenuInputButton> buttonPress = new ButtonPress<MenuInputButton>();

        public MenuItemElement(MenuItemElementProps props) : base(props)
        {
            if (props.Children.Count == 1)
            {
                child = Finalize(props.Children.First());
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

        public override void OnBlur()
        {
            base.OnBlur();
            buttonPress.Clear();
        }

        public override void OnAccept()
        {
            base.OnAccept();

            Props.OnAccept?.Invoke();
        }

        public override string StyleTypeId => "menuitem";

        public override bool CanHaveFocus => Props.Enabled;

        public override void DoLayout(IWidgetRenderContext renderContext, Size size)
            => DoLayoutForSingleChild(renderContext, size, child);

        public override Size CalcIdealContentSize(IWidgetRenderContext renderContext, Size maxSize)
            => child.CalcIdealMarginSize(renderContext, maxSize);

        public override void Draw(IWidgetRenderContext renderContext, Rectangle clientArea)
            => renderContext.DrawChild(clientArea, child);

        public override void OnInputEvent(InputEventArgs input)
        {
            buttonPress.HandleInputEvent(input);
        }
    }

    public class MenuItemElementProps : RenderElementProps
    {
        public Action OnAccept { get; set; }

        public List<IRenderable> Children { get; set; } = new List<IRenderable>();

        /// <summary>
        /// A purely informational property, not used by the MenuItemElement.
        /// </summary>
        public string Text { get; set; }
    }

}
