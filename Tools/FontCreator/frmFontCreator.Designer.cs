namespace FontCreatorApp
{
    partial class frmFontCreator
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
			FontCreatorApp.FontBuilderParameters fontBuilderParameters1 = new FontCreatorApp.FontBuilderParameters();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFontCreator));
			this.btnNext = new System.Windows.Forms.Button();
			this.btnPrevious = new System.Windows.Forms.Button();
			this.bottomPanel = new System.Windows.Forms.Panel();
			this.pnlWarning = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pnlCreateFont = new System.Windows.Forms.Panel();
			this.createFont1 = new FontCreatorApp.CreateFont();
			this.pnlEditGlyphs = new System.Windows.Forms.Panel();
			this.editGlyphs1 = new FontCreatorApp.EditGlyphs();
			this.pnlSaveFont = new System.Windows.Forms.Panel();
			this.saveFont1 = new FontCreatorApp.SaveFont();
			this.bottomPanel.SuspendLayout();
			this.pnlWarning.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.pnlCreateFont.SuspendLayout();
			this.pnlEditGlyphs.SuspendLayout();
			this.pnlSaveFont.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnNext
			// 
			this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNext.Location = new System.Drawing.Point(656, 13);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(75, 23);
			this.btnNext.TabIndex = 0;
			this.btnNext.Text = "Next >>";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnPrevious
			// 
			this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPrevious.Enabled = false;
			this.btnPrevious.Location = new System.Drawing.Point(575, 13);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.Size = new System.Drawing.Size(75, 23);
			this.btnPrevious.TabIndex = 1;
			this.btnPrevious.Text = "Previous <<";
			this.btnPrevious.UseVisualStyleBackColor = true;
			this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
			this.btnPrevious.MouseEnter += new System.EventHandler(this.btnPrevious_MouseEnter);
			this.btnPrevious.MouseLeave += new System.EventHandler(this.btnPrevious_MouseLeave);
			// 
			// bottomPanel
			// 
			this.bottomPanel.Controls.Add(this.pnlWarning);
			this.bottomPanel.Controls.Add(this.btnNext);
			this.bottomPanel.Controls.Add(this.btnPrevious);
			this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomPanel.Location = new System.Drawing.Point(0, 552);
			this.bottomPanel.Name = "bottomPanel";
			this.bottomPanel.Size = new System.Drawing.Size(743, 48);
			this.bottomPanel.TabIndex = 2;
			// 
			// pnlWarning
			// 
			this.pnlWarning.Controls.Add(this.label1);
			this.pnlWarning.Controls.Add(this.pictureBox1);
			this.pnlWarning.Location = new System.Drawing.Point(3, 0);
			this.pnlWarning.Name = "pnlWarning";
			this.pnlWarning.Size = new System.Drawing.Size(389, 48);
			this.pnlWarning.TabIndex = 3;
			this.pnlWarning.Visible = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(56, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(279, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "If you go back, you will lose all changes made in this step.";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::FontCreatorApp.Properties.Resources.Warning;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(50, 50);
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// pnlCreateFont
			// 
			this.pnlCreateFont.Controls.Add(this.createFont1);
			this.pnlCreateFont.Location = new System.Drawing.Point(12, 12);
			this.pnlCreateFont.Name = "pnlCreateFont";
			this.pnlCreateFont.Size = new System.Drawing.Size(581, 299);
			this.pnlCreateFont.TabIndex = 3;
			this.pnlCreateFont.Visible = false;
			// 
			// createFont1
			// 
			this.createFont1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.createFont1.Location = new System.Drawing.Point(0, 0);
			this.createFont1.Name = "createFont1";
			this.createFont1.Size = new System.Drawing.Size(581, 299);
			this.createFont1.TabIndex = 0;
			// 
			// pnlEditGlyphs
			// 
			this.pnlEditGlyphs.Controls.Add(this.editGlyphs1);
			this.pnlEditGlyphs.Location = new System.Drawing.Point(12, 317);
			this.pnlEditGlyphs.Name = "pnlEditGlyphs";
			this.pnlEditGlyphs.Size = new System.Drawing.Size(432, 207);
			this.pnlEditGlyphs.TabIndex = 1;
			this.pnlEditGlyphs.Visible = false;
			// 
			// editGlyphs1
			// 
			this.editGlyphs1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editGlyphs1.Location = new System.Drawing.Point(0, 0);
			this.editGlyphs1.Name = "editGlyphs1";
			this.editGlyphs1.Size = new System.Drawing.Size(432, 207);
			this.editGlyphs1.TabIndex = 0;
			// 
			// pnlSaveFont
			// 
			this.pnlSaveFont.Controls.Add(this.saveFont1);
			this.pnlSaveFont.Location = new System.Drawing.Point(460, 317);
			this.pnlSaveFont.Name = "pnlSaveFont";
			this.pnlSaveFont.Size = new System.Drawing.Size(262, 172);
			this.pnlSaveFont.TabIndex = 1;
			this.pnlSaveFont.Visible = false;
			// 
			// saveFont1
			// 
			this.saveFont1.AgateFont = null;
			this.saveFont1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.saveFont1.FontName = "";
			this.saveFont1.Location = new System.Drawing.Point(0, 0);
			this.saveFont1.Name = "saveFont1";
			this.saveFont1.Size = new System.Drawing.Size(262, 172);
			this.saveFont1.TabIndex = 0;
			this.saveFont1.ValidInputChanged += new System.EventHandler(this.saveFont1_ValidInputChanged);
			// 
			// frmFontCreator
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(743, 600);
			this.Controls.Add(this.pnlSaveFont);
			this.Controls.Add(this.pnlEditGlyphs);
			this.Controls.Add(this.bottomPanel);
			this.Controls.Add(this.pnlCreateFont);
			this.Name = "frmFontCreator";
			this.Text = "Font Creator";
			this.bottomPanel.ResumeLayout(false);
			this.pnlWarning.ResumeLayout(false);
			this.pnlWarning.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.pnlCreateFont.ResumeLayout(false);
			this.pnlEditGlyphs.ResumeLayout(false);
			this.pnlSaveFont.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Panel pnlCreateFont;
        private CreateFont createFont1;
        private System.Windows.Forms.Panel pnlEditGlyphs;
        private EditGlyphs editGlyphs1;
        private System.Windows.Forms.Panel pnlSaveFont;
        private SaveFont saveFont1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlWarning;
        private System.Windows.Forms.Label label1;
    }
}