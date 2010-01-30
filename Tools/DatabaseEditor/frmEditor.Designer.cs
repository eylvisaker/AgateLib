namespace DatabaseEditor
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
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
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
			this.databaseEditor1 = new AgateDataLib.DatabaseEditor();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 529);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(632, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(632, 24);
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
			// databaseEditor1
			// 
			this.databaseEditor1.Database = null;
			this.databaseEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.databaseEditor1.Location = new System.Drawing.Point(0, 24);
			this.databaseEditor1.Name = "databaseEditor1";
			this.databaseEditor1.Size = new System.Drawing.Size(632, 505);
			this.databaseEditor1.TabIndex = 3;
			this.databaseEditor1.Visible = false;
			// 
			// frmEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(632, 551);
			this.Controls.Add(this.databaseEditor1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "frmEditor";
			this.Text = "Agate Database Editor";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newDatabaseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openDatabaseToolStripMenuItem;
		private AgateDataLib.DatabaseEditor databaseEditor1;
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
	}
}

