namespace NotebookLib
{
    partial class HSeparator
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
            this.SuspendLayout();
            // 
            // HSeparator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.MaximumSize = new System.Drawing.Size(10000, 4);
            this.MinimumSize = new System.Drawing.Size(0, 4);
            this.Name = "HSeparator";
            this.Size = new System.Drawing.Size(355, 4);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.HSeparator_Paint);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
