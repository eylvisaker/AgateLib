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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Rendering
{
	public class CssRenderer : IGuiRenderer
	{
		private CssAdapter mAdapter;
		Surface mBlankSurface;
		ICssImageProvider mImageProvider = new CssDefaultImageProvider();
		private CssAdapter adapter;


		public CssRenderer(CssAdapter adapter)
		{
			mAdapter = adapter;

			PixelBuffer buffer = new PixelBuffer(PixelFormat.ARGB8888, new Size(10, 10));
			buffer.Clear(Color.White);

			mBlankSurface = new Surface(buffer);
		}

		public Gesture ActiveGesture { get; set; }

		public void Update(Gui gui, double deltaTime)
		{
			foreach (var widget in gui.Desktop.Descendants)
			{
				var style = mAdapter.GetStyle(widget);

				if (ActiveGesture != null)
					style.Animator.Gesture = ActiveGesture.TargetWidget == widget ? ActiveGesture : null;

				style.Animator.Update(deltaTime);

				widget.OnUpdate(deltaTime);
			}
		}

		public void Draw(Gui gui)
		{
			foreach (var window in gui.Desktop.Windows)
			{
				DrawComponent(window);
			}
		}

		private void DrawComponent(Container window)
		{
			var style = mAdapter.GetStyle(window);
			if (style.Animator.Visible == false)
				return;

			DrawComponentStyle(window);
			DrawComponentContents(window);
		}

		private bool PushClipRect(CssStyle style)
		{
			Rectangle clipRect = style.Widget.ClientToScreen(new Rectangle(0, 0, style.Widget.ClientRect.Width, style.Widget.ClientRect.Height));

			if (style.Data.Overflow == CssOverflow.Visible)
				return false;

			Display.PushClipRect(clipRect);
			return true;
		}

		private void DrawComponentContents(Container window)
		{
			var style = mAdapter.GetStyle(window);
			bool clipping = false;

			try
			{
				clipping = PushClipRect(style);

				foreach (var control in window.Children)
				{
					if (control is Container)
						DrawComponent((Container)control);
					else
						DrawComponentStyle(control);
				}
			}
			finally
			{
				if (clipping)
					Display.PopClipRect();
			}
		}

		private void DrawComponentStyle(Widget control)
		{
			CssStyle style = mAdapter.GetStyle(control);

			mAdapter.SetFont(control);

			if (control is ITextAlignment)
			{
				ITextAlignment txa = (ITextAlignment)control;

				txa.TextAlign = ConvertTextAlign(style.Data.Text.Align);
			}

			if (style.Animator.Visible == false)
				return;

			DrawBackground(style);
			DrawBorder(style);

			SetFontProperties(style);
			control.DrawImpl();
		}

		private OriginAlignment ConvertTextAlign(CssTextAlign cssTextAlign)
		{
			switch (cssTextAlign)
			{
				case CssTextAlign.Right:
					return OriginAlignment.TopRight;

				case CssTextAlign.Center:
					return OriginAlignment.TopCenter;

				default:
					return OriginAlignment.TopLeft;
			}
		}

		private void SetFontProperties(CssStyle style)
		{
			style.Widget.FontColor = style.Data.Font.Color;
		}

		private void DrawBackground(CssStyle style)
		{
			Rectangle clipRect;
			var control = style.Widget;

			switch (style.Data.Background.Clip)
			{
				case CssBackgroundClip.Content_Box:
					clipRect = control.ClientRect;
					break;

				case CssBackgroundClip.Padding_Box:
					clipRect = Rectangle.FromLTRB(
						control.WidgetRect.Left + style.BoxModel.Border.Left,
						control.WidgetRect.Top + style.BoxModel.Border.Top,
						control.WidgetRect.Right + style.BoxModel.Border.Right,
						control.WidgetRect.Bottom + style.BoxModel.Border.Bottom);

					break;

				case CssBackgroundClip.Border_Box:
				default:
					clipRect = control.WidgetRect;
					break;
			}

			clipRect = control.Parent.ClientToScreen(clipRect);

			if (style.Data.Background.Color.A > 0)
			{
				mBlankSurface.Color = style.Data.Background.Color;
				mBlankSurface.Draw(clipRect);
			}
			if (string.IsNullOrEmpty(style.Data.Background.Image) == false)
			{
				Surface backgroundImage = mImageProvider.GetImage(style.Data.Background.Image);
				Point origin = clipRect.Location;

				origin.X += (int)style.Data.Background.Position.Left.Amount;
				origin.Y += (int)style.Data.Background.Position.Top.Amount;

				switch (style.Data.Background.Repeat)
				{
					case CssBackgroundRepeat.No_Repeat:
						DrawClipped(backgroundImage, origin, clipRect);
						break;

					case CssBackgroundRepeat.Repeat:
						DrawRepeatedClipped(backgroundImage, origin, clipRect, true, true);
						break;

					case CssBackgroundRepeat.Repeat_X:
						DrawRepeatedClipped(backgroundImage, origin, clipRect, true, false);
						break;

					case CssBackgroundRepeat.Repeat_Y:
						DrawRepeatedClipped(backgroundImage, origin, clipRect, false, true);
						break;
				}
			}

		}

		private void DrawRepeatedClipped(Surface image, Point startPt, Rectangle clipRect, bool repeatX, bool repeatY)
		{
			Rectangle srcRect = new Rectangle(0, 0, image.SurfaceWidth, image.SurfaceHeight);

			DrawRepeatedClipped(image, srcRect, startPt, clipRect, repeatX, repeatY);
		}
		private void DrawRepeatedClipped(Surface image, Rectangle srcRect, Point startPt, Rectangle clipRect, bool repeatX, bool repeatY)
		{
			int countX = (int)Math.Ceiling(clipRect.Width / (double)srcRect.Width);
			int countY = (int)Math.Ceiling(clipRect.Height / (double)srcRect.Height);

			if (repeatX && startPt.X != clipRect.X) startPt.X -= image.SurfaceWidth;
			if (repeatY && startPt.Y != clipRect.Y) startPt.Y -= image.SurfaceHeight;

			if (startPt.X + countX * image.SurfaceWidth < clipRect.Right) countX++;
			if (startPt.Y + countY * image.SurfaceHeight < clipRect.Bottom) countY++;

			if (repeatX == false) countX = 1;
			if (repeatY == false) countY = 1;

			for (int j = 0; j < countY; j++)
			{
				Point destPt = new Point(startPt.X, startPt.Y + j * srcRect.Height);

				for (int i = 0; i < countX; i++)
				{
					DrawClipped(image, srcRect, destPt, clipRect);

					destPt.X += srcRect.Width;
				}
			}
		}

		private void DrawClipped(Surface image, Point dest, Rectangle clipRect)
		{
			Rectangle srcRect = new Rectangle(0, 0, image.SurfaceWidth, image.SurfaceHeight);
			DrawClipped(image, srcRect, dest, clipRect);
		}

		private static void DrawClipped(Surface image, Rectangle srcRect, Point dest, Rectangle clipRect)
		{
			Rectangle destRect = new Rectangle(dest.X, dest.Y, srcRect.Width, srcRect.Height);

			if (clipRect.Contains(destRect) == false)
			{
				int lc = 0, tc = 0, rc = 0, bc = 0;

				if (destRect.Left < clipRect.Left) lc = clipRect.Left - destRect.Left;
				if (destRect.Top < clipRect.Top) tc = clipRect.Top - destRect.Top;
				if (destRect.Right > clipRect.Right) rc = clipRect.Right - destRect.Right;
				if (destRect.Bottom > clipRect.Bottom) bc = clipRect.Bottom - destRect.Bottom;

				destRect = Rectangle.FromLTRB(destRect.Left + lc, destRect.Top + tc, destRect.Right + rc, destRect.Bottom + bc);
				srcRect = Rectangle.FromLTRB(srcRect.Left + lc, srcRect.Top + tc, srcRect.Right + rc, srcRect.Bottom + bc);

				if (destRect.Width == 0 || destRect.Height == 0)
					return;
			}

			image.Draw(srcRect, destRect);
		}


		private void TileSurface(Surface frameSurface, Rectangle src, Rectangle dest)
		{
			DrawRepeatedClipped(frameSurface, src, dest.Location, dest, true, true);
		}
		void DrawFrame(Rectangle destOuterRect, Surface frameSurface,
									   Rectangle frameSourceInner, Rectangle frameSourceOuter)
		{
			Rectangle destInnerRect = destOuterRect;
			Size delta = new Size(frameSourceInner.X - frameSourceOuter.X, frameSourceInner.Y - frameSourceOuter.Y);

			destInnerRect.X += delta.Width;
			destInnerRect.Y += delta.Height;
			destInnerRect.Width -= (delta.Width) * 2;
			destInnerRect.Height -= (delta.Height) * 2;

			Rectangle src, dest;
			Rectangle outer = frameSourceOuter, inner = frameSourceInner;

			// top left
			src = Rectangle.FromLTRB(outer.Left, outer.Top, inner.Left, inner.Top);
			dest = Rectangle.FromLTRB(destOuterRect.Left, destOuterRect.Top, destInnerRect.Left, destInnerRect.Top);

			frameSurface.Draw(src, dest);

			// top
			src = Rectangle.FromLTRB(inner.Left, outer.Top, inner.Right, inner.Top);
			dest = Rectangle.FromLTRB(destInnerRect.Left, destOuterRect.Top, destInnerRect.Right, destInnerRect.Top);

			TileSurface(frameSurface, src, dest);

			// top right
			src = Rectangle.FromLTRB(inner.Right, outer.Top, outer.Right, inner.Top);
			dest = Rectangle.FromLTRB(destInnerRect.Right, destOuterRect.Top, destOuterRect.Right, destInnerRect.Top);

			frameSurface.Draw(src, dest);

			// left
			src = Rectangle.FromLTRB(outer.Left, inner.Top, inner.Left, inner.Bottom);
			dest = Rectangle.FromLTRB(destOuterRect.Left, destInnerRect.Top, destInnerRect.Left, destInnerRect.Bottom);

			TileSurface(frameSurface, src, dest);

			// right
			src = Rectangle.FromLTRB(inner.Right, inner.Top, outer.Right, inner.Bottom);
			dest = Rectangle.FromLTRB(destInnerRect.Right, destInnerRect.Top, destOuterRect.Right, destInnerRect.Bottom);

			TileSurface(frameSurface, src, dest);

			// bottom left
			src = Rectangle.FromLTRB(outer.Left, inner.Bottom, inner.Left, outer.Bottom);
			dest = Rectangle.FromLTRB(destOuterRect.Left, destInnerRect.Bottom, destInnerRect.Left, destOuterRect.Bottom);

			frameSurface.Draw(src, dest);

			// bottom
			src = Rectangle.FromLTRB(inner.Left, inner.Bottom, inner.Right, outer.Bottom);
			dest = Rectangle.FromLTRB(destInnerRect.Left, destInnerRect.Bottom, destInnerRect.Right, destOuterRect.Bottom);

			TileSurface(frameSurface, src, dest);

			// bottom right
			src = Rectangle.FromLTRB(inner.Right, inner.Bottom, outer.Right, outer.Bottom);
			dest = Rectangle.FromLTRB(destInnerRect.Right, destInnerRect.Bottom, destOuterRect.Right, destOuterRect.Bottom);

			frameSurface.Draw(src, dest);
		}

		private void DrawBorder(CssStyle style)
		{
			Rectangle borderRect = style.Widget.Parent.ClientToScreen(style.Widget.WidgetRect);

			var border = style.Data.Border;

			if (string.IsNullOrEmpty(border.Image.Source))
			{
				DrawOrdinaryBorder(style, borderRect);
			}
			else
			{
				DrawImageBorder(style, borderRect);
			}
		}

		private void DrawImageBorder(CssStyle style, Rectangle borderRect)
		{
			Surface image = mImageProvider.GetImage(style.Data.Border.Image.Source);

			Rectangle outerRect = new Rectangle(0, 0, image.SurfaceWidth, image.SurfaceHeight);
			Rectangle innerRect = Rectangle.FromLTRB(
				(int)style.Data.Border.Image.Slice.Left.Amount,
				(int)style.Data.Border.Image.Slice.Top.Amount,
				(int)(outerRect.Width - style.Data.Border.Image.Slice.Right.Amount),
				(int)(outerRect.Height - style.Data.Border.Image.Slice.Bottom.Amount));

			DrawFrame(borderRect, image, innerRect, outerRect);
		}

		private void DrawOrdinaryBorder(CssStyle style, Rectangle borderRect)
		{
			var border = style.Data.Border;

			if (border.Top.Color.A == 0 &&
				border.Left.Color.A == 0 &&
				border.Right.Color.A == 0 &&
				border.Bottom.Color.A == 0)
			{
				return;
			}

			// draw top
			Rectangle rect = new Rectangle(borderRect.X, borderRect.Y, borderRect.Width, (int)border.Top.Width.Amount);

			mBlankSurface.Color = border.Top.Color;
			mBlankSurface.Draw(rect);

			// draw bottom
			rect = new Rectangle(borderRect.X, borderRect.Bottom - (int)border.Bottom.Width.Amount, borderRect.Width, (int)border.Bottom.Width.Amount);

			mBlankSurface.Color = border.Bottom.Color;
			mBlankSurface.Draw(rect);


			// draw left
			rect = new Rectangle(borderRect.X, borderRect.Y, (int)border.Left.Width.Amount, borderRect.Height);

			mBlankSurface.Color = border.Left.Color;
			mBlankSurface.Draw(rect);

			// draw right
			rect = new Rectangle(borderRect.Right - (int)border.Right.Width.Amount, borderRect.Y, (int)border.Right.Width.Amount, borderRect.Height);

			mBlankSurface.Color = border.Right.Color;
			mBlankSurface.Draw(rect);
		}

	}
}
