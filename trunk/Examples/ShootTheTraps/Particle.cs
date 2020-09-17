using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace ShootTheTraps
{
    public class Particle : GameObject
    {
        double mCreateTime;
        Color mColor;
        int alpha = 255;

        /// Creates a new instance of Particle */
        public Particle(Color clr)
        {
            Acceleration.Y = GRAVITY;

            mCreateTime = Timing.TotalMilliseconds;
            mColor = clr;
        }

        public override void Draw()
        {
            double now = Timing.TotalMilliseconds;

            alpha = (int)(255 * (1 - (now - mCreateTime) / 1000.0));
            if (alpha < 0)
                alpha = 0;

            Display.DrawRect((int)Position.X, (int)Position.Y, 1, 1, Color.FromArgb(alpha, mColor));
        }

        public override bool DeleteMe
        {
            get
            {
                if (alpha <= 0)
                    return true;
                else
                    return false;
            }
        }
    }

}
