using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Platform;
using AgateLib.InputLib.Legacy;
using AgateLib.ApplicationModels;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.Platform.WinForms;

namespace ShootTheTraps
{
	class App : Scene
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			using (var setup = new AgateSetupWinForms(args))
			{
				setup.DesiredDisplayWindowResolution = new Size(800, 600);
				setup.AssetLocations.Path = "";

				setup.InitializeAgateLib();

				SceneStack.Start(new App());
			}
		}

		// Game declarations
		//int mDelay = 10; // time in milliseconds to wait
		int mDisplayedScore = 0;
		double mGameOverTime = 0;
		double mLevelTime = 0;
		double mBonusTime = 0;
		int mDisplayedMultiplier = 1;

		ShootTraps mGame;

		// graphics declaration
		IFont mFont;
		Surface mBackground;

		protected override void OnSceneStart()
		{
			mFont = AgateLib.DefaultAssets.Fonts.AgateSans;
			mFont.Size = 14;

			mBackground = new Surface("Resources/background.png");

			Input.Unhandled.MouseDown += Mouse_MouseDown;
			Input.Unhandled.MouseMove += Mouse_MouseMove;
			Input.Unhandled.KeyDown += Keyboard_KeyDown;

			NewGame();
		}

		#region --- Introduction ---

		bool mShowIntro = true;
		bool mShowHelpText = true;

		readonly string mIntroduction = @"{1}Shoot the Traps{2}

Your mission: Destroy as many filthy traps as you can.
These are traps: {0}
They are ugly.

You have a cannon, which shoots where you point it.
{4}Point and click to shoot.{3} The traps have no chance to survive 
against your mighty cannon.

{4}Right-click to release the traps.{3}

Each trap you destroy will net you 50 points.
The white ones are especially hideous, worthy of 100 points.
If you destroy multiple traps at once you receive bonus points!

{4}You must score a certain number of points to pass each level.{3}

Good luck. You'll need it.

Click to start.";

		string[] mIntroLines;

		void DrawIntro()
		{
			if (mIntroLines == null)
			{
				mIntroLines = mIntroduction.Split('\n');
			}

			IFont font = mFont;

			int largestWidth = 0;

			for (int i = 0; i < mIntroLines.Length; i++)
			{
				largestWidth = Math.Max(largestWidth, font.MeasureString(mIntroLines[i]).Width);
			}

			// center introduction text
			Point textPt = new Point((Display.Coordinates.Width - largestWidth) / 2, 20);
			Rectangle boxArea = new Rectangle(
				textPt.X - 10,
				textPt.Y - 10,
				largestWidth + 20,
				(mIntroLines.Length + 1) * font.FontHeight + 20);

			Display.FillRect(boxArea, Color.FromArgb(196, Color.Black));

			for (int i = 0; i < mIntroLines.Length; i++)
			{
				font.DrawText(textPt.X, textPt.Y, mIntroLines[i],
					Trap.Image,
					LayoutCacheAlterFont.Scale(2, 2),
					LayoutCacheAlterFont.Scale(1, 1),
					LayoutCacheAlterFont.Color(Color.White),
					LayoutCacheAlterFont.Color(Color.Yellow));

				textPt.Y += font.FontHeight;
			}
		}

		#endregion

		void Keyboard_KeyDown(object sender, AgateInputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Escape)
			{
				SceneFinished = true;
			}
			if (e.KeyCode == KeyCode.NumPadPlus)
			{
				mGame.SkipToNextLevel();
			}
		}

		public override void Update(double time_ms)
		{
			if (mShowIntro)
				return;

			mGame.Update(Display.DeltaTime);
			UpdateDisplay();
		}
		public override void Draw()
		{
			Display.Clear(Color.LightBlue);
			mBackground.Draw(Display.Coordinates);

			if (mShowIntro)
			{
				DrawIntro();
				return;
			}

			mGame.Draw();
			DrawInformation();
		}

		void Mouse_MouseMove(object sender, AgateInputEventArgs e)
		{
			mGame.MouseMove(e.MousePosition.X, e.MousePosition.Y);
		}
		void Mouse_MouseDown(object sender, AgateInputEventArgs e)
		{
			if (mShowIntro)
			{
				mShowIntro = false;
				NewGame();
				return;
			}

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
			if ((e.MouseButton & MouseButton.Primary) != 0)
			{
				mGame.FireBullet(e.MousePosition.X, e.MousePosition.Y);
			}
			// right click
			if ((e.MouseButton & MouseButton.Secondary) != 0)
			{
				// make sure that the score is done updating.
				if (mGame.Score == mDisplayedScore)
					mGame.FireTraps();

				if (mGame.FiredTraps)
					mShowHelpText = false;

			}
		}

		private void NewGame()
		{
			mGame = new ShootTraps(Display.Coordinates.Width, Display.Coordinates.Height - 50);
			mDisplayedScore = 0;
			mGameOverTime = 0;
			mDisplayedMultiplier = 1;

			mLevelTime = Timing.TotalMilliseconds;

			if (mLevelTime == 0)
				mLevelTime = 1;
		}
		private double GameOverTime
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
			int displayIncrement = 2;
			int scoreDiff = mGame.Score - mDisplayedScore;

			if (scoreDiff > 1000)
				displayIncrement = 100;
			else if (scoreDiff > 200)
				displayIncrement = 10;


			if (Math.Abs(scoreDiff) < displayIncrement)
				mDisplayedScore = mGame.Score;

			if (mGame.Score > mDisplayedScore) mDisplayedScore += displayIncrement;
			if (mGame.Score < mDisplayedScore) mDisplayedScore -= displayIncrement;

		}

		const double mLevelTextTotalTime = 10000;

		private void DrawInformation()
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
				DrawMultiplierText();
			}
			else if (mGame.GameOver && mDisplayedScore == mGame.Score)
			{
				DrawGameOverText();
			}
			else if ((mGame.CanAdvanceLevel && (mGame.Score == mDisplayedScore || mGame.EndOfLevelBonus))
				|| mLevelTime != 0)
			{
				DrawBetweenLevelText();
			}
		}

		private void DrawMultiplierText()
		{
			if (mDisplayedMultiplier == mGame.ScoreMultiplier)
				return;

			int textY = 160 + mFont.FontHeight * 4;

			CenterText(mFont, textY, "Multiplier: " + mGame.ScoreMultiplier.ToString(), Color.White, Color.Black);

			if (mBonusTime == 0)
				mDisplayedMultiplier = mGame.ScoreMultiplier;
		}
		private void DrawBetweenLevelText()
		{
			if (mLevelTime == 0)
				mLevelTime = Timing.TotalMilliseconds;

			if (mGame.Level == 0)
				mLevelTime -= mLevelTextTotalTime / 2;

			if (Timing.TotalMilliseconds - mLevelTime < mLevelTextTotalTime / 2)
				DrawLevelEndText();
			else
			{
				if (mGame.CanAdvanceLevel)
					mGame.NextLevel();

				DrawLevelBeginText();
			}
		}
		private void DrawLevelBeginText()
		{
			mFont.Size = 48;

			int textHeight = mFont.FontHeight * 3;
			int textY = 160;

			Display.FillRect(
				new Rectangle(0, textY, Display.Coordinates.Width, textHeight),
				Color.FromArgb(128, Color.Black));

			// back the border color for the text oscillate between red and black
			int r = (int)(255 * Math.Abs(Math.Sin(12 * (Timing.TotalMilliseconds - mLevelTime) / mLevelTextTotalTime)));
			var borderColor = Color.FromArgb(r, 0, 0);

			CenterText(mFont, textY, "Level " + mGame.Level, Color.White, borderColor);

			mFont.Size = 24;
			CenterText(mFont, textY + mFont.FontHeight * 2, mGame.LevelMessage, Color.White, borderColor);

			if (Timing.TotalMilliseconds - mLevelTime > mLevelTextTotalTime)
				mLevelTime = 0;
		}
		private void DrawLevelEndText()
		{
			mFont.Size = 48;

			int textHeight = mFont.FontHeight * 3;
			int textY = 160;

			Display.FillRect(
				new Rectangle(0, textY, Display.Coordinates.Width, textHeight),
				Color.FromArgb(128, Color.Black));

			// back the border color for the text oscillate between red and black
			int b = (int)(255 * Math.Abs(Math.Sin(12 * (Timing.TotalMilliseconds - mLevelTime) / mLevelTextTotalTime)));
			var borderColor = Color.FromArgb(0, 0, b);

			CenterText(mFont, textY, "End of Level " + mGame.Level, Color.White, borderColor);

			mFont.Size = 24;
			CenterText(mFont, textY + mFont.FontHeight * 2,
				"BONUS for remaining pulls: " + mGame.BonusPoints.ToString(),
				Color.White, borderColor);

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

			mFont.Size = (int)(14 * scale);

			CenterText(mFont, (int)(200 + fontHeight - scale * fontHeight / 2.0),
				"GAME OVER", Color.White, Color.Black);

			mFont.Size = 21;

			if (ContinueYet)
				CenterText(mFont, 240 + (int)(mFont.FontHeight), "Click to restart", Color.White, Color.Black);
		}
		private void DrawBonusText()
		{
			int textY = 160;
			mFont.Size = 28;

			Color bonusColor = Color.White;

			if (Timing.TotalMilliseconds % 500 < 250)
				bonusColor = Color.Yellow;

			Display.FillRect(
				new Rectangle(0, textY, Display.Coordinates.Width, mFont.FontHeight * 4),
				Color.FromArgb(128, Color.Black));

			CenterText(mFont, textY, "HIT " + mGame.TrapsHit + " TRAPS", Color.White, Color.Black);
			textY += mFont.FontHeight;

			CenterText(mFont, textY, "BONUS: " + mGame.BonusPoints, bonusColor, Color.Black);

			if (Timing.TotalMilliseconds - mBonusTime > 2000)
			{
				mBonusTime = 0;
				mGame.ClearBonus();
			}
		}
		private int DrawBottomStatus()
		{
			mFont.Color = Color.White;
			mFont.Size = 14;

			int fontHeight = mFont.FontHeight;
			int textBoxHeight = mFont.FontHeight * 2;

			Point textStart = new Point(10, Display.Coordinates.Height - textBoxHeight);

			if (mShowHelpText && mLevelTime == 0)
			{
				mFont.DrawText(textStart.X, textStart.Y - fontHeight, "Left-click to shoot... Right-click to release traps.");
			}

			Display.FillRect(new Rectangle(0, textStart.Y, Display.Coordinates.Width, textBoxHeight), Color.Black);

			textStart.X = Display.Coordinates.Width / 4;

			mFont.DrawText(textStart.X, textStart.Y, "Score: " + mDisplayedScore);
			mFont.DrawText(textStart.X, textStart.Y + fontHeight, "Need: " +
				Math.Max(0, mGame.LevelRequirement - mGame.PointsThisLevel + (mGame.Score - mDisplayedScore)));

			if (mGame.ScoreMultiplier != 1)
			{
				mFont.Size = 24;
				CenterText(mFont, textStart.Y, "x " + mGame.ScoreMultiplier.ToString(), Color.White);
			}

			mFont.Size = 14;
			textStart.X = Display.Coordinates.Width * 3 / 4;

			mFont.DrawText(textStart.X, textStart.Y, "Level: " + mGame.Level);
			mFont.DrawText(textStart.X, textStart.Y + fontHeight, "Pulls Left: " + mGame.PullsLeft);
			return fontHeight;
		}

		private void CenterText(IFont font, int y, string text, Color color)
		{
			Size size = font.MeasureString(text);

			int x = (Display.Coordinates.Width - size.Width) / 2;

			font.Color = color;
			font.DrawText(x, y, text);
		}
		private void CenterText(IFont font, int y, string text, Color color, Color borderColor)
		{
			Size size = font.MeasureString(text);

			int x = (Display.Coordinates.Width - size.Width) / 2;

			DrawBorderedText(font, x, y, text, color, borderColor);
		}

		private static void DrawBorderedText(IFont font, int x, int y, string text, Color color, Color borderColor)
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
