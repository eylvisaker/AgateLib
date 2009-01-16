using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateLib.Resources;
using AgateLib.Sprites;

namespace PackedSpriteCreator
{
    public partial class SpriteEditor : UserControl
    {
        AgateResourceCollection mResources;
        SpriteResource mCurrentSprite;
        SpriteRenderer drawer = new SpriteRenderer();
        AgateLib.DisplayLib.DisplayWindow wind;
        NewSprite mAgateSprite;

        public SpriteEditor()
        {
            InitializeComponent();
        }

        public AgateResourceCollection Resources
        {
            get { return mResources; }
            set
            {
                mResources = value;

                ResourcesChanged();
            }
        }

        private void ResourcesChanged()
        {
            lstSprites.Items.Clear();

            if (mResources == null)
            {
                lstSprites.Enabled = false;
                return;
            }

            lstSprites.Enabled = true;
            lstSprites.Items.AddRange(mResources.Sprites.ToArray());
            
        }

        bool changingSprite = false;

        private void lstSprites_SelectedIndexChanged(object sender, EventArgs e)
        {
            mCurrentSprite = lstSprites.SelectedItem as SpriteResource;

            if (mCurrentSprite == null)
            {
                propertiesPanel.Visible = false;
                return;
            }

            propertiesPanel.Visible = true;

            try
            {
                changingSprite = true;
                UpdateSprite();

                txtName.Text = mCurrentSprite.Name;
                nudTimePerFrame.Value = (decimal)mCurrentSprite.TimePerFrame;

                FillFrameList(-1);
            }
            finally
            {
                changingSprite = false;
            }
        }

        private void UpdateSprite()
        {
            UpdateSprite(new NewSprite(Resources, mCurrentSprite.Name));
        }
        private void UpdateSprite(NewSprite updatedSprite)
        {
            if (mAgateSprite != null)
                mAgateSprite.Dispose();

            mAgateSprite = updatedSprite ;
            mAgateSprite.IsAnimating = chkAnimating.Checked;

        }

        
        private void FillFrameList(int newSelection)
        {
            lstFrames.Items.Clear();
            lstFrames.Items.AddRange(mCurrentSprite.Frames.ToArray());

            newSelection = Math.Min(newSelection, lstFrames.Items.Count - 1);
            lstFrames.SelectedIndex = newSelection;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (propertiesPanel.Visible == false)
                return;

            drawer.DrawSprite(wind, mAgateSprite); 
        }

        private void SpriteEditor_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            if (Parent == null)
                return;

            wind = new AgateLib.DisplayLib.DisplayWindow(AgateLib.DisplayLib.CreateWindowParams.FromControl(agateRenderTarget1));

        }

        private void txtName_Validating(object sender, CancelEventArgs e)
        {
            if (changingSprite) return;

            if (Resources.Contains(txtName.Text) == false)
                return;

            if (Resources[txtName.Text] == mCurrentSprite)
                return;

            e.Cancel = true;
        }

        private void txtName_Validated(object sender, EventArgs e)
        {
            if (changingSprite) return;

            Resources.Remove(mCurrentSprite);
            
            mCurrentSprite.Name = txtName.Text;

            Resources.Add(mCurrentSprite);

            int index = lstSprites.SelectedIndex;
            lstSprites.Items.RemoveAt(index);
            lstSprites.Items.Insert(index, mCurrentSprite);
            lstSprites.SelectedIndex = index;
        }

        private void nudTimePerFrame_ValueChanged(object sender, EventArgs e)
        {
            if (changingSprite) return;

            mCurrentSprite.TimePerFrame = (double)nudTimePerFrame.Value;
            mAgateSprite.TimePerFrame = (double)nudTimePerFrame.Value;

        }

        private void lstFrames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFrames.SelectedIndex == -1)
            {
                btnDelete.Enabled = false;
                btnMoveDown.Enabled = false;
                btnMoveUp.Enabled = false;
            }
            else
            {
                btnMoveUp.Enabled = true;
                btnMoveDown.Enabled = true;
                btnDelete.Enabled = true;

                if (lstFrames.SelectedIndex == 0)
                {
                    btnMoveUp.Enabled = false;
                }
                if (lstFrames.SelectedIndex == lstFrames.Items.Count-1)
                {
                    btnMoveDown.Enabled = false;
                }
            }

            if (changingSprite)
                return;

            if (chkAnimating.Checked == false)
                mAgateSprite.CurrentFrameIndex = lstFrames.SelectedIndex;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddSpriteFrames frm = new frmAddSpriteFrames();
            frm.Size = AgateLib.WinForms.Interop.Convert(mCurrentSprite.Size);

            if (frm.ShowDialog(this) == DialogResult.OK)
            {

            }
        }
        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            int index = lstFrames.SelectedIndex;

            var frame = mCurrentSprite.Frames[index];
            mCurrentSprite.Frames.RemoveAt(index);
            mCurrentSprite.Frames.Insert(index - 1, frame);

            UpdateSprite();
            FillFrameList(index - 1);

        }
        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            int index = lstFrames.SelectedIndex;

            var frame = mCurrentSprite.Frames[index];
            mCurrentSprite.Frames.RemoveAt(index);
            mCurrentSprite.Frames.Insert(index + 1, frame);

            UpdateSprite();
            FillFrameList(index + 1);

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = lstFrames.SelectedIndex;

            mCurrentSprite.Frames.RemoveAt(index);

            UpdateSprite();
            FillFrameList(index );


        }

        private void chkAnimating_CheckedChanged(object sender, EventArgs e)
        {
            mAgateSprite.IsAnimating = chkAnimating.Checked;
        }
    }
}
