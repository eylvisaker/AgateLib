using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateOTK
{
	static class GeoHelper
	{
		public static OpenTK.Math.Matrix4 ConvertAgateMatrix(Matrix4x4 matrix, bool invertY)
		{
			int sign = invertY ? -1 : 1;

			return new OpenTK.Math.Matrix4(
				new OpenTK.Math.Vector4(matrix[0, 0], sign * matrix[1, 0], matrix[2, 0], matrix[3, 0]),
				new OpenTK.Math.Vector4(matrix[0, 1], sign * matrix[1, 1], matrix[2, 1], matrix[3, 1]),
				new OpenTK.Math.Vector4(matrix[0, 2], sign * matrix[1, 2], matrix[2, 2], matrix[3, 2]),
				new OpenTK.Math.Vector4(matrix[0, 3], sign * matrix[1, 3], matrix[2, 3], matrix[3, 3]));
		}

	}
}
