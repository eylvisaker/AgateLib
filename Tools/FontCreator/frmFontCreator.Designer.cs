namespace FontCreator
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboFamily = new System.Windows.Forms.ComboBox();
            this.nudSize = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.chkBold = new System.Windows.Forms.CheckBox();
            this.chkItalic = new System.Windows.Forms.CheckBox();
            this.chkUnderline = new System.Windows.Forms.CheckBox();
            this.chkStrikeout = new System.Windows.Forms.CheckBox();
            this.txtSampleText = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkBorder = new System.Windows.Forms.CheckBox();
            this.btnBorderColor = new System.Windows.Forms.Button();
            this.cboEdges = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkTextRenderer = new System.Windows.Forms.CheckBox();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboBg = new System.Windows.Forms.ComboBox();
            this.btnDisplayColor = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.renderTarget = new ERY.AgateLib.WinForms.AgateRenderTarget();
            this.zoomRenderTarget = new ERY.AgateLib.WinForms.AgateRenderTarget();
            ((System.ComponentModel.ISupportInitialize)(this.nudSize)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sample Text";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Font Family";
            // 
            // cboFamily
            // 
            this.cboFamily.DropDownHeight = 210;
            this.cboFamily.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFamily.DropDownWidth = 300;
            this.cboFamily.FormattingEnabled = true;
            this.cboFamily.IntegralHeight = false;
            this.cboFamily.Location = new System.Drawing.Point(79, 27);
            this.cboFamily.Name = "cboFamily";
            this.cboFamily.Size = new System.Drawing.Size(145, 21);
            this.cboFamily.TabIndex = 3;
            this.cboFamily.SelectedIndexChanged += new System.EventHandler(this.cboFamily_SelectedIndexChanged);
            // 
            // nudSize
            // 
            this.nudSize.DecimalPlaces = 1;
            this.nudSize.Location = new System.Drawing.Point(79, 54);
            this.nudSize.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSize.Name = "nudSize";
            this.nudSize.Size = new System.Drawing.Size(59, 20);
            this.nudSize.TabIndex = 4;
            this.nudSize.Value = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            this.nudSize.ValueChanged += new System.EventHandler(this.nudSize_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Font Size";
            // 
            // chkBold
            // 
            this.chkBold.AutoSize = true;
            this.chkBold.Location = new System.Drawing.Point(48, 80);
            this.chkBold.Name = "chkBold";
            this.chkBold.Size = new System.Drawing.Size(47, 17);
            this.chkBold.TabIndex = 6;
            this.chkBold.Text = "Bold";
            this.chkBold.UseVisualStyleBackColor = true;
            this.chkBold.CheckedChanged += new System.EventHandler(this.chkBold_CheckedChanged);
            // 
            // chkItalic
            // 
            this.chkItalic.AutoSize = true;
            this.chkItalic.Location = new System.Drawing.Point(48, 103);
            this.chkItalic.Name = "chkItalic";
            this.chkItalic.Size = new System.Drawing.Size(48, 17);
            this.chkItalic.TabIndex = 7;
            this.chkItalic.Text = "Italic";
            this.chkItalic.UseVisualStyleBackColor = true;
            this.chkItalic.CheckedChanged += new System.EventHandler(this.chkItalic_CheckedChanged);
            // 
            // chkUnderline
            // 
            this.chkUnderline.AutoSize = true;
            this.chkUnderline.Location = new System.Drawing.Point(113, 80);
            this.chkUnderline.Name = "chkUnderline";
            this.chkUnderline.Size = new System.Drawing.Size(71, 17);
            this.chkUnderline.TabIndex = 8;
            this.chkUnderline.Text = "Underline";
            this.chkUnderline.UseVisualStyleBackColor = true;
            this.chkUnderline.CheckedChanged += new System.EventHandler(this.chkUnderline_CheckedChanged);
            // 
            // chkStrikeout
            // 
            this.chkStrikeout.AutoSize = true;
            this.chkStrikeout.Location = new System.Drawing.Point(113, 103);
            this.chkStrikeout.Name = "chkStrikeout";
            this.chkStrikeout.Size = new System.Drawing.Size(68, 17);
            this.chkStrikeout.TabIndex = 9;
            this.chkStrikeout.Text = "Strikeout";
            this.chkStrikeout.UseVisualStyleBackColor = true;
            this.chkStrikeout.CheckedChanged += new System.EventHandler(this.chkStrikeout_CheckedChanged);
            // 
            // txtSampleText
            // 
            this.txtSampleText.Location = new System.Drawing.Point(80, 19);
            this.txtSampleText.Multiline = true;
            this.txtSampleText.Name = "txtSampleText";
            this.txtSampleText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSampleText.Size = new System.Drawing.Size(216, 69);
            this.txtSampleText.TabIndex = 10;
            this.txtSampleText.Text = "abcdefghijklmnopqrstuvwxyz\r\nABCDEFGHIJKLMNOPQRSTUVWXYZ\r\n01234567890\r\n!@#$%^&*(),<" +
                ".>/?;:\'\"-_=+\\|";
            this.txtSampleText.TextChanged += new System.EventHandler(this.txtSampleText_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 234);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.renderTarget);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.zoomRenderTarget);
            this.splitContainer1.Size = new System.Drawing.Size(641, 350);
            this.splitContainer1.SplitterDistance = 239;
            this.splitContainer1.TabIndex = 11;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkBorder);
            this.groupBox1.Controls.Add(this.btnBorderColor);
            this.groupBox1.Controls.Add(this.cboEdges);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.chkTextRenderer);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cboFamily);
            this.groupBox1.Controls.Add(this.nudSize);
            this.groupBox1.Controls.Add(this.chkStrikeout);
            this.groupBox1.Controls.Add(this.chkItalic);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkUnderline);
            this.groupBox1.Controls.Add(this.chkBold);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 216);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Font Creation Options";
            // 
            // chkBorder
            // 
            this.chkBorder.AutoSize = true;
            this.chkBorder.Location = new System.Drawing.Point(9, 180);
            this.chkBorder.Name = "chkBorder";
            this.chkBorder.Size = new System.Drawing.Size(91, 17);
            this.chkBorder.TabIndex = 15;
            this.chkBorder.Text = "Create Border";
            this.chkBorder.UseVisualStyleBackColor = true;
            this.chkBorder.CheckedChanged += new System.EventHandler(this.chkBorder_CheckedChanged);
            // 
            // btnBorderColor
            // 
            this.btnBorderColor.BackColor = System.Drawing.Color.Black;
            this.btnBorderColor.Location = new System.Drawing.Point(106, 176);
            this.btnBorderColor.Name = "btnBorderColor";
            this.btnBorderColor.Size = new System.Drawing.Size(32, 23);
            this.btnBorderColor.TabIndex = 14;
            this.btnBorderColor.UseVisualStyleBackColor = false;
            this.btnBorderColor.Click += new System.EventHandler(this.btnBorderColor_Click);
            // 
            // cboEdges
            // 
            this.cboEdges.DropDownHeight = 210;
            this.cboEdges.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEdges.DropDownWidth = 300;
            this.cboEdges.FormattingEnabled = true;
            this.cboEdges.IntegralHeight = false;
            this.cboEdges.Location = new System.Drawing.Point(79, 149);
            this.cboEdges.Name = "cboEdges";
            this.cboEdges.Size = new System.Drawing.Size(145, 21);
            this.cboEdges.TabIndex = 12;
            this.cboEdges.SelectedIndexChanged += new System.EventHandler(this.cboEdges_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Glyph Edges";
            // 
            // chkTextRenderer
            // 
            this.chkTextRenderer.AutoSize = true;
            this.chkTextRenderer.Location = new System.Drawing.Point(9, 126);
            this.chkTextRenderer.Name = "chkTextRenderer";
            this.chkTextRenderer.Size = new System.Drawing.Size(207, 17);
            this.chkTextRenderer.TabIndex = 10;
            this.chkTextRenderer.Text = "Use TextRenderer instead of Graphics";
            this.chkTextRenderer.UseVisualStyleBackColor = true;
            this.chkTextRenderer.CheckedChanged += new System.EventHandler(this.chkTextRenderer_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboBg);
            this.groupBox2.Controls.Add(this.btnDisplayColor);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtSampleText);
            this.groupBox2.Location = new System.Drawing.Point(253, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(302, 170);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Display Options";
            // 
            // cboBg
            // 
            this.cboBg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBg.FormattingEnabled = true;
            this.cboBg.Items.AddRange(new object[] {
            "Dark Background",
            "Light Background"});
            this.cboBg.Location = new System.Drawing.Point(147, 95);
            this.cboBg.Name = "cboBg";
            this.cboBg.Size = new System.Drawing.Size(149, 21);
            this.cboBg.TabIndex = 13;
            this.cboBg.SelectedIndexChanged += new System.EventHandler(this.cboBg_SelectedIndexChanged);
            // 
            // btnDisplayColor
            // 
            this.btnDisplayColor.BackColor = System.Drawing.Color.White;
            this.btnDisplayColor.Location = new System.Drawing.Point(80, 94);
            this.btnDisplayColor.Name = "btnDisplayColor";
            this.btnDisplayColor.Size = new System.Drawing.Size(28, 23);
            this.btnDisplayColor.TabIndex = 12;
            this.btnDisplayColor.UseVisualStyleBackColor = false;
            this.btnDisplayColor.Click += new System.EventHandler(this.btnDisplayColor_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Text Color";
            // 
            // renderTarget
            // 
            this.renderTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.renderTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderTarget.Location = new System.Drawing.Point(0, 0);
            this.renderTarget.Name = "renderTarget";
            this.renderTarget.Size = new System.Drawing.Size(239, 350);
            this.renderTarget.TabIndex = 0;
            this.renderTarget.Resize += new System.EventHandler(this.renderTarget_Resize);
            // 
            // zoomRenderTarget
            // 
            this.zoomRenderTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.zoomRenderTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zoomRenderTarget.Location = new System.Drawing.Point(0, 0);
            this.zoomRenderTarget.Name = "zoomRenderTarget";
            this.zoomRenderTarget.Size = new System.Drawing.Size(398, 350);
            this.zoomRenderTarget.TabIndex = 1;
            this.zoomRenderTarget.Resize += new System.EventHandler(this.renderTarget_Resize);
            // 
            // frmFontCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 596);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmFontCreator";
            this.Text = "Bitmap Font Creator";
            ((System.ComponentModel.ISupportInitialize)(this.nudSize)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ERY.AgateLib.WinForms.AgateRenderTarget renderTarget;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboFamily;
        private System.Windows.Forms.NumericUpDown nudSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkBold;
        private System.Windows.Forms.CheckBox chkItalic;
        private System.Windows.Forms.CheckBox chkUnderline;
        private System.Windows.Forms.CheckBox chkStrikeout;
        private System.Windows.Forms.TextBox txtSampleText;
        private ERY.AgateLib.WinForms.AgateRenderTarget zoomRenderTarget;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkTextRenderer;
        private System.Windows.Forms.ComboBox cboEdges;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkBorder;
        private System.Windows.Forms.Button btnBorderColor;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboBg;
        private System.Windows.Forms.Button btnDisplayColor;
        private System.Windows.Forms.Label label5;
    }
}

