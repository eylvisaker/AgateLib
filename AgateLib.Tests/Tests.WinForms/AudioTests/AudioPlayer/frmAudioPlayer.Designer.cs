// The contents of this file are public domain.
// You may use them as you wish.
//
namespace AgateLib.Tests.AudioTests.AudioPlayer
{
	partial class frmAudioTester
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
			this.btnStopAll = new System.Windows.Forms.Button();
			this.btnSound = new System.Windows.Forms.Button();
			this.lstFiles = new System.Windows.Forms.ListBox();
			this.btnMusic = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.panValue = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.statusLabel = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.btnPlayLastSound = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.label3 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.panValue)).BeginInit();
			this.SuspendLayout();
			// 
			// btnStopAll
			// 
			this.btnStopAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStopAll.Location = new System.Drawing.Point(329, 285);
			this.btnStopAll.Name = "btnStopAll";
			this.btnStopAll.Size = new System.Drawing.Size(75, 23);
			this.btnStopAll.TabIndex = 7;
			this.btnStopAll.Text = "Stop All";
			this.btnStopAll.UseVisualStyleBackColor = true;
			this.btnStopAll.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnSound
			// 
			this.btnSound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSound.Location = new System.Drawing.Point(293, 227);
			this.btnSound.Name = "btnSound";
			this.btnSound.Size = new System.Drawing.Size(111, 23);
			this.btnSound.TabIndex = 6;
			this.btnSound.Text = "Play as Sound";
			this.btnSound.UseVisualStyleBackColor = true;
			this.btnSound.Click += new System.EventHandler(this.btnSound_Click);
			// 
			// lstFiles
			// 
			this.lstFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstFiles.FormattingEnabled = true;
			this.lstFiles.Location = new System.Drawing.Point(12, 64);
			this.lstFiles.Name = "lstFiles";
			this.lstFiles.Size = new System.Drawing.Size(236, 225);
			this.lstFiles.TabIndex = 5;
			// 
			// btnMusic
			// 
			this.btnMusic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMusic.Location = new System.Drawing.Point(293, 256);
			this.btnMusic.Name = "btnMusic";
			this.btnMusic.Size = new System.Drawing.Size(111, 23);
			this.btnMusic.TabIndex = 4;
			this.btnMusic.Text = "Play as Music";
			this.btnMusic.UseVisualStyleBackColor = true;
			this.btnMusic.Click += new System.EventHandler(this.btnMusic_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(281, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(42, 13);
			this.label1.TabIndex = 8;
			this.label1.Text = "Volume";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDown1.DecimalPlaces = 2;
			this.numericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.numericUpDown1.Location = new System.Drawing.Point(329, 62);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(63, 20);
			this.numericUpDown1.TabIndex = 9;
			this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
			// 
			// panValue
			// 
			this.panValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.panValue.DecimalPlaces = 2;
			this.panValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
			this.panValue.Location = new System.Drawing.Point(329, 88);
			this.panValue.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.panValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
			this.panValue.Name = "panValue";
			this.panValue.Size = new System.Drawing.Size(63, 20);
			this.panValue.TabIndex = 11;
			this.panValue.ValueChanged += new System.EventHandler(this.panValue_ValueChanged);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(281, 90);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(26, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "Pan";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(329, 23);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 12;
			this.button1.Text = "Browse...";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// statusLabel
			// 
			this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.statusLabel.Location = new System.Drawing.Point(12, 306);
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(275, 40);
			this.statusLabel.TabIndex = 13;
			this.statusLabel.Text = "label3";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(12, 25);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(295, 20);
			this.textBox1.TabIndex = 14;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// btnPlayLastSound
			// 
			this.btnPlayLastSound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPlayLastSound.Location = new System.Drawing.Point(274, 180);
			this.btnPlayLastSound.Name = "btnPlayLastSound";
			this.btnPlayLastSound.Size = new System.Drawing.Size(130, 23);
			this.btnPlayLastSound.TabIndex = 15;
			this.btnPlayLastSound.Text = "Play Last Sound";
			this.btnPlayLastSound.UseVisualStyleBackColor = true;
			this.btnPlayLastSound.Click += new System.EventHandler(this.btnPlayLastSound_Click);
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 20;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(91, 13);
			this.label3.TabIndex = 16;
			this.label3.Text = "Path to audio files";
			// 
			// frmAudioTester
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(416, 345);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnPlayLastSound);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.statusLabel);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.panValue);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnStopAll);
			this.Controls.Add(this.btnSound);
			this.Controls.Add(this.lstFiles);
			this.Controls.Add(this.btnMusic);
			this.Name = "frmAudioTester";
			this.Text = "Audio Tester";
			this.Load += new System.EventHandler(this.frmAudioTester_Load);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.panValue)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnStopAll;
		private System.Windows.Forms.Button btnSound;
		private System.Windows.Forms.ListBox lstFiles;
		private System.Windows.Forms.Button btnMusic;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.NumericUpDown panValue;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.FolderBrowserDialog folderBrowser;
		private System.Windows.Forms.Label statusLabel;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button btnPlayLastSound;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label3;
	}
}

