using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FontCreator
{
    public partial class frmWarningSplash : Form
    {
        public frmWarningSplash()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            
            Properties.Settings.Default.SkipWarning = chkSkipWarning.Checked;
        }
    }
}
