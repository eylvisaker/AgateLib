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
			this.label1 = new System.Windows.Forms.Label();
			this.cboTableLookup = new System.Windows.Forms.ComboBox();
			this.cboDisplayField = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.chkPrimaryKey = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.gridColumns)).BeginInit();
			this.groupBox1.SuspendLayout();
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
			this.gridColumns.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridColumns_RowEnter);
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
			this.groupBox1.Controls.Add(this.chkPrimaryKey);
			this.groupBox1.Controls.Add(this.cboDisplayField);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.cboTableLookup);
			this.groupBox1.Controls.Add(this.label1);
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
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(59, 50);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(73, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Table Lookup";
			// 
			// cboTableLookup
			// 
			this.cboTableLookup.DisplayMember = "Name";
			this.cboTableLookup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTableLookup.FormattingEnabled = true;
			this.cboTableLookup.Location = new System.Drawing.Point(138, 47);
			this.cboTableLookup.Name = "cboTableLookup";
			this.cboTableLookup.Size = new System.Drawing.Size(189, 21);
			this.cboTableLookup.TabIndex = 2;
			this.cboTableLookup.SelectedIndexChanged += new System.EventHandler(this.cboTableLookup_SelectedIndexChanged);
			// 
			// cboDisplayField
			// 
			this.cboDisplayField.DisplayMember = "Name";
			this.cboDisplayField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDisplayField.Enabled = false;
			this.cboDisplayField.FormattingEnabled = true;
			this.cboDisplayField.Location = new System.Drawing.Point(138, 74);
			this.cboDisplayField.Name = "cboDisplayField";
			this.cboDisplayField.Size = new System.Drawing.Size(189, 21);
			this.cboDisplayField.TabIndex = 4;
			this.cboDisplayField.SelectedIndexChanged += new System.EventHandler(this.cboDisplayField_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(27, 77);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(105, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Lookup Display Field";
			// 
			// chkPrimaryKey
			// 
			this.chkPrimaryKey.AutoSize = true;
			this.chkPrimaryKey.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkPrimaryKey.Location = new System.Drawing.Point(72, 24);
			this.chkPrimaryKey.Name = "chkPrimaryKey";
			this.chkPrimaryKey.Size = new System.Drawing.Size(81, 17);
			this.chkPrimaryKey.TabIndex = 5;
			this.chkPrimaryKey.Text = "Primary Key";
			this.chkPrimaryKey.UseVisualStyleBackColor = true;
			this.chkPrimaryKey.CheckedChanged += new System.EventHandler(this.chkPrimaryKey_CheckedChanged);
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
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
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
		private System.Windows.Forms.ComboBox cboTableLookup;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cboDisplayField;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkPrimaryKey;

	}
}