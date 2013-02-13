using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace ShootTheTraps
{
	public class ShootTraps : IDisposable
	{
		List<GameObject> mGameObjects;
		Random mRandom;

		readonly int mGroundY;
		readonly int mTrapVYScale;
		readonly int mTrapMaxVX;
		readonly int mTrapMinVX;

		readonly int mGunX;
		readonly int mGunY;

		const int BulletSpeed = 1000;
		const int MaxBullets = 4;

		int[] mLaunchX;

		Surface mGun = new Surface("Resources/barrel.png");
		Point mMousePos;
		double mGunAngle;

		/// <summary>
		/// Creates a new instance of ShootTraps 
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public ShootTraps(int width, int height)
		{
			if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException();

			mGameObjects = new List<GameObject>();
			mRandom = new Random();

			mGroundY = height - 10;

			// choose a y velocity (pixels / second) that will get the 
			// traps nearly to the top of the screen
			mTrapVYScale = (int)Math.Sqrt(2 * GameObject.Gravity * (height - 10));

			double timeInAir = mTrapVYScale * 2 / GameObject.Gravity;

			// choose a minimum x velocity so that the trap will make it at least 1/3
			// the way across the field.
			mTrapMaxVX = mTrapVYScale / 2;
			mTrapMinVX = (int)(width / (3.0 * timeInAir));

			mGunX = width / 2;
			mGunY = mGroundY + 15;

			mLaunchX = new int[4];

			// area of screen where traps are launched from.
			int launchScale = width / 2 - 50;

			mLaunchX[0] = width / 2 - launchScale;
			mLaunchX[1] = width / 2 - launchScale + 40;

			mLaunchX[2] = width - mLaunchX[1];
			mLaunchX[3] = width - mLaunchX[0];

			GameObject.FieldArea = Rectangle.FromLTRB(-10, -20, width + 10, mGunY);

			Bullet.Image = new Surface("Resources/bullet.png");
			Trap.Image = new Surface("Resources/enemy.png");
			Particle.Images.Add(new Surface("Resources/splatter-1.png"));
			Particle.Images.Add(new Surface("Resources/splatter-2.png"));
			Particle.Images.Add(new Surface("Resources/splatter-3.png"));
			Particle.Images.Add(new Surface("Resources/splatter-4.png"));
		}

		public void Dispose()
		{
			mGun.Dispose();
			Bullet.Image.Dispose();
			Trap.Image.Dispose();
			Particle.Images.ForEach(x => x.Dispose());
		}

		private void AddScore(int points)
		{
			Score += points;
			PointsThisLevel += points;
		}
		public void NextLevel()
		{
			if (CanAdvanceLevel == false)
				return;

			CanAdvanceLevel = false;

			Level++;
			PullsLeft = Math.Min(12 + 2 * Level, 21);

			PointsThisLevel = 0;
		}

		public void Update(double milliseconds)
		{
			UpdateAllObjects(milliseconds);
			CheckForCollisions();

			CalcBonus();

			DeleteObjects();
		}
		private void UpdateAllObjects(double milliseconds)
		{
			mGameObjects.ForEach(x => x.Update(milliseconds));
		}

		public void Draw()
		{
			foreach (GameObject obj in mGameObjects)
			{
				obj.Draw();
			}

			mGun.DisplayAlignment = OriginAlignment.BottomCenter;
			mGun.RotationCenter = OriginAlignment.BottomCenter;
			mGun.RotationAngle = mGunAngle;

			mGun.Draw(mGunX, mGunY);
		}

		private void DeleteObjects()
		{
			for (int i = 0; i < mGameObjects.Count; i++)
			{
				if (mGameObjects[i].DeleteMe)
				{
					List<GameObject> extras = mGameObjects[i].CreateDebris();

					mGameObjects.RemoveAt(i--);

					if (extras != null)
						mGameObjects.AddRange(extras);
				}
			}
		}

		private void CalcBonus()
		{
			if (TrapCount != 0)
				return;
			if (BonusAdded)
				return;

			BonusAdded = true;
			BonusPoints = 0;

			if (TrapsHit > 1)
				BonusPoints = 250 * (TrapsHit - 1);

			AddScore(BonusPoints);

			// check for game over conditions
			if (PullsLeft == 0)
			{
				if (PointsThisLevel >= LevelRequirement)
					CanAdvanceLevel = true;
				else
					GameOver = true;
			}
		}

		private void CheckForCollisions()
		{
			foreach (GameObject obj in mGameObjects)
			{
				if (obj is Bullet == false)
					continue;

				Bullet bullet = (Bullet)obj;

				Rectangle bulletRect = bullet.BoundingRect;

				foreach (GameObject t in mGameObjects)
				{
					if (t is Trap == false) continue;
					if (t.DeleteMe) continue;

					Trap trap = (Trap)t;

					if (RectsIntersect(bulletRect, bullet.RotationAngle, trap.BoundingRect, trap.RotationAngle))
					{
						trap.SetDeleteMeFlag();
						trap.ShouldCreateDebris = true;

						TrapsHit++;

						int score = 50;

						if (trap.Color == Color.White)
							score = 100;

						AddScore(score);
					}
				}
			}
		}

		// store these in the object instance so that we
		// don't generate a whole lot of garbage when checking for
		// intersections.
		Vector2[] vertsA = new Vector2[4];
		Vector2[] vertsB = new Vector2[4];

		private bool RectsIntersect(Rectangle rectA, double angleA, Rectangle rectB, double angleB)
		{
			// produce vertices for each rectangle
			ComputeVertices(rectA, angleA, vertsA);
			ComputeVertices(rectB, angleB, vertsB);

			// now we need to do the separating axis test for each edge in each square.
			if (FindSeparatingAxis(vertsA, vertsB))
				return false;

			if (FindSeparatingAxis(vertsB, vertsA))
				return false;

			return true;
		}

		/// <summary>
		/// Checks to see if any of the lines in the first set of vectors groups
		/// all the points in the second set of vectors entirely into one side.
		/// This algorithm can be used to determine if two convex polygons intersect.
		/// </summary>
		/// <param name="va"></param>
		/// <param name="vb"></param>
		/// <returns></returns>
		private bool FindSeparatingAxis(Vector2[] va, Vector2[] vb)
		{
			for (int i = 0; i < va.Length; i++)
			{
				int next = i + 1;
				if (next == va.Length) next = 0;

				int nextnext = next + 1;
				if (nextnext == va.Length) nextnext = 0;

				Vector2 edge = va[next] - va[i];
				
				bool separating = true;

				// first check to see which side of the axis the points in 
				// va are on, stored in the inSide variable.
				Vector2 indiff = va[nextnext] - va[i];
				var indot = indiff.DotProduct(edge);
				int inSide = Math.Sign(indot);
				int lastSide = 0;

				for (int j = 0; j < vb.Length; j++)
				{
					Vector2 diff = vb[j] - va[i];
					
					var dot = diff.DotProduct(edge);
					var side = Math.Sign(dot);

					// this means points in vb are on the same side 
					// of the edge as points in va. Thus, it is not 
					// a separating axis.
					if (side == inSide)
					{
						separating = false;
						break;
					}

					if (lastSide == 0)
						lastSide = side;
					else if (lastSide != side)
					{
						// if we fail here, it means the axis goes right through
						// the polygon defined in vb, so this is not a separating
						// axis.
						separating = false;
						break;
					}
				}

				if (separating)
					return true;
			}

			return false;
		}

		private void ComputeVertices(Rectangle rect, double angle, Vector2[] verts)
		{
			Vector2 center = RectCenter(rect);

			double cos = Math.Cos(angle);
			double sin = Math.Sin(angle);

			// translate so the center of the rect is at the origin, apply the rotation
			// and translate back.
			verts[0] = center + RotatePoint(cos, sin, new Vector2(rect.Left, rect.Top) - center);
			verts[1] = center + RotatePoint(cos, sin, new Vector2(rect.Right, rect.Top) - center);
			verts[2] = center + RotatePoint(cos, sin, new Vector2(rect.Right, rect.Bottom) - center);
			verts[3] = center + RotatePoint(cos, sin, new Vector2(rect.Left, rect.Bottom) - center);
		}

		private Vector2 RectCenter(Rectangle rect)
		{
			return new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
		}

		private Vector2 RotatePoint(double cos, double sin, Vector2 point)
		{
			Vector2 retval = new Vector2(
				(cos * point.X - sin * point.Y),
				(sin * point.X + cos * point.Y));

			return retval;
		}

		public void MouseMove(int mouseX, int mouseY)
		{
			mMousePos = new Point(mouseX, mouseY);

			mGunAngle = Math.Atan2(
				mGunY - mouseY, mouseX - mGunX) - Math.PI / 2;
		}

		public void FireBullet(int towardsX, int towardsY)
		{
			if (BulletCount >= MaxBullets)
				return;
			if (GameOver)
				return;

			Vector3d direction = new Vector3d(towardsX - mGunX, towardsY - mGunY, 0).Normalize();

			if (direction.Y > 0)
			{
				direction.Y = 0;

				if (direction.X == 0)
					direction.X = 1;

				direction = direction.Normalize();
			}

			Bullet ar = new Bullet();

			ar.RotationAngle = mGunAngle;
			ar.Position.X = mGunX;
			ar.Position.Y = mGunY;

			ar.Velocity.X = direction.X * BulletSpeed;
			ar.Velocity.Y = direction.Y * BulletSpeed;

			mGameObjects.Add(ar);
		}
		public void FireTraps()
		{
			if (GameOver)
				return;
			if (PullsLeft <= 0)
				return;

			// Don't fire traps if there are any traps left on the screen, or 
			// if the player has fired some bullets on the screen. No cheating that way!
			if (mGameObjects.Count > 0)
				return;

			PullsLeft--;
			TrapsHit = 0;
			BonusPoints = 0;
			BonusAdded = false;

			int maxTraps = Level / 2 + 2;
			if (maxTraps > 8)
				maxTraps = 8;

			int r = mRandom.Next(maxTraps) + 1;

			if (PullsLeft == 0)
				r = maxTraps;

			for (int i = 0; i < r; i++)
			{
				Trap t = new Trap();

				int xpos = mRandom.Next(mLaunchX.Length);

				t.Position.X = mLaunchX[xpos];
				t.Position.Y = mGroundY;

				t.FinalY = mGroundY + 50;

				t.Velocity.X = mTrapMinVX + mRandom.Next(mTrapMaxVX - mTrapMinVX);
				t.Velocity.Y = -mTrapVYScale * (1 - mRandom.Next(50) / 200.0);

				t.RotationalVelocity = (mRandom.NextDouble() - 0.5) * 40;

				if (t.Position.X > mGunX)
					t.Velocity.X *= -1;

				mGameObjects.Add(t);
			}

		}

		public int BulletCount
		{
			get { return mGameObjects.Count(x => x is Bullet); }
		}
		public int TrapCount
		{
			get { return mGameObjects.Count(x => x is Trap); }
		}

		public bool CanAdvanceLevel { get; private set; }
		public bool GameOver { get; private set; }
		public int Level { get; private set; }
		public int LevelRequirement
		{
			get { return 1000 * Level; }
		}
		public int PullsLeft { get; private set; }
		public int TrapsHit { get; private set; }
		public bool BonusAdded { get; private set; }
		public int BonusPoints { get; private set; }
		public int PointsThisLevel { get; set; }
		public int Score { get; set; }
	}

}