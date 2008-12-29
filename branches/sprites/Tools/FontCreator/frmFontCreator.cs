using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AgateLib.BitmapFont;

namespace FontCreator
{
    public partial class frmFontCreator : Form
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (AgateLib.AgateSetup setup = new AgateLib.AgateSetup(args))
            {
                setup.AskUser = true;
                setup.Initialize(true, false, false);
                if (setup.Cancel)
                    return;

                Application.Run(new frmFontCreator());
            }
        }

        FontCreator sample = new FontCreator();

        public frmFontCreator()
        {
            InitializeComponent();

            sample.SetRenderTarget(renderTarget, zoomRenderTarget);

            int index = 0;

            foreach (FontFamily fam in FontFamily.Families)
            {
                if (fam.Name == "Arial" || fam.Name.Contains("Sans Serif") && index == 0)
                    index = cboFamily.Items.Count;

                cboFamily.Items.Add(fam.Name);
            }

            cboFamily.SelectedIndex = index;
            txtSampleText_TextChanged(null, null);

            foreach (BitmapFontEdgeOptions opt in
                Enum.GetValues(typeof(BitmapFontEdgeOptions)))
            {
                cboEdges.Items.Add(opt);
            }

            cboEdges.SelectedItem = BitmapFontEdgeOptions.IntensityAlphaWhite;

            cboBg.SelectedIndex = 0;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            sample.Draw();
        }

        private void cboFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            sample.FontFamily = cboFamily.SelectedItem.ToString();
        }
		
        private void nudScale_ValueChanged(object sender, EventArgs e)
        {
            sample.DisplayScale = (double)nudScale.Value;
        }
        private void nudSize_ValueChanged(object sender, EventArgs e)
        {
            sample.FontSize = (float)nudSize.Value;
        }
        private void chkBold_CheckedChanged(object sender, EventArgs e)
        {
            sample.Bold = chkBold.Checked;
        }
        private void chkItalic_CheckedChanged(object sender, EventArgs e)
        {
            sample.Italic = chkItalic.Checked;
        }
        private void chkUnderline_CheckedChanged(object sender, EventArgs e)
        {
            sample.Underline = chkUnderline.Checked;
        }
        private void chkStrikeout_CheckedChanged(object sender, EventArgs e)
        {
            sample.Strikeout = chkStrikeout.Checked;
        }
        private void txtSampleText_TextChanged(object sender, EventArgs e)
        {
            sample.Text = txtSampleText.Text;
        }

        private void renderTarget_Resize(object sender, EventArgs e)
        {
            sample.Draw();
        }

        private void btnBorderColor_Click(object sender, EventArgs e)
        {
            colorDialog.Color = btnBorderColor.BackColor;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                btnBorderColor.BackColor = colorDialog.Color;

                sample.Options.BorderColor = ConvertColor(colorDialog.Color);
                sample.Options.BorderColor = AgateLib.Geometry.Color.FromArgb((int)nudOpacity.Value, sample.Options.BorderColor);

                sample.Options.CreateBorder = true;

                chkBorder.Checked = true;

                sample.CreateFont();
            }
        }
        private void nudOpacity_ValueChanged(object sender, EventArgs e)
        {
            sample.Options.BorderColor = AgateLib.Geometry.Color.FromArgb((int)nudOpacity.Value, sample.Options.BorderColor);

            if (chkBorder.Checked)
            {
                sample.CreateFont();
            }
        }

        private AgateLib.Geometry.Color ConvertColor(System.Drawing.Color clr)
        {
            return AgateLib.Geometry.Color.FromArgb(clr.R, clr.G, clr.B);
        }

        private void chkTextRenderer_CheckedChanged(object sender, EventArgs e)
        {
            sample.Options.UseTextRenderer = chkTextRenderer.Checked;
            sample.CreateFont();
        }

        private void cboBg_SelectedIndexChanged(object sender, EventArgs e)
        {
            sample.LightBackground = cboBg.SelectedIndex == 1;
            
        }

        private void btnDisplayColor_Click(object sender, EventArgs e)
        {
            colorDialog.Color = btnDisplayColor.BackColor;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                btnDisplayColor.BackColor = colorDialog.Color;

                sample.DisplayColor = ConvertColor(colorDialog.Color);
            }
        }

        private void chkBorder_CheckedChanged(object sender, EventArgs e)
        {
            sample.Options.CreateBorder = chkBorder.Checked;

            sample.CreateFont();
        }

        private void cboEdges_SelectedIndexChanged(object sender, EventArgs e)
        {
            sample.Options.EdgeOptions = (BitmapFontEdgeOptions)cboEdges.SelectedItem;

            sample.CreateFont();
        }

        private void btnViewFont_Click(object sender, EventArgs e)
        {
            string tempImage = Path.GetTempFileName() + ".png";
            string tempXml = Path.GetTempFileName() + ".xml";

            sample.Font.Save(tempImage, tempXml);

            frmViewFont frm = new frmViewFont();

            frm.ShowDialog(this, tempImage, tempXml);
            frm.Dispose();

            try
            {
                File.Delete(tempXml);
                File.Delete(tempImage);
            }
            catch { }

        }


    }
}