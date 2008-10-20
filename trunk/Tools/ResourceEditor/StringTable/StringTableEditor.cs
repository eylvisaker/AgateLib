using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ERY.AgateLib.Resources;

namespace ResourceEditor.StringTable
{
    public partial class StringTableEditor : UserControl
    {
        AgateResourceManager mResources;

        public StringTableEditor()
        {
            InitializeComponent();
        }

        public AgateResourceManager ResourceManager
        {
            get { return mResources; }
            set
            {
                if (value == mResources && value == null)
                    return;

                if (mResources != null)
                {
                    mResources.Languages.LanguageAdded -= Languages_LanguageAdded;
                    mResources.Languages.LanguageRemoved -= Languages_LanguageRemoved;
                }

                mResources = value;

                if (mResources != null)
                {
                    mResources.Languages.LanguageAdded += new EventHandler<LanguageListChangedEventArgs>(Languages_LanguageAdded);
                    mResources.Languages.LanguageRemoved += new EventHandler<LanguageListChangedEventArgs>(Languages_LanguageRemoved);
                }

                list.Items.Clear();
                UpdateControls();
            }
        }

        void Languages_LanguageRemoved(object sender, LanguageListChangedEventArgs e)
        {
            UpdateControls();
        }
        void Languages_LanguageAdded(object sender, LanguageListChangedEventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            if (mResources == null)
                return;

            ResourceGroup group = ResourceManager.DefaultLanguage;

            // add any items to the list view that are not there
            foreach (string key in group.Strings.Keys)
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

                if (group.Strings.ContainsKey(text))
                    continue;

                list.Items.RemoveAt(i);
                i--;
            }

            // add any languages that don't have entries in the panel
            int tabIndex = list.TabIndex;

            foreach (ResourceGroup language in ResourceManager.Languages)
            {
                if (LanguageEntryExists(language.LanguageName, ref tabIndex))
                    continue;

                StringEntry entry = new StringEntry();
                panel.Controls.Add(entry);

                entry.LanguageName = language.LanguageName;
                entry.Width = panel.ClientRectangle.Width;
                entry.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                entry.TabIndex = ++tabIndex;
                entry.TextChanged += new EventHandler(stringEntry_TextChanged);

                
            }

            // remove any languages from the panel which don't exist any more.
            for (int i = 0; i < panel.Controls.Count; i++)
            {
                StringEntry entry = panel.Controls[i] as StringEntry;
                if (entry == null) continue;

                if (ResourceManager.Languages.Contains(entry.LanguageName))
                    continue;

                entry.TextChanged -= stringEntry_TextChanged;

                panel.Controls.Remove(entry);
                i--;
            }
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

            ResourceGroup group = ResourceManager.DefaultLanguage;

            string name = "New String";

            if (group.Strings.ContainsKey(name))
            {
                int index = 1;

                while (group.Strings.ContainsKey(name + " (" + index + ")"))
                {
                    index++;
                }

                name += " (" + index + ")";
            }

            group.Strings.Add(name, "");

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

            if (ResourceManager.DefaultLanguage.Strings.ContainsKey(newname))
            {
                e.CancelEdit = true;
                System.Media.SystemSounds.Beep.Play();
                SetStatusText("The string \"" + newname + "\" already exists.");
            }
            else
            {
                for (int i = 0; i < ResourceManager.Languages.Count; i++)
                {
                    ERY.AgateLib.Resources.StringTable language = ResourceManager.Languages[i].Strings;

                    if (language.ContainsKey(oldname) == false)
                        continue;

                    string value = language[oldname];

                    language.Remove(oldname);
                    language.Add(newname, value);
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
                foreach (ResourceGroup group in ResourceManager.Languages)
                {
                    group.Strings.Remove(names[i]);
                }
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

            if (entry == null)                return;
            if (list.SelectedItems.Count != 1) return;

            string language = entry.LanguageName;
            string key = list.SelectedItems[0].Text;

            ResourceManager.Languages[language].Strings[key] = entry.Text;
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
            string language = entry.LanguageName;
            string key = list.SelectedItems[0].Text;

            ResourceGroup lang = ResourceManager.Languages[language];

            if (lang.Strings.ContainsKey(key))
            {
                entry.Text = ResourceManager.Languages[language].Strings[key];
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
