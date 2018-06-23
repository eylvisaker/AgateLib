namespace ResourceEditor.DisplayWinds
{
    partial class DisplayWindowEditor
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
            this.listWindows = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listWindows
            // 
            this.listWindows.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listWindows.FormattingEnabled = true;
            this.listWindows.Location = new System.Drawing.Point(3, 3);
            this.listWindows.Name = "listWindows";
            this.listWindows.Size = new System.Drawing.Size(133, 381);
            this.listWindows.TabIndex = 0;
            // 
            // DisplayWindowEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listWindows);
            this.Name = "DisplayWindowEditor";
            this.Size = new System.Drawing.Size(364, 388);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listWindows;
    }
}
