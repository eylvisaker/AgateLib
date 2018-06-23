namespace FontCreatorApp
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditGlyphs));
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
			this.fontDropDown = new System.Windows.Forms.ToolStripDropDownButton();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pctImage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pctZoom)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
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
			this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
			this.splitContainer1.Size = new System.Drawing.Size(861, 605);
			this.splitContainer1.SplitterDistance = 325;
			this.splitContainer1.SplitterWidth = 6;
			this.splitContainer1.TabIndex = 2;
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
			this.splitContainer3.Size = new System.Drawing.Size(325, 599);
			this.splitContainer3.SplitterDistance = 330;
			this.splitContainer3.SplitterWidth = 6;
			this.splitContainer3.TabIndex = 2;
			// 
			// pctImage
			// 
			this.pctImage.BackColor = System.Drawing.Color.White;
			this.pctImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pctImage.Location = new System.Drawing.Point(0, 0);
			this.pctImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.pctImage.Name = "pctImage";
			this.pctImage.Size = new System.Drawing.Size(325, 330);
			this.pctImage.TabIndex = 0;
			this.pctImage.TabStop = false;
			this.pctImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pctImage_Paint);
			// 
			// pctZoom
			// 
			this.pctZoom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pctZoom.Location = new System.Drawing.Point(0, 0);
			this.pctZoom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.pctZoom.Name = "pctZoom";
			this.pctZoom.Size = new System.Drawing.Size(325, 263);
			this.pctZoom.TabIndex = 2;
			this.pctZoom.TabStop = false;
			this.pctZoom.Paint += new System.Windows.Forms.PaintEventHandler(this.pctZoom_Paint);
			this.pctZoom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pctZoom_MouseDown);
			this.pctZoom.MouseLeave += new System.EventHandler(this.pctZoom_MouseLeave);
			this.pctZoom.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pctZoom_MouseMove);
			this.pctZoom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pctZoom_MouseUp);
			this.pctZoom.Resize += new System.EventHandler(this.pctZoom_Resize);
			// 
			// hSeparator1
			// 
			this.hSeparator1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.hSeparator1.Location = new System.Drawing.Point(0, 599);
			this.hSeparator1.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
			this.hSeparator1.MaximumSize = new System.Drawing.Size(15000, 6);
			this.hSeparator1.MinimumSize = new System.Drawing.Size(0, 6);
			this.hSeparator1.Name = "hSeparator1";
			this.hSeparator1.Size = new System.Drawing.Size(325, 6);
			this.hSeparator1.TabIndex = 1;
			this.hSeparator1.TabStop = true;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
			this.splitContainer2.Size = new System.Drawing.Size(530, 605);
			this.splitContainer2.SplitterDistance = 134;
			this.splitContainer2.SplitterWidth = 6;
			this.splitContainer2.TabIndex = 0;
			// 
			// lstItems
			// 
			this.lstItems.ColumnWidth = 20;
			this.lstItems.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstItems.FormattingEnabled = true;
			this.lstItems.HorizontalScrollbar = true;
			this.lstItems.ItemHeight = 20;
			this.lstItems.Location = new System.Drawing.Point(0, 0);
			this.lstItems.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.lstItems.MultiColumn = true;
			this.lstItems.Name = "lstItems";
			this.lstItems.Size = new System.Drawing.Size(530, 134);
			this.lstItems.TabIndex = 0;
			this.lstItems.SelectedIndexChanged += new System.EventHandler(this.lstItems_SelectedIndexChanged);
			this.lstItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstItems_KeyDown);
			// 
			// properties
			// 
			this.properties.Dock = System.Windows.Forms.DockStyle.Fill;
			this.properties.Location = new System.Drawing.Point(0, 0);
			this.properties.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.properties.Name = "properties";
			this.properties.Size = new System.Drawing.Size(530, 465);
			this.properties.TabIndex = 1;
			this.properties.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.properties_PropertyValueChanged);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZoomIn,
            this.btnZoomOut,
            this.fontDropDown,
            this.mouseLabel});
			this.toolStrip1.Location = new System.Drawing.Point(0, 605);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.toolStrip1.Size = new System.Drawing.Size(861, 32);
			this.toolStrip1.TabIndex = 4;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// btnZoomIn
			// 
			this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomIn.Image = global::FontCreatorApp.Properties.Resources.zoom_in;
			this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomIn.Name = "btnZoomIn";
			this.btnZoomIn.Size = new System.Drawing.Size(28, 29);
			this.btnZoomIn.Text = "toolStripButton1";
			this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
			// 
			// btnZoomOut
			// 
			this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnZoomOut.Image = global::FontCreatorApp.Properties.Resources.zoom_out;
			this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnZoomOut.Name = "btnZoomOut";
			this.btnZoomOut.Size = new System.Drawing.Size(28, 29);
			this.btnZoomOut.Text = "toolStripButton2";
			this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
			// 
			// mouseLabel
			// 
			this.mouseLabel.Name = "mouseLabel";
			this.mouseLabel.Size = new System.Drawing.Size(107, 29);
			this.mouseLabel.Text = "mouseLabel";
			// 
			// fontDropDown
			// 
			this.fontDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.fontDropDown.Image = ((System.Drawing.Image)(resources.GetObject("fontDropDown.Image")));
			this.fontDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.fontDropDown.Name = "fontDropDown";
			this.fontDropDown.Size = new System.Drawing.Size(66, 29);
			this.fontDropDown.Text = "Font";
			// 
			// EditGlyphs
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolStrip1);
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "EditGlyphs";
			this.Size = new System.Drawing.Size(861, 637);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
			this.splitContainer3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pctImage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pctZoom)).EndInit();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
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
		private System.Windows.Forms.ToolStripDropDownButton fontDropDown;
	}
}