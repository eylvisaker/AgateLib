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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateFont));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cboFamily = new System.Windows.Forms.ComboBox();
			this.chkBold = new System.Windows.Forms.CheckBox();
			this.chkItalic = new System.Windows.Forms.CheckBox();
			this.chkUnderline = new System.Windows.Forms.CheckBox();
			this.chkStrikeout = new System.Windows.Forms.CheckBox();
			this.txtSampleText = new System.Windows.Forms.TextBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.renderTarget = new AgateLib.Platform.WinForms.Controls.AgateRenderTarget();
			this.zoomRenderTarget = new AgateLib.Platform.WinForms.Controls.AgateRenderTarget();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnGenerateFont = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label11 = new System.Windows.Forms.Label();
			this.lstSizes = new System.Windows.Forms.ListBox();
			this.nudNumberWidthAdjust = new System.Windows.Forms.NumericUpDown();
			this.chkMonospaceNumbers = new System.Windows.Forms.CheckBox();
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
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.chkDisplayBold = new System.Windows.Forms.CheckBox();
			this.nudDisplaySize = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.cboBg = new System.Windows.Forms.ComboBox();
			this.btnDisplayColor = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.mnuSizes = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.txtAddSize = new System.Windows.Forms.ToolStripTextBox();
			this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudNumberWidthAdjust)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudBottomMargin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTopMargin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudOpacity)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudDisplaySize)).BeginInit();
			this.mnuSizes.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 34);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Sample Text";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(20, 38);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(91, 20);
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
			this.cboFamily.Location = new System.Drawing.Point(118, 34);
			this.cboFamily.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.cboFamily.Name = "cboFamily";
			this.cboFamily.Size = new System.Drawing.Size(340, 28);
			this.cboFamily.TabIndex = 3;
			this.cboFamily.SelectedIndexChanged += new System.EventHandler(this.cboFamily_SelectedIndexChanged);
			// 
			// chkBold
			// 
			this.chkBold.AutoSize = true;
			this.chkBold.Checked = true;
			this.chkBold.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkBold.Location = new System.Drawing.Point(16, 28);
			this.chkBold.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.chkBold.Name = "chkBold";
			this.chkBold.Size = new System.Drawing.Size(67, 24);
			this.chkBold.TabIndex = 6;
			this.chkBold.Text = "Bold";
			this.chkBold.UseVisualStyleBackColor = true;
			this.chkBold.CheckedChanged += new System.EventHandler(this.chkBold_CheckedChanged);
			// 
			// chkItalic
			// 
			this.chkItalic.AutoSize = true;
			this.chkItalic.Enabled = false;
			this.chkItalic.Location = new System.Drawing.Point(131, 62);
			this.chkItalic.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.chkItalic.Name = "chkItalic";
			this.chkItalic.Size = new System.Drawing.Size(68, 24);
			this.chkItalic.TabIndex = 7;
			this.chkItalic.Text = "Italic";
			this.chkItalic.UseVisualStyleBackColor = true;
			this.chkItalic.CheckedChanged += new System.EventHandler(this.chkItalic_CheckedChanged);
			// 
			// chkUnderline
			// 
			this.chkUnderline.AutoSize = true;
			this.chkUnderline.Enabled = false;
			this.chkUnderline.Location = new System.Drawing.Point(118, 26);
			this.chkUnderline.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.chkUnderline.Name = "chkUnderline";
			this.chkUnderline.Size = new System.Drawing.Size(103, 24);
			this.chkUnderline.TabIndex = 8;
			this.chkUnderline.Text = "Underline";
			this.chkUnderline.UseVisualStyleBackColor = true;
			this.chkUnderline.CheckedChanged += new System.EventHandler(this.chkUnderline_CheckedChanged);
			// 
			// chkStrikeout
			// 
			this.chkStrikeout.AutoSize = true;
			this.chkStrikeout.Enabled = false;
			this.chkStrikeout.Location = new System.Drawing.Point(131, 85);
			this.chkStrikeout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.chkStrikeout.Name = "chkStrikeout";
			this.chkStrikeout.Size = new System.Drawing.Size(99, 24);
			this.chkStrikeout.TabIndex = 9;
			this.chkStrikeout.Text = "Strikeout";
			this.chkStrikeout.UseVisualStyleBackColor = true;
			this.chkStrikeout.CheckedChanged += new System.EventHandler(this.chkStrikeout_CheckedChanged);
			// 
			// txtSampleText
			// 
			this.txtSampleText.Location = new System.Drawing.Point(120, 29);
			this.txtSampleText.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtSampleText.Multiline = true;
			this.txtSampleText.Name = "txtSampleText";
			this.txtSampleText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtSampleText.Size = new System.Drawing.Size(277, 135);
			this.txtSampleText.TabIndex = 10;
			this.txtSampleText.Text = resources.GetString("txtSampleText.Text");
			this.txtSampleText.TextChanged += new System.EventHandler(this.txtSampleText_TextChanged);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(4, 462);
			this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.renderTarget);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.zoomRenderTarget);
			this.splitContainer1.Size = new System.Drawing.Size(907, 273);
			this.splitContainer1.SplitterDistance = 509;
			this.splitContainer1.SplitterWidth = 12;
			this.splitContainer1.TabIndex = 11;
			// 
			// renderTarget
			// 
			this.renderTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.renderTarget.Dock = System.Windows.Forms.DockStyle.Fill;
			this.renderTarget.Location = new System.Drawing.Point(0, 0);
			this.renderTarget.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.renderTarget.Name = "renderTarget";
			this.renderTarget.Size = new System.Drawing.Size(509, 273);
			this.renderTarget.TabIndex = 0;
			this.renderTarget.MouseMove += new System.Windows.Forms.MouseEventHandler(this.renderTarget_MouseMove);
			this.renderTarget.Resize += new System.EventHandler(this.renderTarget_Resize);
			// 
			// zoomRenderTarget
			// 
			this.zoomRenderTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.zoomRenderTarget.Dock = System.Windows.Forms.DockStyle.Fill;
			this.zoomRenderTarget.Location = new System.Drawing.Point(0, 0);
			this.zoomRenderTarget.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.zoomRenderTarget.Name = "zoomRenderTarget";
			this.zoomRenderTarget.Size = new System.Drawing.Size(386, 273);
			this.zoomRenderTarget.TabIndex = 1;
			this.zoomRenderTarget.Resize += new System.EventHandler(this.renderTarget_Resize);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnGenerateFont);
			this.groupBox1.Controls.Add(this.groupBox3);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.lstSizes);
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
			this.groupBox1.Location = new System.Drawing.Point(4, 5);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.groupBox1.Size = new System.Drawing.Size(884, 264);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Font Creation Options";
			// 
			// btnGenerateFont
			// 
			this.btnGenerateFont.Location = new System.Drawing.Point(324, 210);
			this.btnGenerateFont.Name = "btnGenerateFont";
			this.btnGenerateFont.Size = new System.Drawing.Size(134, 38);
			this.btnGenerateFont.TabIndex = 10;
			this.btnGenerateFont.Text = "Generate Font";
			this.btnGenerateFont.UseVisualStyleBackColor = true;
			this.btnGenerateFont.Click += new System.EventHandler(this.btnGenerateFont_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.chkBold);
			this.groupBox3.Controls.Add(this.chkUnderline);
			this.groupBox3.Controls.Add(this.chkItalic);
			this.groupBox3.Controls.Add(this.chkStrikeout);
			this.groupBox3.Location = new System.Drawing.Point(206, 78);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(263, 126);
			this.groupBox3.TabIndex = 26;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Styles";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(20, 78);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(104, 20);
			this.label11.TabIndex = 25;
			this.label11.Text = "Create Sizes:";
			// 
			// lstSizes
			// 
			this.lstSizes.ColumnWidth = 40;
			this.lstSizes.ContextMenuStrip = this.mnuSizes;
			this.lstSizes.FormattingEnabled = true;
			this.lstSizes.ItemHeight = 20;
			this.lstSizes.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "11",
            "12",
            "14",
            "16",
            "18",
            "22",
            "26",
            "30",
            "36",
            "48"});
			this.lstSizes.Location = new System.Drawing.Point(24, 104);
			this.lstSizes.MultiColumn = true;
			this.lstSizes.Name = "lstSizes";
			this.lstSizes.Size = new System.Drawing.Size(142, 124);
			this.lstSizes.TabIndex = 24;
			this.lstSizes.SelectedIndexChanged += new System.EventHandler(this.lstSizes_SelectedIndexChanged);
			// 
			// nudNumberWidthAdjust
			// 
			this.nudNumberWidthAdjust.DecimalPlaces = 1;
			this.nudNumberWidthAdjust.Location = new System.Drawing.Point(713, 42);
			this.nudNumberWidthAdjust.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
			this.nudNumberWidthAdjust.Size = new System.Drawing.Size(88, 26);
			this.nudNumberWidthAdjust.TabIndex = 23;
			this.nudNumberWidthAdjust.ValueChanged += new System.EventHandler(this.nudNumberWidthAdjust_ValueChanged);
			// 
			// chkMonospaceNumbers
			// 
			this.chkMonospaceNumbers.AutoSize = true;
			this.chkMonospaceNumbers.Checked = true;
			this.chkMonospaceNumbers.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMonospaceNumbers.Location = new System.Drawing.Point(494, 44);
			this.chkMonospaceNumbers.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.chkMonospaceNumbers.Name = "chkMonospaceNumbers";
			this.chkMonospaceNumbers.Size = new System.Drawing.Size(186, 24);
			this.chkMonospaceNumbers.TabIndex = 22;
			this.chkMonospaceNumbers.Text = "Monospace Numbers";
			this.chkMonospaceNumbers.UseVisualStyleBackColor = true;
			this.chkMonospaceNumbers.CheckedChanged += new System.EventHandler(this.chkMonospaceNumbers_CheckedChanged);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(721, 184);
			this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(61, 20);
			this.label10.TabIndex = 21;
			this.label10.Text = "Bottom";
			// 
			// nudBottomMargin
			// 
			this.nudBottomMargin.Location = new System.Drawing.Point(789, 181);
			this.nudBottomMargin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
			this.nudBottomMargin.Size = new System.Drawing.Size(80, 26);
			this.nudBottomMargin.TabIndex = 20;
			this.nudBottomMargin.ValueChanged += new System.EventHandler(this.nudBottomMargin_ValueChanged);
			// 
			// nudTopMargin
			// 
			this.nudTopMargin.Location = new System.Drawing.Point(608, 181);
			this.nudTopMargin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
			this.nudTopMargin.Size = new System.Drawing.Size(80, 26);
			this.nudTopMargin.TabIndex = 19;
			this.nudTopMargin.ValueChanged += new System.EventHandler(this.nudTopMargin_ValueChanged);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(494, 184);
			this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(100, 20);
			this.label9.TabIndex = 18;
			this.label9.Text = "Margins: Top";
			// 
			// nudOpacity
			// 
			this.nudOpacity.Location = new System.Drawing.Point(705, 141);
			this.nudOpacity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.nudOpacity.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nudOpacity.Name = "nudOpacity";
			this.nudOpacity.Size = new System.Drawing.Size(81, 26);
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
			this.label6.Location = new System.Drawing.Point(701, 115);
			this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(118, 20);
			this.label6.TabIndex = 16;
			this.label6.Text = "Border Opacity:";
			// 
			// chkBorder
			// 
			this.chkBorder.AutoSize = true;
			this.chkBorder.Location = new System.Drawing.Point(499, 127);
			this.chkBorder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.chkBorder.Name = "chkBorder";
			this.chkBorder.Size = new System.Drawing.Size(135, 24);
			this.chkBorder.TabIndex = 15;
			this.chkBorder.Text = "Create Border";
			this.chkBorder.UseVisualStyleBackColor = true;
			this.chkBorder.CheckedChanged += new System.EventHandler(this.chkBorder_CheckedChanged);
			// 
			// btnBorderColor
			// 
			this.btnBorderColor.BackColor = System.Drawing.Color.Black;
			this.btnBorderColor.Location = new System.Drawing.Point(639, 121);
			this.btnBorderColor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnBorderColor.Name = "btnBorderColor";
			this.btnBorderColor.Size = new System.Drawing.Size(48, 35);
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
			this.cboEdges.Location = new System.Drawing.Point(603, 78);
			this.cboEdges.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.cboEdges.Name = "cboEdges";
			this.cboEdges.Size = new System.Drawing.Size(216, 28);
			this.cboEdges.TabIndex = 12;
			this.cboEdges.SelectedIndexChanged += new System.EventHandler(this.cboEdges_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(494, 83);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 20);
			this.label4.TabIndex = 11;
			this.label4.Text = "Glyph Edges";
			// 
			// chkTextRenderer
			// 
			this.chkTextRenderer.AutoSize = true;
			this.chkTextRenderer.Checked = true;
			this.chkTextRenderer.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTextRenderer.Location = new System.Drawing.Point(494, 224);
			this.chkTextRenderer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.chkTextRenderer.Name = "chkTextRenderer";
			this.chkTextRenderer.Size = new System.Drawing.Size(307, 24);
			this.chkTextRenderer.TabIndex = 10;
			this.chkTextRenderer.Text = "Use TextRenderer instead of Graphics";
			this.chkTextRenderer.UseVisualStyleBackColor = true;
			this.chkTextRenderer.CheckedChanged += new System.EventHandler(this.chkTextRenderer_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.chkDisplayBold);
			this.groupBox2.Controls.Add(this.nudDisplaySize);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.cboBg);
			this.groupBox2.Controls.Add(this.btnDisplayColor);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.txtSampleText);
			this.groupBox2.Location = new System.Drawing.Point(4, 279);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.groupBox2.Size = new System.Drawing.Size(884, 173);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Display Options";
			// 
			// chkDisplayBold
			// 
			this.chkDisplayBold.AutoSize = true;
			this.chkDisplayBold.Location = new System.Drawing.Point(446, 127);
			this.chkDisplayBold.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.chkDisplayBold.Name = "chkDisplayBold";
			this.chkDisplayBold.Size = new System.Drawing.Size(67, 24);
			this.chkDisplayBold.TabIndex = 10;
			this.chkDisplayBold.Text = "Bold";
			this.chkDisplayBold.UseVisualStyleBackColor = true;
			this.chkDisplayBold.CheckedChanged += new System.EventHandler(this.chkDisplayBold_CheckedChanged);
			// 
			// nudDisplaySize
			// 
			this.nudDisplaySize.Location = new System.Drawing.Point(525, 74);
			this.nudDisplaySize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.nudDisplaySize.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.nudDisplaySize.Name = "nudDisplaySize";
			this.nudDisplaySize.Size = new System.Drawing.Size(82, 26);
			this.nudDisplaySize.TabIndex = 17;
			this.nudDisplaySize.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
			this.nudDisplaySize.ValueChanged += new System.EventHandler(this.nudDisplaySize_ValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(433, 76);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 20);
			this.label3.TabIndex = 16;
			this.label3.Text = "Size";
			// 
			// cboBg
			// 
			this.cboBg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboBg.FormattingEnabled = true;
			this.cboBg.Items.AddRange(new object[] {
            "Dark Background",
            "Light Background"});
			this.cboBg.Location = new System.Drawing.Point(625, 31);
			this.cboBg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.cboBg.Name = "cboBg";
			this.cboBg.Size = new System.Drawing.Size(176, 28);
			this.cboBg.TabIndex = 13;
			this.cboBg.SelectedIndexChanged += new System.EventHandler(this.cboBg_SelectedIndexChanged);
			// 
			// btnDisplayColor
			// 
			this.btnDisplayColor.BackColor = System.Drawing.Color.White;
			this.btnDisplayColor.Location = new System.Drawing.Point(525, 29);
			this.btnDisplayColor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnDisplayColor.Name = "btnDisplayColor";
			this.btnDisplayColor.Size = new System.Drawing.Size(42, 35);
			this.btnDisplayColor.TabIndex = 12;
			this.btnDisplayColor.UseVisualStyleBackColor = false;
			this.btnDisplayColor.Click += new System.EventHandler(this.btnDisplayColor_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(433, 36);
			this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 20);
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
			// mnuSizes
			// 
			this.mnuSizes.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.mnuSizes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.removeToolStripMenuItem});
			this.mnuSizes.Name = "mnuSizes";
			this.mnuSizes.Size = new System.Drawing.Size(212, 97);
			// 
			// addToolStripMenuItem
			// 
			this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtAddSize});
			this.addToolStripMenuItem.Name = "addToolStripMenuItem";
			this.addToolStripMenuItem.Size = new System.Drawing.Size(211, 30);
			this.addToolStripMenuItem.Text = "Add";
			// 
			// txtAddSize
			// 
			this.txtAddSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtAddSize.Name = "txtAddSize";
			this.txtAddSize.Size = new System.Drawing.Size(100, 31);
			this.txtAddSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAddSize_KeyPress);
			// 
			// removeToolStripMenuItem
			// 
			this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
			this.removeToolStripMenuItem.Size = new System.Drawing.Size(211, 30);
			this.removeToolStripMenuItem.Text = "Remove";
			this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
			// 
			// CreateFont
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.splitContainer1);
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "CreateFont";
			this.Size = new System.Drawing.Size(916, 739);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudNumberWidthAdjust)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudBottomMargin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTopMargin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudOpacity)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudDisplaySize)).EndInit();
			this.mnuSizes.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private AgateLib.Platform.WinForms.Controls.AgateRenderTarget renderTarget;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboFamily;
        private System.Windows.Forms.CheckBox chkBold;
        private System.Windows.Forms.CheckBox chkItalic;
        private System.Windows.Forms.CheckBox chkUnderline;
        private System.Windows.Forms.CheckBox chkStrikeout;
        private System.Windows.Forms.TextBox txtSampleText;
        private AgateLib.Platform.WinForms.Controls.AgateRenderTarget zoomRenderTarget;
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
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown nudTopMargin;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.NumericUpDown nudBottomMargin;
		private System.Windows.Forms.CheckBox chkMonospaceNumbers;
		private System.Windows.Forms.NumericUpDown nudNumberWidthAdjust;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.ListBox lstSizes;
		private System.Windows.Forms.CheckBox chkDisplayBold;
		private System.Windows.Forms.NumericUpDown nudDisplaySize;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnGenerateFont;
		private System.Windows.Forms.ContextMenuStrip mnuSizes;
		private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
		private System.Windows.Forms.ToolStripTextBox txtAddSize;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
	}
}

