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
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Mathematics
{
	/// <summary>
	/// Structure which indicates a 4x4 matrix.
	/// </summary>
	public struct Matrix4x4
	{
		float m11, m12, m13, m14;
		float m21, m22, m23, m24;
		float m31, m32, m33, m34;
		float m41, m42, m43, m44;

		/// <summary>
		/// The identity 4x4 matrix.
		/// </summary>
		public static readonly Matrix4x4 Identity = new Matrix4x4(
			1, 0, 0, 0,
			0, 1, 0, 0,
			0, 0, 1, 0,
			0, 0, 0, 1);
		/// <summary>
		/// Creates a 4x4 matrix which represents a translation.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		public static Matrix4x4 Translation(double x, double y, double z)
		{
			return Translation((float)x, (float)y, (float)z);
		}
		/// <summary>
		/// Creates a 4x4 matrix which represents a translation.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		public static Matrix4x4 Translation(float x, float y, float z)
		{
			return new Matrix4x4(
				1, 0, 0, x,
				0, 1, 0, y,
				0, 0, 1, z,
				0, 0, 0, 1);
		}
		/// <summary>
		/// Creates a 4x4 matrix which represents a translation.
		/// </summary>
		/// <param name="vec">The translation vector</param>
		/// <returns></returns>
		public static Matrix4x4 Translation(Vector3f vec)
		{
			return Translation(vec.X, vec.Y, vec.Z);
		}

		/// <summary>
		/// Creates a 4x4 matrix which represents a scaling operation.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		public static Matrix4x4 Scale(float x, float y, float z)
		{
			return new Matrix4x4(
				x, 0, 0, 0,
				0, y, 0, 0,
				0, 0, z, 0,
				0, 0, 0, 1);
		}
		/// <summary>
		/// Creates a 4x4 matrix which rotates about the x-axis.
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static Matrix4x4 RotateX(float angle)
		{
			float cos = (float)Math.Cos(angle);
			float sin = (float)Math.Sin(angle);

			return new Matrix4x4(
				1, 0, 0, 0,
				0, cos, -sin, 0,
				0, sin, cos, 0,
				0, 0, 0, 1);
		}
		/// <summary>
		/// Creates a 4x4 matrix which rotates about the y-axis.
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static Matrix4x4 RotateY(float angle)
		{
			float cos = (float)Math.Cos(angle);
			float sin = (float)Math.Sin(angle);

			return new Matrix4x4(
				cos, 0, sin, 0,
				0, 1, 0, 0,
				-sin, 0, cos, 0,
				0, 0, 0, 1);
		}
		/// <summary>
		/// Creates a 4x4 matrix which rotates about the z-axis.
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static Matrix4x4 RotateZ(double angle)
		{
			return RotateZ((float)angle);
		}
		/// <summary>
		/// Creates a 4x4 matrix which rotates about the z-axis.
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static Matrix4x4 RotateZ(float angle)
		{
			float cos = (float)Math.Cos(angle);
			float sin = (float)Math.Sin(angle);

			return new Matrix4x4(
				cos, -sin, 0, 0,
				sin, cos, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1);
		}
		/// <summary>
		/// Creates a 4x4 matrix which reflects x coordinates.
		/// </summary>
		/// <returns></returns>
		public static Matrix4x4 ReflectX()
		{
			return new Matrix4x4(
				-1,0, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1);
		}
		/// <summary>
		/// Creates a 4x4 matrix which reflects y coordinates.
		/// </summary>
		/// <returns></returns>
		public static Matrix4x4 ReflectY()
		{
			return new Matrix4x4(
				1, 0, 0, 0,
				0,-1, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1);
		}
		/// <summary>
		/// Creates a 4x4 matrix which reflects z coordinates.
		/// </summary>
		/// <returns></returns>
		public static Matrix4x4 ReflectZ()
		{
			return new Matrix4x4(
				1, 0, 0, 0,
				0, 1, 0, 0,
				0, 0,-1, 0,
				0, 0, 0, 1);
		}
		/// <summary>
		/// Creates a view matrix given a camera position, and target to look at and an up direction.
		/// </summary>
		/// <param name="eye">The camera position.</param>
		/// <param name="target">The object being looked at (only the direction of target - eye is relevant)</param>
		/// <param name="up">Which direction is up.</param>
		/// <returns></returns>
		public static Matrix4x4 ViewLookAt(Vector3f eye, Vector3f target, Vector3f up)
		{
			// equation from
			// http://pyopengl.sourceforge.net/documentation/manual/gluLookAt.3G.xml

			Vector3f f = (target - eye);

			f /= f.Magnitude;
			up /= up.Magnitude;

			Vector3f s = f.CrossProduct(up);
			s /= s.Magnitude;
			s /= s.Magnitude;

			Vector3f u = s.CrossProduct(f);

			Matrix4x4 result = new Matrix4x4(
				s.X, s.Y, s.Z, -s.DotProduct(eye),
				u.X, u.Y, u.Z, -u.DotProduct(eye),
				-f.X, -f.Y, -f.Z, f.DotProduct(eye),
				0, 0, 0, 1);

			return result;
		}
		/// <summary>
		/// Creates a projection matrix for perspective corrected views.
		/// </summary>
		/// <param name="fieldOfViewY">The vertical field of view in degrees.</param>
		/// <param name="aspect">The aspect ratio of the view port.</param>
		/// <param name="zNear">The z value of the near clipping plane.</param>
		/// <param name="zFar">The z value of the far clipping plane.</param>
		/// <returns></returns>
		public static Matrix4x4 Projection(float fieldOfViewY, float aspect, float zNear, float zFar)
		{
			if (zFar == zNear)
				throw new ArgumentException("zFar and zNear must not be the same.");

			// equation from 
			// http://pyopengl.sourceforge.net/documentation/manual/gluPerspective.3G.xml

			float fovInRad = (float)(Math.PI * fieldOfViewY / 180.0);
			float cot = (float)(1.0 / Math.Tan(fovInRad / 2));
			float zDiff = zFar - zNear;

			return new Matrix4x4(
				cot / aspect, 0, 0, 0,
				0, cot, 0, 0,
				0, 0, -(zFar + zNear) / zDiff, -2 * zFar * zNear / zDiff,
				0, 0, -1, 0);
		}
		/// <summary>
		/// Creates a projection matrix for an orthogonal perspective, as is used in 
		/// 2D drawing.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="zNear"></param>
		/// <param name="zFar"></param>
		/// <returns></returns>
		public static Matrix4x4 Ortho(RectangleF r, float zNear, float zFar)
		{
			// equation from 
			// http://pyopengl.sourceforge.net/documentation/manual/glOrtho.3G.xml

			return new Matrix4x4(
				2 / r.Width, 0, 0, -(r.Right + r.Left) / r.Width,
				0, -2 / r.Height, 0, (r.Top + r.Bottom) / r.Height,
				0, 0, -2 / (zFar - zNear), -(zFar + zNear) / (zFar - zNear),
				0, 0, 0, 1);
		}

		/// <summary>
		/// Constructs a 4x4 matrix.
		/// </summary>
		/// <param name="a11"></param>
		/// <param name="a12"></param>
		/// <param name="a13"></param>
		/// <param name="a14"></param>
		/// <param name="a21"></param>
		/// <param name="a22"></param>
		/// <param name="a23"></param>
		/// <param name="a24"></param>
		/// <param name="a31"></param>
		/// <param name="a32"></param>
		/// <param name="a33"></param>
		/// <param name="a34"></param>
		/// <param name="a41"></param>
		/// <param name="a42"></param>
		/// <param name="a43"></param>
		/// <param name="a44"></param>
		public Matrix4x4(float a11, float a12, float a13, float a14,
					   float a21, float a22, float a23, float a24,
					   float a31, float a32, float a33, float a34,
					   float a41, float a42, float a43, float a44)
		{
			m11 = a11; m12 = a12; m13 = a13; m14 = a14;
			m21 = a21; m22 = a22; m23 = a23; m24 = a24;
			m31 = a31; m32 = a32; m33 = a33; m34 = a34;
			m41 = a41; m42 = a42; m43 = a43; m44 = a44;
		}

		/// <summary>
		/// Gets or sets a value in the matrix.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		public float this[int row, int col]
		{
			get
			{
				if (col < 0 || col > 3) throw new ArgumentOutOfRangeException("col");

				switch (row)
				{
					case 0: return SelectValue(col, m11, m12, m13, m14);
					case 1: return SelectValue(col, m21, m22, m23, m24);
					case 2: return SelectValue(col, m31, m32, m33, m34);
					case 3: return SelectValue(col, m41, m42, m43, m44);
					default: throw new ArgumentOutOfRangeException("row");
				}
			}
			set
			{
				if (col < 0 || col > 3) throw new ArgumentOutOfRangeException("col");

				switch (row)
				{
					case 0:
						SetValue(col, value, ref m11, ref m12, ref m13, ref m14);
						break;
					case 1:
						SetValue(col, value, ref m21, ref m22, ref m23, ref m24);
						break;
					case 2:
						SetValue(col, value, ref m31, ref m32, ref m33, ref m34);
						break;
					case 3:
						SetValue(col, value, ref m41, ref m42, ref m43, ref m44);
						break;
					default:
						throw new ArgumentOutOfRangeException("row");
				}
			}
		}

		private void SetValue(int index, float value, ref float v1, ref float v2, ref float v3, ref float v4)
		{
			switch (index)
			{
				case 0: v1 = value; break;
				case 1: v2 = value; break;
				case 2: v3 = value; break;
				case 3: v4 = value; break;
				default:
					throw new ArgumentOutOfRangeException("index");
			}
		}

		private float SelectValue(int index, float v1, float v2, float v3, float v4)
		{
			switch (index)
			{
				case 0: return v1;
				case 1: return v2;
				case 2: return v3;
				case 3: return v4;
				default:
					throw new ArgumentOutOfRangeException("index");
			}
		}

		/// <summary>
		/// Returns the transpose of a matrix.
		/// </summary>
		/// <returns></returns>
		public Matrix4x4 Transpose()
		{
			return new Matrix4x4(
				m11, m21, m31, m41,
				m12, m22, m32, m42,
				m13, m23, m33, m43,
				m14, m24, m34, m44);
		}

		private Matrix4x4 Mult(Matrix4x4 m)
		{
			Matrix4x4 result = new Matrix4x4();

			for (int row = 0; row < 4; row++)
			{
				for (int col = 0; col < 4; col++)
				{
					for (int inner = 0; inner < 4; inner++)
					{
						result[row, col] +=
							this[row, inner] * m[inner, col];
					}
				}
			}

			return result;
		}
		/// <summary>
		/// Multiplies two matrices together.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right)
		{
			return left.Mult(right);
		}
		/// <summary>
		/// Multiplies a matrix on the left by a column vector on the right.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Vector4f operator *(Matrix4x4 left, Vector4f right)
		{
			Vector4f result = new Vector4f();

			for (int row = 0; row < 4; row++)
			{
				for (int col = 0; col < 4; col++)
				{
					result[row] += left[row, col] * right[col];
				}
			}

			return result;
		}
	}
}
