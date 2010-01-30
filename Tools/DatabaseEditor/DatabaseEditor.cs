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

				if (Database.Tables.Contains(ed.AgateTable) == false)
				{
					pagesToRemove.Add(tab);
				}
			}

			foreach (var tab in pagesToRemove)
				tabs.TabPages.Remove(tab);
		}

		private void lstTables_DoubleClick(object sender, EventArgs e)
		{
			if (lstTables.SelectedItems.Count == 0)
				return;

			object obj = lstTables.SelectedItems[0].Tag ;
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
			foreach (TabPage tab in tabs.TabPages)
			{
				Control ctrl = tab.Controls[0];
				
				if (ctrl is TableEditor)
				{
					TableEditor tb = (TableEditor)ctrl;

					if (tb.AgateTable == table)
					{
						tabs.SelectedTab = tab;
						return;
					}
				}
			}

			TabPage page = new TabPage(table.Name);
			
			TableEditor editor = new TableEditor();
			editor.Database = Database;
			editor.AgateTable = table;
			editor.Dock = DockStyle.Fill;

			page.Controls.Add(editor);

			tabs.TabPages.Add(page);

			tabs.SelectedTab = page;
		}

		private void NewTable()
		{
			MessageBox.Show("Creating new table");
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

			if (Database.Tables.ContainsTable(e.Label))
			{
				e.CancelEdit = true;
			}
			else
				table.Name = e.Label;
		}

		private void lstTables_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F2)
			{
				if (lstTables.SelectedItems.Count == 0)
					return;

				lstTables.SelectedItems[0].BeginEdit();
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
			}
		}

	}

	delegate void InvokeDelegate();

}
