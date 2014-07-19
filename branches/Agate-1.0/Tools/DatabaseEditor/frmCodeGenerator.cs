using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AgateDatabaseEditor
{
	public partial class frmCodeGenerator : Form
	{
		public frmCodeGenerator()
		{
			InitializeComponent();

			cboCodeProvider.Items.Add(typeof(Microsoft.CSharp.CSharpCodeProvider));
			cboCodeProvider.Items.Add(typeof(Microsoft.VisualBasic.VBCodeProvider));
			cboCodeProvider.SelectedIndex = 0;

			UpdateControls();
		}

		public string StartDirectory
		{
			get
			{
				return folderBrowser.SelectedPath ;
			}
			set
			{
				folderBrowser.SelectedPath = value;
			}
		}
		public string Directory
		{
			get { return txtDirectory.Text; }
			set { txtDirectory.Text = value; }
		}
		public string Namespace
		{
			get { return txtNamespace.Text; }
			set { txtNamespace.Text = value; }
		}
		public Type CodeDomProviderType
		{
			get
			{
				if (cboCodeProvider.SelectedIndex > -1)
					return (Type)(cboCodeProvider.SelectedItem);
				else
					return null;
			}
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			if (folderBrowser.ShowDialog() == DialogResult.OK)
			{
				txtDirectory.Text = folderBrowser.SelectedPath;
			}
		}

		private void txtDirectory_TextChanged(object sender, EventArgs e)
		{
			UpdateControls();
		}

		private void txtNamespace_TextChanged(object sender, EventArgs e)
		{
			UpdateControls();
		}

		private void cboCodeProvider_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateControls();
		}

		private void UpdateControls()
		{
			bool okenable = true;

			if (cboCodeProvider.SelectedIndex == -1) okenable = false;
			if (txtNamespace.Text.Length == 0) okenable = false;
			if (txtDirectory.Text.Length == 0) okenable = false;

			btnOK.Enabled = okenable;
		}

	}
}
