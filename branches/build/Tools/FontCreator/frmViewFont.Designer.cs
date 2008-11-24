namespace FontCreator
{
    partial class frmViewFont
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
            this.pctImage = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lstItems = new System.Windows.Forms.ListBox();
            this.properties = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.pctImage)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pctImage
            // 
            this.pctImage.BackColor = System.Drawing.Color.White;
            this.pctImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pctImage.Location = new System.Drawing.Point(0, 0);
            this.pctImage.Name = "pctImage";
            this.pctImage.Size = new System.Drawing.Size(266, 468);
            this.pctImage.TabIndex = 0;
            this.pctImage.TabStop = false;
            this.pctImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pctImage_Paint);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pctImage);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(638, 468);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 2;
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
            this.splitContainer2.Panel1.Controls.Add(this.lstItems);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.properties);
            this.splitContainer2.Size = new System.Drawing.Size(368, 468);
            this.splitContainer2.SplitterDistance = 159;
            this.splitContainer2.TabIndex = 0;
            // 
            // lstItems
            // 
            this.lstItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstItems.FormattingEnabled = true;
            this.lstItems.HorizontalScrollbar = true;
            this.lstItems.Location = new System.Drawing.Point(0, 0);
            this.lstItems.MultiColumn = true;
            this.lstItems.Name = "lstItems";
            this.lstItems.Size = new System.Drawing.Size(368, 147);
            this.lstItems.TabIndex = 0;
            this.lstItems.SelectedIndexChanged += new System.EventHandler(this.lstItems_SelectedIndexChanged);
            // 
            // properties
            // 
            this.properties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.properties.Location = new System.Drawing.Point(0, 0);
            this.properties.Name = "properties";
            this.properties.Size = new System.Drawing.Size(368, 305);
            this.properties.TabIndex = 1;
            // 
            // frmViewFont
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 468);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmViewFont";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "View Font";
            ((System.ComponentModel.ISupportInitialize)(this.pctImage)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pctImage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PropertyGrid properties;
        private System.Windows.Forms.ListBox lstItems;
    }
}