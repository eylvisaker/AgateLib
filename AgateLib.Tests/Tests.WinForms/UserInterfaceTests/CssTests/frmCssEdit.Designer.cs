namespace AgateLib.Tests.UserInterfaceTests.CssTests
{
	partial class frmCssEdit
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
			this.artGuiTest = new AgateLib.Platform.WinForms.Controls.AgateRenderTarget();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.panel1 = new System.Windows.Forms.Panel();
			this.cboPropertyItems = new System.Windows.Forms.ComboBox();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.lblFrames = new System.Windows.Forms.Label();
			this.txtCss = new System.Windows.Forms.TextBox();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.btnHideShow = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.artGuiTest);
			this.splitContainer1.Size = new System.Drawing.Size(926, 590);
			this.splitContainer1.SplitterDistance = 472;
			this.splitContainer1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnHideShow);
			this.panel1.Controls.Add(this.cboPropertyItems);
			this.panel1.Controls.Add(this.propertyGrid1);
			this.panel1.Controls.Add(this.lblFrames);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(472, 291);
			this.panel1.TabIndex = 1;
			// 
			// cboPropertyItems
			// 
			this.cboPropertyItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPropertyItems.FormattingEnabled = true;
			this.cboPropertyItems.Location = new System.Drawing.Point(12, 47);
			this.cboPropertyItems.Name = "cboPropertyItems";
			this.cboPropertyItems.Size = new System.Drawing.Size(454, 21);
			this.cboPropertyItems.TabIndex = 2;
			this.cboPropertyItems.SelectedIndexChanged += new System.EventHandler(this.cboPropertyItems_SelectedIndexChanged);
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.propertyGrid1.Location = new System.Drawing.Point(12, 74);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(457, 208);
			this.propertyGrid1.TabIndex = 1;
			// 
			// lblFrames
			// 
			this.lblFrames.AutoSize = true;
			this.lblFrames.Location = new System.Drawing.Point(12, 15);
			this.lblFrames.Name = "lblFrames";
			this.lblFrames.Size = new System.Drawing.Size(35, 13);
			this.lblFrames.TabIndex = 0;
			this.lblFrames.Text = "label1";
			// 
			// txtCss
			// 
			this.txtCss.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtCss.Location = new System.Drawing.Point(0, 0);
			this.txtCss.Multiline = true;
			this.txtCss.Name = "txtCss";
			this.txtCss.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtCss.Size = new System.Drawing.Size(472, 295);
			this.txtCss.TabIndex = 0;
			this.txtCss.TextChanged += new System.EventHandler(this.txtCss_TextChanged);
			// 
			// artGuiTest
			// 
			this.artGuiTest.Dock = System.Windows.Forms.DockStyle.Fill;
			this.artGuiTest.Location = new System.Drawing.Point(0, 0);
			this.artGuiTest.Name = "artGuiTest";
			this.artGuiTest.Size = new System.Drawing.Size(450, 590);
			this.artGuiTest.TabIndex = 0;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.txtCss);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.panel1);
			this.splitContainer2.Size = new System.Drawing.Size(472, 590);
			this.splitContainer2.SplitterDistance = 295;
			this.splitContainer2.TabIndex = 2;
			// 
			// btnHideShow
			// 
			this.btnHideShow.Location = new System.Drawing.Point(391, 10);
			this.btnHideShow.Name = "btnHideShow";
			this.btnHideShow.Size = new System.Drawing.Size(75, 23);
			this.btnHideShow.TabIndex = 3;
			this.btnHideShow.Text = "Hide/Show";
			this.btnHideShow.UseVisualStyleBackColor = true;
			this.btnHideShow.Click += new System.EventHandler(this.btnHideShow_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(926, 590);
			this.Controls.Add(this.splitContainer1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private AgateLib.Platform.WinForms.Controls.AgateRenderTarget artGuiTest;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TextBox txtCss;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblFrames;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.ComboBox cboPropertyItems;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.Button btnHideShow;
	}
}

