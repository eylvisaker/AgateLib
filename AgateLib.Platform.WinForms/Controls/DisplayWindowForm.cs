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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AgateLib.Platform.WinForms.Controls
{
	/// <summary>
	/// Form which is used for a generic Display.
	/// </summary>
	/// <remarks>
	/// [Experimental - This class will be moved to into a different assembly
	/// in the future.]
	/// </remarks>
	public partial class DisplayWindowForm : Form
	{

		/// <summary>
		/// Constructs a DisplayWindowForm object.
		/// </summary>
		public DisplayWindowForm()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);            // No Need To Erase Form Background
			SetStyle(ControlStyles.Opaque, true);                          // No Need To Draw Form Background
			SetStyle(ControlStyles.ResizeRedraw, true);                    // Redraw On Resize
			SetStyle(ControlStyles.UserPaint, true);                       // We'll Handle Painting Ourselves

			InitializeComponent();

		}
		/// <summary>
		/// The control which is rendered into.
		/// </summary>
		public Control RenderTarget => agateRenderTarget1;

		private void DisplayWindowForm_Deactivate(object sender, EventArgs e)
		{
			AgateApp.IsActive = false;
		}

		private void DisplayWindowForm_Activated(object sender, EventArgs e)
		{
			AgateApp.IsActive = true;
		}

		private void DisplayWindowForm_Load(object sender, EventArgs e)
		{

		}
	}
}