//
//    Copyright (c) 2006-2018 Erik Ylvisaker
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
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AgateLib.Mathematics
{
	public static class Vector2X
	{
		/// <summary>
		/// Returns the cross product of two 2D vectors as a scalar value.
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float Cross(Vector2 a, Vector2 b)
		{
			return a.X * b.Y - a.Y * b.X;
		}

		/// <summary>
		/// Returns the projection of this vector in the direction of another vector.
		/// </summary>
		/// <param name="t"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Vector2 ProjectionOn(this Vector2 t, Vector2 direction)
		{
			var normal = direction;
			normal.Normalize();

			return Vector2.Dot(t, normal) * normal;
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
		public static Vector2 FromPolar(float radius, float angle)
		{
			Vector2 result = new Vector2(
				radius * (float)Math.Cos(angle),
				radius * (float)Math.Sin(angle));

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
		public static Vector2 FromPolarDegrees(float radius, float angle)
		{
			return FromPolar(radius, angle * (float)Math.PI / 180.0f);
		}

		/// <summary>
		/// Rotates the vector in the counterclockwise direction about the origin.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="angle">The angle in radians</param>
		/// <returns></returns>
		public static Vector2 Rotate(this Vector2 v, float angle)
		{
			float c = (float)Math.Cos(angle);
			float s = (float)Math.Sin(angle);

			return new Vector2(
				v.X * c + v.Y * s,
				-v.X * s + v.Y * c);
		}

		/// <summary>
		/// Rotates the vector in the counterclockwise direction about the origin.
		/// </summary>
		/// <param name="angleInDegrees">The angle in degrees</param>
		/// <returns></returns>
		public static Vector2 RotateDegrees(this Vector2 v, float angleInDegrees)
		{
			return v.Rotate((float)Math.PI / 180 * angleInDegrees);
		}
		/// <summary>
		/// Parses a string of the format "x,y" into a Vector2 object.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Vector2 Parse(string value)
		{
			var coords = value.Split(',');

			if (coords.Length != 2)
				throw new FormatException("Value was not the correct format.");

			return new Vector2(float.Parse(coords[0]), float.Parse(coords[1]));
		}

		/// <summary>
		/// Adds a vector to every vector in an enumerable.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="items"></param>
		/// <returns></returns>
		public static IEnumerable<Vector2> Add(Vector2 v, IEnumerable<Vector2> items)
		{
			foreach (var item in items)
				yield return v + item;
		}


		/// <summary>
		/// Adds a vector to every vector in an enumerable.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="v"></param>
		/// <returns></returns>
		public static IEnumerable<Vector2> Add(IEnumerable<Vector2> item, Vector2 v)
		{
			return Add(v, item);
		}

		/// <summary>
		/// Performs equality comparison to within a tolerance value.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="tolerance"></param>
		/// <returns></returns>
		public static bool Equals(Vector2 a, Vector2 b, float tolerance = 1e-6f)
		{
			return Math.Abs(a.X - b.X) < tolerance &&
				   Math.Abs(a.Y - b.Y) < tolerance;
		}

		/// <summary>
		/// Returns the angle made between this point and the origin.
		/// </summary>
		public static float Angle(this Vector2 v) => (float)Math.Atan2(v.Y, v.X);

		/// <summary>
		/// Computes and returns the angle between two vectors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static double AngleBetween(Vector2 a, Vector2 b)
		{
			return Math.Acos(Vector2.Dot(a, b) / (a.Length() * b.Length()));
		}

		/// <summary>
		/// Tests whether the vector is zero within tolerance.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static bool IsZero(this Vector2 v, float tolerance = 1e-6f)
		{
			if (v.X > tolerance) return false;
			if (v.Y > tolerance) return false;
			if (v.X < -tolerance) return false;
			if (v.Y < -tolerance) return false;

			return true;
		}
	}
}
