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
        List<ImportImageInfo> images = new List<ImportImageInfo>();
        bool updating = false;

        public frmAddSpriteFrames()
        {
            InitializeComponent();

            pctInitialPreview.Location = new Point();
        }

        public Size SpriteSize { get; set; }

        ImportImageInfo CurrentImage
        {
            get
            {
                if (lstImages.SelectedIndices.Count != 1)
                    return null;

                return images[lstImages.SelectedIndex];
            }
        }
        
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog(this) == DialogResult.Cancel)
                return;

            AddFiles(openFile.FileNames);
        }

        private void AddFiles(IEnumerable<string> files)
        {
            foreach (string filename in files)
            {
                ImportImageInfo info = new ImportImageInfo();

                info.FullPath = filename;
                info.Image = new Bitmap(filename);

                images.Add(info);
                lstImages.Items.Add(info);
            }
        }
        private void frmAddSpriteFrames_Load(object sender, EventArgs e)
        {
            if (openFile.ShowDialog(this) == DialogResult.Cancel)
            {
                this.DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            AddFiles(openFile.FileNames);
        }

        private void chkTransparent_CheckedChanged(object sender, EventArgs e)
        {
            if (updating)
                return;

            CurrentImage.UseColorKey = chkTransparent.Checked;
            UpdateControls();
        }

        private void pctInitialPreview_MouseClick(object sender, MouseEventArgs e)
        {
            if (CurrentImage == null)
                return;

            var data = CurrentImage.Image.LockBits(new Rectangle(e.X, e.Y, 1, 1),
                 System.Drawing.Imaging.ImageLockMode.ReadOnly,
                 System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int[] dest = new int[1];

            System.Runtime.InteropServices.Marshal.Copy(data.Scan0, dest, 0, 1);

            CurrentImage.ColorKey = Color.FromArgb(dest[0]);
            CurrentImage.Image.UnlockBits(data);

            UpdateControls();
        }

        private void lstImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentImage == null)
            {
                splitContainer1.Panel2.Enabled = false;  
                pctInitialPreview.Image = null;
                return;
            }

            splitContainer1.Panel2.Enabled = true;

            pctInitialPreview.Image = CurrentImage.Image;
            pctInitialPreview.Size = CurrentImage.Image.Size;

            UpdateControls();
        }

        private void UpdateControls()
        {

        }

    }
}
