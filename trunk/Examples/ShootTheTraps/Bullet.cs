using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace ShootTheTraps
{
	public class Bullet : GameObject
	{
		public static Surface Image { get; set; }

		public Bullet()
		{
		}

		/// <summary>
		/// Draws the image for the bullet.
		/// </summary>
		public override void Draw()
		{
			Vector3d direction = Velocity.Normalize();

			Image.DisplayAlignment = OriginAlignment.Center;
			Image.RotationCenter = OriginAlignment.Center;
			Image.RotationAngle = RotationAngle;

			Image.Draw((float)Position.X, (float)Position.Y);

		}

		/// <summary>
		/// Delete the bullet if it has gone outside the bounds of the screen.
		/// </summary>
		public override bool DeleteMe
		{
			get
			{
				if (OutsideField)
					return true;

				return false;
			}
		}
	}
}
