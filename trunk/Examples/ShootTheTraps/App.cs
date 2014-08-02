using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Platform.WindowsForms.Resources;
using AgateLib.Platform;
using AgateLib.Platform.WindowsForms.ApplicationModels;
using AgateLib.InputLib.Legacy;

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
			new PassiveModel(args).Run(() => new App().Run(args));
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
		FontSurface mFont;
		FontSurface mLargeFont;
		Surface mBackground;

		protected override void Initialize()
		{
			mFont = BuiltinResources.AgateSans14;
			mLargeFont = BuiltinResources.AgateSans24;
			mBackground = new Surface("Resources/background.png");

			Mouse.MouseDown += new InputEventHandler(Mouse_MouseDown);
			Mouse.MouseMove += Mouse_MouseMove;
			Keyboard.KeyDown += Keyboard_KeyDown;

			NewGame();
		}

		#region --- Introduction ---

		bool mShowIntro = true;
		bool mShowHelpText = true;

		readonly string mIntroduction = @"{1}Shoot the Traps{2}

You mission: Destroy as many filthy traps as you can.
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

			FontSurface font = mFont;

			int largestWidth = 0;

			for (int i = 0; i < mIntroLines.Length; i++)
			{
				largestWidth = Math.Max(largestWidth, font.MeasureString(mIntroLines[i]).Width);
			}

			// center introduction text
			Point textPt = new Point((Display.RenderTarget.Width - largestWidth) / 2, 20);
			Rectangle boxArea = new Rectangle(
				textPt.X - 10, 
				textPt.Y - 10,
				largestWidth + 20,
				(mIntroLines.Length +1) * font.FontHeight + 20);

			Display.FillRect(boxArea, Color.FromArgb(196, Color.Black));

			for (int i = 0; i < mIntroLines.Length; i++)
			{
				font.DrawText(textPt.X, textPt.Y, mIntroLines[i],
					Trap.Image,
					AlterFont.Scale(2, 2),
					AlterFont.Scale(1, 1),
					AlterFont.Color(Color.White),
					AlterFont.Color(Color.Yellow));

				textPt.Y += font.FontHeight;
			}
		}

		#endregion

		void Keyboard_KeyDown(InputEventArgs e)
		{
			if (e.KeyCode == KeyCode.NumPadPlus)
			{
				mGame.SkipToNextLevel();
			}
		}

		protected override string ApplicationTitle
		{
			get { return "Shoot the Traps"; }
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
			if (mShowIntro)
				return;

			mGame.Update(Display.DeltaTime);
			UpdateDisplay();
		}
		protected override void Render()
		{
			Display.Clear(Color.LightBlue);
			mBackground.Draw(new Rectangle(Point.Empty, WindowSize));

			if (mShowIntro)
			{
				DrawIntro();
				return;
			}

			mGame.Draw();
			DrawInformation();
		}

		void Mouse_MouseMove(InputEventArgs e)
		{
			mGame.MouseMove(e.MousePosition.X, e.MousePosition.Y);
		}
		void Mouse_MouseDown(InputEventArgs e)
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
			if ((e.MouseButtons & MouseButton.Primary) != 0)
			{
				mGame.FireBullet(e.MousePosition.X, e.MousePosition.Y);
			}
			// right click
			if ((e.MouseButtons & MouseButton.Secondary) != 0)
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
			mGame = new ShootTraps(Display.RenderTarget.Width, Display.RenderTarget.Height - 50);
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
			mLargeFont.SetScale(2, 2);

			int textHeight = mLargeFont.FontHeight * 3;
			int textY = 160;

			Display.FillRect(
				new Rectangle(0, textY, Display.RenderTarget.Width, textHeight),
				Color.FromArgb(128, Color.Black));

			// back the border color for the text oscillate between red and black
			int r = (int)(255 * Math.Abs(Math.Sin(12 * (Timing.TotalMilliseconds - mLevelTime) / mLevelTextTotalTime)));
			var borderColor = Color.FromArgb(r, 0, 0);

			CenterText(mLargeFont, textY, "Level " + mGame.Level, Color.White, borderColor);

			mLargeFont.SetScale(1, 1);
			CenterText(mLargeFont, textY + mLargeFont.FontHeight * 2, mGame.LevelMessage, Color.White, borderColor);

			if (Timing.TotalMilliseconds - mLevelTime > mLevelTextTotalTime)
				mLevelTime = 0;
		}
		private void DrawLevelEndText()
		{
			mLargeFont.SetScale(2, 2);

			int textHeight = mLargeFont.FontHeight * 3;
			int textY = 160;

			Display.FillRect(
				new Rectangle(0, textY, Display.RenderTarget.Width, textHeight),
				Color.FromArgb(128, Color.Black));

			// back the border color for the text oscillate between red and black
			int b = (int)(255 * Math.Abs(Math.Sin(12 * (Timing.TotalMilliseconds - mLevelTime) / mLevelTextTotalTime)));
			var borderColor = Color.FromArgb(0, 0, b);

			CenterText(mLargeFont, textY, "End of Level " + mGame.Level, Color.White, borderColor);

			mLargeFont.SetScale(1, 1);
			CenterText(mLargeFont, textY + mLargeFont.FontHeight * 2,
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

			mFont.SetScale(scale, scale);

			CenterText(mFont, (int)(200 + fontHeight - scale * fontHeight / 2.0),
				"GAME OVER", Color.White, Color.Black);

			mFont.SetScale(1.5, 1.5);

			if (ContinueYet)
				CenterText(mFont, 240 + (int)(fontHeight * mFont.ScaleHeight), "Click to restart", Color.White, Color.Black);
		}
		private void DrawBonusText()
		{
			int textY = 160;
			mFont.SetScale(2, 2);

			Color bonusColor = Color.White;

			if (Timing.TotalMilliseconds % 500 < 250)
				bonusColor = Color.Yellow;

			Display.FillRect(
				new Rectangle(0, textY, Display.RenderTarget.Width, mFont.FontHeight * 4),
				Color.FromArgb(128, Color.Black));

			CenterText(mFont, textY, "HIT " + mGame.TrapsHit + " TRAPS", Color.White, Color.Black);
			textY += (int)(mFont.FontHeight * mFont.ScaleHeight);

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
			mFont.SetScale(1, 1);

			int fontHeight = mFont.FontHeight;
			int textBoxHeight = mFont.FontHeight * 2;

			Point textStart = new Point(10, Display.RenderTarget.Height - textBoxHeight);

			if (mShowHelpText && mLevelTime == 0)
			{
				mFont.DrawText(textStart.X, textStart.Y - fontHeight, "Left-click to shoot... Right-click to release traps.");
			}

			Display.FillRect(new Rectangle(0, textStart.Y, Display.RenderTarget.Width, textBoxHeight), Color.Black);

			textStart.X = Display.RenderTarget.Width / 4;

			mFont.DrawText(textStart.X, textStart.Y, "Score: " + mDisplayedScore);
			mFont.DrawText(textStart.X, textStart.Y + fontHeight, "Need: " +
				Math.Max(0, mGame.LevelRequirement - mGame.PointsThisLevel + (mGame.Score - mDisplayedScore)));

			if (mGame.ScoreMultiplier != 1)
				CenterText(mLargeFont, textStart.Y, "x " + mGame.ScoreMultiplier.ToString(), Color.White);

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
