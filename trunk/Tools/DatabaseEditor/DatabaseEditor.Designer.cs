namespace AgateDatabaseEditor
{
	partial class DatabaseEditor
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseEditor));
			this.lstTables = new System.Windows.Forms.ListView();
			this.largeImages = new System.Windows.Forms.ImageList(this.components);
			this.smallImages = new System.Windows.Forms.ImageList(this.components);
			this.tabs = new System.Windows.Forms.TabControl();
			this.tabContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.closeTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lvContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.largeIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.smallIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.listToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tableContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.duplicateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.editColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabContextMenu.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.lvContextMenu.SuspendLayout();
			this.tableContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// lstTables
			// 
			this.lstTables.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstTables.LabelEdit = true;
			this.lstTables.LargeImageList = this.largeImages;
			this.lstTables.Location = new System.Drawing.Point(0, 0);
			this.lstTables.Name = "lstTables";
			this.lstTables.Size = new System.Drawing.Size(172, 538);
			this.lstTables.SmallImageList = this.smallImages;
			this.lstTables.TabIndex = 6;
			this.lstTables.UseCompatibleStateImageBehavior = false;
			this.lstTables.View = System.Windows.Forms.View.SmallIcon;
			this.lstTables.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lstTables_AfterLabelEdit);
			this.lstTables.DoubleClick += new System.EventHandler(this.lstTables_DoubleClick);
			this.lstTables.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstTables_MouseUp);
			this.lstTables.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstTables_MouseDown);
			this.lstTables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstTables_KeyDown);
			// 
			// largeImages
			// 
			this.largeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("largeImages.ImageStream")));
			this.largeImages.TransparentColor = System.Drawing.Color.Transparent;
			this.largeImages.Images.SetKeyName(0, "TableHSLarge.png");
			this.largeImages.Images.SetKeyName(1, "NewTableLarge.png");
			// 
			// smallImages
			// 
			this.smallImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smallImages.ImageStream")));
			this.smallImages.TransparentColor = System.Drawing.Color.Transparent;
			this.smallImages.Images.SetKeyName(0, "TableHS.png");
			this.smallImages.Images.SetKeyName(1, "NewTable.png");
			// 
			// tabs
			// 
			this.tabs.ContextMenuStrip = this.tabContextMenu;
			this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabs.Location = new System.Drawing.Point(0, 0);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(441, 538);
			this.tabs.TabIndex = 7;
			this.tabs.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tabs_MouseUp);
			// 
			// tabContextMenu
			// 
			this.tabContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeTabToolStripMenuItem});
			this.tabContextMenu.Name = "tabContextMenu";
			this.tabContextMenu.Size = new System.Drawing.Size(104, 26);
			// 
			// closeTabToolStripMenuItem
			// 
			this.closeTabToolStripMenuItem.Name = "closeTabToolStripMenuItem";
			this.closeTabToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.closeTabToolStripMenuItem.Text = "Close";
			this.closeTabToolStripMenuItem.Click += new System.EventHandler(this.closeTabToolStripMenuItem_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.lstTables);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tabs);
			this.splitContainer1.Size = new System.Drawing.Size(617, 538);
			this.splitContainer1.SplitterDistance = 172;
			this.splitContainer1.TabIndex = 8;
			// 
			// lvContextMenu
			// 
			this.lvContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem});
			this.lvContextMenu.Name = "lvContextMenu";
			this.lvContextMenu.Size = new System.Drawing.Size(100, 26);
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.largeIconsToolStripMenuItem,
            this.smallIconsToolStripMenuItem,
            this.listToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
			this.viewToolStripMenuItem.Text = "View";
			// 
			// largeIconsToolStripMenuItem
			// 
			this.largeIconsToolStripMenuItem.Name = "largeIconsToolStripMenuItem";
			this.largeIconsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.largeIconsToolStripMenuItem.Text = "Large Icons";
			this.largeIconsToolStripMenuItem.Click += new System.EventHandler(this.largeIconsToolStripMenuItem_Click);
			// 
			// smallIconsToolStripMenuItem
			// 
			this.smallIconsToolStripMenuItem.Checked = true;
			this.smallIconsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.smallIconsToolStripMenuItem.Name = "smallIconsToolStripMenuItem";
			this.smallIconsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.smallIconsToolStripMenuItem.Text = "Small Icons";
			this.smallIconsToolStripMenuItem.Click += new System.EventHandler(this.smallIconsToolStripMenuItem_Click);
			// 
			// listToolStripMenuItem
			// 
			this.listToolStripMenuItem.Name = "listToolStripMenuItem";
			this.listToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.listToolStripMenuItem.Text = "List";
			this.listToolStripMenuItem.Click += new System.EventHandler(this.listToolStripMenuItem_Click);
			// 
			// tableContextMenu
			// 
			this.tableContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.editColumnsToolStripMenuItem,
            this.toolStripSeparator1,
            this.renameToolStripMenuItem,
            this.duplicateToolStripMenuItem,
            this.toolStripSeparator2,
            this.deleteToolStripMenuItem});
			this.tableContextMenu.Name = "tableContextMenu";
			this.tableContextMenu.Size = new System.Drawing.Size(155, 148);
			// 
			// renameToolStripMenuItem
			// 
			this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
			this.renameToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.renameToolStripMenuItem.Text = "Rename";
			this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
			// 
			// duplicateToolStripMenuItem
			// 
			this.duplicateToolStripMenuItem.Name = "duplicateToolStripMenuItem";
			this.duplicateToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.duplicateToolStripMenuItem.Text = "Duplicate";
			this.duplicateToolStripMenuItem.Click += new System.EventHandler(this.duplicateToolStripMenuItem_Click);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
			// 
			// editColumnsToolStripMenuItem
			// 
			this.editColumnsToolStripMenuItem.Name = "editColumnsToolStripMenuItem";
			this.editColumnsToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.editColumnsToolStripMenuItem.Text = "Edit Columns...";
			this.editColumnsToolStripMenuItem.Click += new System.EventHandler(this.editColumnsToolStripMenuItem_Click);
			// 
			// DatabaseEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Name = "DatabaseEditor";
			this.Size = new System.Drawing.Size(617, 538);
			this.tabContextMenu.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.lvContextMenu.ResumeLayout(false);
			this.tableContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lstTables;
		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ImageList smallImages;
		private System.Windows.Forms.ImageList largeImages;
		private System.Windows.Forms.ContextMenuStrip lvContextMenu;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem largeIconsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem smallIconsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem listToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip tableContextMenu;
		private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip tabContextMenu;
		private System.Windows.Forms.ToolStripMenuItem closeTabToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem duplicateToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem editColumnsToolStripMenuItem;
	}
}
