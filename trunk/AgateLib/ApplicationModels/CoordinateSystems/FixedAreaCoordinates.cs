using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.ApplicationModels.CoordinateSystems
{
	/// <summary>
	/// Constructs a coordinate system that gives a render area constrained to a 
	/// specified range of sizes. This optionally (by default) preserves the aspect
	/// ratio of the display, providing extra space outside the requested render area
	/// which the application must fill in somehow.
	/// </summary>
	public class FixedAreaCoordinates : ICoordinateSystemCreator
	{
		public FixedAreaCoordinates()
		{
			PreserveDisplayAspectRatio = true;
		}

		public Rectangle DetermineCoordinateSystem(Size displayWindowSize, double aspectRatio)
		{
			var retval = GetUnshiftedRectangle(displayWindowSize, aspectRatio);

			retval.X += Origin.X;
			retval.Y += Origin.Y;

			return retval;
		}

		private Rectangle GetUnshiftedRectangle(Size displayWindowSize, double aspectRatio)
		{
			Size desiredArea = displayWindowSize;

			desiredArea = AdjustToRange(aspectRatio, desiredArea);

			if (PreserveDisplayAspectRatio)
			{
				double desiredAspectRatio = desiredArea.Width / (double)desiredArea.Height;

				if (desiredAspectRatio < aspectRatio)
				{
					int width = (int)(desiredArea.Height * aspectRatio);
					int extraWidth = width - desiredArea.Width;

					return Rectangle.FromLTRB(
						-extraWidth / 2,
						0,
						desiredArea.Width + extraWidth / 2,
						desiredArea.Height);
				}
				else if (desiredAspectRatio > aspectRatio)
				{
					int height = (int)(desiredArea.Width / aspectRatio);
					int extraHeight = height - desiredArea.Height;

					return Rectangle.FromLTRB(
						0,
						-extraHeight / 2,
						desiredArea.Width,
						desiredArea.Height + extraHeight / 2);
				}
			}

			return new Rectangle(0, 0, desiredArea.Width, desiredArea.Height);
		}

		private Size AdjustToRange(double aspectRatio, Size area)
		{
			if (MinWidth != null && (int)MinWidth > area.Height)
				area.Width = MinWidth.Value;
			if (MaxWidth != null && (int)MaxWidth < area.Height)
				area.Width = MaxWidth.Value;
			if (MinHeight != null && (int)MinHeight > area.Height)
				area.Height = MinHeight.Value;
			if (MaxHeight != null && (int)MaxHeight < area.Height)
				area.Height = MaxHeight.Value;

			return area;
		}

		public bool PreserveDisplayAspectRatio { get; set; }
		public int? MinHeight { get; set; }
		public int? MaxHeight { get; set; }
		public int? MinWidth { get; set; }
		public int? MaxWidth { get; set; }

		/// <summary>
		/// The value of the coordinate system in the upper left corner of 
		/// the display area.
		/// </summary>
		public Point Origin { get; set; }
	}
}
