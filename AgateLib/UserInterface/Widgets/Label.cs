using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Widgets
{
    public class Label : Widget<LabelProps, WidgetState>
    {
        public Label(LabelProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new LabelElement(new LabelElementProps {
                Text = Props.Text });
        }
    }

    public class LabelProps : WidgetProps
    {
        public string Text { get; set; }
    }

    public class LabelElement : RenderElement<LabelElementProps>
    {
        public LabelElement(LabelElementProps props) : base(props)
        {
        }

        public override string StyleTypeIdentifier => "Label";

        public override Size CalcIdealContentSize(IWidgetRenderContext renderContext, Size maxSize)
        {
            if (string.IsNullOrEmpty(Props.Text))
            {
                return Style.Font.MeasureString("M");
            }
            else
            {
                return Style.Font.MeasureString(Props.Text);
            }
        }

        public override void Draw(IWidgetRenderContext renderContext, Rectangle clientArea)
        {
            Style.Font.DrawText(renderContext.SpriteBatch,
                clientArea.Location.ToVector2(), Props.Text);
        }
    }

    public class LabelElementProps : RenderElementProps
    {
        public LabelElementProps()
        {
        }

        public string Text { get; set; }
    }
}
