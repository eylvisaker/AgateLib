using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using AgateLib.BitmapFont;

namespace FontCreator
{
    public partial class frmFontCreator : Form
    {
        int mCurrentPage;

        public frmFontCreator()
        {
            InitializeComponent();

            Icon = AgateLib.WinForms.FormUtil.AgateLibIcon;
            CurrentPage = 1;
        }

        AgateLib.DisplayLib.FontSurface AgateFont
        {
            get { return this.createFont1.FontBuilder.Font; }
        }
        int CurrentPage
        {
            get { return mCurrentPage; }
            set
            {
                pnlCreateFont.Visible = false;
                pnlEditGlyphs.Visible = false;
                pnlSaveFont.Visible = false;

                Panel pnl = null;

                switch (value)
                {
                    case 1:
                        pnl = pnlCreateFont;
                        break;

                    case 2:
                        string tempImage = Path.GetTempFileName() + ".png";

                        ((BitmapFontImpl)AgateFont.Impl).Surface.SaveTo(tempImage);

                        editGlyphs1.SetFont(tempImage, ((BitmapFontImpl)AgateFont.Impl).FontMetrics);

                        pnl = pnlEditGlyphs;
                        break;

                    case 3:
                        pnl = pnlSaveFont;
                        break;

                    default:
                        throw new InvalidOperationException("Wrong page number!");
                }

                pnl.Dock = DockStyle.Fill;
                pnl.Visible = true;

                mCurrentPage = value;
            }
        }

        public void SaveFont()
        {
             //sample.SaveFont(frm.ResourceFilename, frm.FontName, frm.ImageFilename);
            createFont1.FontBuilder.SaveFont(
                saveFont1.ResourceFilename,
                saveFont1.FontName,
                saveFont1.ImageFilename);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            CurrentPage--;

            if (CurrentPage == 1)
                btnPrevious.Enabled = false;

            btnNext.Enabled = true;
            btnNext.Text = "Next >>";
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage == 3)
            {
                SaveFont();

                switch (MessageBox.Show(this,
                    "Successfully saved font.  Create a new font?" + Environment.NewLine +
                    "Click yes to start over, no to quit.", "Font Complete", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button2))
                {
                    case DialogResult.Yes:
                        CurrentPage = 1;
                        saveFont1.ResetControls();
                        break;

                    case DialogResult.No:
                        this.Close();
                        break;
                }

                return;
            }

            CurrentPage++;

            if (CurrentPage == 3)
            {
                btnNext.Enabled = saveFont1.ValidInput;
                btnNext.Text = "Finish";
            }

            btnPrevious.Enabled = true;
        }

        private void btnPrevious_MouseEnter(object sender, EventArgs e)
        {
            if (CurrentPage == 2)
                pnlWarning.Visible = true;
        }
        private void btnPrevious_MouseLeave(object sender, EventArgs e)
        {
            pnlWarning.Visible = false;
        }

        private void saveFont1_ValidInputChanged(object sender, EventArgs e)
        {
            btnNext.Enabled = saveFont1.ValidInput;
            btnNext.Text = "Finish";
        }
    }
}
