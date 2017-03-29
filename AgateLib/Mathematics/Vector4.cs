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
using System.Runtime.Serialization;

namespace AgateLib.Mathematics
{
	/// <summary>
	/// Structure which describes a vector in 3-space.
	/// </summary>
	[DataContract]
	public struct Vector4
	{
		/// <summary>
		/// Vector representing the origin.
		/// </summary>
		public static readonly Vector4 Zero = new Vector4();

		/// <summary>
		/// Vector representing the origin.
		/// </summary>
		[Obsolete("Use Vector2.Zero to be more explicit", true)]
		public static readonly Vector4 Empty = new Vector4();

		/// <summary>
		/// Returns a unit vector that points in the +X direction.
		/// </summary>
		public static readonly Vector4 UnitX = new Vector4(1, 0, 0, 0);

		/// <summary>
		/// Returns a unit vector that points in the +Y direction.
		/// </summary>
		public static readonly Vector4 UnitY = new Vector4(0, 1, 0, 0);

		/// <summary>
		/// Returns a unit vector that points in the +Z direction.
		/// </summary>
		public static readonly Vector4 UnitZ = new Vector4(0, 0, 1, 0);

		/// <summary>
		/// Returns a unit vector that points in the +W direction.
		/// </summary>
		public static readonly Vector4 UnitW = new Vector4(0, 0, 0, 1);

		/// <summary>
		/// Constructs a Vector4 object.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="w"></param>
		public Vector4(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}
		/// <summary>
		/// Constructs a Vector4 object.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="w"></param>
		public Vector4(double x, double y, double z, double w)
		{
			X = (float)x;
			Y = (float)y;
			Z = (float)z;
			W = (float)w;
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
		/// Homogenous W coordinate.
		/// </summary>
		[DataMember]
		public float W;

		/// <summary>
		/// Gets or sets a component by index.  They are in the order 0: X, 1: Y, 2: Z, 3: W.
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		public float this[int component]
		{
			get
			{
				switch (component)
				{
					case 0: return X;
					case 1: return Y;
					case 2: return Z;
					case 3: return W;
					default: throw new ArgumentOutOfRangeException();
				}
			}
			set
			{
				switch (component)
				{
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
					case 3: W = value; break;
					default: throw new ArgumentOutOfRangeException();
				}
			}
		}
		
		/// <summary>
		/// Returns true if this vector's components are all zero.
		/// </summary>
		public bool IsEmpty
		{
			get { return X == 0 && Y == 0 && Z == 0 && W == 0; }
		}

		/// <summary>
		/// Returns the square of the length of the vector.
		/// </summary>
		public float MagnitudeSquared
		{
			get { return X * X + Y * Y + Z * Z + W * W; }
		}
		/// <summary>
		/// Returns the length of the vector.
		/// </summary>
		public float Magnitude
		{
			get { return (float)Math.Sqrt(MagnitudeSquared); }
		}
		/// <summary>
		/// Returns a vector pointing in the same direction as this one, with magnitude 1.
		/// </summary>
		/// <returns></returns>
		public Vector4 Normalize()
		{
			Vector4 result = this / Magnitude;

			return result;
		}

		/// <summary>
		/// Explicit conversion to a Vector4f object.
		/// </summary>
		/// <param name="v"></param>
		public static explicit operator Vector4f(Vector4 v)
		{
			return new Vector4f(v.X, v.Y, v.Z, v.W);
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector4 operator +(Vector4 a, Vector4 b)
		{
			return new Vector4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
		}
		/// <summary>
		/// Subtracts two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector4 operator -(Vector4 a, Vector4 b)
		{
			return new Vector4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
		}
		/// <summary>
		/// Unary - operator: multiples vector by -1.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static Vector4 operator -(Vector4 a)
		{
			return new Vector4(-a.X, -a.Y, -a.Z, -a.W);
		}
		/// <summary>
		/// Scales a vector by a scalar floating point value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector4 operator *(Vector4 a, float b)
		{
			return new Vector4(a.X * b, a.Y * b, a.Z * b, a.W * b);
		}
		/// <summary>
		/// Divides a vector's components by a floating point value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector4 operator /(Vector4 a, float b)
		{
			return a * (1.0f / b);
		}

		/// <summary>
		/// Computes and returns the dot product with another vector.
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public float DotProduct(Vector4 b)
		{
			return X * b.X + Y * b.Y + Z * b.Z + W * b.W;
		}
		/// <summary>
		/// Computes and returns the dot product between two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float DotProduct(Vector4 a, Vector4 b)
		{
			return a.DotProduct(b);
		}
	}
}
