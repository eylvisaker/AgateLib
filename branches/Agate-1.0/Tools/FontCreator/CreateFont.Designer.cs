namespace FontCreator
{
    partial class CreateFont
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateFont));
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
			this.label10 = new System.Windows.Forms.Label();
			this.nudBottomMargin = new System.Windows.Forms.NumericUpDown();
			this.nudTopMargin = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			this.nudOpacity = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.chkBorder = new System.Windows.Forms.CheckBox();
			this.btnBorderColor = new System.Windows.Forms.Button();
			this.cboEdges = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.chkTextRenderer = new System.Windows.Forms.CheckBox();
			this.nudScale = new System.Windows.Forms.NumericUpDown();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.cboBg = new System.Windows.Forms.ComboBox();
			this.btnDisplayColor = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.chkMonospaceNumbers = new System.Windows.Forms.CheckBox();
			this.nudNumberWidthAdjust = new System.Windows.Forms.NumericUpDown();
			this.renderTarget = new AgateLib.WinForms.AgateRenderTarget();
			this.zoomRenderTarget = new AgateLib.WinForms.AgateRenderTarget();
			((System.ComponentModel.ISupportInitialize)(this.nudSize)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudBottomMargin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTopMargin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudOpacity)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudScale)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudNumberWidthAdjust)).BeginInit();
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
			this.label2.Location = new System.Drawing.Point(13, 25);
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
			this.cboFamily.Location = new System.Drawing.Point(79, 22);
			this.cboFamily.Name = "cboFamily";
			this.cboFamily.Size = new System.Drawing.Size(145, 21);
			this.cboFamily.TabIndex = 3;
			this.cboFamily.SelectedIndexChanged += new System.EventHandler(this.cboFamily_SelectedIndexChanged);
			// 
			// nudSize
			// 
			this.nudSize.DecimalPlaces = 1;
			this.nudSize.Location = new System.Drawing.Point(79, 49);
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
            140,
            0,
            0,
            65536});
			this.nudSize.ValueChanged += new System.EventHandler(this.nudSize_ValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(22, 51);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(51, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Font Size";
			// 
			// chkBold
			// 
			this.chkBold.AutoSize = true;
			this.chkBold.Location = new System.Drawing.Point(48, 75);
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
			this.chkItalic.Location = new System.Drawing.Point(190, 75);
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
			this.chkUnderline.Location = new System.Drawing.Point(113, 75);
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
			this.chkStrikeout.Location = new System.Drawing.Point(170, 51);
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
			this.txtSampleText.Size = new System.Drawing.Size(186, 89);
			this.txtSampleText.TabIndex = 10;
			this.txtSampleText.Text = resources.GetString("txtSampleText.Text");
			this.txtSampleText.TextChanged += new System.EventHandler(this.txtSampleText_TextChanged);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(3, 260);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.renderTarget);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.zoomRenderTarget);
			this.splitContainer1.Size = new System.Drawing.Size(564, 150);
			this.splitContainer1.SplitterDistance = 208;
			this.splitContainer1.SplitterWidth = 8;
			this.splitContainer1.TabIndex = 11;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.nudNumberWidthAdjust);
			this.groupBox1.Controls.Add(this.chkMonospaceNumbers);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.nudBottomMargin);
			this.groupBox1.Controls.Add(this.nudTopMargin);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.nudOpacity);
			this.groupBox1.Controls.Add(this.label6);
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
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(265, 251);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Font Creation Options";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(157, 190);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(40, 13);
			this.label10.TabIndex = 21;
			this.label10.Text = "Bottom";
			// 
			// nudBottomMargin
			// 
			this.nudBottomMargin.Location = new System.Drawing.Point(203, 188);
			this.nudBottomMargin.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.nudBottomMargin.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
			this.nudBottomMargin.Name = "nudBottomMargin";
			this.nudBottomMargin.Size = new System.Drawing.Size(53, 20);
			this.nudBottomMargin.TabIndex = 20;
			this.nudBottomMargin.ValueChanged += new System.EventHandler(this.nudBottomMargin_ValueChanged);
			// 
			// nudTopMargin
			// 
			this.nudTopMargin.Location = new System.Drawing.Point(82, 188);
			this.nudTopMargin.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.nudTopMargin.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
			this.nudTopMargin.Name = "nudTopMargin";
			this.nudTopMargin.Size = new System.Drawing.Size(53, 20);
			this.nudTopMargin.TabIndex = 19;
			this.nudTopMargin.ValueChanged += new System.EventHandler(this.nudTopMargin_ValueChanged);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(6, 190);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(69, 13);
			this.label9.TabIndex = 18;
			this.label9.Text = "Margins: Top";
			// 
			// nudOpacity
			// 
			this.nudOpacity.Location = new System.Drawing.Point(147, 162);
			this.nudOpacity.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nudOpacity.Name = "nudOpacity";
			this.nudOpacity.Size = new System.Drawing.Size(54, 20);
			this.nudOpacity.TabIndex = 17;
			this.nudOpacity.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
			this.nudOpacity.ValueChanged += new System.EventHandler(this.nudOpacity_ValueChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(144, 146);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 13);
			this.label6.TabIndex = 16;
			this.label6.Text = "Border Opacity:";
			// 
			// chkBorder
			// 
			this.chkBorder.AutoSize = true;
			this.chkBorder.Location = new System.Drawing.Point(9, 153);
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
			this.btnBorderColor.Location = new System.Drawing.Point(103, 149);
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
			this.cboEdges.Location = new System.Drawing.Point(79, 121);
			this.cboEdges.Name = "cboEdges";
			this.cboEdges.Size = new System.Drawing.Size(145, 21);
			this.cboEdges.TabIndex = 12;
			this.cboEdges.SelectedIndexChanged += new System.EventHandler(this.cboEdges_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 124);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(67, 13);
			this.label4.TabIndex = 11;
			this.label4.Text = "Glyph Edges";
			// 
			// chkTextRenderer
			// 
			this.chkTextRenderer.AutoSize = true;
			this.chkTextRenderer.Checked = true;
			this.chkTextRenderer.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTextRenderer.Location = new System.Drawing.Point(6, 216);
			this.chkTextRenderer.Name = "chkTextRenderer";
			this.chkTextRenderer.Size = new System.Drawing.Size(207, 17);
			this.chkTextRenderer.TabIndex = 10;
			this.chkTextRenderer.Text = "Use TextRenderer instead of Graphics";
			this.chkTextRenderer.UseVisualStyleBackColor = true;
			this.chkTextRenderer.CheckedChanged += new System.EventHandler(this.chkTextRenderer_CheckedChanged);
			// 
			// nudScale
			// 
			this.nudScale.DecimalPlaces = 1;
			this.nudScale.Location = new System.Drawing.Point(80, 143);
			this.nudScale.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.nudScale.Name = "nudScale";
			this.nudScale.Size = new System.Drawing.Size(120, 20);
			this.nudScale.TabIndex = 14;
			this.nudScale.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.nudScale.ValueChanged += new System.EventHandler(this.nudScale_ValueChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.cboBg);
			this.groupBox2.Controls.Add(this.btnDisplayColor);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.txtSampleText);
			this.groupBox2.Controls.Add(this.nudScale);
			this.groupBox2.Location = new System.Drawing.Point(274, 3);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(272, 186);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Display Options";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(8, 145);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(67, 13);
			this.label8.TabIndex = 15;
			this.label8.Text = "Scale Factor";
			// 
			// cboBg
			// 
			this.cboBg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboBg.FormattingEnabled = true;
			this.cboBg.Items.AddRange(new object[] {
            "Dark Background",
            "Light Background"});
			this.cboBg.Location = new System.Drawing.Point(147, 115);
			this.cboBg.Name = "cboBg";
			this.cboBg.Size = new System.Drawing.Size(119, 21);
			this.cboBg.TabIndex = 13;
			this.cboBg.SelectedIndexChanged += new System.EventHandler(this.cboBg_SelectedIndexChanged);
			// 
			// btnDisplayColor
			// 
			this.btnDisplayColor.BackColor = System.Drawing.Color.White;
			this.btnDisplayColor.Location = new System.Drawing.Point(80, 114);
			this.btnDisplayColor.Name = "btnDisplayColor";
			this.btnDisplayColor.Size = new System.Drawing.Size(28, 23);
			this.btnDisplayColor.TabIndex = 12;
			this.btnDisplayColor.UseVisualStyleBackColor = false;
			this.btnDisplayColor.Click += new System.EventHandler(this.btnDisplayColor_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(19, 118);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(55, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "Text Color";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(30, 173);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(46, 13);
			this.label7.TabIndex = 16;
			this.label7.Text = "Scale:";
			// 
			// chkMonospaceNumbers
			// 
			this.chkMonospaceNumbers.AutoSize = true;
			this.chkMonospaceNumbers.Checked = true;
			this.chkMonospaceNumbers.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMonospaceNumbers.Location = new System.Drawing.Point(48, 98);
			this.chkMonospaceNumbers.Name = "chkMonospaceNumbers";
			this.chkMonospaceNumbers.Size = new System.Drawing.Size(127, 17);
			this.chkMonospaceNumbers.TabIndex = 22;
			this.chkMonospaceNumbers.Text = "Monospace Numbers";
			this.chkMonospaceNumbers.UseVisualStyleBackColor = true;
			this.chkMonospaceNumbers.CheckedChanged += new System.EventHandler(this.chkMonospaceNumbers_CheckedChanged);
			// 
			// nudNumberWidthAdjust
			// 
			this.nudNumberWidthAdjust.DecimalPlaces = 1;
			this.nudNumberWidthAdjust.Location = new System.Drawing.Point(181, 97);
			this.nudNumberWidthAdjust.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.nudNumberWidthAdjust.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
			this.nudNumberWidthAdjust.Name = "nudNumberWidthAdjust";
			this.nudNumberWidthAdjust.Size = new System.Drawing.Size(59, 20);
			this.nudNumberWidthAdjust.TabIndex = 23;
			this.nudNumberWidthAdjust.ValueChanged += new System.EventHandler(this.nudNumberWidthAdjust_ValueChanged);
			// 
			// renderTarget
			// 
			this.renderTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.renderTarget.Dock = System.Windows.Forms.DockStyle.Fill;
			this.renderTarget.Location = new System.Drawing.Point(0, 0);
			this.renderTarget.Name = "renderTarget";
			this.renderTarget.Size = new System.Drawing.Size(208, 150);
			this.renderTarget.TabIndex = 0;
			this.renderTarget.Resize += new System.EventHandler(this.renderTarget_Resize);
			// 
			// zoomRenderTarget
			// 
			this.zoomRenderTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.zoomRenderTarget.Dock = System.Windows.Forms.DockStyle.Fill;
			this.zoomRenderTarget.Location = new System.Drawing.Point(0, 0);
			this.zoomRenderTarget.Name = "zoomRenderTarget";
			this.zoomRenderTarget.Size = new System.Drawing.Size(348, 150);
			this.zoomRenderTarget.TabIndex = 1;
			this.zoomRenderTarget.Resize += new System.EventHandler(this.renderTarget_Resize);
			// 
			// CreateFont
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.splitContainer1);
			this.Name = "CreateFont";
			this.Size = new System.Drawing.Size(570, 413);
			((System.ComponentModel.ISupportInitialize)(this.nudSize)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudBottomMargin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTopMargin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudOpacity)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudScale)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudNumberWidthAdjust)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private AgateLib.WinForms.AgateRenderTarget renderTarget;
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
        private AgateLib.WinForms.AgateRenderTarget zoomRenderTarget;
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
        private System.Windows.Forms.NumericUpDown nudOpacity;
        private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown nudScale;
		private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown nudTopMargin;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.NumericUpDown nudBottomMargin;
		private System.Windows.Forms.CheckBox chkMonospaceNumbers;
		private System.Windows.Forms.NumericUpDown nudNumberWidthAdjust;
    }
}

