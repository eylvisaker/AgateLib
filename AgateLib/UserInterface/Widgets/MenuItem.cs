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

        public override IRenderElement Render()
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
                child = props.Children.First().Render();
            }
            else
            {
                child = new FlexBox(new FlexBoxProps { Children = props.Children });
            }

            Children = new List<IRenderElement> { child };
        }

        public override string StyleTypeIdentifier => "menuitem";

        public override bool CanHaveFocus => true;

        public override Size CalcIdealContentSize(IWidgetRenderContext renderContext, Size maxSize)
            => child.CalcIdealContentSize(renderContext, maxSize);

        public override void Draw(IWidgetRenderContext renderContext, Rectangle clientArea)
            => renderContext.DrawChild(clientArea, child);
    }
}
