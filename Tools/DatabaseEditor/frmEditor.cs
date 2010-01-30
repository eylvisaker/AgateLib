using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DatabaseEditor
{
	public partial class frmEditor : Form
	{
		string filename;

		public frmEditor()
		{
			InitializeComponent();
		}

		private void openDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (CheckSave() == false)
				return;
			if (openDatabase.ShowDialog() == DialogResult.Cancel)
				return;

			databaseEditor1.Visible = true;
			databaseEditor1.Database = AgateLib.Data.AgateDatabase.FromFile(openDatabase.FileName);

			filename = openDatabase.FileName;
		}

		private void newDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (CheckSave() == false)
				return;

			databaseEditor1.Visible = true;
			databaseEditor1.Database = new AgateLib.Data.AgateDatabase();
		}

		/// <summary>
		/// Asks the user if they want to save before continuing the next operation.
		/// Returns false if the next operation should be canceled.
		/// </summary>
		/// <returns></returns>
		private bool CheckSave()
		{
			if (databaseEditor1.Database == null)
				return true;

			string name = System.IO.Path.GetFileName(filename);

			if (string.IsNullOrEmpty(filename))
				name = "untitled database";

			var result = MessageBox.Show(this,
				"Do you want to save " + name + " first?",
				"Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

			if (result == DialogResult.Yes)
			{
				return Save();
			}
			else if (result == DialogResult.No)
			{
				return true;
			}
			else
				return false;
		}
		/// <summary>
		/// Saves the file, asking the user if there is no filename.
		/// Returns false if the file couldn't be saved.
		/// </summary>
		/// <returns></returns>
		private bool Save()
		{
			if (string.IsNullOrEmpty(filename))
				return SaveAs();

			AgateDataLib.DatabaseWriter w = new AgateDataLib.DatabaseWriter();
			w.Database = databaseEditor1.Database;

			try
			{
				w.WriteData(filename);
			}
			catch (Exception e)
			{
				MessageBox.Show("Failed to save file." + Environment.NewLine + e.Message);
				return false;
			}

			return true;
		}
		/// <summary>
		/// Saves the file, asking the user for a filename.
		/// Returns false if the file couldn't be saved.
		/// </summary>
		/// <returns></returns>
		private bool SaveAs()
		{
			saveDatabase.FileName = filename;

			if (saveDatabase.ShowDialog() == DialogResult.OK)
			{
				filename = saveDatabase.FileName;

				return Save();
			}
			else
				return false;
		}

		private void saveDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Save();
		}


		private void saveDatabaseAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveAs();
		}

		private void generateCodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmCodeGenerator frm = new frmCodeGenerator();

			frm.Namespace = databaseEditor1.Database.CodeNamespace;
			frm.StartDirectory = System.IO.Path.GetDirectoryName(filename);

			if (frm.ShowDialog() == DialogResult.OK)
			{
				databaseEditor1.Database.CodeNamespace = frm.Namespace;

				GenerateCode(frm.CodeDomProviderType, frm.Directory, frm.Namespace);
			}
		}

		private void GenerateCode(Type type, string directory, string theNamespace)
		{
			GenerateCode((System.CodeDom.Compiler.CodeDomProvider)
				Activator.CreateInstance(type), directory, theNamespace);
		}
		private void GenerateCode(System.CodeDom.Compiler.CodeDomProvider provider, string directory, string theNamespace)
		{
			CodeGenerator gen = new CodeGenerator();

			gen.Provider = provider;
			gen.Directory = directory;
			gen.Namespace = theNamespace;

			gen.Run(databaseEditor1.Database);

		}

		private void importDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmImportTable import = new frmImportTable();

			import.Database = databaseEditor1.Database;

			if (import.ShowDialog() == DialogResult.OK)
			{
				databaseEditor1.DatabaseRefresh();
			}
		}

		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
