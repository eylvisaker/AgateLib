using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.UserInterface.Widgets
{
	public class ProgressBar : Widget
	{
		Surface mSurf;
		Rectangle innerArea = new Rectangle(3, 3, 107, 2);

		public int Value { get; set; }
		public int Max { get; set; }
		public Gradient Gradient { get; set; }

		public ProgressBar()
		{
			mSurf = null;//= InterfaceRenderer.GetImage("meter");

			Gradient = new AgateLib.Geometry.Gradient(Color.White);
			Height = mSurf.DisplayHeight;
		}
		internal override Size ComputeSize(int? minWidth, int? minHeight, int? maxWidth, int? maxHeight)
		{
			Size retval = new Size();

			retval.Height = mSurf.DisplayHeight;
			retval.Width = 40;

			return retval;
		}

		public override void DrawImpl()
		{
			Rectangle destRect = ClientToScreen(
				new Rectangle(X, Y, Width, Height));

			DrawSurface(destRect);

			if (Max > 0)
			{
				double percentage = Value / (double)Max;

				int maxBarWidth = Width - (mSurf.SurfaceWidth - innerArea.Width);
				int width = (int)(percentage * maxBarWidth);

				destRect.X += innerArea.X;
				destRect.Y += innerArea.Y;

				destRect.Width = width;
				destRect.Height = innerArea.Height;

				var grad = new Gradient(Gradient.TopLeft);
				grad.TopRight = Gradient.Interpolate(width, 0);
				grad.BottomRight = grad.TopRight;

				Display.FillRect(destRect, grad);
			}
		}

		private void DrawSurface(Rectangle destRect)
		{
			Rectangle srcRect = new Rectangle(0, 0, mSurf.SurfaceWidth, mSurf.SurfaceHeight);
			Rectangle innerSrcRect = innerArea;
			Rectangle innerDestRect = destRect;
			Surface surf = mSurf;

			innerDestRect.X += innerArea.X;
			innerDestRect.Y += innerArea.Y;
			innerDestRect.Width -= (srcRect.Width - innerSrcRect.Width);
			innerDestRect.Height -= (srcRect.Height - innerSrcRect.Height);

			Rectangle src, dest;

			// left
			src = Rectangle.FromLTRB(srcRect.Left, srcRect.Top, innerSrcRect.Left, srcRect.Bottom);
			dest = Rectangle.FromLTRB(destRect.Left, destRect.Top, innerDestRect.Left, destRect.Bottom);

			surf.Draw(src, dest);

			// middle
			src = Rectangle.FromLTRB(innerSrcRect.Left, srcRect.Top, innerSrcRect.Right, srcRect.Bottom);
			dest = Rectangle.FromLTRB(innerDestRect.Left, destRect.Top, innerDestRect.Right, destRect.Bottom);

			surf.Draw(src, dest);

			// right
			src = Rectangle.FromLTRB(innerSrcRect.Right, srcRect.Top, srcRect.Right, srcRect.Bottom);
			dest = Rectangle.FromLTRB(innerDestRect.Right, destRect.Top, destRect.Right, destRect.Bottom);

			surf.Draw(src, dest);
		}
	}
}
