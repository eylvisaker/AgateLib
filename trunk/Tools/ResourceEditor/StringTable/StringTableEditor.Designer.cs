namespace ResourceEditor.StringTable
{
    partial class StringTableEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSort = new System.Windows.Forms.ToolStripButton();
            this.btnSortUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cboFilter = new System.Windows.Forms.ToolStripComboBox();
            this.btnApplyFilter = new System.Windows.Forms.ToolStripButton();
            this.btnClearFilter = new System.Windows.Forms.ToolStripButton();
            this.list = new System.Windows.Forms.ListView();
            this.panel = new System.Windows.Forms.TableLayoutPanel();
            this.defaultLanguageEntry = new ResourceEditor.StringTable.StringEntry();
            this.entryPanel = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            this.entryPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnRemove,
            this.toolStripSeparator1,
            this.btnSort,
            this.btnSortUp,
            this.toolStripSeparator2,
            this.cboFilter,
            this.btnApplyFilter,
            this.btnClearFilter});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(451, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAdd.Image = global::ResourceEditor.Properties.Resources.Plus;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(23, 22);
            this.btnAdd.Text = "Add String";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemove.Image = global::ResourceEditor.Properties.Resources.Minus;
            this.btnRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(23, 22);
            this.btnRemove.Text = "Remove String";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSort
            // 
            this.btnSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSort.Image = global::ResourceEditor.Properties.Resources.SortHS;
            this.btnSort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(23, 22);
            this.btnSort.Text = "Sort";
            // 
            // btnSortUp
            // 
            this.btnSortUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSortUp.Image = global::ResourceEditor.Properties.Resources.SortUpHS;
            this.btnSortUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSortUp.Name = "btnSortUp";
            this.btnSortUp.Size = new System.Drawing.Size(23, 22);
            this.btnSortUp.Text = "SortUp";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // cboFilter
            // 
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.Size = new System.Drawing.Size(121, 25);
            // 
            // btnApplyFilter
            // 
            this.btnApplyFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnApplyFilter.Image = global::ResourceEditor.Properties.Resources.Filter2HS;
            this.btnApplyFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApplyFilter.Name = "btnApplyFilter";
            this.btnApplyFilter.Size = new System.Drawing.Size(23, 22);
            this.btnApplyFilter.Text = "toolStripButton3";
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClearFilter.Image = global::ResourceEditor.Properties.Resources.Broom;
            this.btnClearFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(23, 22);
            this.btnClearFilter.Text = "toolStripButton4";
            // 
            // list
            // 
            this.list.Dock = System.Windows.Forms.DockStyle.Left;
            this.list.HideSelection = false;
            this.list.LabelEdit = true;
            this.list.Location = new System.Drawing.Point(0, 25);
            this.list.MultiSelect = false;
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(118, 318);
            this.list.TabIndex = 3;
            this.list.UseCompatibleStateImageBehavior = false;
            this.list.View = System.Windows.Forms.View.List;
            this.list.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.list_MouseDoubleClick);
            this.list.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.list_AfterLabelEdit);
            this.list.SelectedIndexChanged += new System.EventHandler(this.list_SelectedIndexChanged);
            this.list.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.list_ItemSelectionChanged);
            this.list.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.list_BeforeLabelEdit);
            this.list.KeyDown += new System.Windows.Forms.KeyEventHandler(this.list_KeyDown);
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.AutoScroll = true;
            this.panel.ColumnCount = 1;
            this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panel.Location = new System.Drawing.Point(0, 67);
            this.panel.Name = "panel";
            this.panel.RowCount = 1;
            this.panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panel.Size = new System.Drawing.Size(333, 251);
            this.panel.TabIndex = 6;
            // 
            // defaultStringEntry
            // 
            this.defaultLanguageEntry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.defaultLanguageEntry.LanguageName = "Default";
            this.defaultLanguageEntry.Location = new System.Drawing.Point(3, 0);
            this.defaultLanguageEntry.Name = "defaultStringEntry";
            this.defaultLanguageEntry.Size = new System.Drawing.Size(327, 61);
            this.defaultLanguageEntry.TabIndex = 5;
            this.defaultLanguageEntry.TextChanged += new System.EventHandler(this.stringEntry_TextChanged);
            // 
            // entryPanel
            // 
            this.entryPanel.Controls.Add(this.defaultLanguageEntry);
            this.entryPanel.Controls.Add(this.panel);
            this.entryPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entryPanel.Location = new System.Drawing.Point(118, 25);
            this.entryPanel.Name = "entryPanel";
            this.entryPanel.Size = new System.Drawing.Size(333, 318);
            this.entryPanel.TabIndex = 7;
            // 
            // StringTableEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.entryPanel);
            this.Controls.Add(this.list);
            this.Controls.Add(this.toolStrip1);
            this.Name = "StringTableEditor";
            this.Size = new System.Drawing.Size(451, 343);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.entryPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnRemove;
        private System.Windows.Forms.ListView list;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnSort;
        private System.Windows.Forms.ToolStripButton btnSortUp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripComboBox cboFilter;
        private System.Windows.Forms.ToolStripButton btnApplyFilter;
        private System.Windows.Forms.ToolStripButton btnClearFilter;
        private StringEntry defaultLanguageEntry;
        private System.Windows.Forms.TableLayoutPanel panel;
        private System.Windows.Forms.Panel entryPanel;
    }
}
