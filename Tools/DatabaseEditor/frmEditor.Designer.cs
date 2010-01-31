namespace AgateDatabaseEditor
{
	partial class frmEditor
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditor));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveDatabaseAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.importDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.generateCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openDatabase = new System.Windows.Forms.OpenFileDialog();
			this.saveDatabase = new System.Windows.Forms.SaveFileDialog();
			this.mainToolStrip = new System.Windows.Forms.ToolStrip();
			this.btnNew = new System.Windows.Forms.ToolStripButton();
			this.btnOpen = new System.Windows.Forms.ToolStripButton();
			this.btnSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
			this.databaseEditor1 = new AgateDatabaseEditor.DatabaseEditor();
			this.tableToolStrip = new System.Windows.Forms.ToolStrip();
			this.btnDesignTable = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.btnSortAscending = new System.Windows.Forms.ToolStripButton();
			this.btnSortDescending = new System.Windows.Forms.ToolStripButton();
			this.statusStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.mainToolStrip.SuspendLayout();
			this.toolStripContainer2.BottomToolStripPanel.SuspendLayout();
			this.toolStripContainer2.ContentPanel.SuspendLayout();
			this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer2.SuspendLayout();
			this.tableToolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 0);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(862, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statusLabel
			// 
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(118, 17);
			this.statusLabel.Text = "toolStripStatusLabel1";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(862, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDatabaseToolStripMenuItem,
            this.openDatabaseToolStripMenuItem,
            this.saveDatabaseToolStripMenuItem,
            this.saveDatabaseAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// newDatabaseToolStripMenuItem
			// 
			this.newDatabaseToolStripMenuItem.Name = "newDatabaseToolStripMenuItem";
			this.newDatabaseToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
			this.newDatabaseToolStripMenuItem.Text = "&New Database";
			this.newDatabaseToolStripMenuItem.Click += new System.EventHandler(this.newDatabaseToolStripMenuItem_Click);
			// 
			// openDatabaseToolStripMenuItem
			// 
			this.openDatabaseToolStripMenuItem.Name = "openDatabaseToolStripMenuItem";
			this.openDatabaseToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
			this.openDatabaseToolStripMenuItem.Text = "&Open Database...";
			this.openDatabaseToolStripMenuItem.Click += new System.EventHandler(this.openDatabaseToolStripMenuItem_Click);
			// 
			// saveDatabaseToolStripMenuItem
			// 
			this.saveDatabaseToolStripMenuItem.Name = "saveDatabaseToolStripMenuItem";
			this.saveDatabaseToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
			this.saveDatabaseToolStripMenuItem.Text = "&Save Database";
			this.saveDatabaseToolStripMenuItem.Click += new System.EventHandler(this.saveDatabaseToolStripMenuItem_Click);
			// 
			// saveDatabaseAsToolStripMenuItem
			// 
			this.saveDatabaseAsToolStripMenuItem.Name = "saveDatabaseAsToolStripMenuItem";
			this.saveDatabaseAsToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
			this.saveDatabaseAsToolStripMenuItem.Text = "S&ave database as...";
			this.saveDatabaseAsToolStripMenuItem.Click += new System.EventHandler(this.saveDatabaseAsToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(168, 6);
			// 
			// quitToolStripMenuItem
			// 
			this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
			this.quitToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
			this.quitToolStripMenuItem.Text = "Quit";
			this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importDataToolStripMenuItem,
            this.toolStripSeparator2,
            this.generateCodeToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem.Text = "Tools";
			// 
			// importDataToolStripMenuItem
			// 
			this.importDataToolStripMenuItem.Name = "importDataToolStripMenuItem";
			this.importDataToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.importDataToolStripMenuItem.Text = "Import Data...";
			this.importDataToolStripMenuItem.Click += new System.EventHandler(this.importDataToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
			// 
			// generateCodeToolStripMenuItem
			// 
			this.generateCodeToolStripMenuItem.Name = "generateCodeToolStripMenuItem";
			this.generateCodeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.generateCodeToolStripMenuItem.Text = "Generate Code";
			this.generateCodeToolStripMenuItem.Click += new System.EventHandler(this.generateCodeToolStripMenuItem_Click);
			// 
			// openDatabase
			// 
			this.openDatabase.Filter = "Agate Database|*.adb|All files|*.*";
			// 
			// saveDatabase
			// 
			this.saveDatabase.DefaultExt = "adb";
			this.saveDatabase.Filter = "Agate Database (*.adb)|*.adb|All files|*.*";
			// 
			// mainToolStrip
			// 
			this.mainToolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnOpen,
            this.btnSave});
			this.mainToolStrip.Location = new System.Drawing.Point(3, 24);
			this.mainToolStrip.Name = "mainToolStrip";
			this.mainToolStrip.Size = new System.Drawing.Size(81, 25);
			this.mainToolStrip.TabIndex = 4;
			this.mainToolStrip.Text = "toolStrip1";
			// 
			// btnNew
			// 
			this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnNew.Image = global::AgateDatabaseEditor.Properties.Resources.NewDocumentHS;
			this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(23, 22);
			this.btnNew.Text = "toolStripButton1";
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// btnOpen
			// 
			this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
			this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(23, 22);
			this.btnOpen.Text = "toolStripButton2";
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// btnSave
			// 
			this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
			this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(23, 22);
			this.btnSave.Text = "toolStripButton3";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// toolStripContainer2
			// 
			// 
			// toolStripContainer2.BottomToolStripPanel
			// 
			this.toolStripContainer2.BottomToolStripPanel.Controls.Add(this.statusStrip1);
			// 
			// toolStripContainer2.ContentPanel
			// 
			this.toolStripContainer2.ContentPanel.Controls.Add(this.databaseEditor1);
			this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(862, 583);
			this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer2.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer2.Name = "toolStripContainer2";
			this.toolStripContainer2.Size = new System.Drawing.Size(862, 654);
			this.toolStripContainer2.TabIndex = 6;
			this.toolStripContainer2.Text = "toolStripContainer2";
			// 
			// toolStripContainer2.TopToolStripPanel
			// 
			this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.menuStrip1);
			this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.mainToolStrip);
			this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.tableToolStrip);
			// 
			// databaseEditor1
			// 
			this.databaseEditor1.Database = null;
			this.databaseEditor1.DirtyState = false;
			this.databaseEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.databaseEditor1.Location = new System.Drawing.Point(0, 0);
			this.databaseEditor1.Name = "databaseEditor1";
			this.databaseEditor1.Size = new System.Drawing.Size(862, 583);
			this.databaseEditor1.TabIndex = 3;
			this.databaseEditor1.Visible = false;
			this.databaseEditor1.DirtyStateChanged += new System.EventHandler(this.databaseEditor1_DirtyStateChanged);
			this.databaseEditor1.StatusText += new System.EventHandler<AgateDatabaseEditor.StatusTextEventArgs>(this.databaseEditor1_StatusText);
			this.databaseEditor1.TableActiveStatusChanged += new System.EventHandler(this.databaseEditor1_TableActiveStatusChanged);
			// 
			// tableToolStrip
			// 
			this.tableToolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.tableToolStrip.Enabled = false;
			this.tableToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnDesignTable,
            this.toolStripSeparator3,
            this.toolStripLabel1,
            this.btnSortAscending,
            this.btnSortDescending});
			this.tableToolStrip.Location = new System.Drawing.Point(84, 24);
			this.tableToolStrip.Name = "tableToolStrip";
			this.tableToolStrip.Size = new System.Drawing.Size(177, 25);
			this.tableToolStrip.TabIndex = 10;
			this.tableToolStrip.Text = "toolStrip1";
			// 
			// btnDesignTable
			// 
			this.btnDesignTable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnDesignTable.Image = ((System.Drawing.Image)(resources.GetObject("btnDesignTable.Image")));
			this.btnDesignTable.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnDesignTable.Name = "btnDesignTable";
			this.btnDesignTable.Size = new System.Drawing.Size(82, 22);
			this.btnDesignTable.Text = "Edit Columns";
			this.btnDesignTable.Click += new System.EventHandler(this.btnDesignTable_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(31, 22);
			this.toolStripLabel1.Text = "Sort:";
			// 
			// btnSortAscending
			// 
			this.btnSortAscending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSortAscending.Image = ((System.Drawing.Image)(resources.GetObject("btnSortAscending.Image")));
			this.btnSortAscending.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSortAscending.Name = "btnSortAscending";
			this.btnSortAscending.Size = new System.Drawing.Size(23, 22);
			this.btnSortAscending.Text = "toolStripButton1";
			this.btnSortAscending.Click += new System.EventHandler(this.btnSortAscending_Click);
			// 
			// btnSortDescending
			// 
			this.btnSortDescending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSortDescending.Image = ((System.Drawing.Image)(resources.GetObject("btnSortDescending.Image")));
			this.btnSortDescending.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSortDescending.Name = "btnSortDescending";
			this.btnSortDescending.Size = new System.Drawing.Size(23, 22);
			this.btnSortDescending.Text = "toolStripButton1";
			this.btnSortDescending.Click += new System.EventHandler(this.btnSortDescending_Click);
			// 
			// frmEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(862, 654);
			this.Controls.Add(this.toolStripContainer2);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "frmEditor";
			this.Text = "Agate Database Editor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEditor_FormClosing);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.mainToolStrip.ResumeLayout(false);
			this.mainToolStrip.PerformLayout();
			this.toolStripContainer2.BottomToolStripPanel.ResumeLayout(false);
			this.toolStripContainer2.BottomToolStripPanel.PerformLayout();
			this.toolStripContainer2.ContentPanel.ResumeLayout(false);
			this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer2.TopToolStripPanel.PerformLayout();
			this.toolStripContainer2.ResumeLayout(false);
			this.toolStripContainer2.PerformLayout();
			this.tableToolStrip.ResumeLayout(false);
			this.tableToolStrip.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newDatabaseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openDatabaseToolStripMenuItem;
		private DatabaseEditor databaseEditor1;
		private System.Windows.Forms.OpenFileDialog openDatabase;
		private System.Windows.Forms.ToolStripMenuItem saveDatabaseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveDatabaseAsToolStripMenuItem;
		private System.Windows.Forms.SaveFileDialog saveDatabase;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem generateCodeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importDataToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripStatusLabel statusLabel;
		private System.Windows.Forms.ToolStrip mainToolStrip;
		private System.Windows.Forms.ToolStripButton btnNew;
		private System.Windows.Forms.ToolStripButton btnOpen;
		private System.Windows.Forms.ToolStripButton btnSave;
		private System.Windows.Forms.ToolStripContainer toolStripContainer2;
		private System.Windows.Forms.ToolStrip tableToolStrip;
		private System.Windows.Forms.ToolStripButton btnDesignTable;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripButton btnSortAscending;
		private System.Windows.Forms.ToolStripButton btnSortDescending;
	}
}

