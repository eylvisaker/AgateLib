using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsStore
{
	public static class Interop
	{
		public static SharpDX.Color4 ToColor4(this AgateLib.Geometry.Color color)
		{
			var retval = new Color4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);

			return retval;
		}

		public static Geometry.Size ToAgateSize(this Windows.Foundation.Size size)
		{
			return new Geometry.Size((int)size.Width, (int)size.Height);
		}
		public static Geometry.Point ToAgatePoint(this Windows.Foundation.Point pt)
		{
			return new Geometry.Point((int)pt.X, (int)pt.Y);
		}
	}
}
