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
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace AgateLib.Mathematics
{
	/// <summary>
	/// Structure which describes a vector in 3-space.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	[DataContract]
	public struct Vector3f
	{
		/// <summary>
		/// Vector representing the origin.
		/// </summary>
		public static readonly Vector3f Zero = new Vector3f();

		/// <summary>
		/// Vector representing the origin.
		/// </summary>
		[Obsolete("Use Vector2.Zero to be more explicit", true)]
		public static readonly Vector3f Empty = new Vector3f();

		/// <summary>
		/// Returns a unit vector that points in the +X direction.
		/// </summary>
		public static readonly Vector3f UnitX = new Vector3f(1, 0, 0);

		/// <summary>
		/// Returns a unit vector that points in the +Y direction.
		/// </summary>
		public static readonly Vector3f UnitY = new Vector3f(0, 1, 0);

		/// <summary>
		/// Returns a unit vector that points in the +Z direction.
		/// </summary>
		public static readonly Vector3f UnitZ = new Vector3f(0, 0, 1);

		/// <summary>
		/// Constructs a Vector3 object.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public Vector3f(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}
		/// <summary>
		/// Constructs a Vector3 object.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public Vector3f(double x, double y, double z)
		{
			X = (float)x;
			Y = (float)y;
			Z = (float)z;
		}

		/// <summary>
		/// X coordinate.
		/// </summary>
		[DataMember]
		public float X;

		/// <summary>
		/// Y coordinate.
		/// </summary>
		[DataMember]
		public float Y;

		/// <summary>
		/// Z coordinate.
		/// </summary>
		[DataMember]
		public float Z;

		/// <summary>
		/// Returns true if this vector's components are all zero.
		/// </summary>
		public bool IsZero
		{
			get { return X == 0 && Y == 0 && Z == 0; }
		}

		[Obsolete("Use IsZero instead.", true)]
		public bool IsEmpty => IsZero;

		/// <summary>
		/// Returns the square of the length of the vector.
		/// </summary>
		public float MagnitudeSquared => X * X + Y * Y + Z * Z;

		/// <summary>
		/// Returns the length of the vector.
		/// </summary>
		public float Magnitude => (float)Math.Sqrt(MagnitudeSquared);

		/// <summary>
		/// Returns a vector pointing in the same direction as this one, with magnitude 1.
		/// </summary>
		/// <returns></returns>
		public Vector3f Normalize() => this / Magnitude;

		/// <summary>
		/// Explicit conversion to a Vector3 object.
		/// </summary>
		/// <param name="v"></param>
		public static explicit operator Vector3(Vector3f v)
		{
			return new Vector3(v.X, v.Y, v.Z);
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3f operator +(Vector3f a, Vector3f b)
		{
			return new Vector3f(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}

		/// <summary>
		/// Subtracts two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3f operator -(Vector3f a, Vector3f b)
		{
			return new Vector3f(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}

		/// <summary>
		/// Unary - operator: multiples vector by -1.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static Vector3f operator -(Vector3f a)
		{
			return new Vector3f(-a.X, -a.Y, -a.Z);
		}

		/// <summary>
		/// Scales a vector by a scalar floating point value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3f operator *(Vector3f a, float b)
		{
			return new Vector3f(a.X * b, a.Y * b, a.Z * b);
		}

		/// <summary>
		/// Scales a vector by a scalar floating point value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3f operator *(float a, Vector3f b)
		{
			return new Vector3f(a * b.X, a * b.Y, a * b.Z);
		}

		/// <summary>
		/// Scales a vector by a scalar floating point value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3f operator *(Vector3f a, double b)
		{
			return a * (float)b;
		}

		/// <summary>
		/// Scales a vector by a scalar floating point value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3f operator *(double a, Vector3f b)
		{
			return b * (float)a;
		}
		/// <summary>
		/// Divides a vector's components by a floating point value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3f operator /(Vector3f a, float b)
		{
			return a * (1.0f / b);
		}

		/// <summary>
		/// Divides a vector's components by a floating point value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3f operator /(Vector3f a, double b)
		{
			return a * (float)(1.0 / b);
		}

		/// <summary>
		/// Performs equality comparison to within a tolerance value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="tolerance"></param>
		/// <returns></returns>
		public static bool Equals(Vector3f a, Vector3f b, double tolerance)
		{
			return Math.Abs(a.X - b.X) < tolerance &&
				   Math.Abs(a.Y - b.Y) < tolerance &&
				   Math.Abs(a.Z - b.Z) < tolerance;
		}

		/// <summary>
		/// Computes and returns the dot product with another vector.
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public float DotProduct(Vector3f b)
		{
			return DotProduct(this, b);
		}

		/// <summary>
		/// Computes and returns the dot product between two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float DotProduct(Vector3f a, Vector3f b)
		{
			return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
		}

		/// <summary>
		/// Returns the cross product of two vectors.
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public Vector3f CrossProduct(Vector3f b)
		{
			return CrossProduct(this, b);
		}

		/// <summary>
		/// Returns the cross product of two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3f CrossProduct(Vector3f a, Vector3f b)
		{
			return new Vector3f(
				a.Y * b.Z - a.Z * b.Y,
				a.Z * b.X - a.X * b.Z,
				a.X * b.Y - a.Y * b.X);
		}

		/// <summary>
		/// Computes and returns the angle between two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float AngleBetween(Vector3f a, Vector3f b)
		{
			return (float)Math.Acos(DotProduct(a, b) / (a.Magnitude * b.Magnitude));
		}

		/// <summary>
		/// Computes and returns the distance between two points.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float DistanceBetween(Vector3f a, Vector3f b)
		{
			return (a - b).Magnitude;
		}

		/// <summary>
		/// Returns a string representation of the Vector3 object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.CurrentCulture,
				"(X={0},Y={1},Z={2})", X, Y, Z);
		}

		/// <summary>
		/// Creates a Vector3 from polar spherical coordinates.
		/// </summary>
		/// <param name="length"></param>
		/// <param name="theta"></param>
		/// <param name="phi"></param>
		/// <returns></returns>
		public static Vector3f FromPolar(int length, float theta, float phi)
		{
			return length * new Vector3f(
				Math.Sin(theta) * Math.Cos(phi),
				Math.Sin(theta) * Math.Sin(phi),
				Math.Cos(theta));
		}
	}
}
