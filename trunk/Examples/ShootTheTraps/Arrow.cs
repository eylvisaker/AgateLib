using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace ShootTheTraps
{
    public class Arrow : GameObject
    {
        const int LENGTH = 10;
        static int sXMax;
        const int YMIN = -20;
        const int YMAX = 1000;
        const int XMIN = -20;

        /// <summary>
        /// Allows the game engine to set how far to the right is too far for an arrow to travel.
        /// </summary>
        public static int XMax
        {
            get { return sXMax; }
            set { sXMax = value; }
        }
        
        public Arrow()
        {
            //acceleration.my = gravity;
        }

        /// <summary>
        /// Draws a short line for the arrow.
        /// </summary>
        public override void Draw()
        {
            Vector3d direction = Velocity.Normalize();

            Display.DrawLine(
                (int)Position.X, (int)Position.Y,
                (int)(Position.X + direction.X * LENGTH), (int)(Position.Y + direction.Y * LENGTH), Color.Black);
        }

        /// <summary>
        /// Delete the arrow if it has gone outside the bounds of the screen.
        /// </summary>
        public override bool DeleteMe
        {
            get
            {
                if (Position.X < XMIN || Position.X > sXMax)
                    return true;
                else if (Position.Y < YMIN || Position.Y > YMAX)
                    return true;

                return false;
            }
        }

    }

}
