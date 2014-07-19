namespace FontCreator
{
    partial class EditGlyphs
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.pctImage = new System.Windows.Forms.PictureBox();
			this.pctZoom = new System.Windows.Forms.PictureBox();
			this.hSeparator1 = new NotebookLib.HSeparator();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.lstItems = new System.Windows.Forms.ListBox();
			this.properties = new System.Windows.Forms.PropertyGrid();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
			this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
			this.mouseLabel = new System.Windows.Forms.ToolStripLabel();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pctImage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pctZoom)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.toolStrip1.SuspendLayout();
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
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
			this.splitContainer1.Panel1.Controls.Add(this.hSeparator1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(574, 389);
			this.splitContainer1.SplitterDistance = 217;
			this.splitContainer1.TabIndex = 2;
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.pctImage);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.pctZoom);
			this.splitContainer3.Size = new System.Drawing.Size(217, 385);
			this.splitContainer3.SplitterDistance = 118;
			this.splitContainer3.TabIndex = 2;
			// 
			// pctImage
			// 
			this.pctImage.BackColor = System.Drawing.Color.White;
			this.pctImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pctImage.Location = new System.Drawing.Point(0, 0);
			this.pctImage.Name = "pctImage";
			this.pctImage.Size = new System.Drawing.Size(217, 118);
			this.pctImage.TabIndex = 0;
			this.pctImage.TabStop = false;
			this.pctImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pctImage_Paint);
			// 
			// pctZoom
			// 
			this.pctZoom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pctZoom.Location = new System.Drawing.Point(0, 0);
			this.pctZoom.Name = "pctZoom";
			this.pctZoom.Size = new System.Drawing.Size(217, 263);
			this.pctZoom.TabIndex = 2;
			this.pctZoom.TabStop = false;
			this.pctZoom.MouseLeave += new System.EventHandler(this.pctZoom_MouseLeave);
			this.pctZoom.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pctZoom_MouseMove);
			this.pctZoom.Resize += new System.EventHandler(this.pctZoom_Resize);
			this.pctZoom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pctZoom_MouseDown);
			this.pctZoom.Paint += new System.Windows.Forms.PaintEventHandler(this.pctZoom_Paint);
			this.pctZoom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pctZoom_MouseUp);
			// 
			// hSeparator1
			// 
			this.hSeparator1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.hSeparator1.Location = new System.Drawing.Point(0, 385);
			this.hSeparator1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
			this.hSeparator1.MaximumSize = new System.Drawing.Size(10000, 4);
			this.hSeparator1.MinimumSize = new System.Drawing.Size(0, 4);
			this.hSeparator1.Name = "hSeparator1";
			this.hSeparator1.Size = new System.Drawing.Size(217, 4);
			this.hSeparator1.TabIndex = 1;
			this.hSeparator1.TabStop = true;
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
			this.splitContainer2.Size = new System.Drawing.Size(353, 389);
			this.splitContainer2.SplitterDistance = 101;
			this.splitContainer2.TabIndex = 0;
			// 
			// lstItems
			// 
			this.lstItems.ColumnWidth = 20;
			this.lstItems.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstItems.FormattingEnabled = true;
			this.lstItems.HorizontalScrollbar = true;
			this.lstItems.Location = new System.Drawing.Point(0, 0);
			this.lstItems.MultiColumn = true;
			this.lstItems.Name = "lstItems";
			this.lstItems.Size = new System.Drawing.Size(353, 95);
			this.lstItems.TabIndex = 0;
			this.lstItems.SelectedIndexChanged += new System.EventHandler(this.lstItems_SelectedIndexChanged);
			this.lstItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstItems_KeyDown);
			// 
			// properties
			// 
			this.properties.Dock = System.Windows.Forms.DockStyle.Fill;
			this.properties.Location = new System.Drawing.Point(0, 0);
			this.properties.Name = "properties";
			this.properties.Size = new System.Drawing.Size(353, 284);
			this.properties.TabIndex = 1;
			this.properties.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.properties_PropertyValueChanged);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZoomIn,
            this.btnZoomOut,
            this.mouseLabel});
			this.toolStrip1.Location = new System.Drawing.Point(0, 389);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(574, 25);
			this.toolStrip1.TabIndex = 4;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// btnZoomIn
			// 
			this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomIn.Image = global::FontCreator.Properties.Resources.zoom_in;
			this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomIn.Name = "btnZoomIn";
			this.btnZoomIn.Size = new System.Drawing.Size(23, 22);
			this.btnZoomIn.Text = "toolStripButton1";
			this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
			// 
			// btnZoomOut
			// 
			this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomOut.Image = global::FontCreator.Properties.Resources.zoom_out;
			this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomOut.Name = "btnZoomOut";
			this.btnZoomOut.Size = new System.Drawing.Size(23, 22);
			this.btnZoomOut.Text = "toolStripButton2";
			this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
			// 
			// mouseLabel
			// 
			this.mouseLabel.Name = "mouseLabel";
			this.mouseLabel.Size = new System.Drawing.Size(71, 22);
			this.mouseLabel.Text = "mouseLabel";
			// 
			// EditGlyphs
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "EditGlyphs";
			this.Size = new System.Drawing.Size(574, 414);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			this.splitContainer3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pctImage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pctZoom)).EndInit();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pctImage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PropertyGrid properties;
        private System.Windows.Forms.ListBox lstItems;
		private NotebookLib.HSeparator hSeparator1;
		private System.Windows.Forms.PictureBox pctZoom;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripLabel mouseLabel;
		private System.Windows.Forms.ToolStripButton btnZoomIn;
		private System.Windows.Forms.ToolStripButton btnZoomOut;
    }
}