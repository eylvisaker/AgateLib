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
	public class FixedAspectRatioCoordinates : ICoordinateSystemCreator
	{
		public FixedAspectRatioCoordinates()
		{
			PreserveDisplayAspectRatio = true;
			AspectRatio = 16 / (double)9;
		}

		public Rectangle DetermineCoordinateSystem(Size displayWindowSize)
		{
			var retval = GetUnshiftedRectangle(displayWindowSize);

			retval.X += Origin.X;
			retval.Y += Origin.Y;

			return retval;
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

		/// <summary>
		/// The value of the coordinate system in the upper left corner of 
		/// the display area.
		/// </summary>
		public Point Origin { get; set; }
	}
}
