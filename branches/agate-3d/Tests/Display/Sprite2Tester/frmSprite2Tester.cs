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
using AgateLib.Resources;
using AgateLib.Sprites;
using AgateLib.DisplayLib;

namespace ERY.Sprite2Tester
{
    public partial class frmSprite2Tester : Form
    {
        public frmSprite2Tester()
        {
            InitializeComponent();
        }

        ISprite mSprite;
        DisplayWindow wind;

        AgateLib.Geometry.Point mSpritePosition = new AgateLib.Geometry.Point(96, 96);

        private void frmSprite2Tester_Load(object sender, EventArgs e)
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
            wind = new DisplayWindow(CreateWindowParams.FromControl(pctGraphics));

            //srcSurf = new Surface();

            SetSprite(new AgateLib.Sprites.Old.Sprite("attacke.png", 96, 96));

            Display.PackAllSurfaces();

            return true;

        }

        private void SetSprite(ISprite sprite)
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
            Display.Clear(AgateLib.Geometry.Color.Green);


            // draw the grid
            AgateLib.Geometry.Color clr = AgateLib.Geometry.Color.FromArgb(0, 164, 0);

            for (int x = 0; x < pctGraphics.Width; x += 16)
                Display.DrawRect(new AgateLib.Geometry.Rectangle(0, 0, x, Display.RenderTarget.Height), clr);

            for (int y = 0; y < pctGraphics.Height; y += 16)
                Display.DrawRect(new AgateLib.Geometry.Rectangle(0, 0, Display.RenderTarget.Width, y), clr);


            int crossSize = 5;

            Display.DrawRect(new AgateLib.Geometry.Rectangle(mSpritePosition, mSprite.DisplaySize), AgateLib.Geometry.Color.Red);

            // draw cross
            Display.DrawRect(new AgateLib.Geometry.Rectangle(mSpritePosition.X - crossSize, mSpritePosition.Y, crossSize * 2 + 1, 1),
                AgateLib.Geometry.Color.White);
            Display.DrawRect(new AgateLib.Geometry.Rectangle(mSpritePosition.X, mSpritePosition.Y - crossSize, 1, crossSize * 2 + 1),
                AgateLib.Geometry.Color.White);

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

        private void btnLoadSprite_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                //string filename = openFile.FileName;

                ////AgateResourceCollection resources = ResourceLoader.LoadResources(filename);

                ////if (resources.Sprites.ToArray().Length == 1)
                //{
                ////	AgateLib.Utility.FileManager.ImagePath.Clear();
                ////	AgateLib.Utility.FileManager.ImagePath.Add(System.IO.Path.GetDirectoryName(filename));

                //    //ISprite sp = resources.CreateSprite(resources.CurrentLanguage.Sprites.ToArray()[0].Name);

                ////	SetSprite(sp);
                //}
                //else
                //    throw new NotImplementedException();

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

        private void nudScale_ValueChanged(object sender, EventArgs e)
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

		}

        private void chkVSync_CheckedChanged(object sender, EventArgs e)
        {
            Display.VSync = chkVSync.Checked;
		}

    }
}
