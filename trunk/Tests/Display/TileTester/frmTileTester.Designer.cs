namespace Tests.TileTester
{
    partial class frmTileTester
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTileTester));
            this.agateRenderTarget1 = new AgateLib.WinForms.AgateRenderTarget();
            this.chkScrollX = new System.Windows.Forms.CheckBox();
            this.chkScrollY = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // agateRenderTarget1
            // 
            this.agateRenderTarget1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.agateRenderTarget1.Location = new System.Drawing.Point(12, 12);
            this.agateRenderTarget1.Name = "agateRenderTarget1";
            this.agateRenderTarget1.Size = new System.Drawing.Size(362, 413);
            this.agateRenderTarget1.TabIndex = 0;
            // 
            // chkScrollX
            // 
            this.chkScrollX.AutoSize = true;
            this.chkScrollX.Location = new System.Drawing.Point(380, 12);
            this.chkScrollX.Name = "chkScrollX";
            this.chkScrollX.Size = new System.Drawing.Size(62, 17);
            this.chkScrollX.TabIndex = 1;
            this.chkScrollX.Text = "Scroll X";
            this.chkScrollX.UseVisualStyleBackColor = true;
            // 
            // chkScrollY
            // 
            this.chkScrollY.AutoSize = true;
            this.chkScrollY.Location = new System.Drawing.Point(380, 35);
            this.chkScrollY.Name = "chkScrollY";
            this.chkScrollY.Size = new System.Drawing.Size(62, 17);
            this.chkScrollY.TabIndex = 2;
            this.chkScrollY.Text = "Scroll Y";
            this.chkScrollY.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 437);
            this.Controls.Add(this.chkScrollY);
            this.Controls.Add(this.chkScrollX);
            this.Controls.Add(this.agateRenderTarget1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Tile Tester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AgateLib.WinForms.AgateRenderTarget agateRenderTarget1;
        private System.Windows.Forms.CheckBox chkScrollX;
        private System.Windows.Forms.CheckBox chkScrollY;
    }
}

