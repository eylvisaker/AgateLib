using AgateLib.Platform.WindowsForms;
using AgateLib.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PackedSpriteCreator
{
    public partial class frmSpriteCreator : Form
    {
        public frmSpriteCreator()
        {
            InitializeComponent();

            Icon = AgateLib.Platform.WindowsForms.WinForms.FormUtil.AgateLibIcon;
            spriteEditor1.Enabled = false;
        }

        private void newResourceFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spriteEditor1.Resources = new AgateLib.Resources.Legacy.AgateResourceCollection();
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

            spriteEditor1.Resources = AgateLib.Resources.Legacy.AgateResourceLoader.LoadResources(openDialog.FileName);
            spriteEditor1.Enabled = true;

            System.IO.Directory.SetCurrentDirectory(
                System.IO.Path.GetDirectoryName(openDialog.FileName));

			AgateLib.IO.FileProvider.SurfaceAssets = new FileSystemProvider(".");
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
