using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Geometry
{
    /// <summary>
    /// Structure which describes a vector in 3-space.
    /// </summary>
    public struct Vector3
    {
        private float mX, mY, mZ;

        /// <summary>
        /// Constructs a Vector3 object.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3(float x, float y, float z)
        {
            mX = x;
            mY = y;
            mZ = z;
        }
        /// <summary>
        /// Constructs a Vector3 object.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3(double x, double y, double z)
        {
            mX = (float)x;
            mY = (float)y;
            mZ = (float)z;
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
        /// Vector representing the origin.
        /// </summary>
        public static readonly Vector3 Empty = new Vector3();

        public bool IsEmpty
        {
            get { return X == 0 && Y == 0 && Z == 0; }
        }
    }
}
