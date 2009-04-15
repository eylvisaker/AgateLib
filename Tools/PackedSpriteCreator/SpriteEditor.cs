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
		Sprite mAgateSprite;

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

				OnResourcesChanged();
			}
		}

		private void OnResourcesChanged()
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
			try
			{
				UpdateSprite(new Sprite(Resources, mCurrentSprite.Name));
			}
			catch (Exception e)
			{
				MessageBox.Show(this, string.Format(
					"Failed to load sprite {0}." + Environment.NewLine + "{1}", mCurrentSprite.Name,
					e.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void UpdateSprite(Sprite updatedSprite)
		{
			if (mAgateSprite != null)
				mAgateSprite.Dispose();

			mAgateSprite = updatedSprite;
			mAgateSprite.IsAnimating = chkAnimating.Checked;
		}

		private void FillFrameList(int newSelection)
		{
			FillFrameList(new int[] { newSelection });
		}
		private void FillFrameList(IEnumerable<int> selections)
		{
			lstFrames.Items.Clear();
			lstFrames.Items.AddRange(mCurrentSprite.ChildElements.ToArray());

			lstFrames.SelectedIndices.Clear();
			foreach (var index in selections)
				lstFrames.SelectedIndices.Add(index);

			lstFrames_SelectedIndexChanged(this, EventArgs.Empty);
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

				if (lstFrames.SelectedIndices.Contains(0))
				{
					btnMoveUp.Enabled = false;
				}
				if (lstFrames.SelectedIndices.Contains(lstFrames.Items.Count - 1))
				{
					btnMoveDown.Enabled = false;
				}
				if (lstFrames.SelectedIndices.Count == 0)
					btnDelete.Enabled = false;
			}

			if (changingSprite)
				return;

			if (mAgateSprite != null)
			{
				if (chkAnimating.Checked == false)
					mAgateSprite.CurrentFrameIndex = lstFrames.SelectedIndex;
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			frmAddSpriteFrames frm = new frmAddSpriteFrames();
			frm.SpriteSize = AgateLib.WinForms.Interop.Convert(mCurrentSprite.Size);

			if (frm.ShowDialog(this) == DialogResult.OK)
			{

			}
		}
		private void btnMoveUp_Click(object sender, EventArgs e)
		{
			List<int> selections = new List<int>();

			foreach (int index in lstFrames.SelectedIndices)
			{
				var frame = mCurrentSprite.ChildElements[index];
				mCurrentSprite.ChildElements.RemoveAt(index);
				mCurrentSprite.ChildElements.Insert(index - 1, frame);

				selections.Add(index - 1);
			}

			UpdateSprite();
			FillFrameList(selections);

		}
		private void btnMoveDown_Click(object sender, EventArgs e)
		{
			List<int> selections = new List<int>();

			foreach (int index in lstFrames.SelectedIndices)
			{
				var frame = mCurrentSprite.ChildElements[index];
				mCurrentSprite.ChildElements.RemoveAt(index);
				mCurrentSprite.ChildElements.Insert(index + 1, frame);

				selections.Add(index + 1);
			}

			UpdateSprite();
			FillFrameList(selections);

		}
		private void btnDelete_Click(object sender, EventArgs e)
		{
			var framesToDelete = new List<SpriteResource.SpriteSubResource>();

			foreach (int index in lstFrames.SelectedIndices)
			{
				framesToDelete.Add(mCurrentSprite.ChildElements[index]);
			}

			foreach (SpriteResource.SpriteFrameResource frame in framesToDelete)
			{
				mCurrentSprite.ChildElements.Remove(frame);
			}

			UpdateSprite();
			FillFrameList(-1);
		}


		private void chkAnimating_CheckedChanged(object sender, EventArgs e)
		{
			if (mAgateSprite == null)
				return;

			mAgateSprite.IsAnimating = chkAnimating.Checked;
		}

		private void btnNewSprite_Click(object sender, EventArgs e)
		{
			frmNewSprite frm = new frmNewSprite();

			if (frm.ShowDialog(this, Resources) == DialogResult.Cancel)
				return;

			frmAddSpriteFrames frmAdd = new frmAddSpriteFrames();
			frmAdd.SpriteSize = frm.SpriteFrameSize;

			if (frmAdd.ShowDialog(this) == DialogResult.Cancel)
				return;

			SpriteResource res = new SpriteResource(frm.SpriteName);



			Resources.Add(res);

			OnResourcesChanged();
		}
	}
}
