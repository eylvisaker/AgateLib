namespace Tests
{
	partial class frmLauncher
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
			this.lstTests = new System.Windows.Forms.ListBox();
			this.displayList = new System.Windows.Forms.ComboBox();
			this.inputList = new System.Windows.Forms.ComboBox();
			this.audioList = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lstTests
			// 
			this.lstTests.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstTests.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lstTests.FormattingEnabled = true;
			this.lstTests.Location = new System.Drawing.Point(324, 12);
			this.lstTests.MultiColumn = true;
			this.lstTests.Name = "lstTests";
			this.lstTests.Size = new System.Drawing.Size(349, 472);
			this.lstTests.TabIndex = 0;
			this.lstTests.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstTests_DrawItem);
			this.lstTests.DoubleClick += new System.EventHandler(this.lstTests_DoubleClick);
			this.lstTests.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstTests_KeyUp);
			// 
			// displayList
			// 
			this.displayList.DisplayMember = "FriendlyName";
			this.displayList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.displayList.FormattingEnabled = true;
			this.displayList.Location = new System.Drawing.Point(70, 12);
			this.displayList.Name = "displayList";
			this.displayList.Size = new System.Drawing.Size(229, 21);
			this.displayList.TabIndex = 8;
			this.displayList.SelectedIndexChanged += new System.EventHandler(this.displayList_SelectedIndexChanged);
			// 
			// inputList
			// 
			this.inputList.DisplayMember = "FriendlyName";
			this.inputList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.inputList.FormattingEnabled = true;
			this.inputList.Location = new System.Drawing.Point(70, 66);
			this.inputList.Name = "inputList";
			this.inputList.Size = new System.Drawing.Size(229, 21);
			this.inputList.TabIndex = 10;
			this.inputList.SelectedIndexChanged += new System.EventHandler(this.inputList_SelectedIndexChanged);
			// 
			// audioList
			// 
			this.audioList.DisplayMember = "FriendlyName";
			this.audioList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.audioList.FormattingEnabled = true;
			this.audioList.Location = new System.Drawing.Point(70, 39);
			this.audioList.Name = "audioList";
			this.audioList.Size = new System.Drawing.Size(229, 21);
			this.audioList.TabIndex = 12;
			this.audioList.SelectedIndexChanged += new System.EventHandler(this.audioList_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(9, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(55, 18);
			this.label1.TabIndex = 9;
			this.label1.Text = "Display";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 69);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 18);
			this.label2.TabIndex = 11;
			this.label2.Text = "Input";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(23, 42);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(41, 18);
			this.label3.TabIndex = 13;
			this.label3.Text = "Audio";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// frmLauncher
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(685, 494);
			this.Controls.Add(this.lstTests);
			this.Controls.Add(this.inputList);
			this.Controls.Add(this.audioList);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.displayList);
			this.Name = "frmLauncher";
			this.Text = "AgateLib Test Launcher";
			this.Load += new System.EventHandler(this.frmLauncher_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lstTests;
		private System.Windows.Forms.ComboBox displayList;
		private System.Windows.Forms.ComboBox inputList;
		private System.Windows.Forms.ComboBox audioList;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
	}
}