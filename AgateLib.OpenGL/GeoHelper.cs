//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Mathematics;

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
