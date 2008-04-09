// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ERY.AgateLib;

namespace ERY.SpriteTester
{
    public partial class frmSpriteTester : Form
    {
        public frmSpriteTester()
        {
            InitializeComponent();

            toolStripStatusLabel1.Text = "";
        }

        AgateSetup mDisplaySetup;
        Sprite mSprite;
        DisplayWindow wind;

        ERY.AgateLib.Point mSpritePosition = new ERY.AgateLib.Point(96, 96);
    
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

            foreach (Sprite.AnimType anim in Enum.GetValues(typeof(Sprite.AnimType)))
            {
                cboAnimationType.Items.Add(anim);
            }

            nudTimePerFrame.Value = 50;
        }


        private bool InitDisplay()
        {
            // setup the display
            mDisplaySetup = new AgateSetup();
            mDisplaySetup.AskUser = true;
            mDisplaySetup.UseAudio = false;
            mDisplaySetup.UseInput = false;

            mDisplaySetup.InitializeDisplay();

            if (mDisplaySetup.Cancel)
                throw new Exception("User pressed cancel");

            Sprite.UseSpriteCache = false;

            // This will create a display "window" that renders to the graphics
            // control on this form
            // It doesn't matter if this goes out of scope, because a reference
            // will be maintained by the Display object.
            wind = new DisplayWindow(pctGraphics);
            

            SetSprite(new Sprite(@"Images\attacke.png", 96, 96));

            //Display.PackAllSurfaces();

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

            chkAnimating.Checked = mSprite.Animating;
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
            mSprite.AnimationStarted += new Sprite.SpriteEventHandler(mSprite_AnimationStarted);
            mSprite.AnimationStopped += new Sprite.SpriteEventHandler(mSprite_AnimationStopped);
            mSprite.PlayDirectionChanged += new Sprite.SpriteEventHandler(mSprite_PlayDirectionChanged);
        }

        void mSprite_PlayDirectionChanged(Sprite sprite)
        {
            chkPlayReverse.Checked = sprite.PlayReverse;
        }
        void mSprite_AnimationStopped(Sprite sprite)
        {
            chkAnimating.Checked = false;
        }
        void mSprite_AnimationStarted(Sprite sprite)
        {
            chkAnimating.Checked = true;
        }

        internal void UpdateDisplay()
        {
            if (this.Visible == false)
                return;

            Display.BeginFrame();
            Display.Clear(ERY.AgateLib.Color.Green);


            // draw the grid
            ERY.AgateLib.Color clr = ERY.AgateLib.Color.FromArgb(0, 164, 0);

            for (int x = 0; x < pctGraphics.Width; x += 16)
                Display.DrawRect(new ERY.AgateLib.Rectangle(0, 0, x, Display.RenderTarget.Height), clr);

            for (int y = 0; y < pctGraphics.Height; y += 16)
                Display.DrawRect(new ERY.AgateLib.Rectangle(0, 0, Display.RenderTarget.Width, y), clr);


            int crossSize = 5;

            Display.DrawRect(new ERY.AgateLib.Rectangle(mSpritePosition, mSprite.DisplaySize), ERY.AgateLib.Color.Red);

            Display.DrawRect(new ERY.AgateLib.Rectangle(mSpritePosition.X - crossSize, mSpritePosition.Y, crossSize * 2, 0), ERY.AgateLib.Color.White);
            Display.DrawRect(new ERY.AgateLib.Rectangle(mSpritePosition.X, mSpritePosition.Y - crossSize, 0, crossSize * 2), ERY.AgateLib.Color.White);

            mSprite.Update();
            mSprite.Draw(mSpritePosition);

            Display.EndFrame();

            toolStripStatusLabel1.Text = Display.FramesPerSecond.ToString();
        }

        private void nudTimePerFrame_ValueChanged(object sender, EventArgs e)
        {
            mSprite.TimePerFrame = (int)nudTimePerFrame.Value;
        }

        private void cboAlignment_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string filename = openFile.FileName;

                SetSprite(new Sprite(filename,
                    int.Parse(txtWidth.Text), int.Parse(txtHeight.Text)));

            }
        }

        private void chkAnimating_CheckedChanged(object sender, EventArgs e)
        {
            mSprite.Animating = chkAnimating.Checked;
        }
        private void chkPlayReverse_CheckedChanged(object sender, EventArgs e)
        {
            mSprite.PlayReverse = chkPlayReverse.Checked;
        }

        private void cboAnimationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            mSprite.AnimationType = (Sprite.AnimType)cboAnimationType.SelectedItem;
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            mSprite.StartAnimation();
        }

        private void cboFrame_SelectedIndexChanged(object sender, EventArgs e)
        {
            mSprite.CurrentFrameIndex = (int)cboFrame.SelectedItem;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            mSprite.SetScale((double)nudScale.Value, (double)nudScale.Value);
        }

        private void nudAngle_ValueChanged(object sender, EventArgs e)
        {
            mSprite.RotationAngleDegrees = (double)nudAngle.Value;
        }

        private void cboRotation_SelectedIndexChanged(object sender, EventArgs e)
        {
            mSprite.RotationCenter = (OriginAlignment)cboRotation.SelectedItem;
        }

        private void frmSpriteTester_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDisplaySetup.Dispose();
        }

    }
}