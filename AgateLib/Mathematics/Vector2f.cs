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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Mathematics
{
	/// <summary>
	/// Structure which describes a vector in 2-space.  The Vector2 class 
	/// contains overloads for mathematical operations to make computation expressions
	/// involving the Vector2 simple and expressive.
	/// </summary>
	[DataContract]
	public struct Vector2f
	{
		/// <summary>
		/// Returns a unit vector that points in the +X direction.
		/// </summary>
		/// <returns></returns>
		public static readonly Vector2f UnitX = new Vector2f(1, 0);

		/// <summary>
		/// Returns a unit vector that points in the +Y direction.
		/// </summary>
		/// <returns></returns>
		public static readonly Vector2f UnitY = new Vector2f(0, 1);

		/// <summary>
		/// Vector representing the origin.
		/// </summary>
		public static readonly Vector2f Zero = new Vector2f();

		/// <summary>
		/// Vector representing the origin.
		/// </summary>
		[Obsolete("Use Vector2.Zero to be more explicit", true)]
		public static readonly Vector2f Empty = new Vector2f();

		/// <summary>
		/// Constructs a Vector2 object.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public Vector2f(float x, float y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Constructs a Vector2 object.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public Vector2f(double x, double y)
		{
			X = (float)x;
			Y = (float)y;
		}

		/// <summary>
		/// Constructs a Vector2 object from a Point.
		/// </summary>
		/// <param name="pt"></param>
		public Vector2f(Point pt)
		{
			X = pt.X;
			Y = pt.Y;
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
		/// Explicit conversion to a Vector2 object.
		/// </summary>
		/// <param name="v"></param>
		public static explicit operator Vector2(Vector2f v)
		{
			return new Vector2(v.X, v.Y);
		}

		/// <summary>
		/// Converts to a Vector3 object, with a Z value of zero.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static explicit operator Vector3f(Vector2f v)
		{
			return new Vector3f(v.X, v.Y, 0);
		}

		/// <summary>
		/// Converts to a PointF object.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static explicit operator PointF(Vector2f v)
		{
			return new PointF(v.X, v.Y);
		}

		/// <summary>
		/// Converts to a Point object.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static explicit operator Point(Vector2f v)
		{
			return new Point((int)v.X, (int)v.Y);
		}

		/// <summary>
		/// Returns true if this vector has zero for all components.
		/// </summary>
		public bool IsZero
		{
			get { return X == 0 && Y == 0; }
		}

		/// <summary>
		/// Returns true if this vector has zero for all components.
		/// </summary>
		[Obsolete("Use IsZero to be more explicit")]
		public bool IsEmpty
		{
			get { return X == 0 && Y == 0; }
		}

		/// <summary>
		/// Returns the square of the length of the vector.
		/// </summary>
		public float MagnitudeSquared
		{
			get { return X * X + Y * Y; }
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
		public Vector2f Normalize()
		{
			Vector2f result = this / Magnitude;

			return result;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2f operator +(Vector2f a, Vector2f b)
		{
			return new Vector2f(a.X + b.X, a.Y + b.Y);
		}

		/// <summary>
		/// Adds a vector to every vector in an enumerable.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static IEnumerable<Vector2f> operator +(Vector2f a, IEnumerable<Vector2f> b)
		{
			foreach (var item in b)
				yield return a + item;
		}


		/// <summary>
		/// Adds a vector to every vector in an enumerable.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static IEnumerable<Vector2f> operator +(IEnumerable<Vector2f> a, Vector2f b)
		{
			return b + a;
		}

		/// <summary>
		/// Subtracts two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2f operator -(Vector2f a, Vector2f b)
		{
			return new Vector2f(a.X - b.X, a.Y - b.Y);
		}

		/// <summary>
		/// Adds a vector to every vector in an enumerable.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static IEnumerable<Vector2f> operator -(Vector2f a, IEnumerable<Vector2f> b)
		{
			foreach (var item in b)
				yield return a - item;
		}

		/// <summary>
		/// Adds a vector to every vector in an enumerable.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static IEnumerable<Vector2f> operator -(IEnumerable<Vector2f> a, Vector2f b)
		{
			foreach (var item in a)
				yield return item - b;
		}

		/// <summary>
		/// Unary - operator: multiples vector by -1.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static Vector2f operator -(Vector2f a)
		{
			return new Vector2f(-a.X, -a.Y);
		}

		/// <summary>
		/// Scales a vector by a scalar floating point value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2f operator *(Vector2f a, float b)
		{
			return new Vector2f(a.X * b, a.Y * b);
		}

		/// <summary>
		/// Scales a vector by a scalar floating point value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2f operator *(float b, Vector2f a)
		{
			return new Vector2f(a.X * b, a.Y * b);
		}

		/// <summary>
		/// Scales a vector by a scalar floating point value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2f operator *(Vector2f a, double b)
		{
			return new Vector2f(a.X * b, a.Y * b);
		}

		/// <summary>
		/// Scales a vector by a scalar floating point value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2f operator *(double b, Vector2f a)
		{
			return new Vector2f(a.X * b, a.Y * b);
		}

		/// <summary>
		/// Divides a vector's components by a floating point value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2f operator /(Vector2f a, float b)
		{
			return a * (1.0f / b);
		}

		/// <summary>
		/// Divides a vector's components by a floating point value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2f operator /(Vector2f a, double b)
		{
			return a * (float)(1.0f / b);
		}

		/// <summary>
		/// Performs equality comparison. This requires exact equality, so 
		/// floating-point precision may be a problem. The Equals method is
		/// better if this is a concern.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator ==(Vector2f a, Vector2f b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		/// <summary>
		/// Performs inequality comparison. This requires exact equality, so 
		/// floating-point precision may be a problem. The Equals method is
		/// better if this is a concern.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator !=(Vector2f a, Vector2f b)
		{
			return a.X != b.X || a.Y != b.Y;
		}

		/// <summary>
		/// Overrides the base class method. Use overload which accepts
		/// tolerance instead.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (!(obj is Vector2f))
				return false;

			return this == (Vector2f) obj;
		}

		/// <summary>
		/// Returns the hash code for the vector.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		/// <summary>
		/// Performs equality comparison to within a tolerance value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="tolerance"></param>
		/// <returns></returns>
		public static bool Equals(Vector2f a, Vector2f b, double tolerance)
		{
			return Math.Abs(a.X - b.X) < tolerance &&
			       Math.Abs(a.Y - b.Y) < tolerance;
		}

		/// <summary>
		/// Performs equality comparison to within a tolerance value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="tolerance"></param>
		/// <returns></returns>
		public bool Equals(Vector2f b, double tolerance)
		{
			return Math.Abs(X - b.X) < tolerance &&
				   Math.Abs(Y - b.Y) < tolerance;
		}

		/// <summary>
		/// Computes and returns the dot product with another vector.
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public float DotProduct(Vector2f b)
		{
			return DotProduct(this, b);
		}

		/// <summary>
		/// Computes and returns the dot product between two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float DotProduct(Vector2f a, Vector2f b)
		{
			return a.X * b.X + a.Y * b.Y;
		}

		/// <summary>
		/// Computes and returns the angle between two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float AngleBetween(Vector2f a, Vector2f b)
		{
			return (float)Math.Acos(DotProduct(a, b) / (a.Magnitude * b.Magnitude));
		}

		/// <summary>
		/// Computes and returns the distance between two points.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float DistanceBetween(Vector2f a, Vector2f b)
		{
			return (a - b).Magnitude;
		}

		/// <summary>
		/// Returns a string representation of the Vector2 object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.CurrentCulture,
				"(X={0};Y={1})", X, Y);
		}

		/// <summary>
		/// Produces a Vector2 object from polar coordinates. 
		/// </summary>
		/// <remarks>Angles in the first
		/// quadrant (between 0 and pi/2) will have positive x and y coordinates, 
		/// which will be downright in the usual screen coordinates. If you want it
		/// to point in the upper right, using -angle instead.
		/// </remarks>
		/// <param name="radius"></param>
		/// <param name="angle">The angle in radians</param>
		/// <returns></returns>
		public static Vector2f FromPolar(double radius, double angle)
		{
			Vector2f result = new Vector2f(
				radius * Math.Cos(angle),
				radius * Math.Sin(angle));

			return result;
		}

		/// <summary>
		/// Produces a Vector2 object from polar coordinates. 
		/// </summary>
		/// <remarks>Angles in the first
		/// quadrant (between 0 and 90) will have positive x and y coordinates, 
		/// which will be downright in the usual screen coordinates. If you want it
		/// to point in the upper right, using -angle instead.
		/// </remarks>
		/// <param name="radius"></param>
		/// <param name="angle">The angle in degrees</param>
		/// <returns></returns>
		public static Vector2f FromPolarDegrees(double radius, double angle)
		{
			return FromPolar(radius, angle * Math.PI / 180.0);
		}
	}
}
