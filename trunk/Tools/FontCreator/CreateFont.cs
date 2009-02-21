using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AgateLib;
using AgateLib.BitmapFont;

namespace FontCreator
{
    public partial class CreateFont : UserControl 
    {
        FontBuilder sample;

        bool AnyDesignMode
        {
            get
            {
                Control p = this;

                do
                {
                    if (p.Site != null && p.Site.DesignMode)
                        return true;

                    p = p.Parent;

                } while (p != null);

                return false;
            }
        }

        public CreateFont()
        {
            InitializeComponent();
        }

        public FontBuilder FontBuilder { get { return sample; } }

        protected override void  OnLoad(EventArgs e)
        {
 	        base.OnLoad(e);

            if (AnyDesignMode)
                return;

             sample = new FontBuilder();

            sample.SetRenderTarget(renderTarget, zoomRenderTarget);

            int index = 0;

			List<string> fonts = new List<string>();
            foreach (FontFamily fam in FontFamily.Families)
            {
				fonts.Add(fam.Name);
			}
			fonts.Sort();

			foreach (string family in fonts)
			{
                if (family == "Arial" || family.Contains("Sans Serif") && index == 0)
                    index = cboFamily.Items.Count;

                cboFamily.Items.Add(family);
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

            if (sample == null)
                return;

            sample.Draw();
        }

        private void cboFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sample == null)
                return;

            sample.FontFamily = cboFamily.SelectedItem.ToString();
        }
		
        private void nudScale_ValueChanged(object sender, EventArgs e)
        {
            if (sample == null)
                return;

            sample.DisplayScale = (double)nudScale.Value;
        }
        private void nudSize_ValueChanged(object sender, EventArgs e)
        {
            if (sample == null)
                return;

            sample.FontSize = (float)nudSize.Value;
        }
        private void chkBold_CheckedChanged(object sender, EventArgs e)
        {
            if (sample == null)
                return;

            sample.Bold = chkBold.Checked;
        }
        private void chkItalic_CheckedChanged(object sender, EventArgs e)
        {
            if (sample == null)
                return;

            sample.Italic = chkItalic.Checked;
        }
        private void chkUnderline_CheckedChanged(object sender, EventArgs e)
        {
            if (sample == null)
                return;

            sample.Underline = chkUnderline.Checked;
        }
        private void chkStrikeout_CheckedChanged(object sender, EventArgs e)
        {
            if (sample == null)
                return;

            sample.Strikeout = chkStrikeout.Checked;
        }
        private void txtSampleText_TextChanged(object sender, EventArgs e)
        {
            if (sample == null)
                return;

            sample.Text = txtSampleText.Text;
        }

        private void renderTarget_Resize(object sender, EventArgs e)
        {
            if (sample == null)
                return;

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

            ((BitmapFontImpl)sample.Font.Impl).Surface.SaveTo(tempImage);

            EditGlyphs frm = new EditGlyphs();

            //frm.ShowDialog(this, tempImage, ((BitmapFontImpl)sample.Font.Impl).FontMetrics);
            frm.Dispose();

            try
            {
                File.Delete(tempImage);
            }
            catch { }

        }

    }
}