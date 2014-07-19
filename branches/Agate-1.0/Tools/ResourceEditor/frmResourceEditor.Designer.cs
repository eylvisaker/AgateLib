namespace ResourceEditor
{
    partial class frmResourceEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmResourceEditor));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainbook = new NotebookLib.Notebook();
            this.notebookPage5 = new NotebookLib.NotebookPage();
            this.notebookPage3 = new NotebookLib.NotebookPage();
            this.notebookPage4 = new NotebookLib.NotebookPage();
            this.fontPage = new NotebookLib.NotebookPage();
            this.pageStrings = new NotebookLib.NotebookPage();
            this.stringTableEditor1 = new ResourceEditor.StringTable.StringTableEditor();
            this.pageWindow = new NotebookLib.NotebookPage();
            this.displayWindowEditor1 = new ResourceEditor.DisplayWinds.DisplayWindowEditor();
            this.pageNumbers = new NotebookLib.NotebookPage();
            this.notebookPage1 = new NotebookLib.NotebookPage();
            this.notebookPage2 = new NotebookLib.NotebookPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newResourceFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.validateResourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createDistributionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCut = new System.Windows.Forms.ToolStripButton();
            this.btnCopy = new System.Windows.Forms.ToolStripButton();
            this.btnPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.mainbook.SuspendLayout();
            this.pageStrings.SuspendLayout();
            this.pageWindow.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.mainbook);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(608, 398);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(608, 469);
            this.toolStripContainer1.TabIndex = 4;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(608, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(109, 17);
            this.statusLabel.Text = "toolStripStatusLabel1";
            // 
            // mainbook
            // 
            this.mainbook.BackColor = System.Drawing.SystemColors.Control;
            this.mainbook.Controls.Add(this.notebookPage5);
            this.mainbook.Controls.Add(this.notebookPage4);
            this.mainbook.Controls.Add(this.pageStrings);
            this.mainbook.Controls.Add(this.pageWindow);
            this.mainbook.Controls.Add(this.notebookPage3);
            this.mainbook.Controls.Add(this.pageNumbers);
            this.mainbook.Controls.Add(this.notebookPage1);
            this.mainbook.Controls.Add(this.notebookPage2);
            this.mainbook.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainbook.Location = new System.Drawing.Point(0, 0);
            this.mainbook.Name = "mainbook";
            this.mainbook.Navigator.Margin = new System.Windows.Forms.Padding(3);
            this.mainbook.Navigator.PageBackColor = System.Drawing.SystemColors.Control;
            this.mainbook.NavigatorType = NotebookLib.NavigatorType.ListBook;
            this.mainbook.SelectedIndex = 5;
            this.mainbook.Size = new System.Drawing.Size(608, 398);
            this.mainbook.SplitterLocation = 206;
            this.mainbook.TabIndex = 0;
            // 
            // notebookPage5
            // 
            this.notebookPage5.BackColor = System.Drawing.SystemColors.Control;
            this.notebookPage5.Image = global::ResourceEditor.Properties.Resources.font;
            this.notebookPage5.Location = new System.Drawing.Point(210, 0);
            this.notebookPage5.Name = "notebookPage5";
            this.notebookPage5.Order = 45;
            this.notebookPage5.Size = new System.Drawing.Size(398, 398);
            this.notebookPage5.TabIndex = 11;
            this.notebookPage5.Text = "Fonts";
            // 
            // notebookPage3
            // 
            this.notebookPage3.BackColor = System.Drawing.SystemColors.Control;
            this.notebookPage3.Image = global::ResourceEditor.Properties.Resources.SoundEffect;
            this.notebookPage3.Location = new System.Drawing.Point(210, 0);
            this.notebookPage3.Name = "notebookPage3";
            this.notebookPage3.Order = 50;
            this.notebookPage3.ShowPage = false;
            this.notebookPage3.Size = new System.Drawing.Size(398, 398);
            this.notebookPage3.TabIndex = 8;
            this.notebookPage3.Text = "Sound Effects";
            // 
            // notebookPage4
            // 
            this.notebookPage4.BackColor = System.Drawing.SystemColors.Control;
            this.notebookPage4.Image = global::ResourceEditor.Properties.Resources.Music;
            this.notebookPage4.Location = new System.Drawing.Point(210, 0);
            this.notebookPage4.Name = "notebookPage4";
            this.notebookPage4.Order = 60;
            this.notebookPage4.ShowPage = false;
            this.notebookPage4.Size = new System.Drawing.Size(398, 398);
            this.notebookPage4.TabIndex = 9;
            this.notebookPage4.Text = "Music";
            // 
            // fontPage
            // 
            this.fontPage.BackColor = System.Drawing.SystemColors.Control;
            this.fontPage.Image = global::ResourceEditor.Properties.Resources.font;
            this.fontPage.Location = new System.Drawing.Point(210, 0);
            this.fontPage.Name = "fontPage";
            this.fontPage.Order = 70;
            this.fontPage.Size = new System.Drawing.Size(398, 398);
            this.fontPage.TabIndex = 13;
            this.fontPage.Text = "Fonts";
            // 
            // pageStrings
            // 
            this.pageStrings.BackColor = System.Drawing.SystemColors.Control;
            this.pageStrings.Controls.Add(this.stringTableEditor1);
            this.pageStrings.Image = global::ResourceEditor.Properties.Resources.Strings;
            this.pageStrings.Location = new System.Drawing.Point(210, 0);
            this.pageStrings.Name = "pageStrings";
            this.pageStrings.Order = 0;
            this.pageStrings.Size = new System.Drawing.Size(398, 398);
            this.pageStrings.TabIndex = 1;
            this.pageStrings.Text = "Strings";
            // 
            // stringTableEditor1
            // 
            this.stringTableEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stringTableEditor1.Location = new System.Drawing.Point(0, 0);
            this.stringTableEditor1.Name = "stringTableEditor1";
            this.stringTableEditor1.ResourceManager = null;
            this.stringTableEditor1.Size = new System.Drawing.Size(398, 398);
            this.stringTableEditor1.TabIndex = 0;
            this.stringTableEditor1.StatusText += new System.EventHandler<ResourceEditor.StatusTextEventArgs>(this.stringTableEditor1_StatusText);
            // 
            // pageWindow
            // 
            this.pageWindow.BackColor = System.Drawing.SystemColors.Control;
            this.pageWindow.Controls.Add(this.displayWindowEditor1);
            this.pageWindow.Image = global::ResourceEditor.Properties.Resources.DisplayWindow;
            this.pageWindow.Location = new System.Drawing.Point(210, 0);
            this.pageWindow.Name = "pageWindow";
            this.pageWindow.Order = 20;
            this.pageWindow.Size = new System.Drawing.Size(398, 398);
            this.pageWindow.TabIndex = 4;
            this.pageWindow.Text = "Display Windows";
            // 
            // displayWindowEditor1
            // 
            this.displayWindowEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displayWindowEditor1.Location = new System.Drawing.Point(0, 0);
            this.displayWindowEditor1.Name = "displayWindowEditor1";
            this.displayWindowEditor1.Size = new System.Drawing.Size(398, 398);
            this.displayWindowEditor1.TabIndex = 0;
            // 
            // pageNumbers
            // 
            this.pageNumbers.BackColor = System.Drawing.SystemColors.Control;
            this.pageNumbers.Image = global::ResourceEditor.Properties.Resources.Numbers;
            this.pageNumbers.Location = new System.Drawing.Point(210, 0);
            this.pageNumbers.Name = "pageNumbers";
            this.pageNumbers.Order = 10;
            this.pageNumbers.ShowPage = false;
            this.pageNumbers.Size = new System.Drawing.Size(398, 398);
            this.pageNumbers.TabIndex = 2;
            this.pageNumbers.Text = "Numbers";
            // 
            // notebookPage1
            // 
            this.notebookPage1.BackColor = System.Drawing.SystemColors.Control;
            this.notebookPage1.Image = global::ResourceEditor.Properties.Resources.sprite;
            this.notebookPage1.Location = new System.Drawing.Point(210, 0);
            this.notebookPage1.Name = "notebookPage1";
            this.notebookPage1.Order = 40;
            this.notebookPage1.Size = new System.Drawing.Size(398, 398);
            this.notebookPage1.TabIndex = 6;
            this.notebookPage1.Text = "Sprites";
            // 
            // notebookPage2
            // 
            this.notebookPage2.BackColor = System.Drawing.SystemColors.Control;
            this.notebookPage2.Image = global::ResourceEditor.Properties.Resources.Surfaces;
            this.notebookPage2.Location = new System.Drawing.Point(210, 0);
            this.notebookPage2.Name = "notebookPage2";
            this.notebookPage2.Order = 30;
            this.notebookPage2.Size = new System.Drawing.Size(398, 398);
            this.notebookPage2.TabIndex = 5;
            this.notebookPage2.Text = "Surfaces";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(608, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newResourceFileToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripSeparator3,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newResourceFileToolStripMenuItem
            // 
            this.newResourceFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newResourceFileToolStripMenuItem.Image")));
            this.newResourceFileToolStripMenuItem.Name = "newResourceFileToolStripMenuItem";
            this.newResourceFileToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.newResourceFileToolStripMenuItem.Text = "&New Resource File...";
            this.newResourceFileToolStripMenuItem.Click += new System.EventHandler(this.newResourceFileToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(171, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveAsToolStripMenuItem.Text = "S&ave as...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(171, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.cutToolStripMenuItem.Text = "C&ut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::ResourceEditor.Properties.Resources.Delete;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.validateResourcesToolStripMenuItem,
            this.createDistributionToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // validateResourcesToolStripMenuItem
            // 
            this.validateResourcesToolStripMenuItem.Name = "validateResourcesToolStripMenuItem";
            this.validateResourcesToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.validateResourcesToolStripMenuItem.Text = "Verify Resources...";
            // 
            // createDistributionToolStripMenuItem
            // 
            this.createDistributionToolStripMenuItem.Name = "createDistributionToolStripMenuItem";
            this.createDistributionToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.createDistributionToolStripMenuItem.Text = "Create Distribution...";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnOpen,
            this.btnSave,
            this.toolStripSeparator2,
            this.btnCut,
            this.btnCopy,
            this.btnPaste,
            this.toolStripSeparator4});
            this.toolStrip1.Location = new System.Drawing.Point(3, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(162, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(23, 22);
            this.btnNew.Text = "New XML Resource File";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(23, 22);
            this.btnOpen.Text = "Open XML Resource File";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.Text = "Save Current File";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnCut
            // 
            this.btnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCut.Image = ((System.Drawing.Image)(resources.GetObject("btnCut.Image")));
            this.btnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(23, 22);
            this.btnCut.Text = "Cut";
            this.btnCut.Click += new System.EventHandler(this.btnCut_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
            this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(23, 22);
            this.btnCopy.Text = "Copy";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnPaste.Image")));
            this.btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(23, 22);
            this.btnPaste.Text = "Paste";
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "xml";
            this.openFileDialog.Filter = "XML files (*.xml)|*.xml|All Files|*.*";
            this.openFileDialog.Title = "Open AgateLib Resource File";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "XML files (*.xml)|*.xml|All Files|*.*";
            this.saveFileDialog.Title = "Save AgateLib Resource File";
            // 
            // frmResourceEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 469);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmResourceEditor";
            this.Text = "AgateLib Resource Editor";
            this.Load += new System.EventHandler(this.frmResourceEditor_Load);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.mainbook.ResumeLayout(false);
            this.pageStrings.ResumeLayout(false);
            this.pageWindow.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newResourceFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnCut;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnCopy;
        private System.Windows.Forms.ToolStripButton btnPaste;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private NotebookLib.Notebook mainbook;
        private NotebookLib.NotebookPage pageNumbers;
        private NotebookLib.NotebookPage pageStrings;
        private NotebookLib.NotebookPage notebookPage2;
        private NotebookLib.NotebookPage notebookPage1;
        private NotebookLib.NotebookPage pageWindow;
        private NotebookLib.NotebookPage notebookPage3;
        private NotebookLib.NotebookPage notebookPage4;
        private NotebookLib.NotebookPage notebookPage5;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem validateResourcesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createDistributionToolStripMenuItem;
        private ResourceEditor.StringTable.StringTableEditor stringTableEditor1;
        private ResourceEditor.DisplayWinds.DisplayWindowEditor displayWindowEditor1;
        private NotebookLib.NotebookPage fontPage;

    }
}

