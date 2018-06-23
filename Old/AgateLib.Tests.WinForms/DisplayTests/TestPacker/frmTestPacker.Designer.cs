// The contents of this file are public domain.
// You may use them as you wish.
//
namespace AgateLib.Tests.DisplayTests.TestPacker
{
	partial class frmTestPacker
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.btnMany = new System.Windows.Forms.Button();
			this.btnOne = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			this.btnLotsSorted = new System.Windows.Forms.Button();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanel2 = new System.Windows.Forms.StatusBarPanel();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(512, 384);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// btnMany
			// 
			this.btnMany.Location = new System.Drawing.Point(257, 388);
			this.btnMany.Name = "btnMany";
			this.btnMany.Size = new System.Drawing.Size(93, 23);
			this.btnMany.TabIndex = 1;
			this.btnMany.Text = "Add Lots";
			this.btnMany.UseVisualStyleBackColor = true;
			this.btnMany.Click += new System.EventHandler(this.btnMany_Click);
			// 
			// btnOne
			// 
			this.btnOne.Location = new System.Drawing.Point(356, 388);
			this.btnOne.Name = "btnOne";
			this.btnOne.Size = new System.Drawing.Size(75, 23);
			this.btnOne.TabIndex = 2;
			this.btnOne.Text = "Add One";
			this.btnOne.UseVisualStyleBackColor = true;
			this.btnOne.Click += new System.EventHandler(this.btnOne_Click);
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(437, 388);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(75, 23);
			this.btnClear.TabIndex = 4;
			this.btnClear.Text = "Clear";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnLotsSorted
			// 
			this.btnLotsSorted.Location = new System.Drawing.Point(121, 388);
			this.btnLotsSorted.Name = "btnLotsSorted";
			this.btnLotsSorted.Size = new System.Drawing.Size(130, 23);
			this.btnLotsSorted.TabIndex = 5;
			this.btnLotsSorted.Text = "Add Lots (Sorted)";
			this.btnLotsSorted.UseVisualStyleBackColor = true;
			this.btnLotsSorted.Click += new System.EventHandler(this.btnLotsSorted_Click);
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 414);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1,
            this.statusBarPanel2});
			this.statusBar1.ShowPanels = true;
			this.statusBar1.Size = new System.Drawing.Size(507, 22);
			this.statusBar1.TabIndex = 6;
			this.statusBar1.Text = "statusBar1";
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.Name = "statusBarPanel1";
			this.statusBarPanel1.Text = "statusBarPanel1";
			this.statusBarPanel1.Width = 200;
			// 
			// statusBarPanel2
			// 
			this.statusBarPanel2.Name = "statusBarPanel2";
			this.statusBarPanel2.Text = "statusBarPanel2";
			this.statusBarPanel2.Width = 200;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(507, 436);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.btnLotsSorted);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.btnOne);
			this.Controls.Add(this.btnMany);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button btnMany;
		private System.Windows.Forms.Button btnOne;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Button btnLotsSorted;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel2;
	}
}

