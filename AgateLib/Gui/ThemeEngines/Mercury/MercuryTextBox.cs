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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	/// <summary>
	/// Class which draws text boxes for the Mercury theme engine.
	/// </summary>
	public class MercuryTextBox : MercuryWidget
	{
		public Surface Image { get; set; }
		public Surface Disabled { get; set; }
		public Surface Hover { get; set; }
		public Surface Focus { get; set; }
		public Rectangle StretchRegion { get; set; }

		public MercuryTextBox(MercuryScheme scheme, Widget widget)
			: base(scheme, widget)
		{
			Margin = 3;

			if (scheme.TextBox != null)
			{
				Image = scheme.TextBox.Image;
				Disabled = scheme.TextBox.Disabled;
				Hover = scheme.TextBox.Hover;
				Focus = scheme.TextBox.Focus;
				StretchRegion = scheme.TextBox.StretchRegion;
			}
		}

		#region --- Cache ---

		public FrameBuffer TextBoxFrameBuffer { get; set; }
		public Surface TextBoxSurface
		{
			get
			{
				if (TextBoxFrameBuffer == null)
					return null;
				else
					return TextBoxFrameBuffer.RenderTarget;
			}
		}

		public Point Origin;

		#endregion

		public void MouseDownInTextBox(TextBox textBox, Point clientLocation)
		{
			textBox.MoveInsertionPoint(
				TextBoxClientToTextLocation(textBox, clientLocation), false);

		}
		public void MouseMoveInTextBox(TextBox textBox, Point clientLocation)
		{
			if (textBox.MouseDownIn)
			{
				textBox.MoveInsertionPoint(
					TextBoxClientToTextLocation(textBox, clientLocation), true);
			}
		}
		public void MouseUpInTextBox(TextBox textBox, Point clientLocation)
		{

		}
		public void UpdateCache(TextBox textBox)
		{
			if (Dirty == false)
				return;

			Size fixedSize = StretchRegionFixedSize(Image.SurfaceSize, StretchRegion);

			Size surfSize = new Size(textBox.Size.Width - fixedSize.Width,
						 textBox.Size.Height - fixedSize.Height);

			if (TextBoxFrameBuffer == null || TextBoxFrameBuffer.Size != surfSize)
			{
				if (TextBoxFrameBuffer != null)
					TextBoxFrameBuffer.Dispose();

				TextBoxFrameBuffer = new FrameBuffer(surfSize);
				Origin = Point.Empty;
			}

			Point ip = InsertionPointLocation(textBox);
			ip.X -= StretchRegion.X;
			ip.Y -= StretchRegion.Y;
			int bottom = ip.Y + InsertionPointHeight;

			if (ip.Y < 0)
				Origin.Y += ip.Y;
			if (bottom > surfSize.Height)
				Origin.Y += bottom - surfSize.Height;
			if (ip.X < 0)
				Origin.X += ip.X;
			if (ip.X >= surfSize.Width)
				Origin.X += ip.X - surfSize.Width + 1;

			FrameBuffer old = Display.RenderTarget;
			Display.RenderTarget = TextBoxFrameBuffer;
			Display.RenderState.AlphaBlend = false;
			Display.BeginFrame();

			Display.Clear(Color.FromArgb(0, 0, 0, 0));

			if (textBox.Enabled)
				WidgetFont.Color = FontColor;
			else
				WidgetFont.Color = FontColorDisabled;

			WidgetFont.DrawText(-Origin.X, -Origin.Y, textBox.Text);

			Display.EndFrame();
			Display.RenderTarget = old;
			Display.RenderState.AlphaBlend = true;

			Dirty = false;
		}

		public override void DrawWidget(Widget w)
		{
			DrawTextBox((TextBox)w);
		}
		public void DrawTextBox(TextBox textBox)
		{
			Surface image = Image;

			if (textBox.Enabled == false)
				image = Disabled;

			Point location = textBox.PointToScreen(new Point(0, 0));
			Size size = (Size)DisplaySize;

			DrawStretchImage(location, size,
				image, StretchRegion);

			if (textBox.Enabled)
			{
				if (textBox.HasFocus)
				{
					DrawStretchImage(location, size, Focus, StretchRegion);
				}
				if (textBox.MouseIn)
				{
					DrawStretchImage(location, size, Hover, StretchRegion);
				}
			}

			WidgetFont.DisplayAlignment = OriginAlignment.TopLeft;

			SetControlFontColor(textBox);

			location.X += StretchRegion.X;
			location.Y += StretchRegion.Y;


			if (TextBoxSurface == null)
			{
				WidgetFont.DrawText(location, textBox.Text);
			}
			else
			{
				TextBoxSurface.Draw(location);
			}

			if (textBox.HasFocus)
			{
				Point loc = InsertionPointLocation(textBox);

				loc = textBox.PointToScreen(loc);

				DrawInsertionPoint(textBox, loc, InsertionPointHeight,
					Timing.TotalMilliseconds - textBox.IPTime);
			}
		}

		private int TextBoxClientToTextLocation(TextBox textBox, Point clientLocation)
		{
			clientLocation.X += Origin.X - StretchRegion.X;
			clientLocation.Y += Origin.Y - StretchRegion.Y;

			Size sz = Size.Empty;
			int last = 0;
			int index;

			int line = clientLocation.Y / WidgetFont.FontHeight;
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
				sz = WidgetFont.MeasureString(textBox.Text.Substring(linestart, index - linestart));

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

			Size sz = WidgetFont.MeasureString(
				textBox.Text.Substring(lineStart, textBox.InsertionPoint - lineStart));

			Point loc = new Point(
				sz.Width + StretchRegion.X,
				lines * WidgetFont.FontHeight + StretchRegion.Y);

			loc.X -= Origin.X;
			loc.Y -= Origin.Y;

			loc.Y++;

			return loc;
		}

		private void DrawInsertionPoint(Widget widget, Point location, int size, double time)
		{
			int val = (int)time / InsertionPointBlinkTime;

			if (val % 2 == 1)
				return;

			Display.DrawLine(location,
				new Point(location.X, location.Y + size),
				WidgetFont.Color);
		}

		public override Size MinSize(Widget w)
		{
			Size retval = new Size();

			retval.Width = 40;
			retval.Height = WidgetFont.FontHeight;
			retval.Height += Image.SurfaceHeight - StretchRegion.Height;

			return retval;
		}
		public override Size MaxSize(Widget w)
		{
			Size retval = MinSize(w);
			TextBox t = (TextBox)w;

			retval.Width = 9000;

			if (t.MultiLine)
				retval.Height = 9000;

			return retval;
		}
	}
}
