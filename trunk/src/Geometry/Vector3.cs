using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Geometry
{
    public struct Vector3
    {
        private float mX, mY, mZ;

        public Vector3(float x, float y, float z)
        {
            mX = x;
            mY = y;
            mZ = z;
        }
        public Vector3(double x, double y, double z)
        {
            mX = (float)x;
            mY = (float)y;
            mZ = (float)z;
        }
        public float X
        {
            get { return mX; }
            set { mX = value; }
        }
        public float Y
        {
            get { return mY; }
            set { mY = value; }
        }
        public float Z
        {
            get { return mZ; }
            set { mZ = value; }
        }

        public static Vector3 Empty = new Vector3();
    }
}
