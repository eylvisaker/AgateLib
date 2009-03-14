﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using System.Diagnostics;

namespace AgateLib.Gui.ThemeEngines.Graphite
{
    using Gui;

    public class Graphite : IGuiThemeEngine 
    {
        public Graphite()
        {
            Scheme = GraphiteScheme.DefaultScheme;
        }

        public GraphiteScheme Scheme { get; set; }
        public static bool DebugOutlines { get; set; }


        private void DrawStretchImage(Point loc, Size size,
            Surface surface, Rectangle stretchRegion)
        {
            Rectangle scaled = new Rectangle(
                loc.X + stretchRegion.X,
                loc.Y + stretchRegion.Y,
                size.Width - (surface.SurfaceWidth - stretchRegion.Right) - stretchRegion.X,
                size.Height - (surface.SurfaceHeight - stretchRegion.Bottom) - stretchRegion.Y);

            // draw top left
            surface.Draw(
                new Rectangle(0, 0, stretchRegion.Left, stretchRegion.Top),
                new Rectangle(loc.X, loc.Y, stretchRegion.Left, stretchRegion.Top));

            // draw top middle
            surface.Draw(
                new Rectangle(stretchRegion.Left, 0, stretchRegion.Width, stretchRegion.Top),
                new Rectangle(loc.X + stretchRegion.Left, loc.Y,
                    scaled.Width, stretchRegion.Top));

            // draw top right
            surface.Draw(
                new Rectangle(stretchRegion.Right, 0, surface.SurfaceWidth - stretchRegion.Right, stretchRegion.Top),
                new Rectangle(scaled.Right, loc.Y, surface.SurfaceWidth - stretchRegion.Right, stretchRegion.Top));

            // draw middle left
            surface.Draw(
                new Rectangle(0, stretchRegion.Top, stretchRegion.Left, stretchRegion.Height),
                new Rectangle(loc.X, loc.Y + stretchRegion.Top, stretchRegion.Left, scaled.Height));

            // draw middle
            surface.Draw(
                stretchRegion,
                scaled);

            // draw middle right
            surface.Draw(
                new Rectangle(stretchRegion.Right, stretchRegion.Top, surface.SurfaceWidth - stretchRegion.Right, stretchRegion.Height),
                new Rectangle(scaled.Right, scaled.Top, surface.SurfaceWidth - stretchRegion.Right, scaled.Height));

            // draw bottom left
            surface.Draw(
                new Rectangle(0, stretchRegion.Bottom, stretchRegion.Left, surface.SurfaceHeight - stretchRegion.Bottom),
                new Rectangle(loc.X, scaled.Bottom, stretchRegion.Left, surface.SurfaceHeight - stretchRegion.Bottom));

            // draw bottom middle
            surface.Draw(
                new Rectangle(stretchRegion.Left, stretchRegion.Bottom, stretchRegion.Width, surface.SurfaceHeight - stretchRegion.Bottom),
                new Rectangle(scaled.Left, scaled.Bottom, scaled.Width, surface.SurfaceHeight - stretchRegion.Bottom));

            // draw bottom right
            surface.Draw(
                new Rectangle(stretchRegion.Right, stretchRegion.Bottom, surface.SurfaceWidth - stretchRegion.Right, surface.SurfaceHeight - stretchRegion.Bottom),
                new Rectangle(scaled.Right, scaled.Bottom, surface.SurfaceWidth - stretchRegion.Right, surface.SurfaceHeight - stretchRegion.Bottom));

        }

        #region --- Interface Dispatchers ---

        public void DrawWidget(Widget widget)
        {
            if (widget is GuiRoot)
                return;

            if (DebugOutlines)
            {
                Display.DrawRect(new Rectangle(widget.ScreenLocation, widget.Size),
                    Color.Red);
            }

            if (widget is Window) DrawWindow((Window)widget);
            if (widget is Label) DrawLabel((Label)widget);
            if (widget is Button) DrawButton((Button)widget);
            if (widget is CheckBox) DrawCheckbox((CheckBox)widget);
            if (widget is TextBox) DrawTextBox((TextBox)widget);
        }


        public Size RequestClientAreaSize(Container widget, Size clientSize)
        {
            throw new NotImplementedException();
        }

        public Size CalcMinSize(Widget widget)
        {
            if (widget is Label) return CalcMinLabelSize((Label)widget);
            if (widget is Button) return CalcMinButtonSize((Button)widget);
            if (widget is CheckBox) return CalcMinCheckBoxSize((CheckBox)widget);
            if (widget is TextBox) return CalcMinTextBoxSize((TextBox)widget);

            return Size.Empty;
        }


        #endregion

        #region --- TextBox ---

        private void DrawTextBox(TextBox textBox)
        {
            Surface image = Scheme.TextBox;

            if (textBox.Enabled == false)
                image = Scheme.TextBoxDisabled;
            else if (textBox.MouseIn)
                image = Scheme.TextBoxMouseOver;

            DrawStretchImage(textBox.PointToScreen(Point.Empty), textBox.Size,
                image, Scheme.TextBoxStretchRegion);

            Scheme.ControlFont.DisplayAlignment = OriginAlignment.TopLeft;

            SetControlFontColor(textBox);

            Scheme.ControlFont.DrawText(
                textBox.PointToScreen(Scheme.TextBoxStretchRegion.Location),
                textBox.Text);

            if (textBox.HasFocus)
            {
                Size size = Scheme.ControlFont.StringDisplaySize(
                    textBox.Text.Substring(0, textBox.InsertionPoint));

                Point loc = new Point(
                    size.Width + Scheme.TextBoxStretchRegion.X,
                    Scheme.TextBoxStretchRegion.Y);

                loc = textBox.PointToScreen(loc);
                loc.Y++;
                loc.X += 2;

                DrawInsertionPoint(textBox, loc, Scheme.ControlFont.StringDisplayHeight("M") - 2,
                    Timing.TotalMilliseconds - textBox.IPTime);
            }
        }

        private void DrawInsertionPoint(Widget widget, Point location, int size, double time)
        {
            int val = (int)time / Scheme.InsertionPointBlinkTime;
            //Debug.Print("{0}  {1}", time, val);

            if (val % 2 == 1)
                return;

            Display.DrawLine(location, 
                new Point(location.X, location.Y + size),
                Color.Black);
        }



        private Size CalcMinTextBoxSize(TextBox textBox)
        {
            Size retval = new Size();

            retval.Width = 40;
            retval.Height = Scheme.ControlFont.StringDisplayHeight("M");
            retval.Height += Scheme.TextBox.SurfaceHeight - Scheme.TextBoxStretchRegion.Height;

            return retval;
        }

        #endregion
        #region --- CheckBox ---

        private void DrawCheckbox(CheckBox checkbox)
        {
            Surface surf = checkbox.Checked ? Scheme.CheckBoxDown : Scheme.CheckBoxUp;
            Point destPoint = checkbox.PointToScreen(
                Origin.Calc(OriginAlignment.CenterLeft, checkbox.Size));

            surf.DisplayAlignment = OriginAlignment.CenterLeft;
            surf.Draw(destPoint);

            SetControlFontColor(checkbox);

            destPoint.X += surf.DisplayWidth + Scheme.CheckBoxSpacing;
            Scheme.ControlFont.DrawText(destPoint, checkbox.Text);
        }

        private Size CalcMinCheckBoxSize(CheckBox checkbox)
        {
            Size text = Scheme.ControlFont.StringDisplaySize(checkbox.Text);
            Size box = Scheme.CheckBoxUp.SurfaceSize;

            return new Size(
                box.Width + Scheme.CheckBoxSpacing + text.Width,
                Math.Max(box.Height, text.Height));
        }

        #endregion
        #region --- Label ---

        private void DrawLabel(Label label)
        {
            Point location = new Point();

            location = DisplayLib.Origin.Calc(label.TextAlignment, label.Size);
            location.X += label.ScreenLocation.X;
            location.Y += label.ScreenLocation.Y;

            SetControlFontColor(label);

            Scheme.ControlFont.DisplayAlignment = label.TextAlignment;
            Scheme.ControlFont.DrawText(location, label.Text);
        }

        private Size CalcMinLabelSize(Label label)
        {
            Size retval = Scheme.ControlFont.StringDisplaySize(label.Text);

            return retval;
        }

        #endregion
        #region --- Button ---

        private void DrawButton(Button button)
        {
            Surface image = Scheme.Button;

            if (button.DrawActivated)
                image = Scheme.ButtonActivate;
            else if (button.Enabled == false)
                image = Scheme.ButtonDisabled;
            else if (button.MouseIn)
                image = Scheme.ButtonMouseOver;

            DrawStretchImage(button.PointToScreen(Point.Empty), button.Size,
                image, Scheme.ButtonStretchRegion);

            SetControlFontColor(button);

            Scheme.ControlFont.DisplayAlignment = OriginAlignment.Center;
            Scheme.ControlFont.DrawText(
                button.PointToScreen(Origin.Calc(OriginAlignment.Center, button.Size)),
                button.Text);
        }

        private Size CalcMinButtonSize(Button button)
        {
            Size textSize = Scheme.ControlFont.StringDisplaySize(button.Text);
            Size buttonBorder = new Size(
                Scheme.Button.SurfaceWidth - Scheme.ButtonStretchRegion.Width,
                Scheme.Button.SurfaceHeight - Scheme.ButtonStretchRegion.Height);

            return new Size(
                textSize.Width + buttonBorder.Width, textSize.Height + buttonBorder.Height);
        }

        #endregion
        #region --- Window ---

        public void DrawWindow(Window window)
        {
            DrawWindowBackground(window);
            DrawWindowTitle(window);
            DrawWindowDecorations(window);
        }

        public int WindowTitlebarSize { 
            get
            {
                return Scheme.TitleFont.StringDisplayHeight("M") + 6;
            }
        }

        protected virtual void DrawWindowBackground(Window window)
        {
            Display.FillRect(new Rectangle(
                window.ScreenLocation, window.Size), Scheme.WindowBackColor);

            Rectangle rect = new Rectangle(
                window.ScreenLocation, window.Size);

            Display.DrawRect(rect, Scheme.WindowBorderColor);

            if (Scheme.DropShadowSize > 0)
                DrawDropShadow(rect);
            else
                Display.DrawRect(rect, Scheme.WindowBorderColor);
        }

        private void DrawDropShadow(Rectangle rect)
        {
            for (int i = 0; i <= Scheme.DropShadowSize; i++)
            {
                Color fadeColor = Scheme.WindowBorderColor;
                fadeColor.A = (byte)(
                    fadeColor.A * (Scheme.DropShadowSize - i) / (2 * Scheme.DropShadowSize));

                Display.DrawRect(rect, fadeColor);

                rect.X--;
                rect.Y--;
                rect.Width += 2;
                rect.Height += 2;
            }
        }
        protected virtual void DrawWindowTitle(Window window)
        {
            Point windowLocation = window.ScreenLocation;

            Display.FillRect(new Rectangle(windowLocation.X + 1, windowLocation.Y - 1,
                window.Size.Width - 2, WindowTitlebarSize - 2),
                Color.LightBlue);

            Point fontPosition = new Point(windowLocation.X + 8, windowLocation.Y + 3);
            if (Scheme.CenterTitle)
            {
                fontPosition.X = windowLocation.X + window.Width / 2;
                fontPosition.Y = windowLocation.Y + WindowTitlebarSize / 2;
                Scheme.TitleFont.DisplayAlignment = OriginAlignment.Center;
            }

            Scheme.TitleFont.Color = Scheme.FontColor;

            Scheme.TitleFont.DrawText(
                fontPosition,
                window.Text);

            Scheme.TitleFont.DisplayAlignment = OriginAlignment.TopLeft;
        }
        protected virtual void DrawWindowDecorations(Window window)
        {
            Scheme.CloseButton.DisplayAlignment = OriginAlignment.TopRight;
            Scheme.CloseButton.Draw(
                new Point(window.ScreenLocation.X + window.Size.Width,
                    window.ScreenLocation.Y));
        }

        #endregion


        
        public Rectangle GetClientArea(Container widget)
        {
            if (widget is Window) return GetWindowClientArea((Window)widget);

            return new Rectangle(Point.Empty, widget.Size);
        }
        public Rectangle GetWindowClientArea(Window widget)
        {
            return new Rectangle(
                3, WindowTitlebarSize, 
                widget.Width - 6, widget.Height - WindowTitlebarSize - 3);
        }


        private void SetControlFontColor(Widget widget)
        {
            if (widget.Enabled)
                Scheme.ControlFont.Color = Scheme.FontColor;
            else
                Scheme.ControlFont.Color = Scheme.FontColorDisabled;
        }
    }
}