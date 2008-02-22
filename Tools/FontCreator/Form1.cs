using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FontCreator
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (ERY.AgateLib.AgateSetup setup = new ERY.AgateLib.AgateSetup())
            {
                setup.Initialize(true, false, false);
                if (setup.Cancel)
                    return;

                Application.Run(new Form1());
            }
        }

        FontCreator sample = new FontCreator();

        public Form1()
        {
            InitializeComponent();

            sample.SetRenderTarget(renderTarget, zoomRenderTarget);

            int index = 0;

            foreach (FontFamily fam in FontFamily.Families)
            {
                cboFamily.Items.Add(fam.Name);

                if (fam.Name.Contains("Times"))
                    index = cboFamily.Items.Count;
            }

            cboFamily.SelectedIndex = index;
            txtSampleText_TextChanged(null, null);
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


    }
}