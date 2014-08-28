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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.OpenGL
{
	static class GeoHelper
	{
		public static OpenTK.Matrix4 ConvertAgateMatrix(Matrix4x4 matrix, bool invertY)
		{
			int sign = invertY ? -1 : 1;

			return new OpenTK.Matrix4(
				new OpenTK.Vector4(matrix[0, 0], sign * matrix[1, 0], matrix[2, 0], matrix[3, 0]),
				new OpenTK.Vector4(matrix[0, 1], sign * matrix[1, 1], matrix[2, 1], matrix[3, 1]),
				new OpenTK.Vector4(matrix[0, 2], sign * matrix[1, 2], matrix[2, 2], matrix[3, 2]),
				new OpenTK.Vector4(matrix[0, 3], sign * matrix[1, 3], matrix[2, 3], matrix[3, 3]));
		}

	}
}
