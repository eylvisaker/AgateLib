//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
namespace AgateLib.WinForms
{
	/// <summary>
	/// Form which allos the user to choose what drivers should be used.
	/// </summary>
	partial class SetSystemsForm
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
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.displayList = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.inputList = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.audioList = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(190, 95);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(54, 23);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(250, 95);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(54, 23);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// displayList
			// 
			this.displayList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.displayList.DisplayMember = "FriendlyName";
			this.displayList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.displayList.FormattingEnabled = true;
			this.displayList.Location = new System.Drawing.Point(76, 12);
			this.displayList.Name = "displayList";
			this.displayList.Size = new System.Drawing.Size(228, 21);
			this.displayList.TabIndex = 8;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 27);
			this.label1.TabIndex = 9;
			this.label1.Text = "Display";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 69);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 30);
			this.label2.TabIndex = 11;
			this.label2.Text = "Input";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// inputList
			// 
			this.inputList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inputList.DisplayMember = "FriendlyName";
			this.inputList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.inputList.FormattingEnabled = true;
			this.inputList.Location = new System.Drawing.Point(76, 66);
			this.inputList.Name = "inputList";
			this.inputList.Size = new System.Drawing.Size(228, 21);
			this.inputList.TabIndex = 10;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 42);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(58, 27);
			this.label3.TabIndex = 13;
			this.label3.Text = "Audio";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// audioList
			// 
			this.audioList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.audioList.DisplayMember = "FriendlyName";
			this.audioList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.audioList.FormattingEnabled = true;
			this.audioList.Location = new System.Drawing.Point(76, 39);
			this.audioList.Name = "audioList";
			this.audioList.Size = new System.Drawing.Size(228, 21);
			this.audioList.TabIndex = 12;
			// 
			// SetSystemsForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(316, 130);
			this.Controls.Add(this.inputList);
			this.Controls.Add(this.audioList);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.displayList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "SetSystemsForm";
			this.Text = "Select Drivers";
			this.Load += new System.EventHandler(this.frmSetSystems_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ComboBox displayList;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox inputList;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox audioList;

	}
}

