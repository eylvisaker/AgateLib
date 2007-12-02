using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Geometry
{
    /// <summary>
    /// Structure which describes a vector in 3-space.
    /// </summary>
    public struct Vector2
    {
        private float mX, mY;

        /// <summary>
        /// Constructs a Vector2 object.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2(float x, float y)
        {
            mX = x;
            mY = y;
        }
        /// <summary>
        /// Constructs a Vector2 object.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2(double x, double y)
        {
            mX = (float)x;
            mY = (float)y;
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
        /// Vector representing the origin.
        /// </summary>
        public static readonly Vector2 Empty = new Vector2();

        /// <summary>
        /// Returns true if this vector has zero for all components.
        /// </summary>
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
        /// Returns a normalized version of this vector
        /// </summary>
        /// <returns></returns>
        public Vector2 Normalize()
        {
            Vector2 retval = this;
            retval /= Magnitude;

            return retval;
        }
        /// <summary>
        /// Scales a vector by a scalar floating point value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.X * b, a.Y * b);
        }
        /// <summary>
        /// Divides a vector's components by a floating point value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2 operator /(Vector2 a, float b)
        {
            return a * (1.0f / b);
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
        /// Unary - operator: multiples vector by -1.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(-a.X, -a.Y);
        }

        /// <summary>
        /// Computes and returns the dot product with another vector.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public float DotProduct(Vector2 b)
        {
            return DotProduct(this, b);
        }
        /// <summary>
        /// Computes and returns the dot product between two vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float DotProduct(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
    }
}
