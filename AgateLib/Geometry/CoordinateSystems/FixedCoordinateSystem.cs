using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Geometry.CoordinateSystems
{
	public class FixedCoordinateSystem : ICoordinateSystem
	{
		public FixedCoordinateSystem(Rectangle coords)
		{
			Coordinates = coords;
		}

		Size mRenderTargetSize;

		public Size RenderTargetSize
		{
			get { return mRenderTargetSize; }
			set
			{
				mRenderTargetSize = value;
			}
		}
		public Rectangle Coordinates { get; set; }
	}
}
