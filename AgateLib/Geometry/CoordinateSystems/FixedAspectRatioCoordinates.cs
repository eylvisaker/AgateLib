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
using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Geometry.CoordinateSystems
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
			var retval = GetUnshiftedRectangle(RenderTargetSize);

			retval.X += Origin.X;
			retval.Y += Origin.Y;

			Coordinates = retval;
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
