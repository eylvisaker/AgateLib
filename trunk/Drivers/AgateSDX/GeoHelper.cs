using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using SlimDX;

namespace AgateSDX
{
	static class GeoHelper
	{
		public static Matrix TransformAgateMatrix(Matrix4x4 value)
		{
			Matrix retval = new Matrix();

			retval.M11 = value[0, 0];
			retval.M21 = value[1, 0];
			retval.M31 = value[2, 0];
			retval.M41 = value[3, 0];

			retval.M12 = value[0, 1];
			retval.M22 = value[1, 1];
			retval.M32 = value[2, 1];
			retval.M42 = value[3, 1];

			retval.M13 = value[0, 2];
			retval.M23 = value[1, 2];
			retval.M33 = value[2, 2];
			retval.M43 = value[3, 2];

			retval.M14 = value[0, 3];
			retval.M24 = value[1, 3];
			retval.M34 = value[2, 3];
			retval.M44 = value[3, 3];

			return retval;
		}
	}
}
