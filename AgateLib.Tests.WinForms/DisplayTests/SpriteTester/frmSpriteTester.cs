// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Sprites;
using AgateLib.Platform.WinForms;
using AgateLib.Utility;
using AgateLib.Platform.WinForms.IO;
using AgateLib.Resources;
using Color = AgateLib.DisplayLib.Color;
using Point = AgateLib.Mathematics.Geometry.Point;
using Rectangle = AgateLib.Mathematics.Geometry.Rectangle;

namespace AgateLib.Tests.DisplayTests.SpriteTester
{
	public partial class frmSpriteTester : Form
	{
		public frmSpriteTester()
		{
			InitializeComponent();
		}

		Sprite mSprite;
		DisplayWindow wind;

		Point mSpritePosition = new Point(96, 96);

		private void frmSpriteTester_Load(object sender, EventArgs e)
		{
			InitDisplay();

			cboAlignment.Items.Clear();

			foreach (OriginAlignment al in Enum.GetValues(typeof(OriginAlignment)))
			{
				cboAlignment.Items.Add(al);
				cboRotation.Items.Add(al);
			}

			cboAlignment.SelectedItem = mSprite.DisplayAlignment;
			cboRotation.SelectedItem = mSprite.RotationCenter;

			cboAnimationType.Items.Clear();

			foreach (SpriteAnimType anim in Enum.GetValues(typeof(SpriteAnimType)))
			{
				cboAnimationType.Items.Add(anim);
			}

			nudTimePerFrame.Value = 50;

			try
			{
				Icon = new Icon(@"../../src/AgateLib.ico");
			}
			catch
			{
			}

		}


		private bool InitDisplay()
		{

			// This will create a display "window" that renders to the graphics
			// control on this form
			// It doesn't matter if this goes out of scope, because a reference
			// will be maintained by the Display object.
			wind = DisplayWindow.CreateFromControl(pctGraphics);

			//srcSurf = new Surface();

			SetSprite(new Sprite("Images/attacke.png", 96, 96));

			Display.PackAllSurfaces();

			return true;

		}

		private void SetSprite(Sprite sprite)
		{
			ClearEvents();

			mSprite = sprite;

			SetEvents();

			cboFrame.Items.Clear();
			for (int i = 0; i < mSprite.Frames.Count; i++)
				cboFrame.Items.Add(i);

			chkAnimating.Checked = mSprite.IsAnimating;
			chkPlayReverse.Checked = mSprite.PlayReverse;
			cboAlignment.SelectedItem = mSprite.DisplayAlignment;
			cboAnimationType.SelectedItem = mSprite.AnimationType;
			nudTimePerFrame.Value = (decimal)mSprite.TimePerFrame;

			double scalex, scaley;

			mSprite.GetScale(out scalex, out scaley);
			nudScale.Value = (decimal)scalex;

			//for (int i = 0; i < mSprite.Frames.Count; i++)
			//{
			//    mSprite.Frames[i].Surface.SaveTo("frame" + i.ToString() + ".png", ImageFileFormat.Png);
			//}

			GC.Collect();
		}

		private void ClearEvents()
		{
			if (mSprite == null)
				return;

			mSprite.AnimationStarted -= mSprite_AnimationStarted;
			mSprite.AnimationStopped -= mSprite_AnimationStopped;
			mSprite.PlayDirectionChanged -= mSprite_PlayDirectionChanged;
		}

		private void SetEvents()
		{
			mSprite.AnimationStarted += new SpriteEventHandler(mSprite_AnimationStarted);
			mSprite.AnimationStopped += new SpriteEventHandler(mSprite_AnimationStopped);
			mSprite.PlayDirectionChanged += new SpriteEventHandler(mSprite_PlayDirectionChanged);
		}

		void mSprite_PlayDirectionChanged(ISprite sprite)
		{
			chkPlayReverse.Checked = sprite.PlayReverse;
		}
		void mSprite_AnimationStopped(ISprite sprite)
		{
			chkAnimating.Checked = false;
		}
		void mSprite_AnimationStarted(ISprite sprite)
		{
			chkAnimating.Checked = true;
		}

		internal void UpdateDisplay()
		{
			if (this.Visible == false)
				return;

			Display.BeginFrame();
			Display.Clear(Color.Green);


			// draw the grid
			Color clr = Color.FromRgb(0, 164, 0);

			for (int x = 0; x < pctGraphics.Width; x += 16)
				Display.Primitives.DrawRect(clr,new Rectangle(0, 0, x, Display.RenderTarget.Height));

			for (int y = 0; y < pctGraphics.Height; y += 16)
				Display.Primitives.DrawRect(clr,new Rectangle(0, 0, Display.RenderTarget.Width, y));


			int crossSize = 5;

			Display.Primitives.DrawRect(Color.Red,new Rectangle(mSpritePosition, mSprite.DisplaySize));

			// draw cross
			Display.Primitives.DrawRect(Color.White,
				new Rectangle(mSpritePosition.X - crossSize, mSpritePosition.Y, crossSize * 2 + 1, 1));
			Display.Primitives.DrawRect(Color.White,
				new Rectangle(mSpritePosition.X, mSpritePosition.Y - crossSize, 1, crossSize * 2 + 1));

			mSprite.Update();
			mSprite.Draw(mSpritePosition);

			//srcSurf.Draw(10, 300);

			Display.EndFrame();

			lblFrameRate.Text = "Frame Rate: " + Display.FramesPerSecond.ToString();
		}

		private void nudTimePerFrame_ValueChanged(object sender, EventArgs e)
		{
			if (mSprite == null)
				return;

			mSprite.TimePerFrame = (int)nudTimePerFrame.Value;
		}

		private void cboAlignment_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (mSprite == null)
				return;
			if (cboAlignment.SelectedItem == null)
				return;


			mSprite.DisplayAlignment = (OriginAlignment)cboAlignment.SelectedItem;
		}

		private void btn64_Click(object sender, EventArgs e)
		{
			txtWidth.Text = "64";
			txtHeight.Text = "64";
		}
		private void btn96_Click(object sender, EventArgs e)
		{
			txtWidth.Text = "96";
			txtHeight.Text = "96";
		}
		private void btn128_Click(object sender, EventArgs e)
		{
			txtWidth.Text = "128";
			txtHeight.Text = "128";
		}

		private void btnLoadSprite_Click(object sender, EventArgs e)
		{
			if (openFile.ShowDialog() != DialogResult.OK)
				return;

			Sprite spTest = null;
			string filename = openFile.FileName;

			try
			{
				spTest = new Sprite(filename,
					int.Parse(txtWidth.Text), int.Parse(txtHeight.Text));
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.GetType().Name);
				System.Diagnostics.Debug.WriteLine(ex.Message);

			}

			if (spTest != null)
			{
				SetSprite(spTest);
			}
			else
			{
				// since loading the sprite from the file failed, try it as a resource file.
				AgateResourceManager resources = new AgateResourceManager(filename);

				AgateApp.Assets = new FileSystemProvider
					(System.IO.Path.GetDirectoryName(filename));

				if (resources.Sprites.ToArray().Length == 1)
				{
					var sprites = resources.Sprites.ToArray();

					Sprite sp = (Sprite)resources.Display.GetSprite(resources.Sprites.First());

					SetSprite(sp);
				}
				else
				{
					frmChooseSprite frm = new frmChooseSprite();

					if (frm.ShowDialog(this, resources) == DialogResult.OK)
					{
						Sprite sp = (Sprite)resources.Display.GetSprite(frm.SelectedSprite);

						SetSprite(sp);
					}
				}
			}
		}

		private void chkAnimating_CheckedChanged(object sender, EventArgs e)
		{
			if (mSprite == null)
				return;

			mSprite.IsAnimating = chkAnimating.Checked;
		}
		private void chkPlayReverse_CheckedChanged(object sender, EventArgs e)
		{
			if (mSprite == null)
				return;

			mSprite.PlayReverse = chkPlayReverse.Checked;
		}

		private void cboAnimationType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (mSprite == null)
				return;
			if (cboAnimationType.SelectedItem == null)
				return;

			mSprite.AnimationType = (SpriteAnimType)cboAnimationType.SelectedItem;
		}

		private void btnRestart_Click(object sender, EventArgs e)
		{
			if (mSprite == null)
				return;

			mSprite.StartAnimation();
		}

		private void cboFrame_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (mSprite == null)
				return;

			mSprite.CurrentFrameIndex = (int)cboFrame.SelectedItem;
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			if (mSprite == null)
				return;

			mSprite.SetScale((double)nudScale.Value, (double)nudScale.Value);
		}

		private void nudAngle_ValueChanged(object sender, EventArgs e)
		{
			if (mSprite == null)
				return;

			mSprite.RotationAngleDegrees = (double)nudAngle.Value;
		}

		private void cboRotation_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (mSprite == null)
				return;

			mSprite.RotationCenter = (OriginAlignment)cboRotation.SelectedItem;
		}

		private void chkVSync_CheckedChanged(object sender, EventArgs e)
		{
			Display.RenderState.WaitForVerticalBlank = chkVSync.Checked;
		}

	}
}