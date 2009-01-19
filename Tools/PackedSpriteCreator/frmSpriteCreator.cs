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

            Icon = AgateLib.WinForms.FormUtil.AgateLibIcon;
        }

        private void newResourceFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spriteEditor1.Resources = new AgateLib.Resources.AgateResourceCollection();
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

            spriteEditor1.Resources = AgateLib.Resources.AgateResourceLoader.LoadResources(openDialog.FileName);

            System.IO.Directory.SetCurrentDirectory(
                System.IO.Path.GetDirectoryName(openDialog.FileName));

            AgateLib.Utility.AgateFileProvider.ImageProvider.PathList.Clear();
            AgateLib.Utility.AgateFileProvider.ImageProvider.AddPath(".");
        }

        private void closeResourceFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spriteEditor1.Resources = null;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
