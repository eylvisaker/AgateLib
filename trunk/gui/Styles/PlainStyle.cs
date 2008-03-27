//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.Geometry;

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

                if (btn.Enabled == false)
                    Display.FillRect(rect, color.AverageColor);
                else if (btn.DrawDown)
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

                if (btn.Enabled == true)
                {
                    style.DrawText(textPt.X, textPt.Y, OriginAlignment.Center, btn.ForeColor, btn.Text);
                }
                else
                {
                    style.DrawText(textPt.X + 1, textPt.Y + 1, OriginAlignment.Center, Color.White, btn.Text);
                    style.DrawText(textPt.X, textPt.Y, OriginAlignment.Center, Color.Gray, btn.Text);
                }

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

                style.DrawText(label.ScreenLocation, OriginAlignment.TopLeft, label.ForeColor, label.Text);
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
            DragAnchor dragAnchor;
            SizeAnchor sizeAnchor;
            Button closeButton;
            readonly Size closeSize = new Size(35, 18);
            const int titleSize = 25;

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
                window.SetClientArea(new Rectangle(1, titleSize, window.Bounds.Width - 2, window.Bounds.Height - titleSize - 1));

                dragAnchor.Size = new Size(window.Bounds.Width, 20); 
                closeButton.Location = new Point(window.ClientSize.Width - closeSize.Width - 8, -titleSize);
                sizeAnchor.Location = new Point(window.ClientSize.Width - sizeAnchor.Width, 
                                                window.ClientSize.Height -sizeAnchor.Height);
            }

            public override void Component_Paint(object sender, EventArgs e)
            {
                Display.FillRect(window.ScreenBounds, Color.LightGray);
                Display.FillRect(window.ClientToScreen(new Rectangle(new Point(0, 0), window.ClientSize)), window.BackColor);

                style.DrawText(window.ClientToScreen(7, -titleSize / 2), OriginAlignment.CenterLeft,
                    window.ForeColor, window.Title);
            }

            public override void DoAutoSize()
            {
            }

            private void AddChildren()
            {
                dragAnchor = new DragAnchor(window);
                dragAnchor.Location = new Point(-1, -titleSize);
                dragAnchor.Anchor = Anchor.Top | Anchor.Left | Anchor.Right;

                closeButton = new Button(window, "X");
                closeButton.AutoSize = false;
                closeButton.Size = closeSize;
                closeButton.Anchor = Anchor.Top | Anchor.Right;
                closeButton.Click += new EventHandler(closeButton_Click);
                sizeAnchor = new SizeAnchor(window);
                sizeAnchor.Anchor = Anchor.Bottom | Anchor.Right;
            }

            void closeButton_Click(object sender, EventArgs e)
            {
                window.Close();
            }

        }

        private FontSurface mFont;
        public FontSurface Font
        {
            get { return mFont; }
        }

        public PlainStyle()
        {
            mFont = new FontSurface("Arial", 12);
        }

        public override void ConnectStyle(Type componentType, Component target)
        {
            if (componentType.Equals(typeof(Button))) target.AttachStyle(new PlainButton(this));
            else if (componentType.Equals(typeof(Label))) target.AttachStyle(new PlainLabel(this));
            else if (componentType.Equals(typeof(Window))) target.AttachStyle(new PlainWindow(this));
            else
                throw new NotImplementedException("Style not available for component type " + componentType.ToString());
        }

        internal void DrawText(Point pt, OriginAlignment align, Color color, string text)
        {
            DrawText(pt.X, pt.Y, align, color, text);
        }
        internal void DrawText(int x, int y, OriginAlignment align, Color color, string text)
        {
            Font.DisplayAlignment = align;
            Font.Color = color;

            Font.DrawText(x, y, text);
        }
    }
}
