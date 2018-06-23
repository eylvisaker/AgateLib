using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using AgateLib.Resources.Legacy;

namespace ResourceEditor.StringTable
{

	public partial class StringTableEditor : UserControl
	{
		AgateResourceCollection mResources;

		public StringTableEditor()
		{
			InitializeComponent();
		}

		public AgateResourceCollection ResourceManager
		{
			get { return mResources; }
			set
			{
				if (value == mResources && value == null)
					return;

				mResources = value;

				list.Items.Clear();
				UpdateControls();
			}
		}

		private void UpdateControls()
		{
			if (mResources == null)
				return;

			// add any items to the list view that are not there
			foreach (string key in ResourceManager.Strings.Keys)
			{
				if (ListBoxContainsItem(key))
					continue;

				ListViewItem item = new ListViewItem(key);
				list.Items.Add(item);

			}
			// remove extra items from the list view.
			for (int i = 0; i < list.Items.Count; i++)
			{
				string text = list.Items[i].Text;

				if (ResourceManager.Strings.ContainsKey(text))
					continue;

				list.Items.RemoveAt(i);
				i--;
			}

			int tabIndex = list.TabIndex;

		}

		private bool LanguageEntryExists(string p, ref int tabIndex)
		{
			foreach (StringEntry entry in panel.Controls)
			{
				if (entry.LanguageName.Equals(p, StringComparison.InvariantCultureIgnoreCase) == false)
					continue;

				tabIndex = Math.Max(tabIndex, entry.TabIndex);

				return true;
			}

			return false;
		}

		private bool ListBoxContainsItem(string key)
		{
			for (int i = 0; i < list.Items.Count; i++)
			{
				ListViewItem item = list.Items[i];

				if (item.Text == key)
					return true;
			}

			return false;
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			list.SelectedItems.Clear();

			string name = "New String";

			if (ResourceManager.Strings.ContainsKey(name))
			{
				int index = 1;

				while (ResourceManager.Strings.ContainsKey(name + " (" + index + ")"))
				{
					index++;
				}

				name += " (" + index + ")";
			}

			ResourceManager.Strings.Add(name, "");

			ListViewItem item = new ListViewItem(name);

			list.Items.Add(item);

			item.BeginEdit();
		}

		private void list_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void list_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			System.Diagnostics.Debug.Print("After Label Edit: {0}, {1}, {2}",
				e.Item, e.Label, e.CancelEdit);

			if (e.Label == null)
				return;

			ListViewItem item = list.Items[e.Item];

			string oldname = item.Text;
			string newname = e.Label;

			if (newname == oldname)
				return;

			if (ResourceManager.Strings.ContainsKey(newname))
			{
				e.CancelEdit = true;
				System.Media.SystemSounds.Beep.Play();
				SetStatusText("The string \"" + newname + "\" already exists.");
			}
			else
			{
				var strings = ResourceManager.Strings;

				if (strings.ContainsKey(oldname))
				{
					string value = strings[oldname];

					strings.Remove(oldname);
					strings.Add(newname, value);
				}
			}
		}
		private void list_BeforeLabelEdit(object sender, LabelEditEventArgs e)
		{
			System.Diagnostics.Debug.Print("Before Label Edit: {0}, {1}, {2}",
				e.Item, e.Label, e.CancelEdit);
		}


		private void SetStatusText(string text)
		{
			if (StatusText != null)
				StatusText(this, new StatusTextEventArgs(text));
		}

		public event EventHandler<StatusTextEventArgs> StatusText;

		private void list_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
				Remove();
			else if (e.KeyCode == Keys.F2)
			{
				if (list.SelectedItems.Count == 1)
					EditCurrentItem();
			}

		}
		private void EditCurrentItem()
		{
			if (list.SelectedItems.Count != 1)
				throw new InvalidOperationException("Cannot edit multiple items at once.");

			list.SelectedItems[0].BeginEdit();
		}
		private void btnRemove_Click(object sender, EventArgs e)
		{
			Remove();
		}

		private void Remove()
		{
			List<string> names = new List<string>();

			for (int i = 0; i < list.SelectedItems.Count; i++)
				names.Add(list.SelectedItems[i].Text);

			for (int i = 0; i < names.Count; i++)
			{
				ResourceManager.Strings.Remove(names[i]);
			}

			UpdateControls();
		}

		private void list_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (list.SelectedItems.Count == 1)
				list.SelectedItems[0].BeginEdit();
		}

		private void stringEntry_TextChanged(object sender, EventArgs e)
		{
			StringEntry entry = sender as StringEntry;

			Debug.Assert(entry != null);
			Debug.Assert(list.SelectedItems.Count <= 1);

			if (entry == null) return;
			if (list.SelectedItems.Count != 1) return;

			string key = list.SelectedItems[0].Text;

			ResourceManager.Strings[key] = entry.Text;
		}

		private void list_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			Debug.Print("Selection count: {0}", list.SelectedItems.Count);

			if (list.SelectedItems.Count != 1)
				panel.Enabled = false;
			else
			{
				panel.Enabled = true;

				foreach (StringEntry entry in panel.Controls)
				{
					LoadValue(entry);
				}
			}
		}
		private void LoadValue(StringEntry entry)
		{
			string key = list.SelectedItems[0].Text;

			if (ResourceManager.Strings.ContainsKey(key))
			{
				entry.Text = ResourceManager.Strings[key];
			}
			else
				entry.Text = "";

		}

		private void list_Resize(object sender, EventArgs e)
		{
			listHeader.Width = list.ClientSize.Width - 5;
		}
	}
}
