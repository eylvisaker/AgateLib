namespace PackedSpriteCreator
{
    partial class frmAddSpriteFrames
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.pctInitialPreview = new System.Windows.Forms.PictureBox();
            this.detailsPanel = new System.Windows.Forms.Panel();
            this.pnlPicture = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.chkTransparent = new System.Windows.Forms.CheckBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.pnlTransparentColor = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pctInitialPreview)).BeginInit();
            this.detailsPanel.SuspendLayout();
            this.pnlPicture.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(390, 306);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(309, 306);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Add Frames";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Source Image";
            // 
            // openFile
            // 
            this.openFile.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.gif;*.png;*.tga|All Files|*.*";
            // 
            // txtFilename
            // 
            this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilename.Location = new System.Drawing.Point(12, 25);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(419, 20);
            this.txtFilename.TabIndex = 3;
            this.txtFilename.TextChanged += new System.EventHandler(this.txtFilename_TextChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnBrowse.Location = new System.Drawing.Point(437, 23);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(28, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // pctInitialPreview
            // 
            this.pctInitialPreview.Location = new System.Drawing.Point(0, 0);
            this.pctInitialPreview.Name = "pctInitialPreview";
            this.pctInitialPreview.Size = new System.Drawing.Size(194, 150);
            this.pctInitialPreview.TabIndex = 5;
            this.pctInitialPreview.TabStop = false;
            this.pctInitialPreview.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pctInitialPreview_MouseClick);
            // 
            // detailsPanel
            // 
            this.detailsPanel.Controls.Add(this.pnlTransparentColor);
            this.detailsPanel.Controls.Add(this.chkTransparent);
            this.detailsPanel.Controls.Add(this.label2);
            this.detailsPanel.Controls.Add(this.pnlPicture);
            this.detailsPanel.Location = new System.Drawing.Point(12, 51);
            this.detailsPanel.Name = "detailsPanel";
            this.detailsPanel.Size = new System.Drawing.Size(453, 249);
            this.detailsPanel.TabIndex = 6;
            // 
            // pnlPicture
            // 
            this.pnlPicture.AutoScroll = true;
            this.pnlPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlPicture.Controls.Add(this.pctInitialPreview);
            this.pnlPicture.Location = new System.Drawing.Point(273, 21);
            this.pnlPicture.Name = "pnlPicture";
            this.pnlPicture.Size = new System.Drawing.Size(177, 128);
            this.pnlPicture.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(270, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Initial Image";
            // 
            // chkTransparent
            // 
            this.chkTransparent.AutoSize = true;
            this.chkTransparent.Location = new System.Drawing.Point(3, 6);
            this.chkTransparent.Name = "chkTransparent";
            this.chkTransparent.Size = new System.Drawing.Size(110, 17);
            this.chkTransparent.TabIndex = 6;
            this.chkTransparent.Text = "Transparent Color";
            this.chkTransparent.UseVisualStyleBackColor = true;
            this.chkTransparent.CheckedChanged += new System.EventHandler(this.chkTransparent_CheckedChanged);
            // 
            // pnlTransparentColor
            // 
            this.pnlTransparentColor.BackColor = System.Drawing.Color.Black;
            this.pnlTransparentColor.Enabled = false;
            this.pnlTransparentColor.Location = new System.Drawing.Point(119, 3);
            this.pnlTransparentColor.Name = "pnlTransparentColor";
            this.pnlTransparentColor.Size = new System.Drawing.Size(23, 21);
            this.pnlTransparentColor.TabIndex = 7;
            this.pnlTransparentColor.Visible = false;
            // 
            // frmAddSpriteFrames
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(477, 341);
            this.Controls.Add(this.detailsPanel);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddSpriteFrames";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Sprite Frames";
            this.Load += new System.EventHandler(this.frmAddSpriteFrames_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pctInitialPreview)).EndInit();
            this.detailsPanel.ResumeLayout(false);
            this.detailsPanel.PerformLayout();
            this.pnlPicture.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.PictureBox pctInitialPreview;
        private System.Windows.Forms.Panel detailsPanel;
        private System.Windows.Forms.Panel pnlPicture;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlTransparentColor;
        private System.Windows.Forms.CheckBox chkTransparent;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}