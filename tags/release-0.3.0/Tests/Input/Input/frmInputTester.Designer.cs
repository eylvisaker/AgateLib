// The contents of this file are public domain.
// You may use them as you wish.
//
namespace InputTester
{
    partial class Form1
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
            this.lblKeyPress = new System.Windows.Forms.Label();
            this.lblMouseMove = new System.Windows.Forms.Label();
            this.lblMouseButton = new System.Windows.Forms.Label();
            this.lblKeyEvent = new System.Windows.Forms.Label();
            this.lblKeyString = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.lblJoystick = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.agateRenderTarget1 = new AgateLib.WinForms.AgateRenderTarget();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblKeyPress
            // 
            this.lblKeyPress.Location = new System.Drawing.Point(12, 9);
            this.lblKeyPress.Name = "lblKeyPress";
            this.lblKeyPress.Size = new System.Drawing.Size(180, 17);
            this.lblKeyPress.TabIndex = 0;
            this.lblKeyPress.Text = "Pressed Key";
            // 
            // lblMouseMove
            // 
            this.lblMouseMove.Location = new System.Drawing.Point(12, 64);
            this.lblMouseMove.Name = "lblMouseMove";
            this.lblMouseMove.Size = new System.Drawing.Size(180, 16);
            this.lblMouseMove.TabIndex = 1;
            this.lblMouseMove.Text = "label1";
            // 
            // lblMouseButton
            // 
            this.lblMouseButton.Location = new System.Drawing.Point(12, 91);
            this.lblMouseButton.Name = "lblMouseButton";
            this.lblMouseButton.Size = new System.Drawing.Size(191, 15);
            this.lblMouseButton.TabIndex = 2;
            this.lblMouseButton.Text = "label1";
            // 
            // lblKeyEvent
            // 
            this.lblKeyEvent.AutoSize = true;
            this.lblKeyEvent.Location = new System.Drawing.Point(246, 9);
            this.lblKeyEvent.Name = "lblKeyEvent";
            this.lblKeyEvent.Size = new System.Drawing.Size(56, 13);
            this.lblKeyEvent.TabIndex = 4;
            this.lblKeyEvent.Text = "Key Event";
            // 
            // lblKeyString
            // 
            this.lblKeyString.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeyString.Location = new System.Drawing.Point(12, 32);
            this.lblKeyString.Name = "lblKeyString";
            this.lblKeyString.Size = new System.Drawing.Size(213, 21);
            this.lblKeyString.TabIndex = 5;
            this.lblKeyString.Text = "Pressed Key String []";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(297, 109);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(79, 20);
            this.numericUpDown1.TabIndex = 6;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(246, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Joystick";
            // 
            // lblJoystick
            // 
            this.lblJoystick.AutoSize = true;
            this.lblJoystick.Location = new System.Drawing.Point(246, 142);
            this.lblJoystick.Name = "lblJoystick";
            this.lblJoystick.Size = new System.Drawing.Size(66, 13);
            this.lblJoystick.TabIndex = 8;
            this.lblJoystick.Text = "Joystick Info";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(249, 80);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Clear All Keys";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // agateRenderTarget1
            // 
            this.agateRenderTarget1.Location = new System.Drawing.Point(12, 111);
            this.agateRenderTarget1.Name = "agateRenderTarget1";
            this.agateRenderTarget1.Size = new System.Drawing.Size(200, 180);
            this.agateRenderTarget1.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 303);
            this.Controls.Add(this.agateRenderTarget1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblJoystick);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.lblKeyString);
            this.Controls.Add(this.lblKeyEvent);
            this.Controls.Add(this.lblMouseButton);
            this.Controls.Add(this.lblMouseMove);
            this.Controls.Add(this.lblKeyPress);
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblKeyPress;
        private System.Windows.Forms.Label lblMouseMove;
        private System.Windows.Forms.Label lblMouseButton;
        private System.Windows.Forms.Label lblKeyEvent;
        private System.Windows.Forms.Label lblKeyString;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblJoystick;
        private System.Windows.Forms.Button button1;
        private AgateLib.WinForms.AgateRenderTarget agateRenderTarget1;
    }
}

