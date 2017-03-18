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
using System.Collections;
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
	public struct Vector2
	{
		/// <summary>
		/// Vector representing the origin.
		/// </summary>
		public static readonly Vector2 Zero = new Vector2();

		/// <summary>
		/// Vector representing the origin.
		/// </summary>
		[Obsolete("Use Vector2.Zero to be more explicit", true)]
		public static readonly Vector2 Empty = new Vector2();

		/// <summary>
		/// Returns a unit vector that points in the +X direction.
		/// </summary>
		public static readonly Vector2 UnitX = new Vector2(1, 0);

		/// <summary>
		/// Returns a unit vector that points in the +Y direction.
		/// </summary>
		public static readonly Vector2 UnitY = new Vector2(0, 1);

		/// <summary>
		/// Constructs a Vector2 object.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public Vector2(double x, double y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Constructs a Vector2 object from a Point.
		/// </summary>
		/// <param name="pt"></param>
		public Vector2(Point pt)
		{
			X = pt.X;
			Y = pt.Y;
		}

		/// <summary>
		/// X coordinate.
		/// </summary>
		[DataMember]
		public double X;

		/// <summary>
		/// Y coordinate.
		/// </summary>
		[DataMember]
		public double Y;

		/// <summary>
		/// Explicit conversion to a Vector2f object.
		/// </summary>
		/// <param name="v"></param>
		public static explicit operator Vector2f(Vector2 v)
		{
			return new Vector2f(v.X, v.Y);
		}

		/// <summary>
		/// Converts to a Vector3 object, with a Z value of zero.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static explicit operator Vector3(Vector2 v)
		{
			return new Vector3(v.X, v.Y, 0);
		}

		/// <summary>
		/// Converts to a PointF object.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static explicit operator PointF(Vector2 v)
		{
			return new PointF((float)v.X, (float)v.Y);
		}

		/// <summary>
		/// Converts to a Point object.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static explicit operator Point(Vector2 v)
		{
			return new Point((int)v.X, (int)v.Y);
		}

		/// <summary>
		/// Returns true if this vector has zero for all components.
		/// </summary>
		public bool IsZero => X == 0 && Y == 0;

		/// <summary>
		/// Returns true if this vector has zero for all components.
		/// </summary>
		[Obsolete("Use IsZero to be more explicit", true)]
		public bool IsEmpty => IsZero;

		/// <summary>
		/// Returns the square of the length of the vector.
		/// </summary>
		public double MagnitudeSquared => X * X + Y * Y;

		/// <summary>
		/// Returns the length of the vector.
		/// </summary>
		public double Magnitude => Math.Sqrt(MagnitudeSquared);

		/// <summary>
		/// Returns the angle made between this point and the origin.
		/// </summary>
		public double Angle => Math.Atan2(Y, X);

		/// <summary>
		/// Returns a vector pointing in the same direction as this one, with magnitude 1.
		/// </summary>
		/// <returns></returns>
		public Vector2 Normalize()
		{
			Vector2 result = this / Magnitude;

			return result;
		}

		/// <summary>
		/// Adds two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2 operator +(Vector2 a, Vector2 b)
		{
			return new Vector2(a.X + b.X, a.Y + b.Y);
		}

		/// <summary>
		/// Adds a vector to every vector in an enumerable.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static IEnumerable<Vector2> operator +(Vector2 a, IEnumerable<Vector2> b)
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
		public static IEnumerable<Vector2> operator +(IEnumerable<Vector2> a, Vector2 b)
		{
			return b + a;
		}

		/// <summary>
		/// Subtracts two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2 operator -(Vector2 a, Vector2 b)
		{
			return new Vector2(a.X - b.X, a.Y - b.Y);
		}

		/// <summary>
		/// Adds a vector to every vector in an enumerable.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static IEnumerable<Vector2> operator -(Vector2 a, IEnumerable<Vector2> b)
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
		public static IEnumerable<Vector2> operator -(IEnumerable<Vector2> a, Vector2 b)
		{
			foreach (var item in a)
				yield return item - b;
		}

		/// <summary>
		/// Unary - operator: multiples vector by -1.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static Vector2 operator -(Vector2 a)
		{
			return new Vector2(-a.X, -a.Y);
		}

		/// <summary>
		/// Scales a vector by a scalar value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2 operator *(Vector2 a, double b)
		{
			return new Vector2(a.X * b, a.Y * b);
		}

		/// <summary>
		/// Scales a vector by a scalar value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2 operator *(double b, Vector2 a)
		{
			return new Vector2(a.X * b, a.Y * b);
		}
		
		/// <summary>
		/// Divides a vector's components by a scalar value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2 operator /(Vector2 a, double b)
		{
			return a * (1.0f / b);
		}

		/// <summary>
		/// Performs equality comparison. This requires exact equality, so 
		/// doubleing-point precision may be a problem. The Equals method is
		/// better if this is a concern.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator ==(Vector2 a, Vector2 b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		/// <summary>
		/// Performs inequality comparison. This requires exact equality, so 
		/// doubleing-point precision may be a problem. The Equals method is
		/// better if this is a concern.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator !=(Vector2 a, Vector2 b)
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
			if (!(obj is Vector2))
				return false;

			return this == (Vector2)obj;
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
		public static bool Equals(Vector2 a, Vector2 b, double tolerance)
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
		public bool Equals(Vector2 b, double tolerance)
		{
			return Math.Abs(X - b.X) < tolerance &&
				   Math.Abs(Y - b.Y) < tolerance;
		}

		/// <summary>
		/// Computes and returns the dot product with another vector.
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public double DotProduct(Vector2 b)
		{
			return DotProduct(this, b);
		}

		/// <summary>
		/// Computes and returns the dot product between two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double DotProduct(Vector2 a, Vector2 b)
		{
			return a.X * b.X + a.Y * b.Y;
		}

		/// <summary>
		/// Computes and returns the angle between two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double AngleBetween(Vector2 a, Vector2 b)
		{
			return Math.Acos(DotProduct(a, b) / (a.Magnitude * b.Magnitude));
		}

		/// <summary>
		/// Computes and returns the distance between two points.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double DistanceBetween(Vector2 a, Vector2 b)
		{
			return (a - b).Magnitude;
		}

		/// <summary>
		/// Computes and returns the square of the sitance between two points.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double DistanceSquared(Vector2 a, Vector2 b)
		{
			return (a - b).MagnitudeSquared;
		}

		/// <summary>
		/// Returns a string representation of the Vector2 object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.CurrentCulture,
				"({0},{1})", X, Y);
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
		public static Vector2 FromPolar(double radius, double angle)
		{
			Vector2 result = new Vector2(
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
		public static Vector2 FromPolarDegrees(double radius, double angle)
		{
			return FromPolar(radius, angle * Math.PI / 180.0);
		}

		/// <summary>
		/// Rotates the vector in the counterclockwise direction about the origin.
		/// </summary>
		/// <param name="angle">The angle in radians</param>
		/// <returns></returns>
		public Vector2 Rotate(double angle)
		{
			return new Vector2(
				X * Math.Cos(angle) + Y * Math.Sin(angle),
				-X * Math.Sin(angle) + Y * Math.Cos(angle));
		}
		
		/// <summary>
		/// Rotates the vector in the counterclockwise direction about the origin.
		/// </summary>
		/// <param name="angleInDegrees">The angle in degrees</param>
		/// <returns></returns>
		public Vector2 RotateDegrees(double angleInDegrees)
		{
			return Rotate(Math.PI / 180 * angleInDegrees);
		}
	}
}
