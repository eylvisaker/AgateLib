using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace ShootTheTraps
{

	public class ShootTraps
	{
		List<GameObject> mGameObjects;
		Random mRandom;

		readonly int yvalue;
		readonly int halfx;
		readonly int yvelScale;
		readonly int xvelScale;

		readonly int xpos;
		readonly int ypos;

		const int ARROWSPEED = 800;
		const int MAXARROWS = 4;

		private int mScore;

		int[] mXVals;

		int mLevel = 0;
		int mPullsLeft = 0;
		int mTrapsHit = 0;
		int mBonusPoints = 0;
		int mPointsThisLevel = 0;


		bool mGameOver = false;
		bool mBonusDone = false;
		bool mCanAdvanceLevel = true;

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

			halfx = width / 2;
			yvalue = height - 10;
			yvelScale = (int)Math.Sqrt(2 * Trap.GRAVITY * (height - 10));
			xvelScale = (int)width / 2;

			xpos = halfx;
			ypos = yvalue - 5;


			mXVals = new int[4];

			mXVals[0] = 10;
			mXVals[1] = 40;

			mXVals[2] = 2 * halfx - mXVals[1];
			mXVals[3] = 2 * halfx - mXVals[0];

			//this.width = width;
			//this.height = height;

			Arrow.XMax = width + 10;

		}

		private void AddScore(int points)
		{
			mScore += points;
			mPointsThisLevel += points;
		}
		public void NextLevel()
		{
			if (mCanAdvanceLevel == false)
				return;

			mCanAdvanceLevel = false;

			mLevel++;
			mPullsLeft = Math.Min(12 + 2 * mLevel, 21);

			mPointsThisLevel = 0;
		}

		public void Update(double milliseconds)
		{
			UpdateAllObjects(milliseconds);
			CheckForCollisions();

			CalcBonus();

			DeleteObjects();
		}
		public void Draw()
		{
			foreach (GameObject obj in mGameObjects)
			{
				obj.Draw();
			}

		}

		private void DeleteObjects()
		{
			for (int i = 0; i < mGameObjects.Count; i++)
			{
				if (mGameObjects[i].DeleteMe)
				{
					List<GameObject> extras = mGameObjects[i].DeleteObjects();

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
			if (mBonusDone)
				return;

			mBonusDone = true;
			mBonusPoints = 0;

			if (mTrapsHit > 1)
				mBonusPoints = 250 * (mTrapsHit - 1);

			AddScore(mBonusPoints);

			// check for game over conditions
			if (mPullsLeft == 0)
			{
				if (mPointsThisLevel >= LevelRequirement)
					mCanAdvanceLevel = true;
				else
					mGameOver = true;
			}
		}

		private void CheckForCollisions()
		{

			// check for collisions
			foreach (GameObject obj in mGameObjects)
			{
				if (!(obj is Arrow))
					continue;

				Arrow ar = (Arrow)obj;

				foreach (GameObject t in mGameObjects)
				{
					if (!(t is Trap))
						continue;

					Trap trap = (Trap)t;

					if (trap.ContainsPoint(ar.Position) && trap.DeleteMe == false)
					{
						trap.SetDeleteMeFlag();
						trap.mDoDeleteObjects = true;

						mTrapsHit++;

						int score = 50;

						if (trap.Color == Color.White)
							score = 100;

						AddScore(score);
					}
				}
			}
		}

		private void UpdateAllObjects(double milliseconds)
		{
			foreach (GameObject obj in mGameObjects)
			{
				obj.Update(milliseconds);
			}
		}



		public void FireArrow(int towardsX, int towardsY)
		{
			if (ArrowCount >= MAXARROWS)
				return;
			if (mGameOver)
				return;

			Vector3d direction = new Vector3d(towardsX - xpos, towardsY - ypos, 0).Normalize();

			if (direction.Y > 0)
			{
				direction.Y = 0;

				if (direction.X == 0)
					direction.X = 1;

				direction = direction.Normalize();
			}

			Arrow ar = new Arrow();

			ar.Position.X = xpos;
			ar.Position.Y = ypos;

			ar.Velocity.X = direction.X * ARROWSPEED;
			ar.Velocity.Y = direction.Y * ARROWSPEED;

			mGameObjects.Add(ar);
		}

		public void FireTraps()
		{
			if (mGameObjects.Count > 0)
				return;
			if (mGameOver)
				return;
			if (mPullsLeft <= 0)
				return;

			mPullsLeft--;
			mBonusDone = false;
			mTrapsHit = 0;
			mBonusPoints = 0;

			int maxTraps = mLevel / 2 + 2;
			if (maxTraps > 8)
				maxTraps = 8;

			int r = mRandom.Next(maxTraps) + 1;

			if (mPullsLeft == 0)
				r = maxTraps;

			for (int i = 0; i < r; i++)
			{
				Trap t = new Trap();

				int xpos = mRandom.Next(mXVals.Length);

				t.Position.X = mXVals[xpos];
				t.Position.Y = yvalue;

				t.FinalY = yvalue + 50;

				t.Velocity.X = 10 + mRandom.Next(xvelScale - 10);
				t.Velocity.Y = -yvelScale * (1 - mRandom.Next(50) / 200.0);

				if (t.Position.X > halfx)
					t.Velocity.X *= -1;

				mGameObjects.Add(t);
			}

		}

		public int ArrowCount
		{
			get
			{
				int result = 0;

				foreach (GameObject obj in mGameObjects)
				{
					if (obj is Arrow)
						result++;
				}

				return result;
			}
		}

		public int TrapCount
		{
			get
			{
				return mGameObjects.Count - ArrowCount;
			}
		}

		public bool CanAdvanceLevel
		{
			get
			{
				return mCanAdvanceLevel;
			}
		}
		public bool GameOver
		{
			get
			{
				return mGameOver;
			}
		}
		public int Level
		{
			get
			{
				return mLevel;
			}
		}
		public int PullsLeft
		{
			get
			{
				return mPullsLeft;
			}
		}
		public int TrapsHit
		{
			get
			{
				return mTrapsHit;
			}
		}
		public bool AddedBonus
		{
			get
			{
				return mBonusDone;
			}
		}
		public int BonusPoints
		{
			get
			{
				return mBonusPoints;
			}
		}
		public int LevelRequirement
		{
			get
			{
				return 1000 * mLevel;
			}
		}
		public int PointsThisLevel
		{
			get { return mPointsThisLevel; }
			set { mPointsThisLevel = value; }
		}

		public int Score
		{
			get { return mScore; }
			set { mScore = value; }
		}
	}

}