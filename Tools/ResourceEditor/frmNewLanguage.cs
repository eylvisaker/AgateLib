using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ERY.AgateLib.Resources;

namespace ResourceEditor
{
    public partial class frmNewLanguage : Form
    {
        private Button btnCancel;
        private Label label1;
        private TextBox textBox1;
        private Panel pnlError;
        private PictureBox pictureBox1;
        private Label label2;
        private Button btnOK;
    
        public frmNewLanguage()
        {
            InitializeComponent();

            UpdateControls();
        }
        public frmNewLanguage(AgateResourceManager res) : this()
        {
            resources = res;


            UpdateControls();
        }

        AgateResourceManager resources;



        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            if (resources == null)
            {
                btnOK.Enabled = false;
                return;
            }

            if (resources.ContainsLanguage(textBox1.Text))
            {
                btnOK.Enabled = false;
                pnlError.Visible = true;
            }
            else
            {
                btnOK.Enabled = true;
                pnlError.Visible = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            resources.AddLanguage(textBox1.Text);
            resources.SetCurrentLanguage(textBox1.Text);
        }
    }
}