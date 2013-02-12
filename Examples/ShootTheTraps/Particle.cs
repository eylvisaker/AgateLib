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
		int mAlpha = 255;
		int mImageIndex;
		const double particleLifeTimeMilliseconds = 3000;

		public static List<Surface> Images { get; private set; }


		static Particle()
		{
			Images = new List<Surface>();
		}

		/// Creates a new instance of Particle */
		public Particle(Color clr, Random rnd)
		{
			Acceleration.Y = Gravity;

			mCreateTime = Timing.TotalMilliseconds;
			mColor = clr;

			mImageIndex = rnd.Next(Images.Count);
		}

		public override void Draw()
		{
			double now = Timing.TotalMilliseconds;
			double elapsed = now - mCreateTime;

			mAlpha = (int)(255 * (1 - elapsed / particleLifeTimeMilliseconds));
			if (mAlpha < 0)
			{
				mAlpha = 0;
				return;
			}

			var image = Images[mImageIndex];

			image.DisplayAlignment = OriginAlignment.Center;
			image.RotationCenter = OriginAlignment.Center;
			image.Color = Color.FromArgb(mAlpha, mColor);
			image.RotationAngle = RotationAngle;

			image.Draw((float)Position.X, (float)Position.Y);
		}

		public override bool DeleteMe
		{
			get
			{
				if (OutsideField)
					return true;

				if (mAlpha <= 0)
					return true;
				else
					return false;
			}
		}
	}

}
