namespace AgateDatabaseEditor
{
	partial class frmImportTable
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
			this.txtFileContents = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.hSeparator1 = new ERY.NotebookLib.HSeparator();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkMergeDelimiters = new System.Windows.Forms.CheckBox();
			this.txtOther = new System.Windows.Forms.TextBox();
			this.chkOther = new System.Windows.Forms.CheckBox();
			this.chkSpace = new System.Windows.Forms.CheckBox();
			this.chkTab = new System.Windows.Forms.CheckBox();
			this.chkSemicolon = new System.Windows.Forms.CheckBox();
			this.chkComma = new System.Windows.Forms.CheckBox();
			this.cboTextQualifier = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.chkFirstRow = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.pnlTableWarning = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lstColumns = new System.Windows.Forms.ListBox();
			this.label4 = new System.Windows.Forms.Label();
			this.propColumns = new System.Windows.Forms.PropertyGrid();
			this.panel1 = new System.Windows.Forms.Panel();
			this.chkOverwrite = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.pnlTableWarning.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtFileContents
			// 
			this.txtFileContents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFileContents.Location = new System.Drawing.Point(12, 93);
			this.txtFileContents.Multiline = true;
			this.txtFileContents.Name = "txtFileContents";
			this.txtFileContents.ReadOnly = true;
			this.txtFileContents.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtFileContents.Size = new System.Drawing.Size(533, 138);
			this.txtFileContents.TabIndex = 0;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(389, 512);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(470, 512);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// hSeparator1
			// 
			this.hSeparator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.hSeparator1.Location = new System.Drawing.Point(12, 235);
			this.hSeparator1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
			this.hSeparator1.MaximumSize = new System.Drawing.Size(10000, 4);
			this.hSeparator1.MinimumSize = new System.Drawing.Size(0, 4);
			this.hSeparator1.Name = "hSeparator1";
			this.hSeparator1.Size = new System.Drawing.Size(533, 4);
			this.hSeparator1.TabIndex = 3;
			this.hSeparator1.TabStop = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkMergeDelimiters);
			this.groupBox1.Controls.Add(this.txtOther);
			this.groupBox1.Controls.Add(this.chkOther);
			this.groupBox1.Controls.Add(this.chkSpace);
			this.groupBox1.Controls.Add(this.chkTab);
			this.groupBox1.Controls.Add(this.chkSemicolon);
			this.groupBox1.Controls.Add(this.chkComma);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(354, 75);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Delimiters";
			// 
			// chkMergeDelimiters
			// 
			this.chkMergeDelimiters.AutoSize = true;
			this.chkMergeDelimiters.Location = new System.Drawing.Point(226, 48);
			this.chkMergeDelimiters.Name = "chkMergeDelimiters";
			this.chkMergeDelimiters.Size = new System.Drawing.Size(104, 17);
			this.chkMergeDelimiters.TabIndex = 15;
			this.chkMergeDelimiters.Text = "Merge Delimiters";
			this.chkMergeDelimiters.UseVisualStyleBackColor = true;
			this.chkMergeDelimiters.CheckedChanged += new System.EventHandler(this.chkMergeDelimiters_CheckedChanged);
			// 
			// txtOther
			// 
			this.txtOther.Location = new System.Drawing.Point(287, 23);
			this.txtOther.MaxLength = 1;
			this.txtOther.Name = "txtOther";
			this.txtOther.Size = new System.Drawing.Size(23, 20);
			this.txtOther.TabIndex = 5;
			this.txtOther.TextChanged += new System.EventHandler(this.DelimiterCheck_CheckedChanged);
			// 
			// chkOther
			// 
			this.chkOther.AutoSize = true;
			this.chkOther.Location = new System.Drawing.Point(226, 25);
			this.chkOther.Name = "chkOther";
			this.chkOther.Size = new System.Drawing.Size(55, 17);
			this.chkOther.TabIndex = 4;
			this.chkOther.Text = "Other:";
			this.chkOther.UseVisualStyleBackColor = true;
			this.chkOther.CheckedChanged += new System.EventHandler(this.DelimiterCheck_CheckedChanged);
			// 
			// chkSpace
			// 
			this.chkSpace.AutoSize = true;
			this.chkSpace.Location = new System.Drawing.Point(113, 48);
			this.chkSpace.Name = "chkSpace";
			this.chkSpace.Size = new System.Drawing.Size(57, 17);
			this.chkSpace.TabIndex = 3;
			this.chkSpace.Text = "Space";
			this.chkSpace.UseVisualStyleBackColor = true;
			this.chkSpace.CheckedChanged += new System.EventHandler(this.DelimiterCheck_CheckedChanged);
			// 
			// chkTab
			// 
			this.chkTab.AutoSize = true;
			this.chkTab.Location = new System.Drawing.Point(17, 48);
			this.chkTab.Name = "chkTab";
			this.chkTab.Size = new System.Drawing.Size(45, 17);
			this.chkTab.TabIndex = 2;
			this.chkTab.Text = "Tab";
			this.chkTab.UseVisualStyleBackColor = true;
			this.chkTab.CheckedChanged += new System.EventHandler(this.DelimiterCheck_CheckedChanged);
			// 
			// chkSemicolon
			// 
			this.chkSemicolon.AutoSize = true;
			this.chkSemicolon.Location = new System.Drawing.Point(113, 25);
			this.chkSemicolon.Name = "chkSemicolon";
			this.chkSemicolon.Size = new System.Drawing.Size(75, 17);
			this.chkSemicolon.TabIndex = 1;
			this.chkSemicolon.Text = "Semicolon";
			this.chkSemicolon.UseVisualStyleBackColor = true;
			this.chkSemicolon.CheckedChanged += new System.EventHandler(this.DelimiterCheck_CheckedChanged);
			// 
			// chkComma
			// 
			this.chkComma.AutoSize = true;
			this.chkComma.Checked = true;
			this.chkComma.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkComma.Location = new System.Drawing.Point(17, 25);
			this.chkComma.Name = "chkComma";
			this.chkComma.Size = new System.Drawing.Size(61, 17);
			this.chkComma.TabIndex = 0;
			this.chkComma.Text = "Comma";
			this.chkComma.UseVisualStyleBackColor = true;
			this.chkComma.CheckedChanged += new System.EventHandler(this.DelimiterCheck_CheckedChanged);
			// 
			// cboTextQualifier
			// 
			this.cboTextQualifier.FormattingEnabled = true;
			this.cboTextQualifier.Items.AddRange(new object[] {
            "{none}",
            "\'",
            "\""});
			this.cboTextQualifier.Location = new System.Drawing.Point(456, 22);
			this.cboTextQualifier.Name = "cboTextQualifier";
			this.cboTextQualifier.Size = new System.Drawing.Size(61, 21);
			this.cboTextQualifier.TabIndex = 5;
			this.cboTextQualifier.Text = "\"";
			this.cboTextQualifier.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(378, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Text Qualifier:";
			// 
			// backgroundWorker1
			// 
			this.backgroundWorker1.WorkerSupportsCancellation = true;
			this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			// 
			// chkFirstRow
			// 
			this.chkFirstRow.AutoSize = true;
			this.chkFirstRow.Checked = true;
			this.chkFirstRow.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkFirstRow.Location = new System.Drawing.Point(381, 60);
			this.chkFirstRow.Name = "chkFirstRow";
			this.chkFirstRow.Size = new System.Drawing.Size(164, 17);
			this.chkFirstRow.TabIndex = 7;
			this.chkFirstRow.Text = "First row contains field names";
			this.chkFirstRow.UseVisualStyleBackColor = true;
			this.chkFirstRow.CheckedChanged += new System.EventHandler(this.chkFirstRow_CheckedChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 255);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 13);
			this.label2.TabIndex = 8;
			this.label2.Text = "Table Name:";
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(86, 252);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(157, 20);
			this.txtName.TabIndex = 9;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// pnlTableWarning
			// 
			this.pnlTableWarning.Controls.Add(this.chkOverwrite);
			this.pnlTableWarning.Controls.Add(this.label3);
			this.pnlTableWarning.Controls.Add(this.pictureBox1);
			this.pnlTableWarning.Location = new System.Drawing.Point(250, 243);
			this.pnlTableWarning.Name = "pnlTableWarning";
			this.pnlTableWarning.Size = new System.Drawing.Size(295, 58);
			this.pnlTableWarning.TabIndex = 10;
			this.pnlTableWarning.Visible = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(27, 6);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(226, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "There is already a table by the specified name.";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::AgateDatabaseEditor.Properties.Resources.warning;
			this.pictureBox1.Location = new System.Drawing.Point(5, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(16, 16);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// lstColumns
			// 
			this.lstColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lstColumns.Enabled = false;
			this.lstColumns.FormattingEnabled = true;
			this.lstColumns.Location = new System.Drawing.Point(3, 16);
			this.lstColumns.Name = "lstColumns";
			this.lstColumns.Size = new System.Drawing.Size(120, 186);
			this.lstColumns.TabIndex = 11;
			this.lstColumns.SelectedIndexChanged += new System.EventHandler(this.lstColumns_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(47, 13);
			this.label4.TabIndex = 12;
			this.label4.Text = "Columns";
			// 
			// propColumns
			// 
			this.propColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.propColumns.CommandsVisibleIfAvailable = false;
			this.propColumns.Enabled = false;
			this.propColumns.HelpVisible = false;
			this.propColumns.Location = new System.Drawing.Point(129, 16);
			this.propColumns.Name = "propColumns";
			this.propColumns.Size = new System.Drawing.Size(401, 196);
			this.propColumns.TabIndex = 13;
			this.propColumns.Click += new System.EventHandler(this.propColumns_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.propColumns);
			this.panel1.Controls.Add(this.lstColumns);
			this.panel1.Location = new System.Drawing.Point(12, 291);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(533, 215);
			this.panel1.TabIndex = 14;
			// 
			// chkOverwrite
			// 
			this.chkOverwrite.AutoSize = true;
			this.chkOverwrite.Location = new System.Drawing.Point(32, 25);
			this.chkOverwrite.Name = "chkOverwrite";
			this.chkOverwrite.Size = new System.Drawing.Size(183, 17);
			this.chkOverwrite.TabIndex = 2;
			this.chkOverwrite.Text = "I know, and I want to overwrite it.";
			this.chkOverwrite.UseVisualStyleBackColor = true;
			this.chkOverwrite.CheckedChanged += new System.EventHandler(this.chkOverwrite_CheckedChanged);
			// 
			// frmImportTable
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(557, 547);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.pnlTableWarning);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.chkFirstRow);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cboTextQualifier);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.hSeparator1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.txtFileContents);
			this.Name = "frmImportTable";
			this.Text = "Import Data";
			this.Load += new System.EventHandler(this.frmImportData_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.pnlTableWarning.ResumeLayout(false);
			this.pnlTableWarning.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtFileContents;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private ERY.NotebookLib.HSeparator hSeparator1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtOther;
		private System.Windows.Forms.CheckBox chkOther;
		private System.Windows.Forms.CheckBox chkSpace;
		private System.Windows.Forms.CheckBox chkTab;
		private System.Windows.Forms.CheckBox chkSemicolon;
		private System.Windows.Forms.CheckBox chkComma;
		private System.Windows.Forms.ComboBox cboTextQualifier;
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.Windows.Forms.CheckBox chkFirstRow;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Panel pnlTableWarning;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListBox lstColumns;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.PropertyGrid propColumns;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox chkMergeDelimiters;
		private System.Windows.Forms.CheckBox chkOverwrite;
	}
}