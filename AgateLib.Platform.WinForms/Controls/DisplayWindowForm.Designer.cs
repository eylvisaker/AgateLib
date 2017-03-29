//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

namespace AgateLib.Platform.WinForms.Controls
{
	/// <summary>
	/// A basic form used for rendering into.
	/// </summary>
	partial class DisplayWindowForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisplayWindowForm));
			this.agateRenderTarget1 = new AgateRenderTarget();
			this.SuspendLayout();
			// 
			// agateRenderTarget1
			// 
			this.agateRenderTarget1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.agateRenderTarget1.Location = new System.Drawing.Point(0, 0);
			this.agateRenderTarget1.Name = "agateRenderTarget1";
			this.agateRenderTarget1.Size = new System.Drawing.Size(172, 218);
			this.agateRenderTarget1.TabIndex = 0;
			// 
			// DisplayWindowForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(172, 218);
			this.Controls.Add(this.agateRenderTarget1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "DisplayWindowForm";
			this.Text = "DisplayWindowForm";
			this.Deactivate += new System.EventHandler(this.DisplayWindowForm_Deactivate);
			this.Load += new System.EventHandler(this.DisplayWindowForm_Load);
			this.Activated += new System.EventHandler(this.DisplayWindowForm_Activated);
			this.ResumeLayout(false);

		}

		#endregion

		private AgateRenderTarget agateRenderTarget1;




	}
}