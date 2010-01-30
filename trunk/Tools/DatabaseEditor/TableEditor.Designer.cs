namespace AgateDatabaseEditor
{
	partial class TableEditor
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
			this.gridView = new System.Windows.Forms.DataGridView();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.editColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// gridView
			// 
			this.gridView.AllowUserToOrderColumns = true;
			this.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridView.ContextMenuStrip = this.contextMenuStrip1;
			this.gridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridView.Location = new System.Drawing.Point(0, 0);
			this.gridView.Name = "gridView";
			this.gridView.Size = new System.Drawing.Size(150, 150);
			this.gridView.TabIndex = 8;
			this.gridView.VirtualMode = true;
			this.gridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.gridView_UserDeletingRow);
			this.gridView.CancelRowEdit += new System.Windows.Forms.QuestionEventHandler(this.gridView_CancelRowEdit);
			this.gridView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.gridView_CellValueNeeded);
			this.gridView.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridView_RowValidated);
			this.gridView.RowDirtyStateNeeded += new System.Windows.Forms.QuestionEventHandler(this.gridView_RowDirtyStateNeeded);
			this.gridView.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.gridView_CellValuePushed);
			this.gridView.NewRowNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.gridView_NewRowNeeded);
			this.gridView.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.gridView_ColumnWidthChanged);
			this.gridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridView_CellContentClick);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editColumnsToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(155, 26);
			// 
			// editColumnsToolStripMenuItem
			// 
			this.editColumnsToolStripMenuItem.Name = "editColumnsToolStripMenuItem";
			this.editColumnsToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.editColumnsToolStripMenuItem.Text = "Edit Columns...";
			this.editColumnsToolStripMenuItem.Click += new System.EventHandler(this.editColumnsToolStripMenuItem_Click);
			// 
			// TableEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridView);
			this.Name = "TableEditor";
			((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView gridView;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem editColumnsToolStripMenuItem;
	}
}
