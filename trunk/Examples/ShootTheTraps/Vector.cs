using System;
using System.Collections.Generic;
using System.Text;

namespace ShootTheTraps
{
    public struct Vector3d
    {
        public double mX, mY, mZ;

        public double X
        {
            get { return mX; }
            set { mX = value; }
        }


        public double Y
        {
            get { return mY; }
            set { mY = value; }
        }

        public double Z
        {
            get { return mZ; }
            set { mZ = value; }
        }

        public Vector3d(double _x, double _y, double _z)
        {
            mX = _x;
            mY = _y;
            mZ = _z;
        }
        public Vector3d(int _x, int _y, int _z)
        {
            mX = (double)_x;
            mY = (double)_y;
            mZ = (double)_z;
        }

        public Vector3d(Vector3d copy)
        {
            mX = copy.mX;
            mY = copy.mY;
            mZ = copy.mZ;
        }

        public static Vector3d operator *(Vector3d a, double scaleFactor)
        {
            Vector3d result = new Vector3d(a);

            result.mX *= scaleFactor;
            result.mY *= scaleFactor;
            result.mZ *= scaleFactor;

            return result;
        }
        public static Vector3d operator +(Vector3d a, Vector3d b)
        {
            Vector3d result = new Vector3d(a);

            result.mX += b.mX;
            result.mY += b.mY;
            result.mZ += b.mZ;

            return result;
        }
        public static Vector3d operator -(Vector3d a, Vector3d b)
        {
            Vector3d result = new Vector3d(a);

            result.mX -= b.mX;
            result.mY -= b.mY;
            result.mZ -= b.mZ;

            return result;

        }

        public double MagnitudeSquared
        {
            get
            {
                return mX * mX + mY * mY + mZ * mZ;
            }
        }
        public double Magnitude
        {
            get
            {
                return Math.Sqrt(MagnitudeSquared);
            }
        }

        public Vector3d Normalize()
        {
            double mag = Magnitude;

            return new Vector3d(mX / mag, mY / mag, mZ / mag);
        }
    }

}
