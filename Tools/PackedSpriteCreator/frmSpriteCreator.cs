using AgateLib.Platform.WinForms;
using AgateLib.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateLib.Platform.WinForms.IO;
using AgateLib.Resources;
using AgateLib.Platform.WinForms.Controls;

namespace PackedSpriteCreator
{
	public partial class frmSpriteCreator : Form
	{
		public frmSpriteCreator()
		{
			InitializeComponent();

			Icon = FormUtil.AgateLibIcon;
			spriteEditor1.Enabled = false;
		}

		private void newResourceFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			spriteEditor1.Resources = new AgateLib.Resources.DataModel.ResourceDataModel();
		}

		private void openResourceFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFiles();
		}
		private void btnOpen_Click(object sender, EventArgs e)
		{
			OpenFiles();
		}
		private void OpenFiles()
		{
			if (openDialog.ShowDialog() == DialogResult.Cancel)
				return;

			System.IO.Directory.SetCurrentDirectory(
				System.IO.Path.GetDirectoryName(openDialog.FileName));

			var provider = new FileSystemProvider(".");

			spriteEditor1.Resources = new ResourceDataLoader(provider).Load(openDialog.FileName);
			spriteEditor1.Enabled = true;

			AgateLib.IO.Assets.Images = new FileSystemProvider(".");
		}

		private void closeResourceFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			spriteEditor1.Resources = null;
			spriteEditor1.Enabled = false;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
			spriteEditor1.Enabled = false;
		}
	}
}
