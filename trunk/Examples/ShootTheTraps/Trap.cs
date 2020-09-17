using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace ShootTheTraps
{
    public class Trap : GameObject
    {
        const int width = 14;
        const int height = 8;
        int mFinalY = 0;
        Color mColor = Color.Red;

        bool delete = false;

        static Random sRandom;
        static Color[] sColors = { Color.White, Color.Blue, Color.Red, Color.Purple, Color.Yellow,
            Color.Green };

        /// Creates a new instance of Trap */
        public Trap()
        {
            if (sRandom == null)
                sRandom = new Random();

            // only gravity affects this object.
            Acceleration = new Vector3d(0, GRAVITY, 0);

            mColor = sColors[sRandom.Next(sColors.Length)];
        }

        /// <summary>
        /// The highest (low on screen) value that the trap can get before it
        /// should be deleted.
        /// </summary>
        public int FinalY
        {
            get { return mFinalY; }
            set { mFinalY = value; }
        }

        public void SetDeleteMeFlag()
        {
            delete = true;
        }

        public bool ContainsPoint(Vector3d pt)
        {
            Vector3d dist = Position - pt;

            // formula for oval
            dist.X /= width;
            dist.Y /= height;

            if (dist.Magnitude <= 1)
                return true;
            else
                return false;
        }

        public override void Draw()
        {
            Display.FillRect((int)Position.X - width / 2, (int)Position.Y - height / 2, width, height, Color);

            if (Position.Y > mFinalY && Velocity.Y > 0)
                delete = true;
            else
                delete = false;
        }

        public override bool DeleteMe
        {
            get
            {
                return delete;
            }
        }

        public Color Color
        {
            get { return mColor; }
            set { mColor = value; }
        }

        const int NUMPARTICLES = 20;
        const double particleSpeed = 100;

        protected override List<GameObject> DeleteObjectsInternal()
        {
            List<GameObject> retval = new List<GameObject>();
            Vector3d totalVelocity = new Vector3d(0, 0, 0);

            for (int i = 0; i < NUMPARTICLES; i++)
            {
                Particle p = new Particle(Color);

                p.Position = Position;

                p.Velocity.X = sRandom.NextDouble() * 2 - 1;
                p.Velocity.Y = sRandom.NextDouble() * 2 - 1;

                p.Velocity = p.Velocity.Normalize() * (sRandom.NextDouble() * particleSpeed);

                totalVelocity = totalVelocity + p.Velocity;
                retval.Add(p);
            }

            // now apply conservation of momentum, by giving a small portion
            // of the excess momentum to each particle
            Vector3d give = totalVelocity * (-1.0 / NUMPARTICLES);

            for (int i = 0; i < NUMPARTICLES; i++)
            {
                Particle p = (Particle)retval[i];

                p.Velocity = p.Velocity + Velocity + give;
            }


            return retval;
        }

    }
}