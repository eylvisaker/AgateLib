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

namespace AgateLib.Platform.WinForms.GuiDebug
{
	partial class frmGuiDebug
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
			System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("FacetScene");
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tvWidgets = new System.Windows.Forms.TreeView();
			this.tabs = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.pgWidget = new System.Windows.Forms.PropertyGrid();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.pgStyle = new System.Windows.Forms.PropertyGrid();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.pgAnimator = new System.Windows.Forms.PropertyGrid();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.tpEvents = new System.Windows.Forms.TabPage();
			this.txtEvent = new System.Windows.Forms.TextBox();
			this.chkSelect = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tabs.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tpEvents.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tvWidgets);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tabs);
			this.splitContainer1.Size = new System.Drawing.Size(260, 760);
			this.splitContainer1.SplitterDistance = 407;
			this.splitContainer1.TabIndex = 0;
			// 
			// tvWidgets
			// 
			this.tvWidgets.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvWidgets.HideSelection = false;
			this.tvWidgets.Location = new System.Drawing.Point(0, 0);
			this.tvWidgets.Name = "tvWidgets";
			treeNode2.Name = "Node0";
			treeNode2.Text = "FacetScene";
			this.tvWidgets.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
			this.tvWidgets.Size = new System.Drawing.Size(260, 407);
			this.tvWidgets.TabIndex = 0;
			this.tvWidgets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvWidgets_AfterSelect);
			// 
			// tabs
			// 
			this.tabs.Controls.Add(this.tpEvents);
			this.tabs.Controls.Add(this.tabPage1);
			this.tabs.Controls.Add(this.tabPage2);
			this.tabs.Controls.Add(this.tabPage3);
			this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabs.Location = new System.Drawing.Point(0, 0);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(260, 349);
			this.tabs.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.pgWidget);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(252, 323);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Widget";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// pgWidget
			// 
			this.pgWidget.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pgWidget.Location = new System.Drawing.Point(3, 3);
			this.pgWidget.Name = "pgWidget";
			this.pgWidget.Size = new System.Drawing.Size(246, 317);
			this.pgWidget.TabIndex = 1;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.pgStyle);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(252, 323);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Style";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// pgStyle
			// 
			this.pgStyle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pgStyle.Location = new System.Drawing.Point(3, 3);
			this.pgStyle.Name = "pgStyle";
			this.pgStyle.Size = new System.Drawing.Size(246, 317);
			this.pgStyle.TabIndex = 1;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.pgAnimator);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(252, 323);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Animator";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// pgAnimator
			// 
			this.pgAnimator.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pgAnimator.Location = new System.Drawing.Point(3, 3);
			this.pgAnimator.Name = "pgAnimator";
			this.pgAnimator.Size = new System.Drawing.Size(246, 317);
			this.pgAnimator.TabIndex = 0;
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// tpEvents
			// 
			this.tpEvents.Controls.Add(this.chkSelect);
			this.tpEvents.Controls.Add(this.txtEvent);
			this.tpEvents.Location = new System.Drawing.Point(4, 22);
			this.tpEvents.Name = "tpEvents";
			this.tpEvents.Size = new System.Drawing.Size(252, 323);
			this.tpEvents.TabIndex = 3;
			this.tpEvents.Text = "Events";
			this.tpEvents.UseVisualStyleBackColor = true;
			// 
			// txtEvent
			// 
			this.txtEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtEvent.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtEvent.Location = new System.Drawing.Point(0, 23);
			this.txtEvent.Multiline = true;
			this.txtEvent.Name = "txtEvent";
			this.txtEvent.ReadOnly = true;
			this.txtEvent.Size = new System.Drawing.Size(253, 300);
			this.txtEvent.TabIndex = 0;
			// 
			// chkSelect
			// 
			this.chkSelect.AutoSize = true;
			this.chkSelect.Checked = true;
			this.chkSelect.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSelect.Location = new System.Drawing.Point(3, 3);
			this.chkSelect.Name = "chkSelect";
			this.chkSelect.Size = new System.Drawing.Size(96, 17);
			this.chkSelect.TabIndex = 1;
			this.chkSelect.Text = "Select on click";
			this.chkSelect.UseVisualStyleBackColor = true;
			this.chkSelect.CheckedChanged += new System.EventHandler(this.chkSelect_CheckedChanged);
			// 
			// frmGuiDebug
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(260, 760);
			this.Controls.Add(this.splitContainer1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "frmGuiDebug";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "FacetScene Debugger";
			this.Load += new System.EventHandler(this.frmGuiDebug_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tabs.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tpEvents.ResumeLayout(false);
			this.tpEvents.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TreeView tvWidgets;
		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.PropertyGrid pgWidget;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.PropertyGrid pgStyle;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.PropertyGrid pgAnimator;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.TabPage tpEvents;
		private System.Windows.Forms.TextBox txtEvent;
		private System.Windows.Forms.CheckBox chkSelect;
	}
}