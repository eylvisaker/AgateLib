//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
namespace AgateLib.WinForms
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
			this.agateRenderTarget1 = new AgateLib.WinForms.AgateRenderTarget();
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