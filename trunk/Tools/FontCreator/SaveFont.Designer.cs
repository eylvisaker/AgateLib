namespace FontCreator
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
            this.SuspendLayout();
            // 
            // btnBrowseResource
            // 
            this.btnBrowseResource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseResource.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnBrowseResource.Location = new System.Drawing.Point(222, 59);
            this.btnBrowseResource.Name = "btnBrowseResource";
            this.btnBrowseResource.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseResource.TabIndex = 2;
            this.btnBrowseResource.Text = "Browse...";
            this.btnBrowseResource.UseVisualStyleBackColor = true;
            this.btnBrowseResource.Click += new System.EventHandler(this.btnBrowseResource_Click);
            // 
            // txtResources
            // 
            this.txtResources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResources.Location = new System.Drawing.Point(110, 33);
            this.txtResources.Name = "txtResources";
            this.txtResources.Size = new System.Drawing.Size(187, 20);
            this.txtResources.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtResources, "A path to an AgateLib resource file into which to save the font metrics.");
            this.txtResources.TextChanged += new System.EventHandler(this.txtResources_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 23);
            this.label1.TabIndex = 5;
            this.label1.Text = "Resource File";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Image File";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtImage
            // 
            this.txtImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImage.Location = new System.Drawing.Point(110, 98);
            this.txtImage.Name = "txtImage";
            this.txtImage.Size = new System.Drawing.Size(187, 20);
            this.txtImage.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtImage, "The path to the location where the image data for the font will be saved.");
            this.txtImage.TextChanged += new System.EventHandler(this.txtImage_TextChanged);
            // 
            // btnBrowseImage
            // 
            this.btnBrowseImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseImage.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnBrowseImage.Location = new System.Drawing.Point(222, 124);
            this.btnBrowseImage.Name = "btnBrowseImage";
            this.btnBrowseImage.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseImage.TabIndex = 4;
            this.btnBrowseImage.Text = "Browse...";
            this.btnBrowseImage.UseVisualStyleBackColor = true;
            this.btnBrowseImage.Click += new System.EventHandler(this.btnBrowseImage_Click);
            // 
            // dialogResources
            // 
            this.dialogResources.CreatePrompt = true;
            this.dialogResources.DefaultExt = "xml";
            this.dialogResources.Filter = "XML files|*.xml|All Files|*.*";
            this.dialogResources.OverwritePrompt = false;
            this.dialogResources.Title = "Select AgateLib resource file to save to";
            // 
            // dialogImage
            // 
            this.dialogImage.DefaultExt = "png";
            this.dialogImage.Filter = "PNG image (*.png)|*.png|BMP image (*.bmp)|*.bmp|JPEG Image (*.jpg)|*.jpg;*.jpeg";
            this.dialogImage.Title = "Save font image data as";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Font Name";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtFontName
            // 
            this.txtFontName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFontName.Location = new System.Drawing.Point(110, 7);
            this.txtFontName.Name = "txtFontName";
            this.txtFontName.Size = new System.Drawing.Size(187, 20);
            this.txtFontName.TabIndex = 0;
            this.toolTip1.SetToolTip(this.txtFontName, "The name of the font in the resource file.  This name \r\nis used as a key to retri" +
                    "eve the font from the reosurce\r\nfile at runtime.");
            this.txtFontName.TextChanged += new System.EventHandler(this.txtFontName_TextChanged);
            // 
            // SaveFont
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtFontName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtImage);
            this.Controls.Add(this.btnBrowseImage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtResources);
            this.Controls.Add(this.btnBrowseResource);
            this.Name = "SaveFont";
            this.Size = new System.Drawing.Size(309, 161);
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
    }
}