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
            Point[] lines = new Point[8];

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
                Color dark = btn.BackColor;
                dark.R = (byte)(0.8 * dark.R);
                dark.G = (byte)(0.8 * dark.G);
                dark.B = (byte)(0.8 * dark.B);

                Gradient color = new Gradient(btn.BackColor, btn.BackColor, dark, dark);
                Gradient reverse = new Gradient(dark, dark, btn.BackColor, btn.BackColor);

                Rectangle rect = btn.Bounds;
                rect.X++;                rect.Y++;
                rect.Width -= 2;                rect.Height -= 2;

                SetLines();

                if (btn.DrawDown)
                    Display.FillRect(rect, reverse);
                else
                    Display.FillRect(rect, color);

                
                Point textPt = new Point(
                    btn.Bounds.Left + btn.Bounds.Width / 2, btn.Bounds.Top + btn.Bounds.Height / 2);

                if (btn.DrawHover)
                {
                    style.DrawText(textPt.X + 1, textPt.Y, Color.Yellow, OriginAlignment.Center, btn.Text);
                    style.DrawText(textPt.X - 1, textPt.Y, Color.Yellow, OriginAlignment.Center, btn.Text);
                    style.DrawText(textPt.X, textPt.Y + 1, Color.Yellow, OriginAlignment.Center, btn.Text);
                    style.DrawText(textPt.X, textPt.Y - 1, Color.Yellow, OriginAlignment.Center, btn.Text);
                }

                style.DrawText(textPt.X, textPt.Y, btn.ForeColor, OriginAlignment.Center, btn.Text);

                Display.DrawLineSegments(lines, Color.Black);

            }

            private void SetLines()
            {
                // last point isn't used.

                lines[0] = new Point(btn.Bounds.X + 1, btn.Bounds.Y);
                lines[1] = new Point(btn.Bounds.Right - 1, btn.Bounds.Y);

                lines[2] = new Point(btn.Bounds.Right, btn.Bounds.Y + 1);
                lines[3] = new Point(btn.Bounds.Right, btn.Bounds.Bottom - 1);

                lines[4] = new Point(btn.Bounds.X + 1, btn.Bounds.Bottom);
                lines[5] = new Point(btn.Bounds.Right - 1, btn.Bounds.Bottom);

                lines[6] = new Point(btn.Bounds.X, btn.Bounds.Y + 1);
                lines[7] = new Point(btn.Bounds.X, btn.Bounds.Bottom - 1);
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

        internal void DrawText(int x, int y, Color color, OriginAlignment align, string text)
        {
            Font.DisplayAlignment =align;
            Font.Color = color;

            Font.DrawText(x, y, text);
        }
    }
}
