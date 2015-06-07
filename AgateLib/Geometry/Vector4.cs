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
using System.Runtime.Serialization;
using System.Text;

namespace AgateLib.Geometry
{
	/// <summary>
	/// Structure which describes a vector in 3-space.
	/// </summary>
	[DataContract]
	public struct Vector4
	{
		[DataMember]
		private float mX, mY, mZ, mW;

		/// <summary>
		/// Constructs a Vector4 object.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="w"></param>
		public Vector4(float x, float y, float z, float w)
		{
			mX = x;
			mY = y;
			mZ = z;
			mW = w;
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
			mX = (float)x;
			mY = (float)y;
			mZ = (float)z;
			mW = (float)w;
		}
		/// <summary>
		/// X coordinate.
		/// </summary>
		public float X
		{
			get { return mX; }
			set { mX = value; }
		}
		/// <summary>
		/// Y coordinate.
		/// </summary>
		public float Y
		{
			get { return mY; }
			set { mY = value; }
		}
		/// <summary>
		/// Z coordinate.
		/// </summary>
		public float Z
		{
			get { return mZ; }
			set { mZ = value; }
		}
		/// <summary>
		/// Homogenous W coordinate.
		/// </summary>
		public float W
		{
			get { return mW; }
			set { mW = value; }
		}

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
					case 0: return mX;
					case 1: return mY;
					case 2: return mZ;
					case 3: return mW;
					default: throw new ArgumentOutOfRangeException();
				}
			}
			set
			{
				switch (component)
				{
					case 0: mX = value; break;
					case 1: mY = value; break;
					case 2: mZ = value; break;
					case 3: mW = value; break;
					default: throw new ArgumentOutOfRangeException();
				}
			}
		}
		/// <summary>
		/// Vector representing the origin.
		/// </summary>
		public static readonly Vector4 Empty = new Vector4();

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
