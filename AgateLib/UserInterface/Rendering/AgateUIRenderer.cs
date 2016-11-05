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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.UserInterface.Rendering.Animators;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Rendering
{
	public class AgateUIRenderer : IGuiRenderer
	{
		private IWidgetAdapter mAdapter;
		Surface mBlankSurface;
		IUserInterfaceImageProvider mImageProvider = new DefaultImageProvider();
		Gui mGui;

		Dictionary<Widget, IWidgetAnimator> mAnimators = new Dictionary<Widget, IWidgetAnimator>();

		IWidgetAnimator mRootAnimator { get { return GetOrCreateAnimator(mGui.Desktop); } }

		List<IWidgetAnimator> animsToRemove = new List<IWidgetAnimator>();

		public AgateUIRenderer(IWidgetAdapter adapter)
		{
			mAdapter = adapter;

			PixelBuffer buffer = new PixelBuffer(PixelFormat.ARGB8888, new Size(10, 10));
			buffer.Clear(Color.White);

			mBlankSurface = new Surface(buffer);
		}

		public Gui MyGui { get { return mGui; } set { mGui = value; } }
		public Gesture ActiveGesture { get; set; }

		public void Update(double deltaTime)
		{
			UpdateAnimatorTree();

			InTransition = false;

			foreach (var anim in mAnimators.Values)
			{
				if (ActiveGesture != null && ActiveGesture.TargetWidget == anim.Widget)
				{
					anim.Gesture = ActiveGesture;
				}
				else
					anim.Gesture = null;

				anim.Update(deltaTime);
				anim.Widget.OnUpdate(deltaTime);

				if (anim.InTransition)
					InTransition = true;
			}
		}

		public IWidgetAnimator GetAnimator(Widget widget)
		{
			return GetOrCreateAnimator(widget);
		}
		IWidgetAnimator GetOrCreateAnimator(Widget widget)
		{
			if (mAnimators.ContainsKey(widget) == false)
			{
				mAnimators.Add(widget, new WidgetAnimator(mAdapter.GetStyle(widget)));
			}

			return mAnimators[widget];
		}


		#region --- Animation Tree Updating ---

		public void UpdateAnimatorTree()
		{
			// update animator tree
			var rootAnimator = GetOrCreateAnimator(MyGui.Desktop);

			TreeAddMissingAnimators(rootAnimator, MyGui.Desktop);
			TreeRemoveDeadAnimators(rootAnimator, MyGui.Desktop);
		}

		private void TreeAddMissingAnimators(IWidgetAnimator anim, Container container)
		{
			foreach (var widget in container.Children)
			{
				var childAnim = GetOrCreateAnimator(widget);

				if (anim.Children.Contains(childAnim) == false)
				{
					anim.Children.Add(childAnim);
					childAnim.Parent = anim;
				}

				if (widget is Container)
				{
					TreeAddMissingAnimators(childAnim, (Container)widget);
				}
			}
		}
		private void TreeRemoveDeadAnimators(IWidgetAnimator anim, Widget widget)
		{
			lock (animsToRemove)
			{
				animsToRemove.Clear();

				foreach (var animator in anim.Children)
				{
					if (animator.IsDead)
						animsToRemove.Add(animator);
				}

				foreach (var deadAnimator in animsToRemove)
				{
					anim.Children.Remove(deadAnimator);
					mAnimators.Remove(deadAnimator.Widget);
				}

				foreach (var animator in anim.Children)
				{
					TreeRemoveDeadAnimators(animator, animator.Widget);
				}
			}
		}

		#endregion;

		public bool InTransition { get; private set; }

		public void Draw()
		{
			foreach (var anim in mRootAnimator.Children)
			{
				DrawComponent(anim);
			}
		}

		private void DrawComponent(IWidgetAnimator anim)
		{
			if (anim.Visible == false)
				return;

			DrawComponentStyle(anim);
			DrawComponentContents(anim);
		}

		private bool PushClipRect(IWidgetAnimator anim)
		{
			if (anim.Style.Overflow == Overflow.Visible)
				return false;
			if (anim.ClientRect.Width == 0 || anim.ClientRect.Height == 0)
				return true;

			Rectangle clipRect = anim.ClientToScreen(new Rectangle(0, 0, anim.ClientRect.Width, anim.ClientRect.Height), false);

			Display.PushClipRect(clipRect);
			return true;
		}

		private void DrawComponentContents(IWidgetAnimator anim)
		{
			bool clipping = false;

			if (anim.Style.Overflow != Overflow.Visible &&
				 (anim.ClientRect.Width == 0 || anim.ClientRect.Height == 0))
			{
				return;
			}

			try
			{
				clipping = PushClipRect(anim);

				foreach (var child in anim.Children)
				{
					DrawComponent(child);
				}
			}
			finally
			{
				if (clipping)
					Display.PopClipRect();
			}
		}

		private void DrawComponentStyle(IWidgetAnimator anim)
		{
			var style = anim.Style;

			mAdapter.SetFont(anim.Widget);

			if (anim.Widget is ITextAlignment)
			{
				ITextAlignment txa = (ITextAlignment)anim.Widget;

				txa.TextAlign = ConvertTextAlign(style.TextAlign);
			}

			if (anim.Visible == false)
				return;

			DrawBackground(anim);
			DrawBorder(anim);

			SetFontProperties(style);
			anim.Widget.DrawImpl(anim.ClientToScreen(new Rectangle(Point.Empty, anim.ClientRect.Size)));
		}

		private OriginAlignment ConvertTextAlign(TextAlign textAlign)
		{
			switch (textAlign)
			{
				case TextAlign.Right:
					return OriginAlignment.TopRight;

				case TextAlign.Center:
					return OriginAlignment.TopCenter;

				default:
					return OriginAlignment.TopLeft;
			}
		}

		private void SetFontProperties(IWidgetStyle style)
		{
			style.Widget.FontColor = style.FontColor;
		}

		private void DrawBackground(IWidgetAnimator anim)
		{
			Rectangle clipRect;
			var style = anim.Style;
			var control = anim.Widget;

			switch (style.BackgroundClip)
			{
				case BackgroundClip.Content_Box:
					clipRect = anim.ClientRect;
					break;

				case BackgroundClip.Padding_Box:
					clipRect = Rectangle.FromLTRB(
						anim.WidgetRect.Left + style.BoxModel.Border.Left,
						anim.WidgetRect.Top + style.BoxModel.Border.Top,
						anim.WidgetRect.Right + style.BoxModel.Border.Right,
						anim.WidgetRect.Bottom + style.BoxModel.Border.Bottom);

					break;

				case BackgroundClip.Border_Box:
				default:
					clipRect = anim.WidgetRect;
					break;
			}

			clipRect = anim.ParentCoordinateSystem.ClientToScreen(clipRect);

			if (style.BackgroundColor.A > 0)
			{
				mBlankSurface.Color = style.BackgroundColor;
				mBlankSurface.Draw(clipRect);
			}
			if (string.IsNullOrEmpty(style.BackgroundImage) == false)
			{
				Surface backgroundImage = mImageProvider.GetImage(style.BackgroundImage);
				Point origin = clipRect.Location;
				var backgroundPosition = style.BackgroundPosition;

				origin.X += backgroundPosition.X;
				origin.Y += backgroundPosition.Y;

				switch (style.BackgroundRepeat)
				{
					case BackgroundRepeat.No_Repeat:
						DrawClipped(backgroundImage, origin, clipRect);
						break;

					case BackgroundRepeat.Repeat:
						DrawRepeatedClipped(backgroundImage, origin, clipRect, true, true);
						break;

					case BackgroundRepeat.Repeat_X:
						DrawRepeatedClipped(backgroundImage, origin, clipRect, true, false);
						break;

					case BackgroundRepeat.Repeat_Y:
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

		private void DrawBorder(IWidgetAnimator anim)
		{
			Rectangle borderRect = anim.ParentCoordinateSystem.ClientToScreen(
				anim.WidgetRect);
			var style = anim.Style;

			if (string.IsNullOrEmpty(style.BorderImage))
			{
				DrawOrdinaryBorder(style, borderRect);
			}
			else
			{
				DrawImageBorder(style, borderRect);
			}
		}

		private void DrawImageBorder(IWidgetStyle style, Rectangle borderRect)
		{
			Surface image = mImageProvider.GetImage(style.BorderImage);

			Rectangle slice = style.BorderImageSlice;
			Rectangle outerRect = new Rectangle(0, 0, image.SurfaceWidth, image.SurfaceHeight);
			Rectangle innerRect = Rectangle.FromLTRB(
				slice.Left, 
				slice.Top, 
				outerRect.Width - slice.Right,
				outerRect.Height - slice.Bottom);

			DrawFrame(borderRect, image, innerRect, outerRect);
		}

		private void DrawOrdinaryBorder(IWidgetStyle style, Rectangle borderRect)
		{
			var border = style.Border;

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
