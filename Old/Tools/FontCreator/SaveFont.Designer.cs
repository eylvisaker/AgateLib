namespace FontCreatorApp
{
    partial class SaveFont
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
			this.components = new System.ComponentModel.Container();
			this.btnBrowseResource = new System.Windows.Forms.Button();
			this.txtResources = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtImage = new System.Windows.Forms.TextBox();
			this.btnBrowseImage = new System.Windows.Forms.Button();
			this.dialogResources = new System.Windows.Forms.SaveFileDialog();
			this.dialogImage = new System.Windows.Forms.SaveFileDialog();
			this.label3 = new System.Windows.Forms.Label();
			this.txtFontName = new System.Windows.Forms.TextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnBrowseResource
			// 
			this.btnBrowseResource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseResource.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnBrowseResource.Location = new System.Drawing.Point(568, 58);
			this.btnBrowseResource.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnBrowseResource.Name = "btnBrowseResource";
			this.btnBrowseResource.Size = new System.Drawing.Size(112, 35);
			this.btnBrowseResource.TabIndex = 1;
			this.btnBrowseResource.Text = "Browse...";
			this.btnBrowseResource.UseVisualStyleBackColor = true;
			this.btnBrowseResource.Click += new System.EventHandler(this.btnBrowseResource_Click);
			// 
			// txtResources
			// 
			this.txtResources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtResources.Location = new System.Drawing.Point(165, 18);
			this.txtResources.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtResources.Name = "txtResources";
			this.txtResources.Size = new System.Drawing.Size(514, 26);
			this.txtResources.TabIndex = 0;
			this.toolTip1.SetToolTip(this.txtResources, "A path to an AgateLib resource file into which to save the font metrics.");
			this.txtResources.TextChanged += new System.EventHandler(this.txtResources_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(18, 23);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(138, 35);
			this.label1.TabIndex = 5;
			this.label1.Text = "Resource File";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(18, 155);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(138, 26);
			this.label2.TabIndex = 8;
			this.label2.Text = "Image File";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// txtImage
			// 
			this.txtImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtImage.Location = new System.Drawing.Point(165, 151);
			this.txtImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtImage.Name = "txtImage";
			this.txtImage.Size = new System.Drawing.Size(514, 26);
			this.txtImage.TabIndex = 3;
			this.toolTip1.SetToolTip(this.txtImage, "The path to the location where the image data for the font will be saved.");
			this.txtImage.TextChanged += new System.EventHandler(this.txtImage_TextChanged);
			// 
			// btnBrowseImage
			// 
			this.btnBrowseImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseImage.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnBrowseImage.Location = new System.Drawing.Point(568, 191);
			this.btnBrowseImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnBrowseImage.Name = "btnBrowseImage";
			this.btnBrowseImage.Size = new System.Drawing.Size(112, 35);
			this.btnBrowseImage.TabIndex = 4;
			this.btnBrowseImage.Text = "Browse...";
			this.btnBrowseImage.UseVisualStyleBackColor = true;
			this.btnBrowseImage.Click += new System.EventHandler(this.btnBrowseImage_Click);
			// 
			// dialogResources
			// 
			this.dialogResources.CreatePrompt = true;
			this.dialogResources.DefaultExt = "yaml";
			this.dialogResources.Filter = "YAML files|*.yaml|All Files|*.*";
			this.dialogResources.OverwritePrompt = false;
			this.dialogResources.Title = "Create AgateLib font file to save fonts to";
			// 
			// dialogImage
			// 
			this.dialogImage.DefaultExt = "png";
			this.dialogImage.Filter = "PNG image (*.png)|*.png|BMP image (*.bmp)|*.bmp|JPEG Image (*.jpg)|*.jpg;*.jpeg";
			this.dialogImage.Title = "Save font image data as";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(18, 115);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(138, 26);
			this.label3.TabIndex = 10;
			this.label3.Text = "Font Name";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// txtFontName
			// 
			this.txtFontName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFontName.Location = new System.Drawing.Point(165, 111);
			this.txtFontName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtFontName.Name = "txtFontName";
			this.txtFontName.Size = new System.Drawing.Size(514, 26);
			this.txtFontName.TabIndex = 2;
			this.toolTip1.SetToolTip(this.txtFontName, "The name of the font in the resource file.  This name \r\nis used as a key to retri" +
        "eve the font from the reosurce\r\nfile at runtime.");
			this.txtFontName.TextChanged += new System.EventHandler(this.txtFontName_TextChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.panel1);
			this.groupBox1.Location = new System.Drawing.Point(4, 234);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.groupBox1.Size = new System.Drawing.Size(676, 346);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Sample";
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Location = new System.Drawing.Point(4, 25);
			this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(663, 312);
			this.panel1.TabIndex = 0;
			// 
			// SaveFont
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtFontName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtImage);
			this.Controls.Add(this.btnBrowseImage);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtResources);
			this.Controls.Add(this.btnBrowseResource);
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "SaveFont";
			this.Size = new System.Drawing.Size(699, 585);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowseResource;
        private System.Windows.Forms.TextBox txtResources;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtImage;
        private System.Windows.Forms.Button btnBrowseImage;
        private System.Windows.Forms.SaveFileDialog dialogResources;
        private System.Windows.Forms.SaveFileDialog dialogImage;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtFontName;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel1;
    }
}