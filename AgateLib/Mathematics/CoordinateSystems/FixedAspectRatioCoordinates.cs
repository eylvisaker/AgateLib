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
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Mathematics.CoordinateSystems
{
	/// <summary>
	/// Constructs a coordinate system that gives a render area constrained to a 
	/// specified range of sizes. This optionally (by default) preserves the aspect
	/// ratio of the display, providing extra space outside the requested render area
	/// which the application must fill in somehow.
	/// </summary>
	public class FixedAspectRatioCoordinates : ICoordinateSystem
	{
		Point mOrigin;
		Size mRenderTargetSize;

		public FixedAspectRatioCoordinates()
		{
			PreserveDisplayAspectRatio = true;
			AspectRatio = 16 / (double)9;
		}

		public Size RenderTargetSize
		{
			get { return mRenderTargetSize; }
			set
			{
				mRenderTargetSize = value;
				DetermineCoordinateSystem();
			}
		}
		/// <summary>
		/// The value of the coordinate system in the upper left corner of 
		/// the display area.
		/// </summary>
		public Point Origin
		{
			get { return mOrigin; }
			set
			{
				mOrigin = value;
				DetermineCoordinateSystem();
			}
		}

		public Rectangle Coordinates { get; private set; }

		public void DetermineCoordinateSystem()
		{
			var result = GetUnshiftedRectangle(RenderTargetSize);

			result.X += Origin.X;
			result.Y += Origin.Y;

			Coordinates = result;
		}

		private Rectangle GetUnshiftedRectangle(Size displayWindowSize)
		{
			var desiredArea = AdjustToRange(displayWindowSize);

			if (PreserveDisplayAspectRatio)
			{
				double desiredAspectRatio = desiredArea.AspectRatio;

				if (AspectRatio < displayWindowSize.AspectRatio)
				{
					int logicalWindowWidth = (int)(desiredArea.Height / (double)displayWindowSize.Height * displayWindowSize.Width);
					int extraWidth = logicalWindowWidth - desiredArea.Width;

					return Rectangle.FromLTRB(
						-extraWidth / 2,
						0,
						desiredArea.Width + extraWidth / 2,
						desiredArea.Height);
				}
				else if (AspectRatio > displayWindowSize.AspectRatio)
				{
					int logicalWindowHeight = (int)(desiredArea.Width / (double)displayWindowSize.Width * displayWindowSize.Height);
					int extraHeight = logicalWindowHeight - desiredArea.Height;

					return Rectangle.FromLTRB(
						0,
						-extraHeight / 2,
						desiredArea.Width,
						desiredArea.Height + extraHeight / 2);
				}
			}

			return new Rectangle(0, 0, desiredArea.Width, desiredArea.Height);
		}

		private Size AdjustToRange(Size area)
		{
			area.Width = (int)(AspectRatio * area.Height);

			if (MinHeight != null && (int)MinHeight > area.Height)
			{
				area.Height = MinHeight.Value;
				area.Width = (int)(area.Height * AspectRatio);
			}
			if (MaxHeight != null && (int)MaxHeight < area.Height)
			{
				area.Height = MaxHeight.Value;
				area.Width = (int)(area.Height * AspectRatio);
			}

			if (MinWidth != null && (int)MinWidth > area.Width)
			{
				area.Width = MinWidth.Value;
				area.Height = (int)(area.Width / AspectRatio);
			}
			if (MaxWidth != null && (int)MaxWidth < area.Width)
			{
				area.Width = MaxWidth.Value;
				area.Height = (int)(area.Width / AspectRatio);
			}

			return area;
		}

		public bool PreserveDisplayAspectRatio { get; set; }
		public int? MinHeight { get; set; }
		public int? MaxHeight { get; set; }
		public int? MinWidth { get; set; }
		public int? MaxWidth { get; set; }

		public double AspectRatio { get; set; }

	}
}
