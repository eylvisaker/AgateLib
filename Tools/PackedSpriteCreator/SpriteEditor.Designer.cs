namespace PackedSpriteCreator
{
    partial class SpriteEditor
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lstSprites = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.propertiesPanel = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstFrames = new System.Windows.Forms.ListBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.nudTimePerFrame = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.agateRenderTarget1 = new AgateLib.Platform.WinForms.Controls.AgateRenderTarget();
            this.chkAnimating = new System.Windows.Forms.CheckBox();
            this.btnNewSprite = new System.Windows.Forms.Button();
            this.propertiesPanel.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimePerFrame)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lstSprites
            // 
            this.lstSprites.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSprites.DisplayMember = "Name";
            this.lstSprites.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstSprites.Enabled = false;
            this.lstSprites.FormattingEnabled = true;
            this.lstSprites.Location = new System.Drawing.Point(129, 3);
            this.lstSprites.Name = "lstSprites";
            this.lstSprites.Size = new System.Drawing.Size(245, 21);
            this.lstSprites.TabIndex = 0;
            this.lstSprites.SelectedIndexChanged += new System.EventHandler(this.lstSprites_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(84, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sprites";
            // 
            // propertiesPanel
            // 
            this.propertiesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertiesPanel.Controls.Add(this.splitContainer1);
            this.propertiesPanel.Location = new System.Drawing.Point(3, 30);
            this.propertiesPanel.Name = "propertiesPanel";
            this.propertiesPanel.Size = new System.Drawing.Size(562, 361);
            this.propertiesPanel.TabIndex = 2;
            this.propertiesPanel.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.nudTimePerFrame);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.txtName);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(562, 361);
            this.splitContainer1.SplitterDistance = 334;
            this.splitContainer1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lstFrames);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.btnMoveDown);
            this.groupBox1.Controls.Add(this.btnMoveUp);
            this.groupBox1.Location = new System.Drawing.Point(3, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(316, 197);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Frames";
            // 
            // lstFrames
            // 
            this.lstFrames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFrames.FormattingEnabled = true;
            this.lstFrames.Location = new System.Drawing.Point(6, 22);
            this.lstFrames.Name = "lstFrames";
            this.lstFrames.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstFrames.Size = new System.Drawing.Size(216, 160);
            this.lstFrames.TabIndex = 4;
            this.lstFrames.SelectedIndexChanged += new System.EventHandler(this.lstFrames_SelectedIndexChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(228, 109);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(82, 23);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(228, 22);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(82, 23);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Add...";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveDown.Location = new System.Drawing.Point(228, 80);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(82, 23);
            this.btnMoveDown.TabIndex = 8;
            this.btnMoveDown.Text = "Move Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveUp.Location = new System.Drawing.Point(228, 51);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(82, 23);
            this.btnMoveUp.TabIndex = 7;
            this.btnMoveUp.Text = "Move Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // nudTimePerFrame
            // 
            this.nudTimePerFrame.Location = new System.Drawing.Point(111, 29);
            this.nudTimePerFrame.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudTimePerFrame.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTimePerFrame.Name = "nudTimePerFrame";
            this.nudTimePerFrame.Size = new System.Drawing.Size(80, 20);
            this.nudTimePerFrame.TabIndex = 3;
            this.nudTimePerFrame.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTimePerFrame.ValueChanged += new System.EventHandler(this.nudTimePerFrame_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Time per frame (ms)";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(79, 3);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(135, 20);
            this.txtName.TabIndex = 1;
            this.txtName.Validated += new System.EventHandler(this.txtName_Validated);
            this.txtName.Validating += new System.ComponentModel.CancelEventHandler(this.txtName_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Name";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.agateRenderTarget1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.chkAnimating);
            this.splitContainer2.Size = new System.Drawing.Size(224, 361);
            this.splitContainer2.SplitterDistance = 230;
            this.splitContainer2.TabIndex = 11;
            // 
            // agateRenderTarget1
            // 
            this.agateRenderTarget1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.agateRenderTarget1.Location = new System.Drawing.Point(0, 0);
            this.agateRenderTarget1.Name = "agateRenderTarget1";
            this.agateRenderTarget1.Size = new System.Drawing.Size(224, 230);
            this.agateRenderTarget1.TabIndex = 0;
            // 
            // chkAnimating
            // 
            this.chkAnimating.AutoSize = true;
            this.chkAnimating.Checked = true;
            this.chkAnimating.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAnimating.Location = new System.Drawing.Point(3, 3);
            this.chkAnimating.Name = "chkAnimating";
            this.chkAnimating.Size = new System.Drawing.Size(72, 17);
            this.chkAnimating.TabIndex = 10;
            this.chkAnimating.Text = "Animating";
            this.chkAnimating.UseVisualStyleBackColor = true;
            this.chkAnimating.CheckedChanged += new System.EventHandler(this.chkAnimating_CheckedChanged);
            // 
            // btnNewSprite
            // 
            this.btnNewSprite.Location = new System.Drawing.Point(3, 3);
            this.btnNewSprite.Name = "btnNewSprite";
            this.btnNewSprite.Size = new System.Drawing.Size(68, 23);
            this.btnNewSprite.TabIndex = 10;
            this.btnNewSprite.Text = "New Sprite";
            this.btnNewSprite.UseVisualStyleBackColor = true;
            this.btnNewSprite.Click += new System.EventHandler(this.btnNewSprite_Click);
            // 
            // SpriteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnNewSprite);
            this.Controls.Add(this.propertiesPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstSprites);
            this.Name = "SpriteEditor";
            this.Size = new System.Drawing.Size(568, 394);
            this.Load += new System.EventHandler(this.SpriteEditor_Load);
            this.propertiesPanel.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudTimePerFrame)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox lstSprites;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel propertiesPanel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudTimePerFrame;
        private System.Windows.Forms.Label label3;
        private AgateLib.Platform.WinForms.Controls.AgateRenderTarget agateRenderTarget1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstFrames;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnNewSprite;
        private System.Windows.Forms.CheckBox chkAnimating;
        private System.Windows.Forms.SplitContainer splitContainer2;
    }
}
