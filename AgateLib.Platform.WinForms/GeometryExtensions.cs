using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WinForms
{
	public static class GeometryExtensions
	{
		/// <summary>
		/// Converts an AgateLib point object to a System.Drawing point object.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public static System.Drawing.Point ToDrawingPoint(this AgateLib.Geometry.Point point)
		{
			return new System.Drawing.Point(point.X, point.Y);
		}

		/// <summary>
		/// Converts an AgateLib size object to a System.Drawing size object.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static System.Drawing.Size ToDrawingSize(this AgateLib.Geometry.Size size)
		{
			return new System.Drawing.Size(size.Width, size.Height);
		}

		/// <summary>
		/// Converts an System.Drawing point object to a AgateLib point object.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public static AgateLib.Geometry.Point ToAgatePoint(this System.Drawing.Point point)
		{
			return new AgateLib.Geometry.Point(point.X, point.Y);
		}

	}
}
