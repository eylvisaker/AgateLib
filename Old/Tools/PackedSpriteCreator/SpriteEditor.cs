using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateLib.DisplayLib.Sprites;
using AgateLib.Platform.WinForms;
using AgateLib.Resources;
using AgateLib.Resources.DataModel;

namespace PackedSpriteCreator
{
	public partial class SpriteEditor : UserControl
	{
		ResourceDataModel resources;
		AgateResourceManager resourceManager;

		string spriteName;
		SpriteResource currentSprite;
		SpriteRenderer drawer = new SpriteRenderer();
		AgateLib.DisplayLib.DisplayWindow wind;
		Sprite agateSprite;

		public SpriteEditor()
		{
			InitializeComponent();
		}

		public ResourceDataModel Resources
		{
			get { return resources; }
			set
			{
				resources = value;

				OnResourcesChanged();
			}
		}

		public AgateResourceManager ResourceManager => resourceManager;

		private void OnResourcesChanged()
		{
			if (resourceManager != null)
			{
				resourceManager.Dispose();
			}

			lstSprites.Items.Clear();

			if (resources != null)
			{
				resourceManager = new AgateResourceManager(resources);
			}
			else
			{
				lstSprites.Enabled = false;
				return;
			}

			lstSprites.Enabled = true;
			lstSprites.Items.AddRange(resourceManager.Sprites.ToArray());

		}

		bool changingSprite = false;

		private void lstSprites_SelectedIndexChanged(object sender, EventArgs e)
		{
			currentSprite = lstSprites.SelectedItem as SpriteResource;

			if (currentSprite == null)
			{
				propertiesPanel.Visible = false;
				return;
			}

			propertiesPanel.Visible = true;

			try
			{
				changingSprite = true;
				UpdateSprite();

				txtName.Text = spriteName;
				nudTimePerFrame.Value = (decimal)currentSprite.Animation.FrameTime;

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
				UpdateSprite((Sprite)ResourceManager.Display.GetSprite(spriteName));
			}
			catch (Exception e)
			{
				MessageBox.Show(this, string.Format(
					"Failed to load sprite {0}." + Environment.NewLine + "{1}", spriteName,
					e.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void UpdateSprite(Sprite updatedSprite)
		{
			if (agateSprite != null)
				agateSprite.Dispose();

			agateSprite = updatedSprite;
			agateSprite.IsAnimating = chkAnimating.Checked;
		}

		private void FillFrameList(int newSelection)
		{
			FillFrameList(new int[] { newSelection });
		}
		private void FillFrameList(IEnumerable<int> selections)
		{
			lstFrames.Items.Clear();
			lstFrames.Items.AddRange(currentSprite.Frames.ToArray());

			lstFrames.SelectedIndices.Clear();
			foreach (var index in selections)
				lstFrames.SelectedIndices.Add(index);

			lstFrames_SelectedIndexChanged(this, EventArgs.Empty);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if (propertiesPanel.Visible == false)
				return;

			drawer.DrawSprite(wind.FrameBuffer, agateSprite);
		}

		private void SpriteEditor_Load(object sender, EventArgs e)
		{
			if (DesignMode)
				return;
			if (Parent == null)
				return;

			wind = new AgateLib.DisplayLib.DisplayWindow(AgateLib.DisplayLib.CreateWindowParams.FromControl(agateRenderTarget1,
			new AgateLib.Geometry.CoordinateSystems.NativeCoordinates()));

		}

		private void txtName_Validating(object sender, CancelEventArgs e)
		{
			if (changingSprite) return;

			if (resources.Sprites.ContainsKey(txtName.Text) == false)
				return;

			if (resources.Sprites[txtName.Text] == currentSprite)
				return;

			e.Cancel = true;
		}

		private void txtName_Validated(object sender, EventArgs e)
		{
			if (changingSprite) return;

			resources.Sprites.Remove(spriteName);

			spriteName = txtName.Text;

			resources.Sprites.Add(spriteName, currentSprite);

			int index = lstSprites.SelectedIndex;
			lstSprites.Items.RemoveAt(index);
			lstSprites.Items.Insert(index, currentSprite);
			lstSprites.SelectedIndex = index;
		}

		private void nudTimePerFrame_ValueChanged(object sender, EventArgs e)
		{
			if (changingSprite) return;

			currentSprite.Animation.FrameTime = (int)nudTimePerFrame.Value;
			agateSprite.TimePerFrame = (double)nudTimePerFrame.Value;

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

			if (agateSprite != null)
			{
				if (chkAnimating.Checked == false)
					agateSprite.CurrentFrameIndex = lstFrames.SelectedIndex;
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			frmAddSpriteFrames frm = new frmAddSpriteFrames();
			frm.SpriteSize = currentSprite.Size.ToDrawingSize();

			if (frm.ShowDialog(this) == DialogResult.OK)
			{

			}
		}
		private void btnMoveUp_Click(object sender, EventArgs e)
		{
			List<int> selections = new List<int>();

			foreach (int index in lstFrames.SelectedIndices)
			{
				var frame = currentSprite.Frames[index];
				currentSprite.Frames.RemoveAt(index);
				currentSprite.Frames.Insert(index - 1, frame);

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
				var frame = currentSprite.Frames[index];
				currentSprite.Frames.RemoveAt(index);
				currentSprite.Frames.Insert(index + 1, frame);

				selections.Add(index + 1);
			}

			UpdateSprite();
			FillFrameList(selections);

		}
		private void btnDelete_Click(object sender, EventArgs e)
		{
			var framesToDelete = new List<SpriteFrameResource>();

			foreach (int index in lstFrames.SelectedIndices)
			{
				framesToDelete.Add(currentSprite.Frames[index]);
			}

			foreach (SpriteFrameResource frame in framesToDelete)
			{
				currentSprite.Frames.Remove(frame);
			}

			UpdateSprite();
			FillFrameList(-1);
		}


		private void chkAnimating_CheckedChanged(object sender, EventArgs e)
		{
			if (agateSprite == null)
				return;

			agateSprite.IsAnimating = chkAnimating.Checked;
		}

		private void btnNewSprite_Click(object sender, EventArgs e)
		{
			frmNewSprite frm = new frmNewSprite();

			if (frm.ShowDialog(this, ResourceManager) == DialogResult.Cancel)
				return;

			frmAddSpriteFrames frmAdd = new frmAddSpriteFrames();
			frmAdd.SpriteSize = frm.SpriteFrameSize;

			if (frmAdd.ShowDialog(this) == DialogResult.Cancel)
				return;

			SpriteResource res = new SpriteResource();

			resources.Sprites.Add(frm.SpriteName, res);

			OnResourcesChanged();
		}
	}
}
