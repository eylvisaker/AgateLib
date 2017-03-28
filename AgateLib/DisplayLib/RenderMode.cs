//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Static class which contains built in IRenderMode objects.
	/// Render modes are used to determine how a backbuffer is scaled to fit
	/// an on screen render area.
	/// </summary>
	public static class RenderMode
	{
		class RetainAspectRatioRenderMode : IRenderMode
		{
			public override string ToString() => "Retain Aspect Ratio";

			public Rectangle DestRect(Size backBufferSize, Size renderTargetSize)
			{
				var targetScale = TargetScale(backBufferSize, renderTargetSize);

				Size targetSize = new Size(
					(int)(backBufferSize.Width * targetScale),
					(int)(backBufferSize.Height * targetScale));
				Point targetPoint = new Point(
					(renderTargetSize.Width - targetSize.Width) / 2,
					(renderTargetSize.Height - targetSize.Height) / 2);

				var result = new Rectangle(targetPoint, targetSize);

				return result;
			}

			public Point MousePoint(Point windowPoint, Size backBufferSize, Size renderTargetSize)
			{
				var targetScale = TargetScale(backBufferSize, renderTargetSize);

				var destRect = DestRect(backBufferSize, renderTargetSize);
				var distance = new Point(
					windowPoint.X - destRect.X,
					windowPoint.Y - destRect.Y);

				return new Point(
					(int)(distance.X / targetScale),
					(int)(distance.Y / targetScale));
			}

			private double TargetScale(Size backBufferSize, Size renderTargetSize)
			{
				double scaleWidth = renderTargetSize.Width / (double)backBufferSize.Width;
				double scaleHeight = renderTargetSize.Height / (double)backBufferSize.Height;

				double targetScale = Math.Min(scaleWidth, scaleHeight);
				return targetScale;
			}

		}

		class StretchRenderMode : IRenderMode
		{
			public override string ToString() => "Stretch";

			public Rectangle DestRect(Size backBufferSize, Size renderTargetSize)
			{
				return new Rectangle(Point.Zero, renderTargetSize);
			}

			public Point MousePoint(Point windowPoint, Size backBufferSize, Size renderTargetSize)
			{
				var scaleX = renderTargetSize.Width / (double)backBufferSize.Width;
				var scaleY = renderTargetSize.Height / (double)backBufferSize.Height;

				return new Point(
					(int)(windowPoint.X / scaleX),
					(int)(windowPoint.Y / scaleY));
			}
		}

		/// <summary>
		/// Returns a new IRenderMode object which preserves the aspect ratio of image.
		/// </summary>
		public static IRenderMode RetainAspectRatio => new RetainAspectRatioRenderMode();

		/// <summary>
		/// Returns a new IRenderMode object which will stretch an image to fit.
		/// </summary>
		public static IRenderMode Stretch => new StretchRenderMode();
	}

	/// <summary>
	/// Interface for an object which determines how to scale a backbuffer to the on
	/// screen display area.
	/// </summary>
	public interface IRenderMode
	{
		/// <summary>
		/// Computes the destination rectangle given the backbuffer and render target sizes.
		/// </summary>
		/// <param name="backBufferSize"></param>
		/// <param name="renderTargetSize"></param>
		/// <returns></returns>
		Rectangle DestRect(Size backBufferSize, Size renderTargetSize);

		/// <summary>
		/// Takes a point for the mouse in the window and transforms it
		/// to be a point in the back buffer.
		/// </summary>
		/// <param name="windowPoint"></param>
		/// <param name="backBufferSize"></param>
		/// <param name="renderTargetSize"></param>
		/// <returns></returns>
		Point MousePoint(Point windowPoint, Size backBufferSize, Size renderTargetSize);
	}
}
