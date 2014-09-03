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

	public enum Dimension
	{
		Vertical,
		Horizontal,
	}
}
