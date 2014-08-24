using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsPhone
{
	public static class Interop
	{
		public static AgateLib.Geometry.Size ToAgateSize(this System.Windows.Size size)
		{
			return new Geometry.Size(
				(int)size.Width,
				(int)size.Height);
		}

		public static AgateLib.Geometry.Point ToAgatePoint(this System.Windows.Point point)
		{
			return new AgateLib.Geometry.Point((int)point.X, (int)point.Y);
		}

		public static SharpDX.Color4 ToColor4(this AgateLib.Geometry.Color color)
		{
			var retval = new Color4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);

			return retval;
		}
	}
}
