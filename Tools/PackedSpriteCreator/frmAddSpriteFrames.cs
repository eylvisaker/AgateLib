using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PackedSpriteCreator
{
    public partial class frmAddSpriteFrames : Form
    {
        Bitmap image;
        
        public frmAddSpriteFrames()
        {
            InitializeComponent();
        }

        public Size SpriteSize { get; set; }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog(this) == DialogResult.Cancel)
                return;

            txtFilename.Text = openFile.FileName;
        }

        private void frmAddSpriteFrames_Load(object sender, EventArgs e)
        {
            if (openFile.ShowDialog(this) == DialogResult.Cancel)
            {
                this.DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            txtFilename.Text = openFile.FileName;
        }

        private void txtFilename_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(txtFilename.Text) == false)
            {
                detailsPanel.Enabled = false;
                pctInitialPreview.Image = null;
                return;
            }


            try
            {
                image = new Bitmap(txtFilename.Text);
            }
            catch
            {
                detailsPanel.Enabled = false;
                pctInitialPreview.Image = null;
                return;
            }

            pctInitialPreview.Image = image;
            pctInitialPreview.Size = image.Size;
            
        }

        private void chkTransparent_CheckedChanged(object sender, EventArgs e)
        {
            pnlTransparentColor.Visible = UseTransparentColor;
        }

        private void pctInitialPreview_MouseClick(object sender, MouseEventArgs e)
        {
            var data = image.LockBits(new Rectangle(e.X, e.Y, 1, 1),
                 System.Drawing.Imaging.ImageLockMode.ReadOnly,
                 System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int[] dest = new int[1];

            System.Runtime.InteropServices.Marshal.Copy(data.Scan0, dest, 0, 1);

            TransparentColor = Color.FromArgb(dest[0]);

            image.UnlockBits(data);
        }

        public Color TransparentColor
        {
            get { return pnlTransparentColor.BackColor; }
            set { pnlTransparentColor.BackColor = value; }
        }
        public bool UseTransparentColor
        {
            get { return chkTransparent.Checked; }
            set { chkTransparent.Checked = value; }
        }
    }
}
