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
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Mathematics.CoordinateSystems
{
	public class SingleFixedDimension : ICoordinateSystem
	{
		Size mRenderTargetSize;

		public SingleFixedDimension()
		{
			FixedDimension = Dimension.Vertical;
			FixedDimensionValue = 600;
		}

		public Rectangle Coordinates { get; private set; }

		public Size RenderTargetSize
		{
			get { return mRenderTargetSize; }
			set
			{
				mRenderTargetSize = value;
				DetermineCoordinateSystem();
			}
		}

		private void DetermineCoordinateSystem()
		{
			if (FixedDimensionValue < 1)
				throw new InvalidOperationException();

			switch (FixedDimension)
			{
				case Dimension.Vertical:
				case Dimension.Horizontal:
					SetRect(FixedDimension);
					break;

				case Dimension.Smaller:
					if (RenderTargetSize.AspectRatio >= 1)
						SetRect(Dimension.Vertical);
					else
						SetRect(Dimension.Horizontal);
					break;

				case Dimension.Larger:
					if (RenderTargetSize.AspectRatio >= 1)
						SetRect(Dimension.Horizontal);
					else
						SetRect(Dimension.Vertical);
					break;
			}
		}

		private void SetRect(Dimension dimension)
		{
			switch (dimension)
			{
				case Dimension.Vertical:
					Coordinates = new Rectangle(0, 0, (int)(FixedDimensionValue * RenderTargetSize.AspectRatio), FixedDimensionValue);
					break;

				case Dimension.Horizontal:
					Coordinates = new Rectangle(0, 0, FixedDimensionValue, (int)(FixedDimensionValue / RenderTargetSize.AspectRatio));
					break;
			}
		}

		/// <summary>
		/// The value of the fixed dimension.
		/// </summary>
		public int FixedDimensionValue { get; set; }

		/// <summary>
		/// Whether to keep the vertical or horizontal dimension fixed. Defaults to vertical
		/// </summary>
		public Dimension FixedDimension { get; set; }
	}

	/// <summary>
	/// Indicates a dimension which will be fixed.
	/// </summary>
	public enum Dimension
	{
		Vertical,
		Horizontal,
		Smaller,
		Larger,
	}
}
