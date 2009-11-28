using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Gui.ThemeEngines.Mercury.Cache;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	public class Mercury : IGuiThemeEngine
	{
		public Mercury()
			: this(MercuryScheme.CreateDefaultScheme())
		{ }
		public Mercury(MercuryScheme scheme)
		{
			this.Scheme = scheme;
		}

		public MercuryScheme Scheme { get; set; }
		public static bool DebugOutlines { get; set; }

		#region --- Updates ---

		public void Update(GuiRoot guiRoot)
		{
			UpdateCaches(guiRoot);
		}

		private void UpdateCaches(Container container)
		{
			foreach (var widget in container.Children)
			{
				if (widget is Container)
					UpdateCaches((Container)widget);
				else if (widget is TextBox)
					UpdateCache((TextBox)widget);
			}
		}

		#endregion

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
			if (widget is RadioButton) DrawRadioButton((RadioButton)widget);
			if (widget is TextBox) DrawTextBox((TextBox)widget);
			if (widget is ListBox) DrawListBox((ListBox)widget);
			if (widget is VerticalScrollBar) Scheme.VerticalScrollBar.DrawScrollBar((ScrollBar)widget);
			if (widget is HorizontalScrollBar) Scheme.HorizontalScrollBar.DrawScrollBar((ScrollBar)widget);

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
			if (widget is TextBox) return CalcTextBoxMinSize((TextBox)widget);
			if (widget is RadioButton) return CalcMinRadioButtonSize((RadioButton)widget);
			if (widget is VerticalScrollBar) return Scheme.VerticalScrollBar.CalcMinScrollBarSize((ScrollBar)widget);
			if (widget is HorizontalScrollBar) return Scheme.HorizontalScrollBar.CalcMinScrollBarSize((ScrollBar)widget);

			return Size.Empty;
		}

		public Size CalcMaxSize(Widget widget)
		{
			if (widget is TextBox) return CalcTextBoxMaxSize((TextBox)widget);

			return new Size(int.MaxValue, int.MinValue);
		}
		public bool HitTest(Widget widget, Point screenLocation)
		{
			if (widget is Button) return HitTestButton((Button)widget, screenLocation);
			if (widget is CheckBox) return HitTestCheckBox((CheckBox)widget, screenLocation);
			if (widget is TextBox) return HitTestTextBox((TextBox)widget, screenLocation);

			return true;
		}

		public int ThemeMargin(Widget widget)
		{
			if (widget is Button) return Scheme.Button.Margin;
			if (widget is CheckBox) return Scheme.CheckBox.Margin;
			if (widget is TextBox) return Scheme.TextBox.Margin;

			return 0;
		}

		public void MouseDownInWidget(Widget widget, Point clientLocation)
		{
			if (widget is TextBox) MouseDownInTextBox((TextBox)widget, clientLocation);
			if (widget is VerticalScrollBar) Scheme.VerticalScrollBar.MouseDownInScrollBar((ScrollBar)widget, clientLocation);
			if (widget is HorizontalScrollBar) Scheme.HorizontalScrollBar.MouseDownInScrollBar((ScrollBar)widget, clientLocation);

		}
		public void MouseMoveInWidget(Widget widget, Point clientLocation)
		{
			if (widget is TextBox) MouseMoveInTextBox((TextBox)widget, clientLocation);
			if (widget is VerticalScrollBar) Scheme.VerticalScrollBar.MouseMoveInScrollBar((ScrollBar)widget, clientLocation);
			if (widget is HorizontalScrollBar) Scheme.HorizontalScrollBar.MouseMoveInScrollBar((ScrollBar)widget, clientLocation);
		}
		public void MouseUpInWidget(Widget widget, Point clientLocation)
		{
			if (widget is TextBox) MouseUpInTextBox((TextBox)widget, clientLocation);
			if (widget is VerticalScrollBar) Scheme.VerticalScrollBar.MouseUpInScrollBar((ScrollBar)widget, clientLocation);
			if (widget is HorizontalScrollBar) Scheme.HorizontalScrollBar.MouseUpInScrollBar((ScrollBar)widget, clientLocation);
		}

		#endregion

		#region --- ListBox ---

		private void DrawListBox(ListBox listBox)
		{
			Surface image = Scheme.ListBox.Image;

			if (listBox.Enabled == false)
				image = Scheme.ListBox.Disabled;

			Point location = listBox.PointToScreen(new Point(0, 0));
			Size size = listBox.Size;

			DrawStretchImage(location, size,
				image, Scheme.TextBox.StretchRegion);
		}

		#endregion
		#region --- TextBox ---

		private void MouseDownInTextBox(TextBox textBox, Point clientLocation)
		{
			textBox.MoveInsertionPoint(
				TextBoxClientToTextLocation(textBox, clientLocation), false);
		
		}
		private void MouseMoveInTextBox(TextBox textBox, Point clientLocation)
		{
			if (textBox.MouseDownIn)
			{
				textBox.MoveInsertionPoint(
					TextBoxClientToTextLocation(textBox, clientLocation), true);
			}
		}
		private void MouseUpInTextBox(TextBox textBox, Point clientLocation)
		{

		}		

		private static TextBoxCache GetTextBoxCache(TextBox textBox)
		{
			if (textBox.Cache == null)
				textBox.Cache = new TextBoxCache();

			return (TextBoxCache)textBox.Cache;
		}
		private void UpdateCache(TextBox textBox)
		{
			TextBoxCache c = GetTextBoxCache(textBox);

			if (c.Dirty == false)
				return;

			Size fixedSize = StretchRegionFixedSize(Scheme.TextBox.Image.SurfaceSize,
				Scheme.TextBox.StretchRegion);

			Size surfSize = new Size(textBox.Size.Width - fixedSize.Width,
						 textBox.Size.Height - fixedSize.Height);

			if (c.TextBoxFrameBuffer == null || c.TextBoxFrameBuffer.Size != surfSize)
			{
				if (c.TextBoxFrameBuffer != null)
					c.TextBoxFrameBuffer.Dispose();

				c.TextBoxFrameBuffer = new FrameBuffer(surfSize);
				c.Origin = Point.Empty;
			}

			Point ip = InsertionPointLocation(textBox);
			ip.X -= Scheme.TextBox.StretchRegion.X;
			ip.Y -= Scheme.TextBox.StretchRegion.Y;
			int bottom = ip.Y + Scheme.InsertionPointHeight;

			if (ip.Y < 0)
				c.Origin.Y += ip.Y;
			if (bottom > surfSize.Height)
				c.Origin.Y += bottom - surfSize.Height;
			if (ip.X < 0)
				c.Origin.X += ip.X;
			if (ip.X >= surfSize.Width)
				c.Origin.X += ip.X - surfSize.Width + 1;

			FrameBuffer old = Display.RenderTarget;
			Display.RenderTarget = c.TextBoxFrameBuffer;
			Display.RenderState.AlphaBlend = false;
			Display.BeginFrame();

			Display.Clear(Color.FromArgb(0,0,0,0));

			if (textBox.Enabled)
				Scheme.WidgetFont.Color = Scheme.FontColor;
			else
				Scheme.WidgetFont.Color = Scheme.FontColorDisabled;

			Scheme.WidgetFont.DrawText(-c.Origin.X, -c.Origin.Y, textBox.Text);

			Display.EndFrame();
			Display.RenderTarget = old;
			Display.RenderState.AlphaBlend = true;

			c.Dirty = false;
		}

		private void DrawTextBox(TextBox textBox)
		{
			Surface image = Scheme.TextBox.Image;

			if (textBox.Enabled == false)
				image = Scheme.TextBox.Disabled;

			Point location = textBox.PointToScreen(new Point(0, 0));
			Size size = textBox.Size;
			
			DrawStretchImage(location, size,
				image, Scheme.TextBox.StretchRegion);

			if (textBox.Enabled)
			{
				if (textBox.HasFocus)
				{
					DrawStretchImage(location, size,
						Scheme.TextBox.Focus, Scheme.TextBox.StretchRegion);
				}
				if (textBox.MouseIn)
				{
					DrawStretchImage(location, size,
						Scheme.TextBox.Hover, Scheme.TextBox.StretchRegion);
				}
			}

			Scheme.WidgetFont.DisplayAlignment = OriginAlignment.TopLeft;

			SetControlFontColor(textBox);

			location.X += Scheme.TextBox.StretchRegion.X;
			location.Y += Scheme.TextBox.StretchRegion.Y;

			TextBoxCache c = (TextBoxCache)textBox.Cache;

			if (c == null || c.TextBoxSurface == null)
			{
				Scheme.WidgetFont.DrawText(
					location,
					textBox.Text);
			}
			else
			{
				c.TextBoxSurface.Draw(location);
			}

			if (textBox.HasFocus)
			{
				Point loc = InsertionPointLocation(textBox);

				loc = textBox.PointToScreen(loc);

				DrawInsertionPoint(textBox, loc, Scheme.InsertionPointHeight,
					Timing.TotalMilliseconds - textBox.IPTime);
			}
		}

		private int TextBoxClientToTextLocation(TextBox textBox, Point clientLocation)
		{
			TextBoxCache c = GetTextBoxCache(textBox);

			clientLocation.X += c.Origin.X - Scheme.TextBox.StretchRegion.X;
			clientLocation.Y += c.Origin.Y - Scheme.TextBox.StretchRegion.Y;

			Size sz = Size.Empty;
			int last = 0;
			int index;

			int line = clientLocation.Y / Scheme.WidgetFont.FontHeight;
			int linestart = 0;
			for (index = 0; index < textBox.Text.Length; index++)
			{
				if (textBox.Text[index] == '\n')
				{
					if (index < textBox.Text.Length - 1)
						index++;

					line--;
					linestart = index;
				}

				if (line == 0)
					goto searchX;
			}

			// we only get here if the mouse click was below the last line.
			index = linestart;

		searchX:
			for (; index < textBox.Text.Length; index++)
			{
				sz = Scheme.WidgetFont.MeasureString(textBox.Text.Substring(linestart, index - linestart));

				if (textBox.Text[index] == '\n')
				{
					break;
				}

				if (sz.Width > clientLocation.X)
				{
					goto found;
				}

				last = sz.Width;

			}

		found:
			double pass = (sz.Width - clientLocation.X) / (double)(sz.Width - last);

			// if it's halfway over, put the insertion on the right side of the character, 
			// otherwise put it on the left.
			if (pass <= 0.5)
				return index;
			else
				return index - 1;
		}

		/// <summary>
		/// Returns the local insertion point location in the textbox in pixels.
		/// </summary>
		/// <param name="textBox"></param>
		/// <returns></returns>
		private Point InsertionPointLocation(TextBox textBox)
		{
			int lineStart = 0;
			int lines = 0;

			if (textBox.MultiLine)
			{
				for (int i = 0; i < textBox.InsertionPoint; i++)
				{
					if (textBox.Text[i] == '\n')
					{
						lineStart = i;
						lines++;
					}
				}
			}

			Size sz = Scheme.WidgetFont.StringDisplaySize(
				textBox.Text.Substring(lineStart, textBox.InsertionPoint - lineStart));

			Point loc = new Point(
				sz.Width + Scheme.TextBox.StretchRegion.X,
				lines * Scheme.WidgetFont.FontHeight + Scheme.TextBox.StretchRegion.Y);

			TextBoxCache c = textBox.Cache as TextBoxCache;

			if (c != null)
			{
				loc.X -= c.Origin.X;
				loc.Y -= c.Origin.Y;
			}

			loc.Y++;

			return loc;
		}

		private void DrawInsertionPoint(Widget widget, Point location, int size, double time)
		{
			int val = (int)time / Scheme.InsertionPointBlinkTime;

			if (val % 2 == 1)
				return;

			Display.DrawLine(location,
				new Point(location.X, location.Y + size),
				Scheme.WidgetFont.Color);
		}

		private Size CalcTextBoxMinSize(TextBox textBox)
		{
			Size retval = new Size();

			retval.Width = 40;
			retval.Height = Scheme.WidgetFont.FontHeight;
			retval.Height += Scheme.TextBox.Image.SurfaceHeight - Scheme.TextBox.StretchRegion.Height;

			return retval;
		}
		private Size CalcTextBoxMaxSize(TextBox textBox)
		{
			Size retval = CalcTextBoxMinSize(textBox);

			retval.Width = 9000;

			if (textBox.MultiLine)
				retval.Height = 9000;

			return retval;
		}

		private bool HitTestTextBox(TextBox textBox, Point screenLocation)
		{
			Point local = textBox.PointToClient(screenLocation);

			return true;
		}

		#endregion
		#region --- CheckBox ---

		private void DrawCheckbox(CheckBox checkbox)
		{
			Surface surf;

			if (checkbox.Enabled == false) 
				surf = Scheme.CheckBox.Disabled;
			else
				surf = Scheme.CheckBox.Image;

			Point destPoint = checkbox.PointToScreen(
				Origin.Calc(OriginAlignment.CenterLeft, checkbox.Size));

			surf.DisplayAlignment = OriginAlignment.CenterLeft;
			surf.Draw(destPoint);

			if (checkbox.Enabled)
			{
				if (checkbox.HasFocus)
				{
					Scheme.CheckBox.Focus.DisplayAlignment = OriginAlignment.CenterLeft;
					Scheme.CheckBox.Focus.Draw(destPoint);
				}
				if (checkbox.MouseIn)
				{
					Scheme.CheckBox.Hover.DisplayAlignment = OriginAlignment.CenterLeft;
					Scheme.CheckBox.Hover.Draw(destPoint);
				}
				if (checkbox.Checked)
				{
					Scheme.CheckBox.Check.Color = Color.White;
					Scheme.CheckBox.Check.DisplayAlignment = OriginAlignment.CenterLeft;
					Scheme.CheckBox.Check.Draw(destPoint);
				}
			}
			else if (checkbox.Checked)
			{
				Scheme.CheckBox.Check.Color = Color.Gray;
				Scheme.CheckBox.Check.DisplayAlignment = OriginAlignment.CenterLeft;
				Scheme.CheckBox.Check.Draw(destPoint);
			}

			SetControlFontColor(checkbox);

			destPoint.X += surf.DisplayWidth + Scheme.CheckBox.Spacing;

			Scheme.WidgetFont.DisplayAlignment = OriginAlignment.CenterLeft;
			Scheme.WidgetFont.DrawText(destPoint, checkbox.Text);
		}
		private Size CalcMinCheckBoxSize(CheckBox checkbox)
		{
			Size text = Scheme.WidgetFont.StringDisplaySize(checkbox.Text);
			Size box = Scheme.CheckBox.Image.SurfaceSize;

			return new Size(
				box.Width + Scheme.CheckBox.Spacing + text.Width,
				Math.Max(box.Height, text.Height));
		}
		private bool HitTestCheckBox(CheckBox checkBox, Point screenLocation)
		{
			Point local = checkBox.PointToClient(screenLocation);

			int right = Scheme.CheckBox.Image.SurfaceWidth +
					Scheme.WidgetFont.StringDisplayWidth(checkBox.Text) + Scheme.CheckBox.Spacing * 2;

			if (local.X > right)
				return false;

			return true;
		}

		#endregion
		#region --- Radio Button ---

		private void DrawRadioButton(RadioButton radiobutton)
		{
			Surface surf;

			if (radiobutton.Enabled == false)
				surf = Scheme.RadioButton.Disabled;
			else
				surf = Scheme.RadioButton.Image;

			Point destPoint = radiobutton.PointToScreen(
				Origin.Calc(OriginAlignment.CenterLeft, radiobutton.Size));

			surf.DisplayAlignment = OriginAlignment.CenterLeft;
			surf.Draw(destPoint);

			if (radiobutton.Enabled)
			{
				if (radiobutton.HasFocus)
				{
					Scheme.RadioButton.Focus.DisplayAlignment = OriginAlignment.CenterLeft;
					Scheme.RadioButton.Focus.Draw(destPoint);
				}
				if (radiobutton.MouseIn)
				{
					Scheme.RadioButton.Hover.DisplayAlignment = OriginAlignment.CenterLeft;
					Scheme.RadioButton.Hover.Draw(destPoint);
				}
				if (radiobutton.Checked)
				{
					Scheme.RadioButton.Check.Color = Color.White;
					Scheme.RadioButton.Check.DisplayAlignment = OriginAlignment.CenterLeft;
					Scheme.RadioButton.Check.Draw(destPoint);
				}
			}
			else if (radiobutton.Checked)
			{
				Scheme.RadioButton.Check.Color = Scheme.FontColorDisabled;
				Scheme.RadioButton.Check.DisplayAlignment = OriginAlignment.CenterLeft;
				Scheme.RadioButton.Check.Draw(destPoint);
			}

			SetControlFontColor(radiobutton);

			destPoint.X += surf.DisplayWidth + Scheme.RadioButton.Spacing;

			Scheme.WidgetFont.DisplayAlignment = OriginAlignment.CenterLeft;
			Scheme.WidgetFont.DrawText(destPoint, radiobutton.Text);
		}

		private Size CalcMinRadioButtonSize(RadioButton radiobutton)
		{
			Size text = Scheme.WidgetFont.StringDisplaySize(radiobutton.Text);
			Size box = Scheme.RadioButton.Image.SurfaceSize;

			return new Size(
				box.Width + Scheme.RadioButton.Spacing + text.Width,
				Math.Max(box.Height, text.Height));
		}


		private bool HitTestRadioButton(RadioButton radioButton, Point screenLocation)
		{
			Point local = radioButton.PointToClient(screenLocation);

			int right = Scheme.RadioButton.Image.SurfaceWidth +
					Scheme.WidgetFont.StringDisplayWidth(radioButton.Text) + Scheme.RadioButton.Spacing * 2;

			if (local.X > right)
				return false;

			return true;
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

			Scheme.WidgetFont.DisplayAlignment = label.TextAlignment;
			Scheme.WidgetFont.DrawText(location, label.Text);
		}

		private Size CalcMinLabelSize(Label label)
		{
			Size retval = Scheme.WidgetFont.StringDisplaySize(label.Text);

			return retval;
		}

		#endregion
		#region --- Button ---

		private void DrawButton(Button button)
		{
			Surface image = Scheme.Button.Image;

			bool isDefault = button.IsDefaultButton;

			if (button.Enabled == false)
				image = Scheme.Button.Disabled;
			else if (button.DrawActivated)
				image = Scheme.Button.Pressed;
			else if (isDefault)
				image = Scheme.Button.Default;

			Point location = button.PointToScreen(Point.Empty);
			Size size = new Size(button.Width, button.Height);

			DrawStretchImage(location, size,
				image, Scheme.Button.StretchRegion);

			if (button.Enabled)
			{
				if (button.MouseIn)
				{
					DrawStretchImage(location, size,
						Scheme.Button.Hover, Scheme.Button.StretchRegion);
				}
				if (button.HasFocus)
				{
					DrawStretchImage(location, size,
						Scheme.Button.Focus, Scheme.Button.StretchRegion);
				}
			}

			
			// Draw button text
			SetControlFontColor(button);

			Scheme.WidgetFont.DisplayAlignment = OriginAlignment.Center;
			location = Origin.Calc(OriginAlignment.Center, button.Size);

			// drop the text down a bit if the button is being pushed.
			if (button.DrawActivated)
			{
				location.X++;
				location.Y++;
			}

			Scheme.WidgetFont.DrawText(
				button.PointToScreen(location),
				button.Text);
		}

		private Size CalcMinButtonSize(Button button)
		{
			Size textSize = Scheme.WidgetFont.StringDisplaySize(button.Text);
			Size buttonBorder = new Size(
				Scheme.Button.Image.SurfaceWidth - Scheme.Button.StretchRegion.Width,
				Scheme.Button.Image.SurfaceHeight - Scheme.Button.StretchRegion.Height);

			textSize.Width += Scheme.Button.TextPadding * 2;
			textSize.Height += Scheme.Button.TextPadding * 2;

			return new Size(
				textSize.Width + buttonBorder.Width,
				textSize.Height + buttonBorder.Height);
		}

		private bool HitTestButton(Button button, Point screenLocation)
		{
			Point local = button.PointToClient(screenLocation);

			return true;
		}

		#endregion
		#region --- Window ---

		public void DrawWindow(Window window)
		{
			DrawWindowBackground(window);
			DrawWindowTitle(window);
			DrawWindowDecorations(window);
		}

		// TODO: fix this
		public int WindowTitlebarSize
		{
			get { return Scheme.TitleFont.FontHeight + 6; }
		}

		protected virtual void DrawWindowBackground(Window window)
		{
			if (window.ShowTitleBar)
			{
				DrawStretchImage(window.Parent.PointToScreen(
					new Point(window.Location.X, window.Location.Y + this.WindowTitlebarSize)),
					window.Size, Scheme.Window.WithTitle, Scheme.Window.WithTitleStretchRegion);

			}
			else
				throw new NotImplementedException();
		}

		private void DrawDropShadow(Rectangle rect)
		{
			for (int i = 0; i <= Scheme.DropShadowSize; i++)
			{
				Color fadeColor = Color.Red;// Scheme.WindowBorderColor;
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

			DrawStretchImage(windowLocation,
				new Size(window.Width, WindowTitlebarSize), Scheme.Window.TitleBar,
				Scheme.Window.TitleBarStretchRegion);

			Point fontPosition = new Point(windowLocation.X + 8, windowLocation.Y + 3);
			if (Scheme.Window.CenterTitle)
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
			Scheme.Window.CloseButton.DisplayAlignment = OriginAlignment.TopRight;
			Scheme.Window.CloseButton.Draw(
				new Point(window.ScreenLocation.X + window.Size.Width,
					window.ScreenLocation.Y));
		}

		#endregion


		//private bool PointInMargin(Widget widget, Point localPoint, int margin)
		//{
		//    if (localPoint.X < margin) return true;
		//    if (localPoint.Y < margin) return true;
		//    if (localPoint.X >= widget.Width - margin) return true;
		//    if (localPoint.Y >= widget.Height - margin) return true;

		//    return false;
		//}

		private Size StretchRegionFixedSize(Size imageSize, Rectangle stretchRegion)
		{
			return new Size(
				imageSize.Width - stretchRegion.Width,
				imageSize.Height - stretchRegion.Height);
		}

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

		public Rectangle GetClientArea(Container widget)
		{
			if (widget is Window) return GetWindowClientArea((Window)widget);

			return new Rectangle(Point.Empty, widget.Size);
		}
		public Rectangle GetWindowClientArea(Window widget)
		{
			if (widget.ShowTitleBar)
			{
				return new Rectangle(
					Scheme.Window.WithTitleStretchRegion.Left,
					Scheme.Window.WithTitleStretchRegion.Top + WindowTitlebarSize,
					widget.Width - (Scheme.Window.WithTitle.SurfaceWidth - Scheme.Window.WithTitleStretchRegion.Width),
					widget.Height - (Scheme.Window.WithTitle.SurfaceHeight - Scheme.Window.WithTitleStretchRegion.Height));
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		private void SetControlFontColor(Widget widget)
		{
			if (widget.Enabled)
				Scheme.WidgetFont.Color = Scheme.FontColor;
			else
				Scheme.WidgetFont.Color = Scheme.FontColorDisabled;
		}

	}
}