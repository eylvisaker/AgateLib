namespace AgateDatabaseEditor
{
	partial class frmDesignTable
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
			this.gridColumns = new System.Windows.Forms.DataGridView();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnOK = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.gridColumns)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// gridColumns
			// 
			this.gridColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colDataType,
            this.colDescription});
			this.gridColumns.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridColumns.Location = new System.Drawing.Point(0, 0);
			this.gridColumns.Name = "gridColumns";
			this.gridColumns.Size = new System.Drawing.Size(765, 345);
			this.gridColumns.TabIndex = 0;
			this.gridColumns.VirtualMode = true;
			this.gridColumns.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.gridColumns_UserDeletingRow);
			this.gridColumns.CancelRowEdit += new System.Windows.Forms.QuestionEventHandler(this.gridColumns_CancelRowEdit);
			this.gridColumns.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.gridColumns_CellValueNeeded);
			this.gridColumns.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridColumns_RowValidated);
			this.gridColumns.RowDirtyStateNeeded += new System.Windows.Forms.QuestionEventHandler(this.gridColumns_RowDirtyStateNeeded);
			this.gridColumns.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.gridColumns_CellValuePushed);
			this.gridColumns.NewRowNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.gridColumns_NewRowNeeded);
			// 
			// colName
			// 
			this.colName.HeaderText = "Name";
			this.colName.Name = "colName";
			// 
			// colDataType
			// 
			this.colDataType.HeaderText = "Data Type";
			this.colDataType.Name = "colDataType";
			// 
			// colDescription
			// 
			this.colDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colDescription.HeaderText = "Description";
			this.colDescription.Name = "colDescription";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(12, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(741, 183);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Column Properties";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnOK);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 345);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(765, 239);
			this.panel1.TabIndex = 0;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(678, 204);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// frmDesignTable
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(765, 584);
			this.Controls.Add(this.gridColumns);
			this.Controls.Add(this.panel1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmDesignTable";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Table Design";
			((System.ComponentModel.ISupportInitialize)(this.gridColumns)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView gridColumns;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewComboBoxColumn colDataType;
		private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnOK;

	}
}