using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateLib.Resources;

namespace PackedSpriteCreator
{
    public partial class frmNewSprite : Form
    {
        AgateResourceManager resources;

        public frmNewSprite()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IWin32Window parent, AgateResourceManager resources)
        {
            this.resources = resources;

            return ShowDialog(parent);
        }

        public string SpriteName
        {
            get { return txtName.Text; }
        }
        public Size SpriteFrameSize
        {
            get { return new Size(int.Parse(txtWidth.Text), int.Parse(txtHeight.Text)); }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            bool okEnabled = true;
            int unused;

            if (resources.Sprites.Contains(txtName.Text)) okEnabled = false;
            if (int.TryParse(txtWidth.Text, out unused) == false) okEnabled = false;
            if (int.TryParse(txtHeight.Text, out unused) == false) okEnabled = false;

            btnOK.Enabled = okEnabled;
        }
    }
}
