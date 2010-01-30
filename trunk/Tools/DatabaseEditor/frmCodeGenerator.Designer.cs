namespace DatabaseEditor
{
	partial class frmCodeGenerator
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
			this.txtDirectory = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtNamespace = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cboCodeProvider = new System.Windows.Forms.ComboBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.SuspendLayout();
			// 
			// txtDirectory
			// 
			this.txtDirectory.Location = new System.Drawing.Point(102, 25);
			this.txtDirectory.Name = "txtDirectory";
			this.txtDirectory.Size = new System.Drawing.Size(264, 20);
			this.txtDirectory.TabIndex = 0;
			this.txtDirectory.TextChanged += new System.EventHandler(this.txtDirectory_TextChanged);
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(278, 51);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(88, 23);
			this.btnBrowse.TabIndex = 1;
			this.btnBrowse.Text = "Browse...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Output Directory";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(32, 97);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Namespace";
			// 
			// txtNamespace
			// 
			this.txtNamespace.Location = new System.Drawing.Point(102, 94);
			this.txtNamespace.Name = "txtNamespace";
			this.txtNamespace.Size = new System.Drawing.Size(264, 20);
			this.txtNamespace.TabIndex = 4;
			this.txtNamespace.TextChanged += new System.EventHandler(this.txtNamespace_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(22, 134);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(74, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Code Provider";
			// 
			// cboCodeProvider
			// 
			this.cboCodeProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCodeProvider.FormattingEnabled = true;
			this.cboCodeProvider.Location = new System.Drawing.Point(102, 131);
			this.cboCodeProvider.Name = "cboCodeProvider";
			this.cboCodeProvider.Size = new System.Drawing.Size(264, 21);
			this.cboCodeProvider.TabIndex = 6;
			this.cboCodeProvider.SelectedIndexChanged += new System.EventHandler(this.cboCodeProvider_SelectedIndexChanged);
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(212, 182);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(74, 23);
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(292, 182);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(74, 23);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// frmCodeGenerator
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(378, 217);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.cboCodeProvider);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtNamespace);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtDirectory);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmCodeGenerator";
			this.Text = "Generate Code";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtDirectory;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtNamespace;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cboCodeProvider;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.FolderBrowserDialog folderBrowser;
	}
}