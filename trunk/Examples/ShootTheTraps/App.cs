﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace ShootTheTraps
{
	class App : AgateGame
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			new App().Run(args);
		}

		// Game declarations
		//int mDelay = 10; // time in milliseconds to wait
		int mDisplayedScore = 0;
		double mGameOverTime = 0;
		double mLevelTime = 0;
		double mBonusTime = 0;

		ShootTraps mGame;

		// graphics declaration
		FontSurface mFont;
		FontSurface mLargeFont;
		Surface mBackground;

		protected override void Initialize()
		{
			mFont = FontSurface.AgateSans14;
			mLargeFont = FontSurface.AgateSans24;
			mBackground = new Surface("Resources/background.png");

			Mouse.MouseDown += new InputEventHandler(Mouse_MouseDown);
			Mouse.MouseMove += Mouse_MouseMove;

			NewGame();
		}

		protected override void AdjustAppInitParameters(ref AppInitParameters initParams)
		{
			initParams.ShowSplashScreen = false;
		}
		protected override Size WindowSize
		{
			get { return new Size(1280, 720); }
		}
		protected override void Update(double time_ms)
		{
			mGame.Update(Display.DeltaTime);
			UpdateDisplay();
		}
		protected override void Render()
		{
			Display.Clear(Color.LightBlue);
			mBackground.Draw(new Rectangle(Point.Empty, WindowSize));

			mGame.Draw();
			DrawInformation();
		}


		void Mouse_MouseMove(InputEventArgs e)
		{
			mGame.MouseMove(e.MousePosition.X, e.MousePosition.Y);
		}
		void Mouse_MouseDown(InputEventArgs e)
		{
			if (ContinueYet)
			{
				NewGame();
				return;
			}
			if (mLevelTime != 0)
				return;
			if (mBonusTime != 0)
				return;

			// left click
			if ((e.MouseButtons & Mouse.MouseButtons.Primary) != 0)
			{
				mGame.FireBullet(e.MousePosition.X, e.MousePosition.Y);
			}
			// right click
			if ((e.MouseButtons & Mouse.MouseButtons.Secondary) != 0)
			{
				// make sure that the score is done updating.
				if (mGame.Score == mDisplayedScore)
					mGame.FireTraps();
			}
		}

		private void NewGame()
		{
			mGame = new ShootTraps(Display.RenderTarget.Width, Display.RenderTarget.Height - 50);
			mDisplayedScore = 0;
			mGameOverTime = 0;

			mLevelTime = Timing.TotalMilliseconds;

			if (mLevelTime == 0)
				mLevelTime = 1;
		}
		public double GameOverTime
		{
			get
			{
				return mGameOverTime;
			}
		}

		private bool ContinueYet
		{
			get
			{
				if (mGame.GameOver == false)
					return false;

				if (Timing.TotalMilliseconds - mGameOverTime > 5000)
					return true;
				else
					return false;
			}
		}

		public void UpdateDisplay()
		{
			const int displayIncrement = 2;

			if (Math.Abs(mGame.Score - mDisplayedScore) < displayIncrement)
				mDisplayedScore = mGame.Score;

			if (mGame.Score > mDisplayedScore)
				mDisplayedScore += displayIncrement;
			else if (mGame.Score < mDisplayedScore)
				mDisplayedScore -= displayIncrement;

		}
		public void DrawInformation()
		{
			int fontHeight = mFont.FontHeight;
			
			DrawBottomStatus();

			if (mGame.Score != mDisplayedScore && mGame.BonusAdded && mGame.TrapsHit > 1)
			{
				if (mBonusTime == 0)
					mBonusTime = Timing.TotalMilliseconds;
			}

			if (mBonusTime != 0)
			{
				DrawBonusText();
			}
			else if (mGame.GameOver && mDisplayedScore == mGame.Score)
			{
				DrawGameOverText();
			}
			else if ((mGame.CanAdvanceLevel && mGame.Score == mDisplayedScore) || mLevelTime != 0)
			{
				BeginNextLevel();
			}
		}

		private void BeginNextLevel()
		{
			const double mLevelTextTotalTime = 2500;

			if (mGame.CanAdvanceLevel)
			{
				mGame.NextLevel();
				mLevelTime = Timing.TotalMilliseconds;
			}

			mLargeFont.SetScale(2, 2);

			int fontHeight = mLargeFont.FontHeight * 2;
			int textY = 160;

			Display.FillRect(
				new Rectangle(0, textY, Display.RenderTarget.Width, fontHeight), 
				Color.FromArgb(128, Color.Black));

			// back the border color for the text oscillate between red and black
			int r = (int)(255 * Math.Abs(Math.Sin(12 * (Timing.TotalMilliseconds - mLevelTime) / mLevelTextTotalTime)));

			CenterText(mLargeFont, textY, "Level " + mGame.Level, Color.White, Color.FromArgb(r, 0, 0));

			if (Timing.TotalMilliseconds - mLevelTime > mLevelTextTotalTime)
				mLevelTime = 0;
		}

		private void DrawGameOverText()
		{
			int fontHeight = mFont.FontHeight;
			
			if (mGameOverTime == 0)
				mGameOverTime = Timing.TotalMilliseconds;

			double deltaTime = Math.Min((Timing.TotalMilliseconds - mGameOverTime) / 3000, 1);

			double extraScaleFactor = -3 * (1.1 - Math.Pow(deltaTime, 2));

			double scale = 3 + extraScaleFactor;

			mFont.SetScale(scale, scale);

			CenterText(mFont, (int)(200 + fontHeight - scale * fontHeight / 2.0),
				"GAME OVER", Color.White, Color.Black);

			mFont.SetScale(1.5, 1.5);

			if (ContinueYet)
				CenterText(mFont, 240 + (int)(fontHeight * mFont.ScaleHeight), "Click to restart", Color.White, Color.Black);
		}

		private void DrawBonusText()
		{
			mFont.SetScale(2, 2);

			CenterText(mFont, 160, "HIT " + mGame.TrapsHit + " TRAPS", Color.White, Color.Black);

			Color bonusColor = Color.White;

			if (Timing.TotalMilliseconds % 500 < 250)
				bonusColor = Color.Yellow;

			CenterText(mFont, 160 + (int)(mFont.FontHeight * mFont.ScaleHeight), "BONUS: " + mGame.BonusPoints, bonusColor, Color.Black);

			if (Timing.TotalMilliseconds - mBonusTime > 2000)
			{
				mBonusTime = 0;
			}
		}

		private int DrawBottomStatus()
		{
			mFont.Color = Color.White;
			mFont.SetScale(1, 1);

			int fontHeight = mFont.FontHeight;
			int textBoxHeight = mFont.FontHeight * 2;

			Point textStart = new Point(10, Display.RenderTarget.Height - textBoxHeight);

			Display.FillRect(new Rectangle(0, textStart.Y, Display.RenderTarget.Width, textBoxHeight), Color.Black);

			textStart.X = Display.RenderTarget.Width / 4;

			mFont.DrawText(textStart.X, textStart.Y, "Score: " + mDisplayedScore);
			mFont.DrawText(textStart.X, textStart.Y + fontHeight, "Need: " +
				Math.Max(0, mGame.LevelRequirement - mGame.PointsThisLevel + (mGame.Score - mDisplayedScore)));

			textStart.X = Display.RenderTarget.Width * 3 / 4;

			mFont.DrawText(textStart.X, textStart.Y, "Level: " + mGame.Level);
			mFont.DrawText(textStart.X, textStart.Y + fontHeight, "Pulls Left: " + mGame.PullsLeft);
			return fontHeight;
		}

		private void CenterText(FontSurface font, int y, string text, Color color)
		{
			Size size = font.MeasureString(text);

			int x = (Display.RenderTarget.Width - size.Width) / 2;

			font.Color = color;
			font.DrawText(x, y, text);
		}
		private void CenterText(FontSurface font, int y, string text, Color color, Color borderColor)
		{
			Size size = font.MeasureString(text);

			int x = (Display.RenderTarget.Width - size.Width) / 2;

			DrawBorderedText(font, x, y, text, color, borderColor);


		}

		private static void DrawBorderedText(FontSurface font, int x, int y, string text, Color color, Color borderColor)
		{
			font.Color = borderColor;
			font.DrawText(x + 1, y, text);
			font.DrawText(x, y + 1, text);
			font.DrawText(x - 1, y, text);
			font.DrawText(x, y - 1, text);

			font.Color = color;
			font.DrawText(x, y, text);
		}
	}

}
