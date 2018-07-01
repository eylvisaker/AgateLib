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
    public class MenuItem : Widget<MenuItemProps, MenuItemState>
    {
        public MenuItem(MenuItemProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new MenuItemElement(new MenuItemElementProps
            {
                OnAccept = Props.OnAccept,
                Children = { new Label(new LabelProps { Text = Props.Text }) }
            });
        }
    }

    public class MenuItemProps : WidgetProps
    {
        public string Text { get; set; }

        public Action OnAccept { get; set; }
    }

    public class MenuItemState : WidgetState
    {
    }

    public class MenuItemElementProps : RenderElementProps
    {
        public Action OnAccept { get; set; }

        public List<IRenderable> Children { get; set; } = new List<IRenderable>();
    }

    public class MenuItemElement : RenderElement<MenuItemElementProps>
    {
        IRenderElement child;

        public MenuItemElement(MenuItemElementProps props) : base(props)
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
        }

        public override void OnAccept()
        {
            base.OnAccept();

            Props.OnAccept?.Invoke();
        }

        public override string StyleTypeId => "menuitem";

        public override bool CanHaveFocus => true;

        public override Size CalcIdealContentSize(IWidgetRenderContext renderContext, Size maxSize)
            => child.CalcIdealContentSize(renderContext, maxSize);

        public override void Draw(IWidgetRenderContext renderContext, Rectangle clientArea)
            => renderContext.DrawChild(clientArea, child);
    }
}
