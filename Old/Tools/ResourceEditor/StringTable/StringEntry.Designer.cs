namespace ResourceEditor.StringTable
{
    partial class StringEntry
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.languageLabel = new System.Windows.Forms.Label();
            this.box = new System.Windows.Forms.TextBox();
            this.hSeparator1 = new NotebookLib.HSeparator();
            this.SuspendLayout();
            // 
            // languageLabel
            // 
            this.languageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.languageLabel.Location = new System.Drawing.Point(3, 6);
            this.languageLabel.Name = "languageLabel";
            this.languageLabel.Size = new System.Drawing.Size(86, 58);
            this.languageLabel.TabIndex = 0;
            this.languageLabel.Text = "Language Name";
            this.languageLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // box
            // 
            this.box.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.box.Location = new System.Drawing.Point(95, 6);
            this.box.Multiline = true;
            this.box.Name = "box";
            this.box.Size = new System.Drawing.Size(165, 58);
            this.box.TabIndex = 1;
            this.box.TextChanged += new System.EventHandler(this.box_TextChanged);
            // 
            // hSeparator1
            // 
            this.hSeparator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hSeparator1.Location = new System.Drawing.Point(3, 0);
            this.hSeparator1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.hSeparator1.MaximumSize = new System.Drawing.Size(10000, 4);
            this.hSeparator1.MinimumSize = new System.Drawing.Size(0, 4);
            this.hSeparator1.Name = "hSeparator1";
            this.hSeparator1.Size = new System.Drawing.Size(257, 4);
            this.hSeparator1.TabIndex = 2;
            // 
            // StringEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hSeparator1);
            this.Controls.Add(this.box);
            this.Controls.Add(this.languageLabel);
            this.Name = "StringEntry";
            this.Size = new System.Drawing.Size(263, 67);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label languageLabel;
        private System.Windows.Forms.TextBox box;
        private NotebookLib.HSeparator hSeparator1;
    }
}
