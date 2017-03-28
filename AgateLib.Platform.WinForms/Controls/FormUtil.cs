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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using System.IO;
using System.Linq;
using Size = AgateLib.Mathematics.Geometry.Size;

namespace AgateLib.Platform.WinForms.Controls
{
	/// <summary>
	/// Utility class for various windows forms methods that are common to different drivers.
	/// </summary>
	public static class FormUtil
	{
		/// <summary>
		/// Creates a System.Windows.Forms.Form object for rendering to.
		/// </summary>
		/// <param name="frm">Returns the created form.</param>
		/// <param name="renderTarget">Returns the control which is rendered into.</param>
		/// <param name="position">Position of the window.</param>
		/// <param name="title">Title of the window.</param>
		/// <param name="clientWidth">Width of client area, in pixels.</param>
		/// <param name="clientHeight">Height of client area, in pixels.</param>
		/// <param name="startFullscreen">True if we should start with a full-screen window.</param>
		/// <param name="allowResize">True if we should allow the user to resize the window.</param>
		/// <param name="hasFrame">True if a frame and title bar should be present.</param>
		public static void InitializeWindowsForm(
			out Form frm,
			out Control renderTarget,
			ScreenInfo targetScreen,
			WindowPosition position,
			string title, Size clientSize,
			bool startFullscreen, bool allowResize, bool hasFrame)
		{
			DisplayWindowForm mainForm = new DisplayWindowForm();

			// set output values
			frm = mainForm;
			renderTarget = mainForm.RenderTarget;

			// set properties
			frm.Text = title;
			frm.ClientSize = clientSize.ToDrawing();
			frm.KeyPreview = true;
			frm.Icon = FormUtil.AgateLibIcon;

			if (hasFrame == false)
				frm.FormBorderStyle = FormBorderStyle.None;
			else if (allowResize == false)
			{
				frm.FormBorderStyle = FormBorderStyle.FixedSingle;
				frm.MaximizeBox = false;
			}

			var screen = Screen.AllScreens[(int)targetScreen.SystemIndex];

			Point centerPoint = new Point(
				screen.WorkingArea.X + (screen.WorkingArea.Width - frm.Width) / 2,
				screen.WorkingArea.Y + (screen.WorkingArea.Height - frm.Height) / 2);

			switch (position)
			{
				case WindowPosition.DefaultAgate:
				case WindowPosition.AboveCenter:
					frm.StartPosition = FormStartPosition.Manual;
					frm.Location = new System.Drawing.Point(centerPoint.X, centerPoint.Y / 2);

					break;

				case WindowPosition.CenterScreen:
					frm.StartPosition = FormStartPosition.CenterScreen;
					break;

				case WindowPosition.DefaultOS:
					frm.StartPosition = FormStartPosition.WindowsDefaultLocation;
					break;
			}
		}

		/// <summary>
		/// Gets the official icon for AgateLib.
		/// </summary>
		public static Icon AgateLibIcon
		{
			get
			{
				try
				{
					if (Type.GetType("Mono.Runtime") != null)
					{
						return Icons.AgateLib_mono;
					}
					else
						return Icons.AgateLib;
				}
				catch (System.Resources.MissingManifestResourceException e)
				{
					AgateLib.AgateApp.ErrorReporting.Report(ErrorLevel.Warning,
						"Caught a MissingManifestResourceException when looking for the AgateLib Icon.  " +
						"This indicates a problem with the way resources were embedded into AgateLib.Platform.WinForms.dll when it was built.",
						e);

					return null;
				}
			}
		}

		/// <summary>
		/// Gets a System.Windows.Forms.Cursor object which is completely transparent.
		/// </summary>
		internal static Cursor BlankCursor
		{
			get
			{
				System.IO.MemoryStream stream = new System.IO.MemoryStream();
				stream.Write(Icons.blankcursor, 0, Icons.blankcursor.Length);
				stream.Seek(0, System.IO.SeekOrigin.Begin);

				return new Cursor(stream);
			}
		}

		/// <summary>
		/// Converts a System.Windows.Forms.Keys value to a KeyCode value.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static KeyCode TransformWinFormsKey(System.Windows.Forms.Keys id)
		{
			KeyCode myvalue;

			// sometimes windows reports Shift and sometimes ShiftKey.. what gives?
			switch (id)
			{
				case System.Windows.Forms.Keys.Alt:
				case System.Windows.Forms.Keys.Menu:
					myvalue = KeyCode.Alt;
					break;

				case System.Windows.Forms.Keys.Control:
				case System.Windows.Forms.Keys.ControlKey:
					myvalue = KeyCode.Control;
					break;

				case System.Windows.Forms.Keys.Shift:
				case System.Windows.Forms.Keys.ShiftKey:
					myvalue = KeyCode.Shift;
					break;

				default:
					myvalue = (KeyCode)id;
					break;
			}
			return myvalue;
		}

		/// <summary>
		/// Saves a pixel buffer to an image file using a System.Drawing.Bitmap object.
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="filename"></param>
		/// <param name="format"></param>
		public static void SavePixelBuffer(PixelBuffer buffer, string filename, ImageFileFormat format)
		{
			Bitmap bmp = buffer.ToBitmap();
			var dirname = Path.GetDirectoryName(filename);

			if (string.IsNullOrWhiteSpace(dirname) == false)
			{
				Directory.CreateDirectory(dirname);

				if (Directory.Exists(dirname) == false)
					throw new DirectoryNotFoundException("The directory " + dirname + " does not exist.");
			}

			switch (format)
			{
				case ImageFileFormat.Bmp:
					bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Bmp);
					break;

				case ImageFileFormat.Jpg:
					bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
					break;

				case ImageFileFormat.Png:
					bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
					break;

			}

			bmp.Dispose();
		}

		public static KeyCode AgateKeyCode(this KeyEventArgs e)
		{
			return TransformWinFormsKey(e.KeyCode);
		}

		public static KeyModifiers AgateKeyModifiers(this KeyEventArgs e)
		{
			return new KeyModifiers
			{
				Alt = e.Alt,
				Control = e.Control,
				Shift = e.Shift,
			};
		}

		public static MouseButton AgateMouseButton(this MouseEventArgs e)
		{
			var result = InputLib.MouseButton.None;
			var buttons = e.Button;

			if ((buttons & System.Windows.Forms.MouseButtons.Left) != 0)
				result |= InputLib.MouseButton.Primary;
			if ((buttons & System.Windows.Forms.MouseButtons.Right) != 0)
				result |= InputLib.MouseButton.Secondary;
			if ((buttons & System.Windows.Forms.MouseButtons.Middle) != 0)
				result |= InputLib.MouseButton.Middle;
			if ((buttons & System.Windows.Forms.MouseButtons.XButton1) != 0)
				result |= InputLib.MouseButton.ExtraButton1;
			if ((buttons & System.Windows.Forms.MouseButtons.XButton2) != 0)
				result |= InputLib.MouseButton.ExtraButton2;

			return result;
		}
	}
}
