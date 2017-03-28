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
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AgateLib.Platform.WinForms.Controls
{
	/// <summary>
	/// A generic render target control for AgateLib to render to.
	/// </summary>
	public partial class AgateRenderTarget : Panel
	{
		/// <summary>
		/// Constructs an AgateRenderTarget.
		/// </summary>
		public AgateRenderTarget()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);            // No Need To Erase Form Background
			SetStyle(ControlStyles.Opaque, true);                          // No Need To Draw Form Background
			SetStyle(ControlStyles.ResizeRedraw, true);                    // Redraw On Resize
			SetStyle(ControlStyles.UserPaint, true);                       // We'll Handle Painting Ourselves

			InitializeComponent();
		}

		enum ClassStyles : uint
		{
			CS_VREDRAW = 0x0001,
			CS_HREDRAW = 0x0002,
			CS_DBLCLKS = 0x0008,
			CS_OWNDC = 0x0020,
			CS_CLASSDC = 0x0040,
			CS_PARENTDC = 0x0080,
			CS_NOCLOSE = 0x0200,
			CS_SAVEBITS = 0x0800,
			CS_BYTEALIGNCLIENT = 0x1000,
			CS_BYTEALIGNWINDOW = 0x2000,
			CS_GLOBALCLASS = 0x4000,
			CS_IME = 0x00010000,
			CS_DROPSHADOW = 0x00020000
		}
		/// <summary>
		/// 
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				// supposedly this should improve compatibility on Windows Vista
				// when the compositing manager is used and Agate is running through
				// OpenGL.
				CreateParams result = base.CreateParams;
				result.ClassStyle |= (int)ClassStyles.CS_OWNDC;

				return result;
			}
		}

		bool AnyDesignMode
		{
			get
			{
				Control p = this;

				do
				{
					if (p.Site != null && p.Site.DesignMode)
						return true;

					p = p.Parent;

				} while (p != null);

				return false;
			}
		}
		/// <summary>
		/// Overrides the base class OnPaint method, to clear the
		/// client area in the Visual Studio designer.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (AnyDesignMode)
				e.Graphics.Clear(BackColor);

			base.OnPaint(e);

		}
		/// <summary>
		/// Overrides the base class OnClick method, to give focus to the AgateRenderTarget when it is clicked.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			Focus();
		}
	}
}