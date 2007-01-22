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
                dark.R = (byte)(0.7 * dark.R);
                dark.G = (byte)(0.7 * dark.G);
                dark.B = (byte)(0.7 * dark.B);

                Gradient color = new Gradient(btn.BackColor, btn.BackColor, dark, dark);
                Gradient reverse = new Gradient(dark, dark, btn.BackColor, btn.BackColor);

                Rectangle rect = btn.ScreenBounds;
                rect.X++;                rect.Y++;
                rect.Width -= 2;                rect.Height -= 2;

                SetLines();

                if (btn.DrawDown)
                    Display.FillRect(rect, reverse);
                else
                    Display.FillRect(rect, color);

                
                Point textPt = btn.ClientToScreen(new Point(btn.Bounds.Width / 2, btn.Bounds.Height / 2));

                //if (btn.DrawHover)
                //{
                //    style.DrawText(textPt.X + 1, textPt.Y, Color.Yellow, OriginAlignment.Center, btn.Text);
                //    style.DrawText(textPt.X - 1, textPt.Y, Color.Yellow, OriginAlignment.Center, btn.Text);
                //    style.DrawText(textPt.X, textPt.Y + 1, Color.Yellow, OriginAlignment.Center, btn.Text);
                //    style.DrawText(textPt.X, textPt.Y - 1, Color.Yellow, OriginAlignment.Center, btn.Text);
                //}

                style.DrawText(textPt.X, textPt.Y, btn.ForeColor, OriginAlignment.Center, btn.Text);

                Display.DrawLineSegments(lines, Color.Black);

            }
            public override void DoAutoSize()
            {
                Size size = style.Font.StringDisplaySize(btn.Text);

                btn.Size = new Size(size.Width + 4, size.Height + 4);
            }
            private void SetLines()
            {
                // last point isn't used.  so there are strange 1's where the should be 2's.
                Rectangle bounds = btn.ScreenBounds;

                lines[0] = new Point(bounds.X + 1, bounds.Y);
                lines[1] = new Point(bounds.Right - 1, bounds.Y);

                lines[2] = new Point(bounds.Right - 1, bounds.Y + 1);
                lines[3] = new Point(bounds.Right - 1, bounds.Bottom - 1);

                lines[4] = new Point(bounds.X + 1, bounds.Bottom - 1);
                lines[5] = new Point(bounds.Right - 1, bounds.Bottom - 1);

                lines[6] = new Point(bounds.X, bounds.Y + 1);
                lines[7] = new Point(bounds.X, bounds.Bottom - 1);

            }
        }
        class PlainLabel : ComponentStyle
        {
            PlainStyle style;
            Label label;

            public PlainLabel(PlainStyle style)
            {
                this.style = style;
            }
            public override void InitializeAfterConnect()
            {
                label = this.MyComponent as Label;    
            }

            public override void Component_Paint(object sender, EventArgs e)
            {
                if (label.BackColor.A > 0)
                    Display.FillRect(label.ScreenBounds, label.BackColor);

                style.DrawText(label.ScreenLocation, label.ForeColor, OriginAlignment.TopLeft, label.Text);
            }

            public override void DoAutoSize()
            {
                Size size = style.Font.StringDisplaySize(label.Text);

                label.Size = size;
            }
        }
        class PlainWindow : ComponentStyle
        {
            PlainStyle style;
            Window window;

            public PlainWindow(PlainStyle style)
            {
                this.style = style;
            }
            public override void InitializeAfterConnect()
            {
                window = MyComponent as Window;

                AddChildren();
            }
            public override void UpdateClientArea()
            {
                window.SetClientArea(new Rectangle(1, 20, window.Bounds.Width - 2, window.Bounds.Height - 21));
            }

            public override void Component_Paint(object sender, EventArgs e)
            {
                Display.FillRect(window.ScreenBounds, Color.LightGray);
                Display.FillRect(window.ClientToScreen(new Rectangle(new Point(0, 0), window.ClientSize)), window.BackColor);

                style.DrawText(window.ClientToScreen(3, -20), window.ForeColor, OriginAlignment.TopLeft, window.Title);
            }

            public override void DoAutoSize()
            {
            }


            private void AddChildren()
            {
                DragAnchor anchor = new DragAnchor(window);

                anchor.Location = new Point(-1, -20);
                anchor.Size = new Size(window.Bounds.Width, 20);
                anchor.Anchor = Anchor.Top | Anchor.Left | Anchor.Right;

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
            else if (componentType.Equals(typeof(Label))) target.AttachStyle(new PlainLabel(this));
            else if (componentType.Equals(typeof(Window))) target.AttachStyle(new PlainWindow(this));
            else
                throw new NotImplementedException("Style not available for component type " + componentType.ToString());
        }

        internal void DrawText(Point pt, Color color, OriginAlignment align, string text)
        {
            DrawText(pt.X, pt.Y, color, align, text);
        }
        internal void DrawText(int x, int y, Color color, OriginAlignment align, string text)
        {
            Font.DisplayAlignment =align;
            Font.Color = color;

            Font.DrawText(x, y, text);
        }
    }
}
