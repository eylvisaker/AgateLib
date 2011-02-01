using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateDataLib;

namespace AgateDatabaseEditor.Import
{
	class DatabaseImporter
	{
		public AgateLib.Data.AgateDatabase Database { get; set; }

		public void Run()
		{
			if (Database == null)
				throw new InvalidOperationException("Database cannot be null.");

			string filename = OpenFile();
			if (filename == null)
				return;

			if (IsTextFile(filename))
			{
				frmImportTable import = new frmImportTable();

				import.Database = Database;
				import.Filename = filename;
				import.ShowDialog();
			}
			else
			{
				MessageBox.Show("No import filter for the specified filetype.", "Cannot import",
					 MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		private bool IsAccessDatabase(string filename)
		{
			if (Path.GetExtension(filename).ToLowerInvariant() == ".mdb")
				return true;
			else
				return false;
		}

		private bool IsTextFile(string filename)
		{
			string ext = Path.GetExtension(filename).ToLowerInvariant();

			if (ext == ".txt" || ext == ".csv")
				return true;
			else
				return false; 
		}

		private string OpenFile()
		{
			using (OpenFileDialog o = new OpenFileDialog())
			{
				o.Filter = "Text Files (*.txt,*.csv)|*.txt;*.csv|All Files|*.*";

				DialogResult result = o.ShowDialog();

				if (result == DialogResult.Cancel)
					return null;


				return o.FileName;
			}
		}
	}
}
