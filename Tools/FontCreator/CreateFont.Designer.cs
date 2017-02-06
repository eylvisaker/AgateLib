namespace FontCreatorApp
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
			this.txtSampleText = new System.Windows.Forms.TextBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.renderTarget = new AgateLib.Platform.WinForms.Controls.AgateRenderTarget();
			this.zoomRenderTarget = new AgateLib.Platform.WinForms.Controls.AgateRenderTarget();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnGenerateFont = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label11 = new System.Windows.Forms.Label();
			this.lstSizes = new System.Windows.Forms.ListBox();
			this.mnuSizes = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.txtAddSize = new System.Windows.Forms.ToolStripTextBox();
			this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.mnuSizes.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudNumberWidthAdjust)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudBottomMargin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTopMargin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudOpacity)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudDisplaySize)).BeginInit();
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
			this.cboFamily.Size = new System.Drawing.Size(228, 21);
			this.cboFamily.TabIndex = 3;
			this.cboFamily.SelectedIndexChanged += new System.EventHandler(this.SynchronizeParameters);
			// 
			// chkBold
			// 
			this.chkBold.AutoSize = true;
			this.chkBold.Checked = true;
			this.chkBold.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkBold.Location = new System.Drawing.Point(11, 18);
			this.chkBold.Name = "chkBold";
			this.chkBold.Size = new System.Drawing.Size(47, 17);
			this.chkBold.TabIndex = 6;
			this.chkBold.Text = "Bold";
			this.chkBold.UseVisualStyleBackColor = true;
			this.chkBold.CheckedChanged += new System.EventHandler(this.SynchronizeParameters);
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
			this.splitContainer1.Location = new System.Drawing.Point(3, 300);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.renderTarget);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.zoomRenderTarget);
			this.splitContainer1.Size = new System.Drawing.Size(605, 177);
			this.splitContainer1.SplitterDistance = 339;
			this.splitContainer1.SplitterWidth = 8;
			this.splitContainer1.TabIndex = 11;
			// 
			// renderTarget
			// 
			this.renderTarget.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.renderTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.renderTarget.Dock = System.Windows.Forms.DockStyle.Fill;
			this.renderTarget.Location = new System.Drawing.Point(0, 0);
			this.renderTarget.Name = "renderTarget";
			this.renderTarget.Size = new System.Drawing.Size(339, 177);
			this.renderTarget.TabIndex = 0;
			this.renderTarget.MouseMove += new System.Windows.Forms.MouseEventHandler(this.renderTarget_MouseMove);
			this.renderTarget.Resize += new System.EventHandler(this.renderTarget_Resize);
			// 
			// zoomRenderTarget
			// 
			this.zoomRenderTarget.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.zoomRenderTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.zoomRenderTarget.Dock = System.Windows.Forms.DockStyle.Fill;
			this.zoomRenderTarget.Location = new System.Drawing.Point(0, 0);
			this.zoomRenderTarget.Name = "zoomRenderTarget";
			this.zoomRenderTarget.Size = new System.Drawing.Size(258, 177);
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
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(589, 172);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Font Creation Options";
			// 
			// btnGenerateFont
			// 
			this.btnGenerateFont.Location = new System.Drawing.Point(495, 142);
			this.btnGenerateFont.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.btnGenerateFont.Name = "btnGenerateFont";
			this.btnGenerateFont.Size = new System.Drawing.Size(89, 25);
			this.btnGenerateFont.TabIndex = 10;
			this.btnGenerateFont.Text = "Generate Font";
			this.btnGenerateFont.UseVisualStyleBackColor = true;
			this.btnGenerateFont.Click += new System.EventHandler(this.btnGenerateFont_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.chkBold);
			this.groupBox3.Location = new System.Drawing.Point(137, 51);
			this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.groupBox3.Size = new System.Drawing.Size(70, 51);
			this.groupBox3.TabIndex = 26;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Styles";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(13, 51);
			this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(69, 13);
			this.label11.TabIndex = 25;
			this.label11.Text = "Create Sizes:";
			// 
			// lstSizes
			// 
			this.lstSizes.ColumnWidth = 40;
			this.lstSizes.ContextMenuStrip = this.mnuSizes;
			this.lstSizes.FormattingEnabled = true;
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
			this.lstSizes.Location = new System.Drawing.Point(16, 68);
			this.lstSizes.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.lstSizes.MultiColumn = true;
			this.lstSizes.Name = "lstSizes";
			this.lstSizes.Size = new System.Drawing.Size(96, 82);
			this.lstSizes.TabIndex = 24;
			this.lstSizes.SelectedIndexChanged += new System.EventHandler(this.lstSizes_SelectedIndexChanged);
			// 
			// mnuSizes
			// 
			this.mnuSizes.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.mnuSizes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.removeToolStripMenuItem});
			this.mnuSizes.Name = "mnuSizes";
			this.mnuSizes.Size = new System.Drawing.Size(118, 48);
			// 
			// addToolStripMenuItem
			// 
			this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtAddSize});
			this.addToolStripMenuItem.Name = "addToolStripMenuItem";
			this.addToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.addToolStripMenuItem.Text = "Add";
			// 
			// txtAddSize
			// 
			this.txtAddSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtAddSize.Name = "txtAddSize";
			this.txtAddSize.Size = new System.Drawing.Size(100, 23);
			this.txtAddSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAddSize_KeyPress);
			// 
			// removeToolStripMenuItem
			// 
			this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
			this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.removeToolStripMenuItem.Text = "Remove";
			this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
			// 
			// nudNumberWidthAdjust
			// 
			this.nudNumberWidthAdjust.DecimalPlaces = 1;
			this.nudNumberWidthAdjust.Location = new System.Drawing.Point(475, 27);
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
			this.nudNumberWidthAdjust.ValueChanged += new System.EventHandler(this.SynchronizeParameters);
			// 
			// chkMonospaceNumbers
			// 
			this.chkMonospaceNumbers.AutoSize = true;
			this.chkMonospaceNumbers.Checked = true;
			this.chkMonospaceNumbers.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMonospaceNumbers.Location = new System.Drawing.Point(329, 29);
			this.chkMonospaceNumbers.Name = "chkMonospaceNumbers";
			this.chkMonospaceNumbers.Size = new System.Drawing.Size(127, 17);
			this.chkMonospaceNumbers.TabIndex = 22;
			this.chkMonospaceNumbers.Text = "Monospace Numbers";
			this.chkMonospaceNumbers.UseVisualStyleBackColor = true;
			this.chkMonospaceNumbers.CheckedChanged += new System.EventHandler(this.SynchronizeParameters);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(268, 137);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(40, 13);
			this.label10.TabIndex = 21;
			this.label10.Text = "Bottom";
			// 
			// nudBottomMargin
			// 
			this.nudBottomMargin.Location = new System.Drawing.Point(314, 135);
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
			this.nudBottomMargin.ValueChanged += new System.EventHandler(this.SynchronizeParameters);
			// 
			// nudTopMargin
			// 
			this.nudTopMargin.Location = new System.Drawing.Point(209, 135);
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
			this.nudTopMargin.ValueChanged += new System.EventHandler(this.SynchronizeParameters);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(134, 137);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(69, 13);
			this.label9.TabIndex = 18;
			this.label9.Text = "Margins: Top";
			// 
			// nudOpacity
			// 
			this.nudOpacity.Location = new System.Drawing.Point(470, 92);
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
			this.nudOpacity.ValueChanged += new System.EventHandler(this.SynchronizeParameters);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(467, 75);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 13);
			this.label6.TabIndex = 16;
			this.label6.Text = "Border Opacity:";
			// 
			// chkBorder
			// 
			this.chkBorder.AutoSize = true;
			this.chkBorder.Location = new System.Drawing.Point(333, 83);
			this.chkBorder.Name = "chkBorder";
			this.chkBorder.Size = new System.Drawing.Size(91, 17);
			this.chkBorder.TabIndex = 15;
			this.chkBorder.Text = "Create Border";
			this.chkBorder.UseVisualStyleBackColor = true;
			this.chkBorder.CheckedChanged += new System.EventHandler(this.SynchronizeParameters);
			// 
			// btnBorderColor
			// 
			this.btnBorderColor.BackColor = System.Drawing.Color.Black;
			this.btnBorderColor.Location = new System.Drawing.Point(426, 79);
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
			this.cboEdges.Location = new System.Drawing.Point(402, 51);
			this.cboEdges.Name = "cboEdges";
			this.cboEdges.Size = new System.Drawing.Size(145, 21);
			this.cboEdges.TabIndex = 12;
			this.cboEdges.SelectedIndexChanged += new System.EventHandler(this.SynchronizeParameters);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(329, 54);
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
			this.chkTextRenderer.Location = new System.Drawing.Point(132, 106);
			this.chkTextRenderer.Name = "chkTextRenderer";
			this.chkTextRenderer.Size = new System.Drawing.Size(207, 17);
			this.chkTextRenderer.TabIndex = 10;
			this.chkTextRenderer.Text = "Use TextRenderer instead of Graphics";
			this.chkTextRenderer.UseVisualStyleBackColor = true;
			this.chkTextRenderer.CheckedChanged += new System.EventHandler(this.SynchronizeParameters);
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
			this.groupBox2.Location = new System.Drawing.Point(3, 181);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(589, 112);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Display Options";
			// 
			// chkDisplayBold
			// 
			this.chkDisplayBold.AutoSize = true;
			this.chkDisplayBold.Location = new System.Drawing.Point(297, 83);
			this.chkDisplayBold.Name = "chkDisplayBold";
			this.chkDisplayBold.Size = new System.Drawing.Size(47, 17);
			this.chkDisplayBold.TabIndex = 10;
			this.chkDisplayBold.Text = "Bold";
			this.chkDisplayBold.UseVisualStyleBackColor = true;
			this.chkDisplayBold.CheckedChanged += new System.EventHandler(this.chkDisplayBold_CheckedChanged);
			// 
			// nudDisplaySize
			// 
			this.nudDisplaySize.Location = new System.Drawing.Point(350, 48);
			this.nudDisplaySize.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.nudDisplaySize.Name = "nudDisplaySize";
			this.nudDisplaySize.Size = new System.Drawing.Size(55, 20);
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
			this.label3.Location = new System.Drawing.Point(289, 49);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(27, 13);
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
			this.cboBg.Location = new System.Drawing.Point(417, 20);
			this.cboBg.Name = "cboBg";
			this.cboBg.Size = new System.Drawing.Size(119, 21);
			this.cboBg.TabIndex = 13;
			this.cboBg.SelectedIndexChanged += new System.EventHandler(this.cboBg_SelectedIndexChanged);
			// 
			// btnDisplayColor
			// 
			this.btnDisplayColor.BackColor = System.Drawing.Color.White;
			this.btnDisplayColor.Location = new System.Drawing.Point(350, 19);
			this.btnDisplayColor.Name = "btnDisplayColor";
			this.btnDisplayColor.Size = new System.Drawing.Size(28, 23);
			this.btnDisplayColor.TabIndex = 12;
			this.btnDisplayColor.UseVisualStyleBackColor = false;
			this.btnDisplayColor.Click += new System.EventHandler(this.btnDisplayColor_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(289, 23);
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
			// CreateFont
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.splitContainer1);
			this.Name = "CreateFont";
			this.Size = new System.Drawing.Size(611, 480);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.mnuSizes.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudNumberWidthAdjust)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudBottomMargin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTopMargin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudOpacity)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudDisplaySize)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private AgateLib.Platform.WinForms.Controls.AgateRenderTarget renderTarget;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboFamily;
        private System.Windows.Forms.CheckBox chkBold;
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

