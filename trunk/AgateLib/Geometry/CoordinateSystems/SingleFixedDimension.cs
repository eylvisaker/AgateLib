using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Geometry.CoordinateSystems
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
