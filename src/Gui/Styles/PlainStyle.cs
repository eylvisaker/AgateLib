using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.Geometry;
using ERY.AgateLib.GuiBase;

namespace ERY.AgateLib.Gui.Styles
{
    public class PlainStyle : StyleManager
    {
        class PlainButton : ComponentStyle
        {
            PlainStyle style;
            Button btn;

            public PlainButton(PlainStyle style)
            {
                this.style = style;
            }

            public override void InitializeAfterConnect()
            {
                btn = MyComponent as Button;    
            }
            public override void Component_Paint(object sender, EventArgs e)
            {
                Display.FillRect(btn.Bounds, Color.White);

                if (btn.DrawHover)
                    Display.DrawRect(btn.Bounds, Color.Yellow);
                if (btn.DrawDown)
                    Display.DrawRect(btn.Bounds, Color.Blue);

                style.Font.Color = Color.Black;
                style.Font.DisplayAlignment = OriginAlignment.Center;

                style.Font.DrawText(btn.Bounds.Left + btn.Bounds.Width / 2, btn.Bounds.Top + btn.Bounds.Height / 2,
                    btn.Text);
            }
            public override void Component_PaintEnd(object sender, EventArgs e)
            {
            }
            public override void Component_PaintBegin(object sender, EventArgs e)
            {
            }
        }

        private FontSurface mFont;
        public FontSurface Font
        {
            get { return mFont; }
        }

        public PlainStyle()
        {
            mFont = new FontSurface("Arial", 10);
        }

        public override void ConnectStyle(Type componentType, Component target)
        {
            if (componentType.Equals(typeof(Button))) target.AttachStyle(new PlainButton(this));

            else
                throw new NotImplementedException("Style not available for component type " + componentType.ToString());
        }
    }
}
