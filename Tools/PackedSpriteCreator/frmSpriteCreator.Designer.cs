namespace PackedSpriteCreator
{
    partial class frmSpriteCreator
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newResourceFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openResourceFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeResourceFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.openDialog = new System.Windows.Forms.OpenFileDialog();
            this.spriteEditor1 = new PackedSpriteCreator.SpriteEditor();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(564, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newResourceFileToolStripMenuItem,
            this.openResourceFileToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.closeResourceFileToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newResourceFileToolStripMenuItem
            // 
            this.newResourceFileToolStripMenuItem.Name = "newResourceFileToolStripMenuItem";
            this.newResourceFileToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.newResourceFileToolStripMenuItem.Text = "&New resource file";
            this.newResourceFileToolStripMenuItem.Click += new System.EventHandler(this.newResourceFileToolStripMenuItem_Click);
            // 
            // openResourceFileToolStripMenuItem
            // 
            this.openResourceFileToolStripMenuItem.Name = "openResourceFileToolStripMenuItem";
            this.openResourceFileToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.openResourceFileToolStripMenuItem.Text = "&Open resource file...";
            this.openResourceFileToolStripMenuItem.Click += new System.EventHandler(this.openResourceFileToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            // 
            // closeResourceFileToolStripMenuItem
            // 
            this.closeResourceFileToolStripMenuItem.Name = "closeResourceFileToolStripMenuItem";
            this.closeResourceFileToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.closeResourceFileToolStripMenuItem.Text = "&Close resource file";
            this.closeResourceFileToolStripMenuItem.Click += new System.EventHandler(this.closeResourceFileToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(182, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 399);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(564, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(109, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // openDialog
            // 
            this.openDialog.FileName = "openFileDialog1";
            this.openDialog.Filter = "Resource files (*.xml)|*.xml|All Files|*.*";
            // 
            // spriteEditor1
            // 
            this.spriteEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spriteEditor1.Location = new System.Drawing.Point(0, 24);
            this.spriteEditor1.Name = "spriteEditor1";
            this.spriteEditor1.Resources = null;
            this.spriteEditor1.Size = new System.Drawing.Size(564, 375);
            this.spriteEditor1.TabIndex = 2;
            // 
            // frmSpriteCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 421);
            this.Controls.Add(this.spriteEditor1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmSpriteCreator";
            this.Text = "Sprite Creator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newResourceFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openResourceFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeResourceFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private SpriteEditor spriteEditor1;
        private System.Windows.Forms.OpenFileDialog openDialog;
    }
}