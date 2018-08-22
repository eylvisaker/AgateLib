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

namespace AgateLib.UserInterface
{
    public class Button : Widget<ButtonProps>
    {
        public Button(ButtonProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new ButtonElement(new ButtonElementProps
            {
                Text = Props.Text,
                OnAccept = Props.OnAccept,
                OnFocus = Props.OnFocus,
                Enabled = Props.Enabled,
                Children = (Props.Children?.Count ?? 0) > 0 ? Props.Children : new List<IRenderable>{ new Label(new LabelProps { Text = Props.Text }) } 
            }.CopyFromWidgetProps(Props));
        }
    }

    public class ButtonProps : WidgetProps
    {
        /// <summary>
        /// Sets text to display in the menu item. This property is ignored if children are explictly added.
        /// </summary>
        public string Text { get; set; }

        public UserInterfaceEventHandler OnAccept { get; set; }

        public UserInterfaceEventHandler OnFocus { get; set; }

        /// <summary>
        /// Specifies whether the button should be enabled for the user to interact with.
        /// Defaults to true.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Adds children to the menu item. If any child components are added, the Text property is ignored.
        /// </summary>
        public List<IRenderable> Children { get; set; } = new List<IRenderable>();
    }

    public class ButtonElement : RenderElement<ButtonElementProps>
    {
        IRenderElement child;

        public ButtonElement(ButtonElementProps props) : base(props)
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
        }

        public override void OnAccept(UserInterfaceActionEventArgs args)
        { 
            if (!Props.Enabled)
                return;

            if (Props.OnAccept != null)
            {
                Props.OnAccept.Invoke(EventData);
                args.Handled = true;
            }
        }

        public override string StyleTypeId => "button";

        public override bool CanHaveFocus => Props.Enabled;

        public override void DoLayout(IUserInterfaceRenderContext renderContext, Size size)
            => DoLayoutForSingleChild(renderContext, size, child);

        public override Size CalcIdealContentSize(IUserInterfaceRenderContext renderContext, Size maxSize)
            => child.CalcIdealMarginSize(renderContext, maxSize);

        public override void Draw(IUserInterfaceRenderContext renderContext, Rectangle clientArea)
            => renderContext.DrawChild(clientArea, child);

        public override string ToString()
        {
            return $"button:{Props.Text}";
        }
    }

    public class ButtonElementProps : RenderElementProps
    {
        public UserInterfaceEventHandler OnAccept { get; set; }

        public List<IRenderable> Children { get; set; } = new List<IRenderable>();

        /// <summary>
        /// A purely informational property, not used by the ButtonElement.
        /// </summary>
        public string Text { get; set; }
    }

}
