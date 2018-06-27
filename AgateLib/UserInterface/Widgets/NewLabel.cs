using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Widgets
{
    public class NewLabel : Widget<LabelProps, WidgetState>
    {
        public NewLabel(LabelProps props) : base(props)
        {
        }

        public override IRenderElement Render()
        {
            return new LabelElement(new LabelElementProps(Props.Text));
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

        public override Size ComputeIdealSize(IWidgetRenderContext renderContext, Size maxSize)
        {
            if (string.IsNullOrEmpty(Props.Text))
            {
                return Display.Font.MeasureString("M");
            }
            else
            {
                return Display.Font.MeasureString(Props.Text);
            }
        }

        public override void Draw(IWidgetRenderContext renderContext, Rectangle clientArea)
        {
            Display.Font.DrawText(renderContext.SpriteBatch,
                clientArea.Location.ToVector2(), Props.Text);
        }
    }

    public class LabelElementProps : RenderElementProps
    {
        public LabelElementProps(string text)
        {
            this.Text = text;
        }

        public string Text { get; }
    }

}
