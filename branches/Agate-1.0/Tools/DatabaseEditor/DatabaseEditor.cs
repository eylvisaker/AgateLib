using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateLib.Data;

namespace AgateDatabaseEditor
{
	public partial class DatabaseEditor : UserControl
	{
		AgateDatabase mDatabase;
		bool mDirtyState;

		public DatabaseEditor()
		{
			InitializeComponent();
		}

		public AgateDatabase Database
		{
			get
			{
				return mDatabase;
			}
			set
			{
				mDatabase = value;

				DatabaseRefresh();
			}
		}

		public void DatabaseRefresh()
		{
			lstTables.Clear();
			lstTables.Groups.Clear();

			if (Database == null)
			{
				tabs.TabPages.Clear();

				return;
			}

			ListViewGroup taskGroup = new ListViewGroup("Tasks");
			ListViewGroup tableGroup = new ListViewGroup("Tables");

			lstTables.Groups.Add(taskGroup);
			lstTables.Groups.Add(tableGroup);

			ListViewItem newTable = new ListViewItem("New Table");
			newTable.ImageIndex = 1;
			newTable.Tag = new InvokeDelegate(NewTable);
			newTable.Group = taskGroup;

			lstTables.Items.Add(newTable);


			foreach (var table in Database.Tables)
			{
				ListViewItem item = new ListViewItem(table.Name);
				item.ImageIndex = 0;
				item.Tag = table;
				item.Group = tableGroup;

				lstTables.Items.Add(item);
			}

			List<TabPage> pagesToRemove = new List<TabPage>();

			foreach (TabPage tab in tabs.TabPages)
			{
				TableEditor ed = tab.Controls[0] as TableEditor;

				if (ed == null)
					continue;

				if (Database.Tables.Contains(ed.Table) == false)
				{
					pagesToRemove.Add(tab);
				}
			}

			foreach (var tab in pagesToRemove)
				tabs.TabPages.Remove(tab);
		}


		public bool IsTableActive
		{
			get
			{
				if (tabs.TabPages.Count == 0)
					return false;

				if (tabs.SelectedTab.Controls[0] is TableEditor)
					return true;

				return false;
			}
		}

		void OnTableActiveStatusChanged()
		{
			if (TableActiveStatusChanged != null)
				TableActiveStatusChanged(this, EventArgs.Empty);
		}
		public event EventHandler TableActiveStatusChanged;

		private void lstTables_DoubleClick(object sender, EventArgs e)
		{
			openToolStripMenuItem_Click(sender, e);
		}
		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (lstTables.SelectedItems.Count == 0)
				return;

			object obj = lstTables.SelectedItems[0].Tag;
			AgateTable table = obj as AgateTable;
			InvokeDelegate method = obj as InvokeDelegate;

			if (table != null)
			{
				OpenTableTab(table);
			}
			if (method != null)
			{
				method();
			}
		}

		private void OpenTableTab(AgateTable table)
		{
			TabPage page = GetTableTabPage(table);

			if (page == null)
			{
				TableEditor editor = new TableEditor();
				editor.Database = Database;
				editor.Table = table;
				editor.Dock = DockStyle.Fill;
				editor.StatusText += new EventHandler<StatusTextEventArgs>(editor_StatusText);
				editor.SetDirtyFlag += new EventHandler(editor_SetDirtyFlag);

				page = new TabPage(table.Name);
				page.Controls.Add(editor);

				tabs.TabPages.Add(page);
			}

			tabs.SelectedTab = page;

			OnTableActiveStatusChanged();
		}

		void editor_SetDirtyFlag(object sender, EventArgs e)
		{
			DirtyState = true;
		}
		void editor_StatusText(object sender, StatusTextEventArgs e)
		{
			OnStatusText(e);
		}

		/// <summary>
		/// Returns null if the tab page is not open.
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		private TabPage GetTableTabPage(AgateTable table)
		{
			TabPage page = null;

			foreach (TabPage tab in tabs.TabPages)
			{
				Control ctrl = tab.Controls[0];

				if (ctrl is TableEditor)
				{
					TableEditor tb = (TableEditor)ctrl;

					if (tb.Table == table)
					{
						page = tab;
						break;
					}
				}
			}
			return page;
		}

		public bool DirtyState
		{
			get { return mDirtyState; }
			set
			{
				if (value == mDirtyState)
					return;

				mDirtyState = value;
				OnDirtyStateChanged();
			}
		}

		private void OnDirtyStateChanged()
		{
			if (DirtyStateChanged != null)
				DirtyStateChanged(this, EventArgs.Empty);
		}

		public event EventHandler DirtyStateChanged;

		private void OnStatusText(StatusTextIcon icon, string text)
		{
			OnStatusText(new StatusTextEventArgs(icon, text));
		}
		private void OnStatusText(StatusTextEventArgs e)
		{
			if (StatusText != null)
				StatusText(this, e);
		}

		public event EventHandler<StatusTextEventArgs> StatusText;


		private void NewTable()
		{
			AgateTable tbl = new AgateTable();
			tbl.Name = "Table";

			IncrementTableName(tbl);

			while (Database.Tables.ContainsTable(tbl.Name))
			{
				IncrementTableName(tbl);
			}

			Database.Tables.Add(tbl);

			frmDesignTable.EditColumns(Database, tbl);
			OpenTableTab(tbl);

			DatabaseRefresh();

			DirtyState = true;
		}

		private void editColumnsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AgateTable tbl = null;

			if (lstTables.SelectedItems.Count == 0)
				return;
			else if (lstTables.SelectedItems.Count == 1)
			{
				tbl = lstTables.SelectedItems[0].Tag as AgateTable;
			}

			if (tbl == null)
				return;

			
			TabPage tab = GetTableTabPage(tbl);

			if (tab == null)
				return;

			TableEditor ed = tab.Controls[0] as TableEditor;

			if (ed == null)
				return;

			ed.EditColumns();

			DirtyState = true;
		}

		private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TableEditor editor = tabs.SelectedTab.Controls[0] as TableEditor;

			if (editor != null)
			{
				editor.FinalizeData();
			}

			tabs.TabPages.Remove(tabs.SelectedTab);
		}


		private void largeIconsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			lstTables.View = View.LargeIcon;

			smallIconsToolStripMenuItem.Checked = false;
			largeIconsToolStripMenuItem.Checked = true;
			listToolStripMenuItem.Checked = false;
		}
		private void smallIconsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			lstTables.View = View.SmallIcon;

			smallIconsToolStripMenuItem.Checked = true;
			largeIconsToolStripMenuItem.Checked = false;
			listToolStripMenuItem.Checked = false;
		}

		private void listToolStripMenuItem_Click(object sender, EventArgs e)
		{
			lstTables.View = View.List;

			smallIconsToolStripMenuItem.Checked = false;
			largeIconsToolStripMenuItem.Checked = false;
			listToolStripMenuItem.Checked = true;
		}

		private void lstTables_MouseDown(object sender, MouseEventArgs e)
		{

		}
		private void lstTables_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				ContextMenuStrip menu = null;

				if (lstTables.SelectedItems.Count == 0)
					menu = lvContextMenu;
				else if (lstTables.SelectedItems.Count == 1)
				{
					if (lstTables.SelectedItems[0].Tag is AgateTable)
					{
						menu = tableContextMenu;
					}
				}

				if (menu != null)
					menu.Show(lstTables, e.Location);
			}
		}

		private void tabs_MouseUp(object sender, MouseEventArgs e)
		{
			closeTabToolStripMenuItem.Text = "Close " + tabs.SelectedTab.Text;
		}

		private void renameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (lstTables.SelectedItems.Count == 0)
				return;

			lstTables.SelectedItems[0].BeginEdit();

		}

		private void lstTables_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			object obj = lstTables.SelectedItems[0].Tag;
			AgateTable table = obj as AgateTable;

			if (table == null)
				return;

			TabPage page = GetTableTabPage(table);

			if (Database.Tables.ContainsTable(e.Label))
			{
				e.CancelEdit = true;
			}
			else
			{
				table.Name = e.Label;
				page.Text = table.Name;
			}

			DirtyState = true;
		}

		private void lstTables_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F2)
			{
				if (lstTables.SelectedItems.Count == 0)
					return;

				lstTables.SelectedItems[0].BeginEdit();
			}
			else if (e.KeyCode == Keys.Delete)
			{
				deleteToolStripMenuItem_Click(sender, e);
			}
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (lstTables.SelectedItems.Count == 0)
				return;

			object obj = lstTables.SelectedItems[0].Tag;
			AgateTable table = obj as AgateTable;

			if (MessageBox.Show(this,
				"Really delete table " + table.Name + "?" + Environment.NewLine + Environment.NewLine +
				"This operation cannot be undone.",
				"Delete Table?", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				Database.Tables.Remove(table);
				DatabaseRefresh();
			}

			DirtyState = true;
		}

		private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (lstTables.SelectedItems.Count == 0)
				return;

			object obj = lstTables.SelectedItems[0].Tag;
			AgateTable table = obj as AgateTable;

			AgateTable newTable = table.Clone();

			// Append _1 to a new clone, or if there is already this suffix
			// do _(i+1) where i is the suffix.
			do
			{
				IncrementTableName(newTable);
			} while (Database.Tables.ContainsTable(newTable.Name));

			Database.Tables.Add(newTable);
			DatabaseRefresh();

			DirtyState = true;
		}

		private static void IncrementTableName(AgateTable table)
		{
			int uindex = table.Name.LastIndexOf('_');
			bool appendCode = true;

			if (uindex > -1 && uindex < table.Name.Length - 1)
			{
				int value;

				if (int.TryParse(table.Name.Substring(uindex + 1), out value))
				{
					table.Name = table.Name.Substring(0, uindex);
					table.Name += "_" + (value + 1).ToString();

					appendCode = false;
				}
			}

			if (appendCode)
			{
				table.Name += "_1";
			}

		}

		TableEditor CurrentTable
		{
			get
			{
				if (tabs.TabPages.Count == 0)
					return null;

				return tabs.SelectedTab.Controls[0] as TableEditor;
			}
		}


		internal void DesignCurrentTable()
		{
			if (CurrentTable == null)
				return;

			CurrentTable.EditColumns();
			DirtyState = true;
		}

		internal void CurrentTableSortAscending()
		{
			if (CurrentTable == null)
				return;

			CurrentTable.SortAscending();
			DirtyState = true;

		}

		internal void CurrentTableSortDescending()
		{
			if (CurrentTable == null)
				return;

			CurrentTable.SortDescending();
			DirtyState = true;
		}

		private void exportToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (lstTables.SelectedItems.Count == 0) return;

			object obj = lstTables.SelectedItems[0].Tag;
			
			if (obj == null || obj is AgateTable != true)
				return;

			AgateTable table = obj as AgateTable;

			saveExportDialog.FileName = table.Name + ".txt";

			if (saveExportDialog.ShowDialog() == DialogResult.Cancel)
				return;

			using (System.IO.StreamWriter w = new System.IO.StreamWriter(saveExportDialog.FileName))
			{
				for (int i = 0; i < table.Columns.Count; i++)
				{
					if (i != 0) w.Write("\t");

					w.Write(table.Columns[i].Name);
				}

				for (int i = 0; i < table.Rows.Count; i++)
				{
					w.WriteLine();

					for (int j = 0; j < table.Columns.Count; j++)
					{
						if (j != 0) w.Write("\t");

						w.Write(table.Rows[i][table.Columns[j]]);
					}
				}
			}
		}
	}

	delegate void InvokeDelegate();

}
